using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Deltarando_Generator
{
    class Program
    {
        public static Random rng = new Random();
        public static List<World> worlds = new List<World>();
        public static List<ItemType> globalItems = new List<ItemType>();
        public static List<ItemType> globalLocations = new List<ItemType>();
        public static List<ItemType> globalJunkItems = new List<ItemType>();
        public static bool Randomize()
        {
            globalLocations = new List<ItemType>();
            foreach (var item in worlds)
            {
                globalLocations.AddRange(item.locations);
            }
            foreach (var item in worlds)
            {
                item.RandomizeStart();
            }
            int chosenWorld = 0;
            while (globalLocations.Exists(x => x.name == "Null") || globalItems.Exists(x => x.name.Contains("Kingly Key")) || globalJunkItems.Exists(x => x.name.Contains("Kingly Key")))
            {
                globalLocations = new List<ItemType>();
                for (int f = 0; f < worlds.Count; f++)
                {
                    List<ItemType> tempLocs = new List<ItemType>(worlds[f].locations);
                    for (int r = 0; r < worlds[f].starting.Count; r++)
                    {
                        tempLocs.Add(new ItemType(worlds[f].myId + worlds[f].starting[r].name));
                    }
                    for (int r = 0; r < worlds[f].trueStarting.Count; r++)
                    {
                        tempLocs.Add(new ItemType(worlds[f].myId + worlds[f].trueStarting[r].name));
                    }
                    foreach (var item in tempLocs)
                    {
                        if (item.name.Length > 3 && item.name != "Null")
                        {
                            if (!int.TryParse(item.name[0].ToString(), out int temp))
                            {
                                globalLocations.Add(new ItemType(worlds[f].myId + item.name));
                            }
                            else
                            {
                                globalLocations.Add(item);
                            }
                        }
                    }
                    foreach (var item in tempLocs)
                    {
                        if (item.name == "Null")
                        {
                            globalLocations.Add(item);
                        }
                    }
                }
                worlds[chosenWorld].items = globalItems;
                worlds[chosenWorld].itemsJunk = globalJunkItems;
                if (!worlds[chosenWorld].Randomize())
                {
                    return false;
                }
                else
                {
                    globalItems = worlds[chosenWorld].items;
                    globalJunkItems = worlds[chosenWorld].itemsJunk;
                }
                chosenWorld++;
                if (chosenWorld >= worlds.Count)
                {
                    chosenWorld = 0;
                }
            }
            return true;
        }
        public static void Setup()
        {
            worlds = new List<World>();
            if (!Directory.Exists(Directory.GetCurrentDirectory() + "/Players"))
            {
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + "/Players");
            }
            var allPlayers = Directory.GetFiles(Directory.GetCurrentDirectory() + "/Players/");
            foreach (var item in allPlayers)
            {
                var tempWorld = new World();
                tempWorld.myFile = item.Split('/')[item.Split('/').Length - 1];
                tempWorld.SetOptions();
                worlds.Add(tempWorld);
            }
            globalItems = new List<ItemType>();
            globalJunkItems = new List<ItemType>();
            for (int i = 0; i < worlds.Count; i++)
            {
                worlds[i].ShuffleStart();
                if (i.ToString().Length == 3)
                {
                    worlds[i].myId = i.ToString();
                    foreach (var item in worlds[i].items)
                    {
                        globalItems.Add(new ItemType(i.ToString() + item.name));
                    }
                    foreach (var item in worlds[i].itemsJunk)
                    {
                        globalJunkItems.Add(new ItemType(i.ToString() + item.name));
                    }
                }
                else if (i.ToString().Length == 2)
                {
                    worlds[i].myId = "0" + i.ToString();
                    foreach (var item in worlds[i].items)
                    {
                        globalItems.Add(new ItemType("0" + i.ToString() + item.name));
                    }
                    foreach (var item in worlds[i].itemsJunk)
                    {
                        globalJunkItems.Add(new ItemType("0" + i.ToString() + item.name));
                    }
                }
                else if (i.ToString().Length == 1)
                {
                    worlds[i].myId = "00" + i.ToString();
                    foreach (var item in worlds[i].items)
                    {
                        globalItems.Add(new ItemType("00" + i.ToString() + item.name));
                    }
                    foreach (var item in worlds[i].itemsJunk)
                    {
                        globalJunkItems.Add(new ItemType("00" + i.ToString() + item.name));
                    }
                }
                else if (i.ToString().Length == 0)
                {
                    worlds[i].myId = "000" + i.ToString();
                    foreach (var item in worlds[i].items)
                    {
                        globalItems.Add(new ItemType("000" + i.ToString() + item.name));
                    }
                    foreach (var item in worlds[i].itemsJunk)
                    {
                        globalJunkItems.Add(new ItemType("000" + i.ToString() + item.name));
                    }
                }
                worlds[i].items.Clear();
                worlds[i].itemsJunk.Clear();
                if (worlds[i].options[39] == 1)
                {
                    while (!worlds[i].ShuffleRooms())
                    {
                        worlds[i].rooms.Clear();
                        worlds[i].enemyRooms.Clear();
                        worlds[i].AddRooms();
                    }
                }
            }
            Console.WriteLine("Shuffling items...");
        }
        public static void Main(string[] args)
        {
            Setup();
            while (!Randomize())
            {
                Setup();
            }
            for (int y = 0; y < worlds.Count; y++)
            {
                if (!Directory.Exists(Directory.GetCurrentDirectory() + "/output"))
                {
                    Directory.CreateDirectory(Directory.GetCurrentDirectory() + "/output");
                }
                var seed = File.CreateText(Directory.GetCurrentDirectory() + "/output/deltarando_player_" + worlds[y].myName + ".seed");
                seed.WriteLine(y);
                for (int i = 0; i < worlds[y].options.Count; i++)
                {
                    seed.WriteLine(i);
                    seed.WriteLine(worlds[y].options[i]);
                }
                for (int i = 0; i < worlds[y].rooms.Count; i++)
                {
                    seed.WriteLine(worlds[y].rooms[i].name);
                    var tempConnects = new List<string>();
                    for (int e = 0; e < worlds[y].rooms[i].connections.Count; e++)
                    {
                        if (worlds[y].rooms[i].connections[e].Length > 0)
                        {
                            if (worlds[y].rooms[i].connections[e][worlds[y].rooms[i].connections[e].Length - 1].ToString() == "y")
                            {
                                worlds[y].rooms[i].connections[e] = "-1";
                            }
                        }
                    }
                    for (int e = 0; e < worlds[y].rooms[i].connections.Count; e++)
                    {
                        seed.WriteLine(worlds[y].rooms[i].connections[e]);
                    }
                    for (int e = worlds[y].rooms[i].connections.Count; e < 10; e++)
                    {
                        seed.WriteLine("-1");
                    }
                }
                for (int i = 0; i < 200; i++)
                {
                    for (int e = 0; e < 3; e++)
                    {
                        seed.WriteLine(worlds[y].enemyIds[rng.Next(worlds[y].enemyIds.Count)]);
                    }
                }
                for (int i = 0; i < worlds[y].trueStarting.Count; i++)
                {
                    if (worlds[y].trueStarting[i].name != "")
                    {
                        seed.WriteLine(-1);
                        if (worlds[y].trueStarting[i].name.StartsWith(worlds[y].myId))
                        {
                            seed.WriteLine(worlds[y].trueStarting[i].name.Remove(0,3));
                        }
                        else
                        {
                            seed.WriteLine(worlds[y].trueStarting[i].name);
                        }
                    }
                }
                for (int i = 0; i < worlds[y].starting.Count; i++)
                {
                    if (worlds[y].starting[i].name != "")
                    {
                        seed.WriteLine(-2);
                        if (worlds[y].starting[i].name.StartsWith(worlds[y].myId))
                        {
                            seed.WriteLine(worlds[y].starting[i].name.Remove(0, 3));
                        }
                        else
                        {
                            seed.WriteLine(worlds[y].starting[i].name);
                        }
                    }
                }
                for (int i = 0; i < 3; i++)
                {
                    if (worlds[y].equipment[i].name != "")
                    {
                        seed.WriteLine(-i - 10);
                        seed.WriteLine(worlds[y].equipment[i].name);
                    }
                    else
                    {
                        seed.WriteLine(-i - 10);
                        seed.WriteLine("Hands");
                    }
                }
                for (int i = 3; i < 6; i++)
                {
                    if (worlds[y].equipment[i].name != "")
                    {
                        seed.WriteLine(-i - 20 + 3);
                        seed.WriteLine(worlds[y].equipment[i].name);
                    }
                    else
                    {
                        seed.WriteLine(-i - 20 + 3);
                        seed.WriteLine(" ");
                    }
                }
                for (int i = 6; i < 9; i++)
                {
                    if (worlds[y].equipment[i].name != "")
                    {
                        seed.WriteLine(-i - 30 + 6);
                        seed.WriteLine(worlds[y].equipment[i].name);
                    }
                    else
                    {
                        seed.WriteLine(-i - 30 + 6);
                        seed.WriteLine(" ");
                    }
                }
                for (int i = 0; i < worlds[y].locations.Count; i++)
                {
                    if (worlds[y].locations[i].name != "")
                    {
                        seed.WriteLine(i);
                        if (worlds[y].locations[i].name.StartsWith(worlds[y].myId))
                        {
                            seed.WriteLine(worlds[y].locations[i].name.Remove(0, 3));
                        }
                        else
                        {
                            seed.WriteLine(worlds[y].locations[i].name);
                        }
                    }
                }
                seed.Close();
                var textLoc = File.OpenText(Directory.GetCurrentDirectory() + "/locations.txt");
                Dictionary<int, string> locNameDict = new Dictionary<int, string>();
                while (!textLoc.EndOfStream)
                {
                    var itm = textLoc.ReadLine();
                    if (itm.Length > 3)
                    {
                        int locId = int.Parse(itm.Split(' ')[0]);
                        string locName = itm.Split(' ')[1];
                        locNameDict.Add(locId, locName);
                    }
                }
                if (worlds[y].options[55] == 1)
                {
                    var spoiler = File.CreateText(Directory.GetCurrentDirectory() + "/output/spoiler_player_"+worlds[y].myName+".txt");
                    for (int i = 0; i < worlds[y].locsOrder.Count; i++)
                    {
                        if (worlds[y].locations[worlds[y].locsOrder[i]].name.StartsWith(worlds[y].myId))
                        {
                            spoiler.WriteLine(locNameDict[worlds[y].locsOrder[i]] + " => " + worlds[y].locations[worlds[y].locsOrder[i]].name.Remove(0,3) + "\n");
                        }
                        else
                        {
                            spoiler.WriteLine(locNameDict[worlds[y].locsOrder[i]] + " => "+ worlds[int.Parse(worlds[y].locations[worlds[y].locsOrder[i]].name.Remove(3, worlds[y].locations[worlds[y].locsOrder[i]].name.Length-3))].myName + "'s " + worlds[y].locations[worlds[y].locsOrder[i]].name.Remove(0, 3) + "\n");
                        }
                    }
                    spoiler.Close();
                }

            }
        }
    }
}

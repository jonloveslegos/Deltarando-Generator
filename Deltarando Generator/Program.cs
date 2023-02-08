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
        public static void Main(string[] args)
        {
            World world = new World();
            Logic logic = new Logic();
            var rng = new Random();
            logic.SetLogic(world);
            world.AddItems();
            world.AddRooms();
            /*World world2 = new World();
            logic.SetLogic(world2);
            world2.AddItems();
            world2.AddRooms();
            List<ItemType> globalItems = new List<ItemType>();
            List<ItemType> globalJunkItems = new List<ItemType>();
            foreach (var item in world.items)
            {
                globalItems.Add(new ItemType("001"+item.name));
            }
            foreach (var item in world.itemsJunk)
            {
                globalJunkItems.Add(new ItemType("001" + item.name));
            }
            foreach (var item in world2.items)
            {
                globalItems.Add(new ItemType("002" + item.name));
            }
            foreach (var item in world2.itemsJunk)
            {
                globalJunkItems.Add(new ItemType("002" + item.name));
            }
            globalItems = globalItems.OrderBy(x => rng.Next()).ToList();
            globalJunkItems = globalJunkItems.OrderBy(x => rng.Next()).ToList();
            if (world2.options[39] == 1)
            {
                while (!world2.ShuffleRooms())
                {
                    world2.rooms.Clear();
                    world2.AddRooms();
                }
            }*/
            if (world.options[39] == 1)
            {
                while (!world.ShuffleRooms())
                {
                    world.rooms.Clear();
                    world.AddRooms();
                }
            }
            while (!world.Randomize())
            {
                world = new World();
                logic = new Logic();
                logic.SetLogic(world);
                world.AddItems();
                world.AddRooms();
                /*
                world2 = new World();
                logic.SetLogic(world2);
                world2.AddItems();
                world2.AddRooms();
                */
                if (world.options[39] == 1)
                {
                    while (!world.ShuffleRooms())
                    {
                        world.rooms.Clear();
                        world.AddRooms();
                    }
                }
            }
            var seed = File.CreateText(Directory.GetCurrentDirectory() + "/deltarando.seed");
            for (int i = 0; i < world.options.Count; i++)
            {
                seed.WriteLine(i);
                seed.WriteLine(world.options[i]);
            }
            for (int i = 0; i < world.rooms.Count; i++)
            {
                seed.WriteLine(world.rooms[i].name);
                var tempConnects = new List<string>();
                for (int e = 0; e < world.rooms[i].connections.Count; e++)
                {
                    if (world.rooms[i].connections[e].Length > 0)
                    {
                        if (world.rooms[i].connections[e][world.rooms[i].connections[e].Length - 1].ToString() == "y")
                        {
                            world.rooms[i].connections[e] = "-1";
                        }
                    }
                }
                for (int e = 0; e < world.rooms[i].connections.Count; e++)
                {
                    seed.WriteLine(world.rooms[i].connections[e]);
                }
                for (int e = world.rooms[i].connections.Count; e < 10; e++)
                {
                    seed.WriteLine("-1");
                }
            }
            for (int i = 0; i < 200; i++)
            {
                for (int e = 0; e < 3; e++)
                {
                    seed.WriteLine(world.enemyIds[world.rng.Next(world.enemyIds.Count)]);
                }
            }
            for (int i = 0; i < world.trueStarting.Count; i++)
            {
                if (world.trueStarting[i].name != "")
                {
                    seed.WriteLine(-1);
                    seed.WriteLine(world.trueStarting[i].name);
                }
            }
            for (int i = 0; i < world.starting.Count; i++)
            {
                if (world.starting[i].name != "")
                {
                    seed.WriteLine(-2);
                    seed.WriteLine(world.starting[i].name);
                }
            }
            for (int i = 0; i < 3; i++)
            {
                if (world.equipment[i].name != "")
                {
                    seed.WriteLine(-i-10);
                    seed.WriteLine(world.equipment[i].name);
                }
                else
                {
                    seed.WriteLine(-i - 10);
                    seed.WriteLine("Hands");
                }
            }
            for (int i = 3; i < 6; i++)
            {
                if (world.equipment[i].name != "")
                {
                    seed.WriteLine(-i - 20+3);
                    seed.WriteLine(world.equipment[i].name);
                }
                else
                {
                    seed.WriteLine(-i - 20+3);
                    seed.WriteLine(" ");
                }
            }
            for (int i = 6; i < 9; i++)
            {
                if (world.equipment[i].name != "")
                {
                    seed.WriteLine(-i - 30+6);
                    seed.WriteLine(world.equipment[i].name);
                }
                else
                {
                    seed.WriteLine(-i - 30+6);
                    seed.WriteLine(" ");
                }
            }
            for (int i = 0; i < world.locations.Count; i++)
            {
                if (world.locations[i].name != "")
                {
                    seed.WriteLine(i);
                    seed.WriteLine(world.locations[i].name);
                }
            }
            seed.Close();
        }
    }
}

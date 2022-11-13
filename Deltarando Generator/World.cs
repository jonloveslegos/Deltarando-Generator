using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

public delegate bool Del(World world);
public struct ItemType
{
    public string name;
    public ItemType(ItemType itemType)
    {
        name = itemType.name;
    }
    public ItemType(string itemId)
    {
        name = itemId;
    }
}

public static class Rule
{
    public static bool PlacedItem(World world, ItemType itemId)
    {
        if (world.locations.Exists(x => x.name == itemId.name))
        {
            return true;
        }
        if (world.wantedLocs.Exists(x => x.name == itemId.name))
        {
            return true;
        }
        if (world.starting.Exists(x => x.name == itemId.name))
        {
            return true;
        }
        if (world.trueStarting.Exists(x => x.name == itemId.name))
        {
            return true;
        }
        return world.locations.Exists(x => x.name == itemId.name);
    }
    public static bool ReturnTrue(World world)
    {
        return true;
    }
}

public class World
{
    public List<ItemType> locations = new List<ItemType>();
    public List<ItemType> wantedLocs = new List<ItemType>();
    public List<ItemType> items = new List<ItemType>();
    public List<ItemType> itemsJunk = new List<ItemType>();
    public List<ItemType> starting = new List<ItemType>();
    public List<ItemType> trueStarting = new List<ItemType>();
    public List<Predicate<World>> rules = new List<Predicate<World>>();
    public List<bool> options = new List<bool>();
    public Random rng = new Random();
    public World()
    {
        for (var i = 0; i < 300; i++)
        {
            rules.Add((thisWorld) => SpecialLogic.ReturnTrue(thisWorld));
        }
        for (var i = 0; i <= 99; i++)
        {
            options.Add(false);
        }
        for (int i = 0; i <= 300; i++)
        {
            locations.Add(new ItemType(""));
        }
        for (int i = 110; i <= 140; i++)
        {
            locations[i] = new ItemType("Null");
        }
        for (int i = 20; i <= 103; i++)
        {
            locations[i] = new ItemType("Null");
        }
        for (int i = 5; i <= 18; i++)
        {
            locations[i] = new ItemType("Null");
        }
        for (int i = 0; i <= 3; i++)
        {
            locations[i] = new ItemType("Null");
        }
        for (int i = 0; i <= 300; i++)
        {
            wantedLocs.Add(new ItemType(""));
        }
        var file = File.OpenText(Directory.GetCurrentDirectory() + "/options.txt");
        while (!file.EndOfStream)
        {
            var itm = file.ReadLine();
            options[int.Parse(itm.Split('(', ')')[1])] = bool.Parse(itm.Split('=')[1]);
        }
        file.Close();
    }
    public void PlaceItem(int locationId, ItemType itemListId)
    {
        locations[locationId] = new ItemType(itemListId);
    }
    public void PlaceForcedItem(int locationId, ItemType itemListId)
    {
        wantedLocs[locationId] = new ItemType(itemListId);
    }

    public void Randomize()
    {
        var backupLocations = new List<ItemType>(locations);
        var backupItems = new List<ItemType>(items);
        var backupItemsJunk = new List<ItemType>(itemsJunk);
        var lastLocations = new List<List<ItemType>>();
        var lastItems = new List<ItemType>();
        var itemsRemoved = new List<ItemType>();
        int timeSinceBack = 0;
        int backCount = 0;
        while (locations.Exists(x => x.name == "Null"))
        {
            List<int> emptyLocs = new List<int>();
            for (int i = 0; i < locations.Count; i++)
            {
                if (locations[i].name == "Null")
                {
                    emptyLocs.Add(i);
                }
            }
            List<int> toChooseFrom = new List<int>();
            for (int i = 0; i < emptyLocs.Count; i++)
            {
                if (rules[emptyLocs[i]].Invoke(this))
                {
                    toChooseFrom.Add(emptyLocs[i]);
                }
            }
            foreach (var item in toChooseFrom)
            {
                if (wantedLocs[item].name != "")
                {
                    PlaceItem(item, new ItemType(wantedLocs[item]));
                }
            }
            toChooseFrom = new List<int>();
            for (int i = 0; i < emptyLocs.Count; i++)
            {
                if (rules[emptyLocs[i]].Invoke(this))
                {
                    toChooseFrom.Add(emptyLocs[i]);
                }
            }
            if (items.Count+itemsJunk.Count <= 0)
            {
                locations = new List<ItemType>(backupLocations);
                items = new List<ItemType>(backupItems);
                itemsJunk = new List<ItemType>(backupItemsJunk);
                lastLocations = new List<List<ItemType>>();
                lastItems = new List<ItemType>();
                itemsRemoved = new List<ItemType>();
                timeSinceBack = 0;
                backCount = 0;
            }
            else if (toChooseFrom.Count <= 0)
            {
                if (timeSinceBack < 3)
                {
                    for (int i = 0; i < backCount; i++)
                    {
                        if (lastLocations.Count > 1)
                        {
                            locations = new List<ItemType>(lastLocations[lastLocations.Count - 1]);
                            if (!(lastItems[lastItems.Count - 1].name == "Null"))
                            {
                                itemsRemoved.Add(new ItemType(lastItems[lastItems.Count - 1]));
                            }
                            lastLocations.RemoveAt(lastLocations.Count - 1);
                            lastItems.RemoveAt(lastItems.Count - 1);
                        }
                    }
                }
                locations = new List<ItemType>(lastLocations[lastLocations.Count - 1]);
                if (!(lastItems[lastItems.Count - 1].name == "Null"))
                {
                    itemsRemoved.Add(new ItemType(lastItems[lastItems.Count - 1]));
                }
                lastLocations.RemoveAt(lastLocations.Count - 1);
                lastItems.RemoveAt(lastItems.Count - 1);
                timeSinceBack = 0;
                backCount++;
            }
            else
            {
                int chosen = toChooseFrom[rng.Next(toChooseFrom.Count)];
                var chosenItem = -1;
                    lastLocations.Add(new List<ItemType>(locations));
                    if (items.Count <= 0)
                    {
                        chosenItem = rng.Next(itemsJunk.Count);
                        lastItems.Add(new ItemType(itemsJunk[chosenItem]));
                        PlaceItem(chosen, itemsJunk[chosenItem]);
                        itemsJunk.RemoveAt(chosenItem);
                    }
                    else
                    {
                        chosenItem = rng.Next(items.Count);
                        lastItems.Add(new ItemType(items[chosenItem]));
                        PlaceItem(chosen, items[chosenItem]);
                        items.RemoveAt(chosenItem);
                    }

                timeSinceBack++;
                if (timeSinceBack >= 3)
                {
                    foreach (var item in itemsRemoved)
                    {
                        items.Add(new ItemType(item));
                    }
                    itemsRemoved.Clear();
                    backCount = 0;
                }
                string locsLeft = "";
                for (int i = 0; i < emptyLocs.Count; i++)
                {
                    locsLeft += emptyLocs[i].ToString()+ ",";
                }
                string locsAvail = "";
                for (int i = 0; i < toChooseFrom.Count; i++)
                {
                    locsAvail += toChooseFrom[i].ToString() + ",";
                }
                Console.WriteLine("Placed item.\n" + locsAvail + " locations accessible.\n"+ locsLeft +" locations left.\n"+ (items.Count+itemsJunk.Count+ itemsRemoved.Count).ToString()+" items left.");
            }
        }
    }

    public void AddItems()
    {
        if (options[8])
        {
            items.Add(new ItemType("BrokenCake"));
            items.Add(new ItemType("Broken Key A"));
            items.Add(new ItemType("Door Key"));
            items.Add(new ItemType("Broken Key B"));
            items.Add(new ItemType("Broken Key C"));
            items.Add(new ItemType("Field Key"));
            items.Add(new ItemType("Board Key"));
            items.Add(new ItemType("Forest Key"));
            items.Add(new ItemType("Castle Key"));
            items.Add(new ItemType("Field Secret Key"));
            items.Add(new ItemType("Forest Secret Key"));
            if (options[25])
            {
                for (int i = 0; i < 10; i++)
                {
                    items.Add(new ItemType("Kingly Key Piece"));
                }
            }
            else
            {
                items.Add(new ItemType("Kingly Key"));
            }
        }
        else
        {
            PlaceForcedItem(34, new ItemType("BrokenCake"));
            PlaceForcedItem(40, new ItemType("Broken Key A"));
            PlaceForcedItem(33, new ItemType("Door Key"));
            PlaceForcedItem(15, new ItemType("Broken Key B"));
            PlaceForcedItem(13, new ItemType("Broken Key C"));
            PlaceForcedItem(36, new ItemType("Field Key"));
            PlaceForcedItem(37, new ItemType("Board Key"));
            PlaceForcedItem(38, new ItemType("Forest Key"));
            PlaceForcedItem(39, new ItemType("Castle Key"));
            PlaceForcedItem(98, new ItemType("Kingly Key"));
            PlaceForcedItem(102, new ItemType("Field Secret Key"));
            PlaceForcedItem(103, new ItemType("Forest Secret Key"));
        }
        if (options[7])
        {
            if (options[24])
            {
                for (int i = 0; i < 4; i++)
                {
                    itemsJunk.Add(new ItemType("CinnaPill"));
                    itemsJunk.Add(new ItemType("CD Bagel"));
                    itemsJunk.Add(new ItemType("DD-Burger"));
                    itemsJunk.Add(new ItemType("SpagettiCode"));
                }
                itemsJunk.Add(new ItemType("LightCandy"));
                itemsJunk.Add(new ItemType("TensionBit"));
                itemsJunk.Add(new ItemType("TensionGem"));
            }
            for (int i = 0; i < 4; i++)
            {
                itemsJunk.Add(new ItemType("Darkburger"));
                itemsJunk.Add(new ItemType("HeartsDonut"));
                itemsJunk.Add(new ItemType("ChocDiamond"));
                itemsJunk.Add(new ItemType("RouxlsRoux"));
            }
            itemsJunk.Add(new ItemType("Glowshard"));
            items.Add(new ItemType("Top Cake"));
            itemsJunk.Add(new ItemType("LancerCookie"));
            for (int i = 0; i < 3; i++)
            {
                itemsJunk.Add(new ItemType("Spincake"));
                itemsJunk.Add(new ItemType("ClubsSandwich"));
            }
            for (int i = 0; i < 8; i++)
            {
                itemsJunk.Add(new ItemType("Dark Candy"));
            }
            for (int i = 0; i < 2; i++)
            {
                itemsJunk.Add(new ItemType("ReviveMint"));
                items.Add(new ItemType("Manual"));
            }
        }
        else
        {
            PlaceForcedItem(0, new ItemType("Dark Candy"));
            PlaceForcedItem(1, new ItemType("Dark Candy"));
            PlaceForcedItem(2, new ItemType("Dark Candy"));
            PlaceForcedItem(3, new ItemType("Dark Candy"));
            PlaceForcedItem(5, new ItemType("Top Cake"));
            PlaceForcedItem(6, new ItemType("ChocDiamond"));
            PlaceForcedItem(7, new ItemType("HeartsDonut"));
            PlaceForcedItem(8, new ItemType("LancerCookie"));
            PlaceForcedItem(9, new ItemType("Spincake"));
            PlaceForcedItem(10, new ItemType("Glowshard"));
            PlaceForcedItem(11, new ItemType("Manual"));
            PlaceForcedItem(12, new ItemType("Manual"));
            PlaceForcedItem(14, new ItemType("ReviveMint"));
            PlaceForcedItem(21, new ItemType("ClubsSandwich"));
            PlaceForcedItem(22, new ItemType("ReviveMint"));
            PlaceForcedItem(25, new ItemType("Dark Candy"));
            PlaceForcedItem(26, new ItemType("Darkburger"));
            PlaceForcedItem(29, new ItemType("RouxlsRoux"));
        }
        if (options[4])
        {
            for (int i = 0; i < 19; i++)
            {
                itemsJunk.Add(new ItemType("krislvl"));
                itemsJunk.Add(new ItemType("susielvl"));
                itemsJunk.Add(new ItemType("ralseilvl"));
            }
        }
        else
        {
            for (int i = 0; i < 19; i++)
            {
                PlaceForcedItem(41+i,new ItemType("krislvl"));
                PlaceForcedItem(60+i,new ItemType("susielvl"));
                PlaceForcedItem(79+i,new ItemType("ralseilvl"));
            }
        }
        if (options[13])
        {
            items.Add(new ItemType("Susie"));
            items.Add(new ItemType("Ralsei"));
            items.Add(new ItemType("Control Susie"));
        }
        else
        {
            PlaceForcedItem(111, new ItemType("Ralsei"));
            PlaceForcedItem(110, new ItemType("Susie"));
            PlaceForcedItem(112, new ItemType("Control Susie"));
        }
        if (options[11])
        {
            items.Add(new ItemType("ATTACK"));
            items.Add(new ItemType("MAGIC"));
            items.Add(new ItemType("ITEM"));
            items.Add(new ItemType("SPARE"));
            items.Add(new ItemType("DEFEND"));
        }
        else
        {
            starting.Add(new ItemType("ATTACK"));
            starting.Add(new ItemType("MAGIC"));
            starting.Add(new ItemType("ITEM"));
            starting.Add(new ItemType("SPARE"));
            starting.Add(new ItemType("DEFEND"));
        }
        if (options[23])
        {
            items.Add(new ItemType("C Menu"));
        }
        else
        {
            starting.Add(new ItemType("C Menu"));
        }
        if (options[22])
        {
            items.Add(new ItemType("RUN"));
        }
        else
        {
            starting.Add(new ItemType("RUN"));
        }
        if (options[21])
        {
            items.Add(new ItemType("TP BAR"));
        }
        else
        {
            starting.Add(new ItemType("TP BAR"));
        }
        if (options[17])
        {
            items.Add(new ItemType("LEFT SOUL"));
            items.Add(new ItemType("RIGHT SOUL"));
            items.Add(new ItemType("UP SOUL"));
            items.Add(new ItemType("DOWN SOUL"));
        }
        else
        {
            starting.Add(new ItemType("LEFT SOUL"));
            starting.Add(new ItemType("RIGHT SOUL"));
            starting.Add(new ItemType("UP SOUL"));
            starting.Add(new ItemType("DOWN SOUL"));
        }
        if (options[18])
        {
            items.Add(new ItemType("SAVE HEAL"));
        }
        else
        {
            starting.Add(new ItemType("SAVE HEAL"));
        }
        if (options[19])
        {
            items.Add(new ItemType("SAVING"));
        }
        else
        {
            starting.Add(new ItemType("SAVING"));
        }
        for (int i = 123; i <= 125; i++)
        {
            PlaceForcedItem(i, new ItemType("Save"));
        }
        if (options[20])
        {
            for (int i = 0; i < 14; i++)
            {
                itemsJunk.Add(new ItemType("Save"));
            }
        }
        else
        {

            for (int i = 126; i <= 139; i++)
            {
                PlaceForcedItem(i, new ItemType("Save"));
            }
        }
        if (options[10])
        {
            items.Add(new ItemType("kACT"));
            itemsJunk.Add(new ItemType("rHeal Prayer"));
            itemsJunk.Add(new ItemType("rPacify"));
            itemsJunk.Add(new ItemType("sRude Buster"));
            if (options[24])
            {
                items.Add(new ItemType("sACT"));
                items.Add(new ItemType("rACT"));
                itemsJunk.Add(new ItemType("sLife Steal"));
                itemsJunk.Add(new ItemType("kRed Sword"));
                itemsJunk.Add(new ItemType("kFocus Blade"));
                itemsJunk.Add(new ItemType("kX-Slash"));
                itemsJunk.Add(new ItemType("rMulti-Heal"));
                itemsJunk.Add(new ItemType("rPoison"));
                itemsJunk.Add(new ItemType("sHorrid Buster"));
                itemsJunk.Add(new ItemType("sHealing Magic"));
                itemsJunk.Add(new ItemType("rLife Transfer"));
                itemsJunk.Add(new ItemType("sDefense Trade"));
                itemsJunk.Add(new ItemType("rDark Sleep"));
            }
        }
        else
        {
            starting.Add(new ItemType("kACT"));
            starting.Add(new ItemType("rHeal Prayer"));
            starting.Add(new ItemType("rPacify"));
            starting.Add(new ItemType("sRude Buster"));
        }
        if (options[5])
        {
            itemsJunk.Add(new ItemType("Spookysword"));
            itemsJunk.Add(new ItemType("Brave Ax"));
            itemsJunk.Add(new ItemType("Devilsknife"));
            itemsJunk.Add(new ItemType("Ragger"));
            itemsJunk.Add(new ItemType("DaintyScarf"));
            if (options[24])
            {
                itemsJunk.Add(new ItemType("FiberScarf"));
                itemsJunk.Add(new ItemType("MechaSaber"));
                itemsJunk.Add(new ItemType("AutoAxe"));
                itemsJunk.Add(new ItemType("Ragger2"));
                itemsJunk.Add(new ItemType("BounceBlade"));
                itemsJunk.Add(new ItemType("PuppetScarf"));
                itemsJunk.Add(new ItemType("UltiBlade"));
                itemsJunk.Add(new ItemType("Wand-Axe"));
                itemsJunk.Add(new ItemType("Brute Axe"));
                itemsJunk.Add(new ItemType("Chaos Saber"));
            }
        }
        else
        {
            PlaceForcedItem(16, new ItemType("Ragger"));
            PlaceForcedItem(20, new ItemType("Devilsknife"));
            PlaceForcedItem(28, new ItemType("Spookysword"));
            PlaceForcedItem(30, new ItemType("Brave Ax"));
            PlaceForcedItem(31, new ItemType("DaintyScarf"));
        }
        if (options[6])
        {
            if (options[24])
            {
                for (int i = 0; i < 4; i++)
                {
                    itemsJunk.Add(new ItemType("Silver Card"));
                    itemsJunk.Add(new ItemType("GlowWrist"));
                    itemsJunk.Add(new ItemType("B.ShotBowtie"));
                    itemsJunk.Add(new ItemType("FrayedBowtie"));
                }
                for (int i = 0; i < 3; i++)
                {
                    itemsJunk.Add(new ItemType("SpikeBadge"));
                    itemsJunk.Add(new ItemType("Fluffy Shield"));
                    itemsJunk.Add(new ItemType("Silver Watch"));
                }
                for (int i = 0; i < 2; i++)
                {
                    itemsJunk.Add(new ItemType("TwinRibbon"));
                    itemsJunk.Add(new ItemType("TensionBow"));
                }
                itemsJunk.Add(new ItemType("Royal Pin"));
                itemsJunk.Add(new ItemType("ChainMail"));
                itemsJunk.Add(new ItemType("Dealmaker"));
                itemsJunk.Add(new ItemType("SpikeBand"));
                itemsJunk.Add(new ItemType("Mannequin"));
                itemsJunk.Add(new ItemType("Pink Ribbon"));
            }
            for (int i = 0; i < 4; i++)
            {
                itemsJunk.Add(new ItemType("Amber Card"));
            }
            for (int i = 0; i < 2; i++)
            {
                itemsJunk.Add(new ItemType("Dice Brace"));
            }
            itemsJunk.Add(new ItemType("White Ribbon"));
            itemsJunk.Add(new ItemType("IronShackle"));
            itemsJunk.Add(new ItemType("Jevilstail"));
        }
        else
        {
            PlaceForcedItem(27, new ItemType("Amber Card"));
            PlaceForcedItem(32, new ItemType("Amber Card"));
            PlaceForcedItem(17, new ItemType("Dice Brace"));
            PlaceForcedItem(23, new ItemType("White Ribbon"));
            PlaceForcedItem(24, new ItemType("Jevilstail"));
            PlaceForcedItem(35, new ItemType("IronShackle"));
        }
        if (options[3])
        {
            items.Add(new ItemType("FieldShortcutDoor"));
            items.Add(new ItemType("BoardShortcutDoor"));
            items.Add(new ItemType("ForestShortcutDoor"));
            items.Add(new ItemType("CastleShortcutDoor"));
        }
        else
        {
            PlaceForcedItem(140, new ItemType("FieldShortcutDoor"));
            PlaceForcedItem(99, new ItemType("BoardShortcutDoor"));
            PlaceForcedItem(100, new ItemType("ForestShortcutDoor"));
            PlaceForcedItem(101, new ItemType("CastleShortcutDoor"));
        }
        StreamReader file;
        foreach (var item in Directory.GetFiles(Directory.GetCurrentDirectory() + "/custom/armors/"))
        {
            file = File.OpenText(item);
            var thisName = "";
            while (!file.EndOfStream)
            {
                var itm = file.ReadLine();
                if (itm.Contains("name="))
                {
                    thisName = itm.Replace("name=", "");
                }
                if (itm.Contains("amount="))
                {
                    for (int i = 0; i < int.Parse(itm.Replace("amount=", "")); i++)
                    {
                        itemsJunk.Add(new ItemType(thisName));
                    }
                }
            }
            file.Close();
        }
        foreach (var item in Directory.GetFiles(Directory.GetCurrentDirectory() + "/custom/weapons/"))
        {
            file = File.OpenText(item);
            var thisName = "";
            while (!file.EndOfStream)
            {
                var itm = file.ReadLine();
                if (itm.Contains("name="))
                {
                    thisName = itm.Replace("name=", "");
                }
                if (itm.Contains("amount="))
                {
                    for (int i = 0; i < int.Parse(itm.Replace("amount=", "")); i++)
                    {
                        itemsJunk.Add(new ItemType(thisName));
                    }
                }
            }
            file.Close();
        }
        foreach (var item in Directory.GetFiles(Directory.GetCurrentDirectory() + "/custom/spells/"))
        {
            file = File.OpenText(item);
            var thisName = "";
            while (!file.EndOfStream)
            {
                var itm = file.ReadLine();
                if (itm.Contains("name="))
                {
                    thisName = itm.Replace("name=", "");
                }
                if (itm.Contains("amount="))
                {
                    for (int i = 0; i < int.Parse(itm.Replace("amount=", "")); i++)
                    {
                        itemsJunk.Add(new ItemType(thisName));
                    }
                }
            }
            file.Close();
        }
        file = File.OpenText(Directory.GetCurrentDirectory() + "/custom/remove/remove.txt");
        file.ReadLine();
        while (!file.EndOfStream)
        {
            var itm = file.ReadLine();
            if (items.Exists(x => x.name == itm))
            {
                items.Remove(new ItemType(itm));
            }
            else if (itemsJunk.Exists(x => x.name == itm))
            {
                itemsJunk.Remove(new ItemType(itm));
            }
        }
        file.Close();
        file = File.OpenText(Directory.GetCurrentDirectory() + "/custom/remove/remove.txt");
        file.ReadLine();
        while (!file.EndOfStream)
        {
            var itm = file.ReadLine();
            if (items.Exists(x => x.name == itm))
            {
                items.Remove(new ItemType(itm));
            }
            else if (itemsJunk.Exists(x => x.name == itm))
            {
                itemsJunk.Remove(new ItemType(itm));
            }
        }
        file.Close();
        file = File.OpenText(Directory.GetCurrentDirectory() + "/custom/startingItems.txt");
        file.ReadLine();
        file.ReadLine();
        while (!file.EndOfStream)
        {
            var itm = file.ReadLine();
            if (items.Exists(x => x.name == itm))
            {
                trueStarting.Add(new ItemType(itm));
                items.Remove(new ItemType(itm));
            }
            else if (itemsJunk.Exists(x => x.name == itm))
            {
                trueStarting.Add(new ItemType(itm));
                itemsJunk.Remove(new ItemType(itm));
            }
        }
        file.Close();
        while (locations.FindAll(x => x.name == "Null").Count  > items.Count + itemsJunk.Count-(wantedLocs.Count-wantedLocs.FindAll(x => x.name == "").Count))
        {
            itemsJunk.Add(new ItemType("gold"));
        }
    }
}

public static class SpecialLogic
{
    public static bool AreaAccess(World world, int areaId)
    {
        if (areaId == 1)
        {
            return SpecialLogic.AreaAccess(world, areaId - 1) && SpecialLogic.CanCompleteTutorial(world) && Rule.PlacedItem(world, new ItemType("Field Key"));
        }
        else if (areaId == 2)
        {
            return (SpecialLogic.AreaAccess(world, areaId - 1) && SpecialLogic.ActAvailability(world) && Rule.PlacedItem(world, new ItemType("SPARE")) && Rule.PlacedItem(world, new ItemType("RUN")) && Rule.PlacedItem(world, new ItemType("Board Key"))) || (SpecialLogic.AreaAccess(world, 1) && SpecialLogic.ActAvailability(world) && Rule.PlacedItem(world, new ItemType("SPARE")) && Rule.PlacedItem(world, new ItemType("ForestShortcutDoor")) && Rule.PlacedItem(world, new ItemType("FieldShortcutDoor")));
        }
        else if (areaId == 3)
        {
            return (SpecialLogic.AreaAccess(world, areaId - 1) && (Rule.PlacedItem(world, new ItemType("Forest Key"))) || (SpecialLogic.AreaAccess(world, 1) && Rule.PlacedItem(world, new ItemType("ForestShortcutDoor")) && Rule.PlacedItem(world, new ItemType("FieldShortcutDoor"))));
        }
        else if (areaId == 4)
        {
            return (SpecialLogic.AreaAccess(world, areaId-1) && Rule.PlacedItem(world, new ItemType("Castle Key"))) || (SpecialLogic.AreaAccess(world, 1) && Rule.PlacedItem(world, new ItemType("CastleShortcutDoor")) && Rule.PlacedItem(world, new ItemType("FieldShortcutDoor")));
        }
        return true;
    }
    public static bool ReturnTrue(World world)
    {
        return true;
    }
    public static bool ActAvailability(World world)
    {
        return Rule.PlacedItem(world, new ItemType("MAGIC")) && ((Rule.PlacedItem(world, new ItemType("Ralsei")) && Rule.PlacedItem(world, new ItemType("rACT"))) || (Rule.PlacedItem(world, new ItemType("Susie")) && Rule.PlacedItem(world, new ItemType("sACT"))) || Rule.PlacedItem(world, new ItemType("kACT")));
    }
    public static bool CanCompleteTutorial(World world)
    {
        return (Rule.PlacedItem(world, new ItemType("LEFT SOUL")) || Rule.PlacedItem(world, new ItemType("RIGHT SOUL"))) && (Rule.PlacedItem(world, new ItemType("DOWN SOUL")) || Rule.PlacedItem(world, new ItemType("UP SOUL"))) && ((SpecialLogic.ActAvailability(world) && Rule.PlacedItem(world, new ItemType("SPARE"))) || Rule.PlacedItem(world, new ItemType("ATTACK")));
    }
    public static bool UnlockedAreaCount(World world, int amountToUnlock)
    {
        int unlocked = 0;
        if (SpecialLogic.AreaAccess(world, 1))
        {
            unlocked++;
        }
        if (SpecialLogic.AreaAccess(world, 2))
        {
            unlocked++;
        }
        if (SpecialLogic.AreaAccess(world, 3))
        {
            unlocked++;
        }
        if (SpecialLogic.AreaAccess(world, 4))
        {
            unlocked++;
        }
        return (unlocked >= amountToUnlock);
    }
}

public class Logic
{
    public void SetLogic(World world)
    {
        world.rules.Insert(11, (thisWorld) => Rule.PlacedItem(thisWorld, new ItemType("Manual")) && Rule.PlacedItem(thisWorld, new ItemType("C Menu")));
        world.rules.Insert(9, (thisWorld) => Rule.PlacedItem(thisWorld, new ItemType("Top Cake")) && SpecialLogic.AreaAccess(thisWorld, 1));
        world.rules.Insert(113, (thisWorld) => SpecialLogic.AreaAccess(thisWorld, 1));
        world.rules.Insert(140, (thisWorld) => SpecialLogic.AreaAccess(thisWorld, 1));
        world.rules.Insert(0, (thisWorld) => Rule.PlacedItem(thisWorld, new ItemType("Field Secret Key")) && SpecialLogic.AreaAccess(thisWorld, 1));
        world.rules.Insert(1, (thisWorld) => Rule.PlacedItem(thisWorld, new ItemType("Field Secret Key")) && SpecialLogic.AreaAccess(thisWorld, 1));
        world.rules.Insert(23, (thisWorld) => Rule.PlacedItem(thisWorld, new ItemType("RUN")) && Rule.PlacedItem(thisWorld, new ItemType("Field Secret Key")) && SpecialLogic.AreaAccess(thisWorld, 1));
        world.rules.Insert(13, (thisWorld) => Rule.PlacedItem(thisWorld, new ItemType("RUN")) && Rule.PlacedItem(thisWorld, new ItemType("Field Secret Key")) && SpecialLogic.AreaAccess(thisWorld, 1));
        world.rules.Insert(25, (thisWorld) => Rule.PlacedItem(thisWorld, new ItemType("RUN")) && SpecialLogic.AreaAccess(thisWorld, 1));
        world.rules.Insert(26, (thisWorld) => Rule.PlacedItem(thisWorld, new ItemType("RUN")) && SpecialLogic.AreaAccess(thisWorld, 1));
        world.rules.Insert(27, (thisWorld) => Rule.PlacedItem(thisWorld, new ItemType("RUN")) && SpecialLogic.AreaAccess(thisWorld, 1));
        world.rules.Insert(28, (thisWorld) => Rule.PlacedItem(thisWorld, new ItemType("RUN")) && SpecialLogic.AreaAccess(thisWorld, 1));
        world.rules.Insert(2, (thisWorld) => SpecialLogic.AreaAccess(thisWorld, 1));
        world.rules.Insert(3, (thisWorld) => SpecialLogic.AreaAccess(thisWorld, 1));
        world.rules.Insert(34, (thisWorld) => SpecialLogic.AreaAccess(thisWorld, 1));
        world.rules.Insert(36, (thisWorld) => SpecialLogic.CanCompleteTutorial(thisWorld));
        world.rules.Insert(12, (thisWorld) => SpecialLogic.CanCompleteTutorial(thisWorld));
        world.rules.Insert(41, (thisWorld) => SpecialLogic.CanCompleteTutorial(thisWorld));
        world.rules.Insert(111, (thisWorld) => SpecialLogic.CanCompleteTutorial(thisWorld));
        world.rules.Insert(114, (thisWorld) => Rule.PlacedItem(thisWorld, new ItemType("RUN")) && SpecialLogic.AreaAccess(thisWorld, 1));
        world.rules.Insert(115, (thisWorld) => Rule.PlacedItem(thisWorld, new ItemType("RUN")) && SpecialLogic.AreaAccess(thisWorld, 1));
        world.rules.Insert(116, (thisWorld) => Rule.PlacedItem(thisWorld, new ItemType("RUN")) && SpecialLogic.AreaAccess(thisWorld, 1));
        world.rules.Insert(37, (thisWorld) => Rule.PlacedItem(thisWorld, new ItemType("RUN")) && SpecialLogic.AreaAccess(thisWorld, 1));
        world.rules.Insert(117, (thisWorld) => SpecialLogic.AreaAccess(thisWorld, 3));
        world.rules.Insert(118, (thisWorld) => SpecialLogic.AreaAccess(thisWorld, 3));
        world.rules.Insert(119, (thisWorld) => SpecialLogic.AreaAccess(thisWorld, 3));
        world.rules.Insert(14, (thisWorld) => SpecialLogic.AreaAccess(thisWorld, 3));
        world.rules.Insert(18, (thisWorld) => SpecialLogic.AreaAccess(thisWorld, 3));
        world.rules.Insert(19, (thisWorld) => SpecialLogic.AreaAccess(thisWorld, 3));
        world.rules.Insert(6, (thisWorld) => SpecialLogic.AreaAccess(thisWorld, 3));
        world.rules.Insert(7, (thisWorld) => SpecialLogic.AreaAccess(thisWorld, 3));
        world.rules.Insert(8, (thisWorld) => SpecialLogic.AreaAccess(thisWorld, 3));
        world.rules.Insert(39, (thisWorld) => SpecialLogic.AreaAccess(thisWorld, 3));
        world.rules.Insert(16, (thisWorld) => Rule.PlacedItem(thisWorld, new ItemType("Forest Secret Key")) && SpecialLogic.AreaAccess(thisWorld, 3));
        world.rules.Insert(15, (thisWorld) => Rule.PlacedItem(thisWorld, new ItemType("Forest Secret Key")) && SpecialLogic.AreaAccess(thisWorld, 3));
        world.rules.Insert(17, (thisWorld) => Rule.PlacedItem(thisWorld, new ItemType("Forest Secret Key")) && SpecialLogic.AreaAccess(thisWorld, 3));
        world.rules.Insert(5, (thisWorld) => Rule.PlacedItem(thisWorld, new ItemType("BrokenCake")) && Rule.PlacedItem(thisWorld, new ItemType("Forest Secret Key")) && SpecialLogic.AreaAccess(thisWorld, 3));
        world.rules.Insert(120, (thisWorld) => SpecialLogic.AreaAccess(thisWorld, 4));
        world.rules.Insert(121, (thisWorld) => SpecialLogic.AreaAccess(thisWorld, 4));
        world.rules.Insert(122, (thisWorld) => SpecialLogic.AreaAccess(thisWorld, 4));
        world.rules.Insert(99, (thisWorld) => SpecialLogic.AreaAccess(thisWorld, 2));
        world.rules.Insert(33, (thisWorld) => Rule.PlacedItem(thisWorld, new ItemType("Broken Key A")) && Rule.PlacedItem(thisWorld, new ItemType("Broken Key B")) && Rule.PlacedItem(thisWorld, new ItemType("Broken Key C")) && Rule.PlacedItem(thisWorld, new ItemType("Forest Secret Key")) && SpecialLogic.AreaAccess(thisWorld, 3));
        world.rules.Insert(42, (thisWorld) => SpecialLogic.UnlockedAreaCount(thisWorld, 1));
        world.rules.Insert(43, (thisWorld) => SpecialLogic.UnlockedAreaCount(thisWorld, 1));
        world.rules.Insert(44, (thisWorld) => SpecialLogic.UnlockedAreaCount(thisWorld, 1));
        world.rules.Insert(45, (thisWorld) => SpecialLogic.UnlockedAreaCount(thisWorld, 2));
        world.rules.Insert(46, (thisWorld) => SpecialLogic.UnlockedAreaCount(thisWorld, 2));
        world.rules.Insert(47, (thisWorld) => SpecialLogic.UnlockedAreaCount(thisWorld, 2));
        world.rules.Insert(48, (thisWorld) => SpecialLogic.UnlockedAreaCount(thisWorld, 2));
        world.rules.Insert(49, (thisWorld) => SpecialLogic.UnlockedAreaCount(thisWorld, 2));
        world.rules.Insert(50, (thisWorld) => SpecialLogic.UnlockedAreaCount(thisWorld, 3));
        world.rules.Insert(51, (thisWorld) => SpecialLogic.UnlockedAreaCount(thisWorld, 3));
        world.rules.Insert(52, (thisWorld) => SpecialLogic.UnlockedAreaCount(thisWorld, 3));
        world.rules.Insert(53, (thisWorld) => SpecialLogic.UnlockedAreaCount(thisWorld, 3));
        world.rules.Insert(54, (thisWorld) => SpecialLogic.UnlockedAreaCount(thisWorld, 3));
        world.rules.Insert(55, (thisWorld) => SpecialLogic.UnlockedAreaCount(thisWorld, 4));
        world.rules.Insert(56, (thisWorld) => SpecialLogic.UnlockedAreaCount(thisWorld, 4));
        world.rules.Insert(57, (thisWorld) => SpecialLogic.UnlockedAreaCount(thisWorld, 4));
        world.rules.Insert(58, (thisWorld) => SpecialLogic.UnlockedAreaCount(thisWorld, 4));
        world.rules.Insert(59, (thisWorld) => SpecialLogic.UnlockedAreaCount(thisWorld, 4));
        world.rules.Insert(79, (thisWorld) => SpecialLogic.UnlockedAreaCount(thisWorld, 1) && Rule.PlacedItem(thisWorld, new ItemType("Ralsei")));
        world.rules.Insert(80, (thisWorld) => SpecialLogic.UnlockedAreaCount(thisWorld, 1) && Rule.PlacedItem(thisWorld, new ItemType("Ralsei")));
        world.rules.Insert(81, (thisWorld) => SpecialLogic.UnlockedAreaCount(thisWorld, 1) && Rule.PlacedItem(thisWorld, new ItemType("Ralsei")));
        world.rules.Insert(82, (thisWorld) => SpecialLogic.UnlockedAreaCount(thisWorld, 1) && Rule.PlacedItem(thisWorld, new ItemType("Ralsei")));
        world.rules.Insert(83, (thisWorld) => SpecialLogic.UnlockedAreaCount(thisWorld, 2) && Rule.PlacedItem(thisWorld, new ItemType("Ralsei")));
        world.rules.Insert(84, (thisWorld) => SpecialLogic.UnlockedAreaCount(thisWorld, 2) && Rule.PlacedItem(thisWorld, new ItemType("Ralsei")));
        world.rules.Insert(85, (thisWorld) => SpecialLogic.UnlockedAreaCount(thisWorld, 2) && Rule.PlacedItem(thisWorld, new ItemType("Ralsei")));
        world.rules.Insert(86, (thisWorld) => SpecialLogic.UnlockedAreaCount(thisWorld, 2) && Rule.PlacedItem(thisWorld, new ItemType("Ralsei")));
        world.rules.Insert(87, (thisWorld) => SpecialLogic.UnlockedAreaCount(thisWorld, 2) && Rule.PlacedItem(thisWorld, new ItemType("Ralsei")));
        world.rules.Insert(88, (thisWorld) => SpecialLogic.UnlockedAreaCount(thisWorld, 3) && Rule.PlacedItem(thisWorld, new ItemType("Ralsei")));
        world.rules.Insert(89, (thisWorld) => SpecialLogic.UnlockedAreaCount(thisWorld, 3) && Rule.PlacedItem(thisWorld, new ItemType("Ralsei")));
        world.rules.Insert(90, (thisWorld) => SpecialLogic.UnlockedAreaCount(thisWorld, 3) && Rule.PlacedItem(thisWorld, new ItemType("Ralsei")));
        world.rules.Insert(91, (thisWorld) => SpecialLogic.UnlockedAreaCount(thisWorld, 3) && Rule.PlacedItem(thisWorld, new ItemType("Ralsei")));
        world.rules.Insert(92, (thisWorld) => SpecialLogic.UnlockedAreaCount(thisWorld, 3) && Rule.PlacedItem(thisWorld, new ItemType("Ralsei")));
        world.rules.Insert(60, (thisWorld) => SpecialLogic.UnlockedAreaCount(thisWorld, 1) && Rule.PlacedItem(thisWorld, new ItemType("Susie")));
        world.rules.Insert(61, (thisWorld) => SpecialLogic.UnlockedAreaCount(thisWorld, 1) && Rule.PlacedItem(thisWorld, new ItemType("Susie")));
        world.rules.Insert(62, (thisWorld) => SpecialLogic.UnlockedAreaCount(thisWorld, 1) && Rule.PlacedItem(thisWorld, new ItemType("Susie")));
        world.rules.Insert(63, (thisWorld) => SpecialLogic.UnlockedAreaCount(thisWorld, 1) && Rule.PlacedItem(thisWorld, new ItemType("Susie")));
        world.rules.Insert(64, (thisWorld) => SpecialLogic.UnlockedAreaCount(thisWorld, 2) && Rule.PlacedItem(thisWorld, new ItemType("Susie")));
        world.rules.Insert(65, (thisWorld) => SpecialLogic.UnlockedAreaCount(thisWorld, 2) && Rule.PlacedItem(thisWorld, new ItemType("Susie")));
        world.rules.Insert(66, (thisWorld) => SpecialLogic.UnlockedAreaCount(thisWorld, 2) && Rule.PlacedItem(thisWorld, new ItemType("Susie")));
        world.rules.Insert(67, (thisWorld) => SpecialLogic.UnlockedAreaCount(thisWorld, 2) && Rule.PlacedItem(thisWorld, new ItemType("Susie")));
        world.rules.Insert(68, (thisWorld) => SpecialLogic.UnlockedAreaCount(thisWorld, 2) && Rule.PlacedItem(thisWorld, new ItemType("Susie")));
        world.rules.Insert(69, (thisWorld) => SpecialLogic.UnlockedAreaCount(thisWorld, 3) && Rule.PlacedItem(thisWorld, new ItemType("Susie")));
        world.rules.Insert(70, (thisWorld) => SpecialLogic.UnlockedAreaCount(thisWorld, 3) && Rule.PlacedItem(thisWorld, new ItemType("Susie")));
        world.rules.Insert(71, (thisWorld) => SpecialLogic.UnlockedAreaCount(thisWorld, 3) && Rule.PlacedItem(thisWorld, new ItemType("Susie")));
        world.rules.Insert(72, (thisWorld) => SpecialLogic.UnlockedAreaCount(thisWorld, 3) && Rule.PlacedItem(thisWorld, new ItemType("Susie")));
        world.rules.Insert(73, (thisWorld) => SpecialLogic.UnlockedAreaCount(thisWorld, 3) && Rule.PlacedItem(thisWorld, new ItemType("Susie")));
        world.rules.Insert(74, (thisWorld) => SpecialLogic.UnlockedAreaCount(thisWorld, 4) && Rule.PlacedItem(thisWorld, new ItemType("Susie")));
        world.rules.Insert(78, (thisWorld) => SpecialLogic.UnlockedAreaCount(thisWorld, 4) && Rule.PlacedItem(thisWorld, new ItemType("Susie")));
        world.rules.Insert(75, (thisWorld) => SpecialLogic.UnlockedAreaCount(thisWorld, 4) && Rule.PlacedItem(thisWorld, new ItemType("Susie")));
        world.rules.Insert(76, (thisWorld) => SpecialLogic.UnlockedAreaCount(thisWorld, 4) && Rule.PlacedItem(thisWorld, new ItemType("Susie")));
        world.rules.Insert(77, (thisWorld) => SpecialLogic.UnlockedAreaCount(thisWorld, 4) && Rule.PlacedItem(thisWorld, new ItemType("Susie")));
        world.rules.Insert(93, (thisWorld) => SpecialLogic.UnlockedAreaCount(thisWorld, 4) && Rule.PlacedItem(thisWorld, new ItemType("Ralsei")));
        world.rules.Insert(94, (thisWorld) => SpecialLogic.UnlockedAreaCount(thisWorld, 4) && Rule.PlacedItem(thisWorld, new ItemType("Ralsei")));
        world.rules.Insert(95, (thisWorld) => SpecialLogic.UnlockedAreaCount(thisWorld, 4) && Rule.PlacedItem(thisWorld, new ItemType("Ralsei")));
        world.rules.Insert(96, (thisWorld) => SpecialLogic.UnlockedAreaCount(thisWorld, 4) && Rule.PlacedItem(thisWorld, new ItemType("Ralsei")));
        world.rules.Insert(97, (thisWorld) => SpecialLogic.UnlockedAreaCount(thisWorld, 4) && Rule.PlacedItem(thisWorld, new ItemType("Ralsei")));
        world.rules.Insert(100, (thisWorld) => SpecialLogic.AreaAccess(thisWorld, 3));
        world.rules.Insert(126, (thisWorld) => SpecialLogic.AreaAccess(thisWorld, 1));
        world.rules.Insert(102, (thisWorld) => SpecialLogic.AreaAccess(thisWorld, 1));
        world.rules.Insert(129, (thisWorld) => SpecialLogic.AreaAccess(thisWorld, 2));
        world.rules.Insert(38, (thisWorld) => SpecialLogic.AreaAccess(thisWorld, 2));
        world.rules.Insert(130, (thisWorld) => SpecialLogic.AreaAccess(thisWorld, 2));
        world.rules.Insert(131, (thisWorld) => SpecialLogic.AreaAccess(thisWorld, 2));
        world.rules.Insert(103, (thisWorld) => SpecialLogic.AreaAccess(thisWorld, 3));
        world.rules.Insert(132, (thisWorld) => SpecialLogic.AreaAccess(thisWorld, 3));
        world.rules.Insert(133, (thisWorld) => SpecialLogic.AreaAccess(thisWorld, 3));
        world.rules.Insert(134, (thisWorld) => SpecialLogic.AreaAccess(thisWorld, 3));
        world.rules.Insert(135, (thisWorld) => SpecialLogic.AreaAccess(thisWorld, 4));
        world.rules.Insert(136, (thisWorld) => SpecialLogic.AreaAccess(thisWorld, 4));
        world.rules.Insert(137, (thisWorld) => SpecialLogic.AreaAccess(thisWorld, 4));
        world.rules.Insert(139, (thisWorld) => SpecialLogic.AreaAccess(thisWorld, 4));
        world.rules.Insert(101, (thisWorld) => SpecialLogic.AreaAccess(thisWorld, 4));
        world.rules.Insert(112, (thisWorld) => SpecialLogic.AreaAccess(thisWorld, 4));
        world.rules.Insert(21, (thisWorld) => SpecialLogic.AreaAccess(thisWorld, 4));
        world.rules.Insert(22, (thisWorld) => SpecialLogic.AreaAccess(thisWorld, 4));
        world.rules.Insert(29, (thisWorld) => SpecialLogic.AreaAccess(thisWorld, 4));
        world.rules.Insert(30, (thisWorld) => SpecialLogic.AreaAccess(thisWorld, 4));
        world.rules.Insert(31, (thisWorld) => SpecialLogic.AreaAccess(thisWorld, 4));
        world.rules.Insert(32, (thisWorld) => SpecialLogic.AreaAccess(thisWorld, 4));
        world.rules.Insert(35, (thisWorld) => SpecialLogic.AreaAccess(thisWorld, 4));
        world.rules.Insert(40, (thisWorld) => SpecialLogic.AreaAccess(thisWorld, 1) && SpecialLogic.AreaAccess(thisWorld, 4));
        world.rules.Insert(20, (thisWorld) => Rule.PlacedItem(thisWorld, new ItemType("Door Key")) && SpecialLogic.AreaAccess(thisWorld, 4));
        world.rules.Insert(24, (thisWorld) => Rule.PlacedItem(thisWorld, new ItemType("Door Key")) && SpecialLogic.AreaAccess(thisWorld, 4));
        world.rules.Insert(138, (thisWorld) => Rule.PlacedItem(thisWorld, new ItemType("SPARE")) && Rule.PlacedItem(thisWorld, new ItemType("MAGIC")) && SpecialLogic.ActAvailability(thisWorld) && SpecialLogic.AreaAccess(thisWorld, 4));
        world.rules.Insert(98, (thisWorld) => Rule.PlacedItem(thisWorld, new ItemType("SPARE")) && Rule.PlacedItem(thisWorld, new ItemType("MAGIC")) && SpecialLogic.ActAvailability(thisWorld) && SpecialLogic.AreaAccess(thisWorld, 4));
        world.rules.Insert(110, (thisWorld) => Rule.PlacedItem(thisWorld, new ItemType("RUN")) && SpecialLogic.AreaAccess(thisWorld, 1));
        world.rules.Insert(127, (thisWorld) => Rule.PlacedItem(thisWorld, new ItemType("RUN")) && SpecialLogic.AreaAccess(thisWorld, 1));
        world.rules.Insert(128, (thisWorld) => Rule.PlacedItem(thisWorld, new ItemType("RUN")) && SpecialLogic.AreaAccess(thisWorld, 1));
    }
}
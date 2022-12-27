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
        if (world.starting.Exists(x => x.name == itemId.name))
        {
            return true;
        }
        if (world.trueStarting.Exists(x => x.name == itemId.name))
        {
            return true;
        }
        return false;
    }
    public static bool ReturnTrue(World world)
    {
        return true;
    }
}

public class World
{
    public List<ItemType> locations = new List<ItemType>();
    public List<ItemType> equipment = new List<ItemType>();
    public List<ItemType> wantedLocs = new List<ItemType>();
    public List<ItemType> items = new List<ItemType>();
    public List<ItemType> rooms = new List<ItemType>();
    public List<ItemType> itemsJunk = new List<ItemType>();
    public List<ItemType> starting = new List<ItemType>();
    public List<ItemType> trueStarting = new List<ItemType>();
    public List<Predicate<World>> rules = new List<Predicate<World>>();
    public List<Predicate<World>> roomRules = new List<Predicate<World>>();
    public List<int> options = new List<int>();
    public Random rng = new Random();
    public World()
    {
        for (var i = 0; i < 300; i++)
        {
            rules.Add((world) => SpecialLogic.ReturnTrue(world));
        }
        for (var i = 0; i <= 99; i++)
        {
            options.Add(0);
        }
        for (int i = 0; i <= 300; i++)
        {
            locations.Add(new ItemType(""));
        }
        for (int i = 0; i < 3; i++)
        {
            equipment.Add(new ItemType(""));
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
            if (itm.Split('=')[1].ToLower() != "true" && itm.Split('=')[1].ToLower() != "false")
            {
                options[int.Parse(itm.Split('(', ')')[1])] = int.Parse(itm.Split('=')[1]);
            }
            else
            {
                if (bool.Parse(itm.Split('=')[1].ToLower()))
                {
                    options[int.Parse(itm.Split('(', ')')[1])] = 1;
                }
                else
                {
                    options[int.Parse(itm.Split('(', ')')[1])] = 0;
                }
            }
        }
        for (int i = 0; i < 99; i++)
        {
            SpecialLogic.savedAreas.Add(false);
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
        var backupTrueStarting = new List<ItemType>(trueStarting);
        var lastLocations = new List<List<ItemType>>();
        var lastItems = new List<ItemType>();
        var itemsRemoved = new List<ItemType>();
        int timeSinceBack = 0;
        int backCount = 0;
        if (options[1] > 0)
        {
            for (int i = 0; i < options[1]; i++)
            {
                var chosenItem = 0;
                if (items.Count <= 0)
                {
                    chosenItem = rng.Next(itemsJunk.Count);
                    trueStarting.Add(itemsJunk[chosenItem]);
                    itemsJunk.RemoveAt(chosenItem);
                }
                else
                {
                    chosenItem = rng.Next(items.Count);
                    trueStarting.Add(items[chosenItem]);
                    items.RemoveAt(chosenItem);
                }
            }
        }
        while (locations.Exists(x => x.name == "Null") || items.Exists(x => x.name == "Kingly Key Piece") || itemsJunk.Exists(x => x.name == "Kingly Key Piece") || items.Exists(x => x.name == "Kingly Key") || itemsJunk.Exists(x => x.name == "Kingly Key"))
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
                    emptyLocs.Remove(item);
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
            for (int i = 1; i < SpecialLogic.areaAccess.Count + 1; i++)
            {
                if (SpecialLogic.savedAreas[i] == false)
                {
                    SpecialLogic.savedAreas[i] = SpecialLogic.areaAccess[i].Invoke(this);
                }
            }
            if (items.Count + itemsJunk.Count <= 0 || (toChooseFrom.Count <= 0 && lastLocations.Count < 5))
            {
                locations = new List<ItemType>(backupLocations);
                items = new List<ItemType>(backupItems);
                itemsJunk = new List<ItemType>(backupItemsJunk);
                trueStarting = new List<ItemType>(backupTrueStarting);
                lastLocations = new List<List<ItemType>>();
                lastItems = new List<ItemType>();
                itemsRemoved = new List<ItemType>();
                timeSinceBack = 0;
                backCount = 0;
                if (options[1] > 0)
                {
                    for (int i = 0; i < options[1]; i++)
                    {
                        var chosenItem = 0;
                        if (items.Count <= 0)
                        {
                            chosenItem = rng.Next(itemsJunk.Count);
                            trueStarting.Add(itemsJunk[chosenItem]);
                            itemsJunk.RemoveAt(chosenItem);
                        }
                        else
                        {
                            chosenItem = rng.Next(items.Count);
                            trueStarting.Add(items[chosenItem]);
                            items.RemoveAt(chosenItem);
                        }
                    }
                }
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
                for (int i = 1; i < SpecialLogic.areaAccess.Count + 1; i++)
                {
                    SpecialLogic.savedAreas[i] = SpecialLogic.areaAccess[i].Invoke(this);
                }
            }
            else
            {
                int chosen = toChooseFrom[rng.Next(toChooseFrom.Count)];
                var chosenItem = -1;
                lastLocations.Add(new List<ItemType>(locations));
                if ((items.Count <= 0 || toChooseFrom.Count > items.Count * 2) && itemsJunk.Count > 0)
                {
                    if (itemsJunk.Exists(x => x.name == "Kingly Key Piece"))
                    {
                        chosenItem = itemsJunk.FindIndex(x => x.name == "Kingly Key Piece");
                        lastItems.Add(new ItemType(itemsJunk[chosenItem]));
                        PlaceItem(chosen, itemsJunk[chosenItem]);
                        itemsJunk.RemoveAt(chosenItem);
                    }
                    else if (itemsJunk.Exists(x => x.name == "Kingly Key"))
                    {
                        chosenItem = itemsJunk.FindIndex(x => x.name == "Kingly Key");
                        lastItems.Add(new ItemType(itemsJunk[chosenItem]));
                        PlaceItem(chosen, itemsJunk[chosenItem]);
                        itemsJunk.RemoveAt(chosenItem);
                    }
                    else
                    {
                        chosenItem = rng.Next(itemsJunk.Count);
                        lastItems.Add(new ItemType(itemsJunk[chosenItem]));
                        PlaceItem(chosen, itemsJunk[chosenItem]);
                        itemsJunk.RemoveAt(chosenItem);
                    }
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
                    locsLeft += emptyLocs[i].ToString() + ",";
                }
                string locsAvail = "";
                for (int i = 0; i < toChooseFrom.Count; i++)
                {
                    locsAvail += toChooseFrom[i].ToString() + ",";
                }
                Console.WriteLine("Placed item.\n" + locsAvail + " locations accessible.\n" + locsLeft + " locations left.\n" + (items.Count + itemsJunk.Count + itemsRemoved.Count).ToString() + " items left.");
            }
        }
    }

    public void AddRooms()
    {
        rooms.Add(new ItemType("room_field_startA"));
        rooms.Add(new ItemType("room_field_forestA"));
        rooms.Add(new ItemType("room_field_forestB"));
        rooms.Add(new ItemType("room_field1A"));
        rooms.Add(new ItemType("room_field1B"));
        rooms.Add(new ItemType("room_field2A"));
        rooms.Add(new ItemType("room_field2B"));
        rooms.Add(new ItemType("room_field2C"));
        rooms.Add(new ItemType("room_field2AB"));
        rooms.Add(new ItemType("room_field_topchefD"));
        rooms.Add(new ItemType("room_field_topchefA"));
        rooms.Add(new ItemType("room_field_puzzle1B"));
        rooms.Add(new ItemType("room_field_puzzle1A"));
        rooms.Add(new ItemType("room_field_mazeA"));
        rooms.Add(new ItemType("room_field_mazeB"));
        rooms.Add(new ItemType("room_field_puzzle2B"));
        rooms.Add(new ItemType("room_field_puzzle2A"));
        rooms.Add(new ItemType("room_field_getsusieB"));
        rooms.Add(new ItemType("room_field_getsusieA"));
        rooms.Add(new ItemType("room_field_shop1A"));
        rooms.Add(new ItemType("room_field_shop1B"));
        rooms.Add(new ItemType("room_field_shop1C"));
        rooms.Add(new ItemType("room_field_puzzletutorialB"));
        rooms.Add(new ItemType("room_field3A"));
        rooms.Add(new ItemType("room_field3D"));
        rooms.Add(new ItemType("room_field_boxpuzzleA"));
        rooms.Add(new ItemType("room_field_boxpuzzleB"));
        rooms.Add(new ItemType("room_field_4A"));
        rooms.Add(new ItemType("room_field_4B"));
        rooms.Add(new ItemType("room_field_4C"));
        rooms.Add(new ItemType("room_field_secret1B"));
    }

    public void AddItems()
    {
        if (options[8] == 1)
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
            if (options[25] == 1)
            {
                for (int i = 0; i < 10; i++)
                {
                    itemsJunk.Add(new ItemType("Kingly Key Piece"));
                }
            }
            else
            {
                itemsJunk.Add(new ItemType("Kingly Key"));
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
        if (options[7] == 1)
        {
            if (options[24] == 1)
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
        if (options[4] == 1)
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
                locations[41 + i] = new ItemType("krislvl");
                locations[60 + i] = new ItemType("susielvl");
                locations[79 + i] = new ItemType("ralseilvl");
            }
        }
        if (options[13] == 1)
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
        if (options[11] == 1)
        {
            items.Add(new ItemType("FIGHT"));
            items.Add(new ItemType("MAGIC"));
            items.Add(new ItemType("ITEM"));
            items.Add(new ItemType("SPARE"));
            items.Add(new ItemType("DEFEND"));
        }
        else
        {
            trueStarting.Add(new ItemType("FIGHT"));
            trueStarting.Add(new ItemType("MAGIC"));
            trueStarting.Add(new ItemType("ITEM"));
            trueStarting.Add(new ItemType("SPARE"));
            trueStarting.Add(new ItemType("DEFEND"));
        }
        if (options[23] == 1)
        {
            items.Add(new ItemType("C MENU"));
        }
        else
        {
            trueStarting.Add(new ItemType("C MENU"));
        }
        if (options[22] == 1)
        {
            items.Add(new ItemType("RUN"));
        }
        else
        {
            trueStarting.Add(new ItemType("RUN"));
        }
        if (options[21] == 1)
        {
            items.Add(new ItemType("TP BAR"));
        }
        else
        {
            trueStarting.Add(new ItemType("TP BAR"));
        }
        if (options[17] == 1)
        {
            items.Add(new ItemType("LEFT SOUL"));
            items.Add(new ItemType("RIGHT SOUL"));
            items.Add(new ItemType("UP SOUL"));
            items.Add(new ItemType("DOWN SOUL"));
        }
        else
        {
            trueStarting.Add(new ItemType("LEFT SOUL"));
            trueStarting.Add(new ItemType("RIGHT SOUL"));
            trueStarting.Add(new ItemType("UP SOUL"));
            trueStarting.Add(new ItemType("DOWN SOUL"));
        }
        if (options[18] == 1)
        {
            items.Add(new ItemType("SAVE HEAL"));
        }
        else
        {
            trueStarting.Add(new ItemType("SAVE HEAL"));
        }
        if (options[19] == 1)
        {
            items.Add(new ItemType("SAVING"));
        }
        else
        {
            trueStarting.Add(new ItemType("SAVING"));
        }
        if (options[77] == 1)
        {
            for (int i = 123; i <= 125; i++)
            {
                itemsJunk.Add(new ItemType("Save"));
            }
        }
        else
        {
            for (int i = 123; i <= 125; i++)
            {
                PlaceForcedItem(i, new ItemType("Save"));
            }
        }
        if (options[20] == 1)
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
        if (options[10] == 1)
        {
            items.Add(new ItemType("kACT"));
            itemsJunk.Add(new ItemType("rHeal Prayer"));
            itemsJunk.Add(new ItemType("rPacify"));
            itemsJunk.Add(new ItemType("sRude Buster"));
            if (options[24] == 1)
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
            trueStarting.Add(new ItemType("kACT"));
            trueStarting.Add(new ItemType("rHeal Prayer"));
            trueStarting.Add(new ItemType("rPacify"));
            trueStarting.Add(new ItemType("sRude Buster"));
        }
        if (options[5] == 1)
        {
            itemsJunk.Add(new ItemType("Spookysword"));
            itemsJunk.Add(new ItemType("Brave Ax"));
            itemsJunk.Add(new ItemType("Devilsknife"));
            itemsJunk.Add(new ItemType("Ragger"));
            itemsJunk.Add(new ItemType("DaintyScarf"));
            if (options[24] == 1)
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
        if (options[6] == 1)
        {
            if (options[24] == 1)
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
        if (options[3] == 1)
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
        if (options[75] == 1)
        {
            itemsJunk.Add(new ItemType("Wood Blade"));
            itemsJunk.Add(new ItemType("Mane Ax"));
            itemsJunk.Add(new ItemType("Red Scarf"));
        }
        else
        {
            equipment[0] = new ItemType("Wood Blade");
            equipment[1] = new ItemType("Mane Ax");
            equipment[2] = new ItemType("Red Scarf");
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
            var thisChar = "";
            var amt = 0;
            while (!file.EndOfStream)
            {
                var itm = file.ReadLine();
                if (itm.Contains("name="))
                {
                    thisName = itm.Replace("name=", "");
                }
                if (itm.Contains("owner="))
                {
                    thisChar = itm.Replace("owner=", "")[0].ToString();
                }
                if (itm.Contains("amount="))
                {
                    amt = int.Parse(itm.Replace("amount=", ""));
                }
            }
            for (int i = 0; i < amt; i++)
            {
                itemsJunk.Add(new ItemType(thisChar + thisName));
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
                items.RemoveAll(x => x.name == itm);
            }
            if (itemsJunk.Exists(x => x.name == itm))
            {
                itemsJunk.RemoveAll(x => x.name == itm);
            }
        }
        file.Close();
        for (int i = 0; i < 19; i++)
        {
            if (!items.Exists(x => x.name == "susie") && !itemsJunk.Exists(x => x.name == "susie"))
            {
                locations[60 + i] = new ItemType("susielvl");
                items.RemoveAll(x => x.name == "susielvl");
                itemsJunk.RemoveAll(x => x.name == "susielvl");
            }
            if (!items.Exists(x => x.name == "ralsei") && !itemsJunk.Exists(x => x.name == "ralsei"))
            {
                locations[79 + i] = new ItemType("ralseilvl");
                items.RemoveAll(x => x.name == "ralseilvl");
                itemsJunk.RemoveAll(x => x.name == "ralseilvl");
            }
        }
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
        while (locations.FindAll(x => x.name == "Null").Count > items.Count + itemsJunk.Count - options[1] - (wantedLocs.Count - wantedLocs.FindAll(x => x.name == "").Count))
        {
            itemsJunk.Add(new ItemType("gold"));
        }
    }
}

public static class SpecialLogic
{
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
        return ((Rule.PlacedItem(world, new ItemType("LEFT SOUL")) || Rule.PlacedItem(world, new ItemType("RIGHT SOUL"))) && (Rule.PlacedItem(world, new ItemType("DOWN SOUL")) || Rule.PlacedItem(world, new ItemType("UP SOUL"))) && (Rule.PlacedItem(world, new ItemType("DEFEND")) || Rule.PlacedItem(world, new ItemType("FIGHT"))));
    }
    public static bool UnlockedAreaCount(World world, int amountToUnlock)
    {
        int unlocked = 0;
        if (SpecialLogic.savedAreas[1])
        {
            unlocked++;
        }
        if (SpecialLogic.savedAreas[2])
        {
            unlocked++;
        }
        if (SpecialLogic.savedAreas[3])
        {
            unlocked++;
        }
        if (SpecialLogic.savedAreas[4])
        {
            unlocked++;
        }
        return (unlocked >= amountToUnlock);
    }
    public static List<bool> savedAreas = new List<bool>();
    public static Dictionary<int, Func<World, bool>> areaAccess = new Dictionary<int, Func<World, bool>>
    {
        {1, (w) => SpecialLogic.CanCompleteTutorial(w) && ((w.options[76] >= 3 && Rule.PlacedItem(w, new ItemType("SPARE"))) || (Rule.PlacedItem(w, new ItemType("FIGHT")) || (SpecialLogic.ActAvailability(w) && Rule.PlacedItem(w, new ItemType("SPARE"))))) && Rule.PlacedItem(w, new ItemType("Field Key"))},
        {2, (w) => (SpecialLogic.savedAreas[1] && (Rule.PlacedItem(w, new ItemType("RUN")) || w.options[3] == 1) && Rule.PlacedItem(w, new ItemType("Board Key"))) || (SpecialLogic.savedAreas[1] && (SpecialLogic.ActAvailability(w) || w.options[3] == 1) && Rule.PlacedItem(w, new ItemType("BoardShortcutDoor")) && Rule.PlacedItem(w, new ItemType("FieldShortcutDoor")))},
        {3, (w) => (SpecialLogic.savedAreas[2] && (SpecialLogic.ActAvailability(w) || w.options[3] == 1) && Rule.PlacedItem(w, new ItemType("Forest Key"))) || (SpecialLogic.savedAreas[1] && Rule.PlacedItem(w, new ItemType("ForestShortcutDoor")) && Rule.PlacedItem(w, new ItemType("FieldShortcutDoor")))},
        {4, (w) => (SpecialLogic.savedAreas[3] && Rule.PlacedItem(w, new ItemType("Castle Key"))) || (SpecialLogic.savedAreas[1] && Rule.PlacedItem(w, new ItemType("CastleShortcutDoor")) && Rule.PlacedItem(w, new ItemType("FieldShortcutDoor")))},
    };
}

public class Logic
{
    public void SetLogic(World inWorld)
    {
        inWorld.rules[0] = (world) => Rule.PlacedItem(world, new ItemType("Field Secret Key")) && SpecialLogic.savedAreas[1];
        inWorld.rules[1] = (world) => Rule.PlacedItem(world, new ItemType("Field Secret Key")) && SpecialLogic.savedAreas[1];
        inWorld.rules[2] = (world) => SpecialLogic.savedAreas[1];
        inWorld.rules[3] = (world) => SpecialLogic.savedAreas[1];
        inWorld.rules[5] = (world) => Rule.PlacedItem(world, new ItemType("BrokenCake")) && Rule.PlacedItem(world, new ItemType("Forest Secret Key")) && SpecialLogic.savedAreas[3];
        inWorld.rules[6] = (world) => SpecialLogic.savedAreas[3];
        inWorld.rules[7] = (world) => SpecialLogic.savedAreas[3];
        inWorld.rules[8] = (world) => SpecialLogic.savedAreas[3];
        inWorld.rules[9] = (world) => Rule.PlacedItem(world, new ItemType("Top Cake")) && SpecialLogic.savedAreas[1];
        inWorld.rules[11] = (world) => Rule.PlacedItem(world, new ItemType("Manual")) && Rule.PlacedItem(world, new ItemType("C MENU"));
        inWorld.rules[12] = (world) => SpecialLogic.CanCompleteTutorial(world);
        inWorld.rules[13] = (world) => (Rule.PlacedItem(world, new ItemType("RUN")) || world.options[3] == 1) && Rule.PlacedItem(world, new ItemType("Field Secret Key")) && SpecialLogic.savedAreas[1];
        inWorld.rules[14] = (world) => SpecialLogic.savedAreas[3];
        inWorld.rules[15] = (world) => Rule.PlacedItem(world, new ItemType("Forest Secret Key")) && SpecialLogic.savedAreas[3];
        inWorld.rules[16] = (world) => Rule.PlacedItem(world, new ItemType("Forest Secret Key")) && SpecialLogic.savedAreas[3];
        inWorld.rules[17] = (world) => Rule.PlacedItem(world, new ItemType("Forest Secret Key")) && SpecialLogic.savedAreas[3];
        inWorld.rules[18] = (world) => SpecialLogic.savedAreas[3];
        inWorld.rules[19] = (world) => SpecialLogic.savedAreas[3];
        inWorld.rules[20] = (world) => Rule.PlacedItem(world, new ItemType("Door Key")) && SpecialLogic.savedAreas[4];
        inWorld.rules[21] = (world) => SpecialLogic.savedAreas[4];
        inWorld.rules[22] = (world) => SpecialLogic.savedAreas[4];
        inWorld.rules[23] = (world) => (Rule.PlacedItem(world, new ItemType("RUN")) || world.options[3] == 1) && Rule.PlacedItem(world, new ItemType("Field Secret Key")) && SpecialLogic.savedAreas[1];
        inWorld.rules[24] = (world) => Rule.PlacedItem(world, new ItemType("Door Key")) && SpecialLogic.savedAreas[4];
        inWorld.rules[25] = (world) => (Rule.PlacedItem(world, new ItemType("RUN")) || world.options[3] == 1) && SpecialLogic.savedAreas[1];
        inWorld.rules[26] = (world) => (Rule.PlacedItem(world, new ItemType("RUN")) || world.options[3] == 1) && SpecialLogic.savedAreas[1];
        inWorld.rules[27] = (world) => (Rule.PlacedItem(world, new ItemType("RUN")) || world.options[3] == 1) && SpecialLogic.savedAreas[1];
        inWorld.rules[28] = (world) => (Rule.PlacedItem(world, new ItemType("RUN")) || world.options[3] == 1) && SpecialLogic.savedAreas[1];
        inWorld.rules[29] = (world) => SpecialLogic.savedAreas[4];
        inWorld.rules[30] = (world) => SpecialLogic.savedAreas[4];
        inWorld.rules[31] = (world) => SpecialLogic.savedAreas[4];
        inWorld.rules[32] = (world) => SpecialLogic.savedAreas[4];
        inWorld.rules[33] = (world) => Rule.PlacedItem(world, new ItemType("Broken Key A")) && Rule.PlacedItem(world, new ItemType("Broken Key B")) && Rule.PlacedItem(world, new ItemType("Broken Key C")) && Rule.PlacedItem(world, new ItemType("Forest Secret Key")) && SpecialLogic.savedAreas[3];
        inWorld.rules[34] = (world) => SpecialLogic.savedAreas[1];
        inWorld.rules[35] = (world) => SpecialLogic.savedAreas[4];
        inWorld.rules[36] = (world) => SpecialLogic.CanCompleteTutorial(world);
        inWorld.rules[37] = (world) => (Rule.PlacedItem(world, new ItemType("RUN")) || world.options[3] == 1) && SpecialLogic.savedAreas[1];
        inWorld.rules[38] = (world) => SpecialLogic.savedAreas[2] && (SpecialLogic.ActAvailability(world) || world.options[3] == 1);
        inWorld.rules[39] = (world) => SpecialLogic.savedAreas[3];
        inWorld.rules[40] = (world) => SpecialLogic.savedAreas[1] && SpecialLogic.savedAreas[4];
        inWorld.rules[41] = (world) => Rule.PlacedItem(world, new ItemType("FIGHT")) || Rule.PlacedItem(world, new ItemType("DEFEND")) || Rule.PlacedItem(world, new ItemType("SPARE"));
        inWorld.rules[42] = (world) => SpecialLogic.UnlockedAreaCount(world, 1);
        inWorld.rules[43] = (world) => SpecialLogic.UnlockedAreaCount(world, 1);
        inWorld.rules[44] = (world) => SpecialLogic.UnlockedAreaCount(world, 1);
        inWorld.rules[45] = (world) => SpecialLogic.UnlockedAreaCount(world, 2);
        inWorld.rules[46] = (world) => SpecialLogic.UnlockedAreaCount(world, 2);
        inWorld.rules[47] = (world) => SpecialLogic.UnlockedAreaCount(world, 2);
        inWorld.rules[48] = (world) => SpecialLogic.UnlockedAreaCount(world, 2);
        inWorld.rules[49] = (world) => SpecialLogic.UnlockedAreaCount(world, 2);
        inWorld.rules[50] = (world) => SpecialLogic.UnlockedAreaCount(world, 3);
        inWorld.rules[51] = (world) => SpecialLogic.UnlockedAreaCount(world, 3);
        inWorld.rules[52] = (world) => SpecialLogic.UnlockedAreaCount(world, 3);
        inWorld.rules[53] = (world) => SpecialLogic.UnlockedAreaCount(world, 3);
        inWorld.rules[54] = (world) => SpecialLogic.UnlockedAreaCount(world, 3);
        inWorld.rules[55] = (world) => SpecialLogic.UnlockedAreaCount(world, 4);
        inWorld.rules[56] = (world) => SpecialLogic.UnlockedAreaCount(world, 4);
        inWorld.rules[57] = (world) => SpecialLogic.UnlockedAreaCount(world, 4);
        inWorld.rules[58] = (world) => SpecialLogic.UnlockedAreaCount(world, 4);
        inWorld.rules[59] = (world) => SpecialLogic.UnlockedAreaCount(world, 4);
        inWorld.rules[60] = (world) => SpecialLogic.UnlockedAreaCount(world, 1) && Rule.PlacedItem(world, new ItemType("Susie"));
        inWorld.rules[61] = (world) => SpecialLogic.UnlockedAreaCount(world, 1) && Rule.PlacedItem(world, new ItemType("Susie"));
        inWorld.rules[62] = (world) => SpecialLogic.UnlockedAreaCount(world, 1) && Rule.PlacedItem(world, new ItemType("Susie"));
        inWorld.rules[63] = (world) => SpecialLogic.UnlockedAreaCount(world, 1) && Rule.PlacedItem(world, new ItemType("Susie"));
        inWorld.rules[64] = (world) => SpecialLogic.UnlockedAreaCount(world, 2) && Rule.PlacedItem(world, new ItemType("Susie"));
        inWorld.rules[65] = (world) => SpecialLogic.UnlockedAreaCount(world, 2) && Rule.PlacedItem(world, new ItemType("Susie"));
        inWorld.rules[66] = (world) => SpecialLogic.UnlockedAreaCount(world, 2) && Rule.PlacedItem(world, new ItemType("Susie"));
        inWorld.rules[67] = (world) => SpecialLogic.UnlockedAreaCount(world, 2) && Rule.PlacedItem(world, new ItemType("Susie"));
        inWorld.rules[68] = (world) => SpecialLogic.UnlockedAreaCount(world, 2) && Rule.PlacedItem(world, new ItemType("Susie"));
        inWorld.rules[69] = (world) => SpecialLogic.UnlockedAreaCount(world, 3) && Rule.PlacedItem(world, new ItemType("Susie"));
        inWorld.rules[70] = (world) => SpecialLogic.UnlockedAreaCount(world, 3) && Rule.PlacedItem(world, new ItemType("Susie"));
        inWorld.rules[71] = (world) => SpecialLogic.UnlockedAreaCount(world, 3) && Rule.PlacedItem(world, new ItemType("Susie"));
        inWorld.rules[72] = (world) => SpecialLogic.UnlockedAreaCount(world, 3) && Rule.PlacedItem(world, new ItemType("Susie"));
        inWorld.rules[73] = (world) => SpecialLogic.UnlockedAreaCount(world, 3) && Rule.PlacedItem(world, new ItemType("Susie"));
        inWorld.rules[74] = (world) => SpecialLogic.UnlockedAreaCount(world, 4) && Rule.PlacedItem(world, new ItemType("Susie"));
        inWorld.rules[75] = (world) => SpecialLogic.UnlockedAreaCount(world, 4) && Rule.PlacedItem(world, new ItemType("Susie"));
        inWorld.rules[76] = (world) => SpecialLogic.UnlockedAreaCount(world, 4) && Rule.PlacedItem(world, new ItemType("Susie"));
        inWorld.rules[77] = (world) => SpecialLogic.UnlockedAreaCount(world, 4) && Rule.PlacedItem(world, new ItemType("Susie"));
        inWorld.rules[78] = (world) => SpecialLogic.UnlockedAreaCount(world, 4) && Rule.PlacedItem(world, new ItemType("Susie"));
        inWorld.rules[79] = (world) => SpecialLogic.UnlockedAreaCount(world, 1) && Rule.PlacedItem(world, new ItemType("Ralsei"));
        inWorld.rules[80] = (world) => SpecialLogic.UnlockedAreaCount(world, 1) && Rule.PlacedItem(world, new ItemType("Ralsei"));
        inWorld.rules[81] = (world) => SpecialLogic.UnlockedAreaCount(world, 1) && Rule.PlacedItem(world, new ItemType("Ralsei"));
        inWorld.rules[82] = (world) => SpecialLogic.UnlockedAreaCount(world, 1) && Rule.PlacedItem(world, new ItemType("Ralsei"));
        inWorld.rules[83] = (world) => SpecialLogic.UnlockedAreaCount(world, 2) && Rule.PlacedItem(world, new ItemType("Ralsei"));
        inWorld.rules[84] = (world) => SpecialLogic.UnlockedAreaCount(world, 2) && Rule.PlacedItem(world, new ItemType("Ralsei"));
        inWorld.rules[85] = (world) => SpecialLogic.UnlockedAreaCount(world, 2) && Rule.PlacedItem(world, new ItemType("Ralsei"));
        inWorld.rules[86] = (world) => SpecialLogic.UnlockedAreaCount(world, 2) && Rule.PlacedItem(world, new ItemType("Ralsei"));
        inWorld.rules[87] = (world) => SpecialLogic.UnlockedAreaCount(world, 2) && Rule.PlacedItem(world, new ItemType("Ralsei"));
        inWorld.rules[88] = (world) => SpecialLogic.UnlockedAreaCount(world, 3) && Rule.PlacedItem(world, new ItemType("Ralsei"));
        inWorld.rules[89] = (world) => SpecialLogic.UnlockedAreaCount(world, 3) && Rule.PlacedItem(world, new ItemType("Ralsei"));
        inWorld.rules[90] = (world) => SpecialLogic.UnlockedAreaCount(world, 3) && Rule.PlacedItem(world, new ItemType("Ralsei"));
        inWorld.rules[91] = (world) => SpecialLogic.UnlockedAreaCount(world, 3) && Rule.PlacedItem(world, new ItemType("Ralsei"));
        inWorld.rules[92] = (world) => SpecialLogic.UnlockedAreaCount(world, 3) && Rule.PlacedItem(world, new ItemType("Ralsei"));
        inWorld.rules[93] = (world) => SpecialLogic.UnlockedAreaCount(world, 4) && Rule.PlacedItem(world, new ItemType("Ralsei"));
        inWorld.rules[94] = (world) => SpecialLogic.UnlockedAreaCount(world, 4) && Rule.PlacedItem(world, new ItemType("Ralsei"));
        inWorld.rules[95] = (world) => SpecialLogic.UnlockedAreaCount(world, 4) && Rule.PlacedItem(world, new ItemType("Ralsei"));
        inWorld.rules[96] = (world) => SpecialLogic.UnlockedAreaCount(world, 4) && Rule.PlacedItem(world, new ItemType("Ralsei"));
        inWorld.rules[97] = (world) => SpecialLogic.UnlockedAreaCount(world, 4) && Rule.PlacedItem(world, new ItemType("Ralsei"));
        inWorld.rules[98] = (world) => (SpecialLogic.ActAvailability(world) || world.options[3] == 1) && SpecialLogic.savedAreas[4];
        inWorld.rules[99] = (world) => SpecialLogic.savedAreas[2];
        inWorld.rules[100] = (world) => SpecialLogic.savedAreas[3];
        inWorld.rules[101] = (world) => SpecialLogic.savedAreas[4];
        inWorld.rules[102] = (world) => SpecialLogic.savedAreas[1];
        inWorld.rules[103] = (world) => SpecialLogic.savedAreas[3];
        inWorld.rules[110] = (world) => (Rule.PlacedItem(world, new ItemType("RUN")) || world.options[3] == 1) && SpecialLogic.savedAreas[1];
        inWorld.rules[111] = (world) => Rule.PlacedItem(world, new ItemType("FIGHT")) || Rule.PlacedItem(world, new ItemType("DEFEND")) || Rule.PlacedItem(world, new ItemType("SPARE"));
        inWorld.rules[112] = (world) => SpecialLogic.savedAreas[4];
        inWorld.rules[113] = (world) => SpecialLogic.savedAreas[1];
        inWorld.rules[114] = (world) => (Rule.PlacedItem(world, new ItemType("RUN")) || world.options[3] == 1) && SpecialLogic.savedAreas[1];
        inWorld.rules[115] = (world) => (Rule.PlacedItem(world, new ItemType("RUN")) || world.options[3] == 1) && SpecialLogic.savedAreas[1];
        inWorld.rules[116] = (world) => (Rule.PlacedItem(world, new ItemType("RUN")) || world.options[3] == 1) && SpecialLogic.savedAreas[1];
        inWorld.rules[117] = (world) => SpecialLogic.savedAreas[3];
        inWorld.rules[118] = (world) => SpecialLogic.savedAreas[3];
        inWorld.rules[119] = (world) => SpecialLogic.savedAreas[3];
        inWorld.rules[120] = (world) => SpecialLogic.savedAreas[4];
        inWorld.rules[121] = (world) => SpecialLogic.savedAreas[4];
        inWorld.rules[122] = (world) => SpecialLogic.savedAreas[4];
        inWorld.rules[126] = (world) => SpecialLogic.savedAreas[1];
        inWorld.rules[127] = (world) => (Rule.PlacedItem(world, new ItemType("RUN")) || world.options[3] == 1) && SpecialLogic.savedAreas[1];
        inWorld.rules[128] = (world) => (Rule.PlacedItem(world, new ItemType("RUN")) || world.options[3] == 1) && SpecialLogic.savedAreas[1];
        inWorld.rules[129] = (world) => SpecialLogic.savedAreas[2];
        inWorld.rules[130] = (world) => SpecialLogic.savedAreas[2];
        inWorld.rules[131] = (world) => SpecialLogic.savedAreas[2];
        inWorld.rules[132] = (world) => SpecialLogic.savedAreas[3];
        inWorld.rules[133] = (world) => SpecialLogic.savedAreas[3];
        inWorld.rules[134] = (world) => SpecialLogic.savedAreas[3];
        inWorld.rules[135] = (world) => SpecialLogic.savedAreas[4];
        inWorld.rules[136] = (world) => SpecialLogic.savedAreas[4];
        inWorld.rules[137] = (world) => SpecialLogic.savedAreas[4];
        inWorld.rules[138] = (world) => (SpecialLogic.ActAvailability(world) || world.options[3] == 1) && SpecialLogic.savedAreas[4];
        inWorld.rules[139] = (world) => SpecialLogic.savedAreas[4];
        inWorld.rules[140] = (world) => SpecialLogic.savedAreas[1];
    }
}
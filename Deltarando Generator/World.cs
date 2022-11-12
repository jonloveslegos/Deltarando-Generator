using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public delegate bool Del(World world);
public struct ItemType
{
    public int id;
    public string category;
    public ItemType(ItemType itemType)
    {
        id = itemType.id;
        category = itemType.category;
    }
    public ItemType(int itemId, string typeOfItem)
    {
        id = itemId;
        category = typeOfItem;
    }
}

public static class Rule
{
    public static bool PlacedItem(World world, ItemType itemId)
    {
        if (world.locations.Exists(x => x.id == itemId.id && x.category == itemId.category))
        {
            return true;
        }
        if (world.starting.Exists(x => x.id == itemId.id && x.category == itemId.category))
        {
            return true;
        }
        return world.locations.Exists(x => x.id == itemId.id && x.category == itemId.category);
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
            locations.Add(new ItemType(-999, ""));
        }
        for (int i = 110; i <= 140; i++)
        {
            locations[i] = new ItemType(-1, "");
        }
        for (int i = 20; i <= 103; i++)
        {
            locations[i] = new ItemType(-1, "");
        }
        for (int i = 5; i <= 18; i++)
        {
            locations[i] = new ItemType(-1, "");
        }
        for (int i = 0; i <= 3; i++)
        {
            locations[i] = new ItemType(-1,"");
        }
        for (int i = 0; i <= 300; i++)
        {
            wantedLocs.Add(new ItemType(-999, ""));
        }
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
        while (locations.Exists(x => x.id == -1 && x.category == ""))
        {
            List<int> emptyLocs = new List<int>();
            for (int i = 0; i < locations.Count; i++)
            {
                if (locations[i].id == -1 && locations[i].category == "")
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
                            if (!(lastItems[lastItems.Count - 1].id == -1 && lastItems[lastItems.Count - 1].category == ""))
                            {
                                itemsRemoved.Add(new ItemType(lastItems[lastItems.Count - 1]));
                            }
                            lastLocations.RemoveAt(lastLocations.Count - 1);
                            lastItems.RemoveAt(lastItems.Count - 1);
                        }
                    }
                }
                locations = new List<ItemType>(lastLocations[lastLocations.Count - 1]);
                if (!(lastItems[lastItems.Count - 1].id == -1 && lastItems[lastItems.Count - 1].category == ""))
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
                if (wantedLocs[chosen].id != -999)
                {
                    lastItems.Add(new ItemType(-1, ""));
                    PlaceItem(chosen, wantedLocs[chosen]);
                    toChooseFrom.Remove(chosen);
                }
                else
                {
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
            for (int i = 3; i < 15; i++)
            {
                items.Add(new ItemType(i, "key"));
            }
            if (options[25])
            {
                for (int i = 0; i < 10; i++)
                {
                    items.Add(new ItemType(16, "key"));
                }
                items.Remove(new ItemType(12, "key"));
            }
        }
        else
        {
            PlaceForcedItem(34, new ItemType(3, "key"));
            PlaceForcedItem(40, new ItemType(4, "key"));
            PlaceForcedItem(33, new ItemType(5, "key"));
            PlaceForcedItem(15, new ItemType(6, "key"));
            PlaceForcedItem(13, new ItemType(7, "key"));
            PlaceForcedItem(36, new ItemType(8, "key"));
            PlaceForcedItem(37, new ItemType(9, "key"));
            PlaceForcedItem(38, new ItemType(10, "key"));
            PlaceForcedItem(39, new ItemType(11, "key"));
            PlaceForcedItem(98, new ItemType(12, "key"));
            PlaceForcedItem(102, new ItemType(13, "key"));
            PlaceForcedItem(103, new ItemType(14, "key"));
        }
        if (options[7])
        {
            if (options[24])
            {
                for (int i = 0; i < 4; i++)
                {
                    itemsJunk.Add(new ItemType(29, "item"));
                    itemsJunk.Add(new ItemType(16, "item"));
                    itemsJunk.Add(new ItemType(22, "item"));
                    itemsJunk.Add(new ItemType(25, "item"));
                }
                itemsJunk.Add(new ItemType(23, "item"));
                itemsJunk.Add(new ItemType(27, "item"));
                itemsJunk.Add(new ItemType(28, "item"));
            }
            for (int i = 0; i < 4; i++)
            {
                itemsJunk.Add(new ItemType(8, "item"));
                itemsJunk.Add(new ItemType(12, "item"));
                itemsJunk.Add(new ItemType(13, "item"));
                itemsJunk.Add(new ItemType(15, "item"));
            }
            itemsJunk.Add(new ItemType(3, "item"));
            items.Add(new ItemType(6, "item"));
            itemsJunk.Add(new ItemType(9, "item"));
            for (int i = 0; i < 3; i++)
            {
                itemsJunk.Add(new ItemType(7, "item"));
                itemsJunk.Add(new ItemType(11, "item"));
            }
            for (int i = 0; i < 8; i++)
            {
                itemsJunk.Add(new ItemType(1, "item"));
            }
            for (int i = 0; i < 2; i++)
            {
                itemsJunk.Add(new ItemType(2, "item"));
                items.Add(new ItemType(4, "item"));
            }
        }
        else
        {
            PlaceForcedItem(0, new ItemType(1, "item"));
            PlaceForcedItem(1, new ItemType(1, "item"));
            PlaceForcedItem(2, new ItemType(1, "item"));
            PlaceForcedItem(3, new ItemType(1, "item"));
            PlaceForcedItem(5, new ItemType(6, "item"));
            PlaceForcedItem(6, new ItemType(13, "item"));
            PlaceForcedItem(7, new ItemType(12, "item"));
            PlaceForcedItem(8, new ItemType(9, "item"));
            PlaceForcedItem(9, new ItemType(7, "item"));
            PlaceForcedItem(10, new ItemType(3, "item"));
            PlaceForcedItem(11, new ItemType(4, "item"));
            PlaceForcedItem(12, new ItemType(4, "item"));
            PlaceForcedItem(14, new ItemType(2, "item"));
            PlaceForcedItem(21, new ItemType(11, "item"));
            PlaceForcedItem(22, new ItemType(2, "item"));
            PlaceForcedItem(25, new ItemType(1, "item"));
            PlaceForcedItem(26, new ItemType(8, "item"));
            PlaceForcedItem(29, new ItemType(15, "item"));
        }
        if (options[4])
        {
            for (int i = 0; i < 19; i++)
            {
                itemsJunk.Add(new ItemType(1, "krislvl"));
                itemsJunk.Add(new ItemType(1, "susielvl"));
                itemsJunk.Add(new ItemType(1, "ralseilvl"));
            }
        }
        else
        {
            for (int i = 0; i < 19; i++)
            {
                PlaceForcedItem(41+i,new ItemType(1, "krislvl"));
                PlaceForcedItem(60+i,new ItemType(1, "susielvl"));
                PlaceForcedItem(79+i,new ItemType(1, "ralseilvl"));
            }
        }
        if (options[13])
        {
            items.Add(new ItemType(2, "party"));
            items.Add(new ItemType(3, "party"));
            items.Add(new ItemType(2, "cancontrol"));
        }
        else
        {
            PlaceForcedItem(111, new ItemType(3, "party"));
            PlaceForcedItem(110, new ItemType(2, "party"));
            PlaceForcedItem(112, new ItemType(2, "cancontrol"));
        }
        if (options[11])
        {
            items.Add(new ItemType(1, "ability"));
            items.Add(new ItemType(2, "ability"));
            items.Add(new ItemType(3, "ability"));
            items.Add(new ItemType(4, "ability"));
            items.Add(new ItemType(5, "ability"));
        }
        else
        {
            starting.Add(new ItemType(1, "ability"));
            starting.Add(new ItemType(2, "ability"));
            starting.Add(new ItemType(3, "ability"));
            starting.Add(new ItemType(4, "ability"));
            starting.Add(new ItemType(5, "ability"));
        }
        if (options[23])
        {
            items.Add(new ItemType(13, "ability"));
        }
        else
        {
            starting.Add(new ItemType(13, "ability"));
        }
        if (options[22])
        {
            items.Add(new ItemType(12, "ability"));
        }
        else
        {
            starting.Add(new ItemType(12, "ability"));
        }
        if (options[21])
        {
            items.Add(new ItemType(0, "ability"));
        }
        else
        {
            starting.Add(new ItemType(0, "ability"));
        }
        if (options[17])
        {
            items.Add(new ItemType(6, "ability"));
            items.Add(new ItemType(7, "ability"));
            items.Add(new ItemType(8, "ability"));
            items.Add(new ItemType(9, "ability"));
        }
        else
        {
            starting.Add(new ItemType(6, "ability"));
            starting.Add(new ItemType(7, "ability"));
            starting.Add(new ItemType(8, "ability"));
            starting.Add(new ItemType(9, "ability"));
        }
        if (options[17])
        {
            items.Add(new ItemType(10, "ability"));
        }
        else
        {
            starting.Add(new ItemType(10, "ability"));
        }
        if (options[17])
        {
            items.Add(new ItemType(11, "ability"));
        }
        else
        {
            starting.Add(new ItemType(11, "ability"));
        }
        for (int i = 123; i <= 125; i++)
        {
            PlaceForcedItem(i, new ItemType(0, "save"));
        }
        if (options[20])
        {
            for (int i = 0; i < 14; i++)
            {
                itemsJunk.Add(new ItemType(0, "save"));
            }
        }
        else
        {

            for (int i = 126; i <= 139; i++)
            {
                PlaceForcedItem(i, new ItemType(0, "save"));
            }
        }
        if (options[20])
        {
            items.Add(new ItemType(7, "krisspell"));
            items.Add(new ItemType(7, "susiespell"));
            items.Add(new ItemType(7, "ralseispell"));
            itemsJunk.Add(new ItemType(2, "ralseispell"));
            itemsJunk.Add(new ItemType(3, "ralseispell"));
            itemsJunk.Add(new ItemType(4, "susiespell"));
            if (options[24])
            {
                itemsJunk.Add(new ItemType(9, "susiespell"));
                itemsJunk.Add(new ItemType(10, "krisspell"));
                itemsJunk.Add(new ItemType(11, "krisspell"));
                itemsJunk.Add(new ItemType(12, "krisspell"));
                itemsJunk.Add(new ItemType(13, "ralseispell"));
                itemsJunk.Add(new ItemType(14, "ralseispell"));
                itemsJunk.Add(new ItemType(15, "susiespell"));
                itemsJunk.Add(new ItemType(16, "susiespell"));
                itemsJunk.Add(new ItemType(17, "ralseispell"));
                itemsJunk.Add(new ItemType(18, "susiespell"));
                itemsJunk.Add(new ItemType(19, "ralseispell"));
            }
        }
        else
        {
            starting.Add(new ItemType(7, "krisspell"));
            starting.Add(new ItemType(2, "ralseispell"));
            starting.Add(new ItemType(3, "ralseispell"));
            starting.Add(new ItemType(4, "susiespell"));
            if (options[24])
            {
                starting.Add(new ItemType(7, "susiespell"));
                starting.Add(new ItemType(7, "ralseispell"));
                starting.Add(new ItemType(9, "susiespell"));
                starting.Add(new ItemType(10, "krisspell"));
                starting.Add(new ItemType(11, "krisspell"));
                starting.Add(new ItemType(12, "krisspell"));
                starting.Add(new ItemType(13, "ralseispell"));
                starting.Add(new ItemType(14, "ralseispell"));
                starting.Add(new ItemType(15, "susiespell"));
                starting.Add(new ItemType(16, "susiespell"));
                starting.Add(new ItemType(17, "ralseispell"));
                starting.Add(new ItemType(18, "susiespell"));
                starting.Add(new ItemType(19, "ralseispell"));
            }
        }
        if (options[5])
        {
            itemsJunk.Add(new ItemType(5, "weapon"));
            itemsJunk.Add(new ItemType(6, "weapon"));
            itemsJunk.Add(new ItemType(7, "weapon"));
            itemsJunk.Add(new ItemType(9, "weapon"));
            itemsJunk.Add(new ItemType(10, "weapon"));
            if (options[24])
            {
                itemsJunk.Add(new ItemType(11, "weapon"));
                itemsJunk.Add(new ItemType(12, "weapon"));
                itemsJunk.Add(new ItemType(13, "weapon"));
                itemsJunk.Add(new ItemType(14, "weapon"));
                itemsJunk.Add(new ItemType(15, "weapon"));
                itemsJunk.Add(new ItemType(16, "weapon"));
                itemsJunk.Add(new ItemType(17, "weapon"));
                itemsJunk.Add(new ItemType(18, "weapon"));
                itemsJunk.Add(new ItemType(19, "weapon"));
                itemsJunk.Add(new ItemType(20, "weapon"));
            }
        }
        else
        {
            PlaceForcedItem(16, new ItemType(9, "weapon"));
            PlaceForcedItem(20, new ItemType(7, "weapon"));
            PlaceForcedItem(28, new ItemType(5, "weapon"));
            PlaceForcedItem(30, new ItemType(6, "weapon"));
            PlaceForcedItem(31, new ItemType(10, "weapon"));
        }
        if (options[6])
        {
            if (options[24])
            {
                for (int i = 0; i < 4; i++)
                {
                    itemsJunk.Add(new ItemType(8, "armor"));
                    itemsJunk.Add(new ItemType(9, "armor"));
                    itemsJunk.Add(new ItemType(11, "armor"));
                    itemsJunk.Add(new ItemType(21, "armor"));
                }
                for (int i = 0; i < 3; i++)
                {
                    itemsJunk.Add(new ItemType(15, "armor"));
                    itemsJunk.Add(new ItemType(16, "armor"));
                    itemsJunk.Add(new ItemType(18, "armor"));
                }
                for (int i = 0; i < 2; i++)
                {
                    itemsJunk.Add(new ItemType(19, "armor"));
                    itemsJunk.Add(new ItemType(20, "armor"));
                }
                itemsJunk.Add(new ItemType(10, "armor"));
                itemsJunk.Add(new ItemType(12, "armor"));
                itemsJunk.Add(new ItemType(13, "armor"));
                itemsJunk.Add(new ItemType(14, "armor"));
                itemsJunk.Add(new ItemType(17, "armor"));
            }
            for (int i = 0; i < 4; i++)
            {
                itemsJunk.Add(new ItemType(1, "armor"));
            }
            for (int i = 0; i < 2; i++)
            {
                itemsJunk.Add(new ItemType(2, "armor"));
            }
            itemsJunk.Add(new ItemType(3, "armor"));
            itemsJunk.Add(new ItemType(4, "armor"));
            itemsJunk.Add(new ItemType(5, "armor"));
            itemsJunk.Add(new ItemType(7, "armor"));
        }
        else
        {
            PlaceForcedItem(27, new ItemType(1, "armor"));
            PlaceForcedItem(32, new ItemType(1, "armor"));
            PlaceForcedItem(17, new ItemType(2, "armor"));
            PlaceForcedItem(23, new ItemType(4, "armor"));
            PlaceForcedItem(24, new ItemType(7, "armor"));
            PlaceForcedItem(35, new ItemType(5, "armor"));
        }
        if (options[3])
        {
            items.Add(new ItemType(0, "shortcut"));
            items.Add(new ItemType(1, "shortcut"));
            items.Add(new ItemType(2, "shortcut"));
            items.Add(new ItemType(3, "shortcut"));
        }
        else
        {
            PlaceForcedItem(140, new ItemType(0, "shortcut"));
            PlaceForcedItem(99, new ItemType(1, "shortcut"));
            PlaceForcedItem(100, new ItemType(2, "shortcut"));
            PlaceForcedItem(101, new ItemType(3, "shortcut"));
        }
        while (locations.FindAll(x => x.id == -1 && x.category == "").Count  > items.Count + itemsJunk.Count)
        {
            itemsJunk.Add(new ItemType(40, "gold"));
        }
    }
}

public static class SpecialLogic
{
    public static bool AreaAccess(World world, int areaId)
    {
        if (areaId == 1)
        {
            return SpecialLogic.AreaAccess(world, areaId - 1) && SpecialLogic.CanCompleteTutorial(world) && Rule.PlacedItem(world, new ItemType(8, "key"));
        }
        else if (areaId == 2)
        {
            return SpecialLogic.AreaAccess(world, areaId - 1) && SpecialLogic.ActAvailability(world) && Rule.PlacedItem(world, new ItemType(4, "ability")) && Rule.PlacedItem(world, new ItemType(12, "ability")) && Rule.PlacedItem(world, new ItemType(9, "key"));
        }
        else if (areaId == 3)
        {
            return SpecialLogic.AreaAccess(world, areaId - 1) && Rule.PlacedItem(world, new ItemType(10, "key"));
        }
        else if (areaId == 4)
        {
            return SpecialLogic.AreaAccess(world, areaId-1) && Rule.PlacedItem(world, new ItemType(11, "key"));
        }
        return true;
    }
    public static bool ReturnTrue(World world)
    {
        return true;
    }
    public static bool ActAvailability(World world)
    {
        return Rule.PlacedItem(world, new ItemType(2, "ability")) && ((Rule.PlacedItem(world, new ItemType(3, "party")) && Rule.PlacedItem(world, new ItemType(7, "ralseispell"))) || (Rule.PlacedItem(world, new ItemType(2, "party")) && Rule.PlacedItem(world, new ItemType(7, "susiespell"))) || Rule.PlacedItem(world, new ItemType(7, "krisspell")));
    }
    public static bool CanCompleteTutorial(World world)
    {
        return (Rule.PlacedItem(world, new ItemType(6, "ability")) || Rule.PlacedItem(world, new ItemType(7, "ability"))) && (Rule.PlacedItem(world, new ItemType(9, "ability")) || Rule.PlacedItem(world, new ItemType(8, "ability"))) && ((SpecialLogic.ActAvailability(world) && Rule.PlacedItem(world, new ItemType(4, "ability"))) || Rule.PlacedItem(world, new ItemType(1, "ability")));
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
        world.rules.Insert(11, (thisWorld) => Rule.PlacedItem(thisWorld, new ItemType(4, "item")) && Rule.PlacedItem(thisWorld, new ItemType(13, "ability")));
        world.rules.Insert(9, (thisWorld) => Rule.PlacedItem(thisWorld, new ItemType(6, "item")) && SpecialLogic.AreaAccess(thisWorld, 1));
        world.rules.Insert(113, (thisWorld) => SpecialLogic.AreaAccess(thisWorld, 1));
        world.rules.Insert(140, (thisWorld) => SpecialLogic.AreaAccess(thisWorld, 1));
        world.rules.Insert(0, (thisWorld) => Rule.PlacedItem(thisWorld, new ItemType(13, "key")) && SpecialLogic.AreaAccess(thisWorld, 1));
        world.rules.Insert(1, (thisWorld) => Rule.PlacedItem(thisWorld, new ItemType(13, "key")) && SpecialLogic.AreaAccess(thisWorld, 1));
        world.rules.Insert(23, (thisWorld) => Rule.PlacedItem(thisWorld, new ItemType(12, "ability")) && Rule.PlacedItem(thisWorld, new ItemType(13, "key")) && SpecialLogic.AreaAccess(thisWorld, 1));
        world.rules.Insert(13, (thisWorld) => Rule.PlacedItem(thisWorld, new ItemType(12, "ability")) && Rule.PlacedItem(thisWorld, new ItemType(13, "key")) && SpecialLogic.AreaAccess(thisWorld, 1));
        world.rules.Insert(25, (thisWorld) => Rule.PlacedItem(thisWorld, new ItemType(12, "ability")) && SpecialLogic.AreaAccess(thisWorld, 1));
        world.rules.Insert(26, (thisWorld) => Rule.PlacedItem(thisWorld, new ItemType(12, "ability")) && SpecialLogic.AreaAccess(thisWorld, 1));
        world.rules.Insert(27, (thisWorld) => Rule.PlacedItem(thisWorld, new ItemType(12, "ability")) && SpecialLogic.AreaAccess(thisWorld, 1));
        world.rules.Insert(28, (thisWorld) => Rule.PlacedItem(thisWorld, new ItemType(12, "ability")) && SpecialLogic.AreaAccess(thisWorld, 1));
        world.rules.Insert(2, (thisWorld) => SpecialLogic.AreaAccess(thisWorld, 1));
        world.rules.Insert(3, (thisWorld) => SpecialLogic.AreaAccess(thisWorld, 1));
        world.rules.Insert(34, (thisWorld) => SpecialLogic.AreaAccess(thisWorld, 1));
        world.rules.Insert(36, (thisWorld) => SpecialLogic.CanCompleteTutorial(thisWorld));
        world.rules.Insert(12, (thisWorld) => SpecialLogic.CanCompleteTutorial(thisWorld));
        world.rules.Insert(41, (thisWorld) => SpecialLogic.CanCompleteTutorial(thisWorld));
        world.rules.Insert(111, (thisWorld) => SpecialLogic.CanCompleteTutorial(thisWorld));
        world.rules.Insert(114, (thisWorld) => Rule.PlacedItem(thisWorld, new ItemType(12, "ability")) && SpecialLogic.AreaAccess(thisWorld, 1));
        world.rules.Insert(115, (thisWorld) => Rule.PlacedItem(thisWorld, new ItemType(12, "ability")) && SpecialLogic.AreaAccess(thisWorld, 1));
        world.rules.Insert(116, (thisWorld) => Rule.PlacedItem(thisWorld, new ItemType(12, "ability")) && SpecialLogic.AreaAccess(thisWorld, 1));
        world.rules.Insert(37, (thisWorld) => Rule.PlacedItem(thisWorld, new ItemType(12, "ability")) && SpecialLogic.AreaAccess(thisWorld, 1));
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
        world.rules.Insert(16, (thisWorld) => Rule.PlacedItem(thisWorld, new ItemType(14, "key")) && SpecialLogic.AreaAccess(thisWorld, 3));
        world.rules.Insert(15, (thisWorld) => Rule.PlacedItem(thisWorld, new ItemType(14, "key")) && SpecialLogic.AreaAccess(thisWorld, 3));
        world.rules.Insert(17, (thisWorld) => Rule.PlacedItem(thisWorld, new ItemType(14, "key")) && SpecialLogic.AreaAccess(thisWorld, 3));
        world.rules.Insert(5, (thisWorld) => Rule.PlacedItem(thisWorld, new ItemType(3, "key")) && Rule.PlacedItem(thisWorld, new ItemType(14, "key")) && SpecialLogic.AreaAccess(thisWorld, 3));
        world.rules.Insert(120, (thisWorld) => SpecialLogic.AreaAccess(thisWorld, 4));
        world.rules.Insert(121, (thisWorld) => SpecialLogic.AreaAccess(thisWorld, 4));
        world.rules.Insert(122, (thisWorld) => SpecialLogic.AreaAccess(thisWorld, 4));
        world.rules.Insert(99, (thisWorld) => SpecialLogic.AreaAccess(thisWorld, 2));
        world.rules.Insert(33, (thisWorld) => Rule.PlacedItem(thisWorld, new ItemType(4, "key")) && Rule.PlacedItem(thisWorld, new ItemType(6, "key")) && Rule.PlacedItem(thisWorld, new ItemType(7, "key")) && Rule.PlacedItem(thisWorld, new ItemType(14, "key")) && SpecialLogic.AreaAccess(thisWorld, 3));
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
        world.rules.Insert(79, (thisWorld) => SpecialLogic.UnlockedAreaCount(thisWorld, 1) && Rule.PlacedItem(thisWorld, new ItemType(3, "party")));
        world.rules.Insert(80, (thisWorld) => SpecialLogic.UnlockedAreaCount(thisWorld, 1) && Rule.PlacedItem(thisWorld, new ItemType(3, "party")));
        world.rules.Insert(81, (thisWorld) => SpecialLogic.UnlockedAreaCount(thisWorld, 1) && Rule.PlacedItem(thisWorld, new ItemType(3, "party")));
        world.rules.Insert(82, (thisWorld) => SpecialLogic.UnlockedAreaCount(thisWorld, 1) && Rule.PlacedItem(thisWorld, new ItemType(3, "party")));
        world.rules.Insert(83, (thisWorld) => SpecialLogic.UnlockedAreaCount(thisWorld, 2) && Rule.PlacedItem(thisWorld, new ItemType(3, "party")));
        world.rules.Insert(84, (thisWorld) => SpecialLogic.UnlockedAreaCount(thisWorld, 2) && Rule.PlacedItem(thisWorld, new ItemType(3, "party")));
        world.rules.Insert(85, (thisWorld) => SpecialLogic.UnlockedAreaCount(thisWorld, 2) && Rule.PlacedItem(thisWorld, new ItemType(3, "party")));
        world.rules.Insert(86, (thisWorld) => SpecialLogic.UnlockedAreaCount(thisWorld, 2) && Rule.PlacedItem(thisWorld, new ItemType(3, "party")));
        world.rules.Insert(87, (thisWorld) => SpecialLogic.UnlockedAreaCount(thisWorld, 2) && Rule.PlacedItem(thisWorld, new ItemType(3, "party")));
        world.rules.Insert(88, (thisWorld) => SpecialLogic.UnlockedAreaCount(thisWorld, 3) && Rule.PlacedItem(thisWorld, new ItemType(3, "party")));
        world.rules.Insert(89, (thisWorld) => SpecialLogic.UnlockedAreaCount(thisWorld, 3) && Rule.PlacedItem(thisWorld, new ItemType(3, "party")));
        world.rules.Insert(90, (thisWorld) => SpecialLogic.UnlockedAreaCount(thisWorld, 3) && Rule.PlacedItem(thisWorld, new ItemType(3, "party")));
        world.rules.Insert(91, (thisWorld) => SpecialLogic.UnlockedAreaCount(thisWorld, 3) && Rule.PlacedItem(thisWorld, new ItemType(3, "party")));
        world.rules.Insert(92, (thisWorld) => SpecialLogic.UnlockedAreaCount(thisWorld, 3) && Rule.PlacedItem(thisWorld, new ItemType(3, "party")));
        world.rules.Insert(60, (thisWorld) => SpecialLogic.UnlockedAreaCount(thisWorld, 1) && Rule.PlacedItem(thisWorld, new ItemType(2, "party")));
        world.rules.Insert(61, (thisWorld) => SpecialLogic.UnlockedAreaCount(thisWorld, 1) && Rule.PlacedItem(thisWorld, new ItemType(2, "party")));
        world.rules.Insert(62, (thisWorld) => SpecialLogic.UnlockedAreaCount(thisWorld, 1) && Rule.PlacedItem(thisWorld, new ItemType(2, "party")));
        world.rules.Insert(63, (thisWorld) => SpecialLogic.UnlockedAreaCount(thisWorld, 1) && Rule.PlacedItem(thisWorld, new ItemType(2, "party")));
        world.rules.Insert(64, (thisWorld) => SpecialLogic.UnlockedAreaCount(thisWorld, 2) && Rule.PlacedItem(thisWorld, new ItemType(2, "party")));
        world.rules.Insert(65, (thisWorld) => SpecialLogic.UnlockedAreaCount(thisWorld, 2) && Rule.PlacedItem(thisWorld, new ItemType(2, "party")));
        world.rules.Insert(66, (thisWorld) => SpecialLogic.UnlockedAreaCount(thisWorld, 2) && Rule.PlacedItem(thisWorld, new ItemType(2, "party")));
        world.rules.Insert(67, (thisWorld) => SpecialLogic.UnlockedAreaCount(thisWorld, 2) && Rule.PlacedItem(thisWorld, new ItemType(2, "party")));
        world.rules.Insert(68, (thisWorld) => SpecialLogic.UnlockedAreaCount(thisWorld, 2) && Rule.PlacedItem(thisWorld, new ItemType(2, "party")));
        world.rules.Insert(69, (thisWorld) => SpecialLogic.UnlockedAreaCount(thisWorld, 3) && Rule.PlacedItem(thisWorld, new ItemType(2, "party")));
        world.rules.Insert(70, (thisWorld) => SpecialLogic.UnlockedAreaCount(thisWorld, 3) && Rule.PlacedItem(thisWorld, new ItemType(2, "party")));
        world.rules.Insert(71, (thisWorld) => SpecialLogic.UnlockedAreaCount(thisWorld, 3) && Rule.PlacedItem(thisWorld, new ItemType(2, "party")));
        world.rules.Insert(72, (thisWorld) => SpecialLogic.UnlockedAreaCount(thisWorld, 3) && Rule.PlacedItem(thisWorld, new ItemType(2, "party")));
        world.rules.Insert(73, (thisWorld) => SpecialLogic.UnlockedAreaCount(thisWorld, 3) && Rule.PlacedItem(thisWorld, new ItemType(2, "party")));
        world.rules.Insert(74, (thisWorld) => SpecialLogic.UnlockedAreaCount(thisWorld, 4) && Rule.PlacedItem(thisWorld, new ItemType(2, "party")));
        world.rules.Insert(78, (thisWorld) => SpecialLogic.UnlockedAreaCount(thisWorld, 4) && Rule.PlacedItem(thisWorld, new ItemType(2, "party")));
        world.rules.Insert(75, (thisWorld) => SpecialLogic.UnlockedAreaCount(thisWorld, 4) && Rule.PlacedItem(thisWorld, new ItemType(2, "party")));
        world.rules.Insert(76, (thisWorld) => SpecialLogic.UnlockedAreaCount(thisWorld, 4) && Rule.PlacedItem(thisWorld, new ItemType(2, "party")));
        world.rules.Insert(77, (thisWorld) => SpecialLogic.UnlockedAreaCount(thisWorld, 4) && Rule.PlacedItem(thisWorld, new ItemType(2, "party")));
        world.rules.Insert(93, (thisWorld) => SpecialLogic.UnlockedAreaCount(thisWorld, 4) && Rule.PlacedItem(thisWorld, new ItemType(3, "party")));
        world.rules.Insert(94, (thisWorld) => SpecialLogic.UnlockedAreaCount(thisWorld, 4) && Rule.PlacedItem(thisWorld, new ItemType(3, "party")));
        world.rules.Insert(95, (thisWorld) => SpecialLogic.UnlockedAreaCount(thisWorld, 4) && Rule.PlacedItem(thisWorld, new ItemType(3, "party")));
        world.rules.Insert(96, (thisWorld) => SpecialLogic.UnlockedAreaCount(thisWorld, 4) && Rule.PlacedItem(thisWorld, new ItemType(3, "party")));
        world.rules.Insert(97, (thisWorld) => SpecialLogic.UnlockedAreaCount(thisWorld, 4) && Rule.PlacedItem(thisWorld, new ItemType(3, "party")));
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
        world.rules.Insert(20, (thisWorld) => Rule.PlacedItem(thisWorld, new ItemType(5, "key")) && SpecialLogic.AreaAccess(thisWorld, 4));
        world.rules.Insert(24, (thisWorld) => Rule.PlacedItem(thisWorld, new ItemType(5, "key")) && SpecialLogic.AreaAccess(thisWorld, 4));
        world.rules.Insert(138, (thisWorld) => Rule.PlacedItem(thisWorld, new ItemType(4, "ability")) && Rule.PlacedItem(thisWorld, new ItemType(2, "ability")) && SpecialLogic.ActAvailability(thisWorld) && SpecialLogic.AreaAccess(thisWorld, 4));
        world.rules.Insert(98, (thisWorld) => Rule.PlacedItem(thisWorld, new ItemType(4, "ability")) && Rule.PlacedItem(thisWorld, new ItemType(2, "ability")) && SpecialLogic.ActAvailability(thisWorld) && SpecialLogic.AreaAccess(thisWorld, 4));
        world.rules.Insert(110, (thisWorld) => Rule.PlacedItem(thisWorld, new ItemType(12, "ability")) && SpecialLogic.AreaAccess(thisWorld, 1));
        world.rules.Insert(127, (thisWorld) => Rule.PlacedItem(thisWorld, new ItemType(12, "ability")) && SpecialLogic.AreaAccess(thisWorld, 1));
        world.rules.Insert(128, (thisWorld) => Rule.PlacedItem(thisWorld, new ItemType(12, "ability")) && SpecialLogic.AreaAccess(thisWorld, 1));
    }
}
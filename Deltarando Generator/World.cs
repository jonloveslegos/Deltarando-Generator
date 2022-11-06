using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public delegate bool Del(World world);
public class ItemType
{
    public int id = 0;
    public string category = "";
    public ItemType() { }
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
    public List<ItemType> items = new List<ItemType>();
    List<ItemType> acquired = new List<ItemType>();
    public List<Predicate<World>> rules = new List<Predicate<World>>();
    public World()
    {
        for (var i = 0; i < 999; i++)
        {
            rules.Add((thisWorld) => SpecialLogic.ReturnTrue(thisWorld));
        }
    }
    public void PlaceItem(int locationId, ItemType itemType)
    {
        locations[locationId] = itemType;
    }

    public void AddItems()
    {
        for (int i = 0; i < 4; i++)
        {
            items.Add(new ItemType(29, "item"));
            items.Add(new ItemType(8, "armor"));
            items.Add(new ItemType(9, "armor"));
            items.Add(new ItemType(11, "armor"));
            items.Add(new ItemType(21, "armor"));
            items.Add(new ItemType(1, "armor"));
            items.Add(new ItemType(8, "item"));
            items.Add(new ItemType(12, "item"));
            items.Add(new ItemType(13, "item"));
            items.Add(new ItemType(15, "item"));
            items.Add(new ItemType(16, "item"));
            items.Add(new ItemType(22, "item"));
            items.Add(new ItemType(25, "item"));
        }
        for (int i = 0; i < 3; i++)
        {
            items.Add(new ItemType(15, "armor"));
            items.Add(new ItemType(16, "armor"));
            items.Add(new ItemType(18, "armor"));
            items.Add(new ItemType(7, "item"));
            items.Add(new ItemType(11, "item"));
        }
        for (int i = 0; i < 8; i++)
        {
            items.Add(new ItemType(1, "item"));
        }
        for (int i = 0; i < 2; i++)
        {
            items.Add(new ItemType(2, "armor"));
            items.Add(new ItemType(19, "armor"));
            items.Add(new ItemType(20, "armor"));
            items.Add(new ItemType(2, "item"));
            items.Add(new ItemType(4, "item"));
        }
        for (int i = 3; i < 15; i++)
        {
            items.Add(new ItemType(i, "key"));
        }
        for (int i = 0; i < 10; i++)
        {
            items.Add(new ItemType(16, "key"));
        }
        for (int i = 0; i < 14; i++)
        {
            items.Add(new ItemType(0, "save"));
        }
        for (int i = 0; i < 19; i++)
        {
            items.Add(new ItemType(1, "krislvl"));
            items.Add(new ItemType(1, "susielvl"));
            items.Add(new ItemType(1, "ralseilvl"));
        }
        items.Add(new ItemType(40, "gold"));
        items.Add(new ItemType(2, "party"));
        items.Add(new ItemType(3, "party"));
        items.Add(new ItemType(2, "cancontrol"));
        items.Add(new ItemType(1, "ability"));
        items.Add(new ItemType(2, "ability"));
        items.Add(new ItemType(3, "ability"));
        items.Add(new ItemType(4, "ability"));
        items.Add(new ItemType(5, "ability"));
        items.Add(new ItemType(13, "ability"));
        items.Add(new ItemType(12, "ability"));
        items.Add(new ItemType(0, "ability"));
        items.Add(new ItemType(6, "ability"));
        items.Add(new ItemType(7, "ability"));
        items.Add(new ItemType(8, "ability"));
        items.Add(new ItemType(9, "ability"));
        items.Add(new ItemType(10, "ability"));
        items.Add(new ItemType(11, "ability"));
        items.Add(new ItemType(7, "krisspell"));
        items.Add(new ItemType(7, "susiespell"));
        items.Add(new ItemType(7, "ralseispell"));
        items.Add(new ItemType(2, "ralseispell"));
        items.Add(new ItemType(3, "ralseispell"));
        items.Add(new ItemType(4, "susiespell"));
        items.Add(new ItemType(9, "susiespell"));
        items.Add(new ItemType(10, "krisspell"));
        items.Add(new ItemType(11, "krisspell"));
        items.Add(new ItemType(12, "krisspell"));
        items.Add(new ItemType(13, "ralseispell"));
        items.Add(new ItemType(14, "ralseispell"));
        items.Add(new ItemType(15, "susiespell"));
        items.Add(new ItemType(16, "susiespell"));
        items.Add(new ItemType(17, "ralseispell"));
        items.Add(new ItemType(18, "susiespell"));
        items.Add(new ItemType(19, "ralseispell"));
        items.Add(new ItemType(3, "item"));
        items.Add(new ItemType(6, "item"));
        items.Add(new ItemType(9, "item"));
        items.Add(new ItemType(23, "item"));
        items.Add(new ItemType(27, "item"));
        items.Add(new ItemType(28, "item"));
        items.Add(new ItemType(5, "weapon"));
        items.Add(new ItemType(6, "weapon"));
        items.Add(new ItemType(7, "weapon"));
        items.Add(new ItemType(9, "weapon"));
        items.Add(new ItemType(10, "weapon"));
        items.Add(new ItemType(11, "weapon"));
        items.Add(new ItemType(12, "weapon"));
        items.Add(new ItemType(13, "weapon"));
        items.Add(new ItemType(14, "weapon"));
        items.Add(new ItemType(15, "weapon"));
        items.Add(new ItemType(16, "weapon"));
        items.Add(new ItemType(17, "weapon"));
        items.Add(new ItemType(18, "weapon"));
        items.Add(new ItemType(19, "weapon"));
        items.Add(new ItemType(20, "weapon"));
        items.Add(new ItemType(3, "armor"));
        items.Add(new ItemType(4, "armor"));
        items.Add(new ItemType(5, "armor"));
        items.Add(new ItemType(7, "armor"));
        items.Add(new ItemType(10, "armor"));
        items.Add(new ItemType(12, "armor"));
        items.Add(new ItemType(13, "armor"));
        items.Add(new ItemType(14, "armor"));
        items.Add(new ItemType(17, "armor"));
        items.Add(new ItemType(0, "shortcut"));
        items.Add(new ItemType(1, "shortcut"));
        items.Add(new ItemType(2, "shortcut"));
        items.Add(new ItemType(3, "shortcut"));
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
        world.rules.Insert(41, (thisWorld) => Rule.PlacedItem(thisWorld, new ItemType(9, "ability")) && Rule.PlacedItem(thisWorld, new ItemType(8, "ability")));
        world.rules.Insert(111, (thisWorld) => Rule.PlacedItem(thisWorld, new ItemType(9, "ability")) && Rule.PlacedItem(thisWorld, new ItemType(8, "ability")));
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
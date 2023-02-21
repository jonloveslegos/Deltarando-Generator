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
public struct RoomType
{
    public string name;
    public bool hasEnemies;
    public List<string> connections;
    public List<Predicate<World>> connectionRules;
    public RoomType(RoomType itemType)
    {
        name = itemType.name;
        connectionRules = itemType.connectionRules;
        connections = itemType.connections;
        hasEnemies = itemType.hasEnemies;
    }
    public RoomType(string itemId, int connectionCount, List<string> defaultConnections, List<Predicate<World>> defaultRules)
    {
        name = itemId;
        connectionRules = defaultRules;
        connections = defaultConnections;
        hasEnemies = false;
    }
    public RoomType(string itemId, string MainConnection, Predicate<World> defaultRules)
    {
        name = itemId;
        connectionRules = new List<Predicate<World>>();
        connectionRules.Add(defaultRules);
        connections = new List<string>();
        connections.Add(MainConnection);
        hasEnemies = false;
    }
    public RoomType(string itemId, int connectionCount, List<string> defaultConnections)
    {
        name = itemId;
        connectionRules = new List<Predicate<World>>();
        connections = defaultConnections;
        for (int i = 0; i < connectionCount; i++)
        {
            connectionRules.Add((world) => SpecialLogic.ReturnTrue(world));
        }
        hasEnemies = false;
    }
    public RoomType(string itemId, string MainConnection)
    {
        name = itemId;
        connectionRules = new List<Predicate<World>>();
        connectionRules.Add((world) => SpecialLogic.ReturnTrue(world));
        connections = new List<string>();
        connections.Add(MainConnection);
        hasEnemies = false;
    }
    public RoomType(string itemId, bool hasEnemy, int connectionCount, List<string> defaultConnections, List<Predicate<World>> defaultRules)
    {
        name = itemId;
        connectionRules = defaultRules;
        connections = defaultConnections;
        hasEnemies = hasEnemy;
    }
    public RoomType(string itemId, bool hasEnemy, string MainConnection, Predicate<World> defaultRules)
    {
        name = itemId;
        connectionRules = new List<Predicate<World>>();
        connectionRules.Add(defaultRules);
        connections = new List<string>();
        connections.Add(MainConnection);
        hasEnemies = hasEnemy;
    }
    public RoomType(string itemId, bool hasEnemy, int connectionCount, List<string> defaultConnections)
    {
        name = itemId;
        connectionRules = new List<Predicate<World>>();
        connections = defaultConnections;
        for (int i = 0; i < connectionCount; i++)
        {
            connectionRules.Add((world) => SpecialLogic.ReturnTrue(world));
        }
        hasEnemies = hasEnemy;
    }
    public RoomType(string itemId, bool hasEnemy, string MainConnection)
    {
        name = itemId;
        connectionRules = new List<Predicate<World>>();
        connectionRules.Add((world) => SpecialLogic.ReturnTrue(world));
        connections = new List<string>();
        connections.Add(MainConnection);
        hasEnemies = hasEnemy;
    }
}

public static class Rule
{
    public static bool PlacedItem(World world, ItemType itemId)
    {
        if (Deltarando_Generator.Program.globalLocations.Exists(x => x.name == world.myId + itemId.name))
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
    public List<RoomType> rooms = new List<RoomType>();
    public List<int> enemyRooms = new List<int>();
    public List<ItemType> itemsJunk = new List<ItemType>();
    public List<ItemType> starting = new List<ItemType>();
    public List<ItemType> trueStarting = new List<ItemType>();
    public List<string> weaponKNames = new List<string>();
    public List<string> weaponRNames = new List<string>();
    public List<string> weaponSNames = new List<string>();
    public List<string> armorKNames = new List<string>();
    public List<string> armorRNames = new List<string>();
    public List<string> armorSNames = new List<string>();
    public List<int> enemyIds = new List<int>();
    public List<Predicate<World>> rules = new List<Predicate<World>>();
    public List<int> options = new List<int>();
    public List<int> locsOrder = new List<int>();
    public List<ItemType> itemsRemoved = new List<ItemType>();
    public string myName = "DEFAULT";
    public string myFile = "options.txt";
    public string myId = "000";
    List<List<ItemType>> lastLocations = new List<List<ItemType>>();
    List<List<int>> lastOrder = new List<List<int>>();
    List<ItemType> lastItems = new List<ItemType>();
    int backCount = 0;
    List<int> emptyLocs = new List<int>();
    int startEmptyCount = 0;
    public int placedItem = -1;
    public int placedJunkItem = -1;
    public int timeSinceBack = 0;
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
        for (int i = 0; i < 9; i++)
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
        enemyIds.Add(5);
        enemyIds.Add(11);
        enemyIds.Add(16);
        enemyIds.Add(19);
        enemyIds.Add(18);
        enemyIds.Add(23);
        enemyIds.Add(22);
        enemyIds.Add(15);
        enemyIds.Add(14);
        enemyIds.Add(13);
        enemyIds.Add(6);
    }
    public void SetOptions()
    {
        var file = File.OpenText(Directory.GetCurrentDirectory() + "/Players/" + myFile);
        while (!file.EndOfStream)
        {
            var itm = file.ReadLine();
            if (itm.Length > 3)
            {
                if (int.Parse(itm.Split('(', ')')[1]) == -1)
                {
                    myName = itm.Split('=')[1];
                }
                else if (itm.Split('=')[1].ToLower() != "true" && itm.Split('=')[1].ToLower() != "false")
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
    public void RandomizeStart()
    {

        lastLocations = new List<List<ItemType>>();
        lastOrder = new List<List<int>>();
        lastItems = new List<ItemType>();
        itemsRemoved = new List<ItemType>();
        backCount = 0;
        timeSinceBack = 0;
        if (options[1] > 0)
        {
            for (int i = 0; i < options[1]; i++)
            {
                var chosenItem = 0;
                if (items.Count <= 0)
                {
                    chosenItem = Deltarando_Generator.Program.rng.Next(itemsJunk.Count);
                    trueStarting.Add(itemsJunk[chosenItem]);
                    itemsJunk.RemoveAt(chosenItem);
                }
                else
                {
                    chosenItem = Deltarando_Generator.Program.rng.Next(items.Count);
                    trueStarting.Add(items[chosenItem]);
                    items.RemoveAt(chosenItem);
                }
            }
        }
        emptyLocs = new List<int>();
        startEmptyCount = 0;
        for (int i = 0; i < locations.Count; i++)
        {
            if (locations[i].name == "Null")
            {
                emptyLocs.Add(i);
            }
        }
        startEmptyCount = emptyLocs.Count - wantedLocs.FindAll(x => x.name != "").Count;
    }
    public bool Randomize()
    {
            emptyLocs = new List<int>();
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
            bool foundWanted = false;
            foreach (var item in toChooseFrom)
            {
                if (wantedLocs[item].name != "")
                {
                    PlaceItem(item, new ItemType(wantedLocs[item]));
                    foundWanted = true;
                }
            }
            while (foundWanted)
            {
                emptyLocs = new List<int>();
                for (int i = 0; i < locations.Count; i++)
                {
                    if (locations[i].name == "Null")
                    {
                        emptyLocs.Add(i);
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
                foundWanted = false;
                foreach (var item in toChooseFrom)
                {
                    if (wantedLocs[item].name != "")
                    {
                        PlaceItem(item, new ItemType(wantedLocs[item]));
                        foundWanted = true;
                    }
                }
            }
        if (items.Count + itemsJunk.Count <= 0)
        {
            if (emptyLocs.Count > 0)
            {
                Console.WriteLine("Retrying...");
                return false;
            }
        }
        else if (toChooseFrom.Count <= 0)
        {
            if (emptyLocs.Count > 0)
            {
                for (int i = 0; i < Math.Min(10, backCount); i++)
                {
                    if (lastLocations.Count > 1)
                    {
                        locations = new List<ItemType>(lastLocations[lastLocations.Count - 1]);
                        locsOrder = new List<int>(lastOrder[lastOrder.Count - 1]);
                        if (!(lastItems[lastItems.Count - 1].name == "Null"))
                        {
                            itemsRemoved.Add(new ItemType(lastItems[lastItems.Count - 1]));
                        }
                        lastLocations.RemoveAt(lastLocations.Count - 1);
                        lastOrder.RemoveAt(lastOrder.Count - 1);
                        lastItems.RemoveAt(lastItems.Count - 1);
                    }
                }
                locations = new List<ItemType>(lastLocations[lastLocations.Count - 1]);
                locsOrder = new List<int>(lastOrder[lastOrder.Count - 1]);
                if (!(lastItems[lastItems.Count - 1].name == "Null"))
                {
                    itemsRemoved.Add(new ItemType(lastItems[lastItems.Count - 1]));
                }
                lastLocations.RemoveAt(lastLocations.Count - 1);
                lastOrder.RemoveAt(lastOrder.Count - 1);
                lastItems.RemoveAt(lastItems.Count - 1);
                backCount++;
                timeSinceBack = 0;
                Console.WriteLine("Undid placement");
            }
        }
        else
        {
            int chosen = toChooseFrom[Deltarando_Generator.Program.rng.Next(toChooseFrom.Count)];
            var chosenItem = -1;
            lastLocations.Add(new List<ItemType>(locations));
            lastOrder.Add(new List<int>(locsOrder));
            if ((items.Count <= 0 || toChooseFrom.Count > items.Count * 2) && itemsJunk.Count > 0)
            {
                if (itemsJunk.Exists(x => x.name == "Kingly Key Piece"))
                {
                    chosenItem = itemsJunk.FindIndex(x => x.name == "Kingly Key Piece");
                    lastItems.Add(new ItemType(itemsJunk[chosenItem]));
                    PlaceItem(chosen, itemsJunk[chosenItem]);
                    placedJunkItem = chosenItem;
                    itemsJunk.RemoveAt(chosenItem);
                }
                else if (itemsJunk.Exists(x => x.name == "Kingly Key"))
                {
                    chosenItem = itemsJunk.FindIndex(x => x.name == "Kingly Key");
                    lastItems.Add(new ItemType(itemsJunk[chosenItem]));
                    PlaceItem(chosen, itemsJunk[chosenItem]);
                    placedJunkItem = chosenItem;
                    itemsJunk.RemoveAt(chosenItem);
                }
                else
                {
                    chosenItem = Deltarando_Generator.Program.rng.Next(itemsJunk.Count);
                    lastItems.Add(new ItemType(itemsJunk[chosenItem]));
                    PlaceItem(chosen, itemsJunk[chosenItem]);
                    placedJunkItem = chosenItem;
                    itemsJunk.RemoveAt(chosenItem);
                }
            }
            else
            {
                chosenItem = Deltarando_Generator.Program.rng.Next(items.Count);
                lastItems.Add(new ItemType(items[chosenItem]));
                PlaceItem(chosen, items[chosenItem]);
                placedItem = chosenItem;
                items.RemoveAt(chosenItem);
            }
            locsOrder.Add(chosen);
            if (timeSinceBack >= 7)
            {
                foreach (var item in itemsRemoved)
                {
                    items.Add(new ItemType(item));
                }
                itemsRemoved.Clear();
                backCount = 0;
            }
                Console.WriteLine("Placed Item.");// + "\n" + (items.Count + itemsJunk.Count + itemsRemoved.Count).ToString() + " items left.");
        }
        return true;
    }

    public bool ShuffleRooms()
    {
        List<string> locsAvail = new List<string>();
        List<int> roomsReached = new List<int>();
        roomsReached.Add(0);
        foreach (var item in rooms)
        {
            if (item.name != "")
            {
                for (int i = 0; i < item.connections.Count; i++)
                {
                    if (item.connections[i][item.connections[i].Length - 1].ToString() != "y")
                    {
                        locsAvail.Add(item.connections[i]);
                        var tempTP = item.connections[i][item.connections[i].Length - 1].ToString();
                        if (tempTP == "a") { item.connections[i] = "b"; }
                        else if (tempTP == "b") { item.connections[i] = "a"; }
                        else if (tempTP == "c") { item.connections[i] = "d"; }
                        else if (tempTP == "d") { item.connections[i] = "c"; }
                        else if (tempTP == "e") { item.connections[i] = "f"; }
                        else if (tempTP == "f") { item.connections[i] = "e"; }
                        else if (tempTP == "x") { item.connections[i] = "x"; }
                        else if (tempTP == "w") { item.connections[i] = "w"; }
                    }
                }
            }
        }
        var tempTempRoomList = new List<List<RoomType>>();
        tempTempRoomList.Add(rooms);
        var tempTempReachedList = new List<List<int>>();
        tempTempReachedList.Add(roomsReached);
        var tempTempLocsList = new List<List<string>>();
        tempTempLocsList.Add(locsAvail);
        while (locsAvail.Count > 0)
        {
            List<int> tempRooms = new List<int>();
            int newPaths = 0;
            List<string> tempLocs = new List<string>(locsAvail);
            foreach (var item in roomsReached)
            {
                for (int i = 0; i < rooms[item].connections.Count; i++)
                {
                    if (rooms[item].connections[i].Length == 1)
                    {
                        var chosen = Deltarando_Generator.Program.rng.Next(0, tempLocs.Count);
                        tempRooms.Add(rooms.FindIndex(x => x.name == tempLocs[chosen].Remove(tempLocs[chosen].Length - 1)));
                        var loc = tempLocs[chosen];
                        var lookingInd = tempLocs[chosen][tempLocs[chosen].Length - 1].ToString();
                        var tempTP = lookingInd;
                        if (tempTP == "a") { tempTP = "b"; }
                        else if (tempTP == "b") { tempTP = "a"; }
                        else if (tempTP == "c") { tempTP = "d"; }
                        else if (tempTP == "d") { tempTP = "c"; }
                        else if (tempTP == "e") { tempTP = "f"; }
                        else if (tempTP == "f") { tempTP = "e"; }
                        else if (tempTP == "x") { tempTP = "x"; }
                        else if (tempTP == "w") { tempTP = "w"; }
                        var connectionId = rooms[tempRooms[tempRooms.Count - 1]].connections.FindIndex(x => x == lookingInd);
                        var placeName = rooms[tempRooms[tempRooms.Count - 1]].name;
                        rooms[tempRooms[tempRooms.Count - 1]].connections[connectionId] = rooms[item].name + rooms[item].connections[i];
                        tempLocs.Remove(rooms[item].name + rooms[item].connections[i]);
                        rooms[item].connections[i] = loc;
                        tempLocs.Remove(loc);
                        for (int e = 0; e < rooms[tempRooms[tempRooms.Count - 1]].connections.Count; e++)
                        {
                            if (rooms[tempRooms[tempRooms.Count - 1]].connections[e].Length == 1)
                            {
                                newPaths++;
                            }
                        }
                    }
                }
            }
            if (newPaths > 0 || locsAvail.Count <= 0)
            {
                roomsReached = new List<int>(tempRooms);
                locsAvail = new List<string>(tempLocs);
                tempTempRoomList.Add(rooms);
                tempTempReachedList.Add(roomsReached);
                tempTempLocsList.Add(locsAvail);
            }
            else
            {
                return false;
            }
        }
        return true;
    }
    public void ShuffleStart()
    {
        var logic = new Logic();
        logic.SetLogic(this);
        AddItems();
        AddRooms();
    }
    public void AddRooms()
    {
        rooms.Add(new RoomType("room_castle_darkdoor", 1, new List<string>() { "room_field_starta" }, new List<Predicate<World>>() { world => Rule.PlacedItem(world, new ItemType("Field Key")) && SpecialLogic.CanCompleteTutorial(world) }));
        rooms.Add(new RoomType("room_field_start", 2, new List<string>() { "room_field_foresta", "room_castle_darkdoorb" }));
        rooms.Add(new RoomType("room_field_forest", 2, new List<string>() { "room_field1a", "room_field_startb" }));
        rooms.Add(new RoomType("room_field1",true,5, new List<string>() { "room_field2a", "room_field_forestb" , "room_field_checkers4y", "room_forest_area0y", "room_cc_1fy" }, new List<Predicate<World>>() {world => Rule.ReturnTrue(world) ,world => Rule.ReturnTrue(world), world => Rule.PlacedItem(world, new ItemType("FieldShortcutDoor")), world => Rule.PlacedItem(world, new ItemType("FieldShortcutDoor")), world => Rule.PlacedItem(world, new ItemType("FieldShortcutDoor")) }));
        rooms.Add(new RoomType("room_field2",3,new List<string>() { "room_field2Aa", "room_field1b", "room_field_topchefc" }, new List<Predicate<World>>() {world => Rule.PlacedItem(world, new ItemType("Field Secret Key")), world => SpecialLogic.ReturnTrue(world), world => SpecialLogic.ReturnTrue(world)}));
        rooms.Add(new RoomType("room_field2A","room_field2b"));
        rooms.Add(new RoomType("room_field_topchef",2, new List<string>() { "room_field_puzzle1a", "room_field2d" }));
        rooms.Add(new RoomType("room_field_puzzle1", true, 2, new List<string>() { "room_field_mazea", "room_field_topchefb" }));
        rooms.Add(new RoomType("room_field_maze",2, new List<string>() { "room_field_puzzle2a", "room_field_puzzle1b" }));
        rooms.Add(new RoomType("room_field_puzzle2",2, new List<string>() { "room_field_getsusiea", "room_field_mazeb" }));
        rooms.Add(new RoomType("room_field_getsusie",2, new List<string>() { "room_field_shop1a", "room_field_puzzle2b" }));
        rooms.Add(new RoomType("room_field_shop1",4, new List<string>() { "room_field_puzzletutoriala", "room_field_getsusieb", "room_field3c", "room_shop1x" }));
        rooms.Add(new RoomType("room_field_puzzletutorial", "room_field_shop1b"));
        rooms.Add(new RoomType("room_shop1", "room_field_shop1x"));
        rooms.Add(new RoomType("room_field3",2, new List<string>() { "room_field_boxpuzzlea", "room_field_shop1d" }));
        rooms.Add(new RoomType("room_field_boxpuzzle",2, new List<string>() { "room_field4a", "room_field3b" }));
        rooms.Add(new RoomType("room_field4", true, 3, new List<string>() { "room_field_secret1a", "room_field_boxpuzzleb", "room_field_checkers4c" }, new List<Predicate<World>>() { world => SpecialLogic.ReturnTrue(world), world => SpecialLogic.ReturnTrue(world), world => Rule.PlacedItem(world, new ItemType("Board Key")) }));
        rooms.Add(new RoomType("room_field_secret1", true, "room_field4b"));
        rooms.Add(new RoomType("room_field_checkers4",5, new List<string>() { "room_field_checkers2a", "room_field4d", "room_field1y", "room_forest_area0y", "room_cc_1fy" }, new List<Predicate<World>>() { world => Rule.ReturnTrue(world), world => Rule.ReturnTrue(world), world => Rule.PlacedItem(world, new ItemType("BoardShortcutDoor")), world => Rule.PlacedItem(world, new ItemType("BoardShortcutDoor")), world => Rule.PlacedItem(world, new ItemType("BoardShortcutDoor")) }));
        rooms.Add(new RoomType("room_field_checkers2", 2, new List<string>() { "room_field_checkers6a", "room_field_checkers4b" }));
        rooms.Add(new RoomType("room_field_checkers6", 2, new List<string>() { "room_field_checkers3a", "room_field_checkers2b" }));
        rooms.Add(new RoomType("room_field_checkers3", 2, new List<string>() { "room_field_checkers1a", "room_field_checkers6b" }));
        rooms.Add(new RoomType("room_field_checkers1", 2, new List<string>() { "room_field_checkers5a", "room_field_checkers3b" }));
        rooms.Add(new RoomType("room_field_checkers5", 2, new List<string>() { "room_field_checkers7a", "room_field_checkers1b" }));
        rooms.Add(new RoomType("room_field_checkers7", 2, new List<string>() { "room_field_checkersbossa", "room_field_checkers5b" }));
        rooms.Add(new RoomType("room_field_checkersboss", 2, new List<string>() { "room_forest_savepoint1a", "room_field_checkers7b" }, new List<Predicate<World>>() { world => SpecialLogic.ActAvailability(world) && Rule.PlacedItem(world, new ItemType("SPARE")), world => SpecialLogic.ActAvailability(world) && Rule.PlacedItem(world, new ItemType("SPARE")) }));
        rooms.Add(new RoomType("room_forest_savepoint1", 2, new List<string>() { "room_forest_area0a", "room_field_checkersbossb" }, new List<Predicate<World>>() { world => Rule.PlacedItem(world, new ItemType("Forest Key")), world => SpecialLogic.ReturnTrue(world)}));
        rooms.Add(new RoomType("room_forest_area0", true, 5, new List<string>() { "room_forest_area1a", "room_forest_savepoint1b", "room_field1y", "room_field_checkers4y", "room_cc_1fy" }, new List<Predicate<World>>() { world => Rule.ReturnTrue(world), world => Rule.ReturnTrue(world), world => Rule.PlacedItem(world, new ItemType("ForestShortcutDoor")), world => Rule.PlacedItem(world, new ItemType("ForestShortcutDoor")), world => Rule.PlacedItem(world, new ItemType("ForestShortcutDoor")) }));
        rooms.Add(new RoomType("room_forest_area1", true, 2, new List<string>() { "room_forest_area2a", "room_forest_area0b" }));
        rooms.Add(new RoomType("room_forest_area2", 3, new List<string>() { "room_forest_area2Aa", "room_forest_area1b", "room_forest_puzzle1c" }, new List<Predicate<World>>() { world => Rule.PlacedItem(world, new ItemType("Forest Secret Key")), world => SpecialLogic.ReturnTrue(world), world => SpecialLogic.ReturnTrue(world) }));
        rooms.Add(new RoomType("room_forest_area2A", "room_forest_area2b"));
        rooms.Add(new RoomType("room_forest_puzzle1", 2, new List<string>() { "room_forest_beforeclovera", "room_forest_area2d" }));
        rooms.Add(new RoomType("room_forest_beforeclover", true, 3, new List<string>() { "room_forest_area3Aa", "room_forest_puzzle1b", "room_forest_area3c" }, new List<Predicate<World>>() { world => Rule.PlacedItem(world, new ItemType("Forest Secret Key")), world => SpecialLogic.ReturnTrue(world), world => SpecialLogic.ReturnTrue(world) }));
        rooms.Add(new RoomType("room_forest_area3A", "room_forest_beforecloverb"));
        rooms.Add(new RoomType("room_forest_area3",2, new List<string>() { "room_forest_savepoint2a", "room_forest_beforecloverd" }));
        rooms.Add(new RoomType("room_forest_savepoint2", 3, new List<string>() { "room_forest_smitha", "room_forest_area3b", "room_forest_area4c" }, new List<Predicate<World>>() { world => Rule.PlacedItem(world, new ItemType("Forest Secret Key")), world => SpecialLogic.ReturnTrue(world), world => SpecialLogic.ReturnTrue(world) }));
        rooms.Add(new RoomType("room_forest_smith", "room_forest_savepoint2b"));
        rooms.Add(new RoomType("room_forest_area4", true, 2, new List<string>() { "room_forest_dancers1a", "room_forest_savepoint2d" }));
        rooms.Add(new RoomType("room_forest_dancers1", 4, new List<string>() { "room_forest_secret1a", "room_forest_area4b", "room_forest_thrashmakerc", "room_forest_secret1x" }, new List<Predicate<World>>() {world => SpecialLogic.ReturnTrue(world), world => SpecialLogic.ReturnTrue(world), world => SpecialLogic.ReturnTrue(world), world => false }));
        rooms.Add(new RoomType("room_forest_secret1", true, 2, new List<string>() { "room_forest_dancers1b", "room_forest_dancers1x" }, new List<Predicate<World>>() { world => Rule.PlacedItem(world, new ItemType("Forest Secret Key")), world => SpecialLogic.ReturnTrue(world)}));
        rooms.Add(new RoomType("room_forest_thrashmaker", 2, new List<string>() { "room_forest_starwalkera", "room_forest_dancers1d" }));
        rooms.Add(new RoomType("room_forest_starwalker", 2, new List<string>() { "room_forest_area5a", "room_forest_thrashmakerb" }));
        rooms.Add(new RoomType("room_forest_area5", 2, new List<string>() { "room_forest_savepoint_relaxa", "room_forest_starwalkerb" }));
        rooms.Add(new RoomType("room_forest_savepoint_relax", 2, new List<string>() { "room_forest_area5b", "room_forest_savepoint3x" }));
        rooms.Add(new RoomType("room_forest_savepoint3", 2, new List<string>() { "room_forest_fightsusiea", "room_forest_savepoint_relaxx" }, new List<Predicate<World>>() { world => Rule.PlacedItem(world, new ItemType("Castle Key")), world => SpecialLogic.ReturnTrue(world) }));
        rooms.Add(new RoomType("room_forest_fightsusie", 2, new List<string>() { "room_forest_savepoint3b", "room_cc_entrancex" }));
        rooms.Add(new RoomType("room_cc_prison_cells", 2, new List<string>() { "room_cc_prisonlancera", "room_cc_prisonlancerx" }, new List<Predicate<World>>() { x => false, x => false }));
        rooms.Add(new RoomType("room_cc_prisonlancer", 3, new List<string>() { "room_cc_prison_to_elevatora", "room_cc_prison_cellsb", "room_cc_prison_cellsx" }));
        rooms.Add(new RoomType("room_cc_prison_to_elevator", 3, new List<string>() { "room_cc_prison2a", "room_cc_prisonlancerb", "room_cc_prisonelevatorx" }));
        rooms.Add(new RoomType("room_cc_prison2", "room_cc_prison_to_elevatorb"));
        rooms.Add(new RoomType("room_cc_prisonelevator", 3, new List<string>() { "room_cc_prison_to_elevatorx", "room_cc_1fx", "room_cc_prison_prejokerx" }));
        rooms.Add(new RoomType("room_cc_elevator", 2, new List<string>() { "room_cc_1fw", "room_cc_5fw" }));
        rooms.Add(new RoomType("room_cc_prison_prejoker", 2, new List<string>() { "room_cc_jokera", "room_cc_prisonelevatorx" }, new List<Predicate<World>>() { world => Rule.PlacedItem(world, new ItemType("Door Key")), world => SpecialLogic.ReturnTrue(world) }));
        rooms.Add(new RoomType("room_cc_joker", "room_cc_prison_prejokerb"));
        rooms.Add(new RoomType("room_cc_entrance", 2, new List<string>() { "room_cc_1fa", "room_forest_fightsusiex" }));
        rooms.Add(new RoomType("room_cc_1f", true, 8, new List<string>() { "room_cc_rudinna", "room_cc_entranceb", "room_cc_2fc", "room_cc_prisonelevatorx", "room_cc_elevatorw", "room_field1y", "room_field_checkers4y", "room_forest_area0y" }, new List<Predicate<World>>() { world => Rule.ReturnTrue(world), world => Rule.ReturnTrue(world), world => Rule.ReturnTrue(world), world => Rule.ReturnTrue(world), world => Rule.ReturnTrue(world), world => Rule.PlacedItem(world, new ItemType("CastleShortcutDoor")), world => Rule.PlacedItem(world, new ItemType("CastleShortcutDoor")), world => Rule.PlacedItem(world, new ItemType("CastleShortcutDoor")) }));
        rooms.Add(new RoomType("room_cc_rudinn", "room_cc_1fb"));
        rooms.Add(new RoomType("room_cc_2f", 3, new List<string>() { "room_cc_rurus1a", "room_cc_3fc", "room_cc_1fd" }));
        rooms.Add(new RoomType("room_cc_rurus1", "room_cc_2fb"));
        rooms.Add(new RoomType("room_cc_3f", true, 3, new List<string>() { "room_cc_hathya", "room_cc_4fc", "room_cc_2fd" }));
        rooms.Add(new RoomType("room_cc_hathy", "room_cc_3fb"));
        rooms.Add(new RoomType("room_cc_4f", 4, new List<string>() { "room_cc_rurus2a", "room_cc_cloverc", "room_cc_3fd", "room_cc_5fe" }));
        rooms.Add(new RoomType("room_cc_rurus2", "room_cc_4fb"));
        rooms.Add(new RoomType("room_cc_clover", "room_cc_4fd"));
        rooms.Add(new RoomType("room_cc_5f", 5, new List<string>() { "room_cc_lancera", "room_cc_6fc", "room_cc_4ff", "room_shop2x", "room_cc_elevatorw" }));
        rooms.Add(new RoomType("room_cc_lancer", "room_cc_5fb"));
        rooms.Add(new RoomType("room_cc_6f", 2, new List<string>() { "room_cc_thronerooma", "room_cc_5fd" }, new List<Predicate<World>>() { world => SpecialLogic.ActAvailability(world) && Rule.PlacedItem(world, new ItemType("SPARE")), world => SpecialLogic.ActAvailability(world) && Rule.PlacedItem(world, new ItemType("SPARE")) }));
        rooms.Add(new RoomType("room_cc_throneroom", 2, new List<string>() { "room_cc_preroofa", "room_cc_6fb" }));
        rooms.Add(new RoomType("room_cc_preroof", 1, new List<string>() { "room_cc_throneroomb" }));
        rooms.Add(new RoomType("room_shop2", "room_cc_5fx"));
        while (rooms.Count <= 200)
        {
            rooms.Add(new RoomType("", ""));
        }
        for (int i = 0; i < rooms.Count; i++)
        {
            if (rooms[i].hasEnemies)
            {
                enemyRooms.Add(i);
            }
        }
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
                PlaceForcedItem(41 + i, new ItemType("krislvl"));
                PlaceForcedItem(60 + i, new ItemType("susielvl"));
                PlaceForcedItem(79 + i, new ItemType("ralseilvl"));
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
            starting.Add(new ItemType("FIGHT"));
            starting.Add(new ItemType("MAGIC"));
            starting.Add(new ItemType("ITEM"));
            starting.Add(new ItemType("SPARE"));
            starting.Add(new ItemType("DEFEND"));
        }
        if (options[23] == 1)
        {
            items.Add(new ItemType("C MENU"));
        }
        else
        {
            starting.Add(new ItemType("C MENU"));
        }
        if (options[22] == 1)
        {
            items.Add(new ItemType("RUN"));
        }
        else
        {
            starting.Add(new ItemType("RUN"));
        }
        if (options[21] == 1)
        {
            items.Add(new ItemType("TP BAR"));
        }
        else
        {
            starting.Add(new ItemType("TP BAR"));
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
            starting.Add(new ItemType("LEFT SOUL"));
            starting.Add(new ItemType("RIGHT SOUL"));
            starting.Add(new ItemType("UP SOUL"));
            starting.Add(new ItemType("DOWN SOUL"));
        }
        if (options[18] == 1)
        {
            items.Add(new ItemType("SAVE HEAL"));
        }
        else
        {
            starting.Add(new ItemType("SAVE HEAL"));
        }
        if (options[19] == 1)
        {
            items.Add(new ItemType("SAVING"));
        }
        else
        {
            starting.Add(new ItemType("SAVING"));
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
            starting.Add(new ItemType("kACT"));
            starting.Add(new ItemType("rHeal Prayer"));
            starting.Add(new ItemType("rPacify"));
            starting.Add(new ItemType("sRude Buster"));
        }
        if (options[5] == 1)
        {
            itemsJunk.Add(new ItemType("Spookysword"));
            itemsJunk.Add(new ItemType("Brave Ax"));
            itemsJunk.Add(new ItemType("Ragger"));
            itemsJunk.Add(new ItemType("DaintyScarf"));
            weaponKNames.Add("Spookysword");
            weaponSNames.Add("Brave Ax");
            weaponRNames.Add("Ragger");
            weaponRNames.Add("DaintyScarf");
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
                weaponRNames.Add("FiberScarf");
                weaponKNames.Add("MechaSaber");
                weaponSNames.Add("AutoAxe");
                weaponRNames.Add("Ragger2");
                weaponKNames.Add("BounceBlade");
                weaponRNames.Add("PuppetScarf");
                weaponKNames.Add("UltiBlade");
                weaponSNames.Add("Wand-Axe");
                weaponSNames.Add("Brute Axe");
                weaponKNames.Add("Chaos Saber");
                weaponRNames.Add("Chaos Saber");
                weaponSNames.Add("Chaos Saber");
            }
        }
        else
        {
            PlaceForcedItem(16, new ItemType("Ragger"));
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
                    armorKNames.Add("Silver Card");
                    armorRNames.Add("Silver Card");
                    armorSNames.Add("Silver Card");
                    armorKNames.Add("GlowWrist");
                    armorRNames.Add("GlowWrist");
                    armorSNames.Add("GlowWrist");
                    armorKNames.Add("B.ShotBowtie");
                    armorRNames.Add("B.ShotBowtie");
                    armorSNames.Add("B.ShotBowtie");
                    armorKNames.Add("FrayedBowtie");
                    armorRNames.Add("FrayedBowtie");
                }
                for (int i = 0; i < 3; i++)
                {
                    itemsJunk.Add(new ItemType("SpikeBadge"));
                    itemsJunk.Add(new ItemType("Fluffy Shield"));
                    itemsJunk.Add(new ItemType("Silver Watch"));
                    armorKNames.Add("SpikeBadge");
                    armorRNames.Add("SpikeBadge");
                    armorSNames.Add("SpikeBadge");
                    armorKNames.Add("Fluffy Shield");
                    armorRNames.Add("Fluffy Shield");
                    armorKNames.Add("Silver Watch");
                    armorRNames.Add("Silver Watch");
                    armorSNames.Add("Silver Watch");
                }
                for (int i = 0; i < 2; i++)
                {
                    itemsJunk.Add(new ItemType("TwinRibbon"));
                    itemsJunk.Add(new ItemType("TensionBow"));
                    armorKNames.Add("TwinRibbon");
                    armorRNames.Add("TwinRibbon");
                    armorKNames.Add("TensionBow");
                    armorSNames.Add("TensionBow");
                    armorRNames.Add("TensionBow");
                }
                itemsJunk.Add(new ItemType("Royal Pin"));
                itemsJunk.Add(new ItemType("ChainMail"));
                itemsJunk.Add(new ItemType("Dealmaker"));
                itemsJunk.Add(new ItemType("SpikeBand"));
                itemsJunk.Add(new ItemType("Mannequin"));
                itemsJunk.Add(new ItemType("Pink Ribbon"));
                armorKNames.Add("Royal Pin");
                armorSNames.Add("Royal Pin");
                armorRNames.Add("Royal Pin");
                armorKNames.Add("ChainMail");
                armorSNames.Add("ChainMail");
                armorRNames.Add("ChainMail");
                armorKNames.Add("Dealmaker");
                armorSNames.Add("Dealmaker");
                armorRNames.Add("Dealmaker");
                armorKNames.Add("SpikeBand");
                armorSNames.Add("SpikeBand");
                armorRNames.Add("SpikeBand");
                armorKNames.Add("Mannequin");
                armorKNames.Add("Pink Ribbon");
                armorRNames.Add("Pink Ribbon");
            }
            for (int i = 0; i < 4; i++)
            {
                itemsJunk.Add(new ItemType("Amber Card"));
                armorKNames.Add("Amber Card");
                armorSNames.Add("Amber Card");
                armorRNames.Add("Amber Card");
            }
            for (int i = 0; i < 2; i++)
            {
                itemsJunk.Add(new ItemType("Dice Brace"));
                armorKNames.Add("Dice Brace");
                armorSNames.Add("Dice Brace");
                armorRNames.Add("Dice Brace");
            }
            itemsJunk.Add(new ItemType("White Ribbon"));
            itemsJunk.Add(new ItemType("IronShackle"));
            armorKNames.Add("White Ribbon");
            armorRNames.Add("White Ribbon");
            armorKNames.Add("IronShackle");
            armorSNames.Add("IronShackle");
            armorRNames.Add("IronShackle");
        }
        else
        {
            PlaceForcedItem(27, new ItemType("Amber Card"));
            PlaceForcedItem(32, new ItemType("Amber Card"));
            PlaceForcedItem(17, new ItemType("Dice Brace"));
            PlaceForcedItem(23, new ItemType("White Ribbon"));
            PlaceForcedItem(35, new ItemType("IronShackle"));
        }
        if (options[71] == 0)
        {
            itemsJunk.Add(new ItemType("Devilsknife"));
            itemsJunk.Add(new ItemType("Jevilstail"));
            weaponSNames.Add("Devilsknife");
            armorKNames.Add("Jevilstail");
            armorSNames.Add("Jevilstail");
            armorRNames.Add("Jevilstail");
        }
        else
        {
            PlaceForcedItem(20, new ItemType("Devilsknife"));
            PlaceForcedItem(24, new ItemType("Jevilstail"));
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
            weaponKNames.Add("Wood Blade");
            weaponRNames.Add("Red Scarf");
            weaponSNames.Add("Mane Ax");
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
                if (itm.Contains("can_kris_equip=true"))
                {
                    armorKNames.Add(thisName);
                }
                if (itm.Contains("can_ralsei_equip=true"))
                {
                    armorKNames.Add(thisName);
                }
                if (itm.Contains("can_susie_equip=true"))
                {
                    armorKNames.Add(thisName);
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
                if (itm.Contains("can_kris_equip=true"))
                {
                    weaponKNames.Add(thisName);
                }
                if (itm.Contains("can_ralsei_equip=true"))
                {
                    weaponKNames.Add(thisName);
                }
                if (itm.Contains("can_susie_equip=true"))
                {
                    weaponKNames.Add(thisName);
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
            if (!wantedLocs.Exists(x => x.name == "Susie") && !items.Exists(x => x.name == "Susie") && !itemsJunk.Exists(x => x.name == "Susie"))
            {
                locations[60 + i] = new ItemType("susielvl");
                items.RemoveAll(x => x.name == "susielvl");
                itemsJunk.RemoveAll(x => x.name == "susielvl");
            }
            if (!wantedLocs.Exists(x => x.name == "Ralsei") && !items.Exists(x => x.name == "Ralsei") && !itemsJunk.Exists(x => x.name == "Ralsei"))
            {
                locations[79 + i] = new ItemType("ralseilvl");
                items.RemoveAll(x => x.name == "ralseilvl");
                itemsJunk.RemoveAll(x => x.name == "ralseilvl");
            }
        }
        if (options[63] == 1)
        {
            int tempJunkCount = itemsJunk.Count;
            for (int i = 0; i < Math.Max(1,Math.Ceiling(tempJunkCount/4f)); i++)
            {
                itemsJunk.Add(new ItemType("Trap"));
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
        while (locations.FindAll(x => x.name == "Null").Count > items.Count + itemsJunk.Count - options[1] + wantedLocs.FindAll(x => x.name != "").Count)
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
    public static bool CanReachRoom(World world, string roomName)
    {
        List<string> roomsReached = new List<string>() { "room_castle_darkdoor" };
        List<string> allRooms = new List<string>() { "room_castle_darkdoor" };
        while (!allRooms.Contains(roomName))
        {
            if (roomsReached.Count <= 0)
            {
                return false;
            }
            List<string> tempRooms = new List<string>();
            foreach (var item in roomsReached)
            {
                var tempRoom = world.rooms.Find(x => x.name == item);
                for (int i = 0; i < tempRoom.connections.Count; i++)
                {
                    if (!allRooms.Contains(world.rooms.Find(x => x.name == tempRoom.connections[i].Remove(tempRoom.connections[i].Length - 1)).name))
                    {
                        if (tempRoom.connectionRules[i].Invoke(world))
                        {
                            if (world.rooms.Find(x => x.name == tempRoom.connections[i].Remove(tempRoom.connections[i].Length - 1)).connectionRules[world.rooms.Find(x => x.name == tempRoom.connections[i].Remove(tempRoom.connections[i].Length - 1)).connections.FindIndex(x => x.Remove(x.Length - 1) == tempRoom.name)].Invoke(world))
                            {
                                tempRooms.Add(world.rooms.Find(x => x.name == tempRoom.connections[i].Remove(tempRoom.connections[i].Length - 1)).name);
                                allRooms.Add(world.rooms.Find(x => x.name == tempRoom.connections[i].Remove(tempRoom.connections[i].Length - 1)).name);
                            }
                        }
                    }
                }
            }
            roomsReached = new List<string>(tempRooms);
        }
        return true;
    }
    public static bool CanReachEntrance(World world, string roomName)
    {
        List<string> roomsReached = new List<string>() { "room_castle_darkdoor" };
        List<string> allCons = new List<string>();
        List<string> allRooms = new List<string>() { "room_castle_darkdoor" };
        while (!allCons.Contains(roomName))
        {
            if (roomsReached.Count <= 0)
            {
                return false;
            }
            List<string> tempRooms = new List<string>();
            foreach (var item in roomsReached)
            {
                var tempRoom = world.rooms.Find(x => x.name == item);
                for (int i = 0; i < tempRoom.connections.Count; i++)
                {
                    if (!allRooms.Contains(world.rooms.Find(x => x.name == tempRoom.connections[i].Remove(tempRoom.connections[i].Length - 1)).name))
                    {
                        if (tempRoom.connectionRules[i].Invoke(world))
                        {
                            var targetRoom = world.rooms.Find(x => x.name == tempRoom.connections[i].Remove(tempRoom.connections[i].Length - 1));
                            if (tempRoom.connections.Contains(roomName) || targetRoom.connectionRules[targetRoom.connections.FindIndex(x => x.Remove(x.Length - 1) == tempRoom.name)].Invoke(world))
                            {
                                tempRooms.Add(world.rooms.Find(x => x.name == tempRoom.connections[i].Remove(tempRoom.connections[i].Length - 1)).name);
                                allRooms.Add(world.rooms.Find(x => x.name == tempRoom.connections[i].Remove(tempRoom.connections[i].Length - 1)).name);
                                foreach (var conn in world.rooms.Find(x => x.name == tempRoom.connections[i].Remove(tempRoom.connections[i].Length - 1)).connections)
                                {
                                    allCons.Add(conn);
                                }
                            }
                        }
                    }
                }
            }
            roomsReached = new List<string>(tempRooms);
        }
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
    public static bool CanLevelUp(World world, int strength)
    {
        int amountFound = 0;
        foreach (var item in world.enemyRooms)
        {
            if (SpecialLogic.CanReachRoom(world, world.rooms[item].name))
            {
                amountFound++;
            }
        }
        return amountFound > strength*2;
    }
}

public class Logic
{
    public void SetLogic(World inWorld)
    {
        inWorld.rules[0] = (world) => SpecialLogic.CanReachRoom(world, "room_field2A");
        inWorld.rules[1] = (world) => SpecialLogic.CanReachRoom(world, "room_field2A");
        inWorld.rules[2] = (world) => SpecialLogic.CanReachRoom(world, "room_field_puzzle1");
        inWorld.rules[3] = (world) => SpecialLogic.CanReachRoom(world, "room_field_puzzle1");
        inWorld.rules[5] = (world) => Rule.PlacedItem(world, new ItemType("BrokenCake")) && SpecialLogic.CanReachRoom(world, "room_forest_smith");
        inWorld.rules[6] = (world) => SpecialLogic.CanReachRoom(world, "room_forest_savepoint2");
        inWorld.rules[7] = (world) => SpecialLogic.CanReachRoom(world, "room_forest_savepoint2");
        inWorld.rules[8] = (world) => SpecialLogic.CanReachRoom(world, "room_forest_savepoint2");
        inWorld.rules[9] = (world) => Rule.PlacedItem(world, new ItemType("Top Cake")) && SpecialLogic.CanReachRoom(world, "room_field_topchef");
        inWorld.rules[11] = (world) => Rule.PlacedItem(world, new ItemType("Manual")) && Rule.PlacedItem(world, new ItemType("C MENU"));
        inWorld.rules[12] = (world) => SpecialLogic.CanCompleteTutorial(world);
        inWorld.rules[13] = (world) => Rule.PlacedItem(world, new ItemType("Field Secret Key")) && SpecialLogic.CanReachRoom(world, "room_field_secret1");
        inWorld.rules[14] = (world) => SpecialLogic.CanReachRoom(world, "room_forest_dancers1");
        inWorld.rules[15] = (world) => SpecialLogic.CanReachEntrance(world, "room_forest_dancers1x");
        inWorld.rules[16] = (world) => SpecialLogic.CanReachRoom(world, "room_forest_area2A");
        inWorld.rules[17] = (world) => SpecialLogic.CanReachRoom(world, "room_forest_area3A");
        inWorld.rules[18] = (world) => SpecialLogic.CanReachRoom(world, "room_forest_area4");
        inWorld.rules[20] = (world) => Rule.PlacedItem(world, new ItemType("LEFT SOUL")) && Rule.PlacedItem(world, new ItemType("RIGHT SOUL")) && Rule.PlacedItem(world, new ItemType("DOWN SOUL")) && Rule.PlacedItem(world, new ItemType("UP SOUL")) && SpecialLogic.CanReachRoom(world, "room_cc_joker") && SpecialLogic.CanReachRoom(world, "room_cc_prison_prejoker");
        inWorld.rules[21] = (world) => SpecialLogic.CanReachRoom(world, "room_cc_2f");
        inWorld.rules[22] = (world) => SpecialLogic.CanReachRoom(world, "room_cc_4f");
        inWorld.rules[23] = (world) => Rule.PlacedItem(world, new ItemType("Field Secret Key")) && SpecialLogic.CanReachRoom(world, "room_field_maze");
        inWorld.rules[24] = (world) => Rule.PlacedItem(world, new ItemType("LEFT SOUL")) && Rule.PlacedItem(world, new ItemType("RIGHT SOUL")) && Rule.PlacedItem(world, new ItemType("DOWN SOUL")) && Rule.PlacedItem(world, new ItemType("UP SOUL")) && SpecialLogic.CanReachRoom(world, "room_cc_joker") && SpecialLogic.CanReachRoom(world, "room_cc_prison_prejoker");
        inWorld.rules[25] = (world) => SpecialLogic.CanReachRoom(world, "room_shop1");
        inWorld.rules[26] = (world) => SpecialLogic.CanReachRoom(world, "room_shop1");
        inWorld.rules[27] = (world) => SpecialLogic.CanReachRoom(world, "room_shop1");
        inWorld.rules[28] = (world) => SpecialLogic.CanReachRoom(world, "room_shop1");
        inWorld.rules[29] = (world) => SpecialLogic.CanReachRoom(world, "room_shop2");
        inWorld.rules[30] = (world) => SpecialLogic.CanReachRoom(world, "room_shop2");
        inWorld.rules[31] = (world) => SpecialLogic.CanReachRoom(world, "room_shop2");
        inWorld.rules[32] = (world) => SpecialLogic.CanReachRoom(world, "room_shop2");
        inWorld.rules[33] = (world) => Rule.PlacedItem(world, new ItemType("Broken Key A")) && Rule.PlacedItem(world, new ItemType("Broken Key B")) && Rule.PlacedItem(world, new ItemType("Broken Key C")) && SpecialLogic.CanReachRoom(world, "room_forest_smith");
        inWorld.rules[34] = (world) => SpecialLogic.CanReachRoom(world, "room_field_topchef");
        inWorld.rules[35] = (world) => SpecialLogic.CanReachEntrance(world, "room_cc_prison_cellsb");
        inWorld.rules[36] = (world) => SpecialLogic.CanCompleteTutorial(world);
        inWorld.rules[37] = (world) => (Rule.PlacedItem(world, new ItemType("RUN")) || world.options[3] == 1) && SpecialLogic.CanReachRoom(world, "room_field4");
        inWorld.rules[38] = (world) => SpecialLogic.CanReachRoom(world, "room_forest_savepoint1");
        inWorld.rules[39] = (world) => SpecialLogic.CanReachRoom(world, "room_forest_savepoint3");
        inWorld.rules[40] = (world) => SpecialLogic.CanReachRoom(world, "room_shop1") && SpecialLogic.CanReachRoom(world, "room_cc_prison_prejoker");
        inWorld.rules[41] = (world) => Rule.PlacedItem(world, new ItemType("FIGHT")) || Rule.PlacedItem(world, new ItemType("DEFEND")) || Rule.PlacedItem(world, new ItemType("SPARE"));
        inWorld.rules[42] = (world) => SpecialLogic.CanLevelUp(world, 0);
        inWorld.rules[43] = (world) => SpecialLogic.CanLevelUp(world, 0);
        inWorld.rules[44] = (world) => SpecialLogic.CanLevelUp(world, 0);
        inWorld.rules[45] = (world) => SpecialLogic.CanLevelUp(world, 1);
        inWorld.rules[46] = (world) => SpecialLogic.CanLevelUp(world, 1);
        inWorld.rules[47] = (world) => SpecialLogic.CanLevelUp(world, 1);
        inWorld.rules[48] = (world) => SpecialLogic.CanLevelUp(world, 1);
        inWorld.rules[49] = (world) => SpecialLogic.CanLevelUp(world, 1);
        inWorld.rules[50] = (world) => SpecialLogic.CanLevelUp(world, 2);
        inWorld.rules[51] = (world) => SpecialLogic.CanLevelUp(world, 2);
        inWorld.rules[52] = (world) => SpecialLogic.CanLevelUp(world, 2);
        inWorld.rules[53] = (world) => SpecialLogic.CanLevelUp(world, 2);
        inWorld.rules[54] = (world) => SpecialLogic.CanLevelUp(world, 2);
        inWorld.rules[55] = (world) => SpecialLogic.CanLevelUp(world, 3);
        inWorld.rules[56] = (world) => SpecialLogic.CanLevelUp(world, 3);
        inWorld.rules[57] = (world) => SpecialLogic.CanLevelUp(world, 3);
        inWorld.rules[58] = (world) => SpecialLogic.CanLevelUp(world, 3);
        inWorld.rules[59] = (world) => SpecialLogic.CanLevelUp(world, 3);
        inWorld.rules[60] = (world) => Rule.PlacedItem(world, new ItemType("Susie")) && SpecialLogic.CanLevelUp(world, 0);
        inWorld.rules[61] = (world) => Rule.PlacedItem(world, new ItemType("Susie")) && SpecialLogic.CanLevelUp(world, 0);
        inWorld.rules[62] = (world) => Rule.PlacedItem(world, new ItemType("Susie")) && SpecialLogic.CanLevelUp(world, 0);
        inWorld.rules[63] = (world) => Rule.PlacedItem(world, new ItemType("Susie")) && SpecialLogic.CanLevelUp(world, 0);
        inWorld.rules[64] = (world) => Rule.PlacedItem(world, new ItemType("Susie")) && SpecialLogic.CanLevelUp(world, 1);
        inWorld.rules[65] = (world) => Rule.PlacedItem(world, new ItemType("Susie")) && SpecialLogic.CanLevelUp(world, 1);
        inWorld.rules[66] = (world) => Rule.PlacedItem(world, new ItemType("Susie")) && SpecialLogic.CanLevelUp(world, 1);
        inWorld.rules[67] = (world) => Rule.PlacedItem(world, new ItemType("Susie")) && SpecialLogic.CanLevelUp(world, 1);
        inWorld.rules[68] = (world) => Rule.PlacedItem(world, new ItemType("Susie")) && SpecialLogic.CanLevelUp(world, 1);
        inWorld.rules[69] = (world) => Rule.PlacedItem(world, new ItemType("Susie")) && SpecialLogic.CanLevelUp(world, 2);
        inWorld.rules[70] = (world) => Rule.PlacedItem(world, new ItemType("Susie")) && SpecialLogic.CanLevelUp(world, 2);
        inWorld.rules[71] = (world) => Rule.PlacedItem(world, new ItemType("Susie")) && SpecialLogic.CanLevelUp(world, 2);
        inWorld.rules[72] = (world) => Rule.PlacedItem(world, new ItemType("Susie")) && SpecialLogic.CanLevelUp(world, 2);
        inWorld.rules[73] = (world) => Rule.PlacedItem(world, new ItemType("Susie")) && SpecialLogic.CanLevelUp(world, 2);
        inWorld.rules[74] = (world) => Rule.PlacedItem(world, new ItemType("Susie")) && SpecialLogic.CanLevelUp(world, 3);
        inWorld.rules[75] = (world) => Rule.PlacedItem(world, new ItemType("Susie")) && SpecialLogic.CanLevelUp(world, 3);
        inWorld.rules[76] = (world) => Rule.PlacedItem(world, new ItemType("Susie")) && SpecialLogic.CanLevelUp(world, 3);
        inWorld.rules[77] = (world) => Rule.PlacedItem(world, new ItemType("Susie")) && SpecialLogic.CanLevelUp(world, 3);
        inWorld.rules[78] = (world) => Rule.PlacedItem(world, new ItemType("Susie")) && SpecialLogic.CanLevelUp(world, 3);
        inWorld.rules[79] = (world) => Rule.PlacedItem(world, new ItemType("Ralsei")) && SpecialLogic.CanLevelUp(world, 0);
        inWorld.rules[80] = (world) => Rule.PlacedItem(world, new ItemType("Ralsei")) && SpecialLogic.CanLevelUp(world, 0);
        inWorld.rules[81] = (world) => Rule.PlacedItem(world, new ItemType("Ralsei")) && SpecialLogic.CanLevelUp(world, 0);
        inWorld.rules[82] = (world) => Rule.PlacedItem(world, new ItemType("Ralsei")) && SpecialLogic.CanLevelUp(world, 0);
        inWorld.rules[83] = (world) => Rule.PlacedItem(world, new ItemType("Ralsei")) && SpecialLogic.CanLevelUp(world, 1);
        inWorld.rules[84] = (world) => Rule.PlacedItem(world, new ItemType("Ralsei")) && SpecialLogic.CanLevelUp(world, 1);
        inWorld.rules[85] = (world) => Rule.PlacedItem(world, new ItemType("Ralsei")) && SpecialLogic.CanLevelUp(world, 1);
        inWorld.rules[86] = (world) => Rule.PlacedItem(world, new ItemType("Ralsei")) && SpecialLogic.CanLevelUp(world, 1);
        inWorld.rules[87] = (world) => Rule.PlacedItem(world, new ItemType("Ralsei")) && SpecialLogic.CanLevelUp(world, 1);
        inWorld.rules[88] = (world) => Rule.PlacedItem(world, new ItemType("Ralsei")) && SpecialLogic.CanLevelUp(world, 2);
        inWorld.rules[89] = (world) => Rule.PlacedItem(world, new ItemType("Ralsei")) && SpecialLogic.CanLevelUp(world, 2);
        inWorld.rules[90] = (world) => Rule.PlacedItem(world, new ItemType("Ralsei")) && SpecialLogic.CanLevelUp(world, 2);
        inWorld.rules[91] = (world) => Rule.PlacedItem(world, new ItemType("Ralsei")) && SpecialLogic.CanLevelUp(world, 2);
        inWorld.rules[92] = (world) => Rule.PlacedItem(world, new ItemType("Ralsei")) && SpecialLogic.CanLevelUp(world, 2);
        inWorld.rules[93] = (world) => Rule.PlacedItem(world, new ItemType("Ralsei")) && SpecialLogic.CanLevelUp(world, 3);
        inWorld.rules[94] = (world) => Rule.PlacedItem(world, new ItemType("Ralsei")) && SpecialLogic.CanLevelUp(world, 3);
        inWorld.rules[95] = (world) => Rule.PlacedItem(world, new ItemType("Ralsei")) && SpecialLogic.CanLevelUp(world, 3);
        inWorld.rules[96] = (world) => Rule.PlacedItem(world, new ItemType("Ralsei")) && SpecialLogic.CanLevelUp(world, 3);
        inWorld.rules[97] = (world) => Rule.PlacedItem(world, new ItemType("Ralsei")) && SpecialLogic.CanLevelUp(world, 3);
        inWorld.rules[98] = (world) => SpecialLogic.CanReachRoom(world, "room_cc_preroof");
        inWorld.rules[99] = (world) => SpecialLogic.CanReachRoom(world, "room_field_checkers4");
        inWorld.rules[100] = (world) => SpecialLogic.CanReachRoom(world, "room_forest_area0");
        inWorld.rules[101] = (world) => SpecialLogic.CanReachRoom(world, "room_cc_1f");
        inWorld.rules[102] = (world) => SpecialLogic.CanReachRoom(world, "room_field2");
        inWorld.rules[103] = (world) => SpecialLogic.CanReachRoom(world, "room_forest_area2");
        inWorld.rules[110] = (world) => SpecialLogic.CanReachRoom(world, "room_field_getsusie");
        inWorld.rules[111] = (world) => Rule.PlacedItem(world, new ItemType("FIGHT")) || Rule.PlacedItem(world, new ItemType("DEFEND")) || Rule.PlacedItem(world, new ItemType("SPARE"));
        inWorld.rules[112] = (world) => SpecialLogic.CanReachRoom(world, "room_cc_prisonlancer");
        inWorld.rules[113] = (world) => SpecialLogic.CanReachRoom(world, "room_field_topchef");
        inWorld.rules[114] = (world) => SpecialLogic.CanReachRoom(world, "room_field_maze");
        inWorld.rules[115] = (world) => SpecialLogic.CanReachRoom(world, "room_field_maze");
        inWorld.rules[116] = (world) => SpecialLogic.CanReachRoom(world, "room_field_puzzletutorial");
        inWorld.rules[117] = (world) => SpecialLogic.CanReachRoom(world, "room_forest_area1");
        inWorld.rules[118] = (world) => SpecialLogic.CanReachRoom(world, "room_forest_beforeclover");
        inWorld.rules[119] = (world) => SpecialLogic.CanReachRoom(world, "room_forest_area5");
        inWorld.rules[120] = (world) => SpecialLogic.CanReachRoom(world, "room_cc_prison2");
        inWorld.rules[121] = (world) => SpecialLogic.CanReachRoom(world, "room_cc_rudinn");
        inWorld.rules[122] = (world) => SpecialLogic.CanReachRoom(world, "room_cc_hathy");
        inWorld.rules[126] = (world) => SpecialLogic.CanReachRoom(world, "room_field_start");
        inWorld.rules[127] = (world) => SpecialLogic.CanReachRoom(world, "room_field_maze");
        inWorld.rules[128] = (world) => SpecialLogic.CanReachRoom(world, "room_field_shop1");
        inWorld.rules[129] = (world) => SpecialLogic.CanReachRoom(world, "room_field_checkers3");
        inWorld.rules[130] = (world) => SpecialLogic.CanReachRoom(world, "room_field_checkers7");
        inWorld.rules[131] = (world) => SpecialLogic.CanReachRoom(world, "room_forest_savepoint1");
        inWorld.rules[132] = (world) => SpecialLogic.CanReachRoom(world, "room_forest_savepoint2");
        inWorld.rules[133] = (world) => SpecialLogic.CanReachRoom(world, "room_forest_savepoint_relax");
        inWorld.rules[134] = (world) => SpecialLogic.CanReachRoom(world, "room_forest_savepoint3");
        inWorld.rules[135] = (world) => SpecialLogic.CanReachRoom(world, "room_cc_prison_to_elevator");
        inWorld.rules[136] = (world) => SpecialLogic.CanReachRoom(world, "room_cc_1f");
        inWorld.rules[137] = (world) => SpecialLogic.CanReachRoom(world, "room_cc_5f");
        inWorld.rules[138] = (world) => SpecialLogic.CanReachRoom(world, "room_cc_throneroom");
        inWorld.rules[139] = (world) => SpecialLogic.CanReachRoom(world, "room_cc_prison_prejoker");
        inWorld.rules[140] = (world) => SpecialLogic.CanReachRoom(world, "room_field1");
    }
}
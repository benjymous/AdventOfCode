using AoC.Utils;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2019
{
    public class Day25 : IPuzzle
    {
        public string Name { get { return "2019-25"; } }

        static HashSet<string> traps = new HashSet<string>()
        {
            "molten lava",
            "infinite loop",
            "photons",
            "escape pod",
            "giant electromagnet"
        };

        class SearchDroid : NPSA.ASCIITerminal
        {
            int commandCount = 0;
            int combosTried = 0;
            int skippedCombos = 0;
            public SearchDroid(string program) : base(program)
            {
                //cpu.Reserve(5400);
            }

            HashSet<string> HeldItems = new HashSet<string>();

            HashSet<string> LastCombo;

            void TryCombo(IEnumerable<string> items)
            {
                LastCombo = new HashSet<string>(items);
                combosTried++;
                foreach (var item in knownItems)
                {
                    if (HeldItems.Contains(item) && !LastCombo.Contains(item))
                    {
                        inputs.Enqueue($"drop {item}");
                        HeldItems.Remove(item);
                    }
                }

                foreach (var item in items)
                {
                    if (!HeldItems.Contains(item))
                    {
                        inputs.Enqueue($"take {item}");
                        HeldItems.Add(item);
                    }
                }

                inputs.Enqueue("south");
            }

            Queue<string> inputs = new Queue<string>();
            Queue<IEnumerable<string>> itemCombos = null;

            List<string> knownItems = new List<string>();

            public string Run()
            {
                cpu.Run();
                //Console.WriteLine(cpu.Speed());
                //Console.WriteLine($"{commandCount} commands, {combosTried} combos tried, {skippedCombos} combos skipped");
                return buffer.Lines.Last();
            }

            public (string name, IEnumerable<string> exits, IEnumerable<string> items) ParseRoom(List<string> lines)
            {
                // find LAST room header
                var roomName = lines.Where(line => line.StartsWith("==")).LastOrDefault();

                List<string> exits = new List<string>();
                List<string> items = new List<string>();

                if (roomName == null)
                {
                    return (null, exits, items);
                }

                var startIdx = lines.IndexOf(roomName) + 3;


                List<string> current = null;


                for (int i = startIdx; i < lines.Count(); ++i)
                {
                    if (lines[i] == "Doors here lead:")
                    {
                        current = exits;
                    }
                    else if (lines[i] == "Items here:")
                    {
                        current = items;
                    }
                    else if (lines[i].StartsWith("-"))
                    {
                        current.Add(lines[i].Substring(2));
                    }
                }

                return (roomName, exits, items);
            }

            Dictionary<string, Room> rooms = new Dictionary<string, Room>();

            public class Room
            {
                public Room(string r, IEnumerable<string> e)
                {
                    name = r;
                    foreach (var exit in e)
                    {
                        exits[exit] = null;
                    }
                }
                public string name;
                public Dictionary<string, Room> exits = new Dictionary<string, Room>();

                public override string ToString()
                {
                    return name;
                }
            }

            Dictionary<string, string> reverseDir = new Dictionary<string, string>()
            {
                {"north", "south"},
                {"south", "north"},
                {"east", "west"},
                {"west", "east"}
            };

            Stack<(Room room, string exit)> unvisited = new Stack<(Room room, string exit)>();

            Room currentRoom = null;
            Room lastRoom = null;
            string backtrack = null;
            string lastTravel = null;

            public override IEnumerable<string> AutomaticInput()
            {
                var l = buffer.Lines.ToList();
                var roomData = ParseRoom(l);
                buffer.Clear();



                if (roomData.name != null)
                {

                    if (!rooms.TryGetValue(roomData.name, out currentRoom))
                    {
                        //Console.WriteLine($"New room {roomData.name}");
                        var newRoom = new Room(roomData.name, roomData.exits);

                        rooms[roomData.name] = newRoom;

                        foreach (var e in roomData.exits)
                        {
                            if (e != backtrack)
                            {
                                //Console.WriteLine($"new destination, {e} from {roomData.name}");
                                unvisited.Push((newRoom, e));
                            }
                        }

                        foreach (var item in roomData.items)
                        {
                            if (traps.Contains(item))
                            {
                                //Console.WriteLine($"Will ignore {item}");
                            }
                            else
                            {
                                //Console.WriteLine($"Will take {item}");
                                inputs.Enqueue($"take {item}");
                                knownItems.Add(item);
                                HeldItems.Add(item);
                            }
                        }

                        currentRoom = newRoom;
                    }

                    if (backtrack != null && currentRoom.exits.ContainsKey(backtrack) && currentRoom.exits[backtrack] == null)
                    {
                        currentRoom.exits[backtrack] = lastRoom;
                        lastRoom.exits[lastTravel] = currentRoom;
                    }

                    if (roomData.name == "== Security Checkpoint ==" && unvisited.Count == 0)
                    {
                        if (itemCombos == null)
                        {
                            //Console.WriteLine(commandCount);
                            itemCombos = new Queue<IEnumerable<string>>();
                            var perms = knownItems.Combinations().OrderBy(x => x.Count());
                            foreach (var perm in perms)
                            {
                                if (perm.Count() > 0)
                                {
                                    itemCombos.Enqueue(perm);
                                }
                            }
                            //Console.WriteLine($"{itemCombos.Count} combos to try");
                        }
                        else
                        {
                            var sensorResult = l.Where(line => line.Contains("robotic")).LastOrDefault();
                            if (sensorResult.Contains("lighter"))
                            {
                                //Console.WriteLine($"Combo '{string.Join(",", LastCombo)}' is too heavy");

                                Queue<IEnumerable<string>> filteredCombos = new Queue<IEnumerable<string>>();

                                foreach (var combo in itemCombos)
                                {
                                    if (LastCombo.Intersect(combo).Count() == LastCombo.Count())
                                    {
                                        //Console.WriteLine($"Skip '{string.Join(",", combo)}'");
                                        skippedCombos++;
                                    }
                                    else
                                    {
                                        //Console.WriteLine($"Keep '{string.Join(",", combo)}'");
                                        filteredCombos.Enqueue(combo);
                                    }
                                }

                                itemCombos = filteredCombos;

                                //
                            }
                        }
                        if (inputs.Count == 0 && itemCombos.Count > 0)
                        {
                            var combo = itemCombos.Dequeue();
                            TryCombo(combo);
                        }
                    }
                }

                if (inputs.Count == 0)
                {
                    if (unvisited.Count > 0)
                    {
                        //Console.WriteLine($"{unvisited.Count} locations to visit");

                        var location = unvisited.Pop();

                        //Console.WriteLine($"Will next visit {location.room.name} and go {location.exit}");

                        pathFind(location);
                    }
                    else
                    {
                        //Console.WriteLine("Have visited everywhere!");
                        pathFind((rooms["== Security Checkpoint =="], null));
                    }
                }

                if (inputs.Any())
                {
                    var command = inputs.Dequeue();

                    if (reverseDir.ContainsKey(command))
                    {
                        lastTravel = command;
                        lastRoom = currentRoom;
                        backtrack = reverseDir[command];
                    }

                    commandCount++;
                    yield return command;

                }
            }

            private void GoDirection(string direction)
            {
                //Console.WriteLine($"Will go {direction}");
                inputs.Enqueue(direction);

            }

            private void pathFind((Room room, string exit) location)
            {
                if (location.room == currentRoom)
                {
                    GoDirection(location.exit);
                }
                else
                {
                    var route = RouteFind(currentRoom, location.room);
                    if (route.Count > 0)
                    {
                        foreach (var direction in route)
                        {
                            GoDirection(direction);
                        }
                        if (location.exit != null)
                        {
                            GoDirection(location.exit);
                        }
                    }
                }
            }

            public List<string> RouteFind(Room from, Room to, HashSet<Room> tried = null)
            {
                if (tried == null)
                {
                    tried = new HashSet<Room>();
                }

                foreach (var exit in from.exits)
                {
                    if (exit.Value == to)
                    {
                        return new List<string>() { exit.Key };
                    }
                }

                foreach (var exit in from.exits)
                {
                    if (exit.Value != null && !tried.Contains(exit.Value))
                    {
                        var newTried = new HashSet<Room>();
                        foreach (var room in tried)
                        {
                            newTried.Add(room);
                        }
                        newTried.Add(from);

                        var route = RouteFind(exit.Value, to, newTried);
                        if (route != null)
                        {
                            var commands = new List<string> { exit.Key };
                            commands.AddRange(route);
                            return commands;
                        }
                    }
                }

                return null;
            }

        }



        public static int Part1(string input)
        {
            var droid = new SearchDroid(input);

            bool interactive = false;

            droid.SetDisplay(interactive);
            droid.Interactive = interactive;

            var result = droid.Run();

            if (!interactive)
            {
                //Console.WriteLine(result);
            }

            if (result.Contains("Oh, hello")) return int.Parse(result.Split()[11]);

            return 0;
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
        }
    }
}
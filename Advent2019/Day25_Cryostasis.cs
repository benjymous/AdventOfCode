using Advent.Utils.Collections;
using AoC.Utils;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2019
{
    public class Day25 : IPuzzle
    {
        public string Name => "2019-25";

        static readonly HashSet<string> traps = new() { "molten lava", "infinite loop", "photons", "escape pod", "giant electromagnet" };

        class SearchDroid : NPSA.ASCIITerminal
        {
            public SearchDroid(string program) : base(program, 5500) {}

            readonly Dictionary<string, Room> Rooms = new();
            readonly Stack<(Room room, string exit)> UnvisitedNeighbours = new();

            readonly Queue<string> Inputs = new();

            Room CurrentRoom = null, LastRoom = null, CheckpointRoom = null;
            IEnumerable<string[]> ItemCombos = null;
            readonly HashSet<string> HeldItems = new();
            string LastTravelDirection = null;

            public string Run()
            {
                cpu.Run();
                System.Console.WriteLine(cpu.Speed());
                return buffer.Lines.Last();
            }

            Room ParseRoom(IEnumerable<string> lines)
            {
                var roomName = lines.Where(line => line[0] == '=').LastOrDefault(); // find LAST room header

                return (roomName == null) ? CurrentRoom : Rooms.GetOrCalculate(roomName, roomName =>
                {                
                    List<string> current = null, exits = new(), items = new();
                    foreach (var line in lines.Skip(lines.IndexOf(roomName) + 2))
                    {
                        switch (line[0])
                        {
                            case 'D': current = exits; break;
                            case 'I': current = items; break;
                            case '-': current.Add(line[2..]); break;
                        }
                    }

                    var room = new Room(roomName, exits.ToBiMap<string, string, Room>(e => e, e => default));
                    LastRoom?.Link(room, LastTravelDirection);
                    UnvisitedNeighbours.PushRange(room.GetUnvisited().OrderByDescending(v => v.direction));
                    items.Except(traps).ForEach(TakeItem);

                    if (roomName == "== Security Checkpoint ==") CheckpointRoom = room;

                    return room;
                });
            }

            record class Room(string Name, BiMap<string, Room> Exits)
            {
                public bool DeadEnd => Exits.Count == 1;
                public bool IsUnvisited(string direction) => Exits.TryGet(direction, out var r) && r == null;
                public IEnumerable<(Room room, string direction)> GetUnvisited() => Exits.Entries().Where(kvp => kvp.Value == null).Select(kvp => (this, kvp.Key));
                public void Link(Room other, string direction) => (Exits[direction], other.Exits[ReverseDir(direction)]) = (other, this);
            }

            static string ReverseDir(string dir) => dir[0] switch
            {
                'n' => "south",
                's' => "north",
                'e' => "west",
                'w' => "east",
                _ => throw new System.Exception("Unexpected direction")
            };

            public override IEnumerable<string> AutomaticInput()
            {
                var lines = buffer.Pop().WithoutNullOrWhiteSpace();
                CurrentRoom = ParseRoom(lines);
                //System.Console.WriteLine($"{CurrentRoom.Name} {cpu.CycleCount}");

                if (!Inputs.Any())
                {
                    if (CurrentRoom == CheckpointRoom && UnvisitedNeighbours.Count == 0)
                    {
                        ItemCombos ??= HeldItems.Combinations().Select(c => c.ToArray()).OrderBy(c => System.Math.Abs(c.Length - HeldItems.Count / 2)).ToArray();
                        ItemCombos = lines.Where(line => line.Contains("robotic")).Select(line => line.Contains("lighter") ? 1 : -1).FirstOrDefault(0) switch
                        {
                            /* too heavy */ > 0 => ItemCombos.Where(combo => !HeldItems.IsSubsetOf(combo)).ToArray(),
                            /* too light */ < 0 when HeldItems.Count > 1 => ItemCombos.Where(combo => !HeldItems.IsSupersetOf(combo)).ToArray(),
                            _ => ItemCombos
                        };

                        TryNextItemCombo();
                    }
                    else TravelToNextDestination();
                }
                (LastTravelDirection, LastRoom) = (!CurrentRoom.DeadEnd && CurrentRoom.IsUnvisited(Inputs.Peek())) ? (Inputs.Peek(), CurrentRoom) : (null, null);
                //System.Console.WriteLine($"> {Inputs.Peek()}");
                yield return Inputs.Dequeue();
            }

            private void GoDirection(string direction) => Inputs.Enqueue(direction);
            private void TakeItem(string item) { Inputs.Enqueue($"take {item}"); HeldItems.Add(item); }
            private void DropItem(string item) { Inputs.Enqueue($"drop {item}"); HeldItems.Remove(item); }

            void TryNextItemCombo()
            {
                var items = ItemCombos.First();
                HeldItems.Except(items).ForEach(DropItem);
                items.Except(HeldItems).ForEach(TakeItem);
                GoDirection(CurrentRoom.GetUnvisited().First().direction);
            }

            void TravelToNextDestination()
            {
                var (room, exit) = UnvisitedNeighbours.Any() ? UnvisitedNeighbours.Pop() : (CheckpointRoom, null);
                if (room != CurrentRoom)
                {
                    var route = RouteFind(CurrentRoom, room);
                    //System.Console.WriteLine($"Travelling {route.Count()} steps to {room.Name}:{exit}");
                    Inputs.EnqueueRange(route);
                }
                if (exit != null) GoDirection(exit);
            }

            IEnumerable<string> RouteFind(Room from, Room to, HashSet<string> tried = null)
            {
                if (from.Exits.TryGet(to, out var found)) return found.ToEnumerable();

                foreach (var exit in from.Exits.ValuesNonNull.Where(e => tried == null || !tried.Contains(e.Name)))
                {
                    var route = RouteFind(exit, to, (tried ??= new()).Append(from.Name).ToHashSet());
                    if (route != null) return route.Prepend(from.Exits[exit]);
                }

                return null;
            }
        }

        public static int Part1(string input)
        {
            var droid = new SearchDroid(input);

            //bool interactive = false;

            //droid.SetDisplay(interactive);
            //droid.Interactive = interactive;

            var result = droid.Run();

            //if (!interactive)
            //{
            //Console.WriteLine(result);
            //}

            return int.Parse(result.Split()[11]);
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
        }
    }
}
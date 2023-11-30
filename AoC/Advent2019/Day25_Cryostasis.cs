namespace AoC.Advent2019;
public class Day25 : IPuzzle
{
    static readonly HashSet<string> traps = ["molten lava", "infinite loop", "photons", "escape pod", "giant electromagnet"];
    static readonly Dictionary<string, string> Reverse = new() { { "north", "south" }, { "east", "west" }, { "south", "north" }, { "west", "east" } };

    class SearchDroid(string program) : NPSA.ASCIITerminal(program, 5500)
    {
        readonly Stack<(Room room, string exit)> UnvisitedDestinations = new();
        readonly Queue<string> Inputs = [];
        Room CurrentRoom = null, LastRoom = null, CheckpointRoom = null;
        string[][] ItemCombos = null;
        readonly HashSet<string> HeldItems = [];
        string LastTravelDirection = null;

        public new string Run()
        {
            base.Run();
            return buffer.Lines.Last();
        }

        static IEnumerable<(char filter, string value)> FilterLines(IEnumerable<string> lines, char filter = '?') => lines.Select(l => { if (l[0] != '-') filter = l[0]; return l; }).Where(l => l[0] == '-').Select(l => (filter, l[2..]));

        Room GetOrParseRoom(string roomName, IEnumerable<string> lines) => Memoizer.Memoize(roomName, roomName =>
        {
            var filtered = FilterLines(lines).ToArray();
            var exits = filtered.Where(f => f.filter == 'D').Select(f => f.value).ToBiMap(e => e, e => LastTravelDirection != null && e == Reverse[LastTravelDirection] ? LastRoom : default);
            var items = filtered.Where(f => f.filter == 'I' && !traps.Contains(f.value)).Select(f => f.value);

            var room = new Room(roomName, exits);
            if (LastRoom != null) LastRoom.Exits[LastTravelDirection] = room;

            UnvisitedDestinations.PushRange(room.GetUnvisited());
            items.ForEach(TakeItem);

            if (roomName == "== Security Checkpoint ==") CheckpointRoom = room;

            return room;
        });

        record class Room(string Name, BiMap<string, Room> Exits)
        {
            public bool ShouldVisit(string direction) => Exits.Count > 1 && Exits.TryGet(direction, out var r) && r == null;
            public IEnumerable<(Room room, string direction)> GetUnvisited() => Exits.Entries().Where(kvp => kvp.Value == null).Select(kvp => (this, kvp.Key));
        }

        public override IEnumerable<string> AutomaticInput()
        {
            var lines = buffer.Pop().WithoutNullOrWhiteSpace().ToArray();
            var (roomName, roomLines) = lines.Select((line, i) => (line, lines.Skip(i + 2))).Where(v => v.line[0] == '=').LastOrDefault(); // find LAST room header
            if (roomName != null) CurrentRoom = GetOrParseRoom(roomName, roomLines);

            if (Inputs.Count == 0)
                if (CurrentRoom == CheckpointRoom && UnvisitedDestinations.Count == 0)
                    TryItemCombo((ItemCombos = lines.Where(line => line.Contains("robotic")).Select(line => line.Contains("lighter") ? 1 : -1).FirstOrDefault() switch
                    {
                        > 0 => ItemCombos.Where(combo => !HeldItems.IsSubsetOf(combo)).ToArray(),
                        < 0 when HeldItems.Count > 1 => ItemCombos.Where(combo => !HeldItems.IsSupersetOf(combo)).ToArray(),
                        _ => ItemCombos ??= [.. HeldItems.Combinations().Select(c => c.ToArray()).OrderBy(c => Math.Abs(c.Length - (HeldItems.Count / 2)))]
                    }).First());
                else TravelToNextDestination();
            (LastTravelDirection, LastRoom) = CurrentRoom.ShouldVisit(Inputs.Peek()) ? (Inputs.Peek(), CurrentRoom) : (null, null);
            yield return Inputs.Dequeue();
        }

        private void TakeItem(string item) { Inputs.Enqueue($"take {item}"); HeldItems.Add(item); }
        private void DropItem(string item) { Inputs.Enqueue($"drop {item}"); HeldItems.Remove(item); }

        void TryItemCombo(string[] items)
        {
            HeldItems.Except(items).ForEach(DropItem);
            items.Except(HeldItems).ForEach(TakeItem);
            Inputs.Enqueue(CurrentRoom.GetUnvisited().Single().direction);
        }

        void TravelToNextDestination()
        {
            var (room, exit) = UnvisitedDestinations.Count != 0 ? UnvisitedDestinations.Pop() : (CheckpointRoom, null);
            if (room != CurrentRoom) Inputs.EnqueueRange(RouteFind(CurrentRoom, room));
            if (exit != null) Inputs.Enqueue(exit);
        }

        static IEnumerable<string> RouteFind(Room currentRoom, Room destinationRoom, HashSet<string> tried = null)
        {
            if (currentRoom.Exits.TryGet(destinationRoom, out var found)) return [found];
            var (route, exit) = currentRoom.Exits.ValuesNonNull.Where(e => tried == null || !tried.Contains(e.Name)).Select(exit => (RouteFind(exit, destinationRoom, [.. (tried ??= []), currentRoom.Name]), exit)).Where(v => v.Item1 != null).FirstOrDefault();
            return route != default ? ([currentRoom.Exits[exit], .. route]) : null;
        }
    }

    public static int Part1(string input) => int.Parse(new SearchDroid(input).Run().Split()[11]);

    public void Run(string input, ILogger logger) => logger.WriteLine("- Pt1 - " + Part1(input));
}
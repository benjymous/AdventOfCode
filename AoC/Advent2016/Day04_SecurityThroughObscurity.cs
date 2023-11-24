namespace AoC.Advent2016;
public class Day04 : IPuzzle
{
    [method: Regex(@"(.+)-(\d+)\[(.+)\]")]
    class Room(string RoomName, int id, string Checksum)
    {
        public readonly int SectionID = id;

        public bool IsReal => Checksum == RoomName.Where(c => c != '-').GroupBy(c => c).Select(g => (val: g.Key, count: g.Count())).OrderByDescending(v => v.count).ThenBy(v => v.val).Take(5).Select(v => v.val).AsString();

        public bool IsDesiredRoom => RoomName.Split("-").Where(p => p.Length == 9).Any(part => DecryptedStartsWith(part, "north"));

        private bool DecryptedStartsWith(string input, string check)
        {
            for (int i = 0; i < check.Length; ++i)
            {
                if ((char)('a' + ((input[i] - 'a' + SectionID) % 26)) != check[i]) return false;
            }
            return true;
        }
    }

    public static int Part1(string input)
    {
        var rooms = Util.RegexParse<Room>(input);

        return rooms.Where(r => r.IsReal).Sum(r => r.SectionID);
    }

    public static int Part2(string input)
    {
        var found = Util.RegexParse<Room>(input).Where(r => r.IsReal && r.IsDesiredRoom);

        return found.First().SectionID;
    }

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}
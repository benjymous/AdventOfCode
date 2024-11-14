namespace AoC.Advent2016;
public class Day04 : IPuzzle
{
    [Regex(@"(.+)-(\d+)\[(.+)\]")]
    public record class Room(string RoomName, int SectionID, string Checksum)
    {
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

    public static int Part1(Util.AutoParse<Room> rooms) => rooms.Where(r => r.IsReal).Sum(r => r.SectionID);

    public static int Part2(Util.AutoParse<Room> rooms) => rooms.Single(r => r.IsReal && r.IsDesiredRoom).SectionID;

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}
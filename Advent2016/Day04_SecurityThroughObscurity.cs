using AoC.Utils;
using System;
using System.Linq;

namespace AoC.Advent2016
{
    public class Day04 : IPuzzle
    {
        public string Name => "2016-04";

        class Room
        {
            const string nameValid = "abcdefghijklmnopqrstuvwxyz-";

            public Room(string val)
            {
                val = val.Replace("]", "");
                var bits = val.Split('[');

                RoomName = bits[0].Where(nameValid.Contains).AsString();
                SectionID = int.Parse(bits[0].Where(c => c.IsDigit()).AsString());
                Checksum = bits[1];
            }

            public readonly string RoomName;
            public readonly string Checksum;
            public readonly int SectionID;

            public bool IsReal => Checksum == RoomName.Where(c => c != '-').GroupBy(c => c).Select(g => (val: g.Key, count: g.Count())).OrderByDescending(v => v.count).ThenBy(v => v.val).Take(5).Select(v => v.val).AsString();

            public bool IsDesiredRoom => RoomName.Split("-").Where(p => p.Length == 9).Any(part => DecryptedStartsWith(part, "north"));

            private bool DecryptedStartsWith(string input, string check)
            {
                for(int i=0; i< check.Length; ++i)
                {
                    if ((char)('a' + ((input[i] - 'a' + SectionID) % 26)) != check[i]) return false;
                }
                return true;
            }
        }

        public static int Part1(string input)
        {
            var rooms = Util.Parse<Room>(input);

            return rooms.Where(r => r.IsReal).Sum(r => r.SectionID);
        }

        public static int Part2(string input)
        {
            var found = Util.Parse<Room>(input).Where(r => r.IsReal && r.IsDesiredRoom);

            return found.First().SectionID;
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}
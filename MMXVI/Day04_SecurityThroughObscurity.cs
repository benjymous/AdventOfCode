using Advent.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Advent.MMXVI
{
    public class Day04 : IPuzzle
    {
        public string Name { get { return "2016-04"; } }

        class Room
        {

            const string nameValid = "abcdefghijklmnopqrstuvwxyz-";
            const string numberValid = "0123456789";

            public Room(string val)
            {
                val = val.Replace("]", "");
                var bits = val.Split('[');

                RoomName = bits[0].Where(c => nameValid.Contains(c)).AsString();

                SectionID = int.Parse(bits[0].Where(c => numberValid.Contains(c)).AsString());

                Checksum = bits[1];
            }

            public string RoomName { get; private set; }
            public string Checksum { get; private set; }

            public int SectionID { get; private set; }

            public override string ToString()
            {
                return $"{RoomName} {SectionID} [{Checksum}]";
            }

            public bool IsReal
            {
                get
                {
                    var counts = new Dictionary<char, int>();

                    foreach (var c in RoomName)
                    {
                        if (c != '-' && (c >= 'a' && c <= 'z'))
                        {
                            counts.IncrementAtIndex(c);
                        }
                    }

                    var data = counts.OrderBy(kvp => -kvp.Value).ThenBy(kvp => kvp.Key);

                    var test = String.Join("", data.Take(5).Select(kvp => kvp.Key));

                    return test == Checksum;
                }
            }

            private char Increment(char c)
            {
                if (c == '-' || c == ' ') return ' ';
                c++;
                if (c > 'z') return 'a';
                return c;
            }

            public string Decrypt()
            {
                var data = RoomName.ToArray();
                for (int i = 0; i < SectionID; ++i)
                {
                    for (int j = 0; j < data.Length; ++j)
                    {
                        data[j] = Increment(data[j]);
                    }
                }

                return data.AsString();
            }
        }

        public static int Part1(string input)
        {
            var rooms = Util.Parse<Room>(input);

            return rooms.Where(r => r.IsReal).Select(r => r.SectionID).Sum();
        }

        public static int Part2(string input)
        {
            var rooms = Util.Parse<Room>(input);

            var decrypted = rooms.Where(r => r.IsReal).Select(r => Tuple.Create(r.Decrypt(), r));

            var found = decrypted.Where(d => d.Item1.Contains("north"));

            return found.First().Item2.SectionID;
        }

        public void Run(string input, ILogger logger)
        {

            //Console.WriteLine(Part1("aaaaa-bbb-z-y-x-123[abxyz]\na-b-c-d-e-f-g-h-987[abcde]\nnot-a-real-room-404[oarel]\ntotally-real-room-200[decoy]\n"));

            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}
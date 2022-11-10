using AoC.Utils;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2021
{
    public class Day16 : IPuzzle
    {
        public string Name => "2021-16";

        public enum PacketType
        {
            Sum = 0,
            Product = 1,
            Minimum = 2,
            Maximum = 3,
            LiteralValue = 4,
            GreaterThan = 5,
            LessThan = 6,
            EqualTo = 7
        }

        public class Packet
        {
            public static Packet Parse(string input) => new BitStream(input).ReadPacket();
            internal Packet(BitStream stream)
            {
                Header = (stream.ReadInteger(3), (PacketType)stream.ReadInteger(3));
                if (Header.Type == PacketType.LiteralValue)
                {
                    LiteralValue = stream.ReadLiteralValue();
                }
                else
                {
                    if (stream.ReadBit() == 0) // length type
                    {
                        var endPos = stream.ReadInteger(15) + stream.Position;
                        Children = Util.RepeatWhile(stream.ReadPacket, _ => stream.Position < endPos).ToList();
                    }
                    else Children = Util.Repeat(stream.ReadPacket, stream.ReadInteger(11)).ToList();
                }
            }

            public readonly (int Version, PacketType Type) Header;
            public readonly long LiteralValue;
            public readonly List<Packet> Children = new();
            public int VersionSum() => Header.Version + Children.Sum(child => child.VersionSum());
            public long GetValue() => Header.Type switch
            {
                PacketType.LiteralValue => LiteralValue,
                PacketType.Sum          => ChildValues.Sum(),
                PacketType.Product      => ChildValues.Product(),
                PacketType.Minimum      => ChildValues.Min(),
                PacketType.Maximum      => ChildValues.Max(),
                PacketType.GreaterThan  => ChildValues[0] >  ChildValues[1] ? 1 : 0,
                PacketType.LessThan     => ChildValues[0] <  ChildValues[1] ? 1 : 0,
                PacketType.EqualTo      => ChildValues[0] == ChildValues[1] ? 1 : 0,
                _ => 0,
            };
            long[] ChildValues => Children.Select(c => c.GetValue()).ToArray();
        }

        public class BitStream
        {
            public BitStream(string input)
            {
                stream = Bits(input).GetEnumerator();
                stream.MoveNext();
            }

            public int Position => stream.Current.idx;
            public Packet ReadPacket() => new(this);
            public long ReadLiteralValue() => Util.RepeatWhile(ReadChunk, chunk => chunk.continueRead).Select(chunk => chunk.value).Aggregate(0L, (total, val) => (total << 4) + val);
            public int ReadInteger(int numBits) => Util.Repeat(ReadBit, numBits).Aggregate(0, (total, val) => (total << 1) + val);
            public int ReadBit() => stream.Pop().val;

            static IEnumerable<(int, int)> Bits(string raw) => raw.Select(ch => ch.ParseHex()).SelectMany(n => n.BinarySequenceBigEndian(8)).WithIndex().Select(v => (v.Value, v.Index));
            (bool continueRead, int value) ReadChunk() => (ReadBit() == 1, ReadInteger(4));
            readonly IEnumerator<(int val,int idx)> stream;
        }

        public static int Part1(string input)
        {
            return Packet.Parse(input).VersionSum();
        }

        public static long Part2(string input)
        {
            return Packet.Parse(input).GetValue();
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}
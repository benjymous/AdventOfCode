namespace AoC.Advent2021;
public class Day16 : IPuzzle
{
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
        internal Packet(BitStream stream)
        {
            Header = (stream.ReadInteger(3), (PacketType)stream.ReadInteger(3));
            if (Header.Type == PacketType.LiteralValue)
            {
                Value = stream.ReadLiteralValue();
            }
            else
            {
                if (stream.ReadBit() == 0) // length type
                {
                    var endPos = stream.ReadInteger(15) + stream.Position;
                    Children = Util.RepeatWhile(stream.ReadPacket, _ => stream.Position < endPos).ToArray();
                }
                else Children = Util.Repeat(stream.ReadPacket, stream.ReadInteger(11)).ToArray();

                Value = Header.Type switch
                {
                    PacketType.Sum => Children.Sum(c => c.Value),
                    PacketType.Product => Children.Product(c => c.Value),
                    PacketType.Minimum => Children.Min(c => c.Value),
                    PacketType.Maximum => Children.Max(c => c.Value),
                    PacketType.GreaterThan => Children[0].Value > Children[1].Value ? 1 : 0,
                    PacketType.LessThan => Children[0].Value < Children[1].Value ? 1 : 0,
                    PacketType.EqualTo => Children[0].Value == Children[1].Value ? 1 : 0,
                    _ => Value,
                };
            }
        }

        public readonly (int Version, PacketType Type) Header;
        public readonly long Value;
        public readonly Packet[] Children = [];
        public int VersionSum() => Header.Version + Children.Sum(child => child.VersionSum());

        public static implicit operator Packet(string data) => new BitStream(data).ReadPacket();
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

        static IEnumerable<(int, int)> Bits(string raw) => raw.Select(ch => ch.ParseHex()).SelectMany(n => n.BinarySequenceBigEndian(8)).Index().Select(v => (v.Item, v.Index));
        (bool continueRead, int value) ReadChunk() => (ReadBit() == 1, ReadInteger(4));
        readonly IEnumerator<(int val, int idx)> stream;
    }

    public static int Part1(Packet packet) => packet.VersionSum();

    public static long Part2(Packet packet) => packet.Value;

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}
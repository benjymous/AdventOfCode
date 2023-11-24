namespace AoC.Advent2019.NPSA
{

    abstract class ASCIITerminal(string program, int reserve = 0) : IntCPU(program, reserve), ICPUInterrupt
    {
        protected ASCIIBuffer buffer = new();

        protected long finalOutput;

        public bool Interactive { get; set; } = false;

        public void SetDisplay(bool on) => buffer.DisplayLive = on;

        public void OutputReady()
        {
            long v = Output.Dequeue();

            if (v <= 255)
            {
                buffer.Write((char)v);
            }
            else
            {
                finalOutput = v;
            }
        }

        public abstract IEnumerable<string> AutomaticInput();

        public void RequestInput()
        {
            var inputs = AutomaticInput().ToArray();

            if (inputs.Length != 0)
            {
                foreach (var line in inputs)
                {
                    if (Interactive)
                    {
                        Console.WriteLine(line);
                    }
                    foreach (var c in line)
                    {
                        AddInput(c);
                    }
                    AddInput('\n');
                }
            }
            else
            {
                if (Interactive)
                {
                    Console.Write("?> ");
                    var input = Console.ReadLine();
                    AddInput(input.Select(c => (long)c).ToArray());
                    AddInput('\n');
                }
            }
        }
    }

    class InteractiveTerminal : ASCIITerminal
    {
        public InteractiveTerminal(string program) : base(program)
        {
            Interactive = true;
            SetDisplay(true);
        }

        public override IEnumerable<string> AutomaticInput() => throw new NotImplementedException();
    }

    public class ASCIIBuffer
    {
        readonly Dictionary<PackedPos32, char> screenBuffer = [];

        public ManhattanVector2 Cursor { get; } = new ManhattanVector2(0, 0);

        public ManhattanVector2 Max { get; } = new ManhattanVector2(0, 0);

        public bool DisplayLive { get; set; } = false;

        public List<string> Lines { get; set; } = [];
        private readonly List<char> LineBuffer = [];

        public IEnumerable<string> Pop()
        {
            var lines = new List<string>(Lines);
            Lines.Clear();
            return lines;
        }

        public void Write(char c)
        {
            if (DisplayLive) Console.Write(c);

            if (c == '\n')
            {
                Lines.Add(LineBuffer.AsString());
                LineBuffer.Clear();

                if (Cursor.X == 0)
                {
                    Cursor.Y = 0;
                    Cursor.X = 0;
                    if (DisplayLive) Console.WriteLine();
                }

                Cursor.X = 0;
                Cursor.Y++;
            }
            else
            {
                LineBuffer.Add(c);
                screenBuffer[Cursor.AsPackedPos32()] = c;
                Cursor.X++;
            }

            Max.X = Math.Max(Max.X, Cursor.X);
            Max.Y = Math.Max(Max.Y, Cursor.Y);
        }

        public char GetAt(PackedPos32 pos) => screenBuffer.GetOrDefault(pos);

        public PackedPos32 FindCharacter(char c)
        {
            var res = FindAll(c);
            if (res.Count != 0)
            {
                return res.First();
            }

            throw new Exception("Not found");
        }

        public HashSet<PackedPos32> FindAll(char c) => screenBuffer.Where(kvp => kvp.Value == c).Select(kvp => kvp.Key).ToHashSet();
    }
}
namespace AoC.Advent2019;
public class Day23 : IPuzzle
{
    public class NetworkNode(string program, int networkId, NetworkController networkController) : NPSA.IntCPU(program, 3000, [networkId]), NPSA.ICPUInterrupt
    {
        public void OutputReady()
        {
            if (Output.Count == 3) networkController.EnqueuePacket((int)Output.Dequeue(), (Output.Dequeue(), Output.Dequeue()));
        }

        public void RequestInput() => AddInput(networkController.TryGetPacket(networkId, out var data) ? [data.x, data.y] : [-1]);
    }

    public class NAT(NetworkController networkController)
    {
        public (long x, long y) lastPacket;

        public long LastY = -1;
        private readonly int[] IdleCount = new int[256];

        public void NotifyStarvation(int id) => IdleCount[id]++;
        public void NotifySeen(int id) => IdleCount[id] = 0;

        public bool Step()
        {
            while (networkController.TryGetPacket(255, out var packet)) lastPacket = packet;

            if (IdleCount.Count(v => v > 2) == 50)
            {
                networkController.EnqueuePacket(0, lastPacket);
                for (int i = 0; i < IdleCount.Length; ++i) IdleCount[i] = 0;

                if (lastPacket.y == LastY) return false;
                LastY = lastPacket.y;
            }

            return true;
        }
    }

    public class NetworkController
    {
        private readonly List<NetworkNode> nodes = [];
        private readonly Dictionary<int, Queue<(long x, long y)>> messageQueues = [];
        public NAT Nat = null;

        private Queue<(long x, long y)> MessageQueue(int index) => messageQueues.GetOrCreate(index);

        public NetworkController(string program) => nodes = [.. Enumerable.Range(0, 50).Select(i => new NetworkNode(program, i, this))];

        public void EnqueuePacket(int id, (long x, long y) vec)
        {
            MessageQueue(id).Enqueue(vec);
            Nat?.NotifySeen(id);
        }

        public bool TryGetPacket(int id, out (long x, long y) vec)
        {
            if (!MessageQueue(id).TryDequeue(out vec))
            {
                if (Nat != null && id < 255) Nat.NotifyStarvation(id);
                return false;
            }
            return true;
        }

        public void Run()
        {
            while (nodes.All(n => n.Step()) && (Nat != null ? Nat.Step() : MessageQueue(255).Count == 0)) ;
        }
    }

    public static long Part1(string input)
    {
        var network = new NetworkController(input);
        network.Run();
        return network.TryGetPacket(255, out var answer) ? answer.y : 0;
    }

    public static long Part2(string input)
    {
        var network = new NetworkController(input);
        network.Nat = new NAT(network);
        network.Run();
        return network.Nat.lastPacket.y;
    }

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}
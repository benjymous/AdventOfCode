using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2019
{
    public class Day23 : IPuzzle
    {
        public string Name => "2019-23";

        public class NetworkNode : NPSA.ICPUInterrupt
        {
            readonly NPSA.IntCPU cpu;
            readonly int NetworkId;
            readonly NetworkController controller;

            public NetworkNode(string program, int networkId, NetworkController networkController)
            {
                NetworkId = networkId;
                cpu = new NPSA.IntCPU(program) { Interrupt = this };
                cpu.Reserve(3000);
                cpu.Input.Enqueue(networkId);
                controller = networkController;
            }

            public bool Step() => cpu.Step();

            public void OutputReady()
            {
                if (cpu.Output.Count == 3)
                {
                    controller.QueuePacket((int)cpu.Output.Dequeue(), (cpu.Output.Dequeue(), cpu.Output.Dequeue()));
                }
            }

            public void RequestInput()
            {
                if (controller.TryGetPacket(NetworkId, out var data))
                {
                    cpu.Input.Enqueue(data.x);
                    cpu.Input.Enqueue(data.y);
                }
                else
                {
                    cpu.Input.Enqueue(-1);
                }
            }
        }

        public class NAT
        {
            public (long x, long y) lastPacket;

            public long LastY = -1;
            readonly NetworkController controller;
            readonly int[] IdleCount = new int[256];

            public NAT(NetworkController networkController, int numNodes)
            {
                controller = networkController;
                for (int i = 0; i < numNodes; ++i) IdleCount[i] = 0;
            }

            public void NotifyStarvation(int id)
            {
                if (id < 255) IdleCount[id]++;
            }

            public void PacketSeen(int id)
            {
                IdleCount[id] = 0;
            }

            public bool Step()
            {
                while (controller.TryGetPacket(255, out var packet))
                {
                    lastPacket = packet;
                }

                if (IdleCount.Count(v => v > 2) == 50)
                {
                    controller.QueuePacket(0, lastPacket);
                    for (int i = 0; i < IdleCount.Length; ++i) IdleCount[i] = 0;

                    if (lastPacket.y == LastY) return false;
                    LastY = lastPacket.y;
                }

                return true;
            }
        }

        public class NetworkController
        {
            readonly List<NetworkNode> nodes = new();

            public Queue<(long x, long y)>[] messageQueue = new Queue<(long x, long y)>[256];

            public NAT Nat = null;

            public NetworkController(string program, int numNodes)
            {
                messageQueue[255] = new();

                for (int i = 0; i < numNodes; ++i)
                {
                    nodes.Add(new NetworkNode(program, i, this));
                    messageQueue[i] = new();
                }
            }

            public void QueuePacket(int id, (long x, long y) vec)
            {
                messageQueue[id].Enqueue(vec);
                Nat?.PacketSeen(id);
            }

            public bool TryGetPacket(int id, out (long x, long y) vec)
            {
                if (!messageQueue[id].TryDequeue(out vec))
                {
                    Nat?.NotifyStarvation(id);
                    return false;
                }
                return true;
            }

            public void Run()
            {
                while (nodes.All(n => n.Step()) && (Nat != null ? Nat.Step() : !messageQueue[255].Any()));
            }
        }

        public static long Part1(string input)
        {
            var network = new NetworkController(input, 50);
            network.Run();

            network.TryGetPacket(255, out var answer);

            return answer.y;
        }

        public static long Part2(string input)
        {
            var network = new NetworkController(input, 50);
            network.Nat = new NAT(network, 50);
            network.Run();
            return network.Nat.lastPacket.y;
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}
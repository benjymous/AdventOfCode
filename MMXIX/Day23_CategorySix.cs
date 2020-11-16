using System;
using System.Collections.Generic;
using System.Linq;

namespace Advent.MMXIX
{
    public class Day23 : IPuzzle
    {
        public string Name { get { return "2019-23"; } }

        public class BigVec
        {
            public Int64 X;
            public Int64 Y;

            public BigVec(Int64 x, Int64 y)
            {
                X = x;
                Y = y;
            }
        }

        public class NetworkNode : NPSA.ICPUInterrupt
        {
            NPSA.IntCPU cpu;
            int NetworkId;

            NetworkController controller;

            public NetworkNode(string program, int networkId, NetworkController networkController)
            {
                NetworkId = networkId;
                cpu = new NPSA.IntCPU(program);
                cpu.Reserve(3000);
                cpu.Input.Enqueue(networkId);
                cpu.Interrupt = this;
                controller = networkController;
            }

            public bool Step() => cpu.Step();

            public void HasPutOutput()
            {
                Int64 id, x, y;

                if (cpu.Output.Count == 3)
                {
                    id = cpu.Output.Dequeue();
                    x = cpu.Output.Dequeue();
                    y = cpu.Output.Dequeue();

                    controller.QueuePacket((int)id, x, y);

                }
            }

            public void WillReadInput()
            {
                if (controller.TryGetPacket(NetworkId, out var data))
                {
                    cpu.Input.Enqueue(data.X);
                    cpu.Input.Enqueue(data.Y);
                }
                else
                {
                    cpu.Input.Enqueue(-1);
                }
            }
        }

        public class NAT
        {
            public BigVec lastPacket = null;

            public Int64 LastY = -1;

            NetworkController controller;

            Dictionary<int, int> IdleCount = new Dictionary<int, int>();

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

                var idle = IdleCount.Where(v => v.Value > 2).Count();

                if (idle == 50)
                {
                    //Console.WriteLine($"{idle} nodes idle");
                    controller.QueuePacket(0, lastPacket.X, lastPacket.Y);
                    for (int i = 0; i < IdleCount.Count; ++i) IdleCount[i] = 0;

                    if (lastPacket.Y == LastY) return false;
                    LastY = lastPacket.Y;
                }

                return true;
            }
        }

        public class NetworkController
        {
            List<NetworkNode> nodes = new List<NetworkNode>();

            public Dictionary<int, Queue<BigVec>> messageQueue = new Dictionary<int, Queue<BigVec>>();

            public NAT Nat = null;

            public NetworkController(string program, int numNodes)
            {
                messageQueue[255] = new Queue<BigVec>();

                for (int i = 0; i < numNodes; ++i)
                {
                    nodes.Add(new NetworkNode(program, i, this));
                    messageQueue[i] = new Queue<BigVec>();
                }
            }

            public void QueuePacket(int id, Int64 x, Int64 y)
            {
                //Console.WriteLine($"{id} - {x},{y}");
                messageQueue[id].Enqueue(new BigVec(x, y));
                Nat?.PacketSeen(id);
            }

            public bool TryGetPacket(int id, out BigVec vec)
            {
                if (!messageQueue[id].TryDequeue(out vec))
                {
                    Nat?.NotifyStarvation(id);
                    return false;
                }
                return true;
            }

            public bool Step()
            {
                bool running = true;
                foreach (var node in nodes)
                {
                    running &= node.Step();
                }
                if (Nat == null)
                {
                    if (messageQueue[255].Any()) return false;
                }
                else
                {
                    running &= Nat.Step();
                }
                return running;
            }

            public void Run()
            {
                while (Step()) ;
            }
        }

        public static Int64 Part1(string input)
        {
            var network = new NetworkController(input, 50);
            network.Run();

            network.TryGetPacket(255, out var answer);

            return answer.Y;
        }

        public static Int64 Part2(string input)
        {
            var network = new NetworkController(input, 50);
            network.Nat = new NAT(network, 50);
            network.Run();
            return network.Nat.lastPacket.Y;
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}
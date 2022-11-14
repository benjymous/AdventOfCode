using AoC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2016
{
    public class Day11 : IPuzzle
    {
        public string Name => "2016-11";

        public class State
        {
            public State(string input)
            {
                var lines = Util.Split(input.Replace(",", "").Replace(".", ""));

                Dictionary<string, int> nameLookup = new();
                chipsPerFloor = new byte[4];
                generatorsPerFloor = new byte[4];
                for (int i = 0; i < 4; ++i)
                {
                    chipsPerFloor[i] = 0;
                    generatorsPerFloor[i] = 0;
                }

                int floor = 0;
                foreach (var line in lines)
                {
                    var bits = line.Split(' ');

                    for (int i = 0; i < bits.Length; i++)
                    {
                        if (bits[i] == "microchip")
                        {
                            var chipName = bits[i - 1].Replace("-compatible", "");

                            int idx = 0;
                            if (nameLookup.ContainsKey(chipName))
                            {
                                idx = nameLookup[chipName];
                            }
                            else
                            {
                                idx = nameLookup.Count;
                                nameLookup[chipName] = idx;
                            }

                            chipsPerFloor[floor] += (byte)(1 << idx);
                        }
                        if (bits[i] == "generator")
                        {
                            var genName = bits[i - 1];

                            int idx = 0;
                            if (nameLookup.ContainsKey(genName))
                            {
                                idx = nameLookup[genName];
                            }
                            else
                            {
                                idx = nameLookup.Count;
                                nameLookup[genName] = idx;
                            }

                            generatorsPerFloor[floor] += (byte)(1 << idx);
                        }
                    }

                    floor++;
                }
            }

            public State(State previous, int newFloor, int moveChips, int moveGens)
            {
                steps = previous.steps + 1;
                elevator = newFloor;
                var oldfloor = previous.elevator;

                chipsPerFloor = previous.chipsPerFloor.ToArray();
                generatorsPerFloor = previous.generatorsPerFloor.ToArray();

                chipsPerFloor[oldfloor] -= (byte)moveChips;
                chipsPerFloor[newFloor] += (byte)moveChips;

                generatorsPerFloor[oldfloor] -= (byte)moveGens;
                generatorsPerFloor[newFloor] += (byte)moveGens;
            }

            static readonly byte empty = 0;

            public IEnumerable<State> GetMoves()
            {
                var currentChips = chipsPerFloor[elevator].BitSequence().ToArray();
                var currentGens = generatorsPerFloor[elevator].BitSequence().ToArray();

                foreach (var chip1 in currentChips)
                {
                    if (elevator < 3) yield return new State(this, elevator + 1, chip1, empty);
                    if (elevator > 0) yield return new State(this, elevator - 1, chip1, empty);

                    foreach (var chip2 in currentChips)
                    {
                        if (chip1 != chip2)
                        {
                            if (elevator < 3) yield return new State(this, elevator + 1, chip1 + chip2, empty);
                        }
                    }

                    foreach (var gen in currentGens)
                    {
                        if (elevator < 3) yield return new State(this, elevator + 1, chip1, gen);
                    }
                }

                foreach (var gen1 in currentGens)
                {
                    if (elevator < 3) yield return new State(this, elevator + 1, empty, gen1);
                    if (elevator > 0) yield return new State(this, elevator - 1, empty, gen1);

                    foreach (var gen2 in currentGens)
                    {
                        if (gen1 != gen2)
                        {
                            if (elevator < 3) yield return new State(this, elevator + 1, empty, gen1 + gen2);
                        }
                    }
                }
            }

            public bool IsValid()
            {
                for (int i = 0; i < 4; ++i)
                {
                    if (generatorsPerFloor[i] > 0 && chipsPerFloor[i] > 0)
                    {
                        foreach (var bit in chipsPerFloor[i].BitSequence())
                        {
                            if (!((generatorsPerFloor[i] & bit) == bit))
                            {
                                return false;
                            }
                        }
                    }
                }

                return true;
            }

            public int Remaining()
            {
                int sum = 0;
                for (int i = 0; i < 3; ++i)
                {
                    sum += chipsPerFloor[i].CountBits();
                    sum += generatorsPerFloor[i].CountBits();
                }
                return sum;
            }

            public ulong Key()
            {
                ulong result = 0;

                for (int i = 0; i < 4; ++i)
                {
                    result <<= 7;
                    result += chipsPerFloor[i];
                    result <<= 7;
                    result += generatorsPerFloor[i];
                }

                result <<= 4;
                result += (ulong)elevator;

                return result;
            }

            public byte[] chipsPerFloor;
            public byte[] generatorsPerFloor;
            readonly int elevator = 0;
            public int steps = 0;
        }


        private static int FindBestPath(State initialState)
        {
            PriorityQueue<State, int> queue = new();
            queue.Enqueue(initialState, 0);

            Dictionary<ulong, int> cache = new();

            int bestScore = int.MaxValue;
            int closest = int.MaxValue;

            queue.Operate((state) =>
            {
                foreach (var move in state.GetMoves().Where(move => move.IsValid()))
                {
                    int remaining = move.Remaining();

                    if (remaining == 0)
                    {
                        bestScore = Math.Min(bestScore, move.steps);
                        continue;
                    }

                    if (remaining - closest > 2) continue;
                    closest = Math.Min(remaining, closest);

                    var key = move.Key();

                    if ((move.steps >= bestScore) || (cache.TryGetValue(key, out int prevScore) && prevScore <= move.steps)) continue;
                    cache[key] = move.steps;

                    queue.Enqueue(move, move.steps + remaining);
                }
            });

            return bestScore;
        }

        public static int Part1(string input)
        {
            var initialState = new State(input);

            return FindBestPath(initialState);
        }

        public static int Part2(string input)
        {
            var initialState = new State(input);

            int next = initialState.chipsPerFloor.Sum(v => v.CountBits());

            int bit1 = 1 << next;
            int bit2 = 1 << (next + 1);

            initialState.chipsPerFloor[0] += (byte)bit1;
            initialState.chipsPerFloor[0] += (byte)bit2;

            initialState.generatorsPerFloor[0] += (byte)bit1;
            initialState.generatorsPerFloor[0] += (byte)bit2;

            return FindBestPath(initialState);
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}
public static class Extension
{
    public static IEnumerable<byte> Sequence(this byte num)
    {
        byte i = 0;
        while (num > 0)
        {
            if ((num & 1) == 1)
            {
                yield return i;
            }
            num >>= 1; i++;
        }
    }

    public static int CountBits(this byte num)
    {
        int i = 0;
        while (num > 0)
        {
            if ((num & 1) == 1) { i++; }
            num >>= 1;
        }
        return i;
    }
}
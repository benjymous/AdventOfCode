using AoC.Utils;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2017
{
    public class Day09 : IPuzzle
    {
        public string Name => "2017-09";

        public class State
        {
            public State(string i) { input = i; }
            public string input;
            public int GarbageCount = 0;
        }

        public static IEnumerable<char> StripGarbage(State state)
        {
            bool inGarbage = false;
            bool skipNext = false;
            foreach (var ch in state.input)
            {
                if (inGarbage)
                {
                    if (skipNext)
                    {
                        skipNext = false;
                        continue;
                    }

                    if (ch == '!')
                    {
                        skipNext = true;
                        continue;
                    }

                    if (ch == '>')
                    {
                        inGarbage = false;
                        continue;
                    }

                    state.GarbageCount++;
                }
                else
                {
                    if (ch == '<')
                    {
                        inGarbage = true;
                        continue;
                    }
                    yield return ch;
                }
            }
        }

        public static IEnumerable<int> GetGroups(string input)
        {
            int currentScore = 0;
            foreach (var ch in StripGarbage(new State(input)))
            {
                if (ch == '{') currentScore++;
                if (ch == '}')
                {
                    yield return currentScore--;
                }
            }
        }

        public static int GetScore(string input)
        {
            return GetGroups(input).Sum();
        }

        public static int CountGarbage(string input)
        {
            var state = new State(input);
            var stripped = StripGarbage(state).AsString();
            return state.GarbageCount;
        }

        public static int Part1(string input)
        {
            return GetScore(input);
        }

        public static int Part2(string input)
        {
            return CountGarbage(input);
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}
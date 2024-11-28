using AoC.Utils.Parser;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2022.Test
{
    [TestCategory("2022")]
    [TestClass]
    public class Day19Test
    {
        readonly string input = Util.GetInput<Day19>();

        [TestCategory("Test")]

        [DataRow("Blueprint 1: Each ore robot costs 4 ore. Each clay robot costs 2 ore. Each obsidian robot costs 3 ore and 14 clay. Each geode robot costs 2 ore and 7 obsidian.", 9)]
        [DataRow("Blueprint 2: Each ore robot costs 2 ore. Each clay robot costs 3 ore. Each obsidian robot costs 3 ore and 8 clay. Each geode robot costs 3 ore and 12 obsidian.", 24)]

        [DataRow("Blueprint 1: Each ore robot costs 4 ore. Each clay robot costs 4 ore. Each obsidian robot costs 3 ore and 5 clay. Each geode robot costs 3 ore and 18 obsidian.", 3)]
        [DataRow("Blueprint 2: Each ore robot costs 4 ore. Each clay robot costs 4 ore. Each obsidian robot costs 4 ore and 16 clay. Each geode robot costs 2 ore and 15 obsidian.", 0)]
        [DataRow("Blueprint 3: Each ore robot costs 3 ore. Each clay robot costs 4 ore. Each obsidian robot costs 4 ore and 19 clay. Each geode robot costs 4 ore and 11 obsidian.", 0)]
        [DataRow("Blueprint 4: Each ore robot costs 4 ore. Each clay robot costs 3 ore. Each obsidian robot costs 2 ore and 13 clay. Each geode robot costs 2 ore and 9 obsidian.", 12)]
        [DataRow("Blueprint 5: Each ore robot costs 4 ore. Each clay robot costs 4 ore. Each obsidian robot costs 2 ore and 14 clay. Each geode robot costs 4 ore and 15 obsidian.", 0)]
        [DataRow("Blueprint 6: Each ore robot costs 4 ore. Each clay robot costs 3 ore. Each obsidian robot costs 3 ore and 10 clay. Each geode robot costs 2 ore and 10 obsidian.", 30)]
        [DataRow("Blueprint 7: Each ore robot costs 2 ore. Each clay robot costs 4 ore. Each obsidian robot costs 4 ore and 16 clay. Each geode robot costs 4 ore and 17 obsidian.", 7)]
        [DataRow("Blueprint 8: Each ore robot costs 2 ore. Each clay robot costs 4 ore. Each obsidian robot costs 3 ore and 19 clay. Each geode robot costs 4 ore and 12 obsidian.", 16)]
        [DataRow("Blueprint 9: Each ore robot costs 3 ore. Each clay robot costs 3 ore. Each obsidian robot costs 3 ore and 17 clay. Each geode robot costs 2 ore and 13 obsidian.", 18)]
        [DataRow("Blueprint 10: Each ore robot costs 3 ore. Each clay robot costs 4 ore. Each obsidian robot costs 4 ore and 8 clay. Each geode robot costs 2 ore and 10 obsidian.", 60)]
        [DataRow("Blueprint 11: Each ore robot costs 4 ore. Each clay robot costs 3 ore. Each obsidian robot costs 2 ore and 7 clay. Each geode robot costs 3 ore and 8 obsidian.", 121)]
        [DataRow("Blueprint 12: Each ore robot costs 3 ore. Each clay robot costs 4 ore. Each obsidian robot costs 3 ore and 18 clay. Each geode robot costs 4 ore and 16 obsidian.", 0)]
        [DataRow("Blueprint 13: Each ore robot costs 4 ore. Each clay robot costs 3 ore. Each obsidian robot costs 2 ore and 20 clay. Each geode robot costs 3 ore and 9 obsidian.", 13)]
        [DataRow("Blueprint 14: Each ore robot costs 4 ore. Each clay robot costs 3 ore. Each obsidian robot costs 4 ore and 6 clay. Each geode robot costs 3 ore and 11 obsidian.", 98)]
        [DataRow("Blueprint 15: Each ore robot costs 4 ore. Each clay robot costs 4 ore. Each obsidian robot costs 2 ore and 16 clay. Each geode robot costs 4 ore and 16 obsidian.", 0)]
        [DataRow("Blueprint 16: Each ore robot costs 4 ore. Each clay robot costs 3 ore. Each obsidian robot costs 4 ore and 5 clay. Each geode robot costs 3 ore and 19 obsidian.", 48)]
        [DataRow("Blueprint 17: Each ore robot costs 4 ore. Each clay robot costs 4 ore. Each obsidian robot costs 3 ore and 5 clay. Each geode robot costs 4 ore and 11 obsidian.", 102)]
        [DataRow("Blueprint 18: Each ore robot costs 4 ore. Each clay robot costs 3 ore. Each obsidian robot costs 4 ore and 18 clay. Each geode robot costs 3 ore and 13 obsidian.", 0)]
        [DataRow("Blueprint 19: Each ore robot costs 3 ore. Each clay robot costs 3 ore. Each obsidian robot costs 3 ore and 9 clay. Each geode robot costs 3 ore and 7 obsidian.", 228)]
        [DataRow("Blueprint 20: Each ore robot costs 2 ore. Each clay robot costs 3 ore. Each obsidian robot costs 2 ore and 14 clay. Each geode robot costs 3 ore and 20 obsidian.", 40)]
        [DataRow("Blueprint 21: Each ore robot costs 2 ore. Each clay robot costs 4 ore. Each obsidian robot costs 2 ore and 20 clay. Each geode robot costs 2 ore and 17 obsidian.", 21)]
        [DataRow("Blueprint 22: Each ore robot costs 4 ore. Each clay robot costs 4 ore. Each obsidian robot costs 4 ore and 14 clay. Each geode robot costs 3 ore and 16 obsidian.", 0)]
        [DataRow("Blueprint 23: Each ore robot costs 3 ore. Each clay robot costs 3 ore. Each obsidian robot costs 2 ore and 20 clay. Each geode robot costs 3 ore and 18 obsidian.", 0)]
        [DataRow("Blueprint 24: Each ore robot costs 2 ore. Each clay robot costs 2 ore. Each obsidian robot costs 2 ore and 20 clay. Each geode robot costs 2 ore and 14 obsidian.", 72)]
        [DataRow("Blueprint 25: Each ore robot costs 2 ore. Each clay robot costs 4 ore. Each obsidian robot costs 4 ore and 16 clay. Each geode robot costs 3 ore and 13 obsidian.", 50)]
        [DataRow("Blueprint 26: Each ore robot costs 3 ore. Each clay robot costs 3 ore. Each obsidian robot costs 3 ore and 9 clay. Each geode robot costs 2 ore and 10 obsidian.", 182)]
        [DataRow("Blueprint 27: Each ore robot costs 4 ore. Each clay robot costs 3 ore. Each obsidian robot costs 3 ore and 20 clay. Each geode robot costs 2 ore and 19 obsidian.", 0)]
        [DataRow("Blueprint 28: Each ore robot costs 3 ore. Each clay robot costs 3 ore. Each obsidian robot costs 3 ore and 15 clay. Each geode robot costs 2 ore and 8 obsidian.", 112)]
        [DataRow("Blueprint 29: Each ore robot costs 3 ore. Each clay robot costs 4 ore. Each obsidian robot costs 4 ore and 18 clay. Each geode robot costs 4 ore and 12 obsidian.", 0)]
        [DataRow("Blueprint 30: Each ore robot costs 2 ore. Each clay robot costs 2 ore. Each obsidian robot costs 2 ore and 10 clay. Each geode robot costs 2 ore and 11 obsidian.", 420)]
        [DataTestMethod]
        public void Minerals01Test(string input, int expected)
        {
            var bp = Parser.FromString<Day19.Blueprint>(input);

            Assert.AreEqual(expected, bp.Run(24).score);
        }

        [TestCategory("Test")]
        [DataRow("Blueprint 1: Each ore robot costs 4 ore. Each clay robot costs 2 ore. Each obsidian robot costs 3 ore and 14 clay. Each geode robot costs 2 ore and 7 obsidian.", 56)]
        [DataRow("Blueprint 2: Each ore robot costs 2 ore. Each clay robot costs 3 ore. Each obsidian robot costs 3 ore and 8 clay. Each geode robot costs 3 ore and 12 obsidian.", 62)]
        [DataTestMethod]
        public void Minerals02Test(string input, int expected)
        {
            var bp = Parser.FromString<Day19.Blueprint>(input);

            Assert.AreEqual(expected, bp.Run(32).geodes);
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Minerals_Part1_Regression()
        {
            Assert.AreEqual(1653, Day19.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Minerals_Part2_Regression()
        {
            Assert.AreEqual(4212, Day19.Part2(input));
        }
    }
}

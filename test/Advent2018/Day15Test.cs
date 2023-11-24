using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2018.Test
{
    [TestCategory("2018")]
    [TestCategory("BinarySearch")]
    [TestCategory("AStar")]
    [TestClass]
    public class Day15Test
    {
        readonly string input = Util.GetInput<Day15>();

        [TestCategory("Test")]
        [DataTestMethod]
        [DataRow("#######\n#.G...#\n#...EG#\n#.#.#G#\n#..G#E#\n#.....#\n#######", 27730)]
        [DataRow("#######\n#G..#E#\n#E#E.E#\n#G.##.#\n#...#E#\n#...E.#\n#######", 36334)]
        public void ElfAttack01(string layout, int expected)
        {
            Assert.AreEqual(expected, Day15.Part1(layout));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void ElfBattle_Part1_Regression()
        {
            Assert.AreEqual(225096, Day15.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void ElfBattle_Part2_Regression()
        {
            Assert.AreEqual(35354, Day15.Part2(input));
        }
    }
}

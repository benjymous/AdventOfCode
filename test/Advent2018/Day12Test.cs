using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2018.Test
{
    [TestCategory("2018")]
    [TestClass]
    public class Day12Test
    {
        readonly string input = Util.GetInput<Day12>();

        readonly string test = @"initial state: #..#.#..##......###...###

...## => #
..#.. => #
.#... => #
.#.#. => #
.#.## => #
.##.. => #
.#### => #
#.#.# => #
#.### => #
##.#. => #
##.## => #
###.. => #
###.# => #
####. => #".Replace("\r", "");

        [TestMethod]
        public void Plants01Test()
        {
            Assert.AreEqual(325, Day12.Part1(test));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void PlantPropgation_Part1_Regression()
        {
            Assert.AreEqual(2040, Day12.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void PlantPropgation_Part2_Regression()
        {
            Assert.AreEqual(1700000000011, Day12.Part2(input));
        }
    }
}

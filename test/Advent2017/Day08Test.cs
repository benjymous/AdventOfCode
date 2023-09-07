using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2017.Test
{
    [TestCategory("2017")]
    [TestClass]
    public class Day08Test
    {
        readonly string input = Util.GetInput<Day08>();
        readonly string testInput =
@"b inc 5 if a > 1
a inc 1 if b < 5
c dec -10 if a >= 1
c inc -20 if c == 10";

        [DataTestMethod]
        public void Registers01Test()
        {
            Assert.AreEqual(1, Day08.Part1(testInput));
        }

        [DataTestMethod]
        public void Registers02Test()
        {
            Assert.AreEqual(10, Day08.Part2(testInput));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Registers_Part1_Regression()
        {
            Assert.AreEqual(4647, Day08.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Registers_Part2_Regression()
        {
            Assert.AreEqual(5590, Day08.Part2(input));
        }

    }
}

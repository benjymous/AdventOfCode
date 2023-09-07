using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2022.Test
{
    [TestCategory("2022")]
    [TestCategory("Circle")]
    [TestClass]
    public class Day20Test
    {
        readonly string input = Util.GetInput<Day20>();
        readonly string test = "1\n2\n-3\n3\n-2\n0\n4";

        [TestCategory("Test")]
        [DataTestMethod]
        public void CircleShuffle01Test()
        {
            Assert.AreEqual(3, Day20.Part1(test));
        }

        [TestCategory("Test")]
        [DataTestMethod]
        public void CircleShuffle02Test()
        {
            Assert.AreEqual(1623178306, Day20.Part2(test));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void CircleShuffle_Part1_Regression()
        {
            Assert.AreEqual(27726, Day20.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void CircleShuffle_Part2_Regression()
        {
            Assert.AreEqual(4275451658004, Day20.Part2(input));
        }
    }
}

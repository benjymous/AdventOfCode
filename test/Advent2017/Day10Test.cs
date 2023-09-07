using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2017.Test
{
    [TestCategory("2017")]
    [TestCategory("Circle")]
    [TestClass]
    public class Day10Test
    {
        readonly string input = Util.GetInput<Day10>();

        [TestCategory("Test")]
        [DataRow("3, 4, 1, 5", 5, 12)]
        [DataTestMethod]
        public void KnotHash01Test(string input, int listSize, int expected)
        {
            Assert.AreEqual(expected, Day10.RunHash(listSize, Util.ParseNumbers<int>(input), 1).CheckSum());
        }

        [TestCategory("Test")]
        [DataRow("", "a2582a3a0e66e6e86e3812dcb672a272")]
        [DataRow("AoC 2017", "33efeb34ea91902bb2f59c9920caa6cd")]
        [DataRow("1,2,3", "3efbe78a8d82f29979031a4aa0b16a9d")]
        [DataRow("1,2,4", "63960835bcdc130f0b66d7ff4f6a5a8e")]

        [DataTestMethod]
        public void KnotHash02Test(string input, string expected)
        {
            Assert.AreEqual(expected, Day10.Part2(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void KnotHash_Part1_Regression()
        {
            Assert.AreEqual(23874, Day10.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void KnotHash_Part2_Regression()
        {
            Assert.AreEqual("e1a65bfb5a5ce396025fab5528c25a87", Day10.Part2(input));
        }

    }
}

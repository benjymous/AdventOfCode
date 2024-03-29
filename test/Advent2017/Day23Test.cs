using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2017.Test
{
    [TestCategory("2017")]
    [TestClass]
    public class Day23Test
    {
        readonly string input = Util.GetInput<Day23>();

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Coprocessor_Part1_Regression()
        {
            Assert.AreEqual(6724, Day23.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Coprocessor_Part2_Regression()
        {
            Assert.AreEqual(903, Day23.Part2(input));
        }
    }
}

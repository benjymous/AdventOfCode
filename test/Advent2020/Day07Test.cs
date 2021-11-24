using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2020.Test
{
    [TestCategory("2020")]
    [TestClass]
    public class Day07Test
    {
        string input = Util.GetInput<Day07>();

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Haversacks_Part1_Regression()
        {
            Assert.AreEqual(326, Day07.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Haversacks_Part2_Regression()
        {
            Assert.AreEqual(5635, Day07.Part2(input));
        }
    }
}

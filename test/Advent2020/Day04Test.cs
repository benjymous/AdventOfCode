using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2020.Test
{
    [TestCategory("2020")]
    [TestClass]
    public class Day04Test
    {
        string input = Util.GetInput<Day04>();

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Passports_Part1_Regression()
        {
            Assert.AreEqual(170, Day04.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Passports_Part2_Regression()
        {
            Assert.AreEqual(103, Day04.Part2(input));
        }
    }
}

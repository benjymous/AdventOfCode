using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2015.Test
{
    [TestCategory("2015")]
    [TestClass]
    public class Day11Test
    {
        readonly string input = Util.GetInput<Day11>();

        [TestCategory("Regression")]
        [DataTestMethod]
        public void PasswordPolicy_Part1_Regression()
        {
            Assert.AreEqual("hepxxyzz", Day11.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void PasswordPolicy_Part2_Regression()
        {
            Assert.AreEqual("heqaabcc", Day11.Part2(input));
        }
    }
}

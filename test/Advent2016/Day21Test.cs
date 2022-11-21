using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2016.Test
{
    [TestCategory("2016")]
    [TestCategory("RegexFactory")]
    [TestClass]
    public class Day21Test
    {
        readonly string input = Util.GetInput<Day21>();

        [TestCategory("Regression")]
        [DataTestMethod]
        public void PasswordScramble_Part1_Regression()
        {
            Assert.AreEqual("dbfgaehc", Day21.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void PasswordScramble_Part2_Regression()
        {
            Assert.AreEqual("aghfcdeb", Day21.Part2(input));
        }
    }
}

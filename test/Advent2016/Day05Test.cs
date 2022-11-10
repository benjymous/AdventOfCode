using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2016.Test
{
    [TestCategory("2016")]
    [TestCategory("Hashes")]
    [TestClass]
    public class Day05Test
    {
        readonly string input = Util.GetInput<Day05>();

        [TestCategory("Regression")]
        [DataTestMethod]
        public void PasswordHash_Part1_Regression()
        {
            Assert.AreEqual("1a3099aa", Day05.Part1(input, null));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void PasswordHash_Part2_Regression()
        {
            Assert.AreEqual("694190cd", Day05.Part2(input, null));
        }
    }
}

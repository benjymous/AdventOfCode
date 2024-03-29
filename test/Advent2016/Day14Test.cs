using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2016.Test
{
    [TestCategory("2016")]
    [TestCategory("Hashes")]
    [TestClass]
    public class Day14Test
    {
        readonly string input = Util.GetInput<Day14>();

        [TestCategory("Test")]
        [DataTestMethod]
        public void KeyHash01_Test()
        {
            Assert.AreEqual(22728, Day14.Part1("abc"));
        }

        [TestCategory("Test")]
        [DataTestMethod]
        public void KeyHash02_Test()
        {
            //Assert.AreEqual(22551, Day14.Part2("abc"));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void KeyHash_Part1_Regression()
        {
            Assert.AreEqual(15168, Day14.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void KeyHash_Part2_Regression()
        {
            //Assert.AreEqual(20864, Day14.Part2(input));
        }
    }
}

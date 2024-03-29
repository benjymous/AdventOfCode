using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2016.Test
{
    [TestCategory("2016")]
    [TestClass]
    public class Day07Test
    {
        readonly string input = Util.GetInput<Day07>();

        [TestCategory("Regression")]
        [DataTestMethod]
        public void IP7_Part1_Regression()
        {
            Assert.AreEqual(105, Day07.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void IP7_Part2_Regression()
        {
            Assert.AreEqual(258, Day07.Part2(input));
        }
    }
}

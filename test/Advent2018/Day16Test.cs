using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2018.Test
{
    [TestCategory("2018")]
    [TestCategory("ChronCPU")]
    [TestClass]
    public class Day16Test
    {
        readonly string input = Util.GetInput<Day16>();


        [TestCategory("Regression")]
        [DataTestMethod]
        public void ReverseEngineer_Part1_Regression()
        {
            Assert.AreEqual(663, Day16.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void ReverseEngineer_Part2_Regression()
        {
            Assert.AreEqual(525, Day16.Part2(input, new ConsoleOut()));
        }

    }
}

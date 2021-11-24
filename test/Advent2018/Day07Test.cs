using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2018.Test
{
    [TestCategory("2018")]
    [TestClass]
    public class Day07Test
    {
        string input = Util.GetInput<Day07>();

        [TestCategory("Regression")]
        [DataTestMethod]
        public void AssemblyLine_Part1_Regression()
        {
            Assert.AreEqual("OKBNLPHCSVWAIRDGUZEFMXYTJQ", Day07.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void AssemblyLine_Part2_Regression()
        {
            Assert.AreEqual(982, Day07.Part2(input));
        }

    }
}

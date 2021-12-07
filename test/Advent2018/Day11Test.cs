using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2018.Test
{
    [TestCategory("2018")]
    [TestCategory("ManhattanVector")]
    [TestClass]
    public class Day11Test
    {
        string input = Util.GetInput<Day11>();

        [TestCategory("Test")]
        [DataRow(8, 3, 5, 4)] // Fuel cell at 3,5, grid with serial number 8: power level 4
        [DataRow(57, 122, 79, -5)] // Fuel cell at  122,79, grid serial number 57: power level -5
        [DataRow(57, 122, 79, -5)] // Fuel cell at 217,196, grid serial number 39: power level  0
        [DataRow(57, 122, 79, -5)] // Fuel cell at 101,153, grid serial number 71: power level  4
        [DataTestMethod]
        public void PowerLevelTest(int serial, int x, int y, int expected)
        {
            Assert.AreEqual(expected, Advent2018.Day11.Power(serial, x, y));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void PowerGrid_Part1_Regression()
        {
            Assert.AreEqual("243,17", Day11.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void PowerGrid_Part2_Regression()
        {
            Assert.AreEqual("233,228,12", Day11.Part2(input));
        }

    }
}

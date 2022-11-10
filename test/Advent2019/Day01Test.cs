using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2019.Test
{
    [TestCategory("2019")]
    [TestClass]
    public class Day01Test
    {
        readonly string input = Util.GetInput<Day01>();

        [TestCategory("Test")]
        [DataRow(12, 2)]
        [DataRow(14, 2)]
        [DataRow(1969, 654)]
        [DataRow(100756, 33583)]
        [DataRow(1, 0)]
        [DataTestMethod]
        public void Fuel01Test(int input, int expected)
        {
            Assert.IsTrue(Advent2019.Day01.GetFuelRequirement(input) == expected);
        }

        [TestCategory("Test")]
        [DataRow(14, 2)]
        [DataRow(1969, 966)]
        [DataRow(100756, 50346)]
        [DataTestMethod]
        public void Fuel02Test(int input, int expected)
        {
            Assert.IsTrue(Advent2019.Day01.GetFullFuelRequirement(input) == expected);
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void FuelCheck_Part1_Regression()
        {
            Assert.AreEqual(3224048, Day01.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void FuelCheck_Part2_Regression()
        {
            Assert.AreEqual(4833211, Day01.Part2(input));
        }
    }
}

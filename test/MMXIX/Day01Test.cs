using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace Advent.MMXIX.Test
{
    [TestClass]
    public class Day01Test
    {
        [DataRow(12, 2)]
        [DataRow(14, 2)]
        [DataRow(1969, 654)]
        [DataRow(100756, 33583)]
        [DataRow(1, 0)]
        [DataTestMethod]
        public void Fuel01Test(int input, int expected)
        {
            Assert.IsTrue(MMXIX.Day01.GetFuelRequirement(input) == expected);
        }

        [DataRow(14, 2)]
        [DataRow(1969, 966)]
        [DataRow(100756, 50346)]
        [DataTestMethod]
        public void Fuel02Test(int input, int expected)
        {
            Assert.IsTrue(MMXIX.Day01.GetFullFuelRequirement(input) == expected);
        }

        [DataTestMethod]
        public void FuelCheck_Part1_Regression()
        {
            var d = new Day01();
            var input = Util.GetInput(d);
            Assert.AreEqual(3224048, d.FuelCheck01(input));
        }

        [DataTestMethod]
        public void FuelCheck_Part2_Regression()
        {
            var d = new Day01();
            var input = Util.GetInput(d);
            Assert.AreEqual(4833211, d.FuelCheck02(input));
        }
    }
}

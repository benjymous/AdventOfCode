using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace Advent.MMXVIII.Test
{
    [TestClass]
    public class Day03Test
    {
        [DataRow("#1 @ 1,3: 4x4\n#2 @ 3,1: 4x4\n#3 @ 5,5: 2x2\n", 4)]
        [DataTestMethod]
        public void Inventory01Test(string input, int expected)
        {
            Assert.AreEqual(expected, MMXVIII.Day03.Part1(input));
        }

        [DataRow("#1 @ 1,3: 4x4\n#2 @ 3,1: 4x4\n#3 @ 5,5: 2x2\n", "3")]
        [DataTestMethod]
        public void Inventory02Test(string input, string expected)
        {
            Assert.AreEqual(expected, MMXVIII.Day03.Part2(input));
        }

        [DataTestMethod]
        public void Inventory_Part1_Regression()
        {
            var d = new Day03();
            var input = Util.GetInput(d);
            Assert.AreEqual(121259, Day03.Part1(input));
        }

        [DataTestMethod]
        public void Inventory_Part2_Regression()
        {
            var d = new Day03();
            var input = Util.GetInput(d);
            Assert.AreEqual("239", Day03.Part2(input));
        }

    }
}

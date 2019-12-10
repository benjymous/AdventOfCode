using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace Advent.MMXV.Test
{
    [TestCategory("2015")]
    [TestClass]
    public class Day08Test
    {
        string input = Util.GetInput<Day08>();

        [DataRow("\"\"", "")]
        [DataRow("\"abc\"", "abc")]
        [DataRow("\"aaa\\\"aaa\"", "aaa\"aaa")]
        [DataRow("\"\\x27\"", "'")]
        [DataTestMethod]
        public void EscapeTest01(string input, string expected)
        {
            Assert.AreEqual(expected, Day08.Unescape(input));
        }

        // [DataRow("3,26,1001,26,-4,26,3,27,1002,27,2,27,1,27,26,27,4,27,1001,28,-1,28,1005,28,6,99,0,0,5", 139629729)]
        // [DataRow("3,52,1001,52,-5,52,3,53,1,52,56,54,1007,54,5,55,1005,55,26,1001,54,-5,54,1105,1,12,1,53,54,53,1008,54,0,55,1001,55,1,55,2,53,55,53,4,53,1001,56,-1,56,1005,56,6,99,0,0,0,0,10",18216)]
        // [DataTestMethod]
        // public void AmpTest02(string input, int expected)
        // {
        //     Assert.AreEqual(expected, Day07.Part2(input));
        // }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Image_Part1_Regression()
        {
            Assert.AreEqual(0, Day08.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Image_Part2_Regression()
        {     
            Assert.AreEqual(0, Day08.Part2(input));
        }

    }
}

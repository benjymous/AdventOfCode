using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace Advent.MMXVIII.Test
{
    [TestClass]
    public class Day25Test
    {
        [DataRow("0,0,0,0\n3,0,0,0\n0,3,0,0\n0,0,3,0\n0,0,0,3\n0,0,0,6\n9,0,0,0\n12,0,0,0\n", 2)]
        [DataRow("-1,2,2,0\n0,0,2,-2\n0,0,0,-2\n-1,2,0,0\n-2,-2,-2,2\n3,0,2,-1\n-1,3,2,2\n-1,0,-1,0\n0,2,1,-2\n3,0,0,0", 4)]
        [DataRow("1,-1,0,1\n2,0,-1,0\n3,2,-1,0\n0,0,3,1\n0,0,-1,-1\n2,3,-2,0\n-2,2,0,0\n2,-2,0,-1\n1,-1,0,-1\n3,2,0,2", 3)]
        [DataRow("1,-1,-1,-2\n-2,-2,0,1\n0,2,1,3\n-2,3,-2,1\n0,2,3,-2\n-1,-1,1,-2\n0,-2,-1,0\n-2,2,3,-1\n1,2,2,0\n-1,-2,0,-2\n", 8)]
        [DataTestMethod]
        public void Constellation01Test(string input, int expected)
        {
            Assert.AreEqual(expected, MMXVIII.Day25.Part1(input));
        }


        [DataTestMethod]
        public void Constellation_Part1_Regression()
        {
            var d = new Day25();
            var input = Util.GetInput(d);
            Assert.AreEqual(381, MMXVIII.Day25.Part1(input));
        }

        // [DataTestMethod]
        // public void Constellation_Part2_Regression()
        // {
        //     var d = new Day25();
        //     var input = Util.GetInput(d);
        //     Assert.AreEqual(0, MMXVIII.Day25.Part2(input));
        // }

    }
}

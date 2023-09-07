using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2018.Test
{
    [TestCategory("2018")]
    [TestClass]
    public class Day25Test
    {
        readonly string input = Util.GetInput<Day25>();

        [TestCategory("Test")]
        [DataRow("0,0,0,0\n3,0,0,0\n0,3,0,0\n0,0,3,0\n0,0,0,3\n0,0,0,6\n9,0,0,0\n12,0,0,0\n", 2)]
        [DataRow("-1,2,2,0\n0,0,2,-2\n0,0,0,-2\n-1,2,0,0\n-2,-2,-2,2\n3,0,2,-1\n-1,3,2,2\n-1,0,-1,0\n0,2,1,-2\n3,0,0,0", 4)]
        [DataRow("1,-1,0,1\n2,0,-1,0\n3,2,-1,0\n0,0,3,1\n0,0,-1,-1\n2,3,-2,0\n-2,2,0,0\n2,-2,0,-1\n1,-1,0,-1\n3,2,0,2", 3)]
        [DataRow("1,-1,-1,-2\n-2,-2,0,1\n0,2,1,3\n-2,3,-2,1\n0,2,3,-2\n-1,-1,1,-2\n0,-2,-1,0\n-2,2,3,-1\n1,2,2,0\n-1,-2,0,-2\n", 8)]
        [DataTestMethod]
        public void Constellation01Test(string input, int expected)
        {
            Assert.AreEqual(expected, Day25.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Constellation_Part1_Regression()
        {
            Assert.AreEqual(381, Day25.Part1(input));
        }

    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace Advent.MMXVIII.Test
{
    [TestClass]
    public class Day22Test
    {
        [DataRow(0,0, 10,10, 510, 0)]
        [DataRow(1,0, 10,10, 510, 16807)]
        [DataRow(0,1, 10,10, 510, 48271)]
        [DataRow(1,1, 10,10, 510, 145722555)]
        [DataRow(10,10, 10,10, 510, 0)]
        [DataTestMethod]
        public void GeologicIndexTest(int x, int y, int tx, int ty, int depth, int expected)
        {
            var c = new Day22.Cave(tx, ty, depth);
            Assert.AreEqual(expected, c.GeologicIndex(x,y));
        }

        [DataRow(0,0, 10,10, 510, 510)]
        [DataRow(1,0, 10,10, 510, 17317)]
        [DataRow(0,1, 10,10, 510, 8415)]
        [DataRow(1,1, 10,10, 510, 1805)]
        [DataRow(10,10, 10,10, 510, 510)]
        [DataTestMethod]
        public void ErosionLevelTest(int x, int y, int tx, int ty, int depth, int expected)
        {
            var c = new Day22.Cave(tx, ty, depth);
            Assert.AreEqual(expected, c.ErosionLevel(x,y));
        }

        [DataRow(0,0, 10,10, 510, Day22.ROCKY)]
        [DataRow(1,0, 10,10, 510, Day22.WET)]
        [DataRow(0,1, 10,10, 510, Day22.ROCKY)]
        [DataRow(1,1, 10,10, 510, Day22.NARROW)]
        [DataRow(10,10, 10,10, 510, Day22.ROCKY)]
        [DataTestMethod]
        public void TypeTest(int x, int y, int tx, int ty, int depth, char expected)
        {
            var c = new Day22.Cave(tx, ty, depth);
            Assert.AreEqual(expected, c.TypeChar(x,y));
        }

        [DataRow(10,10, 510, 114)]
        [DataTestMethod]
        public void Cave01Test(int tx, int ty, int depth, int expected)
        {
            Assert.AreEqual(expected, Day22.Part1(tx, ty, depth));
        }

        // [DataRow("+1", 1)]
        // [DataRow("+1\n+2\n", 3)]
        // [DataRow("+1, -2, +3, +1", 3)]
        // [DataRow("+1, +1, +1", 3)]
        // [DataRow("+1, +1, -2", 0)]
        // [DataRow("-1, -2, -3", -6)]
        // [DataTestMethod]
        // public void Callibrate01Test(string input, int expected)
        // {
        //     Assert.AreEqual(expected, MMXVIII.Day01.Part1(input));
        // }

        // [DataRow("+1, -1", 0)]
        // [DataRow("+3, +3, +4, -2, -4", 10)]
        // [DataRow("-6, +3, +8, +5, -6", 5)]
        // [DataRow("+7, +7, -2, -7, -4", 14)]
        // [DataTestMethod]
        // public void Callibrate02Test(string input, int expected)
        // {
        //     Assert.AreEqual(expected, MMXVIII.Day01.Part2(input));
        // }

        [DataTestMethod]
        public void Caves_Part1_Regression()
        {
            var d = new Day22();
            var input = Util.GetInput(d);
            Assert.AreEqual(7299, MMXVIII.Day22.Part1(input));
        }

        // [DataTestMethod]
        // public void Caves_Part2_Regression()
        // {
        //     var d = new Day22();
        //     var input = Util.GetInput(d);
        //     Assert.AreEqual(0, MMXVIII.Day22.Part2(input));
        // }

    }
}

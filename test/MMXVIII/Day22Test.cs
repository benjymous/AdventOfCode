using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace Advent.MMXVIII.Test
{
    [TestClass]
    public class Day22Test
    {
        string input = Util.GetInput<Day22>();

        [TestCategory("Test")]
        [DataRow(0,0, 10,10, 510, 0)]
        [DataRow(1,0, 10,10, 510, 16807)]
        [DataRow(0,1, 10,10, 510, 48271)]
        [DataRow(1,1, 10,10, 510, 145722555)]
        [DataRow(10,10, 10,10, 510, 0)]
        [DataTestMethod]
        public void GeologicIndexTest(int x, int y, int tx, int ty, int depth, int expected)
        {
            var c = new Day22.Cave(tx, ty, depth);
            Assert.AreEqual(expected, c.GeologicIndex(new ManhattanVector2(x,y)));
        }

        [TestCategory("Test")]
        [DataRow(0,0, 10,10, 510, 510)]
        [DataRow(1,0, 10,10, 510, 17317)]
        [DataRow(0,1, 10,10, 510, 8415)]
        [DataRow(1,1, 10,10, 510, 1805)]
        [DataRow(10,10, 10,10, 510, 510)]
        [DataTestMethod]
        public void ErosionLevelTest(int x, int y, int tx, int ty, int depth, int expected)
        {
            var c = new Day22.Cave(tx, ty, depth);
            Assert.AreEqual(expected, c.ErosionLevel(new ManhattanVector2(x,y)));
        }

        [TestCategory("Test")]
        [DataRow(0,0, 10,10, 510, Day22.ROCKY)]
        [DataRow(1,0, 10,10, 510, Day22.WET)]
        [DataRow(0,1, 10,10, 510, Day22.ROCKY)]
        [DataRow(1,1, 10,10, 510, Day22.NARROW)]
        [DataRow(10,10, 10,10, 510, Day22.ROCKY)]
        [DataTestMethod]
        public void TypeTest(int x, int y, int tx, int ty, int depth, char expected)
        {
            var c = new Day22.Cave(tx, ty, depth);
            Assert.AreEqual(expected, c.TypeChar(new ManhattanVector2(x,y)));
        }

        [TestCategory("Test")]
        [DataRow(10,10, 510, 114)]
        [DataTestMethod]
        public void Cave01Test(int tx, int ty, int depth, int expected)
        {
            Assert.AreEqual(expected, Day22.Part1(tx, ty, depth));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Caves_Part1_Regression()
        {
            Assert.AreEqual(7299, MMXVIII.Day22.Part1(input));
        }

        // [TestCategory("Regression")]
        // [DataTestMethod]
        // public void Caves_Part2_Regression()
        // {
        //     Assert.AreEqual(0, MMXVIII.Day22.Part2(input));
        // }

    }
}

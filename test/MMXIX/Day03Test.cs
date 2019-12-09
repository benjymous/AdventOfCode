using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace Advent.MMXIX.Test
{
    [TestClass]
    public class Day03Test
    {
        string input = Util.GetInput<Day03>();

        [TestCategory("Test")]
        [DataRow("R8,U5,L5,D3\nU7,R6,D4,L4", 6)]
        [DataRow("R75,D30,R83,U83,L12,D49,R71,U7,L72\nU62,R66,U55,R34,D71,R55,D58,R83", 159)]
        [DataRow("R98,U47,R26,D63,R33,U87,L62,D20,R33,U53,R51\nU98,R91,D20,R16,D67,R40,U7,R15,U6,R7", 135)]
        [DataTestMethod]
        public void WireTestClosest(string input, int expected)
        {
            Assert.AreEqual(expected, Day03.FindIntersection(input, Day03.SearchMode.Closest));
        }

        [TestCategory("Test")]
        [DataRow("R8,U5,L5,D3\nU7,R6,D4,L4", 30)]
        [DataRow("R75,D30,R83,U83,L12,D49,R71,U7,L72\nU62,R66,U55,R34,D71,R55,D58,R83", 610)]
        [DataRow("R98,U47,R26,D63,R33,U87,L62,D20,R33,U53,R51\nU98,R91,D20,R16,D67,R40,U7,R15,U6,R7", 410)]
        [DataTestMethod]
        public void WireTestShortest(string input, int expected)
        {
            Assert.AreEqual(expected, Day03.FindIntersection(input, Day03.SearchMode.Shortest));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Wire_Part1_Regression()
        {
            Assert.AreEqual(293, Day03.FindIntersection(input, Day03.SearchMode.Closest));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Wire_Part2_Regression()
        {
            Assert.AreEqual(27306, Day03.FindIntersection(input, Day03.SearchMode.Shortest));
        }

    }
}

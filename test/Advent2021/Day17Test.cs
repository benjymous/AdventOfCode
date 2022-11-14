using Microsoft.VisualStudio.TestTools.UnitTesting;
using static AoC.Advent2021.Day17;

namespace AoC.Advent2021.Test
{
    [TestCategory("2021")]
    [TestClass]
    public class Day17Test
    {
        readonly string input = Util.GetInput<Day17>();

        [TestCategory("Test")]
        [DataRow("target area: x=20..30, y=-10..-5", 20, -10, true)]
        [DataRow("target area: x=20..30, y=-10..-5", 25, -7, true)]
        [DataRow("target area: x=20..30, y=-10..-5", 30, -5, true)]
        [DataRow("target area: x=20..30, y=-10..-5", 31, -10, false)]
        [DataTestMethod]
        public void TestInRect(string data, int x, int y, bool expected)
        {
            Assert.AreEqual(expected, TestInRect(data, x, y));
        }

        public static bool TestInRect(string input, int x, int y)
        {
            var rect = TargetRect.Create(input);
            return rect.Contains((x, y), false);
        }

        [TestCategory("Test")]
        [DataRow("target area: x=20..30, y=-10..-5", 7, 2, true)]
        [DataRow("target area: x=20..30, y=-10..-5", 6, 3, true)]
        [DataRow("target area: x=20..30, y=-10..-5", 9, 0, true)]
        [DataRow("target area: x=20..30, y=-10..-5", 17, -4, false)]
        [DataRow("target area: x=20..30, y=-10..-5", 0, 0, false)]
        [DataTestMethod]
        public void TestHits(string data, int x, int y, bool expected)
        {
            Assert.AreEqual(expected, TestHits(data, x, y));
        }

        public static bool TestHits(string input, int dx, int dy)
        {
            var target = TargetRect.Create(input);

            return TestShot(target, (dx, dy), false).hit;
        }

        [TestCategory("Test")]
        [DataRow("target area: x=20..30, y=-10..-5", 45)]
        [DataTestMethod]
        public void TestPart1(string data, int expected)
        {
            Assert.AreEqual(expected, Day17.Part1(data));
        }

        [TestCategory("Test")]
        [DataRow("target area: x=20..30, y=-10..-5", 112)]
        [DataTestMethod]
        public void TestPart2(string data, int expected)
        {
            Assert.AreEqual(expected, Day17.Part2(data));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Trajectory_Part1_Regression()
        {
            Assert.AreEqual(5565, Day17.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Trajectory_Part2_Regression()
        {
            Assert.AreEqual(2118, Day17.Part2(input));
        }
    }
}

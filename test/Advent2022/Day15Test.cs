using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2022.Test
{
    [TestCategory("2022")]
    [TestClass]
    public class Day15Test
    {
        readonly string input = Util.GetInput<Day15>();
        readonly string test = @"Sensor at x=2, y=18: closest beacon is at x=-2, y=15
Sensor at x=9, y=16: closest beacon is at x=10, y=16
Sensor at x=13, y=2: closest beacon is at x=15, y=3
Sensor at x=12, y=14: closest beacon is at x=10, y=16
Sensor at x=10, y=20: closest beacon is at x=10, y=16
Sensor at x=14, y=17: closest beacon is at x=10, y=16
Sensor at x=8, y=7: closest beacon is at x=2, y=10
Sensor at x=2, y=0: closest beacon is at x=2, y=10
Sensor at x=0, y=11: closest beacon is at x=2, y=10
Sensor at x=20, y=14: closest beacon is at x=25, y=17
Sensor at x=17, y=20: closest beacon is at x=21, y=22
Sensor at x=16, y=7: closest beacon is at x=15, y=3
Sensor at x=14, y=3: closest beacon is at x=15, y=3
Sensor at x=20, y=1: closest beacon is at x=15, y=3".Replace("\r", "");

        [DataTestMethod]
        public void Beacons01Test()
        {
            Assert.AreEqual(26, Day15.SolvePart1(test, 10));
        }

        [DataTestMethod]
        public void Beacons02Test()
        {
            Assert.AreEqual("56000011", Day15.SolvePart2(test, 20).ToString());
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Beacons_Part1_Regression()
        {
            Assert.AreEqual(5607466, Day15.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Beacons_Part2_Regression()
        {
            Assert.AreEqual("12543202766584", Day15.Part2(input).ToString());
        }
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2019.Test
{
    [TestCategory("2019")]
    [TestCategory("Tree")]
    [TestClass]
    public class Day06Test
    {
        readonly string input = Util.GetInput<Day06>();

        [TestCategory("Test")]
        [DataRow("COM)B\nB)C\nC)D\nD)E\nE)F\nB)G\nG)H\nD)I\nE)J\nJ)K\nK)L", "L", 0)]
        [DataRow("COM)B\nB)C\nC)D\nD)E\nE)F\nB)G\nG)H\nD)I\nE)J\nJ)K\nK)L", "K", 1)]
        [DataRow("COM)B\nB)C\nC)D\nD)E\nE)F\nB)G\nG)H\nD)I\nE)J\nJ)K\nK)L", "J", 2)]
        [DataRow("COM)B\nB)C\nC)D\nD)E\nE)F\nB)G\nG)H\nD)I\nE)J\nJ)K\nK)L", "E", 4)]
        [DataRow("COM)B\nB)C\nC)D\nD)E\nE)F\nB)G\nG)H\nD)I\nE)J\nJ)K\nK)L", "COM", 11)]
        [DataTestMethod]
        public void DescendentsTest(string input, string root, int expected)
        {
            var t = Day06.ParseTree(input);
            var count = t[root].GetDescendantCount();
            Assert.AreEqual(expected, count);
        }

        [TestCategory("Test")]
        [DataRow("COM)B\nB)C\nC)D\nD)E\nE)F\nB)G\nG)H\nD)I\nE)J\nJ)K\nK)L", 42)]
        [DataTestMethod]
        public void AllDescendentsTest(string input, int expected)
        {
            Assert.AreEqual(expected, Day06.Part1(input));
        }

        [TestCategory("Test")]
        [DataRow("COM)B\nB)C\nC)D\nD)E\nE)F\nB)G\nG)H\nD)I\nE)J\nJ)K\nK)L\nK)YOU\nI)SAN", 4)]
        [DataTestMethod]
        public void TraversalTest(string input, int expected)
        {
            Assert.AreEqual(expected, Day06.Part2(input));
        }

        // [DataRow("112233", true)]
        // [DataRow("123444", false)]
        // [DataRow("111122", true)]
        // [DataTestMethod]
        // public void SecureTest02(string input, bool expected)
        // {
        //     Assert.AreEqual(expected, Day04.CheckCriteria(input, true));
        // }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Orbital_Part1_Regression()
        {
            Assert.AreEqual(147223, Day06.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Orbital_Part2_Regression()
        {
            Assert.AreEqual(340, Day06.Part2(input));
        }

    }
}

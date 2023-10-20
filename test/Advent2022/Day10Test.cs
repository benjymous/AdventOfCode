using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2022.Test
{
    [TestCategory("2022")]
    [TestCategory("RegexFactory")]
    [TestClass]
    public class Day10Test
    {
        readonly string input = Util.GetInput<Day10>();

        readonly string test = @"addx 15
addx -11
addx 6
addx -3
addx 5
addx -1
addx -8
addx 13
addx 4
noop
addx -1
addx 5
addx -1
addx 5
addx -1
addx 5
addx -1
addx 5
addx -1
addx -35
addx 1
addx 24
addx -19
addx 1
addx 16
addx -11
noop
noop
addx 21
addx -15
noop
noop
addx -3
addx 9
addx 1
addx -3
addx 8
addx 1
addx 5
noop
noop
noop
noop
noop
addx -36
noop
addx 1
addx 7
noop
noop
noop
addx 2
addx 6
noop
noop
noop
noop
noop
addx 1
noop
noop
addx 7
addx 1
noop
addx -13
addx 13
addx 7
noop
addx 1
addx -33
noop
noop
noop
addx 2
noop
noop
noop
addx 8
noop
addx -1
addx 2
addx 1
noop
addx 17
addx -9
addx 1
addx 1
addx -3
addx 11
noop
noop
addx 1
noop
addx 1
noop
noop
addx -13
addx -19
addx 1
addx 3
addx 26
addx -30
addx 12
addx -1
addx 3
addx 1
noop
noop
noop
addx -9
addx 18
addx 1
addx 2
noop
noop
addx 9
noop
noop
noop
addx -1
addx 2
addx -37
addx 1
addx 3
noop
addx 15
addx -21
addx 22
addx -6
addx 1
noop
addx 2
addx 1
noop
addx -10
noop
noop
addx 20
addx 1
addx 2
addx 2
addx -6
addx -11
noop
noop
noop".Replace("\r", "");

        [TestCategory("Test")]
        [DataTestMethod]
        public void CRT01Test()
        {
            Assert.AreEqual(13140, Day10.Part1(test));
        }

        [TestCategory("Test")]
        [DataTestMethod]
        public void CRT02Test()
        {
            Assert.AreEqual("BA8737FF1F5B748145E3243169BF9978", Day10.Part2(test, new ConsoleOut()));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void CRT_Part1_Regression()
        {
            Assert.AreEqual(13860, Day10.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void CRT_Part2_Regression()
        {
            Assert.AreEqual("6DBA86D87D1C42C2DAE02CB537DA78C4", Day10.Part2(input, new ConsoleOut()));
        }
    }
}

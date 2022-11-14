using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2021.Test
{
    [TestCategory("2021")]
    [TestClass]
    public class Day23Test
    {
        readonly string input = Util.GetInput<Day23>();

        const string test1 = @"#############
#...........#
###B#C#B#D###
  #A#D#C#A#
  #########";

        const string test2 = @"#############
#...........#
###B#C#B#D###
  #D#C#B#A#
  #D#B#A#C#
  #A#D#C#A#
  #########";


        [TestCategory("Test")]
        [DataTestMethod]
        [DataRow(test1, 12521)]
        public void ShrimpTest1(string input, int expected)
        {
            Assert.AreEqual(expected, Day23.ShrimpStacker(input.Replace("\r", "").Split('\n')));
        }

        [TestCategory("Test")]
        [DataTestMethod]
        [DataRow(test2, 44169)]
        public void ShrimpTest2(string input, int expected)
        {
            Assert.AreEqual(expected, Day23.ShrimpStacker(input.Replace("\r", "").Split('\n')));
        }


        [TestCategory("Regression")]
        [DataTestMethod]
        public void Shrimp_Part1_Regression()
        {
            Assert.AreEqual(14371, Day23.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Shrimp_Part2_Regression()
        {
            Assert.AreEqual(40941, Day23.Part2(input));
        }
    }
}

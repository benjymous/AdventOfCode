using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2021.Test
{
    [TestCategory("2021")]
    [TestCategory("PackedVect")]
    [TestCategory("Solver")]
    [TestClass]
    public class Day23Test
    {
        readonly string input = Util.GetInput<Day23>();

        [TestCategory("Test")]
        [TestMethod]
        public void ShrimpTest1()
        {
            const string test1 =
            @"#############
#...........#
###B#C#B#D###
  #A#D#C#A#
  #########";
            Assert.AreEqual(12521, Day23.ShrimpStacker(test1.Replace("\r", "").Split('\n')));
        }

        [TestCategory("Test")]
        [TestMethod]
        public void ShrimpTest2()
        {
            const string test2 =
@"#############
#...........#
###B#C#B#D###
  #D#C#B#A#
  #D#B#A#C#
  #A#D#C#A#
  #########";
            Assert.AreEqual(44169, Day23.ShrimpStacker(test2.Replace("\r", "").Split('\n')));
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

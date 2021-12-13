using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2021.Test
{
    [TestCategory("2021")]
    [TestClass]
    public class Day13Test
    {
        string input = Util.GetInput<Day13>();

        string test = @"6,10
0,14
9,10
0,3
10,4
4,11
6,0
6,12
4,1
0,13
10,12
3,4
3,0
8,4
1,10
2,14
8,10
9,0

fold along y=7
fold along x=5";

        [TestCategory("Test")]
        [DataTestMethod]
        public void Origami01Test()
        {
            Assert.AreEqual(17, Day13.Part1(test.Replace("\r", "")));
        }

        [TestCategory("Test")]
        [DataTestMethod]
        public void Origami02Test()
        {
            Assert.AreEqual("C5BD8018ED6D44DC4DBA14BC4A6CE32B", Day13.Part2(test.Replace("\r", "")));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Origami_Part1_Regression()
        {
            Assert.AreEqual(710, Day13.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Origami_Part2_Regression()
        {
            Assert.AreEqual("1A98F1E8E8399775E1991961C2990153", Day13.Part2(input));
        }
    }
}

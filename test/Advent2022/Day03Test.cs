using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2022.Test
{
    [TestCategory("2022")]
    [TestClass]
    public class Day03Test
    {

        readonly string input = Util.GetInput<Day03>();

        //[TestCategory("Test")]
        //[DataRow("???", 0)]
        //[DataTestMethod]
        //public void XXX01Test(string input, int expected)
        //{
        //    Assert.IsTrue(Day--.Part1(input) == expected);
        //}

        //[TestCategory("Test")]
        //[DataRow("???", 0)]
        //[DataTestMethod]
        //public void XXX02Test(string input, int expected)
        //{
        //    Assert.IsTrue(Day--.Part2(input) == expected);
        //}

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Rucksacks_Part1_Regression()
        {
            Assert.AreEqual(8039, Day03.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Rucksacks_Part2_Regression()
        {
            Assert.AreEqual(2510, Day03.Part2(input));
        }
    }
}

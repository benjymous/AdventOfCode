using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Advent.MMXX.Test
{
    [TestCategory("2020")]
    [TestClass]
    public class Day20Test
    {
        string input = Util.GetInput<Day20>();

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Jigsaw_Part1_Regression()
        {
            Assert.AreEqual(8581320593371L, Day20.Part1(input));
        }

        //[TestCategory("Regression")]
        //[DataTestMethod]
        //public void _Part2_Regression()
        //{
        //    Assert.AreEqual(, Day20.Part2(input));
        //}
    }
}

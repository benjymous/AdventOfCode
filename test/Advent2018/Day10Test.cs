using AoC.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2018.Test
{
    [TestCategory("2018")]
    [TestClass]
    public class Day10Test
    {
        string input = Util.GetInput<Day10>();

        [TestCategory("Regression")]
        [DataTestMethod]
        public void StarSigns_Part1_Regression()
        {
            Assert.AreEqual("12BE5B2645AD2CFDA4AA7A90A0C7328E", Day10.Part1(input).GetMD5String());
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void StarSigns_Part2_Regression()
        {
            Assert.AreEqual(10009, Day10.Part2(input));
        }

    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2016.Test
{
    [TestCategory("2016")]
    [TestClass]
    public class Day02Test
    {
        readonly string input = Util.GetInput<Day02>();

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Keypad_Part1_Regression()
        {
            Assert.AreEqual("53255", Day02.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Keypad_Part2_Regression()
        {
            Assert.AreEqual("7423A", Day02.Part2(input));
        }
    }
}

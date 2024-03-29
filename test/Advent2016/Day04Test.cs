using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2016.Test
{
    [TestCategory("2016")]
    [TestClass]
    public class Day04Test
    {
        readonly string input = Util.GetInput<Day04>();

        [TestCategory("Regression")]
        [DataTestMethod]
        public void EncryptedRooms_Part1_Regression()
        {
            Assert.AreEqual(361724, Day04.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void EncryptedRooms_Part2_Regression()
        {
            Assert.AreEqual(482, Day04.Part2(input));
        }
    }
}

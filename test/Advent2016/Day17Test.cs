using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2016.Test
{
    [TestCategory("2016")]
    [TestClass]
    public class Day17Test
    {
        readonly string input = Util.GetInput<Day17>();

        [TestCategory("Test")]
        [DataRow("ihgpwlah", "DDRRRD")]
        [DataRow("kglvqrro", "DDUDRLRRUDRD")]
        [DataRow("ulqzkmiv", "DRURDRUDDLLDLUURRDULRLDUUDDDRR")]
        [DataTestMethod]
        public void Doors01Test(string input, string expected)
        {
            Assert.AreEqual(expected, Day17.Part1(input));
        }

        [TestCategory("Test")]
        [DataRow("ihgpwlah", 370)]
        [DataRow("kglvqrro", 492)]
        [DataRow("ulqzkmiv", 830)]
        [DataTestMethod]
        public void Doors02Test(string input, int expected)
        {
            Assert.AreEqual(expected, Day17.Part2(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void VaultDoors_Part1_Regression()
        {
            Assert.AreEqual("RDRRULDDDR", Day17.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void VaultDoors_Part2_Regression()
        {
            Assert.AreEqual(392, Day17.Part2(input));
        }
    }
}

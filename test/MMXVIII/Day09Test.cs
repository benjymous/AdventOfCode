using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Advent.MMXVIII.Test
{
    [TestCategory("2018")]
    [TestClass]
    public class Day09Test
    {
        string input = Util.GetInput<Day09>();

        [TestCategory("Test")]
        [DataRow(9, 25, 32)] // 9 players; last marble is worth 25 points: high score is 32
        [DataRow(10, 1618, 8317)] // 10 players; last marble is worth 1618 points: high score is 8317
        [DataRow(13, 7999, 146373)] // 13 players; last marble is worth 7999 points: high score is 146373
        [DataRow(17, 1104, 2764)] // 17 players; last marble is worth 1104 points: high score is 2764
        [DataRow(21, 6111, 54718)] // 21 players; last marble is worth 6111 points: high score is 54718
        [DataRow(30, 5807, 37305)] // 30 players; last marble is worth 5807 points: high score is 37305
        public void Marbles01Test(int numPlayers, int numMarbles, int expected)
        {
            Assert.AreEqual(expected, MMXVIII.Day09.MarbleGame(numPlayers, numMarbles));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void MarbleGame_Part1_Regression()
        {
            Assert.AreEqual(374287UL, Day09.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void MarbleGame_Part2_Regression()
        {
            Assert.AreEqual(3083412635, Day09.Part2(input));
        }

    }
}

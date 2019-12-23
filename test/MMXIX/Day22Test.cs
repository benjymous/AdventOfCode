using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Diagnostics;

namespace Advent.MMXIX.Test
{
    [TestCategory("2019")]
    [TestClass]
    public class Day22Test
    {
        string input = Util.GetInput<Day22>();

        public static IEnumerable<object[]> GetData()
        {
            yield return new object[] { new Day22.Deck(10), "0, 1, 2, 3, 4, 5, 6, 7, 8, 9" };
            yield return new object[] { new Day22.Deck(10).Stack(), "9, 8, 7, 6, 5, 4, 3, 2, 1, 0" };
            yield return new object[] { new Day22.Deck(10).Cut(3), "3, 4, 5, 6, 7, 8, 9, 0, 1, 2" };
            yield return new object[] { new Day22.Deck(10).Cut(-4), "6, 7, 8, 9, 0, 1, 2, 3, 4, 5" };
            yield return new object[] { new Day22.Deck(10).Deal(3), "0, 7, 4, 1, 8, 5, 2, 9, 6, 3" };
        }

        [TestCategory("Test")]
        [DynamicData(nameof(GetData), DynamicDataSourceType.Method)]
        [DataTestMethod]
        public void ShuffleTest(Day22.Deck deck, string expected)
        {
            Assert.AreEqual(expected, deck.ToString());
        }

        [TestCategory("Test")]
        [DataRow("deal with increment 7\ndeal into new stack\ndeal into new stack","0, 3, 6, 9, 2, 5, 8, 1, 4, 7")]
        [DataRow("cut 6\ndeal with increment 7\ndeal into new stack","3, 0, 7, 4, 1, 8, 5, 2, 9, 6")]
        [DataRow("deal into new stack\ncut -2\ndeal with increment 7\ncut 8\ncut -4\ndeal with increment 7\ncut 3\ndeal with increment 9\ndeal with increment 3\ncut -1", "9, 2, 5, 8, 1, 4, 7, 0, 3, 6")]
        [DataRow("deal with increment 7\ndeal with increment 9\ncut -2", "6, 3, 0, 7, 4, 1, 8, 5, 2, 9")]
        [DataTestMethod]
        public void RulesTest(string input, string expected)
        {
            var shuffled = Day22.DoShuffle(10, input);
            Assert.AreEqual(expected, shuffled.ToString());
        }


        [TestCategory("Regression")]
        [DataTestMethod]
        public void Shuffle_Part1_Regression()
        {
            Assert.AreEqual(1498, Day22.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Shuffle_Part2_Regression()
        {
            Assert.AreEqual(74662303452927, Day22.Part2(input));
        }
    }
}

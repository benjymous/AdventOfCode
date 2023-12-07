using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace AoC.Advent2023.Test;

[TestCategory("2023")]
[TestClass]
public class Day07Test
{
    readonly string input = Util.GetInput<Day07>();

    readonly string test =
@"32T3K 765
T55J5 684
KK677 28
KTJJT 220
QQQJA 483".Replace("\r", "");

    [TestCategory("Test")]
    [DataRow("AAAAA", Day07.HandType.FiveOfAKind)]
    [DataRow("AA8AA", Day07.HandType.FourOfAKind)]
    [DataRow("23332", Day07.HandType.FullHouse)]
    [DataRow("TTT98", Day07.HandType.ThreeOfAKind)]
    [DataRow("23432", Day07.HandType.TwoPair)]
    [DataRow("A23A4", Day07.HandType.OnePair)]
    [DataRow("23456", Day07.HandType.HighCard)]

    [DataRow("32T3K", Day07.HandType.OnePair)]
    [DataRow("T55J5", Day07.HandType.ThreeOfAKind)]
    [DataRow("KK677", Day07.HandType.TwoPair)]
    [DataRow("KTJJT", Day07.HandType.TwoPair)]
    [DataRow("QQQJA", Day07.HandType.ThreeOfAKind)]

    [DataTestMethod]
    public void HandTypeTest1(string input, Day07.HandType expected)
    {
        Assert.AreEqual(expected, Day07.GetHandType(input));
    }

    [TestCategory("Test")]
    [DataRow("AAAAA", Day07.HandType.FiveOfAKind)]
    [DataRow("AA8AA", Day07.HandType.FourOfAKind)]
    [DataRow("23332", Day07.HandType.FullHouse)]
    [DataRow("TTT98", Day07.HandType.ThreeOfAKind)]
    [DataRow("23432", Day07.HandType.TwoPair)]
    [DataRow("A23A4", Day07.HandType.OnePair)]
    [DataRow("23456", Day07.HandType.HighCard)]

    [DataRow("32T3K", Day07.HandType.OnePair)]
    [DataRow("T55J5", Day07.HandType.FourOfAKind)]
    [DataRow("KK677", Day07.HandType.TwoPair)]
    [DataRow("KTJJT", Day07.HandType.FourOfAKind)]
    [DataRow("QQQJA", Day07.HandType.FourOfAKind)]

    [DataTestMethod]
    public void HandTypeTest2(string input, Day07.HandType expected)
    {
        Assert.AreEqual(expected, Day07.GetHandType(input.Replace("J", "*")));
    }

    [TestCategory("Test")]
    [DataRow("32T3K", "32T3K")]
    [DataRow("T55J5", "T5555")]
    [DataRow("KK677", "KK677")]
    [DataRow("KTJJT", "KTTTT")]
    [DataRow("QQQJA", "QQQQA")]
    [DataRow("JJJJJ", "AAAAA")]
    [DataRow("JJJJK", "KKKKK")]

    [DataTestMethod]
    public void ApplyJokersTest(string input, string expected)
    {
        Assert.AreEqual(expected, Day07.ApplyJokers(input.Replace("J", "*")));
    }

    [TestCategory("Test")]
    [TestMethod]
    public void OrderingTest1()
    {
        string[] cards = ["T55J5", "KK677", "KTJJT", "QQQJA", "32T3K"];
        string[] expected = ["32T3K", "KTJJT", "KK677", "T55J5", "QQQJA"];

        Assert.AreEqual(string.Join(", ", expected), string.Join(", ", Day07.SortCards(cards.Select(h => new Day07.Hand(h))).Select(h => h.Cards)));
    }

    [TestCategory("Test")]
    [TestMethod]
    public void OrderingTest2()
    {
        string[] cards = ["T55J5", "KK677", "KTJJT", "QQQJA", "32T3K"];
        string[] expected = ["32T3K", "KK677", "T55J5", "QQQJA", "KTJJT"];

        Assert.AreEqual(string.Join(", ", expected), string.Join(", ", Day07.SortCards(cards.Select(h => new Day07.Hand(h.Replace("J", "*")))).Select(h => h.Cards)).Replace("*", "J"));
    }

    [TestCategory("Test")]
    [DataTestMethod]
    public void CardRanking_01Test()
    {
        Assert.AreEqual(6440, Day07.Part1(test));
    }

    [TestCategory("Test")]
    [DataTestMethod]
    public void CardRanking_02Test()
    {
        Assert.AreEqual(5905, Day07.Part2(test));
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void CamelCards_Part1_Regression()
    {
        Assert.AreEqual(251216224, Day07.Part1(input));
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void CamelCards_Part2_Regression()
    {
        Assert.AreEqual(250825971, Day07.Part2(input));
    }
}


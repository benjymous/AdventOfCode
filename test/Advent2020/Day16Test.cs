using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2020.Test
{
    [TestCategory("2020")]
    [TestClass]
    public class Day16Test
    {
        readonly string input = Util.GetInput<Day16>();

        [TestCategory("Test")]
        [DataRow("class: 1-3 or 5-7\n" +
            "row: 6 - 11 or 33 - 44\n" +
            "seat: 13 - 40 or 45 - 50\n" +
            "\n" +
            "your ticket:\n" +
            "7, 1, 14\n" +
            "\n" +
            "nearby tickets:\n" +
            "7, 3, 47\n" +
            "40, 4, 50\n" +
            "55, 2, 20\n" +
            "38, 6, 12",
            "class: 1, row: 7, seat: 14")]
        [DataRow("class: 0-1 or 4-19\n" +
            "row: 0 - 5 or 8 - 19\n" +
            "seat: 0 - 13 or 16 - 19\n" +
            "\n" +
            "your ticket:\n" +
            "11, 12, 13\n" +
            "\n" +
            "nearby tickets:\n" +
            "3, 9, 18\n" +
            "15, 1, 5\n" +
            "5, 14, 9",
            "class: 12, row: 11, seat: 13")]
        [DataTestMethod]
        public void Decoder1Test(string input, string expected)
        {
            Assert.AreEqual(expected, Day16.TestDecode(input));
        }

        [DataTestMethod]
        public void Decoder2Test()
        {
            Assert.AreEqual("arrival location: 137, arrival platform: 139, arrival station: 157, arrival track: 127, class: 151, departure date: 131, departure location: 97, departure platform: 101, departure station: 113, departure time: 109, departure track: 149, duration: 73, price: 59, route: 67, row: 103, seat: 53, train: 163, type: 71, wagon: 61, zone: 107", Day16.TestDecode(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Tickets_Part1_Regression()
        {
            Assert.AreEqual(18227, Day16.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Tickets_Part2_Regression()
        {
            Assert.AreEqual(2355350878831L, Day16.Part2(input));
        }
    }
}

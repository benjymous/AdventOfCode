using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2020.Test
{
    [TestCategory("2020")]
    [TestCategory("PackedVect")]
    [TestClass]
    public class Day11Test
    {
        readonly string input = Util.GetInput<Day11>();

        [TestCategory("Test")]
        [DataRow("L.LL.LL.LL\n" +
                 "LLLLLLL.LL\n" +
                 "L.L.L..L..\n" +
                 "LLLL.LL.LL\n" +
                 "L.LL.LL.LL\n" +
                 "L.LLLLL.LL\n" +
                 "..L.L.....\n" +
                 "LLLLLLLLLL\n" +
                 "L.LLLLLL.L\n" +
                 "L.LLLLL.LL\n", 37)]
        [DataTestMethod]
        public void Seats01Test(string input, int expected)
        {
            Assert.AreEqual(expected, Day11.Part1(input));
        }

        [TestCategory("Test")]
        [DataRow(".......#.\n" +
                 "...#.....\n" +
                 ".#.......\n" +
                 ".........\n" +
                 "..#L....#\n" +
                 "....#....\n" +
                 ".........\n" +
                 "#........\n" +
                 "...#.....\n", 3, 4, 8)]

        [DataRow(".............\n" +
                 ".L.L.#.#.#.#.\n" +
                 ".............\n", 1, 1, 0)]

        [DataRow(".##.##.\n" +
                 "#.#.#.#\n" +
                 "##...##\n" +
                 "...L...\n" +
                 "##...##\n" +
                 "#.#.#.#\n" +
                 ".##.##.\n", 3, 3, 0)]
        [DataTestMethod]
        public void NeighboursTest(string input, int x, int y, int expected)
        {
            int v = TestNeighbours(input, x, y);
            Assert.AreEqual(expected, v);
        }

        public static int TestNeighbours(string input, int tx, int ty)
        {
            var state = new Day11.State(input, QuestionPart.Part2);
            if (!state.Seats.Contains(tx + (ty << 16))) return -1;
            return state.Neighbours(tx + (ty << 16));
        }

        [TestCategory("Test")]
        [DataRow("L.LL.LL.LL\n" +
            "LLLLLLL.LL\n" +
            "L.L.L..L..\n" +
            "LLLL.LL.LL\n" +
            "L.LL.LL.LL\n" +
            "L.LLLLL.LL\n" +
            "..L.L.....\n" +
            "LLLLLLLLLL\n" +
            "L.LLLLLL.L\n" +
            "L.LLLLL.LL\n", 26)]
        [DataTestMethod]
        public void Seats02Test(string input, int expected)
        {
            Assert.AreEqual(expected, Day11.Part2(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Seating_Part1_Regression()
        {
            Assert.AreEqual(2494, Day11.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Seating_Part2_Regression()
        {
            Assert.AreEqual(2306, Day11.Part2(input));
        }
    }
}

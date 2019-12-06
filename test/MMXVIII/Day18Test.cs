using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace Advent.MMXVIII.Test
{
    [TestClass]
    public class Day18Test
    {
        [DataRow('.', "........", '.')]
        [DataRow('.', ".....|..", '.')]
        [DataRow('.', ".|.....|", '.')]
        [DataRow('.', "...||..|", '|')]
        [DataRow('.', "..||.|.|", '|')]

        [DataRow('|', "........", '|')]
        [DataRow('|', "...#....", '|')]
        [DataRow('|', ".#....#.", '|')]
        [DataRow('|', "#.#..#..", '#')]
        [DataRow('|', "#.#.#..#", '#')]

        [DataRow('#', "........", '.')]
        [DataRow('#', ".....#..", '.')]
        [DataRow('#', "....|...", '.')]
        [DataRow('#', ".#...|..", '#')]
        [DataRow('#', ".##..|..", '#')]
        [DataRow('#', ".#...||.", '#')]
        [DataRow('#', ".##..||.", '#')]
        [DataTestMethod]
        public void RulesTest(char current, string neighbours, char expected)
        {
            Assert.AreEqual(expected, MMXVIII.Day18.Step(current, neighbours));
        }

        [DataRow(".#.#...|#.,.....#|##|,.|..|...#.,..|#.....#,#.#|||#|#|,...#.||...,.|....|...,||...#|.#|,|.||||..|.,...#.|..|.", 1147)]
        [DataTestMethod]
        public void Lumber01Test(string input, int expected)
        {
            Assert.AreEqual(expected, MMXVIII.Day18.Part1(input));
        }

        // Stable layout
        [DataRow("......,......,..#|..,..#|..,..|...,......", 1, 6)]
        [DataRow("......,......,..#|..,..#|..,..|...,......", 10, 6)]
        [DataRow("......,......,..#|..,..#|..,..|...,......", 1000, 6)]
        [DataRow("......,......,..#|..,..#|..,..|...,......", 1000000, 6)]

        // Expands to fill right
        [DataRow("......,......,..#|..,..#|..,..||..,......", 1, 10)]
        [DataRow("......,......,..#|..,..#|..,..||..,......", 10, 44)]
        [DataRow("......,......,..#|..,..#|..,..||..,......", 1000, 44)]
        [DataRow("......,......,..#|..,..#|..,..||..,......", 1000000, 44)]

        [DataRow(".#.#...|#.,.....#|##|,.|..|...#.,..|#.....#,#.#|||#|#|,...#.||...,.|....|...,||...#|.#|,|.||||..|.,...#.|..|.", 1, 480)]
        [DataRow(".#.#...|#.,.....#|##|,.|..|...#.,..|#.....#,#.#|||#|#|,...#.||...,.|....|...,||...#|.#|,|.||||..|.,...#.|..|.", 10, 1147)]
        [DataRow(".#.#...|#.,.....#|##|,.|..|...#.,..|#.....#,#.#|||#|#|,...#.||...,.|....|...,||...#|.#|,|.||||..|.,...#.|..|.", 15, 57)]
        [DataRow(".#.#...|#.,.....#|##|,.|..|...#.,..|#.....#,#.#|||#|#|,...#.||...,.|....|...,||...#|.#|,|.||||..|.,...#.|..|.", 20, 0)]
        [DataRow(".#.#...|#.,.....#|##|,.|..|...#.,..|#.....#,#.#|||#|#|,...#.||...,.|....|...,||...#|.#|,|.||||..|.,...#.|..|.", 1000, 0)]
        [DataRow(".#.#...|#.,.....#|##|,.|..|...#.,..|#.....#,#.#|||#|#|,...#.||...,.|....|...,||...#|.#|,|.||||..|.,...#.|..|.", 10000000, 0)]
        [DataTestMethod]
        public void Lumber02Test(string input, int iterations, int expected)
        {
            Assert.AreEqual(expected, MMXVIII.Day18.Run(input, iterations));
        }

        [DataTestMethod]
        public void Lumber_Part1_Regression()
        {
            var d = new Day18();
            var input = Util.GetInput(d);
            Assert.AreEqual(536370, Day18.Part1(input));
        }

        [DataTestMethod]
        public void Lumber_Part2_Regression()
        {
            var d = new Day18();
            var input = Util.GetInput(d);
            Assert.AreEqual(190512, Day18.Part2(input));
        }

    }
}
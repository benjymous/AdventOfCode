using AoC.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using static AoC.Advent2021.Day23;

namespace AoC.Advent2021.Test
{
    [TestCategory("2021")]
    [TestClass]
    public class Day23Test
    {
        string input = Util.GetInput<Day23>();

        const string test = @"#############
#...........#
###B#C#B#D###
  #A#D#C#A#
  #########";

        const string test2 = @"#############
#.........A.#
###.#B#C#D###
  #A#B#C#D#
  #########";

        const string test3 = @"#############
#.....D.D.A.#
###.#B#C#.###
  #A#B#C#.#
  #########";

        const string test4 = @"#############
#.....D.....#
###.#B#C#D###
  #A#B#C#A#
  #########";

        const string test5 = @"#############
#.....D.....#
###B#.#C#D###
  #A#B#C#A#
  #########";

        const string test6a = @"#############
#...B.D.....#
###B#.#C#D###
  #A#.#C#A#
  #########";

        const string test6 = @"#############
#...B.......#
###B#.#C#D###
  #A#D#C#A#
  #########";

        const string test7 = @"#############
#...B.......#
###B#C#.#D###
  #A#D#C#A#
  #########";

        [TestCategory("Test")]
        [DataTestMethod]
        [DataRow(test2, 8)]
        [DataRow(test3, 7008)]
        [DataRow(test4, 9011)]
        [DataRow(test5, 9051)]
        [DataRow(test6a, 9081)]
        //[DataRow(test6, 12081)]  // ??
        //[DataRow(test7, 12481)]  // ??
        //[DataRow(test, 12521)] // ??
        public void ShrimpTest(string input, int expected)
        {
            Assert.AreEqual(expected, Day23.Part1(input.Replace("\r","")));
        }


        [TestCategory("Regression")]
        [DataTestMethod]
        public void Shrimp_Part1_Regression()
        {
            Assert.AreEqual(0, Day23.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Shrimp_Part2_Regression()
        {
            Assert.AreEqual(0, Day23.Part2(input));
        }
    }
}

using AoC.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace AoC.Advent2017.Test
{
    [TestCategory("2017")]
    [TestClass]
    public class Day09Test
    {
        readonly string input = Util.GetInput<Day09>();

        [DataRow("<>", "")] // empty garbage.
        [DataRow("<random characters>", "")] // garbage containing random characters.
        [DataRow("<<<<>", "")] // because the extra<are ignored.
        [DataRow("<{!>}>", "")] // because the first > is canceled.
        [DataRow("<!!>", "")] // because the second ! is canceled, allowing the > to terminate the garbage.
        [DataRow("<!!!>>", "")] // because the second ! and the first > are canceled.
        [DataRow("<{o'i!a,<{i<a>", "")] // which ends at the first >.

        [DataRow("{}", "{}")] //  1 group.
        [DataRow("{{{}}}", "{{{}}}")] //  3 groups.
        [DataRow("{{},{}}", "{{},{}}")] //  also 3 groups.
        [DataRow("{{{},{},{{}}}}", "{{{},{},{{}}}}")] //  6 groups.
        [DataRow("{<{},{},{{}}>}", "{}")] //  1 group (which itself contains garbage).
        [DataRow("{<a>,<a>,<a>,<a>}", "{,,,}")] //  1 group.
        [DataRow("{{<a>},{<a>},{<a>},{<a>}}", "{{},{},{},{}}")] //  5 groups.
        [DataRow("{{<!>},{<!>},{<!>},{<a>}}", "{{}}")] // 2 groups (since all but the last > are canceled
        [DataTestMethod]

        public void GarbageStripTest(string input, string expected)
        {
            Assert.AreEqual(expected, Advent2017.Day09.StripGarbage(new Advent2017.Day09.State(input)).AsString());
        }


        [DataRow("{}", 1)] //  1 group.
        [DataRow("{{{}}}", 3)] //  3 groups.
        [DataRow("{{},{}}", 3)] //  also 3 groups.
        [DataRow("{{{},{},{{}}}}", 6)] //  6 groups.
        [DataRow("{<{},{},{{}}>}", 1)] //  1 group (which itself contains garbage).
        [DataRow("{<a>,<a>,<a>,<a>}", 1)] //  1 group.
        [DataRow("{{<a>},{<a>},{<a>},{<a>}}", 5)] //  5 groups.
        [DataRow("{{<!>},{<!>},{<!>},{<a>}}", 2)] // 2 groups (since all but the last > are
        [DataTestMethod]
        public void GroupCountTest(string input, int expected)
        {
            Assert.AreEqual(expected, Advent2017.Day09.GetGroups(input).Count());
        }


        [DataRow("{}", 1)] // score of 1.
        [DataRow("{{{}}}", 6)] // score of 1 + 2 + 3 = 6.
        [DataRow("{{},{}}", 5)] // score of 1 + 2 + 2 = 5.
        [DataRow("{{{},{},{{}}}}", 16)] // score of 1 + 2 + 3 + 3 + 3 + 4 = 16.
        [DataRow("{<a>,<a>,<a>,<a>}", 1)] //score of 1.
        [DataRow("{{<ab>},{<ab>},{<ab>},{<ab>}}", 9)] // score of 1 + 2 + 2 + 2 + 2 = 9.
        [DataRow("{{<!!>},{<!!>},{<!!>},{<!!>}}", 9)] //score of 1 + 2 + 2 + 2 + 2 = 9.
        [DataRow("{{<a!>},{<a!>},{<a!>},{<ab>}}", 3)] //score of 1 + 2 = 3.
        [DataTestMethod]
        public void ScoreTest(string input, int expected)
        {
            Assert.AreEqual(expected, Advent2017.Day09.GetScore(input));
        }



        [DataRow("<>", 0)]
        [DataRow("<random characters>", 17)]
        [DataRow("<<<<>", 3)]
        [DataRow("<{!>}>", 2)]
        [DataRow("<!!>", 0)]
        [DataRow("<!!!>>", 0)]
        [DataRow("<{o'i!a,<{i<a>", 10)]
        [DataTestMethod]
        public void CountTest(string input, int expected)
        {
            Assert.AreEqual(expected, Advent2017.Day09.CountGarbage(input));
        }


        [TestCategory("Regression")]
        [DataTestMethod]
        public void Garbage_Part1_Regression()
        {
            Assert.AreEqual(9662, Advent2017.Day09.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Garbage_Part2_Regression()
        {
            Assert.AreEqual(4903, Advent2017.Day09.Part2(input));
        }

    }
}

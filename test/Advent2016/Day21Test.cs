using AoC.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace AoC.Advent2016.Test
{
    [TestCategory("2016")]
    [TestCategory("RegexFactory")]
    [TestClass]
    public class Day21Test
    {
        readonly string input = Util.GetInput<Day21>();

        [TestCategory("Test")]
        [DataTestMethod]
        [DataRow("move position 2 to position 0")]
        [DataRow("move position 2 to position 3")]
        [DataRow("move position 2 to position 4")]
        [DataRow("move position 2 to position 6")]
        [DataRow("move position 3 to position 1")]
        [DataRow("move position 3 to position 2")]
        [DataRow("move position 4 to position 6")]
        [DataRow("move position 5 to position 6")]
        [DataRow("move position 6 to position 5")]
        [DataRow("move position 7 to position 0")]
        [DataRow("move position 7 to position 2")]
        [DataRow("reverse positions 0 through 4")]
        [DataRow("reverse positions 1 through 2")]
        [DataRow("reverse positions 1 through 3")]
        [DataRow("reverse positions 1 through 4")]
        [DataRow("reverse positions 1 through 7")]
        [DataRow("reverse positions 2 through 3")]
        [DataRow("reverse positions 2 through 5")]
        [DataRow("reverse positions 2 through 6")]
        [DataRow("reverse positions 2 through 7")]
        [DataRow("reverse positions 3 through 7")]
        [DataRow("reverse positions 4 through 6")]
        [DataRow("reverse positions 4 through 7")]
        [DataRow("reverse positions 5 through 6")]
        [DataRow("rotate based on position of letter a")]
        [DataRow("rotate based on position of letter b")]
        [DataRow("rotate based on position of letter c")]
        [DataRow("rotate based on position of letter d")]
        [DataRow("rotate based on position of letter e")]
        [DataRow("rotate based on position of letter f")]
        [DataRow("rotate based on position of letter g")]
        [DataRow("rotate based on position of letter h")]
        [DataRow("rotate left 0 steps")]
        [DataRow("rotate left 1 step")]
        [DataRow("rotate left 2 steps")]
        [DataRow("rotate left 3 steps")]
        [DataRow("rotate left 5 steps")]
        [DataRow("rotate left 6 steps")]
        [DataRow("rotate left 7 steps")]
        [DataRow("rotate right 0 steps")]
        [DataRow("rotate right 2 steps")]
        [DataRow("rotate right 3 steps")]
        [DataRow("rotate right 4 steps")]
        [DataRow("rotate right 5 steps")]
        [DataRow("rotate right 6 steps")]
        [DataRow("rotate right 7 steps")]
        [DataRow("swap letter a with letter e")]
        [DataRow("swap letter a with letter h")]
        [DataRow("swap letter b with letter a")]
        [DataRow("swap letter b with letter e")]
        [DataRow("swap letter b with letter g")]
        [DataRow("swap letter c with letter f")]
        [DataRow("swap letter d with letter c")]
        [DataRow("swap letter e with letter c")]
        [DataRow("swap letter e with letter h")]
        [DataRow("swap letter f with letter a")]
        [DataRow("swap letter f with letter c")]
        [DataRow("swap letter f with letter d")]
        [DataRow("swap letter f with letter h")]
        [DataRow("swap letter g with letter c")]
        [DataRow("swap letter g with letter h")]
        [DataRow("swap letter h with letter a")]
        [DataRow("swap letter h with letter d")]
        [DataRow("swap letter h with letter g")]
        [DataRow("swap position 1 with position 3")]
        [DataRow("swap position 1 with position 7")]
        [DataRow("swap position 2 with position 7")]
        [DataRow("swap position 3 with position 0")]
        [DataRow("swap position 4 with position 6")]
        [DataRow("swap position 4 with position 7")]
        [DataRow("swap position 5 with position 4")]
        [DataRow("swap position 6 with position 1")]
        [DataRow("swap position 6 with position 3")]
        [DataRow("swap position 6 with position 5")]
        [DataRow("swap position 7 with position 5")]

        public void TryUnscramble(string rule)
        {
            var instructions = Day21.ParseInstructions(rule);
            var instructionsrev = Day21.ReverseInstructions(rule);

            var start = "defghabc";
            var scrambled = instructions[0](start.ToCharArray()).ToArray();
            var unscrambled = instructionsrev[0](scrambled).AsString();

            Assert.AreEqual(start, unscrambled);
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void PasswordScramble_Part1_Regression()
        {
            Assert.AreEqual("dbfgaehc", Day21.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void PasswordScramble_Part2_Regression()
        {
            Assert.AreEqual("aghfcdeb", Day21.Part2(input));
        }
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2021.Test
{
    [TestCategory("2021")]
    [TestCategory("FloodFill")]
    [TestClass]
    public class Day10Test
    {
        readonly string input = Util.GetInput<Day10>();

        readonly string test = @"[({(<(())[]>[[{[]{<()<>>
[(()[<>])]({[<{<<[]>>(
{([(<{}[<>[]}>{[]{[(<()>
(((({<>}<{<{<>}{[]{[]{}
[[<[([]))<([[{}[[()]]]
[{[{({}]{}}([{[{{{}}([]
{<[[]]>}<{[{[{[]{()[[[]
[<(<(<(<{}))><([]([]()
<{([([[(<>()){}]>(<<{{
<{([{{}}[<[[[<>{}]]]>[]]";

        [TestCategory("Test")]
        [DataTestMethod]
        public void Syntax01Test()
        {
            Assert.AreEqual(26397, Day10.Part1(test.Replace("\r", "")));
        }

        [TestCategory("Test")]
        [DataTestMethod]
        public void Syntax02Test()
        {
            Assert.AreEqual(288957, Day10.Part2(test.Replace("\r", "")));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Syntax_Part1_Regression()
        {
            Assert.AreEqual(316851, Day10.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Syntax_Part2_Regression()
        {
            Assert.AreEqual(2182912364, Day10.Part2(input));
        }
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2022.Test
{
    [TestCategory("2022")]
    [TestClass]
    public class Day11Test
    {
        readonly string input = Util.GetInput<Day11>();
        readonly string test = @"Monkey 0:
  Starting items: 79, 98
  Operation: new = old * 19
  Test: divisible by 23
    If true: throw to monkey 2
    If false: throw to monkey 3

Monkey 1:
  Starting items: 54, 65, 75, 74
  Operation: new = old + 6
  Test: divisible by 19
    If true: throw to monkey 2
    If false: throw to monkey 0

Monkey 2:
  Starting items: 79, 60, 97
  Operation: new = old * old
  Test: divisible by 13
    If true: throw to monkey 1
    If false: throw to monkey 3

Monkey 3:
  Starting items: 74
  Operation: new = old + 3
  Test: divisible by 17
    If true: throw to monkey 0
    If false: throw to monkey 1".Replace("\r", "");

        [TestCategory("Test")]
        [DataTestMethod]
        public void Monkeys01Test()
        {
            Assert.AreEqual(10605L, Day11.Part1(test));
        }

        [TestCategory("Test")]
        [DataTestMethod]
        public void Monkeys02Test()
        {
            Assert.AreEqual(2713310158L, Day11.Part2(test));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Monkeys_Part1_Regression()
        {
            Assert.AreEqual(152488L, Day11.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Monkeys_Part2_Regression()
        {
            Assert.AreEqual(51382025916L, Day11.Part2(input));
        }
    }
}

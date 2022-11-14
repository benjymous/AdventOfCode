using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2018.Test
{
    [TestCategory("2018")]
    [TestCategory("BinarySearch")]
    [TestCategory("RegexParse")]
    [TestClass]
    public class Day24Test
    {
        readonly string input = Util.GetInput<Day24>();

        readonly string test = @"Immune System:
17 units each with 5390 hit points (weak to radiation, bludgeoning) with an attack that does 4507 fire damage at initiative 2
989 units each with 1274 hit points (immune to fire; weak to bludgeoning, slashing) with an attack that does 25 slashing damage at initiative 3

Infection:
801 units each with 4706 hit points (weak to radiation) with an attack that does 116 bludgeoning damage at initiative 1
4485 units each with 2961 hit points (immune to radiation; weak to fire, cold) with an attack that does 12 slashing damage at initiative 4";

        [DataTestMethod]
        public void ImmuneTest1()
        {
            Assert.AreEqual(5216, Day24.Part1(test.Replace("\r", "")));
        }

        [DataTestMethod]
        public void ImmuneTest2()
        {
            Assert.AreEqual(51, Day24.Part2(test.Replace("\r", "")));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Immunity_Part1_Regression()
        {
            Assert.AreEqual(18280, Day24.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Immunity_Part2_Regression()
        {
            Assert.AreEqual(4573, Day24.Part2(input));
        }
    }
}

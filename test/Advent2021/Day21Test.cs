using AoC.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using static AoC.Advent2021.Day21;

namespace AoC.Advent2021.Test
{
    [TestCategory("2021")]
    [TestClass]
    public class Day21Test
    {
        string input = Util.GetInput<Day21>();
        string test = "Player 1 starting position: 4\nPlayer 2 starting position: 8";

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Dice1Test()
        {
            Assert.AreEqual(739785, Day21.Part1(test));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Dice2Test()
        {
            Assert.AreEqual(444356092776315, Day21.Part2(test));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Dice_Part1_Regression()
        {
            Assert.AreEqual(412344, Day21.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Dice_Part2_Regression()
        {
            Assert.AreEqual(214924284932572, Day21.Part2(input));
        }
    }
}

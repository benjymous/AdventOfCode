using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AoC.Advent2020.Test
{
    [TestCategory("2020")]
    [TestClass]
    public class Day14Test
    {
        string input = Util.GetInput<Day14>();

        [TestCategory("Test")]
        [DataRow(11, "XXXXXXXXXXXXXXXXXXXXXXXXXXXXX1XXXX0X", 73)]
        [DataRow(101, "XXXXXXXXXXXXXXXXXXXXXXXXXXXXX1XXXX0X", 101)]
        [DataRow(0, "XXXXXXXXXXXXXXXXXXXXXXXXXXXXX1XXXX0X", 64)]
        [DataTestMethod]
        public void MaskV1Test(Int64 value, string input, Int64 expected)
        {
            Assert.AreEqual(expected, Advent2020.Day14.ApplyMaskV1(value, Day14.Mask(input)));
        }

        [TestCategory("Test")]
        [DataRow("mask = XXXXXXXXXXXXXXXXXXXXXXXXXXXXX1XXXX0X\n" +
            "mem[8] = 11\n" +
            "mem[7] = 101\n" +
            "mem[8] = 0", 165)]
        [DataTestMethod]
        public void Docking01Test(string input, Int64 expected)
        {
            Assert.AreEqual(expected, Advent2020.Day14.Part1(input));
        }

        [TestCategory("Test")]
        [DataRow("mask = 000000000000000000000000000000X1001X\n" +
            "mem[42] = 100\n" +
            "mask = 00000000000000000000000000000000X0XX\n" +
            "mem[26] = 1", 208)]
        [DataTestMethod]
        public void Docking02Test(string input, Int64 expected)
        {
            Assert.AreEqual(expected, Advent2020.Day14.Part2(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Docking_Part1_Regression()
        {
            Assert.AreEqual(6386593869035, Day14.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Docking_Part2_Regression()
        {
            Assert.AreEqual(4288986482164, Day14.Part2(input));
        }
    }
}

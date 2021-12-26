using AoC.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using static AoC.Advent2021.Day25;

namespace AoC.Advent2021.Test
{
    [TestCategory("2021")]
    [TestClass]
    public class Day25Test
    {
        string input = Util.GetInput<Day25>();

        string test = @"v...>>.vv>
.vv>>.vv..
>>.>v>...v
>>v>>.>.v.
v>v.vv.v..
>.>>..v...
.vv..>.>v.
v.v..>>v.v
....v..v.>".Replace("\r", "");


        [TestCategory("Test")]
        [DataTestMethod]
        public void CucumberTest()
        {
            Assert.AreEqual(58, Day25.Part1(test));
        }



        [TestCategory("Regression")]
        [DataTestMethod]
        public void Cucumber_Part1_Regression()
        {
            Assert.AreEqual(579, Day25.Part1(input));
        }

    }
}

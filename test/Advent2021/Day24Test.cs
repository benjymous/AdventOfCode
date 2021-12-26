using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2021.Test
{
    [TestCategory("2021")]
    [TestClass]
    public class Day24Test
    {
        string input = Util.GetInput<Day24>();

        public static bool ValidateMonad(long input)
        {
            var vals = Sequence(input).Reverse().ToArray();
            if (vals.Contains(0)) return false;

            return Enumerable.Range(0, 14).Aggregate(0, (z, i) => Day24.CalcPart(vals[i], z, i)) == 0;
        }

        static IEnumerable<int> Sequence(long input)
        {
            for (int i = 0; i < 14; ++i)
            {
                yield return (int)(input % 10);
                input /= 10;
            }
        }


        [TestCategory("Test")]
        [DataTestMethod]
        [DataRow(49917929934999)]
        [DataRow(11911316711816)]

        public void MonadCalcTest(long input)
        {
            Assert.IsTrue(ValidateMonad(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void ALU_Part1_Regression()
        {
            Assert.AreEqual(49917929934999, Day24.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void ALU_Part2_Regression()
        {
            Assert.AreEqual(11911316711816, Day24.Part2(input));
        }
    }
}

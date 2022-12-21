using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2022.Test
{
    [TestCategory("2022")]
    [TestClass]
    public class Day21Test
    {
        readonly string input = Util.GetInput<Day21>();
        readonly string test = @"root: pppw + sjmn
dbpl: 5
cczh: sllz + lgvd
zczc: 2
ptdq: humn - dvpt
dvpt: 3
lfqf: 4
humn: 5
ljgn: 2
sjmn: drzm * dbpl
sllz: 4
pppw: cczh / lfqf
lgvd: ljgn * ptdq
drzm: hmdt - zczc
hmdt: 32";

        [TestCategory("Test")]
        [DataTestMethod]
        public void MathTree01Test()
        {
            Assert.AreEqual(152L, Day21.Part1(test));
        }

        [TestCategory("Test")]
        [DataTestMethod]
        public void MathTree02Test()
        {
            Assert.AreEqual(301L, Day21.Part2(test));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void MathTree_Part1_Regression()
        {
            Assert.AreEqual(10037517593724, Day21.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void MathTree_Part2_Regression()
        {
            Assert.AreEqual(3272260914328, Day21.Part2(input));
        }
    }
}

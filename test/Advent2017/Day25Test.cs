using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2017.Test
{
    [TestCategory("2017")]
    [TestClass]
    public class Day25Test
    {
        readonly string input = Util.GetInput<Day25>();

        readonly string test =
@"Begin in state A.
Perform a diagnostic checksum after 6 steps.

In state A:
    If the current value is 0:
    - Write the value 1.
    - Move one slot to the right.
    - Continue with state B.
    If the current value is 1:
    - Write the value 0.
    - Move one slot to the left.
    - Continue with state B.

In state B:
    If the current value is 0:
    - Write the value 1.
    - Move one slot to the left.
    - Continue with state A.
    If the current value is 1:
    - Write the value 1.
    - Move one slot to the right.
    - Continue with state A.
".Replace("\r", "");

        [TestCategory("Test")]
        [DataTestMethod]
        public void Tape01Test()
        {
            Assert.AreEqual(3, Day25.Part1(test));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Tape_Part1_Regression()
        {
            Assert.AreEqual(2794, Day25.Part1(input));
        }
    }
}

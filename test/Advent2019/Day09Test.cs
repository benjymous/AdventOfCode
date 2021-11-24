using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2019.Test
{
    [TestCategory("2019")]
    [TestCategory("IntCPU")]
    [TestClass]
    public class Day09Test
    {
        string input = Util.GetInput<Day09>();

        [TestCategory("Test")]
        [DataRow("109,1,204,-1,1001,100,1,100,1008,100,16,101,1006,101,0,99", "109,1,204,-1,1001,100,1,100,1008,100,16,101,1006,101,0,99")]
        [DataRow("1102,34915192,34915192,7,4,7,99,0", "1219070632396864")]
        [DataRow("104,1125899906842624,99", "1125899906842624")]
        [DataTestMethod]
        public void BoostTestOutput(string program, string expected)
        {
            Assert.AreEqual(expected, string.Join(",", string.Join("", Day09.Run(program, 1))));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Boost_Part1_Regression()
        {
            Assert.AreEqual("2518058886", Day09.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Boost_Part2_Regression()
        {
            Assert.AreEqual("44292", Day09.Part2(input));
        }
    }
}

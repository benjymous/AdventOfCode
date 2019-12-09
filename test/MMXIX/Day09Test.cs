using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace Advent.MMXIX.Test
{
    [TestClass]
    public class Day09Test
    {
        [DataRow("109,1,204,-1,1001,100,1,100,1008,100,16,101,1006,101,0,99", "109,1,204,-1,1001,100,1,100,1008,100,16,101,1006,101,0,99")]

        [DataRow("104,1125899906842624,99", "1125899906842624")]
        [DataTestMethod]
        public void BoostTestOutput(string program, string expected)
        {
            Assert.AreEqual(expected, string.Join(",", string.Join("",Day09.Run(program, 1))));
        }

        [DataRow("1102,34915192,34915192,7,4,7,99,0", 16)]
        [DataTestMethod]
        public void BoostTestLength(string program, int expected)
        {
            Assert.AreEqual(expected, Day09.Run(program, 1).Length);
        }


        // [DataTestMethod]
        // public void Boost_Part1_Regression()
        // {
        //     var d = new Day09();
        //     var input = Util.GetInput(d);
        //     Assert.AreEqual(3716250, d.Part1(input));
        // }

        // [DataTestMethod]
        // public void Boost_Part2_Regression()
        // {
        //     var d = new Day09();
        //     var input = Util.GetInput(d);
        //     Assert.AreEqual(6472, d.Part2(input));
        // }
    }
}

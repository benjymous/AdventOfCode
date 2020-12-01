using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Advent.MMXIX.Test
{
    [TestCategory("2019")]
    [TestCategory("IntCPU")]
    [TestClass]
    public class Day05Test
    {
        string input = Util.GetInput<Day05>();

        [TestCategory("Test")]
        [DataRow("1002,4,3,4,33", "1002,4,3,4,99")]
        [DataTestMethod]
        public void CPU2Test1(string program, string expected)
        {
            var cpu = new NPSA.IntCPU(program);
            cpu.Run();
            Assert.AreEqual(expected, cpu.ToString());
        }



        // Using position mode, consider whether the input is equal to 8;
        // output 1 (if it is) or 0 (if it is not).
        [DataRow("3,9,8,9,10,9,4,9,99,-1,8", 8, "1")]
        [DataRow("3,9,8,9,10,9,4,9,99,-1,8", 4, "0")]

        // Using position mode, consider whether the input is less than 8;
        // output 1 (if it is) or 0 (if it is not).
        [DataRow("3,9,7,9,10,9,4,9,99,-1,8", 7, "1")]
        [DataRow("3,9,7,9,10,9,4,9,99,-1,8", 9, "0")]

        // Using immediate mode, consider whether the input is equal to 8;
        // output 1 (if it is) or 0 (if it is not).
        [DataRow("3,3,1108,-1,8,3,4,3,99", 8, "1")]
        [DataRow("3,3,1108,-1,8,3,4,3,99", 10, "0")]

        // Using immediate mode, consider whether the input is less than 8;
        // output 1 (if it is) or 0 (if it is not).
        [DataRow("3,3,1107,-1,8,3,4,3,99", 3, "1")]
        [DataRow("3,3,1107,-1,8,3,4,3,99", 8, "0")]

        // take an input, then output 0 if the input was zero or 1 if the input was non-zero:
        [DataRow("3,12,6,12,15,1,13,14,13,4,13,99,-1,0,1,9", 0, "0")] // position mode
        [DataRow("3,12,6,12,15,1,13,14,13,4,13,99,-1,0,1,9", 42, "1")] // position mode

        [DataRow("3,3,1105,-1,9,1101,0,0,12,4,12,99,1", 0, "0")] // immediate mode
        [DataRow("3,3,1105,-1,9,1101,0,0,12,4,12,99,1", 123, "1")] // immediate mode

        // uses an input instruction to ask for a single number.
        // The program will then output 999 if the input value is below 8,
        // output 1000 if the input value is equal to 8, 
        // or output 1001 if the input value is greater than 8.
        [DataRow("3,21,1008,21,8,20,1005,20,22,107,8,21,20,1006,20,31,1106,0,36,98,0,0,1002,21,125,20,4,20,1105,1,46,104,999,1105,1,46,1101,1000,1,20,4,20,1105,1,46,98,99", 1, "999")]
        [DataRow("3,21,1008,21,8,20,1005,20,22,107,8,21,20,1006,20,31,1106,0,36,98,0,0,1002,21,125,20,4,20,1105,1,46,104,999,1105,1,46,1101,1000,1,20,4,20,1105,1,46,98,99", 4, "999")]
        [DataRow("3,21,1008,21,8,20,1005,20,22,107,8,21,20,1006,20,31,1106,0,36,98,0,0,1002,21,125,20,4,20,1105,1,46,104,999,1105,1,46,1101,1000,1,20,4,20,1105,1,46,98,99", 8, "1000")]
        [DataRow("3,21,1008,21,8,20,1005,20,22,107,8,21,20,1006,20,31,1106,0,36,98,0,0,1002,21,125,20,4,20,1105,1,46,104,999,1105,1,46,1101,1000,1,20,4,20,1105,1,46,98,99", 10, "1001")]
        [DataRow("3,21,1008,21,8,20,1005,20,22,107,8,21,20,1006,20,31,1106,0,36,98,0,0,1002,21,125,20,4,20,1105,1,46,104,999,1105,1,46,1101,1000,1,20,4,20,1105,1,46,98,99", 100, "1001")]
        [TestCategory("Test")]
        [DataTestMethod]
        public void CPU2Test2(string program, int input, string expected)
        {
            Assert.AreEqual(expected, Day05.RunProgram(program, input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void CPU2_Part1_Regression()
        {
            Assert.AreEqual("0,0,0,0,0,0,0,0,0,16574641", Day05.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void CPU2_Part2_Regression()
        {
            Assert.AreEqual("15163975", Day05.Part2(input));
        }
    }
}

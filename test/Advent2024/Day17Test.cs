using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2024.Test;

[TestCategory("2024")]
[TestClass]
public class Day17Test
{
    readonly string input = Util.GetInput<Day17>();

    [TestCategory("Test")]
    [DataRow("Register C: 9\n\nProgram: 2,6", new long[] { 0, 1, 9 }, "")]
    [DataRow("Register A: 10\n\nProgram: 5,0,5,1,5,4", new long[] { }, "0,1,2")]
    [DataRow("Register A: 2024\n\nProgram: 0,1,5,4,3,0", new long[] { 0, 0, 0 }, "4,2,5,6,7,7,7,7,3,1,0")]
    [DataRow("Register B: 29\n\nProgram: 1,7", new long[] { 0, 26, 0 }, "")]
    [DataRow("Register B: 2024\nRegister C: 43690\n\nProgram: 4,0", new long[] { 0, 44354, 43690 }, "")]
    [DataRow("Register A: 729\nRegister B: 0\nRegister C: 0\n\nProgram: 0,1,5,4,3,0", new long[] { }, "4,6,3,5,6,3,5,2,1,0")]
    [DataTestMethod]
    public void CpuTest(string input, long[] expectedRegs, string expectedOutput)
    {
        var cpu = Day17.Cpu.Create(input);
        var outputStr = string.Join(",", cpu.Run());

        if (expectedRegs.Length == 3)
        {
            CollectionAssert.AreEqual(expectedRegs, cpu.Registers);
        }
        if (expectedOutput != "")
        {
            Assert.AreEqual(expectedOutput, outputStr);
        }
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void Cpu_Part1_Regression()
    {
        Assert.AreEqual("1,0,2,0,5,7,2,1,3", Day17.Part1(input));
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void Cpu_Part2_Regression()
    {
        Assert.AreEqual(265652340990875, Day17.Part2(input));
    }
}
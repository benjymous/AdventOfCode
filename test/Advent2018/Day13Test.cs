using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2018.Test
{
    [TestCategory("2018")]
    [TestCategory("ManhattanVector")]
    [TestCategory("Direction")]
    [TestClass]
    public class Day13Test
    {
        readonly string input = Util.GetInput<Day13>();

        [TestCategory("Test")]
        [DataRow(">-<", "Crash at (1, 0)")]
        [DataRow("|,v,|,|,|,^,|", "Crash at (0, 3)")]
        [DataRow(@"/->-\        ,|   |  /----\,| /-+--+-\  |,| | |  | v  |,\-+-/  \-+--/,  \------/   ", "Crash at (7, 3)")]
        [TestMethod]
        public void Trains01(string input, string expected)
        {
            var res = Day13.TrainSim(input);
            Assert.AreEqual(expected, res);
        }

        [TestCategory("Test")]
        [DataRow(@"/>-<\  ,|   |  ,| /<+-\,| | | v,\>+</ |,  |   ^,  \<->/", "Last train at (6, 4)")]
        [TestMethod]
        public void Trains02(string input, string expected)
        {
            var res = Day13.TrainSim(input, false);
            Assert.AreEqual(expected, res);
        }

        [TestCategory("Regression")]
        [TestMethod]
        public void Train_Part1_Regression()
        {
            Assert.AreEqual("Crash at (116, 10)", Day13.Part1(input));
        }

        [TestCategory("Regression")]
        [TestMethod]
        public void Train_Part2_Regression()
        {
            Assert.AreEqual("Last train at (116, 25)", Day13.Part2(input));
        }
    }
}

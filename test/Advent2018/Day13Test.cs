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
        [DataRow(">-<", "Crash at 1,0")]
        [DataRow("|,v,|,|,|,^,|", "Crash at 0,3")]
        [DataRow(@"/->-\        ,|   |  /----\,| /-+--+-\  |,| | |  | v  |,\-+-/  \-+--/,  \------/   ", "Crash at 7,3")]
        [DataTestMethod]
        public void Trains01(string input, string expected)
        {
            var t = new Day13.TrainSim(input);
            var res = t.Run();
            Assert.AreEqual(expected, res);
        }

        [TestCategory("Test")]
        [DataRow(@"/>-<\  ,|   |  ,| /<+-\,| | | v,\>+</ |,  |   ^,  \<->/", "Last train at 6,4")]
        [DataTestMethod]
        public void Trains02(string input, string expected)
        {
            var t = new Day13.TrainSim(input)
            {
                StopOnCrash = false
            };
            var res = t.Run();
            Assert.AreEqual(expected, res);
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Train_Part1_Regression()
        {
            Assert.AreEqual("Crash at 116,10", Day13.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Train_Part2_Regression()
        {
            Assert.AreEqual("Last train at 116,25", Day13.Part2(input));
        }

    }
}

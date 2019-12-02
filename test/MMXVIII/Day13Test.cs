using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace Advent.MMXVIII.Test
{
    [TestClass]
    public class Day13Test
    {
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

        [DataRow(@"/>-<\  ,|   |  ,| /<+-\,| | | v,\>+</ |,  |   ^,  \<->/", "Last train at 6,4")]
        [DataTestMethod]
        public void Trains02(string input, string expected)
        {
            var t = new Day13.TrainSim(input);
            t.StopOnCrash = false;
            var res = t.Run();
            Assert.AreEqual(expected, res);                    
        }

        [DataTestMethod]
        public void Train_Part1_Regression()
        {
            var d = new Day13();
            var input = Util.GetInput(d);
            Assert.AreEqual("Crash at 116,10", d.Part1(input));
        }

        [DataTestMethod]
        public void Train_Part2_Regression()
        {
            var d = new Day13();
            var input = Util.GetInput(d);
            Assert.AreEqual("Last train at 116,25", d.Part2(input));
        }
     
    }
}

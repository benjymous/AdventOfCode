using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2017.Test
{
    [TestCategory("2017")]
    [TestClass]
    public class Day16Test
    {
        readonly string input = Util.GetInput<Day16>();

        [DataRow("s1,x3/4,pe/b", "abcde", "baedc")]
        [DataRow("s1,x3/4,pe/b", "baedc", "ceadb")]

        [DataRow("s12", "bfligcohnadkmjpe", "gcohnadkmjpebfli")]
        [DataRow("x3/13", "gcohnadkmjpebfli", "gcofnadkmjpebhli")]
        [DataRow("pf/d", "bhgilcofnadkmjpe", "bhgilcodnafkmjpe")]
        [DataRow("x13/15", "bhgilcodnafkmjpe", "bhgilcodnafkmepj")]
        [DataRow("x7/4", "gbnokfaecmphdilj", "gbnoefakcmphdilj")]
        [DataRow("x14/6", "bgejikfdolnahpcm", "bgejikcdolnahpfm")]
        [DataRow("s4", "nidogjbhpclmaefk", "aefknidogjbhpclm")]
        [DataRow("pn/e", "pecinkdagjbhoflm", "pnciekdagjbhoflm")]

        [DataTestMethod]
        public void Dance01Test(string input, string start, string expected)
        {
            Assert.AreEqual(expected, Day16.DoDance(input, start));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Promenade_Part1_Regression()
        {
            Assert.AreEqual("lgpkniodmjacfbeh", Day16.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Promenade_Part2_Regression()
        {
            Assert.AreEqual("hklecbpnjigoafmd", Day16.Part2(input));
        }

    }
}

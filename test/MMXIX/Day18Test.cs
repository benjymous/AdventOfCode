using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace Advent.MMXIX.Test
{
    [TestCategory("2019")]
    [TestClass]
    public class Day18Test
    {
        string input = Util.GetInput<Day18>();

        [TestCategory("Test")]
        [DataRow("#########\n#b.A.@.a#\n#########", 8)]
        [DataRow("########################\n#f.D.E.e.C.b.A.@.a.B.c.#\n######################.#\n#d.....................#\n########################", 86)] 
        [DataRow("########################\n#...............b.C.D.f#\n#.######################\n#.....@.a.B.c.d.A.e.F.g#\n########################", 132)]
        [DataRow("#################\n#i.G..c...e..H.p#\n########.########\n#j.A..b...f..D.o#\n########@########\n#k.E..a...g..B.n#\n########.########\n#l.F..d...h..C.m#\n#################", 136)]
        [DataRow("########################\n#@..............ac.GI.b#\n###d#e#f################\n###A#B#C################\n###g#h#i################\n########################", 81)]
        [DataTestMethod]
        public void PathTest(string input, int expected)
        {
            Assert.AreEqual(expected, Day18.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Vault_Part1_Regression()
        {
            Assert.AreEqual(4668, Day18.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Vault_Part2_Regression()
        {
            Assert.AreEqual(0, Day18.Part2(input));
        }
    }
}

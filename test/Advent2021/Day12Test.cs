using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2021.Test
{
    [TestCategory("2021")]
    [TestClass]
    public class Day12Test
    {
        string input = Util.GetInput<Day12>();

        [TestCategory("Test")]
        [DataTestMethod]
        [DataRow("start-A\nstart-b\nA-c\nA-b\nb-d\nA-end\nb-end", 10)]
        [DataRow("dc-end\nHN-start\nstart-kj\ndc-start\ndc-HN\nLN-dc\nHN-end\nkj-sa\nkj-HN\nkj-dc", 19)]
        [DataRow("fs-end\nhe-DX\nfs-he\nstart-DX\npj-DX\nend-zg\nzg-sl\nzg-pj\npj-he\nRW-he\nfs-DX\npj-RW\nzg-RW\nstart-pj\nhe-WI\nzg-he\npj-fs\nstart-RW", 226)]
        public void Passages01Test(string input, int expected)
        {
            Assert.AreEqual(expected, Day12.Part1(input.Replace("\r", "")));
        }

        [TestCategory("Test")]
        [DataTestMethod]
        [DataRow("start-A\nstart-b\nA-c\nA-b\nb-d\nA-end\nb-end", 36)]
        [DataRow("dc-end\nHN-start\nstart-kj\ndc-start\ndc-HN\nLN-dc\nHN-end\nkj-sa\nkj-HN\nkj-dc", 103)]
        [DataRow("fs-end\nhe-DX\nfs-he\nstart-DX\npj-DX\nend-zg\nzg-sl\nzg-pj\npj-he\nRW-he\nfs-DX\npj-RW\nzg-RW\nstart-pj\nhe-WI\nzg-he\npj-fs\nstart-RW", 3509)]
        public void Passages02Test(string input, int expected)
        {
            Assert.AreEqual(expected, Day12.Part2(input.Replace("\r", "")));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Passages_Part1_Regression()
        {
            Assert.AreEqual(5157, Day12.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Passages_Part2_Regression()
        {
            Assert.AreEqual(144309, Day12.Part2(input));
        }
    }
}

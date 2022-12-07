using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2022.Test
{
    [TestCategory("2022")]
    [TestClass]
    public class Day07Test
    {
        readonly string input = Util.GetInput<Day07>();
        readonly string test = @"$ cd /
$ ls
dir a
14848514 b.txt
8504156 c.dat
dir d
$ cd a
$ ls
dir a
29116 f
2557 g
62596 h.lst
$ cd a
$ ls
584 i
$ cd ..
$ cd ..
$ cd d
$ ls
4060174 j
8033020 d.log
5626152 d.ext
7214296 k".Replace("\r", "");

        [TestCategory("Test")]
        [DataTestMethod]
        public void Filesystem01Test()
        {
            Assert.AreEqual(95437, Day07.Part1(test));
        }

        [TestCategory("Test")]
        [DataTestMethod]
        public void Filesystem02Test()
        {
            Assert.AreEqual(24933642, Day07.Part2(test));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Filesystem_Part1_Regression()
        {
            Assert.AreEqual(1908462, Day07.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Filesystem_Part2_Regression()
        {
            Assert.AreEqual(3979145, Day07.Part2(input));
        }
    }
}

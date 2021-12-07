using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2020.Test
{
    [TestCategory("2020")]
    [TestCategory("HexVector")]
    [TestClass]
    public class Day24Test
    {
        string input = Util.GetInput<Day24>();

        string test = "sesenwnenenewseeswwswswwnenewsewsw\n" +
            "neeenesenwnwwswnenewnwwsewnenwseswesw\n" +
            "seswneswswsenwwnwse\n" +
            "nwnwneseeswswnenewneswwnewseswneseene\n" +
            "swweswneswnenwsewnwneneseenw\n" +
            "eesenwseswswnenwswnwnwsewwnwsene\n" +
            "sewnenenenesenwsewnenwwwse\n" +
            "wenwwweseeeweswwwnwwe\n" +
            "wsweesenenewnwwnwsenewsenwwsesesenwne\n" +
            "neeswseenwwswnwswswnw\n" +
            "nenwswwsewswnenenewsenwsenwnesesenew\n" +
            "enewnwewneswsewnwswenweswnenwsenwsw\n" +
            "sweneswneswneneenwnewenewwneswswnese\n" +
            "swwesenesewenwneswnwwneseswwne\n" +
            "enesenwswwswneneswsenwnewswseenwsese\n" +
            "wnwnesenesenenwwnenwsewesewsesesew\n" +
            "nenewswnwewswnenesenwnesewesw\n" +
            "eneswnwswnwsenenwnwnwwseeswneewsenese\n" +
            "neswnwewnwnwseenwseesewsenwsweewe\n" +
            "wseweeenwnesenwwwswnew";

        [TestCategory("Test")]
        [DataTestMethod]
        public void Lobby1Test()
        {
            Assert.AreEqual(10, Day24.Part1(test));
        }

        [TestCategory("Test")]
        [DataTestMethod]
        public void Lobby2Test()
        {
            Assert.AreEqual(2208, Day24.Part2(test));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Lobby_Part1_Regression()
        {
            Assert.AreEqual(307, Day24.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Lobby_Part2_Regression()
        {
            Assert.AreEqual(3787, Day24.Part2(input));
        }
    }
}

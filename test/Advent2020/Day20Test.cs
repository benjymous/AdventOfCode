using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2020.Test
{
    [TestCategory("2020")]
    [TestClass]
    public class Day20Test
    {
        string input = Util.GetInput<Day20>();

        string test = "Tile 2311:\n" +
            "..##.#..#.\n" +
            "##..#.....\n" +
            "#...##..#.\n" +
            "####.#...#\n" +
            "##.##.###.\n" +
            "##...#.###\n" +
            ".#.#.#..##\n" +
            "..#....#..\n" +
            "###...#.#.\n" +
            "..###..###\n" +
            "\n" +
            "Tile 1951:\n" +
            "#.##...##.\n" +
            "#.####...#\n" +
            ".....#..##\n" +
            "#...######\n" +
            ".##.#....#\n" +
            ".###.#####\n" +
            "###.##.##.\n" +
            ".###....#.\n" +
            "..#.#..#.#\n" +
            "#...##.#..\n" +
            "\n" +
            "Tile 1171:\n" +
            "####...##.\n" +
            "#..##.#..#\n" +
            "##.#..#.#.\n" +
            ".###.####.\n" +
            "..###.####\n" +
            ".##....##.\n" +
            ".#...####.\n" +
            "#.##.####.\n" +
            "####..#...\n" +
            ".....##...\n" +
            "\n" +
            "Tile 1427:\n" +
            "###.##.#..\n" +
            ".#..#.##..\n" +
            ".#.##.#..#\n" +
            "#.#.#.##.#\n" +
            "....#...##\n" +
            "...##..##.\n" +
            "...#.#####\n" +
            ".#.####.#.\n" +
            "..#..###.#\n" +
            "..##.#..#.\n" +
            "\n" +
            "Tile 1489:\n" +
            "##.#.#....\n" +
            "..##...#..\n" +
            ".##..##...\n" +
            "..#...#...\n" +
            "#####...#.\n" +
            "#..#.#.#.#\n" +
            "...#.#.#..\n" +
            "##.#...##.\n" +
            "..##.##.##\n" +
            "###.##.#..\n" +
            "\n" +
            "Tile 2473:\n" +
            "#....####.\n" +
            "#..#.##...\n" +
            "#.##..#...\n" +
            "######.#.#\n" +
            ".#...#.#.#\n" +
            ".#########\n" +
            ".###.#..#.\n" +
            "########.#\n" +
            "##...##.#.\n" +
            "..###.#.#.\n" +
            "\n" +
            "Tile 2971:\n" +
            "..#.#....#\n" +
            "#...###...\n" +
            "#.#.###...\n" +
            "##.##..#..\n" +
            ".#####..##\n" +
            ".#..####.#\n" +
            "#..#.#..#.\n" +
            "..####.###\n" +
            "..#.#.###.\n" +
            "...#.#.#.#\n" +
            "\n" +
            "Tile 2729:\n" +
            "...#.#.#.#\n" +
            "####.#....\n" +
            "..#.#.....\n" +
            "....#..#.#\n" +
            ".##..##.#.\n" +
            ".#.####...\n" +
            "####.#.#..\n" +
            "##.####...\n" +
            "##..#.##..\n" +
            "#.##...##.\n" +
            "\n" +
            "Tile 3079:\n" +
            "#.#.#####.\n" +
            ".#..######\n" +
            "..#.......\n" +
            "######....\n" +
            "####.#..#.\n" +
            ".#...#.##.\n" +
            "#.#####.##\n" +
            "..#.###...\n" +
            "..#.......\n" +
            "..#.###...";

        [TestCategory("Test")]
        [DataTestMethod]
        public void Jigsaw1Test()
        {
            Assert.AreEqual(20899048083289L, Day20.Part1(test));
        }

        [TestCategory("Test")]
        [DataTestMethod]
        public void Jigsaw2Test()
        {
            Assert.AreEqual(273, Day20.Part2(test));
        }

        /*

        0    1    2    3    4    5    6    7

        0A1  3D0  2C3  1B2  0D3  1A0  2B1  3C2
        DXB  CXA  BXD  AXC  AXC  BXD  CXA  DXB
        3C2  2B1  1A0  0D3  1B2  2C3  3D0  0A1

        */

        [TestCategory("Test")]
        [DataRow("Tile 0000:\n0A1\nDXB\n3C2", 0, "0A1\nDXB\n3C2")]
        [DataRow("Tile 0000:\n0A1\nDXB\n3C2", 1, "3D0\nCXA\n2B1")]
        [DataRow("Tile 0000:\n0A1\nDXB\n3C2", 2, "2C3\nBXD\n1A0")]
        [DataRow("Tile 0000:\n0A1\nDXB\n3C2", 3, "1B2\nAXC\n0D3")]
        [DataRow("Tile 0000:\n0A1\nDXB\n3C2", 4, "0D3\nAXC\n1B2")]
        [DataRow("Tile 0000:\n0A1\nDXB\n3C2", 5, "1A0\nBXD\n2C3")]
        [DataRow("Tile 0000:\n0A1\nDXB\n3C2", 6, "2B1\nCXA\n3D0")]
        [DataRow("Tile 0000:\n0A1\nDXB\n3C2", 7, "3C2\nDXB\n0A1")]

        [DataRow("Tile 0000:\n0A1\nDXB\n3C2", 8, "0A1\nDXB\n3C2")]
        [DataRow("Tile 0000:\n0A1\nDXB\n3C2", 9, "3D0\nCXA\n2B1")]
        [DataTestMethod]
        public void RotationTest(string input, int orientation, string expected)
        {
            var tile = new Day20.Tile(input);
            for (var i = 0; i < orientation; ++i)
            {
                tile.Twizzle();
            }
            Assert.AreEqual(orientation % 8, tile.Orientation);
            Assert.AreEqual(expected, tile.Transformed().Trim().Replace("\r", ""));
            Assert.AreEqual(tile.Edges[0], tile.Transformed().Substring(0, 3)); // top edge should match transform
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Jigsaw_Part1_Regression()
        {
            Assert.AreEqual(8581320593371L, Day20.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Jigsaw_Part2_Regression()
        {
            Assert.AreEqual(2031, Day20.Part2(input));
        }
    }
}

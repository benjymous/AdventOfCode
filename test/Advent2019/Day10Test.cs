using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2019.Test
{
    [TestCategory("2019")]
    [TestCategory("ManhattanVector")]
    [TestClass]
    public class Day10Test
    {
        readonly string input = Util.GetInput<Day10>();

        [TestCategory("Test")]
        [DataRow(".#..#,.....,#####,....#,...##", 8)]
        [DataRow("......#.#.,#..#.#....,..#######.,.#.#.###..,.#..#.....,..#....#.#,#..#....#.,.##.#..###,##...#..#.,.#....####", 33)]
        [DataRow("#.#...#.#.,.###....#.,.#....#...,##.#.#.#.#,....#.#.#.,.##..###.#,..#...##..,..##....##,......#...,.####.###.", 35)]
        [DataRow(".#..#..###,####.###.#,....###.#.,..###.##.#,##.##.#.#.,....###..#,..#.#..#.#,#..#.#.###,,.##...##.#,.....#.#..", 41)]
        [DataRow(".#..##.###...#######,##.############..##.,.#.######.########.#,.###.#######.####.#.,#####.##.#.##.###.##,..#####..#.#########,####################,#.####....###.#.#.##,##.#################,#####.##.###..####..,..######..##.#######,####.##.####...##..#,.#####..#.######.###,##...#.##########...,#.##########.#######,.####.#.###.###.#.##,....##.##.###..#####,.#.#.###########.###,#.#.#.#####.####.###,###.##.####.##.#..##", 210)]
        [DataTestMethod]
        public void AsteroidsTest(string input, int expected)
        {
            Assert.AreEqual(expected, Day10.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Asteroids_Part1_Regression()
        {
            Assert.AreEqual(278, Day10.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Asteroids_Part2_Regression()
        {
            Assert.AreEqual(1417, Day10.Part2(input, 200));
        }
    }
}

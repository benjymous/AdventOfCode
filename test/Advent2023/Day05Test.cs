using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2023.Test;

[TestCategory("2023")]
[TestClass]
public class Day05Test
{
    readonly string input = Util.GetInput<Day05>();

    readonly string test =
@"seeds: 79 14 55 13

seed-to-soil map:
50 98 2
52 50 48

soil-to-fertilizer map:
0 15 37
37 52 2
39 0 15

fertilizer-to-water map:
49 53 8
0 11 42
42 0 7
57 7 4

water-to-light map:
88 18 7
18 25 70

light-to-temperature map:
45 77 23
81 45 19
68 64 13

temperature-to-humidity map:
0 69 1
1 0 69

humidity-to-location map:
60 56 37
56 93 4".Replace("\r", "");

    /*
        //Util.Test(factory.Remap("seed", "soil", 79), 81);
        //Util.Test(factory.Remap("seed", "soil", 14), 14);
        //Util.Test(factory.Remap("seed", "soil", 55), 57);
        //Util.Test(factory.Remap("seed", "soil", 13), 13);

        //Util.Test(factory.Remap("seed", "location", 79), 82);
        //Util.Test(factory.Remap("seed", "location", 14), 43);
        //Util.Test(factory.Remap("seed", "location", 55), 86);
        //Util.Test(factory.Remap("seed", "location", 13), 35);
    */

    [TestCategory("Test")]
    [TestMethod]
    public void Seeds_01Test()
    {
        Assert.AreEqual(35, Day05.Part1(test));
    }

    [TestCategory("Test")]
    [TestMethod]
    public void Seeds_02Test()
    {
        Assert.AreEqual(46, Day05.Part2(test));
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void SeedMapping_Part1_Regression()
    {
        Assert.AreEqual(289863851, Day05.Part1(input));
    }

    [TestCategory("Regression")]
    [DataTestMethod]
    public void SeedMapping_Part2_Regression()
    {
        // Assert.AreEqual(60568880, Day05.Part2(input));
    }
}


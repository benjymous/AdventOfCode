using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace Advent.MMXIX.Test
{
    [TestCategory("2019")]
    [TestClass]
    public class Day08Test
    {
        string input = Util.GetInput<Day08>();

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Image_Part1_Regression()
        {
            Assert.AreEqual(1560, Day08.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Image_Part2_Regression()
        {     
            const string expectedHash = "83314289071513F5D831987A2AE99D107ACF077F8A950D9D9EBA5437B8434833";
            Assert.AreEqual(expectedHash, Day08.Part2(input, new ConsoleOut()));
        }

    }
}

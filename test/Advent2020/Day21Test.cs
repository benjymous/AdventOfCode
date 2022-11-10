using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2020.Test
{
    [TestCategory("2020")]
    [TestClass]
    public class Day21Test
    {
        readonly string input = Util.GetInput<Day21>();

        readonly string test = "mxmxvkd kfcds sqjhc nhms (contains dairy, fish)\n" +
            "trh fvjkl sbzzf mxmxvkd (contains dairy)\n" +
            "sqjhc fvjkl (contains soy)\n" +
            "sqjhc mxmxvkd sbzzf (contains fish)";

        [TestCategory("Test")]
        [DataTestMethod]
        public void Allergens1Test()
        {
            Assert.AreEqual(5, Day21.Part1(test));
        }

        [TestCategory("Test")]
        [DataTestMethod]
        public void Allergens2Test()
        {
            Assert.AreEqual("mxmxvkd,sqjhc,fvjkl", Day21.Part2(test));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Allergens_Part1_Regression()
        {
            Assert.AreEqual(2061, Day21.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Allergens_Part2_Regression()
        {
            Assert.AreEqual("cdqvp,dglm,zhqjs,rbpg,xvtrfz,tgmzqjz,mfqgx,rffqhl", Day21.Part2(input));
        }
    }
}

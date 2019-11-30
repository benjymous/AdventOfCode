using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Advent.Test
{
    [TestClass]
    public class UtilTest
    {
        [DataRow("a", "a")]
        [DataRow("a\nb", "a|b")]
        [DataRow("a,b,c", "a|b|c")]
        [DataTestMethod]
        public void TestSplit(string input, string expected)
        {
            string[] result = Util.Split(input);
            string joined = string.Join("|", result);
            Assert.IsTrue(joined == expected);
        }
    }
}

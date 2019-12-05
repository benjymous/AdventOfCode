using Microsoft.VisualStudio.TestTools.UnitTesting;

[assembly: Parallelize(Workers = 0, Scope = ExecutionScope.MethodLevel)]

namespace Advent.Test
{
    [TestClass]
    public class UtilTest
    {
        [DataRow("a", "a")]
        [DataRow("a\nb", "a|b")]
        [DataRow("a\nb\nc", "a|b|c")]
        [DataRow("a,b,c", "a|b|c")]
        [DataTestMethod]
        public void TestSplit(string input, string expected)
        {
            string[] result = Util.Split(input);
            string joined = string.Join("|", result);
            Assert.AreEqual(expected, joined);
        }

        [TestMethod]
        public void TestManhattanEquality()
        {
            ManhattanVector2 a = new ManhattanVector2(0,0);
            ManhattanVector2 b = new ManhattanVector2(0,0);

            Assert.AreEqual(a,b);

            a.Offset(10,0);
            b = b + new ManhattanVector2(10,0);

            Assert.AreEqual(a,b);
        }

        [DataRow(0,0, 10,0, 10)]
        [DataRow(0,0, 10,10, 20)]
        [DataRow(0,0, 0,10, 10)]
        [DataRow(-100,-100, 100,100, 400)]
        [DataTestMethod]
        public void TestManhattanDistance(int x1, int y1, int x2, int y2, int expected)
        {
            ManhattanVector2 a = new ManhattanVector2(x1,y1);
            ManhattanVector2 b = new ManhattanVector2(x2,y2);  

            Assert.AreEqual(expected, a.Distance(b));
            Assert.AreEqual(expected, b.Distance(a));
        }
    }
}

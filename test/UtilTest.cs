using AoC.Utils;
using AoC.Utils.Collections;
using AoC.Utils.Vectors;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

[assembly: Parallelize(Workers = 0, Scope = ExecutionScope.MethodLevel)]

namespace AoC.Test
{
    [TestCategory("Test")]
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
            ManhattanVector2 a = new ManhattanVector2(0, 0);
            ManhattanVector2 b = new ManhattanVector2(0, 0);

            Assert.AreEqual(a, b);

            a.Offset(10, 0);
            b = b + new ManhattanVector2(10, 0);

            Assert.AreEqual(a, b);
        }

        [DataRow(0, 0, 10, 0, 10)]
        [DataRow(0, 0, 10, 10, 20)]
        [DataRow(0, 0, 0, 10, 10)]
        [DataRow(-100, -100, 100, 100, 400)]
        [DataTestMethod]
        public void TestManhattan2Distance(int x1, int y1, int x2, int y2, int expected)
        {
            ManhattanVector2 a = new ManhattanVector2(x1, y1);
            ManhattanVector2 b = new ManhattanVector2(x2, y2);

            Assert.AreEqual(expected, a.Distance(b));
            Assert.AreEqual(expected, b.Distance(a));
        }

        [DataRow(0, 0, 0, 10, 0, 0, 10)]
        [DataRow(0, 0, 0, 10, 10, 10, 30)]
        [DataRow(0, 0, 0, 0, 10, 0, 10)]
        [DataRow(0, 0, 0, 0, 0, 10, 10)]
        [DataRow(-100, -100, -100, 100, 100, 100, 600)]
        [DataTestMethod]
        public void TestManhattan3Distance(int x1, int y1, int z1, int x2, int y2, int z2, int expected)
        {
            ManhattanVector3 a = new ManhattanVector3(x1, y1, z1);
            ManhattanVector3 b = new ManhattanVector3(x2, y2, z2);

            Assert.AreEqual(expected, a.Distance(b));
            Assert.AreEqual(expected, b.Distance(a));
        }

        [DataRow(0, 0, 0, 0, 10, 0, 0, 0, 10)]
        [DataRow(0, 0, 0, 0, 10, 10, 10, 10, 40)]
        [DataRow(0, 0, 0, 0, 0, 10, 0, 0, 10)]
        [DataRow(0, 0, 0, 0, 0, 0, 10, 0, 10)]
        [DataRow(0, 0, 0, 0, 0, 0, 0, 10, 10)]
        [DataRow(-100, -100, -100, -100, 100, 100, 100, 100, 800)]
        [DataTestMethod]
        public void TestManhattan4Distance(int x1, int y1, int z1, int w1, int x2, int y2, int z2, int w2, int expected)
        {
            ManhattanVector4 a = new ManhattanVector4(x1, y1, z1, w1);
            ManhattanVector4 b = new ManhattanVector4(x2, y2, z2, w2);

            Assert.AreEqual(expected, a.Distance(b));
            Assert.AreEqual(expected, b.Distance(a));
        }

        [DataTestMethod]
        public void CircleCreateTest()
        {
            var circle = Circle<int>.Create(Enumerable.Range(0, 10));

            Assert.AreEqual("0,1,2,3,4,5,6,7,8,9", string.Join(',', circle.Values()));
        }

        [DataTestMethod]
        public void CircleRemoveTest()
        {
            var circle = Circle<int>.Create(Enumerable.Range(0, 10));

            circle = circle.Remove(5);

            Assert.AreEqual("5,6,7,8,9", string.Join(',', circle.Values()));
        }

        [DataTestMethod]
        public void CircleReverseTest()
        {
            var circle = Circle<int>.Create(Enumerable.Range(0, 10));

            circle = circle.Reverse(5);

            Assert.AreEqual("4,3,2,1,0,5,6,7,8,9", string.Join(',', circle.Values()));
        }

        [DataTestMethod]
        public void CircleReverseTest2()
        {
            var circle = Circle<int>.Create(Enumerable.Range(0, 10));

            circle.Forward(3).Reverse(5);

            Assert.AreEqual("0,1,2,7,6,5,4,3,8,9", string.Join(',', circle.Values()));
        }


        [DataTestMethod]
        public void XorTest()
        {
            var nums = new List<int> { 65, 27, 9, 1, 4, 3, 40, 50, 91, 7, 6, 0, 2, 5, 68, 22 };

            Assert.AreEqual(64, nums.Xor());
        }


        [DataTestMethod]
        [DataRow(1, "01")]
        [DataRow(10, "0a")]
        [DataRow(15, "0f")]
        [DataRow(255, "ff")]
        public void HexTest(int val, string expected)
        {
            Assert.AreEqual(expected, val.ToHex());
        }

    }
}

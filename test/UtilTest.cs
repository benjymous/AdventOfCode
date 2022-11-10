using AoC.Utils;
using AoC.Utils.Collections;
using AoC.Utils.Vectors;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.ComponentModel;
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
        [TestCategory("ManhattanVector")]
        public void TestManhattanEquality()
        {
            ManhattanVector2 a = new(0, 0);
            ManhattanVector2 b = new(0, 0);

            Assert.AreEqual(a, b);

            a.Offset(10, 0);
            b += new ManhattanVector2(10, 0);

            Assert.AreEqual(a, b);
        }

        [DataRow(0, 0, 10, 0, 10)]
        [DataRow(0, 0, 10, 10, 20)]
        [DataRow(0, 0, 0, 10, 10)]
        [DataRow(-100, -100, 100, 100, 400)]
        [DataTestMethod]
        [TestCategory("ManhattanVector")]
        [TestCategory("ManhattanVector2")]
        public void TestManhattan2Distance(int x1, int y1, int x2, int y2, int expected)
        {
            ManhattanVector2 a = new(x1, y1);
            ManhattanVector2 b = new(x2, y2);

            Assert.AreEqual(expected, a.Distance(b));
            Assert.AreEqual(expected, b.Distance(a));
        }

        [DataRow(0, 0, 0, 10, 0, 0, 10)]
        [DataRow(0, 0, 0, 10, 10, 10, 30)]
        [DataRow(0, 0, 0, 0, 10, 0, 10)]
        [DataRow(0, 0, 0, 0, 0, 10, 10)]
        [DataRow(-100, -100, -100, 100, 100, 100, 600)]
        [DataTestMethod]
        [TestCategory("ManhattanVector")]
        [TestCategory("ManhattanVector3")]
        public void TestManhattan3Distance(int x1, int y1, int z1, int x2, int y2, int z2, int expected)
        {
            ManhattanVector3 a = new(x1, y1, z1);
            ManhattanVector3 b = new(x2, y2, z2);

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
        [TestCategory("ManhattanVector")]
        [TestCategory("ManhattanVector4")]
        public void TestManhattan4Distance(int x1, int y1, int z1, int w1, int x2, int y2, int z2, int w2, int expected)
        {
            ManhattanVector4 a = new(x1, y1, z1, w1);
            ManhattanVector4 b = new(x2, y2, z2, w2);

            Assert.AreEqual(expected, a.Distance(b));
            Assert.AreEqual(expected, b.Distance(a));
        }

        [DataTestMethod]
        [TestCategory("Circle")]
        public void CircleCreateTest()
        {
            var circle = Circle<int>.Create(Enumerable.Range(0, 10));

            Assert.AreEqual("0,1,2,3,4,5,6,7,8,9", string.Join(',', circle.Values()));
        }

        [DataTestMethod]
        [TestCategory("Circle")]
        public void CircleRemoveTest()
        {
            var circle = Circle<int>.Create(Enumerable.Range(0, 10));

            circle = circle.Remove(5);

            Assert.AreEqual("5,6,7,8,9", string.Join(',', circle.Values()));
        }

        [DataTestMethod]
        [TestCategory("Circle")]
        public void CircleReverseTest()
        {
            var circle = Circle<int>.Create(Enumerable.Range(0, 10));

            circle = circle.Reverse(5);

            Assert.AreEqual("4,3,2,1,0,5,6,7,8,9", string.Join(',', circle.Values()));
        }

        [DataTestMethod]
        [TestCategory("Circle")]
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

        class RegexCreationTest1
        {
            [Regex(@"(.+,.+) (.+,.+,.+) (.+)")]
            public RegexCreationTest1(ManhattanVector2 a, ManhattanVector3 b, int c)
            {
                A = a;
                B = b;
                C = c;
            }

            public ManhattanVector2 A;
            public ManhattanVector3 B;
            public int C;
        }

        [TestMethod]
        [TestCategory("RegexParse")]
        public void RegexParseTest()
        {
            var list = Util.RegexParse<RegexCreationTest1>("10,10 20,20,5 32\n1,2 3,4,5 6\n0,0 1,1,1 2", "\n").ToArray();
            Assert.AreEqual(3, list.Length);

            Assert.AreEqual(new ManhattanVector2(10, 10), list[0].A);
            Assert.AreEqual(new ManhattanVector3(20, 20, 5), list[0].B);
            Assert.AreEqual(32, list[0].C);

            Assert.AreEqual(new ManhattanVector2(1, 2), list[1].A);
            Assert.AreEqual(new ManhattanVector3(3, 4, 5), list[1].B);
            Assert.AreEqual(6, list[1].C);

            Assert.AreEqual(new ManhattanVector2(0, 0), list[2].A);
            Assert.AreEqual(new ManhattanVector3(1, 1, 1), list[2].B);
            Assert.AreEqual(2, list[2].C);
        }

        [TestMethod]
        [TestCategory("RegexParse")]
        public void RegexCreateTest()
        {
            var obj = Util.RegexCreate<RegexCreationTest1>("10,10 20,20,5 32");
            Assert.AreEqual(new ManhattanVector2(10, 10), obj.A);
            Assert.AreEqual(new ManhattanVector3(20, 20,5), obj.B);
            Assert.AreEqual(32, obj.C);
        }

        [TestMethod]
        public void TypeConversionTest()
        {
            var obj = (ManhattanVector2)TypeDescriptor.GetConverter(typeof(ManhattanVector2)).ConvertFromString("1,2");
            Assert.AreEqual(new ManhattanVector2(1, 2), obj);
        }
    }
}

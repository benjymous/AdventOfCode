using AoC.Utils;
using AoC.Utils.Collections;
using AoC.Utils.Parser;
using AoC.Utils.Vectors;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

[assembly: Parallelize(Workers = 64, Scope = ExecutionScope.ClassLevel)]

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

            Assert.AreEqual("0,1,2,3,4,5,6,7,8,9", string.Join(',', circle));
        }

        [DataTestMethod]
        [TestCategory("Circle")]
        public void CircleRemoveTest()
        {
            var circle = Circle<int>.Create(Enumerable.Range(0, 10));

            circle = circle.Remove(5);

            Assert.AreEqual("5,6,7,8,9", string.Join(',', circle));
        }

        [DataTestMethod]
        [TestCategory("Circle")]
        public void CircleReverseTest()
        {
            var circle = Circle<int>.Create(Enumerable.Range(0, 10));

            circle = circle.Reverse(5);

            Assert.AreEqual("4,3,2,1,0,5,6,7,8,9", string.Join(',', circle));
        }

        [DataTestMethod]
        [TestCategory("Circle")]
        public void CircleReverseTest2()
        {
            var circle = Circle<int>.Create(Enumerable.Range(0, 10));

            circle.Forward(3).Reverse(5);

            Assert.AreEqual("0,1,2,7,6,5,4,3,8,9", string.Join(',', circle));
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

        [method: Regex(@"(.+,.+) (.+,.+,.+) (.+)")]
        class RegexCreationTest1(ManhattanVector2 a, ManhattanVector3 b, int c)
        {
            public ManhattanVector2 A = a;
            public ManhattanVector3 B = b;
            public int C = c;
        }

        [method: Regex(@"(.+) (.+) (.+)")]
        class NestedRegexCreationTest(NestedRegexCreationTestInner a, NestedRegexCreationTestInner b, int c)
        {
            public NestedRegexCreationTestInner A = a;
            public NestedRegexCreationTestInner B = b;
            public int C = c;
        }

        [method: Regex(@"(.+)")]

        class NestedRegexCreationTestInner(string val)
        {
            public string Val = val;
        }

        [TestMethod]
        [TestCategory("RegexParse")]
        public void RegexParseTest()
        {
            var list = Parser.Parse<RegexCreationTest1>("10,10 20,20,5 32\n1,2 3,4,5 6\n0,0 1,1,1 2", "\n").ToArray();
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
        public void NestedRegexParseTest()
        {
            var list = Parser.Parse<NestedRegexCreationTest>("One Two 32\nA B 6\nhello there 2", "\n").ToArray();
            Assert.AreEqual(3, list.Length);

            Assert.AreEqual("One", list[0].A.Val);
            Assert.AreEqual("Two", list[0].B.Val);
            Assert.AreEqual(32, list[0].C);

            Assert.AreEqual("A", list[1].A.Val);
            Assert.AreEqual("B", list[1].B.Val);
            Assert.AreEqual(6, list[1].C);

            Assert.AreEqual("hello", list[2].A.Val);
            Assert.AreEqual("there", list[2].B.Val);
            Assert.AreEqual(2, list[2].C);
        }

        [TestMethod]
        [TestCategory("RegexParse")]
        public void RegexCreateTest()
        {
            var obj = Parser.FromString<RegexCreationTest1>("10,10 20,20,5 32");
            Assert.AreEqual(new ManhattanVector2(10, 10), obj.A);
            Assert.AreEqual(new ManhattanVector3(20, 20, 5), obj.B);
            Assert.AreEqual(32, obj.C);
        }

        [method: Regex(@"(.+) (.+) (.+)")]
        class RegexCreationTest2(int[] a1, long[] a2, string[] a3)
        {
            public int[] A1 = a1;
            public long[] A2 = a2;
            public string[] A3 = a3;
        }

        [TestMethod]
        [TestCategory("RegexParse")]
        public void RegexCreateTest2()
        {
            var obj = Parser.FromString<RegexCreationTest2>("1,2,3 100,200,300 tom,dick,harry");
            Assert.AreEqual(3, obj.A1.Length);
            Assert.AreEqual(1, obj.A1[0]);
            Assert.AreEqual(2, obj.A1[1]);
            Assert.AreEqual(3, obj.A1[2]);

            Assert.AreEqual(3, obj.A2.Length);
            Assert.AreEqual(100L, obj.A2[0]);
            Assert.AreEqual(200L, obj.A2[1]);
            Assert.AreEqual(300L, obj.A2[2]);

            Assert.AreEqual(3, obj.A3.Length);
            Assert.AreEqual("tom", obj.A3[0]);
            Assert.AreEqual("dick", obj.A3[1]);
            Assert.AreEqual("harry", obj.A3[2]);
        }

        class RegexCreationTestNoConstructor
        { }

        [TestMethod]
        [TestCategory("RegexParse")]
        public void RegexCreateFailTest()
        {
            Assert.ThrowsException<Exception>(() => Parser.FromString<RegexCreationTestNoConstructor>("blah"));
        }

        [method: Regex(@"(.+) (.+)")]
        class ArrayRegexCreationTest([Split(",")] NestedRegexCreationTestInner[] a, int c)
        {
            public NestedRegexCreationTestInner[] A = a;
            public int C = c;
        }

        [TestMethod]
        [TestCategory("RegexParse")]
        public void ArrayRegexCreateTest()
        {
            var item = Parser.FromString<ArrayRegexCreationTest>("One,Two 32");
            Assert.AreEqual(2, item.A.Length);

            Assert.AreEqual("One", item.A[0].Val);
            Assert.AreEqual("Two", item.A[1].Val);
            Assert.AreEqual(32, item.C);
        }

        [method: Regex(@"(.) (.+)")]
        class RegexCreationDictionary(int a1, [Split(", ", @"(?<key>.+): (?<value>.+)")] Dictionary<int, int> a2)
        {
            public int A1 = a1;
            public Dictionary<int, int> A2 = a2;
        }

        [TestMethod]
        [TestCategory("RegexParse")]
        public void RegexCreateDictionary()
        {
            var obj = Parser.FromString<RegexCreationDictionary>("1 2: 3, 4: 5, 6: 7");
            Assert.AreEqual(1, obj.A1);
            Assert.AreEqual(3, obj.A2.Count);
            Assert.IsTrue(obj.A2.TryGetValue(2, out var v) && v == 3);
            Assert.IsTrue(obj.A2.TryGetValue(4, out v) && v == 5);
            Assert.IsTrue(obj.A2.TryGetValue(6, out v) && v == 7);
        }

        [method: Regex(@"(.) (.+)")]
        class RegexCreationList(int a1, [Split(":")] List<int> a2)
        {
            public int A1 = a1;
            public List<int> A2 = a2;
        }

        [TestMethod]
        [TestCategory("RegexParse")]
        public void RegexCreateList()
        {
            var obj = Parser.FromString<RegexCreationList>("1 2:3:4:5:6:7");
            Assert.AreEqual(1, obj.A1);
            Assert.AreEqual(6, obj.A2.Count);
            Assert.AreEqual(2, obj.A2[0]);
            Assert.AreEqual(3, obj.A2[1]);
            Assert.AreEqual(4, obj.A2[2]);
            Assert.AreEqual(5, obj.A2[3]);
            Assert.AreEqual(6, obj.A2[4]);
            Assert.AreEqual(7, obj.A2[5]);
        }

        [method: Regex(@"(.) (.+)")]
        class RegexCreationHashSet(int a1, [Split(":")] HashSet<int> a2)
        {
            public int A1 = a1;
            public HashSet<int> A2 = a2;
        }

        [TestMethod]
        [TestCategory("RegexParse")]
        public void RegexCreateHashSet()
        {
            var obj = Parser.FromString<RegexCreationHashSet>("1 2:3:4:5:6:7:7:2");
            Assert.AreEqual(1, obj.A1);
            Assert.AreEqual(6, obj.A2.Count);
            Assert.IsTrue(obj.A2.Contains(2));
            Assert.IsTrue(obj.A2.Contains(3));
            Assert.IsTrue(obj.A2.Contains(4));
            Assert.IsTrue(obj.A2.Contains(5));
            Assert.IsTrue(obj.A2.Contains(6));
            Assert.IsTrue(obj.A2.Contains(7));
        }

        [Regex(@"(.+)")]
        record class BoolsRegexCreationTest([Split(",")] bool[] bools);

        [TestMethod]
        [TestCategory("RegexParse")]
        public void RegexParseBooleans()
        {
            var bools = Parser.FromString<BoolsRegexCreationTest>("true, false, True, False, TRUE, FALSE, t, f, T, F, yes, no, Yes, No, YES, NO, y, n, Y, N");
            Assert.AreEqual(20, bools.bools.Length);
            for (int i = 0; i < bools.bools.Length; ++i)
            {
                Assert.AreEqual(i % 2 == 0, bools.bools[i], $"wrong boolean at {i}");
            }
        }

        [Regex("(.+) : (.+)")]
        record class ParamRegexCreationTest([Regex("(.+),(.+)")] (int X, int Y) v1, [Regex("(.+),(.+)")] (int X, int Y) v2);

        [TestMethod]
        [TestCategory("RegexParse")]
        public void RegexParseParamRegex()
        {
            var result = Parser.FromString<ParamRegexCreationTest>("10,11 : 12,13");

            Assert.AreEqual((10, 11), result.v1);
            Assert.AreEqual((12, 13), result.v2);
        }

        [TestMethod]
        [TestCategory("AutoParse")]
        public void AutoParseTest()
        {
            var data = "10,11 : 12,13\n14,15 : 16,17";

            DoThing(data);

            static void DoThing(Parser.AutoArray<ParamRegexCreationTest> result)
            {
                Assert.AreEqual(2, result.Length);

                Assert.AreEqual((10, 11), result[0].v1);
                Assert.AreEqual((12, 13), result[0].v2);

                Assert.AreEqual((14, 15), result[1].v1);
                Assert.AreEqual((16, 17), result[1].v2);
            }
        }

        [TestMethod]
        public void ArrayDimensionTest()
        {
            var arr = new int[10, 5];
            Assert.AreEqual(10, arr.Width());
            Assert.AreEqual(5, arr.Height());
        }

        [TestMethod]
        public void ArrayColumnsTest()
        {
            var arr = new int[10, 5];
            for (int x = 0; x < arr.Width(); ++x)
                for (int y = 0; y < arr.Height(); ++y)
                    arr[x, y] = x + (y * 100);

            var col = arr.Column(1).ToArray();
            Assert.AreEqual(5, col.Length);
            for (int i = 0; i < col.Length; ++i)
            {
                Assert.AreEqual((i * 100) + 1, col[i]);
            }
        }

        [TestMethod]
        public void ArrayRowsTest()
        {
            var arr = new int[10, 5];
            for (int x = 0; x < arr.Width(); ++x)
                for (int y = 0; y < arr.Height(); ++y)
                    arr[x, y] = (x * 100) + y;

            var row = arr.Row(1).ToArray();
            Assert.AreEqual(10, row.Length);
            for (int i = 0; i < row.Length; ++i)
            {
                Assert.AreEqual((i * 100) + 1, row[i]);
            }
        }

        [TestMethod]
        public void WindowsTest()
        {
            var input = "abcdefghijklmnopqrstuvwxyz";
            var windows = input.Windows(4).Select(w => w.AsString()).ToArray();
            Assert.AreEqual(23, windows.Length);
            Assert.AreEqual("abcd", windows[0]);
            Assert.AreEqual("fghi", windows[5]);
            Assert.AreEqual("uvwx", windows[20]);
            Assert.AreEqual("wxyz", windows[22]);
            Assert.AreEqual("wxyz", windows.Last());
        }

        [TestMethod]
        public void NextMultipleTest()
        {
            Assert.AreEqual(20, Util.RoundUpToNextMultiple(11, 10));
            Assert.AreEqual(30, Util.RoundUpToNextMultiple(27, 10));
            Assert.AreEqual(75UL, Util.RoundUpToNextMultiple(73UL, 25UL));

        }
    }
}

using AoC.Utils.Strings;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace Test.Utils.Strings
{
    [TestCategory("Test")]
    [TestClass]
    public class PackedStringTest
    {
        [DataTestMethod]
        [TestCategory("PackedString")]
        [DataRow("aa")]
        [DataRow("ab")]
        [DataRow("ac")]
        [DataRow("zz")]
        [DataRow("?!")]
        public void TwoCCTest(string str)
        {
            PackedString<ushort, PackTwoCC> v1 = str;
            PackedString<ushort, PackTwoCC> v2 = new(str);

            ushort val = v1;
            ushort val2 = 12345;

            Dictionary<PackedString<ushort, PackTwoCC>, string> dict = [];
            dict["11"] = "11";
            dict["22"] = "22";
            dict[v1] = str;
            dict["ff"] = "ff";
            dict["<>"] = "<>";

            Assert.AreEqual(v1, v2);
            Assert.AreEqual(v1, str);
            Assert.AreEqual(str, v1);

            Assert.AreEqual(str, v1.ToString());

            Assert.AreNotEqual(v1, "blah");
            Assert.AreNotEqual("wibble", v1);

            Assert.IsTrue(v1 == val);
            Assert.IsTrue(val == v1);

            Assert.IsFalse(v1 == val2);
            Assert.IsFalse(val2 == v1);

            Assert.AreNotEqual("XX", v1);
            Assert.IsTrue(dict.TryGetValue(str, out var v) && v == str);
            Assert.AreEqual(5, dict.Count);
        }

        [TestMethod]
        [TestCategory("PackedString")]
        public void TwoCCSortingTest()
        {
            string[] strings = ["aa", "!!", "bb", "cc", "zz", "<>", "dd", "ee", "ab", "da", "  ", "ad"];

            var codes = strings.Select(s => (packed: (PackedString<ushort, PackTwoCC>)s, str: s));

            var arr1 = codes.OrderBy(v => v.packed).ToArray();
            var arr2 = codes.OrderBy(v => v.str).ToArray();

            CollectionAssert.AreEqual(arr1, arr2);
        }

        [DataTestMethod]
        [TestCategory("PackedString")]
        [DataRow("aaaa")]
        [DataRow("abcd")]
        [DataRow("acab")]
        [DataRow("zzzz")]
        [DataRow("?!@#")]
        public void FourCCTest(string str)
        {
            PackedString<uint, PackFourCC> v1 = str;
            PackedString<uint, PackFourCC> v2 = new(str);
            PackedString<uint, PackFourCC> v3 = new($"{str.Substring(0, 3)}-");

            uint val = v1;
            uint val2 = 12345;

            Dictionary<PackedString<uint, PackFourCC>, string> dict = [];
            dict["1111"] = "1111";
            dict["2222"] = "2222";
            dict[v1] = str;
            dict["ffff"] = "ffff";
            dict["<><>"] = "<><>";

            Assert.AreEqual(v1, v2);
            Assert.AreEqual(v1, str);
            Assert.AreEqual(str, v1);

            Assert.AreNotEqual(v1, "blah");
            Assert.AreNotEqual(v1, v3);
            Assert.AreNotEqual(v3, v1);
            Assert.AreNotEqual("wibble", v1);

            Assert.AreEqual(str, v1.ToString());

            Assert.IsTrue(v1 == val);
            Assert.IsTrue(val == v1);

            Assert.IsFalse(v1 == val2);
            Assert.IsFalse(val2 == v1);

            Assert.AreNotEqual("XXXX", v1);
            Assert.IsTrue(dict.TryGetValue(str, out var v) && v == str);
            Assert.AreEqual(5, dict.Count);
        }

        [TestMethod]
        [TestCategory("PackedString")]
        public void FourCCSortingTest()
        {
            string[] strings = ["aaaa", "!!!!", "bbbb", "cccc", "zzzz", "<<>>", "dddd", "eeee", "abcd", "dabc", "    ", "adab"];

            var codes = strings.Select(s => (packed: (PackedString<uint, PackFourCC>)s, str: s));

            var arr1 = codes.OrderBy(v => v.packed).ToArray();
            var arr2 = codes.OrderBy(v => v.str).ToArray();

            CollectionAssert.AreEqual(arr1, arr2);
        }

        [DataTestMethod]
        [TestCategory("PackedString")]
        [DataRow("aaaaaa")]
        [DataRow("abcdef")]
        [DataRow("acabde")]
        [DataRow("zzzzzz")]
        public void StringInt6Test(string str)
        {
            PackedString<int, PackAlphaInt6> v1 = str;
            PackedString<int, PackAlphaInt6> v2 = new(str);

            int val = v1;
            int val2 = 12345;

            Dictionary<PackedString<int, PackAlphaInt6>, string> dict = [];
            dict["1111"] = "1111";
            dict["2222"] = "2222";
            dict[v1] = str;
            dict["ffff"] = "ffff";
            dict["<><>"] = "<><>";

            Assert.AreEqual(v1, v2);
            Assert.AreEqual(v1, str);
            Assert.AreEqual(str, v1);

            Assert.AreEqual(str, v1.ToString());

            Assert.AreNotEqual(v1, "blah");
            Assert.AreNotEqual("wibble", v1);

            Assert.IsTrue(v1 == val);
            Assert.IsTrue(val == v1);

            Assert.IsFalse(v1 == val2);
            Assert.IsFalse(val2 == v1);

            Assert.AreNotEqual("XXXX", v1);
            Assert.IsTrue(dict.TryGetValue(str, out var v) && v == str);
            Assert.AreEqual(5, dict.Count);
        }

        [TestMethod]
        [TestCategory("PackedString")]
        public void StringInt6SortingTest()
        {
            string[] strings = ["aaaaaa", "bbbbbb", "cccccc", "zzzzzz", "dddddd", "eeeeee", "abcdef", "dabced", "adabab"];

            var codes = strings.Select(s => (packed: (PackedString<uint, PackFourCC>)s, str: s));

            var arr1 = codes.OrderBy(v => v.packed).ToArray();
            var arr2 = codes.OrderBy(v => v.str).ToArray();

            CollectionAssert.AreEqual(arr1, arr2);
        }
    }
}

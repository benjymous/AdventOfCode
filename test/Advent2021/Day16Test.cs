using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Text;
using static AoC.Advent2021.Day16;

namespace AoC.Advent2021.Test
{
    [TestCategory("2021")]
    [TestClass]
    public class Day16Test
    {
        readonly string input = Util.GetInput<Day16>();

        [TestCategory("Test")]
        [DataRow("D2FE28", "[6:LiteralValue = 2021]")]
        [DataRow("38006F45291200", "[1:LessThan {[6:LiteralValue = 10], [2:LiteralValue = 20]}]")]
        [DataRow("EE00D40C823060", "[7:Maximum {[2:LiteralValue = 1], [4:LiteralValue = 2], [1:LiteralValue = 3]}]")]
        [DataRow("8A004A801A8002F478", "[4:Minimum {[1:Minimum {[5:Minimum {[6:LiteralValue = 15]}]}]}]")]
        [DataTestMethod]
        public void TestPacketDecode(string data, string expected)
        {

            var packet = Day16.Packet.Parse(data);

            string str = ToString(packet);

            Assert.AreEqual(expected, str.ToString());
        }

        private static string ToString(Packet packet)
        {
            var res = new StringBuilder();

            res.Append($"[{packet.Header.Version}:{packet.Header.Type} ");

            if (packet.Header.Type == PacketType.LiteralValue)
            {
                res.Append($"= {packet.LiteralValue}");
            }
            else
            {
                res.Append('{');
                res.Append(string.Join(", ", packet.Children.Select(c => ToString(c))));
                res.Append('}');
            }

            res.Append(']');

            var str = res.ToString();
            return str;
        }

        [TestCategory("Test")]
        [DataRow("8A004A801A8002F478", 16)]
        [DataRow("620080001611562C8802118E34", 12)]
        [DataRow("C0015000016115A2E0802F182340", 23)]
        [DataRow("A0016C880162017C3686B18A3D4780", 31)]
        [DataTestMethod]
        public void TestPacketVersionSum(string data, int expected)
        {
            Assert.AreEqual(expected, Day16.Part1(data));
        }

        [TestCategory("Test")]
        [DataRow("C200B40A82", 3)]
        [DataRow("04005AC33890", 54)]
        [DataRow("880086C3E88112", 7)]
        [DataRow("CE00C43D881120", 9)]
        [DataRow("D8005AC2A8F0", 1)]
        [DataRow("F600BC2D8F", 0)]
        [DataRow("9C005AC2F8F0", 0)]
        [DataRow("9C0141080250320F1802104A08", 1)]
        [DataTestMethod]
        public void TestPacketValue(string data, int expected)
        {
            Assert.AreEqual(expected, Day16.Part2(data));
        }


        [TestCategory("Regression")]
        [DataTestMethod]
        public void Decoder_Part1_Regression()
        {
            Assert.AreEqual(917, Day16.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Decoder_Part2_Regression()
        {
            Assert.AreEqual(2536453523344, Day16.Part2(input));
        }
    }
}

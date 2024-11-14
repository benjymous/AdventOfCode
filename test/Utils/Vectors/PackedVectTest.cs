using AoC.Utils.Vectors;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Test.Utils.Vectors
{
    [TestCategory("Test")]
    [TestClass]
    public class PackedVectTest
    {

        [DataTestMethod]
        [TestCategory("PackedVect")]
        [DataRow(1, 1, 100)]
        [DataRow(1, 2, 200)]
        [DataRow(255, 255, 1000)]
        public void Packed8_8_16Test(int x, int y, int z)
        {
            PackedVect3<int, Pack8_8_16> packed = (x, y, z);

            Assert.AreEqual(x, packed.X);
            Assert.AreEqual(y, packed.Y);
            Assert.AreEqual(z, packed.Z);
        }
    }
}

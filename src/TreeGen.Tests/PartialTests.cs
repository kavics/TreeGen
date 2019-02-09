using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace TreeGen.Tests
{
    [TestClass]
    public class PartialTests
    {
        [TestMethod]
        public void Partial_IdToPathOffsets_2()
        {
            var result = TreeGeneratorAccessor.GetIdToTokenDividersAnfOffsets(2, 8);
            var dividers = result.Item1;
            var offsets = result.Item2;

            Assert.AreEqual("2,6,12,24,48,96,192,384", string.Join(",", dividers));
            Assert.AreEqual("0,3,9,21,45,93,189,381", string.Join(",", offsets));
        }
        [TestMethod]
        public void Partial_IdToPathOffsets_10()
        {
            var result = TreeGeneratorAccessor.GetIdToTokenDividersAnfOffsets(10, 8);
            var dividers = result.Item1;
            var offsets = result.Item2;

            Assert.AreEqual("10,110,1100,11000,110000,1100000,11000000,110000000", string.Join(",", dividers));
            Assert.AreEqual("0,11,121,1221,12221,122221,1222221,12222221", string.Join(",", offsets));
        }

    }

}

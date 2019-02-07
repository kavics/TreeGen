using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TreeGen.Tests
{
    [TestClass]
    public class TokenTests
    {
        [TestMethod]
        public void Pow()
        {
            Assert.AreEqual(4, TreeGenerator.Pow(2, 2));
            Assert.AreEqual(8, TreeGenerator.Pow(2, 3));
            Assert.AreEqual(1024, TreeGenerator.Pow(2, 10));

            Assert.AreEqual(64, TreeGenerator.Pow(4, 3));

            Assert.AreEqual(100000, TreeGenerator.Pow(10, 5));
        }

        [TestMethod]
        public void IdToToken_0()
        {
            Assert.AreEqual("R", TreeGenerator.IdToToken(0));
        }
        [TestMethod]
        public void IdToToken_Negative()
        {
            try
            {
                TreeGenerator.IdToToken(-1);
                Assert.Fail("ArgumentException was not thrown");
            }
            catch(ArgumentException e)
            {
                var msg = e.Message.ToLowerInvariant();
                Assert.IsTrue(e.Message.ToLowerInvariant().Contains("invalid id"));
            }
        }
        [TestMethod]
        public void IdToToken_OneDigit()
        {
            Assert.AreEqual("Ra", TreeGenerator.IdToToken(1));
            Assert.AreEqual("Rb", TreeGenerator.IdToToken(2));
            Assert.AreEqual("Rc", TreeGenerator.IdToToken(3, 3));
            Assert.AreEqual("Rd", TreeGenerator.IdToToken(4, 4));
            Assert.AreEqual("Re", TreeGenerator.IdToToken(5, 10));
            Assert.AreEqual("Rf", TreeGenerator.IdToToken(6, 10));
            Assert.AreEqual("Rg", TreeGenerator.IdToToken(7, 10));
            Assert.AreEqual("Rh", TreeGenerator.IdToToken(8, 10));
            Assert.AreEqual("Ri", TreeGenerator.IdToToken(9, 10));
            Assert.AreEqual("Rj", TreeGenerator.IdToToken(10, 10));
            Assert.AreEqual("Rz", TreeGenerator.IdToToken(26, 26));
        }
        [TestMethod]
        public void IdToToken_TooBigBase()
        {
            try
            {
                TreeGenerator.IdToToken(1, TreeGenerator.MaxBaseNumber + 1);
                Assert.Fail("NotSupportedException was not thrown");
            }
            catch (NotSupportedException e)
            {
                // do nothing
            }
        }

        [TestMethod]
        public void IdToToken_Base2_Level2()
        {
            Assert.AreEqual("R", TreeGenerator.IdToToken(0));
            Assert.AreEqual("Ra", TreeGenerator.IdToToken(1));
            Assert.AreEqual("Rb", TreeGenerator.IdToToken(2));
            Assert.AreEqual("RA", TreeGenerator.IdToToken(3));
            Assert.AreEqual("RAa", TreeGenerator.IdToToken(4));
            Assert.AreEqual("RAb", TreeGenerator.IdToToken(5));
            Assert.AreEqual("RB", TreeGenerator.IdToToken(6));
            Assert.AreEqual("RBa", TreeGenerator.IdToToken(7));
            Assert.AreEqual("RBb", TreeGenerator.IdToToken(8));
        }
        [TestMethod]
        public void IdToToken_Base2_Level3()
        {
            // level2 last
            Assert.AreEqual("RBb", TreeGenerator.IdToToken(8));
            // level3
            Assert.AreEqual("RAA", TreeGenerator.IdToToken(9));
            Assert.AreEqual("RAAa", TreeGenerator.IdToToken(10));
            Assert.AreEqual("RAAb", TreeGenerator.IdToToken(11));
            Assert.AreEqual("RAB", TreeGenerator.IdToToken(12));
            Assert.AreEqual("RABa", TreeGenerator.IdToToken(13));
            Assert.AreEqual("RABb", TreeGenerator.IdToToken(14));
            Assert.AreEqual("RBA", TreeGenerator.IdToToken(15));
            Assert.AreEqual("RBAa", TreeGenerator.IdToToken(16));
            Assert.AreEqual("RBAb", TreeGenerator.IdToToken(17));
            Assert.AreEqual("RBB", TreeGenerator.IdToToken(18));
            Assert.AreEqual("RBBa", TreeGenerator.IdToToken(19));
            Assert.AreEqual("RBBb", TreeGenerator.IdToToken(20));
        }
        [TestMethod]
        public void IdToToken_Base2_Level4()
        {
            // level3 last
            Assert.AreEqual("RBBb", TreeGenerator.IdToToken(20));
            // level4
            Assert.AreEqual("RAAA",  TreeGenerator.IdToToken(21));
            Assert.AreEqual("RAAAa", TreeGenerator.IdToToken(22));
            Assert.AreEqual("RAAAb", TreeGenerator.IdToToken(23));
            Assert.AreEqual("RAAB",  TreeGenerator.IdToToken(24));
            Assert.AreEqual("RAABa", TreeGenerator.IdToToken(25));
            Assert.AreEqual("RAABb", TreeGenerator.IdToToken(26));
            Assert.AreEqual("RABA",  TreeGenerator.IdToToken(27));
            Assert.AreEqual("RABAa", TreeGenerator.IdToToken(28));
            Assert.AreEqual("RABAb", TreeGenerator.IdToToken(29));
            Assert.AreEqual("RABB",  TreeGenerator.IdToToken(30));
            Assert.AreEqual("RABBa", TreeGenerator.IdToToken(31));
            Assert.AreEqual("RABBb", TreeGenerator.IdToToken(32));
            Assert.AreEqual("RBAA",  TreeGenerator.IdToToken(33));
            Assert.AreEqual("RBAAa", TreeGenerator.IdToToken(34));
            Assert.AreEqual("RBAAb", TreeGenerator.IdToToken(35));
            Assert.AreEqual("RBAB",  TreeGenerator.IdToToken(36));
            Assert.AreEqual("RBABa", TreeGenerator.IdToToken(37));
            Assert.AreEqual("RBABb", TreeGenerator.IdToToken(38));
            Assert.AreEqual("RBBA",  TreeGenerator.IdToToken(39));
            Assert.AreEqual("RBBAa", TreeGenerator.IdToToken(40));
            Assert.AreEqual("RBBAb", TreeGenerator.IdToToken(41));
            Assert.AreEqual("RBBB",  TreeGenerator.IdToToken(42));
            Assert.AreEqual("RBBBa", TreeGenerator.IdToToken(43));
            Assert.AreEqual("RBBBb", TreeGenerator.IdToToken(44));
        }
        [TestMethod]
        public void IdToToken_Base2_Transitions()
        {
            // level0
            Assert.AreEqual("R", TreeGenerator.IdToToken(0)); // only one
            // level1
            Assert.AreEqual("Ra", TreeGenerator.IdToToken(1)); // first
            Assert.AreEqual("Rb", TreeGenerator.IdToToken(2)); // last
            // level2
            Assert.AreEqual("RA", TreeGenerator.IdToToken(3)); // first
            Assert.AreEqual("RBb", TreeGenerator.IdToToken(8)); // last
            // level3
            Assert.AreEqual("RAA", TreeGenerator.IdToToken(9)); // first
            Assert.AreEqual("RBBb", TreeGenerator.IdToToken(20)); // last
            // level4
            Assert.AreEqual("RAAA", TreeGenerator.IdToToken(21)); // first
            Assert.AreEqual("RBBBb", TreeGenerator.IdToToken(44)); // last
            // level5
            Assert.AreEqual("RAAAA", TreeGenerator.IdToToken(45)); // first
            Assert.AreEqual("RBBBBb", TreeGenerator.IdToToken(92)); // last
            // level6
            Assert.AreEqual("RAAAAA", TreeGenerator.IdToToken(93)); // first
            Assert.AreEqual("RBBBBBb", TreeGenerator.IdToToken(188)); // last
            // level7
            Assert.AreEqual("RAAAAAA", TreeGenerator.IdToToken(189)); // first
            Assert.AreEqual("RBBBBBBb", TreeGenerator.IdToToken(380)); // last
            // level8
            Assert.AreEqual("RAAAAAAA", TreeGenerator.IdToToken(381)); // first
            Assert.AreEqual("RBBBBBBBb", TreeGenerator.IdToToken(764)); // last
            // level9
            Assert.AreEqual("RAAAAAAAA", TreeGenerator.IdToToken(765)); // first
            Assert.AreEqual("RBBBBBBBBb", TreeGenerator.IdToToken(1532)); // last
            // level10
            Assert.AreEqual("RAAAAAAAAA", TreeGenerator.IdToToken(1533)); // first
            Assert.AreEqual("RBBBBBBBBBb", TreeGenerator.IdToToken(3068)); // last
        }

        /* ================================================= */

        [TestMethod]
        public void TokenToId_R()
        {
            Assert.AreEqual(0, TreeGenerator.TokenToId("R"));
        }
        [TestMethod]
        public void TokenToId_Invalid()
        {
            try
            {
                TreeGenerator.TokenToId("w");
                Assert.Fail("ArgumentException was not thrown");
            }
            catch (ArgumentException e)
            {
                var msg = e.Message.ToLowerInvariant();
                Assert.IsTrue(e.Message.ToLowerInvariant().Contains("invalid token"));
            }
        }
        [TestMethod]
        public void TokenToId_RA()
        {
            Assert.AreEqual(1, TreeGenerator.TokenToId("Ra"));
            Assert.AreEqual(2, TreeGenerator.TokenToId("Rb"));
        }

        [TestMethod]
        public void TokenToId_Base2_Level2()
        {
            Assert.AreEqual(0, TreeGenerator.TokenToId("R"));
            Assert.AreEqual(1, TreeGenerator.TokenToId("Ra"));
            Assert.AreEqual(2, TreeGenerator.TokenToId("Rb"));
            Assert.AreEqual(3, TreeGenerator.TokenToId("RA"));
            Assert.AreEqual(4, TreeGenerator.TokenToId("RAa"));
            Assert.AreEqual(5, TreeGenerator.TokenToId("RAb"));
            Assert.AreEqual(6, TreeGenerator.TokenToId("RB"));
            Assert.AreEqual(7, TreeGenerator.TokenToId("RBa"));
            Assert.AreEqual(8, TreeGenerator.TokenToId("RBb"));
        }
        [TestMethod]
        public void TokenToId_Base2_Transitions()
        {
            // level0
            Assert.AreEqual(0, TreeGenerator.TokenToId("R")); // only one
            // level1
            Assert.AreEqual(1, TreeGenerator.TokenToId("Ra")); // first
            Assert.AreEqual(2, TreeGenerator.TokenToId("Rb")); // last
            // level2
            Assert.AreEqual(3, TreeGenerator.TokenToId("RA")); // first
            Assert.AreEqual(8, TreeGenerator.TokenToId("RBb")); // last
            //level3
            Assert.AreEqual(9, TreeGenerator.TokenToId("RAA")); // first
            Assert.AreEqual(20, TreeGenerator.TokenToId("RBBb")); // last
            // level4
            Assert.AreEqual(21, TreeGenerator.TokenToId("RAAA")); // first
            Assert.AreEqual(44, TreeGenerator.TokenToId("RBBBb")); // last
            // level5
            Assert.AreEqual(45, TreeGenerator.TokenToId("RAAAA")); // first
            Assert.AreEqual(92, TreeGenerator.TokenToId("RBBBBb")); // last
            // level6
            Assert.AreEqual(93, TreeGenerator.TokenToId("RAAAAA")); // first
            Assert.AreEqual(188, TreeGenerator.TokenToId("RBBBBBb")); // last
            // level7
            Assert.AreEqual(189, TreeGenerator.TokenToId("RAAAAAA")); // first
            Assert.AreEqual(380, TreeGenerator.TokenToId("RBBBBBBb")); // last
            // level8
            Assert.AreEqual(381, TreeGenerator.TokenToId("RAAAAAAA")); // first
            Assert.AreEqual(764, TreeGenerator.TokenToId("RBBBBBBBb")); // last
            // level9
            Assert.AreEqual(765, TreeGenerator.TokenToId("RAAAAAAAA")); // first
            Assert.AreEqual(1532, TreeGenerator.TokenToId("RBBBBBBBBb")); // last
            // level10
            Assert.AreEqual(1533, TreeGenerator.TokenToId("RAAAAAAAAA")); // first
            Assert.AreEqual(3068, TreeGenerator.TokenToId("RBBBBBBBBBb")); // last
        }

    }
}

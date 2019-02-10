using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TreeGen.Tests
{
    [TestClass]
    public class TokenTests
    {
        //[TestMethod]
        public void Xxx()
        {
            var containersPerLevel = 2;
            var levelMax = 3;

            TreeNode last = null;
            using (var writer = new StreamWriter(@"D:\Desktop\1.txt"))
            {
                PrintHeader(containersPerLevel, levelMax, writer);
                foreach (var node in TreeGenerator.GenerateTree(containersPerLevel, levelMax))
                {
                    if (node.NodeId < (containersPerLevel + 1) * 2 + 1)
                    {
                        PrintNode(node, writer);
                        continue;
                    }
                    var digits = node.PathDigits;
                    if (digits[0] == 0 && digits.Max() == 1)
                    {
                        writer.WriteLine();
                        PrintNode(last, writer);
                        PrintNode(node, writer);
                    }
                    last = node;
                }
            }
        }
        private void PrintHeader(int containersPerLevel, int levelMax, TextWriter writer)
        {
            writer.WriteLine($"Nodes per level: {containersPerLevel}, maximum level: {levelMax}.");
            writer.WriteLine($"NodeId\tPathId\tPathDigits");
        }
        private void PrintNode(TreeNode node, TextWriter writer)
        {
            var source = node.PathDigits;
            var length = source.Length;
            var digits = new int[length];
            for (int i = digits.Length - 1; i >= 0; i--)
                digits[i] = node.PathDigits[length - i -1];

            writer.WriteLine("{0}\t{1}\t{2}", node.NodeId, node.PathId, string.Join('\t', digits));
        }

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
            Assert.AreEqual("R", TreeGenerator.IdToToken(0, 2));
        }
        [TestMethod]
        public void IdToToken_Negative()
        {
            try
            {
                TreeGenerator.IdToToken(-1, 2);
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
            Assert.AreEqual("Ra", TreeGenerator.IdToToken(1, 2));
            Assert.AreEqual("Rb", TreeGenerator.IdToToken(2, 2));
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
                TreeGenerator.IdToToken(1, TreeGenerator.ContainersPerLevelMax + 1);
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
            Assert.AreEqual("R", TreeGenerator.IdToToken(0, 2));
            Assert.AreEqual("Ra", TreeGenerator.IdToToken(1, 2));
            Assert.AreEqual("Rb", TreeGenerator.IdToToken(2, 2));
            Assert.AreEqual("RA", TreeGenerator.IdToToken(3, 2));
            Assert.AreEqual("RAa", TreeGenerator.IdToToken(4, 2));
            Assert.AreEqual("RAb", TreeGenerator.IdToToken(5, 2));
            Assert.AreEqual("RB", TreeGenerator.IdToToken(6, 2));
            Assert.AreEqual("RBa", TreeGenerator.IdToToken(7, 2));
            Assert.AreEqual("RBb", TreeGenerator.IdToToken(8, 2));
        }
        [TestMethod]
        public void IdToToken_Base2_Level3()
        {
            // level2 last
            Assert.AreEqual("RBb", TreeGenerator.IdToToken(8, 2));
            // level3
            Assert.AreEqual("RAA", TreeGenerator.IdToToken(9, 2));
            Assert.AreEqual("RAAa", TreeGenerator.IdToToken(10, 2));
            Assert.AreEqual("RAAb", TreeGenerator.IdToToken(11, 2));
            Assert.AreEqual("RAB", TreeGenerator.IdToToken(12, 2));
            Assert.AreEqual("RABa", TreeGenerator.IdToToken(13, 2));
            Assert.AreEqual("RABb", TreeGenerator.IdToToken(14, 2));
            Assert.AreEqual("RBA", TreeGenerator.IdToToken(15, 2));
            Assert.AreEqual("RBAa", TreeGenerator.IdToToken(16, 2));
            Assert.AreEqual("RBAb", TreeGenerator.IdToToken(17, 2));
            Assert.AreEqual("RBB", TreeGenerator.IdToToken(18, 2));
            Assert.AreEqual("RBBa", TreeGenerator.IdToToken(19, 2));
            Assert.AreEqual("RBBb", TreeGenerator.IdToToken(20, 2));
        }
        [TestMethod]
        public void IdToToken_Base2_Level4()
        {
            // level3 last
            Assert.AreEqual("RBBb", TreeGenerator.IdToToken(20, 2));
            // level4
            Assert.AreEqual("RAAA",  TreeGenerator.IdToToken(21, 2));
            Assert.AreEqual("RAAAa", TreeGenerator.IdToToken(22, 2));
            Assert.AreEqual("RAAAb", TreeGenerator.IdToToken(23, 2));
            Assert.AreEqual("RAAB",  TreeGenerator.IdToToken(24, 2));
            Assert.AreEqual("RAABa", TreeGenerator.IdToToken(25, 2));
            Assert.AreEqual("RAABb", TreeGenerator.IdToToken(26, 2));
            Assert.AreEqual("RABA",  TreeGenerator.IdToToken(27, 2));
            Assert.AreEqual("RABAa", TreeGenerator.IdToToken(28, 2));
            Assert.AreEqual("RABAb", TreeGenerator.IdToToken(29, 2));
            Assert.AreEqual("RABB",  TreeGenerator.IdToToken(30, 2));
            Assert.AreEqual("RABBa", TreeGenerator.IdToToken(31, 2));
            Assert.AreEqual("RABBb", TreeGenerator.IdToToken(32, 2));
            Assert.AreEqual("RBAA",  TreeGenerator.IdToToken(33, 2));
            Assert.AreEqual("RBAAa", TreeGenerator.IdToToken(34, 2));
            Assert.AreEqual("RBAAb", TreeGenerator.IdToToken(35, 2));
            Assert.AreEqual("RBAB",  TreeGenerator.IdToToken(36, 2));
            Assert.AreEqual("RBABa", TreeGenerator.IdToToken(37, 2));
            Assert.AreEqual("RBABb", TreeGenerator.IdToToken(38, 2));
            Assert.AreEqual("RBBA",  TreeGenerator.IdToToken(39, 2));
            Assert.AreEqual("RBBAa", TreeGenerator.IdToToken(40, 2));
            Assert.AreEqual("RBBAb", TreeGenerator.IdToToken(41, 2));
            Assert.AreEqual("RBBB",  TreeGenerator.IdToToken(42, 2));
            Assert.AreEqual("RBBBa", TreeGenerator.IdToToken(43, 2));
            Assert.AreEqual("RBBBb", TreeGenerator.IdToToken(44, 2));
        }
        [TestMethod]
        public void IdToToken_Base2_Transitions()
        {
            // level0
            Assert.AreEqual("R", TreeGenerator.IdToToken(0, 2)); // only one
                                                              // level1
            Assert.AreEqual("Ra", TreeGenerator.IdToToken(1, 2)); // first
            Assert.AreEqual("Rb", TreeGenerator.IdToToken(2, 2)); // last
                                                               // level2
            Assert.AreEqual("RA", TreeGenerator.IdToToken(3, 2)); // first
            Assert.AreEqual("RBb", TreeGenerator.IdToToken(8, 2)); // last
                                                                // level3
            Assert.AreEqual("RAA", TreeGenerator.IdToToken(9, 2)); // first
            Assert.AreEqual("RBBb", TreeGenerator.IdToToken(20, 2)); // last
                                                                  // level4
            Assert.AreEqual("RAAA", TreeGenerator.IdToToken(21, 2)); // first
            Assert.AreEqual("RBBBb", TreeGenerator.IdToToken(44, 2)); // last
            // level5
            Assert.AreEqual("RAAAA", TreeGenerator.IdToToken(45, 2)); // first
            Assert.AreEqual("RBBBBb", TreeGenerator.IdToToken(92, 2)); // last
            // level6
            Assert.AreEqual("RAAAAA", TreeGenerator.IdToToken(93, 2)); // first
            Assert.AreEqual("RBBBBBb", TreeGenerator.IdToToken(188, 2)); // last
            // level7
            Assert.AreEqual("RAAAAAA", TreeGenerator.IdToToken(189, 2)); // first
            Assert.AreEqual("RBBBBBBb", TreeGenerator.IdToToken(380, 2)); // last
            // level8
            Assert.AreEqual("RAAAAAAA", TreeGenerator.IdToToken(381, 2)); // first
            Assert.AreEqual("RBBBBBBBb", TreeGenerator.IdToToken(764, 2)); // last
            // level9
            Assert.AreEqual("RAAAAAAAA", TreeGenerator.IdToToken(765, 2)); // first
            Assert.AreEqual("RBBBBBBBBb", TreeGenerator.IdToToken(1532, 2)); // last
            // level10
            Assert.AreEqual("RAAAAAAAAA", TreeGenerator.IdToToken(1533, 2)); // first
            Assert.AreEqual("RBBBBBBBBBb", TreeGenerator.IdToToken(3068, 2)); // last
        }

        /* ================================================= */

        [TestMethod]
        public void TokenToId_R()
        {
            Assert.AreEqual(0, TreeGenerator.TokenToId("R", 2));
        }
        [TestMethod]
        public void TokenToId_Invalid()
        {
            try
            {
                TreeGenerator.TokenToId("w", 2);
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
            Assert.AreEqual(1, TreeGenerator.TokenToId("Ra", 2));
            Assert.AreEqual(2, TreeGenerator.TokenToId("Rb", 2));
        }

        [TestMethod]
        public void TokenToId_Base2_Level2()
        {
            Assert.AreEqual(0, TreeGenerator.TokenToId("R", 2));
            Assert.AreEqual(1, TreeGenerator.TokenToId("Ra", 2));
            Assert.AreEqual(2, TreeGenerator.TokenToId("Rb", 2));
            Assert.AreEqual(3, TreeGenerator.TokenToId("RA", 2));
            Assert.AreEqual(4, TreeGenerator.TokenToId("RAa", 2));
            Assert.AreEqual(5, TreeGenerator.TokenToId("RAb", 2));
            Assert.AreEqual(6, TreeGenerator.TokenToId("RB", 2));
            Assert.AreEqual(7, TreeGenerator.TokenToId("RBa", 2));
            Assert.AreEqual(8, TreeGenerator.TokenToId("RBb", 2));
        }
        [TestMethod]
        public void TokenToId_Base2_Transitions()
        {
            // level0
            Assert.AreEqual(0, TreeGenerator.TokenToId("R", 2)); // only one
            // level1
            Assert.AreEqual(1, TreeGenerator.TokenToId("Ra", 2)); // first
            Assert.AreEqual(2, TreeGenerator.TokenToId("Rb", 2)); // last
            // level2
            Assert.AreEqual(3, TreeGenerator.TokenToId("RA", 2)); // first
            Assert.AreEqual(8, TreeGenerator.TokenToId("RBb", 2)); // last
            //level3
            Assert.AreEqual(9, TreeGenerator.TokenToId("RAA", 2)); // first
            Assert.AreEqual(20, TreeGenerator.TokenToId("RBBb", 2)); // last
            // level4
            Assert.AreEqual(21, TreeGenerator.TokenToId("RAAA", 2)); // first
            Assert.AreEqual(44, TreeGenerator.TokenToId("RBBBb", 2)); // last
            // level5
            Assert.AreEqual(45, TreeGenerator.TokenToId("RAAAA", 2)); // first
            Assert.AreEqual(92, TreeGenerator.TokenToId("RBBBBb", 2)); // last
            // level6
            Assert.AreEqual(93, TreeGenerator.TokenToId("RAAAAA", 2)); // first
            Assert.AreEqual(188, TreeGenerator.TokenToId("RBBBBBb", 2)); // last
            // level7
            Assert.AreEqual(189, TreeGenerator.TokenToId("RAAAAAA", 2)); // first
            Assert.AreEqual(380, TreeGenerator.TokenToId("RBBBBBBb", 2)); // last
            // level8
            Assert.AreEqual(381, TreeGenerator.TokenToId("RAAAAAAA", 2)); // first
            Assert.AreEqual(764, TreeGenerator.TokenToId("RBBBBBBBb", 2)); // last
            // level9
            Assert.AreEqual(765, TreeGenerator.TokenToId("RAAAAAAAA", 2)); // first
            Assert.AreEqual(1532, TreeGenerator.TokenToId("RBBBBBBBBb", 2)); // last
            // level10
            Assert.AreEqual(1533, TreeGenerator.TokenToId("RAAAAAAAAA", 2)); // first
            Assert.AreEqual(3068, TreeGenerator.TokenToId("RBBBBBBBBBb", 2)); // last
        }

        /* ================================================= */

        [TestMethod]
        public void IdToToken_Base10_Transitions()
        {
            // level0
            Assert.AreEqual("R", TreeGenerator.IdToToken(0, 10)); // only one
            // level1 abcdefghij
            Assert.AreEqual("Ra", TreeGenerator.IdToToken(1, 10)); // first
            Assert.AreEqual("Rj", TreeGenerator.IdToToken(10, 10)); // last
            // level2
            Assert.AreEqual("RA", TreeGenerator.IdToToken(11, 10)); // first
            Assert.AreEqual("RJj", TreeGenerator.IdToToken(120, 10)); // last
            // level3
            Assert.AreEqual("RAA", TreeGenerator.IdToToken(121, 10)); // first
            Assert.AreEqual("RJJj", TreeGenerator.IdToToken(1220, 10)); // last
            // level4
            Assert.AreEqual("RAAA", TreeGenerator.IdToToken(1221, 10)); // first
            Assert.AreEqual("RJJJj", TreeGenerator.IdToToken(12220, 10)); // last
            // level5
            Assert.AreEqual("RAAAA", TreeGenerator.IdToToken(12221, 10)); // first
            Assert.AreEqual("RJJJJj", TreeGenerator.IdToToken(122220, 10)); // last
            // level6
            Assert.AreEqual("RAAAAA", TreeGenerator.IdToToken(122221, 10)); // first
            Assert.AreEqual("RJJJJJj", TreeGenerator.IdToToken(1222220, 10)); // last
            // level7
            Assert.AreEqual("RAAAAAA", TreeGenerator.IdToToken(1222221, 10)); // first
            Assert.AreEqual("RJJJJJJj", TreeGenerator.IdToToken(12222220, 10)); // last
            // level8
            Assert.AreEqual("RAAAAAAA", TreeGenerator.IdToToken(12222221, 10)); // first
            Assert.AreEqual("RJJJJJJJj", TreeGenerator.IdToToken(122222220, 10)); // last
            // level9
            Assert.AreEqual("RAAAAAAAA", TreeGenerator.IdToToken(122222221, 10)); // first
            Assert.AreEqual("RJJJJJJJJj", TreeGenerator.IdToToken(1222222220, 10)); // last
        }
        [TestMethod]
        public void TokenToId_Base10_Transitions()
        {
            // level0
            Assert.AreEqual(0, TreeGenerator.TokenToId("R", 10)); // only one
            // level1
            Assert.AreEqual(1, TreeGenerator.TokenToId("Ra", 10)); // first
            Assert.AreEqual(10, TreeGenerator.TokenToId("Rj", 10)); // last
            // level2
            Assert.AreEqual(11, TreeGenerator.TokenToId("RA", 10)); // first
            Assert.AreEqual(120, TreeGenerator.TokenToId("RJj", 10)); // last
            //level3
            Assert.AreEqual(121, TreeGenerator.TokenToId("RAA", 10)); // first
            Assert.AreEqual(1220, TreeGenerator.TokenToId("RJJj", 10)); // last
            // level4
            Assert.AreEqual(1221, TreeGenerator.TokenToId("RAAA", 10)); // first
            Assert.AreEqual(12220, TreeGenerator.TokenToId("RJJJj", 10)); // last
            // level5
            Assert.AreEqual(12221, TreeGenerator.TokenToId("RAAAA", 10)); // first
            Assert.AreEqual(122220, TreeGenerator.TokenToId("RJJJJj", 10)); // last
            // level6
            Assert.AreEqual(122221, TreeGenerator.TokenToId("RAAAAA", 10)); // first
            Assert.AreEqual(1222220, TreeGenerator.TokenToId("RJJJJJj", 10)); // last
            // level7
            Assert.AreEqual(1222221, TreeGenerator.TokenToId("RAAAAAA", 10)); // first
            Assert.AreEqual(12222220, TreeGenerator.TokenToId("RJJJJJJj", 10)); // last
            // level8
            Assert.AreEqual(12222221, TreeGenerator.TokenToId("RAAAAAAA", 10)); // first
            Assert.AreEqual(122222220, TreeGenerator.TokenToId("RJJJJJJJj", 10)); // last
            // level9
            Assert.AreEqual(122222221, TreeGenerator.TokenToId("RAAAAAAAA", 10)); // first
            Assert.AreEqual(1222222220, TreeGenerator.TokenToId("RJJJJJJJJj", 10)); // last
            // level10
            Assert.AreEqual(1222222221, TreeGenerator.TokenToId("RAAAAAAAAA", 10)); // first
            Assert.AreEqual(12222222220, TreeGenerator.TokenToId("RJJJJJJJJJj", 10)); // last
        }
    }
}


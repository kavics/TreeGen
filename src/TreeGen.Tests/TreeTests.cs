using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TreeGen.Tests
{
    [TestClass]
    public class TreeTests
    {
        [TestMethod]
        public void Tree_2_CreateNodeById_1()
        {
            var node = TreeGenerator.CreateNode(1, 2);

            Assert.AreEqual(1, node.NodeId);
            Assert.AreEqual("Ra", node.PathToken);
        }
        [TestMethod]
        public void Tree_2_CreateNodeById_20()
        {
            var node = TreeGenerator.CreateNode(20, 2);

            Assert.AreEqual(20, node.NodeId);
            Assert.AreEqual("RBBb", node.PathToken);
        }
        [TestMethod]
        public void Tree_2_CreateNodeByToken_1()
        {
            var node = TreeGenerator.CreateNode("Ra", 2);

            Assert.AreEqual(1, node.NodeId);
            Assert.AreEqual("Ra", node.PathToken);
        }
        [TestMethod]
        public void Tree_2_CreateNodeByToken_20()
        {
            var node = TreeGenerator.CreateNode("RBBb", 2);

            Assert.AreEqual(20, node.NodeId);
            Assert.AreEqual("RBBb", node.PathToken);
        }

        [TestMethod]
        public void Tree_2_Parent()
        {
            var node = TreeGenerator.CreateNode("RBBb", 2);

            var parent = node.Parent;
            Assert.AreEqual("RBB", parent.PathToken);

            parent = parent.Parent;
            Assert.AreEqual("RB", parent.PathToken);

            parent = parent.Parent;
            Assert.AreEqual("R", parent.PathToken);

            parent = parent.Parent;
            Assert.IsNull(parent);
        }

        [TestMethod]
        public void Tree_10_CreateNodeById_1()
        {
            var node = TreeGenerator.CreateNode(1, 10);

            Assert.AreEqual(1, node.NodeId);
            Assert.AreEqual("Ra", node.PathToken);
        }
        [TestMethod]
        public void Tree_10_CreateNodeById_1220()
        {
            var node = TreeGenerator.CreateNode(1220, 10);

            Assert.AreEqual(1220, node.NodeId);
            Assert.AreEqual("RJJj", node.PathToken);
        }
        [TestMethod]
        public void Tree_10_CreateNodeByToken_1()
        {
            var node = TreeGenerator.CreateNode("Ra", 10);

            Assert.AreEqual(1, node.NodeId);
            Assert.AreEqual("Ra", node.PathToken);
        }
        [TestMethod]
        public void Tree_10_CreateNodeByToken_1220()
        {
            var node = TreeGenerator.CreateNode("RJJj", 10);

            Assert.AreEqual(1220, node.NodeId);
            Assert.AreEqual("RJJj", node.PathToken);
        }


        [TestMethod]
        public void Tree_10_Parent()
        {
            var node = TreeGenerator.CreateNode("RJJj", 2);

            var parent = node.Parent;
            Assert.AreEqual("RJJ", parent.PathToken);

            parent = parent.Parent;
            Assert.AreEqual("RJ", parent.PathToken);

            parent = parent.Parent;
            Assert.AreEqual("R", parent.PathToken);

            parent = parent.Parent;
            Assert.IsNull(parent);
        }

        [TestMethod]
        public void Tree_2_Full_Depth2()
        {
            var expectedIds = "0,1,2,3,4,5,6,7,8";
            var expectedPaths = "R,Ra,Rb,RA,RAa,RAb,RB,RBa,RBb";

            var nodes = TreeGenerator.GenerateTree(2, 2)
                .ToArray();

            Assert.AreEqual(expectedIds, string.Join(',', nodes.Select(n => n.NodeId.ToString())));
            Assert.AreEqual(expectedPaths, string.Join(',', nodes.Select(n => n.PathToken)));
        }

        [TestMethod]
        public void Tree_StartFrom_base2()
        {
            // from 1
            var expectedIds = "1,2,3,4,5,6,7,8";
            var expectedPaths = "Ra,Rb,RA,RAa,RAb,RB,RBa,RBb";
            var nodes = TreeGenerator.GenerateTree(2, 2, 1).ToArray();
            Assert.AreEqual(expectedIds, string.Join(',', nodes.Select(n => n.NodeId.ToString())));
            Assert.AreEqual(expectedPaths, string.Join(',', nodes.Select(n => n.PathToken)));

            // from 2
            expectedIds = "2,3,4,5,6,7,8";
            expectedPaths = "Rb,RA,RAa,RAb,RB,RBa,RBb";
            nodes = TreeGenerator.GenerateTree(2, 2, 2).ToArray();
            Assert.AreEqual(expectedIds, string.Join(',', nodes.Select(n => n.NodeId.ToString())));
            Assert.AreEqual(expectedPaths, string.Join(',', nodes.Select(n => n.PathToken)));

            // from 5
            expectedIds = "5,6,7,8";
            expectedPaths = "RAb,RB,RBa,RBb";
            nodes = TreeGenerator.GenerateTree(2, 2, 5).ToArray();
            Assert.AreEqual(expectedIds, string.Join(',', nodes.Select(n => n.NodeId.ToString())));
            Assert.AreEqual(expectedPaths, string.Join(',', nodes.Select(n => n.PathToken)));

            // from 6
            expectedIds = "6,7,8";
            expectedPaths = "RB,RBa,RBb";
            nodes = TreeGenerator.GenerateTree(2, 2, 6).ToArray();
            Assert.AreEqual(expectedIds, string.Join(',', nodes.Select(n => n.NodeId.ToString())));
            Assert.AreEqual(expectedPaths, string.Join(',', nodes.Select(n => n.PathToken)));

            // from 8
            expectedIds = "8";
            expectedPaths = "RBb";
            nodes = TreeGenerator.GenerateTree(2, 2, 8).ToArray();
            Assert.AreEqual(expectedIds, string.Join(',', nodes.Select(n => n.NodeId.ToString())));
            Assert.AreEqual(expectedPaths, string.Join(',', nodes.Select(n => n.PathToken)));
        }
        [TestMethod]
        public void Tree_StartFrom_base10()
        {
            Tuple<string, string> GetNodes(long from, int skip, int count)
            {
                var nodes = TreeGenerator.GenerateTree(10, 6, from).Skip(skip).Take(count).ToArray();
                return new Tuple<string, string>(
                    string.Join(',', nodes.Select(n => n.NodeId.ToString())),
                    string.Join(',', nodes.Select(n => n.PathToken))
                    );
            };

            var result = GetNodes(0, 0, 13);
            Assert.AreEqual("0,1,2,3,4,5,6,7,8,9,10,11,12", result.Item1);
            Assert.AreEqual("R,Ra,Rb,Rc,Rd,Re,Rf,Rg,Rh,Ri,Rj,RA,RAa", result.Item2);

            result = GetNodes(1, 0, 12);
            Assert.AreEqual("1,2,3,4,5,6,7,8,9,10,11,12", result.Item1);
            Assert.AreEqual("Ra,Rb,Rc,Rd,Re,Rf,Rg,Rh,Ri,Rj,RA,RAa", result.Item2);

            result = GetNodes(13, 0, 3);
            Assert.AreEqual("13,14,15", result.Item1);
            Assert.AreEqual("RAb,RAc,RAd", result.Item2);

            // ------------------------------------------------

            result = GetNodes(0, 999, 3);
            Assert.AreEqual("999,1000,1001", result.Item1);
            Assert.AreEqual("RHJi,RHJj,RIA", result.Item2);
            result = GetNodes(1000, 0, 3);
            Assert.AreEqual("1000,1001,1002", result.Item1);
            Assert.AreEqual("RHJj,RIA,RIAa", result.Item2);

            result = GetNodes(1000, 1999, 3);
            Assert.AreEqual("2999,3000,3001", result.Item1);
            Assert.AreEqual("RBGBg,RBGBh,RBGBi", result.Item2);
            result = GetNodes(3000, 0, 4);
            Assert.AreEqual("3000,3001,3002,3003", result.Item1);
            Assert.AreEqual("RBGBh,RBGBi,RBGBj,RBGC", result.Item2);

            result = GetNodes(100000, 999, 3);
            Assert.AreEqual("100999,101000,101001", result.Item1);
            Assert.AreEqual("RIAHAh,RIAHAi,RIAHAj", result.Item2);
            result = GetNodes(101000, 0, 3);
            Assert.AreEqual("101000,101001,101002", result.Item1);
            Assert.AreEqual("RIAHAi,RIAHAj,RIAHB", result.Item2);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Tree_StartFrom_Negative()
        {
            var nodes = TreeGenerator.GenerateTree(2, 2, -1).ToArray();
        }
        [TestMethod]
        public void Tree_StartFrom_OutOfRange()
        {
            var nodes = TreeGenerator.GenerateTree(2, 2, 9).ToArray();
            Assert.AreEqual(0, nodes.Length);
        }

    }
}

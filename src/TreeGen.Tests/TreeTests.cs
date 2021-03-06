﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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
    }
}

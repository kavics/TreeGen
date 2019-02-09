﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
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
            var node = TreeGenerator.CreateNode(21, 2);

            Assert.AreEqual(21, node.NodeId);
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

            Assert.AreEqual(21, node.NodeId);
            Assert.AreEqual("RBBb", node.PathToken);
        }

        [TestMethod]
        public void Tree_2_Parent()
        {
            var node = TreeGenerator.CreateNode("RBBb", 2);

            var parent = node.Parent;
            Assert.AreEqual("RBB", parent);

            parent = parent.Parent;
            Assert.AreEqual("RB", parent);

            parent = parent.Parent;
            Assert.AreEqual("R", parent);

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
        public void Tree_10_CreateNodeById_12220()
        {
            var node = TreeGenerator.CreateNode(21, 10);

            Assert.AreEqual(12220, node.NodeId);
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
        public void Tree_10_CreateNodeByToken_12220()
        {
            var node = TreeGenerator.CreateNode("RJJj", 10);

            Assert.AreEqual(12220, node.NodeId);
            Assert.AreEqual("RJJj", node.PathToken);
        }


        [TestMethod]
        public void Tree_10_Parent()
        {
            var node = TreeGenerator.CreateNode("RJJj", 2);

            var parent = node.Parent;
            Assert.AreEqual("RJJ", parent);

            parent = parent.Parent;
            Assert.AreEqual("RJ", parent);

            parent = parent.Parent;
            Assert.AreEqual("R", parent);

            parent = parent.Parent;
            Assert.IsNull(parent);
        }
    }
}

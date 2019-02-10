﻿using System;

namespace TreeGen
{
    /// <summary>
    /// Represents a node in a generated tree.
    /// </summary>
    public class TreeNode
    {
        internal int NodesPerLevel { get; }

        //TODO: Use different LeavesPerLevel in the fitire version.

        /// <summary>
        /// Gets or sets the Id of the node. The first generated leaf is 1.
        /// </summary>
        public long NodeId { get; set; }

        private TreeNode _parent;
        /// <summary>
        /// Gets the parent TreeNode.
        /// </summary>
        public TreeNode Parent
        {
            get
            {
                if (_parent == null)
                {
                    if (PathToken == "R")
                        return null;
                    if (NodesPerLevel == 0)
                        throw new NotSupportedException("NodesPerLevel is not defined.");
                    if (string.IsNullOrEmpty(PathToken))
                        throw new InvalidOperationException(
                            "Cannot create the Parent because the PathToken is invalid.");
                    var parentToken = PathToken.Substring(0, PathToken.Length - 1);
                    _parent = TreeGenerator.CreateNode(parentToken, NodesPerLevel);
                }
                return _parent;
            }
        }

        /// <summary>
        /// Gets or sets the path represented by a number in the decimal numeral system.
        /// </summary>
        public long PathId { get; set; }

        /// <summary>
        /// Gets or sets the digits of the path-id that is the path representation.
        /// The numeral system depends on the NodesPerLevel.
        /// </summary>
        public int[] PathDigits { get; set; }

        private string _pathToken;
        /// <summary>
        /// Gets or sets the path represented by a token.
        /// </summary>
        public string PathToken
        {
            get
            {
                if (_pathToken == null)
                {
                    if (NodesPerLevel == 0)
                        throw new NotSupportedException("NodesPerLevel is not defined.");
                    _pathToken = TreeGenerator.IdToToken(PathId, NodesPerLevel);
                }
                return _pathToken;
            }
            set => _pathToken = value;
        }

        /// <summary>
        /// Initializes a new TreeNode instance.
        /// </summary>
        internal TreeNode(int nodesPerLevel = 0)
        {
            NodesPerLevel = nodesPerLevel;
        }
    }
}

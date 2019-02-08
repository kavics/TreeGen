using System;

namespace TreeGen
{
    public class TreeNode
    {
        /// <summary>
        /// Gets the current GeneratorSettings instance.
        /// </summary>
        public TreeGeneratorSettings Settings { get; }

        //TODO: Use different LeavesPerLevel in the fitire version.

        /// <summary>
        /// Gets or sets the Id of the node. The first generated leaf is 1.
        /// </summary>
        public int NodeId { get; set; }

        /// <summary>
        /// Gets the parent TreeNode.
        /// </summary>
        public TreeNode Parent { get { throw new NotImplementedException(); } } //UNDONE: NotImplementedException

        /// <summary>
        /// Gets or sets the path represented by a number in the decimal numeral system.
        /// </summary>
        public int PathId { get; set; }

        /// <summary>
        /// Gets or sets the digis of the number that is the path representation.
        /// The numeral system depends on the NodesPerLevel.
        /// </summary>
        public int[] PathDigits { get; set; }

        /// <summary>
        /// Gets the path represented by a token.
        /// </summary>
        public string PathToken { get { throw new NotImplementedException(); } } //UNDONE: NotImplementedException

        /// <summary>
        /// Initializes a new TreeNode instance.
        /// </summary>
        public TreeNode(TreeGeneratorSettings settings)
        {
            Settings = settings;
        }
    }
}

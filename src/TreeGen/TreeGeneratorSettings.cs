using System;
using System.Collections.Generic;
using System.Text;

namespace TreeGen
{
    public class TreeGeneratorSettings
    {
        protected int _nodesPerLevel;
        protected int _levelMax;

        /// <summary>
        /// Gets or sets the width of the containers and leaves in one level.
        /// </summary>
        public int NodesPerLevel
        {
            get => _nodesPerLevel;
            set => _nodesPerLevel = value;
        }

        /// <summary>
        /// Gets or set the maximum level of the generated tree.
        /// Root's level is 0.
        /// </summary>
        public int LevelMax
        {
            get => _levelMax;
            set => _levelMax = value;
        }

        internal ImmutableTreeGeneratorSettings CreateImmutableInstance()
        {
            var clone = new ImmutableTreeGeneratorSettings
            {
                _nodesPerLevel = NodesPerLevel,
                _levelMax = LevelMax
            };
            return clone;
        }
    }

    public class ImmutableTreeGeneratorSettings : TreeGeneratorSettings
    {
        /// <summary>
        /// Gets the width of the containers and leaves in one level.
        /// </summary>
        public new int NodesPerLevel
        {
            get => _nodesPerLevel;
        }

        /// <summary>
        /// Gets the maximum level of the generated tree.
        /// Root's level is 0.
        /// </summary>
        public new int LevelMax
        {
            get => _levelMax;
        }
    }
}

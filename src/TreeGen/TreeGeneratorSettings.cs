using System;
using System.Collections.Generic;
using System.Text;

namespace TreeGen
{
    public class TreeGeneratorSettings
    {
        protected int _nodesPerLevel;

        /// <summary>
        /// Gets or sets the width of the containers and leaves in one level.
        /// </summary>
        public int NodesPerLevel
        {
            get => _nodesPerLevel;
            set => _nodesPerLevel = value;
        }


        internal ImmutableTreeGeneratorSettings CreateImmutableInstance()
        {
            var clone = new ImmutableTreeGeneratorSettings
            {
                _nodesPerLevel = NodesPerLevel,
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
    }
}

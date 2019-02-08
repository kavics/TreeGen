using System;
using System.Collections.Generic;
using System.Text;

namespace TreeGen
{
    public class TreeGeneratorSettings
    {
        /// <summary>
        /// Gets or sets the width of the containers and leaves in one level.
        /// </summary>
        public int NodesPerLevel { get; set; }

        /// <summary>
        /// Gets or set the maximum level of the generated tree.
        /// Root's level is 0.
        /// </summary>
        public int LevelMax { get; set; }

        /// <summary>
        /// Gets the default TreeGeneratorSettings for tests.
        /// </summary>
        public static readonly ImmutableTreeGeneratorSettings Default =
            new ImmutableTreeGeneratorSettings(
                new TreeGeneratorSettings
                {
                    NodesPerLevel = 2,
                    LevelMax = 3
                });
    }

    public class ImmutableTreeGeneratorSettings : TreeGeneratorSettings
    {
        /// <summary>
        /// Gets the width of the containers and leaves in one level.
        /// </summary>
        public new int NodesPerLevel { get; }

        /// <summary>
        /// Gets the maximum level of the generated tree.
        /// Root's level is 0.
        /// </summary>
        public new int LevelMax { get; }

        /// <summary>
        /// Initializes a new instance of the ImmutableTreeGeneratorSettings.
        /// </summary>
        public ImmutableTreeGeneratorSettings(TreeGeneratorSettings original)
        {
            NodesPerLevel = original.NodesPerLevel;
            LevelMax = original.LevelMax;
        }
    }
}

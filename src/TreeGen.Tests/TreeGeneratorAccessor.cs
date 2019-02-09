using System;

namespace TreeGen.Tests
{
    internal class TreeGeneratorAccessor : Accessor
    {
        public TreeGeneratorAccessor() : base(null)
        {
        }

        public static Tuple<int[], int[]> GetIdToTokenDividersAnfOffsets(int nodesPerLevel, int levelMax)
        {
            return (Tuple<int[], int[]>)CallPrivateStaticMethod(typeof(TreeGenerator),
                "GetIdToTokenDividersAnfOffsets",
                new[] { typeof(int), typeof(int) },
                nodesPerLevel, levelMax);
        }
    }
}

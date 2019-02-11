using System;

namespace TreeGen.Tests
{
    internal class TreeGeneratorAccessor : Accessor
    {
        public TreeGeneratorAccessor() : base(null)
        {
        }

        public static Tuple<long[], long[]> GetIdToTokenDividersAnfOffsets(int containersPerLevel, int levelMax)
        {
            return (Tuple<long[], long[]>)CallPrivateStaticMethod(typeof(TreeGenerator),
                "GetIdToTokenDividersAnfOffsets",
                new[] { typeof(int), typeof(int) },
                containersPerLevel, levelMax);
        }
    }
}

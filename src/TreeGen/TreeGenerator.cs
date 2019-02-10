using System;
using System.Collections.Generic;
using System.Linq;

namespace TreeGen
{
    public class TreeGenerator
    {
        public static readonly int MaxNodesPerLevel = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".Length;

        public static IEnumerable<TreeNode> GenerateTree(int nodesPerLevel, int levelMax)
        {
            var id = 0;
            var digits = new int[levelMax];
            var maxDigits = 1;

            yield return new TreeNode(nodesPerLevel)
            {
                NodeId = 0,
                PathId = 0,
                PathDigits = digits.ToArray(),
            };

            for (int i = 1; i < int.MaxValue; i++)
            {
                var d = 0;
                while (true)
                {
                    if (digits[d] < nodesPerLevel)
                    {
                        digits[d]++;
                        break;
                    }
                    else
                    {
                        digits[d] = 0;
                        d++;
                        if (d >= levelMax)
                            yield break;
                        if (d > maxDigits)
                            maxDigits++;
                    }
                }
                var valid = true;
                for (int j = 1; j < maxDigits; j++)
                {
                    if (digits[j] == 0)
                    {
                        valid = false;
                        break;
                    }
                }
                if (valid)
                {
                    id++;

                    yield return new TreeNode(nodesPerLevel)
                    {
                        NodeId = id,
                        PathId = i,
                        PathDigits = digits.ToArray(),
                    };
                }
            }
        }

        public static TreeNode CreateNode(long nodeId, int nodesPerLevel)
        {
            var pathId = IdToPathId(nodeId, nodesPerLevel);
            var pathDigits = GetPathDigits(pathId, nodesPerLevel);
            var pathToken = GetPathToken(pathDigits);
            return new TreeNode
            {
                NodeId = nodeId,
                PathId = pathId,
                PathDigits = pathDigits,
                PathToken = pathToken
            };
        }

        public static TreeNode CreateNode(string pathToken, int nodesPerLevel)
        {
            var pathDigits = ParseToken(pathToken, nodesPerLevel);
            var pathId = GetPathIdFromDigits(pathDigits, nodesPerLevel);
            var nodeId = GetNodeIdFromPathId(pathId, nodesPerLevel);
            return new TreeNode
            {
                NodeId = nodeId,
                PathId = pathId,
                PathDigits = pathDigits,
                PathToken = pathToken
            };
        }

        public static string IdToToken(long id, int nodesPerLevel)
        {
            var pathId = IdToPathId(id, nodesPerLevel);
            var pathDigits = GetPathDigits(pathId, nodesPerLevel);
            return GetPathToken(pathDigits);
        }
        public static long IdToPathId(long id, int nodesPerLevel)
        {
            if (id < 0)
                throw new ArgumentException("Invalid id.");
            if (id == 0)
                return 0;
            if (nodesPerLevel > MaxNodesPerLevel)
                throw new NotSupportedException($"The base number cannot be bigger than {MaxNodesPerLevel}.");
            if (nodesPerLevel < 2)
                throw new NotSupportedException("The base number cannot be less than 2.");

            var @base = nodesPerLevel + 1;
            var levelMax = 12; //UNDONE: this value needs to deducted from the id. Maybe y = log(2,id) ?
            GetIdToTokenDividersAnfOffsets(nodesPerLevel, levelMax, out var dividers, out var offsets);

            var divisions = new List<long>();
            var index = 0;
            while (index < dividers.Length && id >= dividers[index])
            {
                divisions.Add(Math.Max(0, id - offsets[index]) / dividers[index]);
                index++;
            }

            var pathId = id;
            var multiplier = @base;
            for (var i = 0; i < divisions.Count - 1; i++)
            {
                var digit = divisions[i + 1];
                pathId += digit * multiplier;
                multiplier *= @base;
            }

            return pathId;
        }
        /// <summary>Working method</summary>
        private static void GetIdToTokenDividersAnfOffsets(int nodesPerLevel, int levelMax,
            out long[] dividers, out long[] offsets)
        {
            var result = GetIdToTokenDividersAnfOffsets(nodesPerLevel, levelMax);
            dividers = result.Item1;
            offsets = result.Item2;
        }
        /// <summary>Testable method</summary>
        private static Tuple<long[], long[]> GetIdToTokenDividersAnfOffsets(int nodesPerLevel, int levelMax)
        {
            var dividers = new long[levelMax];
            for (int i = 0; i < dividers.Length; i++)
            {
                var multiplier = i == 1 ? nodesPerLevel + 1 : nodesPerLevel;
                dividers[i] = i == 0 ? nodesPerLevel : dividers[i - 1] * multiplier;
            }

            // Expected offsets by numbering system 2
            //
            //  index    0    1    2    3    4    5    6    7
            //           -------------------------------------
            //           1    3    6   12   24   48   96  192
            //                1    3    6   12   24   48   96
            //                     1    3    6   12   24   48
            //                          1    3    6   12   24
            //                               1    3    6   12
            //                                    1    3    6
            //                                         1    3
            //                                              1
            //           -------------------------------------
            // sum()-1   0    3    9   21   45   93  189  381

            var offsets = new long[levelMax];
            for (int i = 0; i < offsets.Length; i++)
            {
                var multiplier = i == 1 ? nodesPerLevel + 1 : nodesPerLevel;
                offsets[i] = i == 0 ? 1 : offsets[i] = offsets[i - 1] * multiplier;
            }
            for (int j = offsets.Length - 1; j >= 0; j--)
                for (int i = 0; i < j; i++)
                    offsets[j] += offsets[i];

            for (int i = 0; i < offsets.Length; i++)
                offsets[i]--;

            return new Tuple<long[], long[]>(dividers, offsets);
        }

        public static long TokenToId(string pathToken, int nodesPerLevel)
        {
            var pathDigits = ParseToken(pathToken, nodesPerLevel);
            var pathId = GetPathIdFromDigits(pathDigits, nodesPerLevel);
            return GetNodeIdFromPathId(pathId, nodesPerLevel);
        }
        public static int[] ParseToken(string pathToken, int nodesPerLevel)
        {
            if (pathToken[0] != 'R')
                throw new ArgumentException("Invalid token.");
            if (pathToken == "R")
                return new int[0];

            var token = pathToken.Substring(1);

            var digits = new List<int>();

            // Last char is optional. Small letter represents a leaf
            var lastChar = token[token.Length - 1];
            if (lastChar >= 'a')
            {
                digits.Add(lastChar - 'a' + 1);
                token = token.Substring(0, token.Length - 1);
            }
            else
            {
                digits.Add(0);
            }

            // Parse nodes (big letters)
            for (var i = token.Length - 1; i >= 0; i--)
                digits.Add( (token[i] - 'A' + 1));

            // Path as number
            return digits.ToArray();
        }
        public static long GetPathIdFromDigits(int[] pathDigits, int nodesPerLevel)
        {
            // Parse nodes (big letters)
            var @base = nodesPerLevel + 1;

            long multiplier = 1;
            long pathId = 0;
            for (var i = 0; i < pathDigits.Length; i++)
            {
                pathId += pathDigits[i] * multiplier;
                multiplier *= @base;
            }
            return pathId;
        }
        private static long GetNodeIdFromPathId(long pathId, int nodesPerLevel)
        {
            var @base = nodesPerLevel + 1;

            // #2 Calculate offset
            var offsets = new List<long>();
            long divider = @base * @base;
            long multiplier = @base;
            do
            {
                offsets.Add((pathId / divider) * multiplier);
                divider *= @base;
                multiplier *= nodesPerLevel;
            } while (pathId >= divider);

            var id = pathId - offsets.Sum();
            return id;
        }

        public static int[] GetPathDigits(long pathId, int nodesPerLevel)
        {
            var id = pathId;
            var @base = nodesPerLevel + 1;

            var pathDigits = new List<int>();
            do
            {
                pathDigits.Add(Convert.ToInt32(id % @base));
                id /= @base;
            } while (id > 0);

            return pathDigits.ToArray();
        }
        public static string GetPathToken(int[] pathDigits)
        {
            var tokenChars = new List<char> {'R'};
            for (var i = pathDigits.Length - 1; i > 0; i--)
                tokenChars.Add((char)('A' + pathDigits[i] - 1));
            if (pathDigits.Length > 0)
                if (pathDigits[0] != 0)
                    tokenChars.Add((char)('a' + pathDigits[0] - 1));
            return new string(tokenChars.ToArray());
        }

        public static int Pow(int x, uint exp)
        {
            var result = 1;
            for (var i = 0; i < exp; i++)
                result *= x;
            return result;
        }
    }
}

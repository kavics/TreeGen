using System;
using System.Collections.Generic;
using System.Linq;

namespace TreeGen
{
    public class TreeGenerator
    {
        public static readonly int ContainersPerLevelMax = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".Length;

        public static IEnumerable<TreeNode> GenerateTree(int containersPerLevel, int levelMax)
        {
            var id = 0;
            var digits = new int[levelMax];
            var maxDigits = 1;

            yield return new TreeNode(containersPerLevel)
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
                    if (digits[d] < containersPerLevel)
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

                    yield return new TreeNode(containersPerLevel)
                    {
                        NodeId = id,
                        PathId = i,
                        PathDigits = digits.ToArray(),
                    };
                }
            }
        }

        public static TreeNode CreateNode(long nodeId, int containersPerLevel)
        {
            var pathId = IdToPathId(nodeId, containersPerLevel);
            var pathDigits = GetPathDigits(pathId, containersPerLevel);
            var pathToken = GetPathToken(pathDigits);
            return new TreeNode(containersPerLevel)
            {
                NodeId = nodeId,
                PathId = pathId,
                PathDigits = pathDigits,
                PathToken = pathToken
            };
        }

        public static TreeNode CreateNode(string pathToken, int containersPerLevel)
        {
            var pathDigits = ParseToken(pathToken, containersPerLevel);
            var pathId = GetPathIdFromDigits(pathDigits, containersPerLevel);
            var nodeId = GetNodeIdFromPathId(pathId, containersPerLevel);
            return new TreeNode(containersPerLevel)
            {
                NodeId = nodeId,
                PathId = pathId,
                PathDigits = pathDigits,
                PathToken = pathToken
            };
        }

        public static string IdToToken(long id, int containersPerLevel)
        {
            var pathId = IdToPathId(id, containersPerLevel);
            var pathDigits = GetPathDigits(pathId, containersPerLevel);
            return GetPathToken(pathDigits);
        }
        public static long IdToPathId(long id, int containersPerLevel)
        {
            if (id < 0)
                throw new ArgumentException("Invalid id.");
            if (id == 0)
                return 0;
            if (containersPerLevel > ContainersPerLevelMax)
                throw new NotSupportedException($"The {nameof(containersPerLevel)} cannot be bigger than {ContainersPerLevelMax}.");
            if (containersPerLevel < 2)
                throw new NotSupportedException($"The {nameof(containersPerLevel)} cannot be less than 2.");

            var @base = containersPerLevel + 1;
            var levelMax = 12; //UNDONE: this value needs to deducted from the id. Maybe y = log(2,id) ?
            GetIdToTokenDividersAnfOffsets(containersPerLevel, levelMax, out var dividers, out var offsets);

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
        private static void GetIdToTokenDividersAnfOffsets(int containersPerLevel, int levelMax,
            out long[] dividers, out long[] offsets)
        {
            var result = GetIdToTokenDividersAnfOffsets(containersPerLevel, levelMax);
            dividers = result.Item1;
            offsets = result.Item2;
        }
        /// <summary>Testable method</summary>
        private static Tuple<long[], long[]> GetIdToTokenDividersAnfOffsets(int containersPerLevel, int levelMax)
        {
            var dividers = new long[levelMax];
            for (int i = 0; i < dividers.Length; i++)
            {
                var multiplier = i == 1 ? containersPerLevel + 1 : containersPerLevel;
                dividers[i] = i == 0 ? containersPerLevel : dividers[i - 1] * multiplier;
            }

            // Expected offsets by numeral system 2
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
                var multiplier = i == 1 ? containersPerLevel + 1 : containersPerLevel;
                offsets[i] = i == 0 ? 1 : offsets[i] = offsets[i - 1] * multiplier;
            }
            for (int j = offsets.Length - 1; j >= 0; j--)
                for (int i = 0; i < j; i++)
                    offsets[j] += offsets[i];

            for (int i = 0; i < offsets.Length; i++)
                offsets[i]--;

            return new Tuple<long[], long[]>(dividers, offsets);
        }

        public static long TokenToId(string pathToken, int containersPerLevel)
        {
            var pathDigits = ParseToken(pathToken, containersPerLevel);
            var pathId = GetPathIdFromDigits(pathDigits, containersPerLevel);
            return GetNodeIdFromPathId(pathId, containersPerLevel);
        }
        public static int[] ParseToken(string pathToken, int containersPerLevel)
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

            return digits.ToArray();
        }
        public static long GetPathIdFromDigits(int[] pathDigits, int containersPerLevel)
        {
            // Parse nodes (big letters)
            var @base = containersPerLevel + 1;

            long multiplier = 1;
            long pathId = 0;
            for (var i = 0; i < pathDigits.Length; i++)
            {
                pathId += pathDigits[i] * multiplier;
                multiplier *= @base;
            }
            return pathId;
        }
        private static long GetNodeIdFromPathId(long pathId, int containersPerLevel)
        {
            var @base = containersPerLevel + 1;

            // #2 Calculate offset
            var offsets = new List<long>();
            long divider = @base * @base;
            long multiplier = @base;
            do
            {
                offsets.Add((pathId / divider) * multiplier);
                divider *= @base;
                multiplier *= containersPerLevel;
            } while (pathId >= divider);

            var id = pathId - offsets.Sum();
            return id;
        }

        public static int[] GetPathDigits(long pathId, int containersPerLevel)
        {
            var id = pathId;
            var @base = containersPerLevel + 1;

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

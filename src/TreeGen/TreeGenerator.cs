using System;
using System.Collections.Generic;
using System.Linq;

namespace TreeGen
{
    public class TreeGenerator
    {
        public static readonly int MaxNodesPerLevel = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".Length;

        public static IEnumerable<TreeNode> GenerateTree(TreeGeneratorSettings settings = null)
        {
            var immutableSettings = new ImmutableTreeGeneratorSettings(settings ?? TreeGeneratorSettings.Default);

            var nodesPerLevel = immutableSettings.NodesPerLevel;
            var levelMax = immutableSettings.LevelMax;

            var @base = nodesPerLevel + 1;

            var id = 0;
            var digits = new int[levelMax];
            var maxDigits = 1;
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

                    yield return new TreeNode(immutableSettings)
                    {
                        NodeId = id,
                        PathId = i,
                        PathDigits = digits.ToArray(),
                    };
                }
            }
        }

        public static string IdToToken(int id, int nodesPerLevel)
        {
            if (id < 0)
                throw new ArgumentException("Invalid id.");
            if (id == 0)
                return "R";
            if (nodesPerLevel > MaxNodesPerLevel)
                throw new NotSupportedException($"The base number cannot be bigger than {MaxNodesPerLevel}.");
            if (nodesPerLevel < 2)
                throw new NotSupportedException("The base number cannot be less than 2.");

            var @base = nodesPerLevel + 1;
            var levelMax = 10; //UNDONE: this value needs to deducted from the id. Maybe y = log(2,id) ?
            GetIdToTokenDividersAnfOffsets(nodesPerLevel, levelMax, out var dividers, out var offsets);

            var divisions = new List<int>();
            var index = 0;
            while (index < dividers.Length && id >= dividers[index])
            {
                divisions.Add(Math.Max(0, id - offsets[index]) / dividers[index]);
                index++;
            }

            var pathNumber = id;
            var multiplier = @base;
            for (int i = 0; i < divisions.Count - 1; i++)
            {
                var digit = divisions[i + 1];
                pathNumber += digit * multiplier;
                multiplier *= @base;
            }

            var pathDigits = GetPathDigits(pathNumber, nodesPerLevel);
            var pathToken = GetPathToken(pathDigits);
            return pathToken;
        }
        /// <summary>Working method</summary>
        private static void GetIdToTokenDividersAnfOffsets(int nodesPerLevel, int levelMax,
            out int[] dividers, out int[] offsets)
        {
            var result = GetIdToTokenDividersAnfOffsets(nodesPerLevel, levelMax);
            dividers = result.Item1;
            offsets = result.Item2;
        }
        /// <summary>Testable method</summary>
        private static Tuple<int[], int[]> GetIdToTokenDividersAnfOffsets(int nodesPerLevel, int levelMax)
        {
            var dividers = new int[levelMax];
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

            var offsets = new int[levelMax];
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

            return new Tuple<int[], int[]>(dividers, offsets);
        }

        public static int TokenToId(string pathToken, int nodesPerLevel)
        {
            if (pathToken[0] != 'R')
                throw new ArgumentException("Invalid token.");
            if (pathToken == "R")
                return 0;

            var token = pathToken.Substring(1);
            var @base = nodesPerLevel + 1;

            // #1 Parse as number
            var digits = new List<int>();

            // Last char is optional. Small letter represents a leaf
            var lastChar = token[token.Length - 1];
            if(lastChar >= 'a')
            {
                digits.Add(lastChar - 'a' + 1);
                token = token.Substring(0, token.Length - 1);
            }
            else
            {
                digits.Add(0);
            }

            // Parse nodes (big letters)
            var multiplier = @base;
            for (int i = token.Length - 1; i >= 0; i--)
            {
                digits.Add(multiplier * (token[i] - 'A' + 1));
                multiplier *= @base;
            }

            // Path as number
            var pathNumber = digits.Sum();

            // #2 Calculate offset
            var offsets = new List<int>();
            var divider = @base * @base;
            multiplier = @base;
            do
            {
                offsets.Add((pathNumber / divider) * multiplier);
                divider *= @base;
                multiplier *= nodesPerLevel;
            } while (pathNumber >= divider);

            var id = pathNumber - offsets.Sum();

            return id;
        }

        public static int[] GetPathDigits(int pathId, int nodesPerLevel)
        {
            var id = pathId;
            var @base = nodesPerLevel + 1;

            var pathDigits = new List<int>();
            do
            {
                pathDigits.Add(id % @base);
                id /= @base;
            } while (id > 0);

            return pathDigits.ToArray();
        }
        public static string GetPathToken(int[] pathDigits)
        {
            var tokenChars = new List<char>();
            tokenChars.Add('R');
            for (int i = pathDigits.Length - 1; i > 0; i--)
                tokenChars.Add((char)('A' + pathDigits[i] - 1));
            if (pathDigits.Length > 0)
                if (pathDigits[0] != 0)
                    tokenChars.Add((char)('a' + pathDigits[0] - 1));
            return new string(tokenChars.ToArray());
        }

        public static int Pow(int x, uint exp)
        {
            var result = 1;
            for (int i = 0; i < exp; i++)
                result *= x;
            return result;
        }
    }
}

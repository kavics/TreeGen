using System;
using System.Collections.Generic;
using System.Linq;

namespace TreeGen
{
    public class TreeGenerator
    {
        public static readonly int MaxBaseNumber = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".Length;

        public static string IdToToken(int id)
        {
            if (id < 0)
                throw new ArgumentException("Invalid id.");
            if (id == 0)
                return "R";
            return IdToToken(id, 2);
        }
        public static string IdToToken(int id, int baseNumber)
        {
            if (baseNumber > MaxBaseNumber)
                throw new NotSupportedException($"The base number cannot be bigger than {MaxBaseNumber}.");
            if (baseNumber < 2)
                throw new NotSupportedException("The base number cannot be less than 2.");

            var @base = baseNumber + 1;

            var xxx = new List<int>();
            for (int divider = @base * baseNumber; id >= divider; divider *= baseNumber)
            {
                var offset = divider - @base;
                xxx.Add(Math.Max(0, (id - offset) / divider));
            }


            var pathNumber = id;
            var multiplier = @base;
            for (int i = 0; i < xxx.Count; i++)
            {
                var digit = xxx[i];
                pathNumber += digit * multiplier;
                multiplier *= @base;
            }

            var x = pathNumber;
            var pathDigits = new List<int>();
            do
            {
                pathDigits.Add(x % @base);
                x /= @base;
            } while (x > 0);

            var tokenChars = new List<char>();
            tokenChars.Add('R');
            for (int i = pathDigits.Count - 1; i > 0; i--)
                tokenChars.Add((char)('A' + pathDigits[i] - 1));
            if (pathDigits.Count > 0)
                if (pathDigits[0] != 0)
                    tokenChars.Add((char)('a' + pathDigits[0] - 1));

            var pathToken = new string(tokenChars.ToArray());

            return pathToken;
        }

        public static int TokenToId(string token)
        {
            if (token[0] != 'R')
                throw new ArgumentException("Invalid token.");
            if (token == "R")
                return 0;
            return TokenToId(token, 2);
        }
        public static int TokenToId(string pathTtoken, int baseNumber)
        {
            var token = pathTtoken;
            if (token[0] == 'R')
                token = token.Substring(1);
            var @base = baseNumber + 1;

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
            var multiplier = 3;
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
                multiplier *= baseNumber;
            } while (pathNumber >= divider);

            var id = pathNumber - offsets.Sum();

            return id;
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

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using TreeGen;

namespace DesignTools
{
    class Program
    {
        static void Main(string[] args)
        {
            var path = GetPath(args);

            if (path != null)
                Run(path);

            if (Debugger.IsAttached)
            {
                Console.Write("press any key to exit ... ");
                Console.ReadKey();
                Console.WriteLine();
            }
        }
        private static string GetPath(string[] args)
        {
            string path;
            if(args.Length == 0)
            {
                path = Path.Combine(
                    Path.GetDirectoryName(
                    Path.GetDirectoryName(
                    Path.GetDirectoryName(
                    Path.GetDirectoryName(
                    Path.GetDirectoryName(
                    Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory)))))), "files");
                if (Directory.Exists(path))
                    return path;

                Console.WriteLine("The tool cannot run because the default path does not exist.");
                return null;
            }

            path = args[0];
            if (Directory.Exists(path))
                return path;

            Console.WriteLine("The tool cannot run because the given path does not exist.");
            return null;
        }

        private static void Run(string outputPath)
        {
            var levelMax = 6;
            foreach (var containersPerLevel in new[] { 2 , 3, 4, 5, 8, 10, 16 })
            {
                Console.Write("Containers per level: " + containersPerLevel);
                var fileName = Path.Combine(outputPath, $"base{containersPerLevel}.txt");

                TreeNode last = null;
                using (var writer = new StreamWriter(fileName))
                {
                    PrintHeader(containersPerLevel, levelMax, writer);
                    foreach (var node in TreeGenerator.GenerateTree(containersPerLevel, levelMax))
                    {
                        if (node.NodeId < containersPerLevel + 4)
                        {
                            PrintNode(node, writer);
                            continue;
                        }
                        var digits = node.PathDigits;
                        if (digits[0] == 0 && digits.Max() == 1)
                        {
                            writer.WriteLine();
                            PrintNode(last, writer);
                            PrintNode(node, writer);
                        }
                        last = node;
                    }
                }
                Console.WriteLine(" ok.");
            }
        }
        private static void PrintHeader(int containersPerLevel, int levelMax, TextWriter writer)
        {
            writer.WriteLine($"Nodes per level: {containersPerLevel}, maximum level: {levelMax}.");
            writer.WriteLine($"NodeId\tPathId\tPathToken\tPathDigits");
        }
        private static void PrintNode(TreeNode node, TextWriter writer)
        {
            var source = node.PathDigits;
            var length = source.Length;
            var digits = new int[length];
            for (int i = digits.Length - 1; i >= 0; i--)
                digits[i] = node.PathDigits[length - i - 1];

            writer.WriteLine("{0}\t{1}\t{2}\t{3}",
                node.NodeId, node.PathId, node.PathToken, string.Join('\t', digits));
        }

    }
}

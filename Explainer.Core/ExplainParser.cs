using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;

namespace Explainer.Core
{    
    public class ExplainParser
    {
        Regex rxFormat;
        Regex rxOffset;
        Regex rxNextNode;

        public ExplainParser()
        {
            rxFormat = new Regex("\"", RegexOptions.IgnoreCase);
            rxOffset = new Regex(@"^\s+", RegexOptions.IgnoreCase);
            rxNextNode = new Regex(@"(->)|(SubPlan)", RegexOptions.IgnoreCase);
        }

        public void ReadFile(string fileName)
        {
            string[] lines = File.ReadAllLines(@"explain.txt")
                .Select(x => rxFormat.Replace(x, ""))
                .ToArray<string>();

            ReadLines(lines);
        }

        void ReadLines(string[] lines)
        {
            var idx = 0;
            var tree = new QueryExpressionTree();
            FeedQueryExpressionTree(tree, lines, ref idx, 0, true);

            Console.WriteLine(tree.ToString());
        }

        public QueryExpressionTree Parse(string[] extarnalLines)
        {
            var idx = 0;
            var tree = new QueryExpressionTree();

            string[] lines = extarnalLines.Select(x => rxFormat.Replace(x, "")).ToArray<string>();

            FeedQueryExpressionTree(tree, lines, ref idx, 0, true);
            return tree;
        }

        int LineOffset(string line)
        {
            return rxOffset.Match(line)?.Length ?? 0; 
        }

        int FeedQueryExpressionTree(IQueryExpressionTreeNode rootNode, string[] lines, ref int row, int currentOffset = 0, bool root = false)
        {
            while(row < lines.Length)
            {
                var line = lines[row];

                if (LineOffset(line) == currentOffset && root && currentOffset == 0)
                {
                    Console.WriteLine(line);
                    var node = rootNode.CreateTreeNode(line.Trim());
                    node.ParseParams(line);
                    row++;                    
                    row = FeedQueryExpressionTree(node, lines, ref row, currentOffset);
                    continue;                    
                }

                if (LineOffset(line) <= currentOffset)
                    return row;

                if (LineOffset(line) > currentOffset)
                {
                    if (rxNextNode.IsMatch(line))
                    {
                        Console.WriteLine(line);
                        var node = rootNode.CreateTreeNode(line.Trim());
                        node.ParseParams(line);
                        row++;                        
                        row = FeedQueryExpressionTree(node, lines, ref row, LineOffset(line));
                        continue;                       
                    }
                    else
                    {
                        Console.WriteLine(line);
                        rootNode.ParseParams(line);                        
                    }
                }
                row++;
            }
            
            return row;
        }

        public static void FeedParams()
        {

        }
    }
}

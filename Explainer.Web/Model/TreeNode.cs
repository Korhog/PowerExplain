using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Explainer.Web.Model
{
    using Explainer.Core;

    public class TreeNode
    {
        public string Name { get; set; }
        public List<TreeNode> Children { get; set; }
        public int TreeMargin { get; set; }

        public TreeNode()
        {
            Children = new List<TreeNode>();
        }        
    }
    
    public static class TreeBuilder
    {
        public static List<TreeNode> Build(string data)
        {
            var parser = new ExplainParser();

            int asciiLine = 13;
            char line = (char)asciiLine;
            string[] lines = data.Split(line);

            var tree = parser.Parse(lines);

            var result = new List<TreeNode>();
            foreach (var node in tree.Nodes)
            {
                result.Add(BuildTreeNode(node, 0));
            }            
            return result;
        }

        public static TreeNode BuildTreeNode(IQueryExpressionTreeNode node, int level)
        {
            var result = new TreeNode
            {
                Name = node.FullDecs,
                TreeMargin = level * 30
            };

            foreach (var child in node.Nodes)            
                result.Children.Add(BuildTreeNode(child, level + 1));            

            return result;
        }
    }
    
}

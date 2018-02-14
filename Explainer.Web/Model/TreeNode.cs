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
        public string Desc { get; set; }

        public string NodeTime { get; set; }
        public string Rows { get; set; }
        public string Rate { get; set; }
        public string RRbF { get; set; }

        public List<TreeNode> Children { get; set; }
        public int TreeMargin { get; set; }

        // Memory
        public string MemoryUsage { get; set; }
        public string MemoryCss { get; set; }

        public TreeNode()
        {
            Children = new List<TreeNode>();
        }        
    }
    
    public static class TreeBuilder
    {
        public static List<TreeNode> Build(string data)
        {
            if (string.IsNullOrEmpty(data))
                return new List<TreeNode>();

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
            string cls = "";

            string rate = null;
            var rows_waited = node.Costs?.Rows ?? 0.0;
            var rows_actial = node.ActualTime?.Rows ?? 0.0;

            if (rows_actial == 0 || rows_waited == 0)
            {
                if (rows_actial == rows_waited)
                    rate = "1";
                else
                    rate = "inf";
            }
            else
                rate = (rows_actial / rows_waited).ToString("0.00");


            var result = new TreeNode
            {
                Name = node.ToString(),
                Desc = node.FullDecs,
                TreeMargin = level * 30,
                MemoryUsage = BuffersToMemory(node.Buffers, out cls),
                NodeTime = node.ActualTime == null ? null : node.ActualTime.FullRecordsAccessTime.ToString() + " ms",
                Rows = node.ActualTime?.Rows.ToString(),
                RRbF = node.RowsRemovedByFilter?.ToString(),
                Rate = rate
            };

            result.MemoryCss = cls;

            foreach (var child in node.Nodes)            
                result.Children.Add(BuildTreeNode(child, level + 1));            

            return result;
        }

        public static string BuffersToMemory(Buffers buffers, out string cls)
        {
            var hits = buffers?.FullHits ?? 0;

            cls = "";
            if (hits == 0)               
                return null;
            

            var size = hits * 8; // количество чтений страниц * базовый размер страницы 8kB
            cls = size > 51200 ? (size > 256000 ? "memory-danger" : "memory-warning") : "memory-success";
            if (size > 1073741824)
                return (size / (double)1073741824).ToString("0.00") + "TB";

            if (size > 1049600)
                return (size / (double)1049600).ToString("0.00") + "GB";

            if (size > 1024)
                return (size / (double)1024).ToString("0.00") + "MB";

            return size + "kB";
        }
    }
    
}

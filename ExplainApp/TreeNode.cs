using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainApp
{
    using Explainer.Core;
    using Windows.UI;
    using Windows.UI.Xaml;

    class TreeNodeBuilder
    {
        public static string BuffersToMemory(long hits, ref Color color)
        {           
            if (hits == 0)
                return null;         

            var size = hits * 8; // количество чтений страниц * базовый размер страницы 8kB
            color = size > 51200 ? (size > 256000 ? ColorHelper.FromArgb(255, 255, 125, 125) : ColorHelper.FromArgb(255, 255, 255, 125)) : ColorHelper.FromArgb(255, 125, 255, 125);
            if (size > 1073741824)  
                return (size / (double)1073741824).ToString("0.00") + "TB";

            if (size > 1049600)            
                return (size / (double)1049600).ToString("0.00") + "GB";            

            if (size > 1024)
                return (size / (double)1024).ToString("0.00") + "MB";

            return size + "kB";
        }
    }

    public class TreeNode
    { 
        ObservableCollection<TreeNode> children;
        public ObservableCollection<TreeNode> Children { get { return children; } }

        public TreeNode(IQueryExpressionTreeNode treeNode, int level = 0)
        {
            children = new ObservableCollection<TreeNode>();

            var step = 20;
            offset = new Thickness(level * step, 0, 0, 0);
            name = treeNode.ToString();
            rowsRemovedByFilter = treeNode.RowsRemovedByFilter;
            fullDesc = treeNode.FullDecs;

            memoryUsage = treeNode.Buffers == null ? null : TreeNodeBuilder.BuffersToMemory(treeNode.Buffers.FullHits, ref memoryUsageColor);

            rows = treeNode.ActualTime?.Rows;
            if (rows.HasValue)
                rowsExclude = rows.Value - treeNode.Costs?.Rows ?? 0;

            nodeTime = treeNode.ActualTime?.FullRecordsAccessTime;

            indicator = name.Contains("Seq Scan") ? Colors.Red : Colors.LightGray;

            foreach(var node in treeNode.Nodes)
            {
                children.Add(new TreeNode(node, level + 1));
            }
        }

        Color indicator;
        public Color Indicator { get { return indicator; } }

        int? rowsRemovedByFilter;
        public int? RRfB { get { return rowsRemovedByFilter; } }

        string name;
        public string Name { get { return name; } }

        Thickness offset;
        public Thickness Offset { get { return offset; } }

        int? rows = null;
        public int? Rows { get { return rows; } }

        int? rowsExclude = null;
        public int? RowsExclude { get { return rowsExclude; } }

        double? nodeTime = null;
        public double? NodeTime { get { return nodeTime; } }

        string fullDesc = null;
        public string FullDesc { get { return fullDesc; } }

        string memoryUsage = null;
        public string MemoryUsage { get { return memoryUsage; } }

        Color memoryUsageColor;
        public Color MemoryUsageColor { get { return memoryUsageColor; } }
    }
}

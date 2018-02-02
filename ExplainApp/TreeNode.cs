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

            rows = treeNode.ActualTime?.Rows;
            if (rows.HasValue)
                rowsExclude = rows.Value - treeNode.Costs?.Rows ?? 0;

            nodeTime = treeNode.ActualTime?.FullRecordsAccessTime;

            indicator = name.Contains("Seq Scan") ? Colors.Red : Colors.Transparent;

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
    }
}

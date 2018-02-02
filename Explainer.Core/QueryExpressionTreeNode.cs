using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.RegularExpressions;

namespace Explainer.Core
{
    /// <summary>
    /// Реализация узла 
    /// </summary>
    public class QueryExpressionTreeNode : IQueryExpressionTreeNode
    {
        string name = "node";
        OperationParams costs;
        OperationParams actualTime;

        ObservableCollection<IQueryExpressionTreeNode> innerNodes;

        public ObservableCollection<IQueryExpressionTreeNode> Nodes { get { return innerNodes; } }

        public OperationParams Costs { get { return costs; } }

        public OperationParams ActualTime { get { return actualTime; } }

        int? rowsRemovedByFilter;
        public int? RowsRemovedByFilter { get { return rowsRemovedByFilter; } }

        public QueryExpressionTreeNode(string nameNode = null)
        {
            if (nameNode != null)
                name = nameNode;

            innerNodes = new ObservableCollection<IQueryExpressionTreeNode>();
        }

        public virtual IQueryExpressionTreeNode CreateTreeNode(string name = null)
        {
            var node = new QueryExpressionTreeNode(name);
            innerNodes.Add(node);
            return node;
        }

        public override string ToString()
        {
            return name;
        }

        public void ParseParams(string paramsLine)
        {
            Regex rxNums = new Regex(@"\d+(,\d+)*", RegexOptions.IgnoreCase);
            Regex rxRows = new Regex(@"rows=\d+", RegexOptions.IgnoreCase);
            Regex rxCost = new Regex(@"cost=\d+.\d+..\d+.\d+", RegexOptions.IgnoreCase);
            Regex rxActualTime = new Regex(@"actual time=\d+.\d+..\d+.\d+", RegexOptions.IgnoreCase);

            var prop = paramsLine.Trim().Split(':').Select(x => x.Trim()).ToArray();
            if (prop.Length == 2)
            { 
                // Дополнительная инфа.
                if (prop[0].ToUpper() == "ROWS REMOVED BY FILTER")
                {
                    int RRbF = 0;
                    if (int.TryParse(prop[1], out RRbF))
                        rowsRemovedByFilter = RRbF;
                }              
            }


            var cost = rxCost.Match(paramsLine);
            var time = rxActualTime.Match(paramsLine);
            var rows = rxRows.Matches(paramsLine);

            if (cost.Success)
            {
                var values = rxNums.Matches(cost.Value.Replace(".", ","));
                if (values.Count == 2)
                {
                    costs = new OperationParams
                    {
                        FirstRecordAccessTime = double.Parse(values[0].Value),
                        FullRecordsAccessTime = double.Parse(values[1].Value),
                        Rows = rows.Count > 0 ? int.Parse(rxNums.Match(rows[0].Value).Value) : 0
                    };
                }

                Console.WriteLine(cost.Value);
            }
            if (time.Success)
            {
                var values = rxNums.Matches(time.Value.Replace(".", ","));
                if (values.Count == 2)
                {
                    actualTime = new OperationParams
                    {
                        FirstRecordAccessTime = double.Parse(values[0].Value),
                        FullRecordsAccessTime = double.Parse(values[1].Value),
                        Rows = rows.Count > 1 ? int.Parse(rxNums.Match(rows[1].Value).Value) : 0
                    };
                }

                Console.WriteLine(time.Value);
            }
        }
    }
}

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
        public OperationParams Costs { get { return costs; } }

        OperationParams actualTime;
        public OperationParams ActualTime { get { return actualTime; } }

        Buffers buffers;
        public Buffers Buffers { get { return buffers; } }

        ObservableCollection<IQueryExpressionTreeNode> innerNodes;

        public ObservableCollection<IQueryExpressionTreeNode> Nodes { get { return innerNodes; } }         

        int? rowsRemovedByFilter;
        public int? RowsRemovedByFilter { get { return rowsRemovedByFilter; } }

        string fullDecs;
        public string FullDecs { get { return fullDecs; } }

        public QueryExpressionTreeNode(string nameNode = null)
        {
            if (nameNode != null)
                name = Regex.Replace(nameNode.Replace("->", ""), @"\([^)]+\)", "").Trim();

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
            if (paramsLine.Trim() != string.Empty)
            {
                if (fullDecs == null)
                    fullDecs = "";
                else
                    fullDecs += fullDecs == "" ? paramsLine.Trim() : ("\n" + paramsLine.Trim());
            }

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
                
                // Память
                if (prop[0].ToUpper() == "BUFFERS")
                {
                    buffers = Buffers.Parse(prop[1]);
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

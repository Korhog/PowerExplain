using System;
using System.Collections.ObjectModel;
using System.Text;

namespace Explainer.Core
{ 
    /// <summary>
    /// Узел дерева запросов
    /// </summary>
    public interface IQueryExpressionTreeNode
    {
        /// <summary>Добавить узел</summary>
        IQueryExpressionTreeNode CreateTreeNode(string name = null);

        /// <summary>Внутренние узлы</summary>
        ObservableCollection<IQueryExpressionTreeNode> Nodes { get; }

        /// <summary>Распарсить параметры</summary>
        void ParseParams(string paramsLine);

        /// <summary>  </summary>
        OperationParams Costs { get; }
        OperationParams ActualTime { get; }

        int? RowsRemovedByFilter { get; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Explainer.Core
{
    /// <summary>
    /// Стоимость операции
    /// </summary>
    public class OperationParams
    {
        public double FirstRecordAccessTime { get; set; }
        public double FullRecordsAccessTime { get; set; }
        public int Rows { get; set; }
    }
}

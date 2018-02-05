using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Explainer.Core
{
    /// <summary>
    /// Стоимость операции
    /// </summary>
    public class OperationParams
    {
        /// <summary> Время до получения первой записи </summary>
        public double FirstRecordAccessTime { get; set; }
        public double FullRecordsAccessTime { get; set; }
        public int Rows { get; set; }
    }


    /// <summary>
    /// Чтение страниц
    /// "  Buffers: shared hit=173408 read=63547 dirtied=26, temp read=6116 written=6114"
    /// </summary>
    public class Buffers
    {
        /// <summary> прочитано из кеша </summary>
        public long? SharedHit { get; set; }

        /// <summary> прочитано с диска </summary>
        public long? Read { get; set; }

        /// <summary> Грязное чтение </summary>
        public long? Dirtied { get; set; }

        /// <summary> вот это я хз что такое </summary>
        public long? TempRead { get; set; }

        /// <summary> запись </summary>
        public long? Written { get; set; }

        /// <summary> запись </summary>
        public long FullHits { get; set; }

        public static Buffers Parse(string line)
        {
            Buffers result = new Buffers();
            Regex rx = new Regex(@"([^\s\d,]+\s){0,1}\S+=\d+", RegexOptions.IgnoreCase);            
            var paramsList = rx.Matches(line.Trim());         
            

            foreach (var param in paramsList)
            {
                var value = (param as Match)?.Value;
                if (string.IsNullOrEmpty(value))
                    continue;

                var pair = value.Split('=');
                if (pair.Length == 2)
                {
                    long hits = 0;
                    if (long.TryParse(pair[1], out hits)) {

                        /// "  Buffers: shared hit=173408 read=63547 dirtied=26, temp read=6116 written=6114"
                        if (pair[0].ToLower() == "shared hit")
                        {
                            result.SharedHit = hits;
                            continue;
                        }

                        if (pair[0].ToLower() == "shared hit")
                        {
                            result.SharedHit = hits;
                            continue;
                        }

                        if (pair[0].ToLower() == "read")
                        {
                            result.Read = hits;
                            continue;
                        }

                        if (pair[0].ToLower() == "dirtied")
                        {
                            result.Dirtied = hits;
                            continue;
                        }

                        if (pair[0].ToLower() == "temp read")
                        {
                            result.TempRead = hits;
                            continue;
                        }

                        if (pair[0].ToLower() == "written")
                        {
                            result.Written = hits;
                            continue;
                        }
                    }
                }                
            }

            result.FullHits = result.SharedHit ?? 0 + result.Read ?? 0 + result.TempRead ?? 0 + result.Written ?? 0 + result.Dirtied ?? 0;
            return result;
        }        
    }
}

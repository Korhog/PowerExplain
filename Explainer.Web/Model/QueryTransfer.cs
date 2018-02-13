using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Explainer.Web.Model
{
    public class DataTransfer
    {
        string data;

        static DataTransfer instance = null;
        static object sync = new object();

        public static DataTransfer GetInstance()
        {
            if (instance == null)
            {
                lock (sync)
                {
                    if (instance == null)
                    {
                        instance = new DataTransfer();
                    }
                }
            }

            return instance;
        }

        public void SetData(string transferData)
        {
            data = transferData;
        }

        public string GetData() { return data; }

    }
}

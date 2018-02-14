using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

namespace Explainer.Web.Model
{
    public class DataTransfer
    {
        ConcurrentDictionary<string, string> container;

        static DataTransfer instance = null;
        static object sync = new object();

        DataTransfer()
        {
            container = new ConcurrentDictionary<string, string>();
        }

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

        public void SetData(string key,string transferData)
        {
            container[key] = transferData;
        }

        public string GetData(string key)
        {
            if (container.ContainsKey(key))
            {
                string data;
                if (container.TryRemove(key, out data))
                    return data;
            }
            return "";
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models.DateTables
{
    public class DtRequest
    {
        public static Dictionary<string, object> HttpData(IEnumerable<KeyValuePair<string, string>> dataIn)
        {
            var dataOut = new Dictionary<string, object>();

            if (dataIn != null)
            {
                foreach (var pair in dataIn)
                {
                    var value = _HttpConv(pair.Value);

                    if (pair.Key.Contains('['))
                    {
                        var keys = pair.Key.Split('[');
                        var innerDic = dataOut;
                        string key;

                        for (int i = 0, ien = keys.Count() - 1; i < ien; i++)
                        {
                            key = keys[i].TrimEnd(']');
                            if (key == "")
                            {
                                // If the key is empty it is an array index value
                                key = innerDic.Count().ToString();
                            }

                            if (!innerDic.ContainsKey(key))
                            {
                                innerDic.Add(key, new Dictionary<string, object>());
                            }
                            innerDic = innerDic[key] as Dictionary<string, object>;
                        }

                        key = keys.Last().TrimEnd(']');
                        if (key == "")
                        {
                            key = innerDic.Count().ToString();
                        }

                        innerDic.Add(key, value);
                    }
                    else
                    {
                        dataOut.Add(pair.Key, value);
                    }
                }
            }

            return dataOut;
        }

        public static object _HttpConv(string dataIn)
        {
            // Boolean
            if (dataIn == "true")
            {
                return true;
            }
            if (dataIn == "false")
            {
                return false;
            }

            // Numeric looking data, but with leading zero
            if (dataIn.IndexOf('0') == 0 && dataIn.Length > 1)
            {
                return dataIn;
            }

            try
            {
                return Convert.ToInt32(dataIn);
            }
            catch (Exception) { }

            try
            {
                return Convert.ToDecimal(dataIn);
            }
            catch (Exception) { }

            return dataIn;
        }
    }
}
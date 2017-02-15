using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models.DateTables
{
    /// <summary>
    /// 分析DataTables的Json返回，定义的一个类
    /// 某些字段含义不明
    /// </summary>
    public class DtResponse
    {
        public int? draw;

        public List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();

        public int? recordsTotal;

        public int? recordsFiltered;

        public string error;

        public List<FieldError> fieldErrors = new List<FieldError>();

        public int? id;

        public Dictionary<string, object> meta = new Dictionary<string, object>();

        public Dictionary<string, object> options = new Dictionary<string, object>();

        public Dictionary<string, Dictionary<string, Dictionary<string, object>>> files =
            new Dictionary<string, Dictionary<string, Dictionary<string, object>>>();

        public ResponseUpload upload =
            new ResponseUpload();

        public class FieldError
        {
            
            public string name { get; set; }

            
            public string status { get; set; }
        }

        public class ResponseUpload
        {            
            public dynamic id { get; set; }
        }

    }
}
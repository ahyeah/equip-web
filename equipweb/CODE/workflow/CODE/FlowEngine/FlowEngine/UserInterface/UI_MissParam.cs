using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowEngine.UserInterface
{
    public class UI_MissParam
    {
       

        /// <summary>
        /// 变量的值
        /// </summary>
        public string Param_Value { get; set; }

        /// <summary>
        /// 变量的类型
        /// </summary>
        public int Param_isFile { get; set; }

        /// <summary>
        /// 变量的描述
        /// </summary>
        public string Param_Desc { get; set; }

        public string Param_SavedFilePath { get; set; }
        public string Param_UploadFilePath { get; set; }
        

    }
}

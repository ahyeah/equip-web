using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowEngine.Modals
{
    /// <summary>
    /// 记录任务的变量值
    /// </summary>
    public class Mission_Param
    {
        /// <summary>
        /// 主键，自增
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Param_Id { get; set; }

        /// <summary>
        /// 变量的名称
        /// </summary>
        public string Param_Name { get; set; }

        /// <summary>
        /// 变量的值
        /// </summary>
        public string Param_Value { get; set; }

        /// <summary>
        /// 变量的类型
        /// </summary>
        public string Param_Type { get; set; }

        /// <summary>
        /// 变量的描述
        /// </summary>
        public string Param_Desc { get; set; }

        /// <summary>
        /// 该变量所属的任务
        /// </summary>
        public virtual Mission Miss_Belong { get; set; }
    }
}

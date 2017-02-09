using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/////////////////////////////////////////////////////////
//2016/5/17--chenbin
//添加CURR_Mission表用以记录工作流事件的当前任务，避免频繁从XML重构工作流，提高效率
////////////////////////////////////////////////////////
namespace FlowEngine.Modals
{
    public class CURR_Mission
    {
        //主键
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Miss_Id
        {
            get;
            set;
        }

        /// <summary>
        /// 任务名称
        /// </summary>
        public string Miss_Name
        {
            get;
            set;
        }

        /// <summary>
        /// 任务描述
        /// </summary>
        public string Miss_Desc
        {
            get;
            set;
        }

        /// <summary>
        /// 进入事件
        /// </summary>
        public string Before_Action
        {
            get;
            set;
        }

        /// <summary>
        /// 当前事件
        /// </summary>
        public string Current_Action
        {
            get;
            set;
        }

        /// <summary>
        /// 离开事件
        /// </summary>
        public string After_Action
        {
            get;
            set;
        }

        /// <summary>
        /// 权限
        /// </summary>
        public string Str_Authority
        {
            get;
            set;
        }

        public int WFE_ID
        {
            get;
            set;
        }

        public virtual WorkFlow_Entity WFE_Parent
        {
            get;
            set;
        }
    }
}

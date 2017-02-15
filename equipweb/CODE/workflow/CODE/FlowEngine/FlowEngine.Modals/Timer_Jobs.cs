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
    /// 任务类别
    /// </summary>
    public enum TIMER_JOB_TYPE { CREATE_WORKFLOW, CHANGE_STATUS, TIME_OUT, EMPTY, Normal };

    /// <summary>
    /// 任务状态
    /// </summary>
    public enum TM_STATUS { TM_STATUS_ACTIVE, TM_STATUS_STOP, TM_FINISH, TM_DELETE };

    /// <summary>
    /// 任务用途
    /// </summary>
    public enum TIMER_USING { FOR_SYSTEM, FOR_CUSTOM}

    /// <summary>
    /// 关于定时性工作的定义
    /// </summary>
    public class Timer_Jobs
    {
        /// <summary>
        /// 工作任务ID， 主键
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int JOB_ID
        {
            get;
            set;
        }

        /// <summary>
        /// 任务名称
        /// </summary>
        public string job_name { get; set; }

        /// <summary>
        /// 任务类型
        /// </summary>
        public TIMER_JOB_TYPE Job_Type { get; set; }

        /// <summary>
        /// 上一次任务执行时间
        /// </summary>
        public DateTime? PreTime { get; set; }

        /// <summary>
        /// 任务状态
        /// </summary>
        public TM_STATUS status { get; set; }

        /// <summary>
        /// 任务用途
        /// </summary>
        public TIMER_USING t_using { get; set; }

        /// <summary>
        /// 该字段可能存储 工作流定义的ID或工作流实体的ID， 具体含义依据不同类型的任务而定
        /// </summary>
        public int workflow_ID { get; set; }

        /// <summary>
        /// 该字段存储任务运行参数
        /// </summary>
        public string run_params { get; set; }

        /// <summary>
        /// 运行结果
        /// </summary>
        public string run_result { get; set; }

        /// <summary>
        /// 表示执行时间的corn表达式
        /// </summary>
        public string corn_express { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime create_time { get; set; }

        /// <summary>
        /// 自定义事件
        /// </summary>
        public string custom_action { get; set; }

        /// <summary>
        /// 自定义标签
        /// </summary>
        public int custom_flag { get; set; }

        #region 保留字段
        /// <summary>
        /// 整数型保留字段
        /// </summary>
        public int INT_RES_1 { get; set; }
        public int INT_RES_2 { get; set; }
        public int INT_RES_3 { get; set; }

        /// <summary>
        /// 字符串型保留字段
        /// </summary>
        public string STR_RES_1 { get; set; }
        public string STR_RES_2 { get; set; }
        public string STR_RES_3 { get; set; }
        #endregion

        public void copy(Timer_Jobs job)
        {
            JOB_ID = job.JOB_ID;

            job_name = job.job_name;

            Job_Type = job.Job_Type;

            PreTime = job.PreTime;

            status = job.status;

            t_using = job.t_using;

            workflow_ID = job.workflow_ID;

            run_params = job.run_params;

            run_result = job.run_result;

            corn_express = job.corn_express;

            INT_RES_1 = job.INT_RES_1;

            INT_RES_2 = job.INT_RES_2;

            INT_RES_3 = job.INT_RES_3;

            STR_RES_1 = job.STR_RES_1;

            STR_RES_2 = job.STR_RES_2;

            STR_RES_3 = job.STR_RES_3;
        }
    }
}

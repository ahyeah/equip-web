using FlowEngine.DAL;
using FlowEngine.Modals;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FlowEngine.TimerManage
{
    /// <summary>
    /// 超时处理任务
    /// </summary>
    public class CTimerTimeout : CTimerMission
    {
        #region 构造函数
        public CTimerTimeout()
        {
            //运行参数            
            //SUSPEND: 将对应工作流实体设置为 WE_SUSPEND
            //DELETE: 将对应工作流实体设置为 WE_DELETED      
            //其他: 不做处理
            m_run_params = "";

            type = "TimeOut";
        }
        #endregion

        #region 属性
        /// <summary>
        /// 与任务关联的工作流实体ID号
        /// </summary>
        private int m_wfentityID = -1;
        public int AttachWFEntityID
        {
            get { return m_wfentityID; }
            set { m_wfentityID = value; }
        }

        /// <summary>
        /// 该任务对应的event名称
        /// </summary>
        private string m_eventName = "";
        public string EventName
        {
            get
            {
                return m_eventName;
            }
            set
            {
                m_eventName = value;
            }
        }

        /// <summary>
        /// 超时用户的Callback， 可用于后期的绩效处理
        /// </summary>
        //private string m_callBackURL = "";
        #endregion

        #region 公共方法
        /// <summary>
        /// 设置该任务对关联工作流实体的操作
        /// </summary>
        /// <param name="action"></param>
        public void SetActionForWF(string action)
        {
            string act = action.ToUpper();
            m_run_params = act;
        }
        /// <summary>
        /// 解析数据库参数到运行参数
        /// </summary>
        /// <param name="strPars"></param>
        public override void ParseRunParam(string strPars)
        {
            JObject jpar = JObject.Parse(strPars);

            m_run_params = jpar["target_status"];
        }

        /// <summary>
        /// 解析运行参数到数据库
        /// </summary>
        /// <returns></returns>
        public override object ParseRunParamsToJObject()
        {
            JObject jpar = new JObject();
            jpar["target_status"] = m_run_params.ToString();
            return jpar;
        }

        /// <summary>
        /// 解析运行结果
        /// </summary>
        /// <returns></returns>
        public override object ParseRunResultToJobject()
        {
            //throw new NotImplementedException();
            return null;
        }

        /// <summary>
        /// 解析运行结果
        /// </summary>
        /// <param name="strResult"></param>
        public override void ParseRunResult(string strResult)
        {
            //throw new NotImplementedException();
            return;
        }

        /// <summary>
        /// 保存该定时性任务
        /// </summary>
        /// <param name="job"></param>
        public override void Save(Timer_Jobs job = null)
        {
            Timer_Jobs self_Job = new Timer_Jobs();

            self_Job.workflow_ID = m_wfentityID;

            self_Job.Job_Type = TIMER_JOB_TYPE.TIME_OUT;

            //保留字段STR_RES_1保存callback URL地址
            self_Job.STR_RES_1 = m_eventName;

            base.Save(self_Job);
        }

        public override void Load(Timer_Jobs job)
        {
            base.Load(job);

            m_wfentityID = job.workflow_ID;
            m_eventName = job.STR_RES_1;
        }
        #endregion

        #region 私有方法

        /// <summary>
        /// 用户定义的Callback调用
        /// </summary>
        protected void _CustomCallBack()
        {
            string custom_param = "{";
            custom_param += "\"entity_id\":" + m_wfentityID.ToString() + ",";
            custom_param += "\"event_name\":\"" + m_eventName + "\"";
            custom_param += "}";
            base.CallCustomAction(custom_param);
        }

        /// <summary>
        /// 该事件的处理函数
        /// </summary>
        protected override void __processing()
        {
            WE_STATUS status = WE_STATUS.SUSPEND;
            WorkFlows wfs = new WorkFlows();
            try
            {
                switch ((string)m_run_params)
                {
                    case "SUSPEND":
                        status = WE_STATUS.SUSPEND;
                        wfs.UpdateWorkFlowEntity(m_wfentityID, "WE_Status", status);
                        break;

                    case "DELETE":
                        status = WE_STATUS.DELETED;
                        wfs.UpdateWorkFlowEntity(m_wfentityID, "WE_Status", status);
                        break;

                    default:
                        break;
                }
                //调用用户定义的回调URL
                _CustomCallBack();
                this.status = TM_STATUS.TM_FINISH;
            }
            catch
            {
                Trace.WriteLine("Timeout processing exception!");
            }
        }
        #endregion
    }
}

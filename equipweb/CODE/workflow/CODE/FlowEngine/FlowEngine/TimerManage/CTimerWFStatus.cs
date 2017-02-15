using FlowEngine.DAL;
using FlowEngine.Modals;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowEngine.TimerManage
{
    /// <summary>
    /// 改变工作流状态
    /// </summary>
    public class CTimerWFStatus : CTimerMission
    {
        #region 构造函数
        public CTimerWFStatus()
        {
            //默认情况下是置工作流为激活状态
            m_run_params = WE_STATUS.ACTIVE;

            type = "ChangeStatus";
        }
        #endregion

        #region 属性
        /// <summary>
        /// 任务执行对象 
        /// </summary>
        private int m_wfentityID = -1;
        #endregion

        #region 公共方法
        /// <summary>
        /// 绑定处理对象
        /// </summary>
        /// <param name="wf">工作流实体</param>
        /// <returns></returns>
        public bool AttachWFEntity(CWorkFlow wf)
        {
            if (wf == null)
                return false;
            m_wfentityID = wf.EntityID;
            return true;
        }

        /// <summary>
        /// 设置该任务的参数
        /// </summary>
        /// <param name="status">设置的工作流实体状态</param>
        public void SetRunParam(WE_STATUS status)
        {
            m_run_params = status;
        }

        /// <summary>
        /// 从数据库记录加载类对象
        /// </summary>
        /// <param name="job">数据库记录</param>
        public override void Load(Timer_Jobs job)
        {
            m_wfentityID = job.workflow_ID;

            base.Load(job);
        }

        /// <summary>
        /// 保存定时性任务到数据库
        /// </summary>
        /// <param name="job"></param>
        public override void Save(Timer_Jobs job = null)
        {
            Timer_Jobs self = new Timer_Jobs();
            self.workflow_ID = m_wfentityID;
            self.Job_Type = TIMER_JOB_TYPE.CHANGE_STATUS;
            base.Save(self);
        }

        public int GetAttachWFEntityID()
        {
            return m_wfentityID;
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 执行设置工作流实体状态的工作
        /// </summary>
        protected override void __processing()
        {
            try
            {
                //更新工作流实体的状态
                WorkFlows wfs = new WorkFlows();
                wfs.UpdateWorkFlowEntity(m_wfentityID, "WE_Status", m_run_params);
                //如果要改变的状态为ACTIVE， 则发送一个空消息，推动工作流运转
                if (((WE_STATUS)m_run_params) == WE_STATUS.ACTIVE)
                    CWFEngine.SubmitSignal(m_wfentityID, new Dictionary<string, string>(), null);
            }
            catch
            {
                string error = string.Format("Set workflow entity {0} error!", m_wfentityID);
                Trace.WriteLine(error);
            }
        }

        /// <summary>
        /// 解析参数
        /// </summary>
        /// <param name="strPars"></param>
        public override void ParseRunParam(string strPars)
        {
            switch(strPars)
            {
                case "ACTIVE":
                    m_run_params = WE_STATUS.ACTIVE;
                    break;

                case "SUSPEND":
                    m_run_params = WE_STATUS.SUSPEND;
                    break;

                case "CREATED":
                    m_run_params = WE_STATUS.CREATED;
                    break;

                case "DONE":
                    m_run_params = WE_STATUS.DONE;
                    break;

                case "DELETED":
                    m_run_params = WE_STATUS.DELETED;
                    break;

                default:
                    m_run_params = WE_STATUS.ACTIVE;
                    break;
            }

        }

        /// <summary>
        /// 解析运行结果
        /// </summary>
        /// <param name="strResult"></param>
        public override void ParseRunResult(string strResult)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 解析运行参数到数据库
        /// </summary>
        /// <returns></returns>
        public override object ParseRunParamsToJObject()
        {
            string str_val = "";
            switch ((WE_STATUS)m_run_params)
            {
                case WE_STATUS.ACTIVE:
                    str_val = "ACTIVE";
                    break;

                case WE_STATUS.SUSPEND:
                    str_val = "SUSPEND";
                    break;

                case WE_STATUS.CREATED:
                    str_val = "CREATED";
                    break;

                case WE_STATUS.DONE:
                    str_val = "DONE";
                    break;

                case WE_STATUS.DELETED:
                    str_val = "DELETED";
                    break;
            }
            return str_val;
        }

        /// <summary>
        /// 解析运行结果
        /// </summary>
        /// <returns></returns>
        public override object ParseRunResultToJobject()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}

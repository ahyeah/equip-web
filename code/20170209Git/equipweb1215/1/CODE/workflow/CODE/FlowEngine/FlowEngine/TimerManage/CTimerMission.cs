using FlowEngine.DAL;
using FlowEngine.Modals;
using Quartz;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FlowEngine.TimerManage
{

    /// <summary>
    /// 定时任务
    /// </summary>
    public abstract class CTimerMission
    {
        #region 构造函数
        public CTimerMission()
        {
            m_jobFunc = new JobProcessing(this.__processing);
            manage = null;
            job_id = -1;
            type = "";
            CustomFlag = 0;
        }
        #endregion

        #region 属性
        /// <summary>
        /// 任务类型
        /// </summary>
        public string type { get; set; }

        /// <summary>
        /// 任务ID
        /// </summary>
        private int job_id;
        public int ID { get { return job_id; } set { job_id = value; } }

        /// <summary>
        /// 是否处于执行状态
        /// </summary>
        private Int32 m_isRun = 0;
        /// <summary>
        /// 定时器误差
        /// </summary>
        public static int Timer_deviation = 0;

        /// <summary>
        /// 用于执行任务时，传递的参数列表
        /// </summary>
        protected object m_run_params;

        /// <summary>
        /// 任务执行结果
        /// </summary>
        protected object m_run_result;

        /// <summary>
        /// 当前任务前一次触发时间
        /// </summary>
        protected DateTime? m_perTime;
        public DateTime? PerTime
        {
            get { return m_perTime; }
        }

        /// <summary>
        /// 当前任务创建时间
        /// </summary>
        protected DateTime m_createTime;
        public DateTime CreateTime
        {
            get { return m_createTime; }
            set { m_createTime = value; }
        }

        /// <summary>
        /// 用户定义的事件
        /// </summary>
        public string CustomAction
        {
            get;
            set;
        }

        /// <summary>
        /// 用户定义标签
        /// </summary>
        public int CustomFlag { get; set; }

        /// <summary>
        /// 任务执行实体函数
        /// </summary>
        delegate void JobProcessing();
        protected abstract void __processing();
        private JobProcessing m_jobFunc;

        public CTimerManage manage { get; set; }

        /// <summary>
        /// 任务状态
        /// TM_STATUS_ACTIVE ： 激活状态
        /// TM_STATUS_STOP: 停止状态
        /// </summary>
        public TM_STATUS status { get; set; }

        /// <summary>
        /// 触发时间
        /// </summary>
        protected TriggerTiming m_triggerT = new TriggerTiming();

        /// <summary>
        /// 任务名称
        /// </summary>
        public string mission_name { get; set; }

        /// <summary>
        /// 任务用途
        /// </summary>
        public TIMER_USING for_using { get; set; }

        private Dictionary<string, object> m_reserve = new Dictionary<string, object>();

        #endregion

        #region 公共方法
        /// <summary>
        /// 设置触发时间
        /// </summary>
        /// <param name="cornStr">Cron 表达式</param>
        /// <returns>是正确的Cron表达式，返回true， 否则返回false</returns>
        public bool SetTriggerTiming(string cornStr)
        {
            return m_triggerT.FromString(cornStr);
        }

        /// <summary>
        /// 获得出发时间的字符串表达式
        /// </summary>
        /// <returns></returns>
        public string GetTriggerTimmingString()
        {
            return m_triggerT.ToString();
        }

        /// <summary>
        /// 解析数据库参数到运行参数
        /// </summary>
        /// <param name="strPars"></param>
        public abstract void ParseRunParam(string strPars);

        /// <summary>
        /// 解析数据库结果到运行结果
        /// </summary>
        /// <param name="strResult"></param>
        public abstract void ParseRunResult(string strResult);

        /// <summary>
        /// 解析运行参数到数据库
        /// </summary>
        /// <returns></returns>
        public abstract object ParseRunParamsToJObject();

        /// <summary>
        /// 解析运行结果到数据库
        /// </summary>
        /// <returns></returns>
        public abstract object ParseRunResultToJobject();

        /// <summary>
        /// 获得与之关联的工作流
        /// </summary>
        /// <returns></returns>
        public virtual CWorkFlow GetAttachWorkFlow()
        {
            return null;
        }

        /// <summary>
        /// 检测当前系统时间是否满足触发条件
        /// </summary>
        /// <returns></returns>
        public bool checkTiming()
        {
            try
            {
                //获得当前系统时间
                DateTime dt = DateTime.Now;

                if (m_triggerT.NextTiming == null)
                    m_triggerT.RefreshNextTiming(dt);

                DateTime nextdt = m_triggerT.NextTiming.Value;

                //考虑到定时器误差
                if (Math.Abs((dt - nextdt).TotalSeconds) <= CTimerMission.Timer_deviation)
                    return true;
            }
            catch
            {
                return false;
            }
            return false;
        }

        /// <summary>
        /// 设置保留字段的值
        /// </summary>
        /// <param name="res_name"></param>
        /// <param name="value"></param>
        public void Set_Res_Value(string res_name, object value)
        {
            try
            {
                switch (res_name)
                {
                    case "STR_RES_1":
                        m_reserve["STR_RES_1"] = Convert.ToString(value);
                        break;

                    case "STR_RES_2":
                        m_reserve["STR_RES_2"] = Convert.ToString(value);
                        break;

                    case "STR_RES_3":
                        m_reserve["STR_RES_3"] = Convert.ToString(value);
                        break;

                    case "INT_RES_1":
                        m_reserve["INT_RES_1"] = Convert.ToInt32(value);
                        break;

                    case "INT_RES_2":
                        m_reserve["INT_RES_2"] = Convert.ToInt32(value);
                        break;

                    case "INT_RES_3":
                        m_reserve["INT_RES_3"] = Convert.ToInt32(value);
                        break;

                    default:
                        break;
                }
            }
            catch
            {
                Trace.WriteLine("Type Error!");
            }
        }

        /// <summary>
        /// 读取保留字段的值
        /// </summary>
        /// <param name="res_name"></param>
        /// <returns></returns>
        public object Read_Res_Value(string res_name)
        {
            try
            {
                return m_reserve[res_name];
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 执行相应工作
        /// </summary>
        public void DoJob()
        {

            //设置当前任务正在执行
            //若当前任务处于真正运行状态，则不做任何操作返回
            if (Interlocked.Exchange(ref m_isRun, 1) == 1)
                return;

            if ((checkTiming() == false) || (status != TM_STATUS.TM_STATUS_ACTIVE))
            {
                Interlocked.Exchange(ref m_isRun, 0);
                return;
            }

            //更新触发时间
            //原理上上次触发时间应更新为当前时间
            //单考虑到定时误差，故将上次触发时间设置为准确触发时间
            m_perTime = m_triggerT.NextTiming;
            //以准确触发时间更新下次触发时间
            m_triggerT.RefreshNextTiming(m_triggerT.NextTiming.Value);

            //异步执行， 加快定时器处理速度
            m_jobFunc.BeginInvoke(new AsyncCallback(this.__JobCallBack), this);

        }

        /// <summary>
        /// 保存到数据库
        /// </summary>
        public virtual void Save(Timer_Jobs job = null)
        {
            //任务名称
            job.job_name = mission_name;
            //上一次执行时间
            job.PreTime = m_perTime;
            //创建时间
            job.create_time = m_createTime;
            //任务状态
            job.status = status;
            //任务用途
            job.t_using = for_using;
            //cron表达式
            job.corn_express = m_triggerT.ToString();
            //job ID
            job.JOB_ID = job_id;
            //运行参数
            object rp = ParseRunParamsToJObject();
            job.run_params = rp == null ? "[]" : rp.ToString();
            //运行结果
            object rr = ParseRunResultToJobject();
            job.run_result = rr == null ? "" : rr.ToString();
            //自定义事件
            job.custom_action = CustomAction;
            //自定义标签
            job.custom_flag = CustomFlag;
            //保留字段
            foreach (var p in m_reserve)
            {
                switch (p.Key)
                {
                    case "STR_RES_1":
                        job.STR_RES_1 = Convert.ToString(p.Value);
                        break;

                    case "STR_RES_2":
                        job.STR_RES_2 = Convert.ToString(p.Value);
                        break;

                    case "STR_RES_3":
                        job.STR_RES_3 = Convert.ToString(p.Value);
                        break;

                    case "INT_RES_1":
                        job.INT_RES_1 = Convert.ToInt32(p.Value);
                        break;

                    case "INT_RES_2":
                        job.INT_RES_2 = Convert.ToInt32(p.Value);
                        break;

                    case "INT_RES_3":
                        job.INT_RES_3 = Convert.ToInt32(p.Value);
                        break;

                    default:
                        break;
                }
            }

            Jobs t_j = new Jobs();
            this.job_id = t_j.SaveJob(job);
        }

        /// <summary>
        /// 从数据库记录加载类对象
        /// </summary>
        /// <param name="job"></param>
        public virtual void Load(Timer_Jobs job)
        {
            //ID
            job_id = job.JOB_ID;
            //前一次执行时间
            m_perTime = job.PreTime;
            //任务状态
            status = job.status;
            //执行时间
            m_triggerT.FromString(job.corn_express);
            m_triggerT.RefreshNextTiming(DateTime.Now);
            //解析运行参数
            ParseRunParam(job.run_params);
            //解析运行结果
            ParseRunResult(job.run_result);
            //名称
            mission_name = job.job_name;
            //创建时间
            m_createTime = job.create_time;
            //自定义事件
            CustomAction = job.custom_action;
            //自定义标签
            CustomFlag = job.custom_flag;
            //读取保留字段
            m_reserve.Clear();
            if (job.STR_RES_1 != "null")
                m_reserve["STR_RES_1"] = job.STR_RES_1;
            if (job.STR_RES_2 != "null")
                m_reserve["STR_RES_2"] = job.STR_RES_2;
            if (job.STR_RES_3 != "null")
                m_reserve["STR_RES_3"] = job.STR_RES_3;

            m_reserve["INT_RES_1"] = job.INT_RES_1;
            m_reserve["INT_RES_2"] = job.INT_RES_2;
            m_reserve["INT_RES_3"] = job.INT_RES_3;
        }
        #endregion

        #region 内部方法
        /// <summary>
        /// 调用自定义事件
        /// </summary>
        /// <param name="json_params">传递的用户参数， 该参数为json结构，具体字段根据实际需求指定，到响应处理函数中进行解析</param>
        /// <returns></returns>
        protected string CallCustomAction(string json_params)
        {
            if (CustomAction != "")
            {
                string strjson1 = string.Format("{{timer_id:{0}, user_params:'{1}'}}", ID, json_params);

                try
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(strjson1);

                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(CustomAction);
                    request.ContentType = @"application/json";
                    request.Accept = "application/xml";
                    request.Method = "POST";
                    request.ContentLength = bytes.Length;

                    Stream postStream = request.GetRequestStream();
                    postStream.Write(bytes, 0, bytes.Length);
                    postStream.Dispose();

                    //结束动作函数返回值
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    Stream responseStream = response.GetResponseStream();
                    StreamReader sr = new StreamReader(responseStream, Encoding.UTF8);
                    //对返回结果进行处理
                    return sr.ReadToEnd();

                }
                catch (Exception e)
                {
                    Trace.WriteLine("EnterEvent error:" + e.Message);
                    return "failed";
                }
            }
            else
                return "";
        }

        /// <summary>
        /// 执行任务函数的回调
        /// </summary>
        /// <param name="sender"></param>
        protected virtual void __JobCallBack(object sender)
        {
            AsyncResult asyRes = (AsyncResult)sender;

            //如果下次执行时间为空，则说明任务执行完毕
            if (((CTimerMission)(asyRes.AsyncState)).m_triggerT.NextTiming == null)
                ((CTimerMission)(asyRes.AsyncState)).status = TM_STATUS.TM_FINISH;

            //若在任务执行过程中，改变了任务状态， 则需要将该任务从待执行任务列表中移除
            if (((CTimerMission)(asyRes.AsyncState)).status != TM_STATUS.TM_STATUS_ACTIVE)
                CTimerManage.RemoveFromActiveList(((CTimerMission)(asyRes.AsyncState)).ID);

            ((CTimerMission)(asyRes.AsyncState)).Save();
            //到此，本次任务执行完成， 将标识置为不在运行状态
            Interlocked.Exchange(ref ((CTimerMission)(asyRes.AsyncState)).m_isRun, 0);

            (asyRes.AsyncDelegate as JobProcessing).EndInvoke(asyRes);
        }
        #endregion
    }
}

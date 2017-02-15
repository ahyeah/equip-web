using FlowEngine.Flow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace FlowEngine.Event
{
    /// <summary>
    /// 定时性任务
    /// </summary>
    public class CTimerEvent : CEvent
    {
        public CTimerEvent(CWorkFlow parent)
            : base(parent)
        {

		}
        /// <summary>
        /// 定时任务的执行间隔， 单位：天
        /// </summary>
        public int Interval
        {
            get; set;
        }

        /// <summary>
        /// Waiting Action: 等待事件（Event）执行的动作
        /// Action（动作）:对应于MVC的Controller
        /// 当定时器定时事件未到时，返回
        /// 与之对应，currentaction 在定时器事件到达时返回
        /// </summary>
        private string m_WaitingAction = "";

        /// <summary>
        /// 定时事件下次触发的时间
        /// </summary>
        private DateTime m_nextTiming = new DateTime();

        /// <summary>
        /// Event的状态
        /// processing: 处理定时任务中
        /// waiting: 等待下次任务到来
        /// 初始状态为waiting
        /// </summary>
        private string m_WorkStatus = "waiting";
        public string workstatus
        {
            get
            {
                return m_WorkStatus;
            }
            set
            {
                m_WorkStatus = value;
            }
        }

        public string waitingaction
        {
            get
            {
                return m_WaitingAction;
            }
            set
            {
                m_WaitingAction = value;
            }
        }

        /// <summary>
        /// 重载currentaction
        /// 如果当前状态为 processing, 则返回currentaction
        /// 如果当前状态为 waiting, 则返回waitingaction
        /// </summary>
        public override string currentaction
        {
            get
            {
                if (m_WorkStatus == "processing")
                {
                    return base.currentaction;
                }
                else if (m_WorkStatus == "waiting")
                {
                    return m_WaitingAction;
                }
                else
                    return "";
            }
            set
            {
                base.currentaction = value;
            }
        }
        
        /// <summary>
        /// 重载Event的UpdatParams， 该函数仅在workflow的submitsignal函数内调用，
        /// 因此可任务该函数是用户处理的时刻
        /// </summary>
        /// <param name="strjson"></param>
        /// <returns></returns>
        public override bool UpdateParams(string strjson)
        {
            DateTime dt = DateTime.Now;
            //当前状态为processing
            if (m_WorkStatus == "processing")
            {                
                //如果提交时间未超时
                if (dt <= m_nextTiming)
                {
                    //更新变量的值
                    if (!base.UpdateParams(strjson))
                        throw new Exception("UpdateParams error");
                                        
                    LeaveEvent("");
                    EnterEvent("");

                    //即本次触发已处理，状态转为等待下次触发
                    m_WorkStatus = "waiting";
                }
                //如果提交时间超时,即认为上一次或若干次任务未处理，因此将未处理任务存储到数据
                //而将本次提交的数据作为最近一次任务提交的数据
                else
                {
                    m_WorkStatus = "waiting";
                    while(dt > m_nextTiming)
                    {
                        LeaveEvent("");
                        EnterEvent("");
                        m_nextTiming.AddDays(this.Interval);
                    }

                    //将本次提交的数据作为最近一次任务数据，进行处理
                    m_WorkStatus = "processing";
                    //更新变量的值
                    if (!base.UpdateParams(strjson))
                        throw new Exception("UpdateParams error");
                    LeaveEvent("");
                    EnterEvent("");

                    m_WorkStatus = "waiting";
                }
            }
            //当前状态为等待
            else if (m_WorkStatus == "waiting")
            {
                //如果等待到触发时间，但有之前的任务未处理（即当前时间大于触发事件+Interval，则将前面未处理的任务更新到数据库）
                while ((dt - m_nextTiming).Days > this.Interval)
                {                    
                    LeaveEvent("");
                    EnterEvent("");
                    m_nextTiming.AddDays(this.Interval);
                }
                //更新变量的值
                if (!base.UpdateParams(strjson))
                    throw new Exception("UpdateParams error");
                LeaveEvent("");
                EnterEvent("");

            }
            return true;
        }
        /// <summary>
        /// 解析XML元素
        /// </summary>
        /// <param name="xmlNode"></param>
        public override void InstFromXmlNode(XmlNode xmlNode)
        {
            if (xmlNode.Attributes["type"].Value != "timerevent")
                return;
            //1. 解析基类CEvent的成员
            base.InstFromXmlNode(xmlNode);            

            //2. 解析TimerEvent的成员
            //解析 waitingaction
            XmlNode actions = xmlNode.SelectSingleNode("actions");
            XmlNode m_WaitingAction = actions.SelectSingleNode("waitingaction");
            //解析Interval
            this.Interval = Convert.ToInt32(xmlNode.Attributes["Interval"].Value);
            //解析状态
            this.m_WorkStatus = xmlNode.SelectSingleNode("status").InnerText;
            
            //3. 解析下次触发时间
            m_nextTiming = DateTime.Parse(xmlNode.SelectSingleNode("Timing").InnerText);
                        
        }

        /// <summary>
        /// 进入事件Action
        /// </summary>
        /// <param name="strjson"></param>
        public override void EnterEvent(string strjson)
        {
        }

        /// <summary>
        /// 离开事件Action
        /// </summary>
        /// <param name="strjson"></param>
        public override void LeaveEvent(string strjson)
        {
        }
    }
}

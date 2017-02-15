using FlowEngine.Modals;
using FlowEngine.TimerManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace FlowEngine.Event
{
    public class CTimeOutProperty : IXMLEntity
    {
        #region 构造函数
        public CTimeOutProperty(CEvent parent)
        {
            Parent = parent;
        }
        #endregion

        #region 相对时间起始点类型
        public enum TIME_START { TIME_WF_CREATE, TIME_EVENT_ENTER } ;
        #endregion

        #region 属性
        /// 说明： m_timeOffset与m_startTime成对使用
        /// m_ExactTime单独使用
        /// 一般情况下，上面两组时间指定方式选一组， 如果两组都指定则以具体时间为标准
        /// <summary>
        /// 相对时间起始点
        /// </summary>
        private TIME_START? m_startTime = null;
        public TIME_START? StartTime
        {
            get
            {
                return m_startTime;
            }
            set
            {
                m_startTime = value;
            }
        }

        /// <summary>
        /// 相对于起点的时间偏移量
        /// </summary>
        private TimeSpan? m_timeOffset = null;
        public TimeSpan? TimeOffset
        {
            get
            {
                return m_timeOffset;
            }
            set
            {
                m_timeOffset = value;
            }
        }

        /// <summary>
        /// 具体时间
        /// </summary>
        private DateTime? m_ExactTime = null;
        public DateTime? ExactTime
        {
            get
            {
                return m_ExactTime;
            }
            set
            {
                m_ExactTime = value;
            }
        }

        /// <summary>
        /// Timeout的动作
        /// </summary>
        private string m_action = "";
        public string Action
        {
            get
            {
                return m_action;
            }
            set
            {
                m_action = value;
            }
        }

        /// <summary>
        /// 所属的Event
        /// </summary>
        public CEvent Parent
        {
            get;
            set;
        }

        /// <summary>
        /// 回调函数的url地址
        /// </summary>
        public string CallbackUrl { get; set; }

        /// <summary>
        /// 该超时任务在定时任务管理器中的ID号
        /// </summary>
        private int m_timerMissionID = -1;
        #endregion

        #region IXMLEntity 接口
        public void InstFromXmlNode(XmlNode xmlNode)
        {
            XmlElement xe = (XmlElement)xmlNode;

            //1. 判断该节点是否为 "timeout", 并读取相关属性
            if (xmlNode.Name.ToLower() != "timeout_setting")
                return;

            m_action = xmlNode.Attributes["action"].Value;
            if (xmlNode.Attributes["linkTimerID"] == null || xmlNode.Attributes["linkTimerID"].Value == "")
                m_timerMissionID = -1;
            else
                m_timerMissionID = Convert.ToInt32(xmlNode.Attributes["linkTimerID"].Value);

            //2. 读取具体时间
            XmlNode eTime = xmlNode.SelectSingleNode("exact_time");
            if (eTime == null || eTime.InnerText.Trim() == "")
                m_ExactTime = null;
            else
            {
                m_ExactTime = DateTime.Parse(eTime.InnerText);
            }

            //3. 读取偏移时间
            XmlNode oTime = xmlNode.SelectSingleNode("offset_time");
            if (oTime == null)
            {
                m_startTime = null;
                m_timeOffset = null;
            }
            else
            {
                string startType = oTime.Attributes["time_start"].Value;
                string offset = oTime.Attributes["time_offset"].Value;

                if (offset.Trim() == "")
                {
                    m_startTime = null;
                    m_timeOffset = null;
                }
                else
                {
                    m_timeOffset = TimeSpan.Parse(offset);
                    switch (startType.ToLower())
                    {
                        case "wf_create":
                            m_startTime = TIME_START.TIME_WF_CREATE;
                            break;

                        case "ev_enter":
                            m_startTime = TIME_START.TIME_EVENT_ENTER;
                            break;

                        default:
                            m_startTime = null;
                            m_timeOffset = null;
                            break;
                    }
                }
            }

            //4. 读取callback的地址
            XmlNode urlNode = xmlNode.SelectSingleNode("call_back");
            if (urlNode != null)
                CallbackUrl = urlNode.InnerText;
            else
                CallbackUrl = "";
        }

        public XmlNode WriteToXmlNode()
        {
            XmlDocument doc = new XmlDocument();

            XmlElement toNode = doc.CreateElement("timeout_setting");
            toNode.SetAttribute("action", m_action);
            toNode.SetAttribute("linkTimerID", Convert.ToString(m_timerMissionID));

            //1. 构建具体时间节点
            if (m_ExactTime != null)
            {
                XmlElement et = doc.CreateElement("exact_time");
                et.InnerText = m_ExactTime.Value.ToString();
                toNode.AppendChild(et);
            }

            //2. 构建偏移时间节点
            if (m_startTime != null && m_timeOffset != null)
            {
                XmlElement ot = doc.CreateElement("offset_time");
                string time_start = "";
                switch (m_startTime)
                {
                    case TIME_START.TIME_WF_CREATE:
                        time_start = "wf_create";
                        break;

                    case TIME_START.TIME_EVENT_ENTER:
                        time_start = "ev_enter";
                        break;
                }
                ot.SetAttribute("time_start", time_start);

                ot.SetAttribute("time_offset", m_timeOffset.Value.ToString());
                toNode.AppendChild(ot);
            }

            //3. 构建callback节点
            XmlElement cb = doc.CreateElement("call_back");
            cb.AppendChild(doc.CreateCDataSection(CallbackUrl));
            toNode.AppendChild(cb);

            return toNode;
        }
        #endregion

        #region 公共方法
        /// <summary>
        /// 设置属性
        /// </summary>
        /// <param name="attr_name">属性名，可选值 exact_time, offset_time, time_start</param>
        /// <param name="val">属性的值，
        /// 当attr_name = "exact_time", val为datetime
        ///   attr_name = "offset_time", val为timespan
        ///   attr_name = "time_start", val为string, 可选值wf_create, ev_enter</param>
        /// <returns></returns>
        public bool SetAttribute(string attr_name, object val)
        {
            try
            {
                switch (attr_name)
                {
                    case "exact_time":
                        m_ExactTime = (val as DateTime?);
                        break;

                    case "offset_time":
                        m_timeOffset = (val as TimeSpan?);
                        break;

                    case "time_start":
                        if ((val as string) == "wf_create")
                            m_startTime = TIME_START.TIME_WF_CREATE;
                        else if ((val as string) == "ev_enter")
                            m_startTime = TIME_START.TIME_EVENT_ENTER;
                        else
                            return false;
                        break;
                    case "action":
                        m_action = (val as string);
                        break;

                    case "call_back":
                        CallbackUrl = (val as string);
                        break;

                    default:
                        return false;
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 该Timeout属性表示的含义是否有效
        /// </summary>
        /// <returns></returns>
        public bool IsEnable()
        {
            if (m_ExactTime != null)        //具体时间指定——有效
                return true;
            else if (m_startTime != null && m_timeOffset != null)                      //具体时间未指定， starttime与timeoffset指定——有效
                return true;
            else
                return false;

        }

        /// <summary>
        /// 注册到Timer管理器
        /// </summary>
        /// <param name="bCreateWF">是否为创建工作流时刻</param>
        public void RegTimeoutTimer(bool bCreateWF)
        {
            //如果含义无效，直接返回
            if (!IsEnable())
                return;

            CTimerTimeout timeout_job = new CTimerTimeout();
            timeout_job.CreateTime = DateTime.Now;
            timeout_job.for_using = TIMER_USING.FOR_SYSTEM;
            timeout_job.mission_name = string.Format("{0}:{1}", Parent.Parent.name, Parent.name);
            timeout_job.status = TM_STATUS.TM_STATUS_ACTIVE;
            timeout_job.SetActionForWF(m_action);
            timeout_job.AttachWFEntityID = Parent.Parent.EntityID;
            timeout_job.CustomAction = CallbackUrl;
            timeout_job.EventName = Parent.name;


            string strCorn = "";

            //如果该函数是调用于工作流创建时刻，但是偏移时间的起始不是TIME_START.TIME_WF_CREATE 则注册失败， 直接返回
            if (bCreateWF)
            {
                if ((m_ExactTime == null) && (m_startTime != TIME_START.TIME_WF_CREATE))
                    return;
            }
            else //如果该函数是调用于进入Event时刻，但是偏移时间的起始不是TIME_START.TIME_WF_CREATE 则注册失败， 直接返回
            {
                if (m_ExactTime != null || m_startTime != TIME_START.TIME_EVENT_ENTER)
                    return;
            }

            if (m_ExactTime != null)
            {
                strCorn = string.Format("{0} {1} {2} {3} {4} {5} {6}",
                                        m_ExactTime.Value.Second,
                                        m_ExactTime.Value.Minute,
                                        m_ExactTime.Value.Hour,
                                        m_ExactTime.Value.Day,
                                        m_ExactTime.Value.Month,
                                        "?",
                                        m_ExactTime.Value.Year);
                timeout_job.SetTriggerTiming(strCorn);
            }
            else
            {
                DateTime run_time = DateTime.Now + m_timeOffset.Value;
                strCorn = string.Format("{0} {1} {2} {3} {4} {5} {6}",
                                        run_time.Second,
                                        run_time.Minute,
                                        run_time.Hour,
                                        run_time.Day,
                                        run_time.Month,
                                        "?",
                                        run_time.Year);
                timeout_job.SetTriggerTiming(strCorn);
            }

            //保存该定时任务，并将其添加到激活任务列表 
            timeout_job.Save();
            m_timerMissionID = timeout_job.ID;
            CTimerManage.AppendMissionToActiveList(timeout_job);
        }

        /// <summary>
        /// 取消该超时任务在定时任务管理器中的注册
        /// </summary>
        public void UnregTimeoutTimer()
        {
            var t = CTimerManage.LoadTimerMission(m_timerMissionID);

            if (t != null)
            {
                t.status = TM_STATUS.TM_FINISH;
                t.Save();
                CTimerManage.RemoveFromActiveList(t.ID);
            }
        }
        #endregion
    }
}

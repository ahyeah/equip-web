using FlowEngine.Modals;
using FlowEngine.TimerManage;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowEngine.Event
{
    class CWaitingEvent : CEvent
    {
        private struct WaitingTime{
            public int Minute;
            public int Hours;
            public int Days;
            public int Months;
            public int Years;
        }
        #region 构造函数
        public CWaitingEvent(CWorkFlow parent) : base(parent)
        {
            
            m_timer.AttachWFEntity(parent);
            m_timer.for_using = TIMER_USING.FOR_SYSTEM;
            m_timer.SetRunParam(WE_STATUS.ACTIVE);
        }

        ~CWaitingEvent()
        {

        }

        #endregion

        #region 属性               

        /// <summary>
        /// 与之关联的定时性任务
        /// </summary>
        public CTimerWFStatus m_timer = new CTimerWFStatus();

        public int m_timerID = -1;

        private WaitingTime m_waitingTime = new WaitingTime() { Minute = 0, Hours = 0, Days = 0, Months = 0, Years = 0};
        
        #endregion

        #region 公共接口
        /// <summary>
        /// 不能序列化为XML node
        /// </summary>
        /// <param name="xmlNode"></param>
        public override void InstFromXmlNode(System.Xml.XmlNode xmlNode)
        {
            return;
        }

        public override System.Xml.XmlNode WriteToXmlNode()
        {
            return null;
        }

        /// <summary>
        /// 进入该事件
        /// </summary>
        /// <param name="strjson"></param>
        public override void EnterEvent(string strjson)
        {
            base.UpdateCurrentEvent();
            Parent.UpdateEntity(WE_STATUS.SUSPEND);

            DateTime dt = DateTime.Now;
            dt = dt.AddYears(m_waitingTime.Years);
            dt = dt.AddMonths(m_waitingTime.Months);
            dt = dt.AddDays(m_waitingTime.Days);
            dt = dt.AddHours(m_waitingTime.Hours);
            dt = dt.AddMinutes(m_waitingTime.Minute);

            string strCorn = string.Format("{0} {1} {2} {3} {4} {5} {6}",
                                        0,
                                        dt.Minute,
                                        dt.Hour,
                                        dt.Day,
                                        dt.Month,
                                        "?",
                                        dt.Year);
            m_timer.SetTriggerTiming(strCorn);
            m_timer.CreateTime = DateTime.Now;
            m_timer.AttachWFEntity(Parent);
            m_timer.Save();
            m_timerID = m_timer.ID;
            CTimerManage.AppendMissionToActiveList(m_timer);
            //m_timeout.RegTimeoutTimer(false);
        }

        /// <summary>
        /// 离开该事件
        /// </summary>
        /// <param name="strjson"></param>
        public override void LeaveEvent(string strjson)
        {
            //保存信息到数据库
            //base.InsertMissionToDB();
            //m_timeout.UnregTimeoutTimer();
            var t = CTimerManage.LoadTimerMission(m_timerID);

            if (t != null)
            {
                t.status = TM_STATUS.TM_FINISH;
                t.Save();
                CTimerManage.RemoveFromActiveList(t.ID);
            }
        }

        /// <summary>
        /// 设置等待时间间隔
        /// </summary>
        /// <param name="str_waiting">"H D M Y"</param>
        /// <returns></returns>
        public bool SetWaitingTime(string str_waiting)
        {
            var subs = str_waiting.Split(new char[] { ' ' });
            if (subs.Length != 5)
                return false;

            try
            {
                m_waitingTime.Minute = Convert.ToInt32(subs[0]);
                m_waitingTime.Hours = Convert.ToInt32(subs[1]);
                m_waitingTime.Days = Convert.ToInt32(subs[2]);
                m_waitingTime.Months = Convert.ToInt32(subs[3]);
                m_waitingTime.Years = Convert.ToInt32(subs[4]);
                return true;

            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 获得等待时间字符串表达格式
        /// </summary>
        /// <returns></returns>
        public string GetWaitingTime()
        {
            return string.Format("{0} {1} {2} {3} {4}", m_waitingTime.Minute, m_waitingTime.Hours, m_waitingTime.Days, m_waitingTime.Months, m_waitingTime.Years);
        }
        #endregion
    }
}

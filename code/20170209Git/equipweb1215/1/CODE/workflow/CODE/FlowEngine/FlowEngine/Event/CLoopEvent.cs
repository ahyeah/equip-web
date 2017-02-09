using FlowEngine.Flow;
using FlowEngine.Modals;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace FlowEngine.Event
{
    /// <summary>
    /// 循环事件
    /// </summary>
    public class CLoopEvent : CEvent
    {
        #region 构造函数
        public CLoopEvent(CWorkFlow parent) : base(parent)
        {
            m_waiting = new CWaitingEvent(parent);
        }

        ~CLoopEvent()
        {

        }
        #endregion

        
        #region 属性
        /// <summary>
        /// 等待事件
        /// </summary>
        private CWaitingEvent m_waiting;

        /// <summary>
        /// 循环条件
        /// </summary>
        private string m_loopCondition = "";


        private string m_cornString = "";
        #endregion

        #region 公共接口
        public override void InstFromXmlNode(XmlNode xmlNode)
        {
            //基类成员解析
            base.InstFromXmlNode(xmlNode);

            //循环条件解析
            XmlNode loopCondition = xmlNode.SelectSingleNode("loop_contidion");
            if (loopCondition != null)
                m_loopCondition = loopCondition.InnerText;
            else
                m_loopCondition = "1 !> 1";

            //成员解析
            m_waiting.name = name + ":wating";
            Parent.events.Add(m_waiting.name, m_waiting);
            m_waiting.m_timer.mission_name = m_waiting.name + ":timer";

            //新增flow
            CFlow flow_w2s = new CFlow(Parent.paramstable, false);
            flow_w2s.expression = "1 == 1";
            flow_w2s.source_event = m_waiting.name;
            flow_w2s.destination_event = this.name;
            flow_w2s.PrePase();
            Parent.AppendFlow(flow_w2s);

            CFlow flow_s2w = new CFlow(Parent.paramstable, false);
            flow_s2w.expression = m_loopCondition;
            flow_s2w.source_event = this.name;
            flow_s2w.destination_event = m_waiting.name;
            flow_s2w.PrePase();
            Parent.AppendFlow(flow_s2w);

            //循环时间控制
            XmlNode cornStr = xmlNode.SelectSingleNode("loop_time");
            m_cornString = cornStr.InnerText;

            XmlNode attTimer = xmlNode.SelectSingleNode("attach_timer");
            if (attTimer != null && attTimer.InnerText.Trim() != "")
                m_waiting.m_timerID = Convert.ToInt32(attTimer.InnerText);

            m_waiting.SetWaitingTime(m_cornString.Trim());

        }

        public override XmlNode WriteToXmlNode()
        {
            //1. 反解析基类部分成员
            XmlNode xn = base.WriteToXmlNode();

            //2. 反解析NormalEvent的成员
            XmlElement xe = (XmlElement)xn;
            xe.SetAttribute("type", "loopevent");

            //3. 反解析循环条件
            XmlDocument doc = new XmlDocument();
            XmlElement con_elem = doc.CreateElement("loop_contidion");
            con_elem.AppendChild(doc.CreateCDataSection(m_loopCondition));
            xn.AppendChild(xn.OwnerDocument.ImportNode(con_elem, true));

            //4. 反解析循环时间
            XmlElement looptime = doc.CreateElement("loop_time");
            looptime.AppendChild(doc.CreateCDataSection(m_waiting.GetWaitingTime()));
            xn.AppendChild(xn.OwnerDocument.ImportNode(looptime, true));

            XmlElement attTimer = doc.CreateElement("attach_timer");
            attTimer.InnerText = Convert.ToString(m_waiting.m_timerID);

            xn.AppendChild(xn.OwnerDocument.ImportNode(attTimer, true));

            return xn;
        }

        /// <summary>
        /// 进入该事件（Event）
        /// </summary>
        /// <param name="strjson"></param>
        public override void EnterEvent(string strjson)
        {
            if (beforeaction != "")
            {
                string strjson1 = "{param:'{";
                foreach (var par in m_beforeActionParams)
                {
                    string tmp = string.Format("{0}:\"{1}\",", par.Key, parselEventActionParams(par.Value));
                    strjson1 += tmp;
                }
                strjson1.TrimEnd(new char[] { ',' });
                strjson1 += "}'}";
                try
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(strjson1);

                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(beforeaction);
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
                    OpResutsfromAction(sr.ReadToEnd());

                }
                catch (Exception e)
                {
                    Trace.WriteLine("EnterEvent error:" + e.Message);
                    //return;
                }
            }
            base.UpdateCurrentEvent();
            m_timeout.RegTimeoutTimer(false);
        }

        /// <summary>
        /// 离开该事件（Event）
        /// </summary>
        /// <param name="strjson"></param>
        public override void LeaveEvent(string strjson)
        {

            if (beforeaction != "")
            {
                string strjson1 = "{param:'{";
                foreach (var par in m_afterActionParams)
                {
                    string tmp = string.Format("{0}:\"{1}\",", par.Key, parselEventActionParams(par.Value));
                    strjson1 += tmp;
                }
                strjson1.TrimEnd(new char[] { ',' });
                strjson1 += "}'}";
                try
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(strjson1);

                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(afteraction);
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
                    OpResutsfromAction(sr.ReadToEnd());

                }
                catch (Exception e)
                {
                    Trace.WriteLine("EnterEvent error:" + e.Message);
                    //return;
                }
            }
            //保存信息到数据库
            base.InsertMissionToDB();
            m_timeout.UnregTimeoutTimer();
        }

        #endregion
    }
}

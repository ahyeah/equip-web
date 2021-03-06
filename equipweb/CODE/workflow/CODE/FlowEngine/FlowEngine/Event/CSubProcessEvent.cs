﻿using FlowEngine.DAL;
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
    /// 子流程事件
    /// </summary>
    public class CSubProcessEvent : CEvent
    {
        /// <summary>
        /// m_wfName 子流程工作流名称
        /// </summary>
        public string WfName { get; set; }

        /// <summary>
        /// 子流程的ID
        /// </summary>
        public int WfEntityId { get; set; }

        /// <summary>
        /// 子工作流的工作方式：
        /// serial——串行
        /// parallel——并行
        /// </summary>
        public string WorkingMode { get; set; }

        /// <summary>
        /// 传递到子流程的参数对应关系
        /// </summary>
        public Dictionary<string, string> m_paramsTo = new Dictionary<string, string>();

        /// <summary>
        /// 接受子流程的返回值对应关系
        /// </summary>
        public Dictionary<string, string> m_paramsFrom = new Dictionary<string, string>();

        public CSubProcessEvent(CWorkFlow parent) : base(parent)
        {

        }

        /// <summary>
        /// 解析XML元素
        /// </summary>
        /// <param name="xmlNode"></param>
        public override void InstFromXmlNode(XmlNode xmlNode)
        {
            if (xmlNode.Attributes["type"].Value != "subprocess")
                return;
            //1. 解析基类CEvent的成员
            base.InstFromXmlNode(xmlNode);
            currentaction = ""; //无论XML是否指定currentaction, 它都为空
            //2. 解析SubProcessEvent的成员

            //3. 反解析子工作流
            XmlNode linwf = xmlNode.SelectSingleNode("linkworkflow");
            WfName = linwf.Attributes["name"].Value;
            if (linwf.Attributes["workingmodel"] == null || linwf.Attributes["workingmodel"].Value.Trim() == "")
                WorkingMode = "serial";
            else
                WorkingMode = linwf.Attributes["workingmodel"].Value;

            if (linwf.Attributes["id"] == null || linwf.Attributes["id"].Value.Trim() == "")
                WfEntityId = -1;
            else
                WfEntityId = Convert.ToInt32(linwf.Attributes["id"].Value);

            //3.1 反解析参数传递
            XmlNode pars_transfer = linwf.SelectSingleNode("paramtransfer");
            // 变量对应关系
            foreach(XmlNode item in pars_transfer.SelectNodes("item"))
            {
                if (item.Attributes["direction"].Value == "to") // to
                    m_paramsTo[item.Attributes["parent"].Value] = item.Attributes["child"].Value;
                else if (item.Attributes["direction"].Value == "from") // from
                    m_paramsFrom[item.Attributes["parent"].Value] = item.Attributes["child"].Value;
                else
                    return;
            }
        }

        /// <summary>
        /// 反解析到XML元素
        /// </summary>
        public override XmlNode WriteToXmlNode()
        {

            //1. 反解析基类部分成员
            XmlNode xn = base.WriteToXmlNode();

            //2. 反解析SubProcessEvent的成员
            XmlElement xe = (XmlElement)xn;
            xe.SetAttribute("type", "subprocess");            

            //3. 反解析相关的工作流            
            XmlElement linkwf = xn.OwnerDocument.CreateElement("linkworkflow");
            linkwf.SetAttribute("name", WfName);
            linkwf.SetAttribute("workingmodel", WorkingMode);
            linkwf.SetAttribute("id", WfEntityId.ToString());

            //3.1 添加参数传递
            XmlElement pars_transfer = linkwf.OwnerDocument.CreateElement("paramtransfer");
            //a. 添加To变量对应关系
            foreach(var par in m_paramsTo)
            {
                XmlElement parTo = linkwf.OwnerDocument.CreateElement("item");
                parTo.SetAttribute("direction", "to");
                parTo.SetAttribute("parent", par.Key);
                parTo.SetAttribute("child", par.Value);
                pars_transfer.AppendChild(parTo);
            }
            //b. 添加From变量对应关系
            foreach(var par in m_paramsFrom)
            {
                XmlElement parFrom = linkwf.OwnerDocument.CreateElement("item");
                parFrom.SetAttribute("direction", "from");
                parFrom.SetAttribute("parent", par.Key);
                parFrom.SetAttribute("child", par.Value);
                pars_transfer.AppendChild(parFrom);
            }
            linkwf.AppendChild(pars_transfer);

            xe.AppendChild(linkwf);
            
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

            CWorkFlow wf = new CWorkFlow();
            //2016/2/12--保证子工作流串号与父工作流相同
            if (!wf.CreateEntity(WfName, this.m_parentWF.EntitySerial))
            {
                Trace.WriteLine("Create {0} SubProcess error!", WfName);
                return;
            }
            WfEntityId = wf.EntityID;
            wf.ParentEntityID = m_parentWF.EntityID;
            //设置子流程初始参数
            foreach(var parTo in m_paramsTo)
            {
                wf.paramstable[parTo.Value].value = m_params[parTo.Key].value;
            }

            //2016/2/12--将自己的Record传给子流程
            //2016/2/14--发现WFEngine中已通过Post_processSubprocess将record传给了子流程，故而取消修改
            //至于为何没有起作用，待调试
            //WorkFlows wfs = new WorkFlows();
            //Mission ms = wfs.GetWFEntityLastMission(wf.EntityID);
            //List<Process_Record> parent_res = wfs.GetMissionRecordInfo(ms.Miss_Id);
            //Dictionary<string, string> res = new Dictionary<string, string>();
            //foreach (var re in parent_res)
            //{
                //如果record中包含事件定义的需要记录的record item则记录到数据库中
            //    if (m_parentWF.GetRecordItems().ContainsKey(re.Re_Name))
            //    {
            //         res[re.Re_Name] = re.Re_Value;
            //    }
            //}
            wf.Start(null); //原始版本 wf.Start(null);

            string sub_status = ""; 
            do
            {
                sub_status = wf.GetCurrentState();
                wf.SubmitSignal("[]");
            } while (wf.GetCurrentState() != sub_status); //给子流程激励，直至其状态不再发生变化，即需要人员介入

            //如果以并行方式工作, 发送激励信号
            //if (WorkingMode == "parallel")
            //    m_parentWF.SubmitSignal("[]");
            base.UpdateCurrentEvent();
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
        }

    }
}

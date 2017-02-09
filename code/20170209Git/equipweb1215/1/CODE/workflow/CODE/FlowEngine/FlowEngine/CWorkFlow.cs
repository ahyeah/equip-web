///////////////////////////////////////////////////////////
//  CWorkFlow.cs
//  Implementation of the Class CWorkFlow
//  Generated by Enterprise Architect
//  Created on:      26-10月-2015 9:46:30
//  Original author: Ailibuli
///////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;



using FlowEngine;
using FlowEngine.Param;
using FlowEngine.Event;
using System.Xml;
using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using FlowEngine.UserInterface;
using FlowEngine.Modals;
using FlowEngine.DAL;
namespace FlowEngine {
	/// <summary>
	/// 工作流管理类
	/// </summary>
    public class CWorkFlow : IXMLEntity, UI_WorkFlow_Entity
    {
        /// <summary>
        /// 该工作流定义的ID号
        /// </summary>
        private int m_defineID = -1;
		/// <summary>
		/// 工作流实体的ID，表示依据该工作模型建立的一个具体工作流，该ID在整个系统中唯一
		/// </summary>
		private int m_entityID = -1;
		/// <summary>
		/// 工作流状态迁移流程，列表的最后一个元素表示当前事件（状态）
		/// </summary>
		private List<string> m_eventProc = new List<string>();
		/// <summary>
		/// 工作流（WorkFlow）中事件（Event）集合，其中Key为事件名称
		/// </summary>
		private Dictionary<string, Event.IEvent> m_events = new Dictionary<string, Event.IEvent>();
		/// <summary>
		/// 工作流的名称
		/// </summary>
		private string m_name = "";
		/// <summary>
		/// 工作流包括的用户数据
		/// </summary>
        private FlowEngine.Param.CParamTable m_paramtable = new FlowEngine.Param.CParamTable();
		/// <summary>
		/// 事件（CEvent）与变量（CParam）之间的联系 该字典的Key为CEvent名称， Value为与该事件对应的变量名组成的字符串数组
		/// </summary>
		private Dictionary<string, List<string>> m_relationEP = new Dictionary<string,List<string>>();
		/// <summary>
		/// 工作流（WorkFlow）的描述
		/// </summary>
		private string m_description = "";
		/// <summary>
		/// 流程（Flow）集合
		/// Key为Flow的源事件（Event）的名称
		/// </summary>
		private Dictionary<string, List<Flow.IFlow>> m_flows = new Dictionary<string, List<Flow.IFlow>>();
		/// <summary>
		/// 工作流当前事件（Event）
		/// </summary>
		private string m_courrentEvent = "";
        /// <summary>
        /// 工作流当前事件（Event）
        /// </summary>
        private List<string> m_recordItems = new List<string>();
        /// <summary>
        /// 工作流串号
        /// </summary>
        private string m_entitySerial = "";
        /// <summary>
        /// 父工作流的EntityID, 如果该工作流不是子工作流则m_parentEntity = -1
        /// </summary>
        private int m_parentEntity = -1;
        /// <summary>
        /// 工作流最后一次状态迁移时间
        /// </summary>
        public DateTime? Last_TransTime
        {
            get;
            set;
        }

		public CWorkFlow(){
            
            
		}

		~CWorkFlow(){
           
		}

        public CParamTable paramstable
        {
            get
            {
                return m_paramtable;
            }
        }

        /// <summary>
        /// 工作流的事件
        /// </summary>
        public IDictionary<string, IEvent> events 
        {
            get 
            {
                return m_events;

            }            
        }
        /// <summary>
        /// 设置获取工作流的父工作流
        /// </summary>
        public int ParentEntityID
        {
            get
            {
                return m_parentEntity;
            }
            set
            {
                m_parentEntity = value;
            }
        }

        public int DefineID
        {
            get
            {
                return m_defineID;
            }
            set
            {
                m_defineID = value;
            }
        }
		/// <summary>
		/// 工作流实体的ID，表示依据该工作模型建立的一个具体工作流，该ID在整个系统中唯一
		/// </summary>
		public int EntityID{
			get{
				return m_entityID;
			}
            set
            {
                m_entityID = value;
            }
		}
        /// <summary>
        /// 设置、获取工作流的串号
        /// </summary>
        public string EntitySerial
        {
            get
            {
                return m_entitySerial;
            }
            set
            {
                m_entitySerial = value;
            }
        }

        /// <summary>
        /// 工作流需要记录的信息
        /// </summary>
        public List<string> RecordItems
        {
            get
            {
                return m_recordItems;
            }
        }

        //更新工作流实体，（修改数据相应的项）
        public void UpdateEntity(WE_STATUS status)
        {
            try
            {
                WorkFlow_Entity wfe = new WorkFlow_Entity
                {
                    WE_Id = m_entityID,
                    WE_Status = status,
                    Last_Trans_Time = this.Last_TransTime
                };
                wfe.WE_Binary = Encoding.Default.GetBytes(WriteToXmlNode().OuterXml);

                WorkFlows wfs = new WorkFlows();
                if (!wfs.SaveWorkFlowEntity(wfe))
                    throw new Exception("Save WorkFlow Entity failed!");

            }
            catch(Exception e)
            {
                return;
            }
        }

        public Dictionary<string, string> GetRecordItems()
        {
            Dictionary<string, string> re = new Dictionary<string, string>();
            foreach(string s in m_recordItems)
            {
                re.Add(s, null);
            }
            return re;
        }
        /// <summary>
        /// 检查SubProcess的状态
        /// </summary>
        private void CheckSubEventStatus()
        {
            //如果当前状态是SubProcess， 则要检测子时间是否已经执行完毕，
            //若已经执行完毕，则需要将返回值从子事件中取回
            if (m_events[m_courrentEvent].GetType().Name == "CSubProcessEvent")
            {
                CSubProcessEvent subE = (CSubProcessEvent)(m_events[m_courrentEvent]);
                WorkFlows wfs = new WorkFlows();

                WorkFlow_Entity wfe = wfs.GetWorkFlowEntity(subE.WfEntityId);
                //自流程已执行完成
                if (wfe.WE_Status == WE_STATUS.DONE)
                {
                    CWorkFlow subWf = new CWorkFlow();
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(Encoding.Default.GetString(wfs.GetWorkFlowEntity(subE.WfEntityId).WE_Binary));
                    subWf.InstFromXmlNode(doc.DocumentElement);
                    subWf.EntityID = subE.WfEntityId;

                    IEvent subEnd = subWf.GetCurrentEvent();
                    //将变量返回回来
                    foreach (var p in subE.m_paramsFrom)
                    {
                        subE.paramlist[p.Key].value = subEnd.paramlist[p.Value].value;
                    }
                    //若子工作流的工作方式为串行， 则父工作流在子流程完成后继续向下执行
                    if (subE.WorkingMode == "serial")
                    {
                        StateTransfer("");
                        if (m_events[m_courrentEvent].GetType().Name == "CEndEvent")
                        {
                            //将该工作流置为已完成（结束）
                            UpdateEntity(WE_STATUS.DONE);
                            //2016.1.3 添加子事件返回

                        }
                        else
                            //更新工作流到数据库，不改变状态
                            UpdateEntity(WE_STATUS.INVALID);
                    }
                }


            }
        }
		/// <summary>
		/// 返回工作流的当前事件（状态）的名称
		/// </summary>
		public string GetCurrentState(){

            CheckSubEventStatus();
            return m_courrentEvent;
		}

        /// <summary>
        /// 返回工作流的当前事件（状态）
        /// </summary>
        public IEvent GetCurrentEvent()
        {
            CheckSubEventStatus();
            return m_events[m_courrentEvent];
        }

        /// <summary>
        /// 解析工作流基础信息
        /// </summary>
        /// <param name="xmlNode"></param>
        public void ParaseBaseInfo(XmlNode xmlNode)
        {
            //1. 判断xmlNode的类型
            if (xmlNode.Name != "workflow")
                return;

            //2. 解析workflow的属性
            m_name = xmlNode.Attributes["name"].Value;
            m_description = xmlNode.Attributes["desc"].Value;
            m_courrentEvent = xmlNode.Attributes["current_event"].Value;

            if (xmlNode.Attributes["entity_serial"] != null)
                m_entitySerial = xmlNode.Attributes["entity_serial"].Value;
            else
                m_entitySerial = "";

            if (xmlNode.Attributes["parent_entityid"] != null)
                m_parentEntity = Convert.ToInt32(xmlNode.Attributes["parent_entityid"].Value);
            else
                m_parentEntity = -1;
        }

        /// <summary>
        /// 解析参数列表
        /// </summary>
        /// <param name="xmlNode"></param>
        public void ParseParams(XmlNode xmlNode)
        {
            m_paramtable.InstFromXmlNode(xmlNode);
        }

		/// <summary>
		/// 解析XML元素
		/// </summary>
		/// <param name="xmlNode"></param>
		public void InstFromXmlNode(XmlNode xmlNode){
            //1. 判断xmlNode的类型
            if (xmlNode.Name != "workflow")
                return;

            //2. 解析workflow的属性
            m_name = xmlNode.Attributes["name"].Value;
            m_description = xmlNode.Attributes["desc"].Value;
            m_courrentEvent = xmlNode.Attributes["current_event"].Value;

            if (xmlNode.Attributes["entity_serial"] != null)
                m_entitySerial = xmlNode.Attributes["entity_serial"].Value;
            else
                m_entitySerial = "";

            if (xmlNode.Attributes["parent_entityid"] != null)
                m_parentEntity = Convert.ToInt32(xmlNode.Attributes["parent_entityid"].Value);
            else
                m_parentEntity = -1;

            //3. 反解析参数表
            XmlNode paramstable = xmlNode.SelectSingleNode("paramtable");
            m_paramtable.InstFromXmlNode(paramstable);

            //4. 解析事件
            XmlNode events = xmlNode.SelectSingleNode("events");
            ParseEvents(events);

            //5. 建立参数与事件之间的联系
            LinkParamEvent();

            //6. 解析流
            XmlNode flows = xmlNode.SelectSingleNode("flows");
            ParseFlows(flows);

            //7. 解析RecordItems
            XmlNode rItems = xmlNode.SelectSingleNode("recorditems");
            ParseRecordItems(rItems);
		}

        /// <summary>
        /// 解析RecordItems
        /// </summary>
        private void ParseRecordItems(XmlNode items)
        {
            if (items.Name != "recorditems")
                return;

            foreach(XmlNode xn in items.ChildNodes)
            {
                if (xn.Name.ToLower() != "item")
                    continue;

                m_recordItems.Add(xn.InnerText);
            }
        }

		/// <summary>
		/// 工作流的名称
		/// </summary>
		public string name{
			get{
				return m_name;
			}
            set{
                m_name = value;
            }
		}

		/// <summary>
		/// 工作流状态迁移		
		/// </summary>
        protected bool StateTransfer(string pars_json)
        {
            bool bTransfered = false;
            if (!m_flows.ContainsKey(m_courrentEvent))
                return bTransfered;
            foreach (Flow.IFlow flow in m_flows[m_courrentEvent])
            {
                if (flow.Evaluate())                {
                    
                    //离开当前事件
                    m_events[flow.source_event].LeaveEvent(pars_json);
                    //设置当前事件
                    m_courrentEvent = flow.destination_event;
                    //进入下一个事件
                    m_events[flow.destination_event].EnterEvent(pars_json);
                    
                    bTransfered = true;
                    break;
                }
            }
            return bTransfered;
		}

		/// <summary>
		/// 向工作流提交信号（Signal：工作流外部的推动动力），即将用户的处理结果（Param参数）提交给工作流。
		/// 用户提交的数据（Param参数）合法则返回true
		/// 用户提交的数据（Param参数）不合法则返回false（不合法的情况：1.  变量类型与CParamTable对应变量类型不符； 2.
		/// 提交的变量不属于当前事件（Event）； 3. 提交的变量值不合法）
		/// </summary>
		/// <param name="json">用户提供的用户处理结果，即参数表（CParamTable）里包含变量的新值</param>
		public bool SubmitSignal(string json){

            //1. 更新当前事件所关联的变量的值
            m_events[m_courrentEvent].UpdateParams(json);

            //2. 状态迁移
            bool bTrans = StateTransfer("");
            if (bTrans)
            {
                Last_TransTime = DateTime.Now;
            }

            if (m_events[m_courrentEvent].GetType().Name == "CEndEvent")
            {
                //将该工作流置为已完成（结束）
                UpdateEntity(WE_STATUS.DONE);
                //2016.1.3 添加子事件返回

            }
            else
                UpdateEntity(WE_STATUS.INVALID);
			return true;
		}

		/// <summary>
		/// 反解析到XML元素
		/// </summary>
		public XmlNode WriteToXmlNode(){
            XmlDocument doc = new XmlDocument();
            //1.创建WorkFlow节点
            XmlElement workflow = doc.CreateElement("workflow");
            workflow.SetAttribute("name", m_name);
            workflow.SetAttribute("desc", m_description);
            workflow.SetAttribute("current_event", m_courrentEvent);
            workflow.SetAttribute("entity_serial", m_entitySerial);
            if (m_parentEntity != -1)
                workflow.SetAttribute("parent_entityid", m_parentEntity.ToString());

            //2. 反解析参数列表
            XmlNode paramstable = m_paramtable.WriteToXmlNode();
            workflow.AppendChild(doc.ImportNode(paramstable, true));

            //3. 反解析事件
            XmlElement events = doc.CreateElement("events");
            foreach(Event.IEvent ev in m_events.Values){
                XmlNode xn = ((IXMLEntity)((Event.CEvent)ev)).WriteToXmlNode();
                if (xn != null)
                    events.AppendChild(doc.ImportNode(xn, true));
            }
            workflow.AppendChild(events);

            //4. 反解析流程
            XmlElement flows = doc.CreateElement("flows");
            foreach(List<Flow.IFlow> fls in m_flows.Values)
            {
                foreach (Flow.IFlow fl in fls)
                {
                    XmlNode xn = ((IXMLEntity)((Flow.IFlow)fl)).WriteToXmlNode();
                    if (xn != null)
                        flows.AppendChild(doc.ImportNode(xn, true));
                }
            }
            workflow.AppendChild(flows);

            //5. 反解析RecordItems
            XmlElement rItems = doc.CreateElement("recorditems");
            foreach(string item in m_recordItems)
            {
                XmlNode xn = doc.CreateElement("item");
                xn.InnerText = item;
                rItems.AppendChild(xn);
            }
            workflow.AppendChild(rItems);
            return workflow;
		}

		/// <summary>
		/// 工作流（WorkFlow）的描述
		/// </summary>
		public string description{
			get{
				return m_description;
			}
            set
            {
                m_description = value;
            }
		}

		/// <summary>
		/// 从XML中解析出事件列表（Events）
		/// </summary>
		/// <param name="xmlNode"></param>
		private void ParseEvents(XmlNode xmlNode){
            if (xmlNode.Name != "events")
                return;

            foreach (XmlNode ev in xmlNode.ChildNodes)
            {
                if (ev.Name != "event")
                    continue;
                switch (ev.Attributes["type"].Value)
                {
                    case "combevent":
                        CCombEvent ce = new CCombEvent(this);
                        ce.InstFromXmlNode(ev);
                        m_events.Add(ce.name, ce);
                        break;

                    case "endevent":
                        CEndEvent ee = new CEndEvent(this);
                        ee.InstFromXmlNode(ev);
                        m_events.Add(ee.name, ee);
                        break;

                    case "startevent":
                        CStartEvent se = new CStartEvent(this);
                        se.InstFromXmlNode(ev);
                        m_events.Add(se.name, se);
                        break;

                    case "normlevent":
                    case "normalevent":
                        CNormlEvent ne = new CNormlEvent(this);
                        ne.InstFromXmlNode(ev);
                        m_events.Add(ne.name, ne);
                        break;

                    case "subprocess":
                        CSubProcessEvent sp = new CSubProcessEvent(this);
                        sp.InstFromXmlNode(ev);
                        m_events.Add(sp.name, sp);
                        break;

                    case "loopevent":
                        CLoopEvent le = new CLoopEvent(this);
                        le.InstFromXmlNode(ev);
                        m_events.Add(le.name, le);
                        break;

                    default:
                        break;
                }
            }
		}

		/// <summary>
		/// 建立变量（Param）与事件（Event）间的联系
		/// </summary>
		private void LinkParamEvent(){
            try
            {
                int paNum = m_paramtable.ParamsNum;
                for (int i = 0; i < paNum; i++)
                {
                    CParam pa = m_paramtable[i];
                    foreach (string strEv in pa.linkEvents)
                    {
                        //如果CParam关联的Event不存在，则异常
                        if (!m_events.ContainsKey(strEv))
                            throw new Exception("Event " + strEv + " is not exist");

                        //如果pa关联的Event暂时未添加到m_relationEP中
                        if (!m_relationEP.ContainsKey(strEv))
                        {
                            m_relationEP.Add(strEv, new List<string>());
                        }
                        
                        //建立事件（strEv）与变量（pa）之间的联系
                        m_relationEP[strEv].Add(pa.name);
                        m_events[strEv].paramlist[pa.name] = pa;
                        m_events[strEv].paramsApp_res[pa.name] = pa.linkEventsApp_res[strEv];
                    }
                }
            }
            catch(Exception e)
            {
                Trace.WriteLine(e.Message);
            }
            
		}

		/// <summary>
		/// 添加一个变量
		/// </summary>
		/// <param name="par">添加的新变量</param>
		public void AppendParam(CParam par){
            try
            {   
                //1. 添加变量到变量列表
                m_paramtable.AppendParam(par);

                //2. 建立变量与事件之间的联系
                foreach(string strev in par.linkEvents)
                {
                    //2.1 判断包含变量的事件是否存在
                    if (!m_events.ContainsKey(strev))
                        throw new Exception("Event " + strev + " is not exist");

                    //2.2 如果par关联的事件还未添加到m_relationEP
                    if (!m_relationEP.ContainsKey(strev))
                    {
                        m_relationEP.Add(strev, new List<string>());
                    }

                    //2.3 建立par与事件之间的关联
                    m_relationEP[strev].Add(par.name);
                }
                
            }
            catch(Exception e)
            {
                Trace.WriteLine(e.Message);
            }
		}

		/// <summary>
		/// 添加一个事件（Event）
		/// </summary>
		/// <param name="ev">添加的新时间（Event）</param>
		public void AppendEvent(IEvent ev){
            try
            {
                //1. 判断ev是否已存在
                if (m_events.ContainsKey(ev.name))
                    throw new Exception("Event " + ev.name + " is already exist");

                //2. 添加ev到事件列表中
                m_events.Add(ev.name, ev);
            }
            catch(Exception e)
            {
                Trace.WriteLine(e.Message);
            }
            
		}

		/// <summary>
		/// 添加流程
		/// </summary>
		/// <param name="flow"></param>
		public void AppendFlow(Flow.IFlow flow){

            try
            {
                //1 如果已存在以flow.source_event为key的项
                if (m_flows.ContainsKey(flow.source_event))
                {
                    List<Flow.IFlow> eFlows = m_flows[flow.source_event];
                                        
                    foreach (Flow.IFlow eFlow in eFlows)
                    {
                        //1.1 判断flow是否已存在
                        //如果两个流的源事件、目的事件相同，且两个流的类型相同，并且表达式相同
                        //则认为这两个流相同
                        if ((eFlow.GetType() == flow.GetType())
                            && (eFlow.destination_event == flow.destination_event)
                            && (eFlow.expression == flow.expression))
                            throw new Exception("Flow is already exist");

                        //1.2 添加流到工作流中
                        eFlows.Add(flow);
                    }
                 }
                else //2 如果已存在以flow.source_event为key的项
                {
                    List<Flow.IFlow> flows = new List<Flow.IFlow>();
                    flows.Add(flow);
                    m_flows.Add(flow.source_event, flows);
                }

                
                
            }
            catch(Exception e)
            {
                Trace.WriteLine(e.Message);
            }
		}

		/// <summary>
		/// 解析流程（Flow）
		/// </summary>
		/// <param name="xmlNode"></param>
		private void ParseFlows(XmlNode xmlNode){
            //1. 检查xmlNode节点的合法性
            if (xmlNode.Name != "flows")
                return;

            //2. 解析每个flow
            foreach(XmlNode fn in xmlNode.ChildNodes)
            {
                /////////////////////////////////////对Flow种类的处理？？？

                Flow.CFlow flow = new Flow.CFlow(m_paramtable);
                flow.InstFromXmlNode(fn);
                if (m_flows.ContainsKey(flow.source_event))
                    m_flows[flow.source_event].Add(flow);
                else
                {
                    List<Flow.IFlow> flows = new List<Flow.IFlow>();
                    flows.Add(flow);
                    m_flows.Add(flow.source_event, flows);
                }
            }
		}

        //保存事件实体创建时的Record Item
        private List<Process_Record> SaveCreateRecord(Dictionary<string, string> records)
        {
            List<Process_Record> record_items = new List<Process_Record>();

            foreach(var item in records)
            {
                Process_Record pr = new Process_Record();
                pr.Re_Name = item.Key;
                pr.Re_Value = item.Value;
                record_items.Add(pr);
            }

            return record_items;
        }
        //根据自身信息创建工作流
        //该函数调用要确保自身信息构建完善
        public bool CreateEntityBySelf(string Ser_Num = null)
        {
            WorkFlows wfs = new WorkFlows();

            WorkFlow_Entity wfe = new WorkFlow_Entity();
            wfe.WE_Status = WE_STATUS.CREATED;
            wfe.WE_Binary = Encoding.Default.GetBytes(WriteToXmlNode().OuterXml);

            //2016/2/12 --保证子工作流串号与父工作流相同
            if (Ser_Num == null)
                wfe.WE_Ser = "";
            else
                wfe.WE_Ser = Ser_Num;

            if (!wfs.AddWorkEntity(name, wfe))
                return false;

            m_entityID = wfe.WE_Id;
            m_entitySerial = wfe.WE_Ser;
            //m_defineID = define.W_ID;

            RegEventsTimeOut(true);
            return true;
        }
        public bool CreateEntity(string wf_name, string Ser_Num = null/*2016/2/12--保证子工作流串号与父工作流相同*/)
        {
            WorkFlows wfs = new WorkFlows();

            XmlDocument doc = new XmlDocument();
            WorkFlow_Define define = wfs.GetWorkFlowDefine(wf_name);
            doc.LoadXml(Encoding.Default.GetString(define.W_Xml));
            InstFromXmlNode((XmlNode)doc.DocumentElement);

           

            WorkFlow_Entity wfe = new WorkFlow_Entity();
            wfe.WE_Status = WE_STATUS.CREATED;
            wfe.WE_Binary = Encoding.Default.GetBytes(WriteToXmlNode().OuterXml);
            
                //wfe.Create_Info = SaveCreateRecord();
            //2016/2/12 --保证子工作流串号与父工作流相同
            if (Ser_Num == null)
                wfe.WE_Ser = "";
            else
                wfe.WE_Ser = Ser_Num;            

            if (!wfs.AddWorkEntity(wf_name, wfe))
                return false;

            m_entityID = wfe.WE_Id;
            m_entitySerial = wfe.WE_Ser;
            m_defineID = define.W_ID;

            RegEventsTimeOut(true);   
            return true;

        }

       
        public void RegEventsTimeOut(bool bCreated = true)
        {
            foreach(var e in m_events)
            {
                e.Value.RegTimeOut(bCreated);
            }
        }

        //工作流开始运行
        public string Start(IDictionary<string, string> record)
        {
            IEvent startE = null;
            foreach(var item in m_events)
            {
                if (item.Value.GetType().Name == "CStartEvent")
                {
                    startE = item.Value;
                    break;
                }
            }
            if (startE == null)
                return null;
            m_courrentEvent = startE.name;
            startE.EnterEvent("");
            UpdateEntity(WE_STATUS.ACTIVE);
            SubmitSignal("[]");

            //状态发生了迁移
            if (GetCurrentState() != startE.name)
            {
                if (record != null)
                {
                    WorkFlows wfs = new WorkFlows();
                    Mission ms = wfs.GetWFEntityLastMission(EntityID);

                    List<Process_Record> res = new List<Process_Record>();
                    foreach (var re in record)
                    {
                        if (GetRecordItems().ContainsKey(re.Key))
                        {
                            Process_Record pre = new Process_Record();
                            pre.Re_Name = re.Key;
                            pre.Re_Value = re.Value;
                            res.Add(pre);
                        }
                    }
                    wfs.LinkRecordInfoToMiss(ms.Miss_Id, res);
                }
            }            
            
            return m_events[m_courrentEvent].currentaction + @"/?wfe_id=" + m_entityID;
        }

        //获取当前事件的处理页面
        public string CurrentEventLink()
        {
            if (m_events[m_courrentEvent] == null || m_events[m_courrentEvent].currentaction.Trim() == "")
                return "";
            else
                return m_events[m_courrentEvent].currentaction + @"/?wfe_id=" + m_entityID;
        }

	}//end CWorkFlow

}//end namespace FlowEngine
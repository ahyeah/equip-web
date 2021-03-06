﻿using FlowDesigner.ConfigItems;
using FlowDesigner.customcontrol;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml;

namespace FlowDesigner.Management
{
    public class param
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Desc { get; set; }
    }
    /// <summary>
    /// 工作流管理
    /// </summary>
    public class WorkFlowMan
    {
        public WorkFlowMan(string name, string desc)
        {
            Name = name;
            Description = desc;
            designer = new DesignerPage();
            designer.Title = Name;
            designer.parent_man = this;
            designer.LinkClick(this.OnCanvasClick);
            designer.LinkMouseMove(this.OnCanvasMouseMove);
            
            //PerAddNEvent = false;
            perAddNFlow = 0;
            params_table = new ObservableCollection<param>();
            
            designer.ParamsList.ItemsSource = params_table;
            
        }

        #region private property
        /// <summary>
        /// 工作流设计界面
        /// </summary>
        private DesignerPage designer;
        /// <summary>
        /// 工作流设计界面插入到父界面元素
        /// </summary>
        private TabControl parent;
        

        /// <summary>
        /// 工作流的定义元素
        /// </summary>
        private List<ConfigItem> wf_itmes = new List<ConfigItem>();
        private int for_events = 1;
        private bool m_perAddNEvent = false;

        private ConfigItem Curr_Selected = null;


        public void InitStartAndEnd()
        {
            //start
            ConfigEvent newSE = new ConfigEvent("StartEvent");
            newSE.name = "Start";
            newSE.description = "Start";
            newSE.OnSelected += this.Proj.win_main.OnConfigItem_Selected;
            newSE.SetPos(50, 50);
            wf_itmes.Add(newSE);
            this.designer.Client.Children.Add(newSE.GetShowItem());

            //end
            ConfigEvent newEE = new ConfigEvent("EndEvent");
            newEE.name = "End";
            newEE.description = "End";
            newEE.OnSelected += this.Proj.win_main.OnConfigItem_Selected;
            newEE.SetPos(50, 150);
            wf_itmes.Add(newEE);
            this.designer.Client.Children.Add(newEE.GetShowItem());
        }
        #endregion

        #region public property
        /// <summary>
        /// 工作流名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 工作流描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 变量表
        /// </summary>
        public ObservableCollection<param> params_table { get; set; }

        public string NewEventType { get; set; }
        public bool PerAddNEvent 
        { 
            get
            {
                return m_perAddNEvent;
            }
            set
            {
                m_perAddNEvent = value;
                if (m_perAddNEvent == false)
                {
                    this.Proj.win_main.EndAddNEvent();
                }
            } 
        }

        public int perAddNFlow { get; set; }

        public WorkFlowsProj Proj { get; set; }

        //新添加的flow
        private ConfigFlow ForAdd = null;
        public ConfigItem SelectedItem { 
            get {
                return Curr_Selected;
            }
            set {
                
                foreach(ConfigItem ce in wf_itmes)
                {
                    if (ce != value)
                        ce.UnSelect();
                    else
                        ce.Selected();
                }

                if (perAddNFlow == 1) //为flow设置起始Event
                {
                    if (value.GetType().Name == "ConfigEvent"
                        || value.GetType().Name == "ConfigSubEvent"
                        || value.GetType().Name == "ConfigLoopEvent")
                    {
                        //创建新flow
                        ForAdd = new ConfigFlow();
                        ForAdd.Start = ((ConfigEvent)value);
                        designer.Client.Children.Add(ForAdd.GetShowItem());
                        perAddNFlow = 2;
                    }

                }
                else if (perAddNFlow == 2) //为flow设置
                {
                    //暂时不允许flow的start, end相等
                    if ((value.GetType().Name == "ConfigEvent"
                        || value.GetType().Name == "ConfigSubEvent"
                        || value.GetType().Name == "ConfigLoopEvent") 
                        && value != Curr_Selected)
                    {                                             
                        ForAdd.End = ((ConfigEvent)value);
                        ForAdd.Connect();
                        wf_itmes.Add(ForAdd);
                        ((ConfigEvent)ForAdd.Start).AttachFlows(ForAdd);
                        ((ConfigEvent)ForAdd.End).AttachFlows(ForAdd);
                        ForAdd.OnSelected += this.Proj.win_main.OnConfigItem_Selected;
                        ForAdd.OnRightButtonUp += ((MainWindow)Application.Current.MainWindow).pop_itemmenu;
                        perAddNFlow = 0;
                        Proj.win_main.AddNewFlow.IsChecked = false;
                    }
                }
                Curr_Selected = value;
            }
        }

        public List<ConfigEvent> GetAllEvents()
        {
            List<ConfigEvent> events = new List<ConfigEvent>();
            
            foreach(ConfigItem ci in wf_itmes)
            {
                if (ci.GetType().Name == "ConfigEvent" 
                    || ci.GetType().Name == "ConfigSubEvent")
                    events.Add((ConfigEvent)ci);
            }
            return events;
        }
        #endregion

        #region method
        /// <summary>
        /// 将设计界面插入到主界面中
        /// </summary>
        /// <param name="mainTab"></param>
        public void LinkToMainTab(TabControl mainTab )
        {
            mainTab.Items.Add(this.designer);
            mainTab.SelectedItem = this.designer;
            
            parent = mainTab;
        }

        public void UnLinkFromMainTab(TabControl mainTab)
        {
            if (mainTab.Items.Contains(this.designer))
                mainTab.Items.Remove(this.designer);
            parent = null;
        }

        public void DeleteParam(string paramName)
        {
            foreach(param pa in params_table)
            {
                if (pa.Name == paramName)
                {
                    params_table.Remove(pa);
                    break;
                }
            }

            foreach (ConfigItem ci in wf_itmes)
            {
                if (ci.GetType().Name == "ConfigEvent"
                    || ci.GetType().Name == "ConfigSubEvent")
                {
                    ((ConfigEvent)ci).LinkParams.Remove(paramName);
                }
            }

        }

        public void DeleteItem(ConfigItem deleted)
        {
            if (deleted.GetType().Name == "ConfigFlow")
            {
                this.designer.Client.Children.Remove(deleted.GetShowItem());
                (deleted as ConfigFlow).Start.DetachFlow((ConfigFlow)deleted);
                (deleted as ConfigFlow).End.DetachFlow((ConfigFlow)deleted);
                wf_itmes.Remove(deleted);
            }
            else if (deleted.GetType().Name == "ConfigEvent" || deleted.GetType().Name == "ConfigSubEvent")
            {
                List<ConfigFlow> flows = deleted.GetAttachFolws();
                foreach (ConfigFlow cf in flows)
                {
                    this.designer.Client.Children.Remove(cf.GetShowItem());                    
                    wf_itmes.Remove(deleted);
                }
                flows.Clear();
                this.designer.Client.Children.Remove(deleted.GetShowItem());
                wf_itmes.Remove(deleted);
            }
        }

        /// <summary>
        /// 添加一个事件
        /// </summary>
        /// <param name="eType"></param>
        public ConfigEvent AddNewEvent(string eType)
        {
            ConfigEvent newE = null;
            switch(eType)
            {
                case "NormalEvent":
                    newE = new ConfigEvent(eType);
                    break;

                case "SubProcess":
                    newE = new ConfigSubEvent(eType);
                    break;

                case "StartEvent":
                    newE = new ConfigEvent(eType);
                    break;

                case "EndEvent":
                    newE = new ConfigEvent(eType);
                    break;

                case "LoopEvent":
                    newE = new ConfigLoopEvent(eType);
                    break;

                default:
                    newE =  null;
                    break;
            }
            if (newE == null)
                return null;

            newE.name = "event" + for_events.ToString();
            newE.description = "event" + for_events.ToString();
            newE.OnSelected += this.Proj.win_main.OnConfigItem_Selected;
            if (eType != "StartEvent" && eType != "EndEvent")
                newE.OnRightButtonUp += ((MainWindow)Application.Current.MainWindow).pop_itemmenu;
            wf_itmes.Add(newE);
            for_events += 1;
            return newE;
        }

        /// <summary>
        /// 左边树形菜单有元素被选择(双击)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnSelected(object sender, MouseButtonEventArgs e)
        {
            if ((TabControl)designer.Parent == null)
                parent.Items.Add(this.designer);

            ((TabControl)designer.Parent).SelectedItem = this.designer;
            Proj.CurrentWF = this;
            Proj.win_main.OnConfigItem_Selected(SelectedItem);
        }

        public void UnlinkOtherWFM(WorkFlowMan wfm)
        {
            if (wfm == null)
                return;

            foreach (ConfigItem ci in wf_itmes)
            {
                if (ci.GetType().Name == "ConfigSubEvent")
                {
                    if (((ConfigSubEvent)ci).LinkWorkFLow == wfm.Name)
                    {
                        ((ConfigSubEvent)ci).LinkWorkFLow = "";
                        ((ConfigSubEvent)ci).LinkParams.Clear();
                    }
                }
            }
        }

        public void OnCanvasClick(object sender, MouseButtonEventArgs e)
        {
            if (PerAddNEvent == true)
            {
                ConfigEvent ne = AddNewEvent(NewEventType);                
                ne.SetPos((int)e.GetPosition((Canvas)sender).X - 75, (int)e.GetPosition((Canvas)sender).Y - 35);
                designer.Client.Children.Add(ne.GetShowItem());
                PerAddNEvent = false;
                Proj.win_main.AddNEvent.IsChecked = false;
                Proj.win_main.AddSEvent.IsChecked = false;
            }
        }

        public void OnCanvasMouseMove(object sender, MouseEventArgs e)
        {
            if (perAddNFlow == 2) //已设置好起点
            {
                ForAdd.DynamicDraw(e.GetPosition((Canvas)sender));
            }
        }

        public XmlElement SaveWorkFlow(XmlDocument Owner)
        {
            XmlElement wf_xml = Owner.CreateElement("work_flow");
            wf_xml.SetAttribute("name", Name);
            wf_xml.SetAttribute("desc", Description);

            //
            XmlElement eId = Owner.CreateElement("for_events");
            eId.SetAttribute("value", for_events.ToString());
            wf_xml.AppendChild(eId);

            //params
            XmlElement pas_xml = Owner.CreateElement("params");
            foreach(param pa in params_table)
            {
                XmlElement pa_xml = Owner.CreateElement("param");
                pa_xml.SetAttribute("name", pa.Name);
                pa_xml.SetAttribute("type", pa.Type);
                pa_xml.SetAttribute("desc", pa.Desc);
                pas_xml.AppendChild(pa_xml);
            }
            wf_xml.AppendChild(pas_xml);

            //ConfigItem
            XmlElement items_xml = Owner.CreateElement("configitems");
            foreach (ConfigItem ci in wf_itmes)
            {
                XmlElement item_xml = ci.SaveConfigItem(Owner);
                items_xml.AppendChild(item_xml);
            }
            wf_xml.AppendChild(items_xml);
            return wf_xml;
        }

        

        public bool LoadWorkFlow(XmlElement source)
        {
            //params
            XmlElement pars_xml = (XmlElement)source.SelectSingleNode("params");
            XmlNodeList parss = pars_xml.SelectNodes("param");
            foreach(XmlNode xn in parss)
            {
                XmlElement xe = (XmlElement)xn;
                bool flag = false;

                foreach(param pa in params_table)
                {
                    if (pa.Name == xe.GetAttribute("name"))
                    {
                        flag = true;
                        break;
                    }
                }
                if (flag == false)
                {
                    params_table.Add(new param()
                    {
                        Name = xe.GetAttribute("name"),
                        Type = xe.GetAttribute("type"),
                        Desc = xe.GetAttribute("desc")
                    });
                }
            }

            //ConfigItem
            XmlElement items_xml = (XmlElement)source.SelectSingleNode("configitems");
            foreach (XmlNode xn1 in items_xml.SelectNodes("item"))
            {
                XmlElement xe1 = (XmlElement)xn1;
                if (xe1.GetAttribute("item_type") == "event")
                {
                    ConfigEvent ce = AddNewEvent(xe1.GetAttribute("event_type"));
                    ce.authority = xe1.GetAttribute("authority");
                    ce.GetShowItem().Margin = new Thickness(
                        Convert.ToDouble(((XmlElement)xe1.SelectSingleNode("shape")).GetAttribute("margin").Split(',')[0]),
                        Convert.ToDouble(((XmlElement)xe1.SelectSingleNode("shape")).GetAttribute("margin").Split(',')[1]),
                        Convert.ToDouble(((XmlElement)xe1.SelectSingleNode("shape")).GetAttribute("margin").Split(',')[1]),
                        Convert.ToDouble(((XmlElement)xe1.SelectSingleNode("shape")).GetAttribute("margin").Split(',')[1]));
                    ce.CurrentAction = ((XmlElement)xe1.SelectSingleNode("actions")).GetAttribute("currentaction");

                    XmlElement ba = (XmlElement)(xe1.SelectSingleNode("actions").SelectSingleNode("beforeaction"));
                    XmlElement aa = (XmlElement)(xe1.SelectSingleNode("actions").SelectSingleNode("afteraction"));
                    if (ba != null) //新版本
                    {
                        ce.BeforeAction.url = ba.GetAttribute("url");
                        XmlNodeList act_pars = ba.SelectSingleNode("action_params").SelectNodes("param");
                        ce.BeforeAction.action_params.Clear();
                        foreach (XmlNode parxn in act_pars)
                        {
                            ce.BeforeAction.action_params[((XmlElement)parxn).GetAttribute("name")] = ((XmlElement)parxn).InnerText;
                        }


                        ce.AfterAction.url = aa.GetAttribute("url");
                        XmlNodeList act_pars1 = aa.SelectSingleNode("action_params").SelectNodes("param");
                        ce.AfterAction.action_params.Clear();
                        foreach (XmlNode parxn in act_pars1)
                        {
                            ce.AfterAction.action_params[((XmlElement)parxn).GetAttribute("name")] = ((XmlElement)parxn).InnerText;
                        }
                    }
                    else
                    {
                        ce.BeforeAction.url = ((XmlElement)xe1.SelectSingleNode("actions")).GetAttribute("beforeaction");
                        ce.AfterAction.url = ((XmlElement)xe1.SelectSingleNode("actions")).GetAttribute("afteraction");
                    }

                    XmlNodeList parsss = xn1.SelectSingleNode("link_params").SelectNodes("param");
                    foreach(XmlNode xn2 in parsss)
                    {
                        ce.LinkParams.Add(((XmlElement)xn2).GetAttribute("name"), ((XmlElement)xn2).GetAttribute("app_reserve"));
                    }
                    ce.name = xe1.GetAttribute("name");
                    ce.description = xe1.GetAttribute("desc");

                    //time_out
                    XmlNode time_out = xn1.SelectSingleNode("time_out");
                    if (time_out != null)
                    {
                        Dictionary<string, object> timeout_setting = (ce.Time_Out as Dictionary<string, object>);
                        XmlNode time_start = time_out.SelectSingleNode("start");
                        if (time_start == null)
                            timeout_setting["start"] = "";
                        else
                            timeout_setting["start"] = time_start.InnerText;

                        XmlNode time_offset = time_out.SelectSingleNode("offset");
                        if (time_offset == null || time_offset.InnerText == "")
                            timeout_setting["offset"] = (null as TimeSpan?);
                        else
                        {
                            TimeSpan? ts = TimeSpan.Parse(time_offset.InnerText);
                            timeout_setting["offset"] = ts;
                        }

                        XmlNode time_exact = time_out.SelectSingleNode("exact");
                        if (time_exact == null || time_exact.InnerText == "")
                            timeout_setting["exact"] = (null as DateTime?);
                        else
                        {
                            DateTime? dt = DateTime.Parse(time_exact.InnerText);
                            timeout_setting["exact"] = dt;
                        }

                        XmlNode time_action = time_out.SelectSingleNode("action");
                        if (time_action == null)
                            timeout_setting["action"] = "";
                        else
                            timeout_setting["action"] = time_action.InnerText;

                        XmlNode time_callback = time_out.SelectSingleNode("callback");
                        if (time_callback == null)
                            timeout_setting["callback"] = "";
                        else
                            timeout_setting["callback"] = time_callback.InnerText;

                    }
                    
                    if (ce.GetType().Name == "ConfigSubEvent")
                    {
                        ((ConfigSubEvent)ce).SetWorkModel(((XmlElement)xe1.SelectSingleNode("LinkWorkflow")).GetAttribute("model"));
                        ((ConfigSubEvent)ce).LinkWorkFLow = ((XmlElement)xe1.SelectSingleNode("LinkWorkflow")).GetAttribute("name");

                        XmlNodeList param_transfer = ((XmlElement)xe1.SelectSingleNode("LinkWorkflow")).SelectNodes("ParamTransfer");
                        foreach(XmlNode pt in param_transfer)
                        {
                            ((ConfigSubEvent)ce).ParamTransfer.Add(new paramtransfer_item()
                            {
                                parent = ((XmlElement)pt).GetAttribute("source"),
                                child = ((XmlElement)pt).GetAttribute("dest"),
                                direction = ((XmlElement)pt).GetAttribute("direction") == "from" ? transfer_direction.from : transfer_direction.to
                            });
                        }
                    }

                    if (ce.GetType().Name == "ConfigLoopEvent")
                    {
                        ((ConfigLoopEvent)ce).LoopCondition = (xe1.SelectSingleNode("LoopSetting").SelectSingleNode("condition") as XmlElement).InnerText;
                        ((ConfigLoopEvent)ce).TimeWaiting = (xe1.SelectSingleNode("LoopSetting").SelectSingleNode("waiting_time") as XmlElement).InnerText;
                    }

                    designer.Client.Children.Add(ce.GetShowItem());
                }
            }

            //flow
            foreach (XmlNode xn3 in items_xml.SelectNodes("item"))
            {
                XmlElement xe2 = (XmlElement)xn3;
                if (xe2.GetAttribute("item_type") == "flow")
                {
                    ConfigFlow cf = new ConfigFlow();
                    foreach(ConfigItem ci in wf_itmes)
                    {
                        if ((ci.GetType().Name == "ConfigEvent" || ci.GetType().Name == "ConfigSubEvent" || ci.GetType().Name == "ConfigLoopEvent") 
                            && ci.name == ((XmlElement)xe2.SelectSingleNode("shape")).GetAttribute("source"))
                        {
                            cf.Start = (ConfigEvent)ci;
                            ((ConfigEvent)ci).AttachFlows(cf);
                        }
                        if ((ci.GetType().Name == "ConfigEvent" || ci.GetType().Name == "ConfigSubEvent" || ci.GetType().Name == "ConfigLoopEvent")
                            && ci.name == ((XmlElement)xe2.SelectSingleNode("shape")).GetAttribute("dest"))
                        { 
                            cf.End = (ConfigEvent)ci;
                            ((ConfigEvent)ci).AttachFlows(cf);
                        }
                    }
                    cf.Express = xe2.GetAttribute("condition");
                    cf.OnSelected += this.Proj.win_main.OnConfigItem_Selected;
                    cf.OnRightButtonUp += ((MainWindow)Application.Current.MainWindow).pop_itemmenu;
                    designer.Client.Children.Add(cf.GetShowItem());
                    cf.Connect();
                    wf_itmes.Add(cf);
                }
            }
            for_events = Convert.ToInt32(((XmlElement)source.SelectSingleNode("for_events")).GetAttribute("value"));
            return true;
        }

        private XmlElement ComplieParamTable(XmlDocument Owner)
        {
            XmlElement root = Owner.CreateElement("paramtable");

            foreach (param pa in params_table)
            {
                XmlElement pa_xml = Owner.CreateElement("param");
                pa_xml.SetAttribute("name", pa.Name);
                pa_xml.SetAttribute("desc", pa.Desc);
                pa_xml.SetAttribute("type", pa.Type);

                //linkevents
                XmlElement les = Owner.CreateElement("linkevents");
                foreach(ConfigItem ci in wf_itmes)
                {
                    if (ci.GetType().Name != "ConfigEvent"
                        && ci.GetType().Name != "ConfigSubEvent"
                        && ci.GetType().Name != "ConfigLoopEvent")
                        continue;

                    if (((ConfigEvent)ci).LinkParams.ContainsKey(pa.Name))
                    {
                        XmlElement le = Owner.CreateElement("linkevent");
                        le.SetAttribute("app_reserve", ((ConfigEvent)ci).LinkParams[pa.Name]);
                        le.InnerText = ci.name;
                        les.AppendChild(le);
                    }
                    
                }
                pa_xml.AppendChild(les);
                root.AppendChild(pa_xml);
            }


            return root;
        }

        private XmlElement CompleLinkWorkFlowForSubEvent(XmlDocument Owner, ConfigSubEvent se)
        {
            XmlElement link = Owner.CreateElement("linkworkflow");
            link.SetAttribute("name", se.LinkWorkFLow);
            link.SetAttribute("workingmodel", se.WorkingModel.ToString());

            XmlElement pt = Owner.CreateElement("paramtransfer");
            foreach(paramtransfer_item pi in se.ParamTransfer)
            {
                XmlElement tr = Owner.CreateElement("item");
                tr.SetAttribute("parent", pi.parent);
                tr.SetAttribute("child", pi.child);
                tr.SetAttribute("direction", pi.direction.ToString());
                pt.AppendChild(tr);
            }
            link.AppendChild(pt);

            return link;

        }

        private XmlElement CompleEvents(XmlDocument Owner)
        {
            XmlElement events = Owner.CreateElement("events");
            foreach (ConfigItem ci in wf_itmes)
            {
                if (ci.GetType().Name != "ConfigEvent"
                    && ci.GetType().Name != "ConfigSubEvent"
                    && ci.GetType().Name != "ConfigLoopEvent")
                    continue;

                XmlElement ev = Owner.CreateElement("event");
                ev.SetAttribute("name", ci.name);
                ev.SetAttribute("desc", ci.description);
                ev.SetAttribute("type", ((ConfigEvent)ci).GetEventType().ToLower());

                XmlElement actions = Owner.CreateElement("actions");

                XmlElement ba = Owner.CreateElement("beforeaction");
                XmlElement baurl = Owner.CreateElement("url");
                baurl.AppendChild(Owner.CreateCDataSection(((ConfigEvent)ci).BeforeAction.url));
                ba.AppendChild(baurl);
                XmlElement ba_params = Owner.CreateElement("action_params");
                foreach(var pp in ((ConfigEvent)ci).BeforeAction.action_params)
                {
                    XmlElement pe = Owner.CreateElement("param");
                    pe.SetAttribute("name", pp.Key);
                    pe.InnerText = pp.Value;
                    ba_params.AppendChild(pe);
                }
                ba.AppendChild(ba_params);
                actions.AppendChild(ba);

                XmlElement ca = Owner.CreateElement("currentaction");
                ca.AppendChild(Owner.CreateCDataSection(((ConfigEvent)ci).CurrentAction));
                actions.AppendChild(ca);

                XmlElement aa = Owner.CreateElement("afteraction");
                XmlElement aaurl = Owner.CreateElement("url");
                aaurl.AppendChild(Owner.CreateCDataSection(((ConfigEvent)ci).AfterAction.url));
                aa.AppendChild(aaurl);
                XmlElement aa_params = Owner.CreateElement("action_params");
                foreach (var pp in ((ConfigEvent)ci).AfterAction.action_params)
                {
                    XmlElement pe = Owner.CreateElement("param");
                    pe.SetAttribute("name", pp.Key);
                    pe.InnerText = pp.Value;
                    aa_params.AppendChild(pe);
                }
                aa.AppendChild(aa_params);
                actions.AppendChild(aa);

                ev.AppendChild(actions);

                XmlElement auts = Owner.CreateElement("authority");
                auts.AppendChild(Owner.CreateCDataSection(((ConfigEvent)ci).authority));
                ev.AppendChild(auts);

                //构建timeout
                XmlElement toNode = Owner.CreateElement("timeout_setting");
                Dictionary<string, object> time_out = (((ConfigEvent)ci).Time_Out as Dictionary<string, object>);
                toNode.SetAttribute("action", time_out.ContainsKey("action") ? time_out["action"] as string : "");

                //1. 构建具体时间节点
                if ((time_out["exact"] as DateTime?) != null)
                {
                    XmlElement et = Owner.CreateElement("exact_time");
                    et.InnerText = time_out.ContainsKey("exact") ? (time_out["exact"] as DateTime?).Value.ToString() : "";
                    toNode.AppendChild(et);
                }

                //2. 构建偏移时间节点
                if ((time_out["start"] as string) != "" && (time_out["offset"] as TimeSpan?) != null)
                {
                    XmlElement ot = Owner.CreateElement("offset_time");
                    string time_start = time_out.ContainsKey("start") ? (time_out["start"] as string) : "";
                    
                    ot.SetAttribute("time_start", time_start);

                    ot.SetAttribute("time_offset", time_out.ContainsKey("offset") ? (time_out["offset"] as TimeSpan?).Value.ToString() : "");
                    toNode.AppendChild(ot);
                }

                //3. 构建callback节点
                XmlElement cb = Owner.CreateElement("call_back");
                cb.AppendChild(Owner.CreateCDataSection(time_out.ContainsKey("callback") ? (time_out["callback"] as string) : ""));
                toNode.AppendChild(cb);
                ev.AppendChild(toNode);

                if (ci.GetType().Name == "ConfigSubEvent")
                    ev.AppendChild(CompleLinkWorkFlowForSubEvent(Owner, (ConfigSubEvent)ci));

                if (ci.GetType().Name == "ConfigLoopEvent")
                {
                    XmlElement con_elem = Owner.CreateElement("loop_contidion");
                    con_elem.AppendChild(Owner.CreateCDataSection((ci as ConfigLoopEvent).LoopCondition));
                    ev.AppendChild(con_elem);

                    
                    XmlElement looptime = Owner.CreateElement("loop_time");
                    looptime.AppendChild(Owner.CreateCDataSection((ci as ConfigLoopEvent).TimeWaiting));
                    ev.AppendChild(looptime);
                }
                events.AppendChild(ev);
            }
            return events;
        }

        private XmlElement CompleFlows(XmlDocument Owner)
        {
            XmlElement flows = Owner.CreateElement("flows");
            foreach (ConfigItem ci in wf_itmes)
            {
                if (ci.GetType().Name != "ConfigFlow")
                    continue;

                XmlElement flow = Owner.CreateElement("flow");
                flow.SetAttribute("source", ((ConfigFlow)ci).StartEvent);
                flow.SetAttribute("destination", ((ConfigFlow)ci).EndEvent);

                XmlElement condition = Owner.CreateElement("condition");
                condition.AppendChild(Owner.CreateCDataSection(((ConfigFlow)ci).Express));
                flow.AppendChild(condition);

                flows.AppendChild(flow);
            }
            return flows;
        }
        public bool Complie(string filePath)
        {
            XmlDocument doc = new XmlDocument();
            XmlElement root = doc.CreateElement("workflow");

            //1. set workflow attributions 
            root.SetAttribute("name", Name);
            root.SetAttribute("desc", Description);
            root.SetAttribute("current_event", "");

            //2. comple paramtable
            XmlElement paramtable = ComplieParamTable(doc);
            root.AppendChild(paramtable);

            //3. comple events
            XmlElement events = CompleEvents(doc);
            root.AppendChild(events);

            //4. comple flows
            XmlElement flows = CompleFlows(doc);
            root.AppendChild(flows);

            //5. comple records
            XmlElement records = doc.CreateElement("recorditems");
            foreach(RecordItem ri in Proj.Record_Items)
            {
                XmlElement item = doc.CreateElement("item");
                item.InnerText = ri.Name;
                records.AppendChild(item);
            }
            root.AppendChild(records);

            doc.AppendChild(root);
            doc.Save(filePath);
            return true;
        }
        #endregion
    }
}

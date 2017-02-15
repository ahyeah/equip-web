using EquipModel.Entities;
using FlowEngine;
using FlowEngine.Modals;
using FlowEngine.UserInterface;
using WebApp.Models;
using WebApp.Models.User;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using EquipModel.Context;
using System.Text;
using EquipBLL.AdminManagment;
using System.IO;
using System.Xml;
using FlowEngine.Event;
using FlowEngine.DAL;
using FlowEngine.Param;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;

namespace WebApp.Controllers
{
    public class A13dot1Controller : CommonController
    {
        
       
        public class MissHisLinkModal
        {
            public int Miss_Id;
            public string Miss_Name;
            public string Miss_Time;
            public int Miss_Type;  //0：一般任务  1：子流程任务
            public string Miss_LinkFlowType;//"Normal" ，"paradel","Serial"
            public int Miss_LinkFlowId;//跳转的工作流Id
        }

        public class ListMissHisLInkModal
        {
            public int Flow_Id;
            public string Flow_Name;
            public List<MissHisLinkModal> Miss;
        }
        public ActionResult A13()
        {
            NoticeManagement NM = new NoticeManagement();
            List<Notice_A13dot1> list = NM.getNoticeForA13dot1Uncomp();
            ViewBag.NoticeCount = list.Count();
            return View();
        }
        public ActionResult Index1(string wfe_id)
        {
            
            return View(getSubmitModel(wfe_id));
        }
        //
        // GET: /A13dot1/
        public ActionResult Index()
        {
            return View(getIndexListRecords("A13dot1", (Session["User"] as EquipModel.Entities.Person_Info).Person_Name));            
        }
        // GET: /A13dot1/装置提报
        public ActionResult ZzSubmit(string wfe_id)
        {
            return View(getSubmitModel(wfe_id));
        }

        // GET: /A13dot1/可靠性工程师分类
        public ActionResult PqCatalog(string  wfe_id)
        {         
            return View(getWFDetail_Model(wfe_id));
        }
               
        // GET: /A13dot1/专业团队审核
        public ActionResult ZytdConfirm(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }
        
        public ActionResult WorkFolw_Detail(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }

        public ActionResult WorkFlow_HisDetailParallel(int Entity_Id)
        {

            List<UI_MISSION> miss;
            ListMissHisLInkModal missModals = new ListMissHisLInkModal();
            missModals.Miss = new List<MissHisLinkModal>();
            UI_WFEntity_Info wfe = CWFEngine.GetWorkFlowEntiy(Entity_Id, true);
            missModals.Flow_Id = Entity_Id;
            missModals.Flow_Name = wfe.name;
            miss = CWFEngine.GetHistoryMissions(wfe.EntityID);
            if (wfe.Status == WE_STATUS.ACTIVE)
            {
                miss.Add(CWFEngine.GetActiveMission<Person_Info>(wfe.EntityID, null, false));
            }
            WorkFlows wfs = new WorkFlows();
            WorkFlow_Entity wfe2 = wfs.GetWorkFlowEntity(wfe.EntityID);
            CWorkFlow wf = new CWorkFlow();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(Encoding.Default.GetString(wfe2.WE_Binary));
            wf.InstFromXmlNode(doc.DocumentElement);
            wf.EntityID = wfe.EntityID;
            IDictionary<string, IEvent> allEvents = wf.events;
            foreach (var item in miss)
            {

                MissHisLinkModal m = new MissHisLinkModal();
                m.Miss_Id = item.Miss_Id;
                m.Miss_Name = item.WE_Event_Desc;
                if (item.Miss_Id > 0)
                {
                    IDictionary<string, string> r = CWFEngine.GetMissionRecordInfo(item.Miss_Id);
                    if (r.Count > 0)
                    {
                        m.Miss_Time = r["time"];
                    }
                    else
                    {
                        m.Miss_Time = "";
                    }
                }
                else
                {
                    m.Miss_Time = "";
                }
                if (allEvents[item.WE_Event_Name].GetType().Name == "CSubProcessEvent")
                {
                    m.Miss_Type = 1;
                    m.Miss_LinkFlowType = ((CSubProcessEvent)allEvents[item.WE_Event_Name]).WorkingMode;
                    m.Miss_LinkFlowId = ((CSubProcessEvent)allEvents[item.WE_Event_Name]).WfEntityId;
                }
                else
                {
                    m.Miss_Type = 0;
                    m.Miss_LinkFlowType = "Normal";
                    m.Miss_LinkFlowId = -1;
                }

                missModals.Miss.Add(m);
            }
            return View(missModals);
        }
        public ActionResult WorkFlow_HisDetail()
        {
          
             List<UI_MISSION> miss;
             ListMissHisLInkModal missModals = new ListMissHisLInkModal();
             missModals.Miss = new List<MissHisLinkModal>();
            UI_WFEntity_Info wfe=CWFEngine.GetMainWorkFlowEntity("20160200010");
            missModals.Flow_Id = wfe.EntityID;
            missModals.Flow_Name = wfe.name;
            miss = CWFEngine.GetHistoryMissions(wfe.EntityID);
            if (wfe.Status == WE_STATUS.ACTIVE)
            {
                miss.Add(CWFEngine.GetActiveMission<Person_Info>(wfe.EntityID, null, false));
            }
                WorkFlows wfs = new WorkFlows();
                WorkFlow_Entity wfe2 = wfs.GetWorkFlowEntity(wfe.EntityID);
                CWorkFlow wf = new CWorkFlow();
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(Encoding.Default.GetString(wfe2.WE_Binary));
                wf.InstFromXmlNode(doc.DocumentElement);
                wf.EntityID = wfe.EntityID;
                IDictionary<string,IEvent> allEvents=wf.events;
            foreach (var item in miss)
            {
               
                MissHisLinkModal m = new MissHisLinkModal();
                m.Miss_Id = item.Miss_Id;
                m.Miss_Name = item.WE_Event_Desc;
                if (item.Miss_Id>0)
                { IDictionary<string, string> r = CWFEngine.GetMissionRecordInfo(item.Miss_Id);
                if (r.Count > 0  )
                {
                    m.Miss_Time = r["time"];
                }
                else
                {
                  m.Miss_Time = "";
               }
                }
                else{
                    m.Miss_Time = "";
                }
                if(allEvents[item.WE_Event_Name].GetType().Name=="CSubProcessEvent")
                {
                    m.Miss_Type = 1;
                    m.Miss_LinkFlowType = ((CSubProcessEvent)allEvents[item.WE_Event_Name]).WorkingMode;
                    m.Miss_LinkFlowId = ((CSubProcessEvent)allEvents[item.WE_Event_Name]).WfEntityId;
                }else
                { m.Miss_Type = 0;
                  m.Miss_LinkFlowType = "Normal";
                  m.Miss_LinkFlowId = -1;
                }

                missModals.Miss.Add(m);
            }
          
            return View(missModals);
        }
    
        public JsonResult WorkFlow_SubProcess( int FlowId)
        {
            List<UI_MISSION> miss;
            ListMissHisLInkModal missModals = new ListMissHisLInkModal();
            missModals.Miss = new List<MissHisLinkModal>();
            UI_WFEntity_Info wfe = CWFEngine.GetWorkFlowEntiy(FlowId,true);
            missModals.Flow_Id = FlowId;
            missModals.Flow_Name = wfe.name;
            miss = CWFEngine.GetHistoryMissions(wfe.EntityID);
            if (wfe.Status == WE_STATUS.ACTIVE)
            {
                miss.Add(CWFEngine.GetActiveMission<Person_Info>(wfe.EntityID, null, false));
            }
            WorkFlows wfs = new WorkFlows();
            WorkFlow_Entity wfe2 = wfs.GetWorkFlowEntity(wfe.EntityID);
            CWorkFlow wf = new CWorkFlow();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(Encoding.Default.GetString(wfe2.WE_Binary));
            wf.InstFromXmlNode(doc.DocumentElement);
            wf.EntityID = wfe.EntityID;
            IDictionary<string, IEvent> allEvents = wf.events;
            foreach (var item in miss)
            {

                MissHisLinkModal m = new MissHisLinkModal();
                m.Miss_Id = item.Miss_Id;
                m.Miss_Name = item.WE_Event_Desc;
                if (item.Miss_Id > 0)
                {
                    IDictionary<string, string> r = CWFEngine.GetMissionRecordInfo(item.Miss_Id);
                    if (r.Count > 0)
                    {
                        m.Miss_Time = r["time"];
                    }
                    else
                    {
                        m.Miss_Time = "";
                    }
                }
                else
                {
                    m.Miss_Time = "";
                }
                if (allEvents[item.WE_Event_Name].GetType().Name == "CSubProcessEvent")
                {
                    m.Miss_Type = 1;
                    m.Miss_LinkFlowType = ((CSubProcessEvent)allEvents[item.WE_Event_Name]).WorkingMode;
                    m.Miss_LinkFlowId = ((CSubProcessEvent)allEvents[item.WE_Event_Name]).WfEntityId;
                }
                else
                {
                    m.Miss_Type = 0;
                    m.Miss_LinkFlowType = "Normal";
                    m.Miss_LinkFlowId = -1;
                }

                missModals.Miss.Add(m);
            }
            return Json(missModals);

        }

        public JsonResult WorkFlow_ListParam(string json1)
        {  
            JObject item = (JObject)JsonConvert.DeserializeObject(json1);

           int Flow_Id = Convert.ToInt32(item["Entity_Id"].ToString());
           int Mission_Id = Convert.ToInt32(item["Mission_Id"].ToString());
          List<UI_MissParam>  MissParams = CWFEngine.GetMissionParams(Flow_Id,Mission_Id);
           return Json(MissParams.ToArray());


        }
        public string ZzSubmit_submitsignal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string flowname = item["Flow_Name"].ToString();
                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();
                signal["ZzSubmit_done"] = "true"; 
                signal["Cj_Name"] = item["Cj_Name"].ToString();
                signal["Zz_Name"] = item["Zz_Name"].ToString();              
                signal["Equip_GyCode"] = item["Equip_GyCode"].ToString();
                signal["Equip_Code"] = item["Equip_Code"].ToString();
                signal["Equip_Type"] = item["Equip_Type"].ToString();
                signal["Problem_Desc"] = item["Problem_Desc"].ToString();  
                signal["Problem_DescFilePath"] = item["Problem_DescFilePath"].ToString();
                signal["Zy_Type"] = item["Zy_Type"].ToString();
                signal["Zy_SubType"] = item["Zy_SubType"].ToString();
                EquipManagment em = new EquipManagment();
                signal["Equip_ABCMark"] = em.getEquip_Info(item["Equip_Code"].ToString()).Equip_ABCmark;
                //signal["Equip_ABCMark"] = "A";//for test               
                signal["Data_Src"] ="人工提报"; //格式统一
                //record
                Dictionary<string, string> record =  new Dictionary<string, string>();
                record["username"] = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
                record["time"] = DateTime.Now.ToString();
                //submit
                CWFEngine.SubmitSignal(Convert.ToInt32(flowname), signal,record);
            }
            catch (Exception e)
            {
                return "";
            }
            return ("/A13dot1/Index");
        }

        public string PqCatalog_submitsignal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                Dictionary<string, string> signal = new Dictionary<string, string>();
                var flowname = Convert.ToInt32(item["Flow_Name"].ToString());
                signal["catlog_type"] = item["catlog_type"].ToString();

                if (item["catlog_type"].ToString() == "完好类" )
                {                   
                    string[] notgood = item["Incomplete_content"].ToString().Split('|');
                    WorkFlows wfsd = new WorkFlows();
                    WorkFlows wfs = new WorkFlows();
                    Mission db_miss = wfsd.GetWFEntityMissions(flowname).Last();//获取该实体最后一个任务
                    WorkFlow_Entity wfe = wfsd.GetWorkFlowEntity(flowname);
                    UI_MISSION ui_mi = new UI_MISSION();
                    ui_mi.WE_Entity_Id = flowname;                   
                    ui_mi.WE_Event_Desc = db_miss.Miss_Desc;
                    ui_mi.WE_Event_Name = db_miss.Event_Name;
                    ui_mi.WE_Name = db_miss.Miss_Name;                   
                    ui_mi.Miss_Id = db_miss.Miss_Id;
                    List<Mission_Param> mis_pars = wfsd.GetMissParams(db_miss.Miss_Id);//获取当前任务参数
                    foreach (var par in mis_pars)
                    {
                        CParam cp = new CParam();
                        cp.type = par.Param_Type;
                        cp.name = par.Param_Name;
                        cp.value = par.Param_Value;
                        cp.description = par.Param_Desc;
                        ui_mi.Miss_Params[cp.name] = cp.value;                        
                        ui_mi.Miss_ParamsDesc[cp.name] = cp.description;//xwm modified
                    }
                    List<Mission> gettime = wfs.GetWFEntityMissions(flowname);//获取实体的所有任务找到第二个任务获取提报时间
                    IDictionary<string, string> timeanduser = CWFEngine.GetMissionRecordInfo(gettime[1].Miss_Id);
                    if (timeanduser.Count > 0)
                    {
                        ui_mi.Mission_Url = timeanduser["username"];
                        ui_mi.WE_Entity_Ser = timeanduser["time"];
                    }
                    else
                    {
                        ui_mi.Mission_Url = "";
                        ui_mi.WE_Entity_Ser = "";
                    }
                    A5dot1Tab1 a5dot1Tab1 = new A5dot1Tab1();
                    DateTime my = DateTime.Now;
                    string yearmonth = "";
                    if (my.Day >= 15)
                    {
                        yearmonth = my.Year.ToString() + my.AddMonths(1).Month.ToString();
                    }
                    else
                    {
                        yearmonth = my.Year.ToString() + my.Month.ToString();
                    }
                   
                    if (notgood[notgood.Count() - 1] == "")
                    {
                        for (int i = 0; i < notgood.Count() - 1; i++)
                        {
                            a5dot1Tab1.cjName = ui_mi.Miss_Params["Cj_Name"].ToString();
                            a5dot1Tab1.zzName = ui_mi.Miss_Params["Zz_Name"].ToString();
                            a5dot1Tab1.sbGyCode = ui_mi.Miss_Params["Equip_GyCode"].ToString();
                            a5dot1Tab1.sbCode = ui_mi.Miss_Params["Equip_Code"].ToString();
                            a5dot1Tab1.sbType = ui_mi.Miss_Params["Equip_Type"].ToString();
                            a5dot1Tab1.zyType = ui_mi.Miss_Params["Zy_Type"].ToString();
                            a5dot1Tab1.notGoodContent = notgood[i];
                            a5dot1Tab1.isRectified = 0;
                            a5dot1Tab1.zzSubmitTime = Convert.ToDateTime(ui_mi.WE_Entity_Ser);
                            a5dot1Tab1.zzUserName = ui_mi.Mission_Url;
                            a5dot1Tab1.yearMonthForStatistic = yearmonth;
                            a5dot1Tab1.temp2 = Convert.ToString(flowname);
                            TablesManagment tm = new TablesManagment();
                            tm.Zzsubmit(a5dot1Tab1);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < notgood.Count(); i++)
                        {
                            a5dot1Tab1.cjName = ui_mi.Miss_Params["Cj_Name"].ToString();
                            a5dot1Tab1.zzName = ui_mi.Miss_Params["Zz_Name"].ToString();
                            a5dot1Tab1.sbGyCode = ui_mi.Miss_Params["Equip_GyCode"].ToString();
                            a5dot1Tab1.sbCode = ui_mi.Miss_Params["Equip_Code"].ToString();
                            a5dot1Tab1.sbType = ui_mi.Miss_Params["Equip_Type"].ToString();
                            a5dot1Tab1.zyType = ui_mi.Miss_Params["Zy_Type"].ToString();
                            a5dot1Tab1.notGoodContent = notgood[i];
                            a5dot1Tab1.isRectified = 0;
                            a5dot1Tab1.zzSubmitTime = Convert.ToDateTime(ui_mi.WE_Entity_Ser);
                            a5dot1Tab1.zzUserName = ui_mi.Mission_Url;
                            a5dot1Tab1.yearMonthForStatistic = yearmonth;
                            a5dot1Tab1.temp2 = Convert.ToString(flowname);
                            TablesManagment tm = new TablesManagment();
                            tm.Zzsubmit(a5dot1Tab1);
                        }
                    }
                }
                else if (item["catlog_type"].ToString() == "隐患")
                {
                    signal["defect_level"] = item["defect_level"].ToString();
                }
                else if (item["catlog_type"].ToString() == "故障")
                {
                    signal["defect_level"] = item["defect_level"].ToString();
                    signal["fault_intensity"] = item["fault_intensity"].ToString();
                }
                Dictionary<string, string> record = new Dictionary<string, string>();
                record["username"] = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
                record["time"] = DateTime.Now.ToString();
                CWFEngine.SubmitSignal(Convert.ToInt32(flowname), signal,record);
            }
            catch (Exception e)
            { return ""; }
            return ("/A13dot1/Index");
        }

        public string ZytdConfirm_submitsignal(string json1)
        {
            JObject item = (JObject)JsonConvert.DeserializeObject(json1);
            string flowname = item["Flow_Name"].ToString();
            string ser = CWFEngine.GetWorkFlowEntiy(Convert.ToInt32(flowname), false).serial;
            Dictionary<string, string> signal = new Dictionary<string, string>();
            //new input paras
            signal["fault_intensity"] = item["fault_intensity"].ToString();
            signal["ZytdConfirm_Result"] = "true";
            Dictionary<string, string> record = new Dictionary<string, string>();
            record["username"] = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
            record["time"] = DateTime.Now.ToString();
            CWFEngine.SubmitSignal(Convert.ToInt32(flowname), signal,record);
            //======================================2016.6.22================================================
            //DRBPM紧急检修计划 : writeDRBPM(string sbcode,string reason, bool isUrgent)
            string Equip_Code = item["Equip_Code"].ToString();
            writeDRBPM(Equip_Code, "一级缺陷", true,ser); // write to drbpm
            //======================================================================================
            return ("/A13dot1/Index");
        }

        public string defectmanagement(string json1)
        {
            List<object> r = new List<object>();//存放返回结果

            TablesManagment tm = new TablesManagment();
            List<UI_WFEntity_Info> miss_done;
            miss_done = CWFEngine.GetWFEntityByHistoryDone(t => t.Miss_WFentity.WE_Wref.W_Name == "A13dot1");
            List<UI_MISSION> his_missdone = new List<UI_MISSION>();//最后一步均为跳转，因此最后一步关联了所需的所有参数，可直接获得最后一步的任务参数即可
            WorkFlows wfsd = new WorkFlows();


            foreach (var item in miss_done)//获得entity_id
            {

                Mission db_miss = wfsd.GetWFEntityMissions(item.EntityID).Last();//获取该实体最后一个任务
                WorkFlow_Entity wfe = wfsd.GetWorkFlowEntity(item.EntityID);


                //CWorkFlow wf = new CWorkFlow();
                WorkFlows wfs = new WorkFlows();


                UI_MISSION ui_mi = new UI_MISSION();
                ui_mi.WE_Entity_Id = item.EntityID;
                ui_mi.WE_Entity_Ser = wfe.WE_Ser;
                ui_mi.WE_Event_Desc = db_miss.Miss_Desc;
                ui_mi.WE_Event_Name = db_miss.Event_Name;
                ui_mi.WE_Name = db_miss.Miss_Name;
                ui_mi.Mission_Url = ""; //历史任务的页面至空
                ui_mi.Miss_Id = db_miss.Miss_Id;


                List<Mission_Param> mis_pars = wfsd.GetMissParams(db_miss.Miss_Id);//获取当前任务参数
                foreach (var par in mis_pars)
                {
                    CParam cp = new CParam();
                    cp.type = par.Param_Type;
                    cp.name = par.Param_Name;
                    cp.value = par.Param_Value;
                    cp.description = par.Param_Desc;
                    ui_mi.Miss_Params[cp.name] = cp.value;
                    // ui_mi.Miss_ParamsAppRes[cp.name] = wf.paramstable[cp.name].linkEventsApp_res[ui_mi.WE_Event_Name];
                    ui_mi.Miss_ParamsDesc[cp.name] = cp.description;//xwm modified
                }

                List<Mission> gettime = wfs.GetWFEntityMissions(item.EntityID);//获取实体的所有任务找到第二个任务获取提报时间
                IDictionary<string, string> timeanduser = CWFEngine.GetMissionRecordInfo(gettime[1].Miss_Id);
                if (timeanduser.Count > 0)
                {
                    ui_mi.Mission_Url = timeanduser["username"];
                    ui_mi.WE_Event_Desc = timeanduser["time"];
                }
                else
                {
                    ui_mi.Mission_Url = "";
                    ui_mi.WE_Event_Desc = "";
                }
                his_missdone.Add(ui_mi);
            }

            for (int i = 0; i < his_missdone.Count; i++)
            {
                string chulijindu = "处理中";
                string[] total_time = new string[4];//最多4个完好问题，因此4个时间
                string completetime = "2010/7/7";
                if (his_missdone[i].Miss_Params["catlog_type"].ToString() == "隐患")
                    continue;
                if (his_missdone[i].Miss_Params["catlog_type"].ToString() == "不需处理")//不需处理
                {
                    chulijindu = "处理完成";
                    Mission db_miss = wfsd.GetWFEntityMissions(his_missdone[i].WE_Entity_Id).Last();
                    //获取当前任务的完成时间
                    IDictionary<string, string> timeanduser = CWFEngine.GetMissionRecordInfo(db_miss.Miss_Id);
                    if (timeanduser.Count > 0)
                    {

                        completetime = timeanduser["time"];
                    }
                    else
                    {

                        completetime = "";
                    }
                    if (completetime == "2010/7/7")
                    {
                        completetime = "";
                    }
                    if (chulijindu == "处理中")
                    {
                        completetime = "";
                    }

                }
                if (his_missdone[i].Miss_Params["catlog_type"].ToString() == "完好类")//完好跳转A5dot1跟踪
                {
                    chulijindu = "处理完成";

                    List<A5dot1Tab1> mission = tm.get_recordbyentity(his_missdone[i].WE_Entity_Id.ToString());

                    for (int k = 0; k < mission.Count; k++)
                    {
                        if (mission[k].isRectified == 0)
                        {
                            chulijindu = "处理中";
                        }

                        total_time[k] = mission[k].pqCheckTime.ToString();
                        if (total_time[k] != null)
                        {
                            if (total_time[k] != "")
                            {
                                if (Convert.ToDateTime(total_time[k]) > Convert.ToDateTime(completetime))
                                {
                                    completetime = total_time[k];
                                }
                            }
                        }

                    }
                    if (completetime == "2010/7/7")
                    {
                        completetime = "";
                    }
                    if (chulijindu == "处理中")
                    {
                        completetime = "";
                    }

                }
                //else if(his_missdone[i].Miss_Params["catlog_type"].ToString()=="隐患")//隐患跳转A11dot2跟踪
                //{
                //    //由于跳转到11dot2会产生新实体号，因此通过串号找到该实体号，此方法得到串号对应的所有实体号
                //    List<WorkFlow_Entity> getentity = wfsd.GetWorkFlowEntitiesbySer(his_missdone[i].WE_Entity_Ser);
                //    foreach (var item in getentity)
                //    {
                //        WorkFlow_Define getentity_name = wfsd.GetWorkFlowDefine(item.WE_Id);//通过每个实体号找到该实体号的名字，就可以确定每个实体号的内容

                //        if(getentity_name.W_Name=="A11dot2")
                //        {
                //            Mission db_miss = wfsd.GetWFEntityMissions(item.WE_Id).Last();

                //            if (db_miss.Miss_Desc == "现场工程师提报隐患")
                //            {
                //                chulijindu = "提报";
                //            }
                //            else if (db_miss.Miss_Desc == "可靠性工程师风险矩阵评估" )
                //            {
                //                chulijindu = "危害识别中";
                //            }
                //            else if(db_miss.Miss_Desc == "专业团队审核")
                //            {
                //                WorkFlow_Entity wfe = wfsd.GetWorkFlowEntity(item.WE_Id);
                //                //CWorkFlow wf = new CWorkFlow();
                //                WorkFlows wfs = new WorkFlows();
                //                UI_MISSION ui = new UI_MISSION();
                //                ui.WE_Entity_Id = item.WE_Id;
                //                ui.WE_Entity_Ser = wfe.WE_Ser;
                //                ui.WE_Event_Desc = db_miss.Miss_Desc;
                //                ui.WE_Event_Name = db_miss.Event_Name;
                //                ui.WE_Name = db_miss.Miss_Name;
                //                ui.Mission_Url = ""; //历史任务的页面至空
                //                ui.Miss_Id = db_miss.Miss_Id;
                //                List<Mission_Param> mis_pars = wfsd.GetMissParams(db_miss.Miss_Id);//获取当前任务参数
                //                foreach (var par in mis_pars)
                //                {
                //                    CParam cp = new CParam();
                //                    cp.type = par.Param_Type;
                //                    cp.name = par.Param_Name;
                //                    cp.value = par.Param_Value;
                //                    cp.description = par.Param_Desc;
                //                    ui.Miss_Params[cp.name] = cp.value;                                   
                //                    ui.Miss_ParamsDesc[cp.name] = cp.description;
                //                }                       
                //                //判断风险评估结果所在区域的颜色
                //                if(ui.Miss_Params["RiskMatrix_Color"].ToString()=="green")
                //                {
                //                    chulijindu="完成危害识别，结果为绿色";
                //                }
                //                else if (ui.Miss_Params["RiskMatrix_Color"].ToString() == "red")
                //                {
                //                    chulijindu = "完成危害识别，结果为红色";
                //                }
                //                else if (ui.Miss_Params["RiskMatrix_Color"].ToString() == "yellow")
                //                {
                //                    chulijindu = "完成危害识别，结果为黄色";
                //                }
                //                if(ui.Miss_Params["ZytdConfirm_Result"].ToString()=="False")
                //                {
                //                    chulijindu = "审核未通过，重新评估";
                //                }

                //            }
                //            else if (db_miss.Miss_Desc == "车间确立应急预案")
                //            {
                //                chulijindu = "应急预案制定与实施中";
                //            }
                //            else if (db_miss.Miss_Desc == "片区监督执行")
                //            {
                //                chulijindu = "处理完成";
                //                WorkFlow_Entity wfe = wfsd.GetWorkFlowEntity(item.WE_Id);
                //                //CWorkFlow wf = new CWorkFlow();
                //                WorkFlows wfs = new WorkFlows();
                //                UI_MISSION ui = new UI_MISSION();
                //                ui.WE_Entity_Id = item.WE_Id;
                //                ui.WE_Entity_Ser = wfe.WE_Ser;
                //                ui.WE_Event_Desc = db_miss.Miss_Desc;
                //                ui.WE_Event_Name = db_miss.Event_Name;
                //                ui.WE_Name = db_miss.Miss_Name;
                //                ui.Mission_Url = ""; //历史任务的页面至空
                //                ui.Miss_Id = db_miss.Miss_Id;
                //                List<Mission_Param> mis_pars = wfsd.GetMissParams(db_miss.Miss_Id);//获取当前任务参数
                //                foreach (var par in mis_pars)
                //                {
                //                    CParam cp = new CParam();
                //                    cp.type = par.Param_Type;
                //                    cp.name = par.Param_Name;
                //                    cp.value = par.Param_Value;
                //                    cp.description = par.Param_Desc;
                //                    ui.Miss_Params[cp.name] = cp.value;
                //                    ui.Miss_ParamsDesc[cp.name] = cp.description;
                //                }            
                //                if(ui.Miss_Params["Supervise_done"].ToString()=="False")
                //                {
                //                    chulijindu = "应急预案制定与实施中";
                //                }
                //                //获取当前任务的完成时间
                //                IDictionary<string, string> timeanduser = CWFEngine.GetMissionRecordInfo(db_miss.Miss_Id);
                //                if (timeanduser.Count > 0)
                //                {

                //                    completetime = timeanduser["time"];
                //                }
                //                else
                //                {

                //                    completetime = "";
                //                }
                //            }
                //            else if (db_miss.Miss_Desc == "跳转到【A11.3】风险管控模块")
                //            {
                //                chulijindu = "风险管控";
                //            }
                //        }
                //        //此处写风险管控if(getentity_name.W_Name=="A11dot3")
                //    }
                //    if (completetime == "2010/7/7")
                //    {
                //        completetime = "";
                //    }
                //    if (chulijindu == "处理中")
                //    {
                //        completetime = "";
                //    }
                //}
                else if (his_missdone[i].Miss_Params["catlog_type"].ToString() == "故障")
                {
                    Mission db_miss = wfsd.GetWFEntityMissions(his_missdone[i].WE_Entity_Id).Last();
                    if (db_miss.Miss_Desc == "可靠性工程师缺陷分类")
                    {
                        chulijindu = "提报";
                    }
                    else if (db_miss.Miss_Desc == "专业团队审核")
                    {
                        chulijindu = "机动处已确认";
                    }
                    //按时间和设备编号在workflow中查询A8.2的实体号
                    string query_list = "E.W_Name,E.WE_Id,M.Event_Name, R.time";
                    string query_condition = string.Format("P.Equip_Code in ({0}) and E.W_Name = 'A8dot2'", his_missdone[i].Miss_Params["Equip_Code"]);
                    //时间为当月
                    DateTime dtime = DateTime.Now;
                    dtime = dtime.AddDays(-dtime.Day + 1);
                    dtime = dtime.AddHours(-dtime.Hour);
                    dtime = dtime.AddMinutes(-dtime.Minute);
                    dtime = dtime.AddSeconds(-dtime.Second);
                    string record_filter = string.Format("time >= '{0}' ", dtime.ToString()); //"1 <> 1";
                    System.Data.DataTable dtA8dot2 = CWFEngine.QueryAllInformation(query_list, query_condition, record_filter);
                    DataRow[] NNullRows = dtA8dot2.Select("WE_Id is not Null");
                    if (NNullRows.Length > 0)
                    {
                        //由于当月一台设备只有一个流程，实体号唯一，任取NNullRows中一行，再取其中ItemArray中第二个数据（按query_list查询条件）即为实体号
                        var entityid = NNullRows[0].ItemArray[1];

                        //获取对应实体号的最后一个任务追踪进度
                        Mission db_missA8dot2 = wfsd.GetWFEntityMissions(Convert.ToInt32(entityid)).Last();
                        //写if而不写else if是因为13.1和8.2是断开的，跳转8.2仍满足db_miss.Miss_Desc == "专业团队审核"
                        if (db_missA8dot2.Miss_Desc == "检修单位按计划建立工单，完善工序、组件")
                        {
                            UI_MISSION ui = new UI_MISSION();
                            List<Mission_Param> mis_pars = wfsd.GetMissParams(db_missA8dot2.Miss_Id);//获取当前任务参数
                            foreach (var par in mis_pars)
                            {
                                CParam cp = new CParam();
                                cp.type = par.Param_Type;
                                cp.name = par.Param_Name;
                                cp.value = par.Param_Value;
                                cp.description = par.Param_Desc;
                                ui.Miss_Params[cp.name] = cp.value;
                                ui.Miss_ParamsDesc[cp.name] = cp.description;
                            }
                            chulijindu = "工单已建立并完善，工单号为：" + ui.Miss_Params["Job_Order"].ToString();
                        }
                        else if (db_missA8dot2.Miss_Desc == "机动处计划科审2")
                        {
                            chulijindu = "检修工单已下达";
                        }
                        else if (db_missA8dot2.Miss_Desc == "物资处采购，填写到货时间")
                        {
                            chulijindu = "物资采购中";
                        }
                        else if (db_missA8dot2.Miss_Desc == "物资处确认到货并通知检修单位")
                        {
                            chulijindu = "物资已到货";
                        }
                        else if (db_missA8dot2.Miss_Desc == "检修单位上传检修方案" || db_missA8dot2.Miss_Desc == "专业团队审批" || db_missA8dot2.Miss_Desc == "检修单位填写检修内容及关键工序，关联作业指导书" || db_missA8dot2.Miss_Desc == "可靠性工程师审批")
                        {
                            chulijindu = "检修方案制定与审判";
                        }
                        else if (db_missA8dot2.Miss_Desc == "现场工程师确认是否可实施计划")
                        {
                            UI_MISSION ui = new UI_MISSION();
                            List<Mission_Param> mis_pars = wfsd.GetMissParams(db_missA8dot2.Miss_Id);//获取当前任务参数
                            foreach (var par in mis_pars)
                            {
                                CParam cp = new CParam();
                                ui.Miss_Params[cp.name] = cp.value;
                            }
                            if (ui.Miss_Params["ZzConfirmPlan_Result"].ToString() == "是")
                            {
                                chulijindu = "检修计划实施中";
                            }
                            else if (ui.Miss_Params["ZzConfirmPlan_Result"].ToString() == "否")
                            {
                                chulijindu = "检修计划延期";
                            }
                        }
                        else if (db_missA8dot2.Miss_Desc == "检修单位确认施工完毕，上传交工资料")
                        {
                            chulijindu = "处理完成";
                            //获取当前任务的完成时间
                            IDictionary<string, string> timeanduser = CWFEngine.GetMissionRecordInfo(db_missA8dot2.Miss_Id);
                            if (timeanduser.Count > 0)
                            {
                                completetime = timeanduser["time"];
                            }
                            else
                            {
                                completetime = "";
                            }
                        }
                    }
                    else
                    {
                        chulijindu = "正在DRBPM中处理！";
                        completetime = "";
                    }

                    if (completetime == "2010/7/7")
                    {
                        completetime = "";
                    }
                    if (chulijindu == "处理中")
                    {
                        completetime = "";
                    }
                }
                //旧的工作流中没有这两个字段，因此需判断是否存在这两个字段
                string NoticeNum = "无";
                string userstate = "无";
                if (his_missdone[i].Miss_Params.ContainsKey("NoticeNum"))
                    NoticeNum = his_missdone[i].Miss_Params["NoticeNum"].ToString();
                if (his_missdone[i].Miss_Params.ContainsKey("User_State"))
                    userstate = his_missdone[i].Miss_Params["User_State"].ToString();
                object o = new
                {
                    index = i + 1,
                    noticenum = NoticeNum,
                    userstate = userstate,
                    //第一步提报参数
                    cjnamae = his_missdone[i].Miss_Params["Cj_Name"].ToString(),
                    zzname = his_missdone[i].Miss_Params["Zz_Name"].ToString(),
                    equip_gycode = his_missdone[i].Miss_Params["Equip_GyCode"].ToString(),
                    equip_code = his_missdone[i].Miss_Params["Equip_Code"].ToString(),
                    equip_type = his_missdone[i].Miss_Params["Equip_Type"].ToString(),
                    problemdetail = his_missdone[i].Miss_Params["Problem_Desc"].ToString() + '$' + his_missdone[i].Miss_Params["Problem_DescFilePath"].ToString(),
                    pro_specialty = his_missdone[i].Miss_Params["Zy_Type"].ToString(),
                    time = his_missdone[i].WE_Event_Desc,//WE_Event_Desc字段暂存时间
                    pqtype = his_missdone[i].Miss_Params["catlog_type"].ToString(),
                    jindu = chulijindu,
                    endtime = completetime,
                };
                r.Add(o);
            }
            int count = r.Count;
            List<A5dot1Tab1> Allmission = tm.get_All();
            for (int te = 0; te < Allmission.Count; te++)
            {
                if (Allmission[te].temp2 == null)
                {
                    string jindu = "处理中";
                    string edtime = "";
                    if (Allmission[te].isRectified == 1)
                    {
                        jindu = "处理完成";
                        edtime = Allmission[te].pqCheckTime.ToString();
                    }


                    object x = new
                    {
                        index = count + 1,

                        noticenum = "无",
                        userstate = "无",
                        //第一步提报参数
                        cjnamae = Allmission[te].cjName,
                        zzname = Allmission[te].zzName,
                        equip_gycode = Allmission[te].sbGyCode,
                        equip_code = Allmission[te].sbCode,
                        equip_type = Allmission[te].sbType,
                        problemdetail = Allmission[te].notGoodContent + '$' + '$',
                        pro_specialty = Allmission[te].zyType,
                        time = Allmission[te].zzSubmitTime.ToString(),
                        pqtype = "完好类",
                        jindu = jindu,
                        endtime = edtime,
                    };
                    count++;
                    r.Add(x);
                }
            }
            //11dot2状态监测
            List<UI_MISSION> miss;
            //有权限
            miss = CWFEngine.GetActiveMissions<Person_Info>(((IObjectContextAdapter)(new EquipWebContext())).ObjectContext, "A11dot2", true);


            List<UI_MISSION> his_missdoneA11dot2 = new List<UI_MISSION>();//最后一步均为跳转，因此最后一步关联了所需的所有参数，可直接获得最后一步的任务参数即可



            foreach (var item in miss)//获得entity_id
            {

                Mission db_miss = wfsd.GetWFEntityMissions(item.WE_Entity_Id).Last();//获取该实体最后一个任务
                WorkFlow_Entity wfe = wfsd.GetWorkFlowEntity(item.WE_Entity_Id);
                //CWorkFlow wf = new CWorkFlow();
                WorkFlows wfs = new WorkFlows();


                UI_MISSION ui_mi = new UI_MISSION();
                ui_mi.WE_Entity_Id = item.WE_Entity_Id;
                ui_mi.WE_Entity_Ser = wfe.WE_Ser;
                ui_mi.WE_Event_Desc = db_miss.Miss_Desc;
                ui_mi.WE_Event_Name = db_miss.Event_Name;
                ui_mi.WE_Name = db_miss.Miss_Name;
                ui_mi.Mission_Url = ""; //历史任务的页面至空
                ui_mi.Miss_Id = db_miss.Miss_Id;


                List<Mission_Param> mis_pars = wfsd.GetMissParams(db_miss.Miss_Id);//获取当前任务参数
                foreach (var par in mis_pars)
                {
                    CParam cp = new CParam();
                    cp.type = par.Param_Type;
                    cp.name = par.Param_Name;
                    cp.value = par.Param_Value;
                    cp.description = par.Param_Desc;
                    ui_mi.Miss_Params[cp.name] = cp.value;
                    // ui_mi.Miss_ParamsAppRes[cp.name] = wf.paramstable[cp.name].linkEventsApp_res[ui_mi.WE_Event_Name];
                    ui_mi.Miss_ParamsDesc[cp.name] = cp.description;//xwm modified
                }
                ui_mi.Mission_Url = "";
                ui_mi.WE_Event_Desc = "";
                List<Mission> gettime = wfs.GetWFEntityMissions(item.WE_Entity_Id);//获取实体的所有任务找到第二个任务获取提报时间
                if (gettime.Count > 1)
                {
                    IDictionary<string, string> timeanduser = CWFEngine.GetMissionRecordInfo(gettime[1].Miss_Id);
                    if (timeanduser.Count > 0)
                    {
                        ui_mi.Mission_Url = timeanduser["username"];
                        ui_mi.WE_Event_Desc = timeanduser["time"];
                    }
                    else
                    {
                        ui_mi.Mission_Url = "";
                        ui_mi.WE_Event_Desc = "";
                    }
                }

                his_missdoneA11dot2.Add(ui_mi);
            }
            foreach (var re in his_missdoneA11dot2)
            {
                string chulijindu = "处理中";
                string completetime = "2010/7/7";
                Mission db_miss = wfsd.GetWFEntityMissions(re.WE_Entity_Id).Last();
                if (db_miss.Miss_Desc == "Start")
                {
                    chulijindu = "提报尚未完成";
                }
                if (db_miss.Miss_Desc == "现场工程师提报隐患")
                {
                    chulijindu = "提报";
                }
                else if (db_miss.Miss_Desc == "可靠性工程师风险矩阵评估")
                {
                    chulijindu = "危害识别中";
                }
                else if (db_miss.Miss_Desc == "专业团队审核")
                {
                    WorkFlow_Entity wfe = wfsd.GetWorkFlowEntity(re.WE_Entity_Id);
                    //CWorkFlow wf = new CWorkFlow();
                    WorkFlows wfs = new WorkFlows();
                    UI_MISSION ui = new UI_MISSION();
                    ui.WE_Entity_Id = re.WE_Entity_Id;
                    ui.WE_Entity_Ser = wfe.WE_Ser;
                    ui.WE_Event_Desc = db_miss.Miss_Desc;
                    ui.WE_Event_Name = db_miss.Event_Name;
                    ui.WE_Name = db_miss.Miss_Name;
                    ui.Mission_Url = ""; //历史任务的页面至空
                    ui.Miss_Id = db_miss.Miss_Id;
                    List<Mission_Param> mis_pars = wfsd.GetMissParams(db_miss.Miss_Id);//获取当前任务参数
                    foreach (var par in mis_pars)
                    {
                        CParam cp = new CParam();
                        cp.type = par.Param_Type;
                        cp.name = par.Param_Name;
                        cp.value = par.Param_Value;
                        cp.description = par.Param_Desc;
                        ui.Miss_Params[cp.name] = cp.value;
                        ui.Miss_ParamsDesc[cp.name] = cp.description;
                    }
                    //判断风险评估结果所在区域的颜色
                    if (ui.Miss_Params["RiskMatrix_Color"].ToString() == "green")
                    {
                        chulijindu = "完成危害识别，结果为绿色";
                    }
                    else if (ui.Miss_Params["RiskMatrix_Color"].ToString() == "red")
                    {
                        chulijindu = "完成危害识别，结果为红色";
                    }
                    else if (ui.Miss_Params["RiskMatrix_Color"].ToString() == "yellow")
                    {
                        chulijindu = "完成危害识别，结果为黄色";
                    }
                    if (ui.Miss_Params["ZytdConfirm_Result"].ToString() == "False")
                    {
                        chulijindu = "审核未通过，重新评估";
                    }

                }
                else if (db_miss.Miss_Desc == "车间确立应急预案")
                {
                    chulijindu = "应急预案制定与实施中";
                }
                else if (db_miss.Miss_Desc == "片区监督执行")
                {
                    chulijindu = "处理完成";
                    WorkFlow_Entity wfe = wfsd.GetWorkFlowEntity(re.WE_Entity_Id);
                    //CWorkFlow wf = new CWorkFlow();
                    WorkFlows wfs = new WorkFlows();
                    UI_MISSION ui = new UI_MISSION();
                    ui.WE_Entity_Id = re.WE_Entity_Id;
                    ui.WE_Entity_Ser = wfe.WE_Ser;
                    ui.WE_Event_Desc = db_miss.Miss_Desc;
                    ui.WE_Event_Name = db_miss.Event_Name;
                    ui.WE_Name = db_miss.Miss_Name;
                    ui.Mission_Url = ""; //历史任务的页面至空
                    ui.Miss_Id = db_miss.Miss_Id;
                    List<Mission_Param> mis_pars = wfsd.GetMissParams(db_miss.Miss_Id);//获取当前任务参数
                    foreach (var par in mis_pars)
                    {
                        CParam cp = new CParam();
                        cp.type = par.Param_Type;
                        cp.name = par.Param_Name;
                        cp.value = par.Param_Value;
                        cp.description = par.Param_Desc;
                        ui.Miss_Params[cp.name] = cp.value;
                        ui.Miss_ParamsDesc[cp.name] = cp.description;
                    }
                    if (ui.Miss_Params["Supervise_done"].ToString() == "False")
                    {
                        chulijindu = "应急预案制定与实施中";
                    }
                    //获取当前任务的完成时间
                    IDictionary<string, string> timeanduser = CWFEngine.GetMissionRecordInfo(db_miss.Miss_Id);
                    if (timeanduser.Count > 0)
                    {

                        completetime = timeanduser["time"];
                    }
                    else
                    {

                        completetime = "";
                    }
                }
                else if (db_miss.Miss_Desc == "跳转到【A11.3】风险管控模块")
                {
                    chulijindu = "风险管控";
                }
                else if (db_miss.Miss_Desc == "专业团队确立管控措施")
                {
                    chulijindu = "专业团队确立管控措施";
                }
                else if (db_miss.Miss_Desc == "机动处组织监督实施")
                {
                    chulijindu = "机动处组织监督实施";
                }
                else if (db_miss.Miss_Desc == "管控措施实施后确认风险是否可接受，并进行风险登记")
                {
                    chulijindu = "管控措施实施后确认风险是否可接受，并进行风险登记";
                }
                else if (db_miss.Miss_Desc == "车间确立管控措施")
                {
                    chulijindu = "车间确立管控措施";
                }
                else if (db_miss.Miss_Desc == "可靠性工程师审核")
                {
                    chulijindu = "可靠性工程师审核";
                }
                else if (db_miss.Miss_Desc == "车间组织监督实施")
                {
                    chulijindu = "车间组织监督实施";
                }
                else if (db_miss.Miss_Desc == "管控措施实施后确认风险是否消除，并进行风险登记")
                {
                    chulijindu = "管控措施实施后确认风险是否消除，并进行风险登记";
                }
                if (completetime == "2010/7/7")
                {
                    completetime = "";
                }
                if (chulijindu == "处理中")
                {
                    completetime = "";
                }
                //旧的工作流中没有这两个字段，因此需判断是否存在这两个字段
                string NoticeNum="无";
                string userstate="无";
                if(re.Miss_Params.ContainsKey("NoticeNum"))
                    NoticeNum=re.Miss_Params["NoticeNum"].ToString();
                if(re.Miss_Params.ContainsKey("User_State"))
                    userstate=re.Miss_Params["User_State"].ToString();

                object o = new
                {
                    index = count + 1,
                    //第一步提报参数
                    cjnamae = re.Miss_Params["Cj_Name"].ToString(),
                    zzname = re.Miss_Params["Zz_Name"].ToString(),
                    equip_gycode = re.Miss_Params["Equip_GyCode"].ToString(),
                    equip_code = re.Miss_Params["Equip_Code"].ToString(),
                    equip_type = re.Miss_Params["Equip_Type"].ToString(),
                    problemdetail = re.Miss_Params["Problem_Desc"].ToString() + '$' + re.Miss_Params["Problem_DescFilePath"].ToString(),
                    pro_specialty = re.Miss_Params["Zy_Type"].ToString(),
                    time = re.WE_Event_Desc,//WE_Event_Desc字段暂存时间
                    pqtype = "隐患类",
                    jindu = chulijindu,
                    endtime = completetime,
                    noticenum = NoticeNum,
                    userstate = userstate
                };
                r.Add(o);
                count++;

            }

            string str = JsonConvert.SerializeObject(r);
            return ("{" + "\"data\": " + str + "}");
        }
        public string tongji()
        {
            List<object> r = new List<object>();//存放返回结果
            int Count_WH_Done = 0;//完好类已封闭
            int Count_YH_Done = 0;//隐患类已封闭
            int Count_GZ_Done = 0;//故障类已封闭
            int Count_WH = 0;//完好类未封闭
            int Count_YH = 0;//隐患类未封闭
            int Count_GZ = 0;//故障类未封闭
            List<UI_WFEntity_Info> miss_done;
            miss_done = CWFEngine.GetWFEntityByHistoryDone(t => t.Miss_WFentity.WE_Wref.W_Name == "A13dot1");
            List<UI_MISSION> his_missdone = new List<UI_MISSION>();//最后一步均为跳转，因此最后一步关联了所需的所有参数，可直接获得最后一步的任务参数即可
            WorkFlows wfsd = new WorkFlows();
            TablesManagment tm = new TablesManagment();

            foreach (var item in miss_done)//获得entity_id
            {

                Mission db_miss = wfsd.GetWFEntityMissions(item.EntityID).Last();//获取该实体最后一个任务
                WorkFlow_Entity wfe = wfsd.GetWorkFlowEntity(item.EntityID);
                //CWorkFlow wf = new CWorkFlow();
                WorkFlows wfs = new WorkFlows();


                UI_MISSION ui_mi = new UI_MISSION();
                ui_mi.WE_Entity_Id = item.EntityID;
                ui_mi.WE_Entity_Ser = wfe.WE_Ser;
                ui_mi.WE_Event_Desc = db_miss.Miss_Desc;
                ui_mi.WE_Event_Name = db_miss.Event_Name;
                ui_mi.WE_Name = db_miss.Miss_Name;
                ui_mi.Mission_Url = ""; //历史任务的页面至空
                ui_mi.Miss_Id = db_miss.Miss_Id;


                List<Mission_Param> mis_pars = wfsd.GetMissParams(db_miss.Miss_Id);//获取当前任务参数
                foreach (var par in mis_pars)
                {
                    CParam cp = new CParam();
                    cp.type = par.Param_Type;
                    cp.name = par.Param_Name;
                    cp.value = par.Param_Value;
                    cp.description = par.Param_Desc;
                    ui_mi.Miss_Params[cp.name] = cp.value;
                    // ui_mi.Miss_ParamsAppRes[cp.name] = wf.paramstable[cp.name].linkEventsApp_res[ui_mi.WE_Event_Name];
                    ui_mi.Miss_ParamsDesc[cp.name] = cp.description;//xwm modified
                }

                List<Mission> gettime = wfs.GetWFEntityMissions(item.EntityID);//获取实体的所有任务找到第二个任务获取提报时间
                IDictionary<string, string> timeanduser = CWFEngine.GetMissionRecordInfo(gettime[1].Miss_Id);
                if (timeanduser.Count > 0)
                {
                    ui_mi.Mission_Url = timeanduser["username"];
                    ui_mi.WE_Event_Desc = timeanduser["time"];
                }
                else
                {
                    ui_mi.Mission_Url = "";
                    ui_mi.WE_Event_Desc = "";
                }
                his_missdone.Add(ui_mi);
            }

            for (int i = 0; i < his_missdone.Count; i++)
            {
                string chulijindu = "处理中";
                string[] total_time = new string[4];
                string completetime = "2010/7/7";

                if (his_missdone[i].Miss_Params["catlog_type"].ToString() == "不需处理")//不需处理
                {
                    chulijindu = "处理完成";
                    Mission db_miss = wfsd.GetWFEntityMissions(his_missdone[i].WE_Entity_Id).Last();
                    //获取当前任务的完成时间
                    IDictionary<string, string> timeanduser = CWFEngine.GetMissionRecordInfo(db_miss.Miss_Id);
                    if (timeanduser.Count > 0)
                    {

                        completetime = timeanduser["time"];
                    }
                    else
                    {

                        completetime = "";
                    }
                    if (completetime == "2010/7/7")
                    {
                        completetime = "";
                    }
                    if (chulijindu == "处理中")
                    {
                        completetime = "";
                    }

                }
                if (his_missdone[i].Miss_Params["catlog_type"].ToString() == "完好类")//完好跳转A5dot1跟踪
                {
                    chulijindu = "处理完成";

                    List<A5dot1Tab1> mission = tm.get_recordbyentity(his_missdone[i].WE_Entity_Id.ToString());

                    for (int k = 0; k < mission.Count; k++)
                    {
                        if (mission[k].isRectified == 0)
                        {
                            chulijindu = "处理中";
                        }

                        total_time[k] = mission[k].pqCheckTime.ToString();
                        if (total_time[k] != null)
                        {
                            if (total_time[k] != "")
                            {
                                if (Convert.ToDateTime(total_time[k]) > Convert.ToDateTime(completetime))
                                {
                                    completetime = total_time[k];
                                }
                            }
                        }

                    }
                    if (completetime == "2010/7/7")
                    {
                        completetime = "";
                    }
                    if (chulijindu == "处理中")
                    {
                        completetime = "";
                    }
                    //统计完好类封闭正在处理的数量
                    if (chulijindu == "处理完成")
                    {
                        Count_WH_Done++;
                    }
                    if (chulijindu == "处理中")
                    {
                        Count_WH++;
                    }
                }
                //else if (his_missdone[i].Miss_Params["catlog_type"].ToString() == "隐患")//隐患跳转A11dot2跟踪
                //{
                //    //由于跳转到11dot2会产生新实体号，因此通过串号找到该实体号，此方法得到串号对应的所有实体号
                //    List<WorkFlow_Entity> getentity = wfsd.GetWorkFlowEntitiesbySer(his_missdone[i].WE_Entity_Ser);
                //    foreach (var item in getentity)
                //    {
                //        WorkFlow_Define getentity_name = wfsd.GetWorkFlowDefine(item.WE_Id);//通过每个实体号找到该实体号的名字，就可以确定每个实体号的内容

                //        if (getentity_name.W_Name == "A11dot2")
                //        {
                //            Mission db_miss = wfsd.GetWFEntityMissions(item.WE_Id).Last();

                //            if (db_miss.Miss_Desc == "现场工程师提报隐患")
                //            {
                //                chulijindu = "提报";
                //            }
                //            else if (db_miss.Miss_Desc == "可靠性工程师风险矩阵评估")
                //            {
                //                chulijindu = "危害识别中";
                //            }
                //            else if (db_miss.Miss_Desc == "专业团队审核")
                //            {
                //                WorkFlow_Entity wfe = wfsd.GetWorkFlowEntity(item.WE_Id);
                //                //CWorkFlow wf = new CWorkFlow();
                //                WorkFlows wfs = new WorkFlows();
                //                UI_MISSION ui = new UI_MISSION();
                //                ui.WE_Entity_Id = item.WE_Id;
                //                ui.WE_Entity_Ser = wfe.WE_Ser;
                //                ui.WE_Event_Desc = db_miss.Miss_Desc;
                //                ui.WE_Event_Name = db_miss.Event_Name;
                //                ui.WE_Name = db_miss.Miss_Name;
                //                ui.Mission_Url = ""; //历史任务的页面至空
                //                ui.Miss_Id = db_miss.Miss_Id;
                //                List<Mission_Param> mis_pars = wfsd.GetMissParams(db_miss.Miss_Id);//获取当前任务参数
                //                foreach (var par in mis_pars)
                //                {
                //                    CParam cp = new CParam();
                //                    cp.type = par.Param_Type;
                //                    cp.name = par.Param_Name;
                //                    cp.value = par.Param_Value;
                //                    cp.description = par.Param_Desc;
                //                    ui.Miss_Params[cp.name] = cp.value;
                //                    ui.Miss_ParamsDesc[cp.name] = cp.description;
                //                }
                //                //判断风险评估结果所在区域的颜色
                //                if (ui.Miss_Params["RiskMatrix_Color"].ToString() == "green")
                //                {
                //                    chulijindu = "完成危害识别，结果为绿色";
                //                }
                //                else if (ui.Miss_Params["RiskMatrix_Color"].ToString() == "red")
                //                {
                //                    chulijindu = "完成危害识别，结果为红色";
                //                }
                //                else if (ui.Miss_Params["RiskMatrix_Color"].ToString() == "yellow")
                //                {
                //                    chulijindu = "完成危害识别，结果为黄色";
                //                }
                //                if (ui.Miss_Params["ZytdConfirm_Result"].ToString() == "False")
                //                {
                //                    chulijindu = "审核未通过，重新评估";
                //                }

                //            }
                //            else if (db_miss.Miss_Desc == "车间确立应急预案")
                //            {
                //                chulijindu = "应急预案制定与实施中";
                //            }
                //            else if (db_miss.Miss_Desc == "片区监督执行")
                //            {
                //                chulijindu = "处理完成";
                //                WorkFlow_Entity wfe = wfsd.GetWorkFlowEntity(item.WE_Id);
                //                //CWorkFlow wf = new CWorkFlow();
                //                WorkFlows wfs = new WorkFlows();
                //                UI_MISSION ui = new UI_MISSION();
                //                ui.WE_Entity_Id = item.WE_Id;
                //                ui.WE_Entity_Ser = wfe.WE_Ser;
                //                ui.WE_Event_Desc = db_miss.Miss_Desc;
                //                ui.WE_Event_Name = db_miss.Event_Name;
                //                ui.WE_Name = db_miss.Miss_Name;
                //                ui.Mission_Url = ""; //历史任务的页面至空
                //                ui.Miss_Id = db_miss.Miss_Id;
                //                List<Mission_Param> mis_pars = wfsd.GetMissParams(db_miss.Miss_Id);//获取当前任务参数
                //                foreach (var par in mis_pars)
                //                {
                //                    CParam cp = new CParam();
                //                    cp.type = par.Param_Type;
                //                    cp.name = par.Param_Name;
                //                    cp.value = par.Param_Value;
                //                    cp.description = par.Param_Desc;
                //                    ui.Miss_Params[cp.name] = cp.value;
                //                    ui.Miss_ParamsDesc[cp.name] = cp.description;
                //                }
                //                if (ui.Miss_Params["Supervise_done"].ToString() == "False")
                //                {
                //                    chulijindu = "应急预案制定与实施中";
                //                }
                //                //获取当前任务的完成时间
                //                IDictionary<string, string> timeanduser = CWFEngine.GetMissionRecordInfo(db_miss.Miss_Id);
                //                if (timeanduser.Count > 0)
                //                {

                //                    completetime = timeanduser["time"];
                //                }
                //                else
                //                {

                //                    completetime = "";
                //                }
                //            }
                //            else if (db_miss.Miss_Desc == "跳转到【A11.3】风险管控模块")
                //            {
                //                chulijindu = "风险管控";
                //            }
                //        }
                //        //此处写风险管控if(getentity_name.W_Name=="A11dot3")
                //    }
                //    if (completetime == "2010/7/7")
                //    {
                //        completetime = "";
                //    }
                //    if (chulijindu == "处理中")
                //    {
                //        completetime = "";
                //    }
                //    //统计完好类封闭正在处理的数量
                //    if (chulijindu == "处理完成")
                //    {
                //        Count_YH_Done++;
                //    }
                //    if (chulijindu != "处理完成")
                //    {
                //        Count_YH++;
                //    }
                //}
                else if (his_missdone[i].Miss_Params["catlog_type"].ToString() == "故障")
                {
                    Mission db_miss = wfsd.GetWFEntityMissions(his_missdone[i].WE_Entity_Id).Last();
                    if (db_miss.Miss_Desc == "可靠性工程师缺陷分类")
                    {
                        chulijindu = "提报";
                    }
                    else if (db_miss.Miss_Desc == "专业团队审核")
                    {
                        chulijindu = "机动处已确认";
                    }
                    //按时间和设备编号在workflow中查询A8.2的实体号
                    string query_list = "E.W_Name,E.WE_Id,M.Event_Name, R.time";
                    string query_condition = string.Format("P.Equip_Code in ({0}) and E.W_Name = 'A8dot2'", his_missdone[i].Miss_Params["Equip_Code"]);
                    //时间为当月
                    DateTime dtime = DateTime.Now;
                    dtime = dtime.AddDays(-dtime.Day + 1);
                    dtime = dtime.AddHours(-dtime.Hour);
                    dtime = dtime.AddMinutes(-dtime.Minute);
                    dtime = dtime.AddSeconds(-dtime.Second);
                    string record_filter = string.Format("time >= '{0}' ", dtime.ToString()); //"1 <> 1";
                    System.Data.DataTable dtA8dot2 = CWFEngine.QueryAllInformation(query_list, query_condition, record_filter);
                    DataRow[] NNullRows = dtA8dot2.Select("WE_Id is not Null");
                    if (NNullRows.Length > 0)
                    {
                        //由于当月一台设备只有一个流程，实体号唯一，任取NNullRows中一行，再取其中ItemArray中第二个数据（按query_list查询条件）即为实体号
                        var entityid = NNullRows[0].ItemArray[1];

                        //获取对应实体号的最后一个任务追踪进度
                        Mission db_missA8dot2 = wfsd.GetWFEntityMissions(Convert.ToInt32(entityid)).Last();
                        //写if而不写else if是因为13.1和8.2是断开的，跳转8.2仍满足db_miss.Miss_Desc == "专业团队审核"
                        if (db_missA8dot2.Miss_Desc == "检修单位按计划建立工单，完善工序、组件")
                        {
                            UI_MISSION ui = new UI_MISSION();
                            List<Mission_Param> mis_pars = wfsd.GetMissParams(db_missA8dot2.Miss_Id);//获取当前任务参数
                            foreach (var par in mis_pars)
                            {
                                CParam cp = new CParam();
                                cp.type = par.Param_Type;
                                cp.name = par.Param_Name;
                                cp.value = par.Param_Value;
                                cp.description = par.Param_Desc;
                                ui.Miss_Params[cp.name] = cp.value;
                                ui.Miss_ParamsDesc[cp.name] = cp.description;
                            }
                            chulijindu = "工单已建立并完善，工单号为：" + ui.Miss_Params["Job_Order"].ToString();
                        }
                        else if (db_missA8dot2.Miss_Desc == "机动处计划科审2")
                        {
                            chulijindu = "检修工单已下达";
                        }
                        else if (db_missA8dot2.Miss_Desc == "物资处采购，填写到货时间")
                        {
                            chulijindu = "物资采购中";
                        }
                        else if (db_missA8dot2.Miss_Desc == "物资处确认到货并通知检修单位")
                        {
                            chulijindu = "物资已到货";
                        }
                        else if (db_missA8dot2.Miss_Desc == "检修单位上传检修方案" || db_missA8dot2.Miss_Desc == "专业团队审批" || db_missA8dot2.Miss_Desc == "检修单位填写检修内容及关键工序，关联作业指导书" || db_missA8dot2.Miss_Desc == "可靠性工程师审批")
                        {
                            chulijindu = "检修方案制定与审判";
                        }
                        else if (db_missA8dot2.Miss_Desc == "现场工程师确认是否可实施计划")
                        {
                            UI_MISSION ui = new UI_MISSION();
                            List<Mission_Param> mis_pars = wfsd.GetMissParams(db_missA8dot2.Miss_Id);//获取当前任务参数
                            foreach (var par in mis_pars)
                            {
                                CParam cp = new CParam();
                                ui.Miss_Params[cp.name] = cp.value;
                            }
                            if (ui.Miss_Params["ZzConfirmPlan_Result"].ToString() == "是")
                            {
                                chulijindu = "检修计划实施中";
                            }
                            else if (ui.Miss_Params["ZzConfirmPlan_Result"].ToString() == "否")
                            {
                                chulijindu = "检修计划延期";
                            }
                        }
                        else if (db_missA8dot2.Miss_Desc == "检修单位确认施工完毕，上传交工资料")
                        {
                            chulijindu = "处理完成";
                            //获取当前任务的完成时间
                            IDictionary<string, string> timeanduser = CWFEngine.GetMissionRecordInfo(db_missA8dot2.Miss_Id);
                            if (timeanduser.Count > 0)
                            {
                                completetime = timeanduser["time"];
                            }
                            else
                            {
                                completetime = "";
                            }
                        }
                    }
                    else
                    {
                        chulijindu = "正在DRBPM中处理！";
                    }
                    completetime = "";
                    if (completetime == "2010/7/7")
                    {
                        completetime = "";
                    }
                    if (chulijindu == "处理中")
                    {
                        completetime = "";
                    }
                    if (chulijindu == "处理完成")
                    {
                        Count_GZ_Done++;
                    }
                    if (chulijindu != "处理完成")
                    {
                        Count_GZ++;
                    }
                }

            }
            List<A5dot1Tab1> Allmission = tm.get_All();
            for (int te = 0; te < Allmission.Count; te++)
            {
                if (Allmission[te].temp2 == null)
                {
                    string jindu = "处理中";
                    string edtime = "";
                    if (Allmission[te].isRectified == 1)
                    {
                        jindu = "处理完成";
                        edtime = Allmission[te].pqCheckTime.ToString();
                    }
                    if (jindu == "处理中")
                    {
                        Count_WH++;
                    }
                    else if (jindu == "处理完成")
                        Count_WH_Done++;
                }
            }
            //舍弃下面注释内容，以前是通过A13dot1找A11dot2的工作流，现在A11dot2全部内容归入缺陷管理表后直接找A11dot2
            ////11dot2状态监测
            //List<UI_MISSION> miss;
            ////有权限
            //miss = CWFEngine.GetActiveMissions<Person_Info>(((IObjectContextAdapter)(new EquipWebContext())).ObjectContext, "A11dot2", true);


            //List<UI_MISSION> his_missdoneA11dot2 = new List<UI_MISSION>();//最后一步均为跳转，因此最后一步关联了所需的所有参数，可直接获得最后一步的任务参数即可



            //foreach (var item in miss)//获得entity_id
            //{

            //    Mission db_miss = wfsd.GetWFEntityMissions(item.WE_Entity_Id).Last();//获取该实体最后一个任务
            //    WorkFlow_Entity wfe = wfsd.GetWorkFlowEntity(item.WE_Entity_Id);
            //    //CWorkFlow wf = new CWorkFlow();
            //    WorkFlows wfs = new WorkFlows();


            //    UI_MISSION ui_mi = new UI_MISSION();
            //    ui_mi.WE_Entity_Id = item.WE_Entity_Id;
            //    ui_mi.WE_Entity_Ser = wfe.WE_Ser;
            //    ui_mi.WE_Event_Desc = db_miss.Miss_Desc;
            //    ui_mi.WE_Event_Name = db_miss.Event_Name;
            //    ui_mi.WE_Name = db_miss.Miss_Name;
            //    ui_mi.Mission_Url = ""; //历史任务的页面至空
            //    ui_mi.Miss_Id = db_miss.Miss_Id;


            //    List<Mission_Param> mis_pars = wfsd.GetMissParams(db_miss.Miss_Id);//获取当前任务参数
            //    foreach (var par in mis_pars)
            //    {
            //        CParam cp = new CParam();
            //        cp.type = par.Param_Type;
            //        cp.name = par.Param_Name;
            //        cp.value = par.Param_Value;
            //        cp.description = par.Param_Desc;
            //        ui_mi.Miss_Params[cp.name] = cp.value;
            //        // ui_mi.Miss_ParamsAppRes[cp.name] = wf.paramstable[cp.name].linkEventsApp_res[ui_mi.WE_Event_Name];
            //        ui_mi.Miss_ParamsDesc[cp.name] = cp.description;//xwm modified
            //    }
            //    ui_mi.Mission_Url = "";
            //    ui_mi.WE_Event_Desc = "";
            //    List<Mission> gettime = wfs.GetWFEntityMissions(item.WE_Entity_Id);//获取实体的所有任务找到第二个任务获取提报时间
            //    if (gettime.Count > 1)
            //    {
            //        IDictionary<string, string> timeanduser = CWFEngine.GetMissionRecordInfo(gettime[1].Miss_Id);
            //        if (timeanduser.Count > 0)
            //        {
            //            ui_mi.Mission_Url = timeanduser["username"];
            //            ui_mi.WE_Event_Desc = timeanduser["time"];
            //        }
            //        else
            //        {
            //            ui_mi.Mission_Url = "";
            //            ui_mi.WE_Event_Desc = "";
            //        }
            //    }

            //    his_missdoneA11dot2.Add(ui_mi);
            //}
            //foreach (var re in his_missdoneA11dot2)
            //{
            //    string chulijindu = "处理中";
            //    string completetime = "2010/7/7";
            //    Mission db_miss = wfsd.GetWFEntityMissions(re.WE_Entity_Id).Last();
            //    if (db_miss.Miss_Desc == "Start")
            //    {
            //        chulijindu = "提报尚未完成";
            //    }
            //    if (db_miss.Miss_Desc == "现场工程师提报隐患")
            //    {
            //        chulijindu = "提报";
            //    }
            //    else if (db_miss.Miss_Desc == "可靠性工程师风险矩阵评估")
            //    {
            //        chulijindu = "危害识别中";
            //    }
            //    else if (db_miss.Miss_Desc == "专业团队审核")
            //    {
            //        WorkFlow_Entity wfe = wfsd.GetWorkFlowEntity(re.WE_Entity_Id);
            //        //CWorkFlow wf = new CWorkFlow();
            //        WorkFlows wfs = new WorkFlows();
            //        UI_MISSION ui = new UI_MISSION();
            //        ui.WE_Entity_Id = re.WE_Entity_Id;
            //        ui.WE_Entity_Ser = wfe.WE_Ser;
            //        ui.WE_Event_Desc = db_miss.Miss_Desc;
            //        ui.WE_Event_Name = db_miss.Event_Name;
            //        ui.WE_Name = db_miss.Miss_Name;
            //        ui.Mission_Url = ""; //历史任务的页面至空
            //        ui.Miss_Id = db_miss.Miss_Id;
            //        List<Mission_Param> mis_pars = wfsd.GetMissParams(db_miss.Miss_Id);//获取当前任务参数
            //        foreach (var par in mis_pars)
            //        {
            //            CParam cp = new CParam();
            //            cp.type = par.Param_Type;
            //            cp.name = par.Param_Name;
            //            cp.value = par.Param_Value;
            //            cp.description = par.Param_Desc;
            //            ui.Miss_Params[cp.name] = cp.value;
            //            ui.Miss_ParamsDesc[cp.name] = cp.description;
            //        }
            //        //判断风险评估结果所在区域的颜色
            //        if (ui.Miss_Params["RiskMatrix_Color"].ToString() == "green")
            //        {
            //            chulijindu = "完成危害识别，结果为绿色";
            //        }
            //        else if (ui.Miss_Params["RiskMatrix_Color"].ToString() == "red")
            //        {
            //            chulijindu = "完成危害识别，结果为红色";
            //        }
            //        else if (ui.Miss_Params["RiskMatrix_Color"].ToString() == "yellow")
            //        {
            //            chulijindu = "完成危害识别，结果为黄色";
            //        }
            //        if (ui.Miss_Params["ZytdConfirm_Result"].ToString() == "False")
            //        {
            //            chulijindu = "审核未通过，重新评估";
            //        }

            //    }
            //    else if (db_miss.Miss_Desc == "车间确立应急预案")
            //    {
            //        chulijindu = "应急预案制定与实施中";
            //    }
            //    else if (db_miss.Miss_Desc == "片区监督执行")
            //    {
            //        chulijindu = "处理完成";
            //        WorkFlow_Entity wfe = wfsd.GetWorkFlowEntity(re.WE_Entity_Id);
            //        //CWorkFlow wf = new CWorkFlow();
            //        WorkFlows wfs = new WorkFlows();
            //        UI_MISSION ui = new UI_MISSION();
            //        ui.WE_Entity_Id = re.WE_Entity_Id;
            //        ui.WE_Entity_Ser = wfe.WE_Ser;
            //        ui.WE_Event_Desc = db_miss.Miss_Desc;
            //        ui.WE_Event_Name = db_miss.Event_Name;
            //        ui.WE_Name = db_miss.Miss_Name;
            //        ui.Mission_Url = ""; //历史任务的页面至空
            //        ui.Miss_Id = db_miss.Miss_Id;
            //        List<Mission_Param> mis_pars = wfsd.GetMissParams(db_miss.Miss_Id);//获取当前任务参数
            //        foreach (var par in mis_pars)
            //        {
            //            CParam cp = new CParam();
            //            cp.type = par.Param_Type;
            //            cp.name = par.Param_Name;
            //            cp.value = par.Param_Value;
            //            cp.description = par.Param_Desc;
            //            ui.Miss_Params[cp.name] = cp.value;
            //            ui.Miss_ParamsDesc[cp.name] = cp.description;
            //        }
            //        if (ui.Miss_Params["Supervise_done"].ToString() == "False")
            //        {
            //            chulijindu = "应急预案制定与实施中";
            //        }
            //        //获取当前任务的完成时间
            //        IDictionary<string, string> timeanduser = CWFEngine.GetMissionRecordInfo(db_miss.Miss_Id);
            //        if (timeanduser.Count > 0)
            //        {

            //            completetime = timeanduser["time"];
            //        }
            //        else
            //        {

            //            completetime = "";
            //        }
            //    }
            //    else if (db_miss.Miss_Desc == "跳转到【A11.3】风险管控模块")
            //    {
            //        chulijindu = "风险管控";
            //    }
            //    else if (db_miss.Miss_Desc == "专业团队确立管控措施")
            //    {
            //        chulijindu = "专业团队确立管控措施";
            //    }
            //    else if (db_miss.Miss_Desc == "机动处组织监督实施")
            //    {
            //        chulijindu = "机动处组织监督实施";
            //    }
            //    else if (db_miss.Miss_Desc == "管控措施实施后确认风险是否可接受，并进行风险登记")
            //    {
            //        chulijindu = "管控措施实施后确认风险是否可接受，并进行风险登记";
            //    }
            //    else if (db_miss.Miss_Desc == "车间确立管控措施")
            //    {
            //        chulijindu = "车间确立管控措施";
            //    }
            //    else if (db_miss.Miss_Desc == "可靠性工程师审核")
            //    {
            //        chulijindu = "可靠性工程师审核";
            //    }
            //    else if (db_miss.Miss_Desc == "车间组织监督实施")
            //    {
            //        chulijindu = "车间组织监督实施";
            //    }
            //    else if (db_miss.Miss_Desc == "管控措施实施后确认风险是否消除，并进行风险登记")
            //    {
            //        chulijindu = "管控措施实施后确认风险是否消除，并进行风险登记";
            //    }
            //    if (completetime == "2010/7/7")
            //    {
            //        completetime = "";
            //    }
            //    if (chulijindu == "处理中")
            //    {
            //        completetime = "";
            //    }
            //    //统计隐患类封闭正在处理的数量
            //    //    if (chulijindu == "处理完成")
            //    //    {
            //    //        Count_YH_Done++;
            //    //    }
            //    //    if (chulijindu != "处理完成")
            //    //    {
            //    //        Count_YH++;
            //    //    }

            //}
            WorkFlows getmission = new WorkFlows();
            Count_YH = getmission.GetActiveMissionsOfEntity("A11dot2").Count;
            string WE_Status = "3";
            string query_list1 = "distinct E.WE_Ser, E.WE_Id, R.username";
            string query_condition1 = "E.W_Name='A11dot2' and E.WE_Status='" + WE_Status + "' and R.username is not null";
            string record_filter1 = "username is not null";
            DataTable dt = CWFEngine.QueryAllInformation(query_list1, query_condition1, record_filter1);
            List<Gxqmodel> Gmlist = new List<Gxqmodel>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Gxqmodel Gm = new Gxqmodel();
                Gm.WE_Id = Convert.ToInt16(dt.Rows[i]["WE_Id"]);
                Gm.WE_Ser = dt.Rows[i]["WE_Ser"].ToString();
                Gmlist.Add(Gm);

            }
            Count_YH_Done = Gmlist.Count;
            string databack = Count_WH + "$" + Count_WH_Done + "$" + Count_YH + "$" + Count_YH_Done + "$" + Count_GZ + "$" + Count_GZ_Done;

            return (databack);
        }
        /// <summary>
        /// 2017-1-18新增，获取待处理通知单
        /// </summary>
        /// <returns></returns>
        public string getNotice_list()
        {
            NoticeManagement NM=new NoticeManagement();
            PersonManagment PM = new PersonManagment();
            List<Equip_Archi> Zz_Name = new List<Equip_Archi>();
            Zz_Name = PM.Get_Person_Zz((Session["User"] as EquipModel.Entities.Person_Info).Person_Id);            
            List<Object> result = new List<object>();
            List<Notice_A13dot1> list = NM.getNoticeForA13dot1();
            List<object> temp = new List<object>();
            PersonManagment.P_viewModal pv = PM.Get_PersonModal((Session["User"] as EquipModel.Entities.Person_Info).Person_Id);
            if(pv.Role_Names.Contains("现场工程师"))
            { 
                foreach(var tt in Zz_Name)
                {
                    object k = tt.EA_Name;
                    temp.Add(k);
                }
                string aa = JsonConvert.SerializeObject(temp);
            
                foreach(var item in list)
                {
                    if(aa.Contains(item.Zz_Name))
                    { 
                        object o = new 
                        {
                            isSubmit=1,
                            Notice_ID=item.Notice_ID,
                            Notice_EquipCode = item.Notice_EquipCode,
                            Notice_EquipType = item.Notice_EquipType,
                            Notice_State = item.Notice_State,
                            Pq_Name = item.Pq_Name,
                            Zz_Name = item.Zz_Name,
                            Notice_Desc = item.Notice_Desc,
                            Notice_CR_Person = item.Notice_CR_Person,
                            Notice_CR_Date = item.Notice_CR_Date,
                            Notice_EquipSpeciality = item.Notice_Speciality
                        };
                        result.Add(o);
                    }
                }
            }
            else
            {
                foreach (var item in list)
                {
                   
                        object o = new
                        {
                            isSubmit = 0,
                            Notice_ID = item.Notice_ID,
                            Notice_EquipCode = item.Notice_EquipCode,
                            Notice_EquipType = item.Notice_EquipType,
                            Notice_State = item.Notice_State,
                            Pq_Name = item.Pq_Name,
                            Zz_Name = item.Zz_Name,
                            Notice_Desc = item.Notice_Desc,
                            Notice_CR_Person = item.Notice_CR_Person,
                            Notice_CR_Date = item.Notice_CR_Date,
                            Notice_EquipSpeciality = item.Notice_Speciality
                        };
                        result.Add(o);
                    
                }
            }
            string str = JsonConvert.SerializeObject(result);
            return ("{" + "\"data\": " + str + "}");
        }
        /// <summary>
        /// 通过通知单号生成A13dot1的工作流
        /// </summary>
        /// <returns></returns>
        public string CreateA13dot1ByNotice(string Notice_Id)
        {
            
            NoticeManagement NM=new NoticeManagement();
            Notice_A13dot1 a_notice=NM.getNoticeForA13dot1ByNI(Notice_Id);
            EquipManagment em = new EquipManagment();
            Equip_Info eqinfo = em.getEquip_Info(a_notice.Notice_EquipCode);
            List<Equip_Archi> Equip_ZzBelong = em.getEquip_ZzBelong(eqinfo.Equip_Id);
            UI_WorkFlow_Entity wfe = CWFEngine.CreateAWFEntityByName("A13dot1");
            NM.modifyNoticeForA13dot1((Session["User"] as EquipModel.Entities.Person_Info).Person_Name, DateTime.Now.ToString(), Notice_Id);
            if (wfe != null)
            {
                Dictionary<string, string> record = wfe.GetRecordItems();
                record["username"] = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
                record["time"] = DateTime.Now.ToString();
                wfe.Start(record);
                int flow_id = wfe.EntityID;
                Dictionary<string, string> signal1 = new Dictionary<string, string>();
                signal1["start_done"] = "true";
                CWFEngine.SubmitSignal(flow_id, signal1, record);
                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();
                signal["ZzSubmit_done"] = "true";
                signal["Cj_Name"] = Equip_ZzBelong[1].EA_Name;
                signal["Zz_Name"] = a_notice.Zz_Name;
                signal["Equip_GyCode"] =eqinfo.Equip_GyCode;
                signal["Equip_Code"] = a_notice.Notice_EquipCode;
                signal["Equip_Type"] =a_notice.Notice_EquipType;
                signal["Problem_Desc"] = a_notice.Notice_Desc;
                signal["Problem_DescFilePath"] = "";
                signal["Zy_Type"] = eqinfo.Equip_Specialty;
                signal["Zy_SubType"] = eqinfo.Equip_PhaseB;
                signal["Equip_ABCMark"] = eqinfo.Equip_ABCmark;
                signal["Data_Src"] = "由现场工程师通过ERP转入";
                signal["NoticeNum"] = Notice_Id;
                signal["User_State"] = a_notice.Notice_State;

                //submit
                //record                    
                record["username"] = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
                record["time"] = DateTime.Now.ToString();
                //submit
                CWFEngine.SubmitSignal(flow_id, signal, record);
            }
            return ("/A13dot1/Index");
        }
        /// <summary>
        /// 删除通知单
        /// </summary>
        /// <param name="Notice_Id"></param>
        /// <returns></returns>
        public string RemoveA13dot1ByNotice(string Notice_Id)
        {
            NoticeManagement NM = new NoticeManagement();
            NM.removeNoticeForA13dot1(Notice_Id);
            return ("/A13dot1/Index");
        }
        /// <summary>
        /// 用于获取现场工程师已提交的通知单中的未完成通知单
        /// </summary>
        /// <returns></returns>
        public string NoticeUncomp()
        {
            NoticeManagement NM = new NoticeManagement();
            PersonManagment PM = new PersonManagment();            
            List<Object> result = new List<object>();
            List<Notice_A13dot1> list = NM.getNoticeForA13dot1Uncomp();                          
            foreach (var item in list)
            {                  
                if(item.Notice_State!="删除"&&item.Notice_State!="完成")
                { 
                     object o = new
                     {                    
                         Notice_ID = item.Notice_ID,
                         Notice_EquipCode = item.Notice_EquipCode,
                         Notice_EquipType = item.Notice_EquipType,
                         Notice_State = item.Notice_State,
                         Pq_Name = item.Pq_Name,
                         Zz_Name = item.Zz_Name,
                         Notice_Desc = item.Notice_Desc,
                         Notice_CR_Person = item.Notice_CR_Person,
                         Notice_CR_Date = item.Notice_CR_Date,
                         Notice_EquipSpeciality = item.Notice_Speciality
                     };
                     result.Add(o);
                }
            }                    
            string str = JsonConvert.SerializeObject(result);
            return ("{" + "\"data\": " + str + "}");
        }
    }
}
﻿using EquipModel.Entities;
using FlowEngine;
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

using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;

using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using FlowEngine.DAL;
using FlowEngine.Modals;
using FlowEngine.Param;
using WebApp.Models.DateTables;
using System.Collections.Specialized;
using EquipBLL.AdminManagment.MenuConfig;
using System.Xml;
using FlowEngine.TimerManage;

namespace WebApp.Controllers
{
    public class DSA11dot2Controller : CommonController
    {
        EquipManagment Em = new EquipManagment();

        
        //public ActionResult Index(string job_title)
        //{
        //    ViewBag.jobtitle = job_title;
        //    return View();

        //}
        public ActionResult ZzSubmit(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }     

        public class EquipListModel
        {
            public int Equip_Id;
            public string Equip_GyCode;
            public string Equip_Code;
            public string Equip_Type;
            public string Equip_Specialty;
            public string Equip_ABCMark;
        }
        public string get_equip_info(string wfe_id)
        {
            WorkFlows wfsd = new WorkFlows();
            Mission db_miss = wfsd.GetWFEntityMissions(Convert.ToInt16(wfe_id)).Last();//获取该实体最后一个任务        


            WorkFlow_Entity wfe = wfsd.GetWorkFlowEntity(Convert.ToInt16(wfe_id));
            //CWorkFlow wf = new CWorkFlow();
            WorkFlows wfs = new WorkFlows();
            UI_MISSION ui = new UI_MISSION();
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
            List<EquipListModel> EquipList = Zz_Equips(ui.Miss_Params["Zz_Name"].ToString());
            List<object> miss_obj = new List<object>();
            int i = 1;
            foreach (var item in EquipList)
            {
                object m = new
                {
                    index = i,
                    Equip_Id = item.Equip_Id,
                    Equip_GyCode = item.Equip_GyCode,
                    Equip_Code = item.Equip_Code,
                    Equip_Type = item.Equip_Type,
                    Equip_Specialty = item.Equip_Specialty,
                    Equip_ABCMark = item.Equip_ABCMark
                };
                miss_obj.Add(m);
                i++;
            }

            string str = JsonConvert.SerializeObject(miss_obj);
            return ("{" + "\"data\": " + str + "}");


        }

        public List<EquipListModel> Zz_Equips(string Zz_name)
        {
            EquipManagment pm = new EquipManagment();

            EquipArchiManagment em = new EquipArchiManagment();





            List<Equip_Info> Equips_of_Zz = new List<Equip_Info>();
            List<EquipListModel> Equip_obj = new List<EquipListModel>();


            Equips_of_Zz = pm.getEquips_OfZz(em.getEa_idbyname(Zz_name.ToString()));
            foreach (var item in Equips_of_Zz)
            {
                EquipListModel io = new EquipListModel();
                io.Equip_Id = item.Equip_Id;
                io.Equip_GyCode = item.Equip_GyCode;
                io.Equip_Code = item.Equip_Code;
                io.Equip_Type = item.Equip_Type;
                io.Equip_Specialty = item.Equip_Specialty;
                io.Equip_ABCMark = item.Equip_ABCmark;
                Equip_obj.Add(io);
            }

            return Equip_obj;

        }




        //提报页面的保存按钮，根据选择的设备个数创建相应11.2的工作流
        public string CreateA11dot2_submitsignal(string json1)
        {


            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                EquipManagment tm = new EquipManagment();
                string temp = item["sample"].ToString();
                JArray jsonVal = JArray.Parse(temp) as JArray;
                dynamic table2 = jsonVal;
                foreach (dynamic T in table2)
                {

                    //加载原工作流工作流
                    CWorkFlow m_workFlow = new CWorkFlow();
                    WorkFlows wfs = new WorkFlows();
                    WorkFlow_Define wf_define = wfs.GetWorkFlowDefine("A11dot2");               
                    m_workFlow = null;
                    if (wf_define != null)
                    {
                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(Encoding.Default.GetString(wf_define.W_Xml));
                        m_workFlow = new CWorkFlow();
                        m_workFlow.InstFromXmlNode((XmlNode)doc.DocumentElement);
                        m_workFlow.DefineID = wf_define.W_ID;

                    }

                    //加载超时属性数据
                    TimerCreateWFPa TCP = new TimerCreateWFPa();
                    TimerCreateWFPa.TimerSetting time_set = new TimerCreateWFPa.TimerSetting();
                    time_set.Time_start = "wf_create";
                    time_set.Exact_time = "";
                    time_set.Offset_time = (DateTime.Now.AddDays(5) - DateTime.Now).ToString();
                    //time_set.Offset_time = (DateTime.Now.AddMinutes(3) - DateTime.Now).ToString();
                    time_set.Action = "INVILID";
                    time_set.Call_back = "http://localhost/CallBack/testCallBack";
                    TCP.AppendTimer("PqAssess", time_set);

                    //创建写入timeout属性的工作流
                    CWorkFlow wf = new CWorkFlow();

                    wf.InstFromXmlNode(m_workFlow.WriteToXmlNode());

                    //修改定时器
                    foreach (var ti in TCP.wf_timer)
                    {
                        try
                        {
                            DateTime? dt = null;

                            if (ti.Value["ExactTime"] != "")
                                dt = DateTime.Parse(ti.Value["ExactTime"]);
                            wf.events[ti.Key].TimeOutProperty.SetAttribute("exact_time", dt);

                            TimeSpan? ts = null;
                            if (ti.Value["OffsetTime"] != "")
                                ts = TimeSpan.Parse(ti.Value["OffsetTime"]);
                            wf.events[ti.Key].TimeOutProperty.SetAttribute("offset_time", ts);

                            wf.events[ti.Key].TimeOutProperty.SetAttribute("time_start", ti.Value["TimeStart"]);

                            wf.events[ti.Key].TimeOutProperty.SetAttribute("action", ti.Value["Action"]);
                            wf.events[ti.Key].TimeOutProperty.SetAttribute("call_back", ti.Value["CallBack"]);

                        }
                        catch
                        {
                            continue;
                        }
                    }

                    //创建工作流
                    wf.CreateEntityBySelf();
                    //开启工作流
                    Dictionary<string, string> record = new Dictionary<string, string>();
                    record.Add("username", "system_temporary");
                    record.Add("time", DateTime.Now.ToString());
                    TCP.wf_record = record;
                    wf.Start((IDictionary<string, string>)(TCP.wf_record));
                    Dictionary<string, string> signal = new Dictionary<string, string>();
                    signal["currentuser"] = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
                    signal["start_done"] = "true";
                    //完成工作流的设备提报第一步（批量提报）
                   
                    string Equip_Code = T.Equip_Code;
                    int Equip_location_EA_Id = tm.getEA_id_byCode(Equip_Code);
                    
                    signal["Zz_Name"] = tm.getZzName(Equip_location_EA_Id);
                    signal["Equip_GyCode"] = T.Equip_GyCode;
                    signal["Equip_Code"] = T.Equip_Code;
                    signal["Equip_ABCMark"] = T.Equip_ABCMark;

                    //record
                    Dictionary<string, string> record1 = new Dictionary<string, string>();
                    record1["username"] = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
                    record1["time"] = DateTime.Now.ToString();
                    //submit
                    CWFEngine.SubmitSignal(Convert.ToInt32(wf.EntityID), signal, record1);
                }
            }
            catch (Exception e)
            {
                return "";
            }
            return ("/A11dot2/Index");

        }



        //A11dot2临时任务在线编辑部分
        private DtResponse ProcessRequest(List<KeyValuePair<string, string>> data)
        {
            DtResponse dt = new DtResponse();
            var http = DtRequest.HttpData(data);
            var Data = http["data"] as Dictionary<string, object>;
            int wfe_id = -1;
            foreach (var d in Data)
            {
                wfe_id = Convert.ToInt32(d.Key);
            }
            foreach (var d in Data)
            {
                int id = Convert.ToInt32(d.Key);
                foreach (var dd in d.Value as Dictionary<string, object>)
                {
                    Dictionary<string, string> signal = new Dictionary<string, string>();
                    if (dd.Key == "problem_desc")
                    {
                        signal["Problem_Desc"] = dd.Value.ToString();
                        signal["ZzSubmit_done"] = "true";//提报标志

                        //record
                        Dictionary<string, string> record1 = new Dictionary<string, string>();
                        record1["username"] = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
                        record1["time"] = DateTime.Now.ToString();
                        //submit
                        CWFEngine.SubmitSignal(Convert.ToInt32(id), signal, record1);
                    }


                }
            }


            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras["Zz_Name"] = null;
            paras["Equip_GyCode"] = null;
            paras["Equip_Code"] = null;
            paras["Problem_Desc"] = null;//隐患问题描述
            paras["RiskMatrix_Color"] = null;//隐患评估结果
            paras["Supervise_done"] = null;//片区监督实施是否完成
            paras["ImplementPlan_done"] = null;//黄色、措施实施是否完成




            if (wfe_id != -1)
            {
                WorkFlows wfsd = new WorkFlows();
                UI_WFEntity_Info wfei = CWFEngine.GetWorkFlowEntityWithParams(wfe_id, paras);
                Dictionary<string, object> m_kv = new Dictionary<string, object>();
                Mission db_miss = wfsd.GetWFEntityMissions(wfe_id).Last();//获取该实体最后一个任务
                m_kv["index_Id"] = wfe_id;
                m_kv["zz_name"] = paras["Zz_Name"].ToString();
                m_kv["sb_gycode"] = paras["Equip_GyCode"].ToString();
                m_kv["sb_code"] = paras["Equip_Code"].ToString();
                m_kv["problem_desc"] = paras["Problem_Desc"].ToString();
                m_kv["riskMatrix_color"] = paras["RiskMatrix_Color"].ToString();




                m_kv["missionname"] = db_miss.Miss_Desc;
                dt.data.Add(m_kv);

            }
            return dt;
        }
        public JsonResult PostChanges()
        {

            var request = System.Web.HttpContext.Current.Request;
            var list = ParsePostForm(request.Form);
            var dtRes = ProcessRequest(list);


            return Json(dtRes);
        }

        private List<KeyValuePair<string, string>> ParsePostForm(NameValueCollection Form)
        {
            var list = new List<KeyValuePair<string, string>>();
            if (Form != null)
            {
                foreach (var f in Form.AllKeys)
                {
                    list.Add(new KeyValuePair<string, string>(f, Form[f]));
                }
            }
            return list;
        }






        //public ActionResult LSA11dot2(string run_result ,string job_title)
        //{
        //    string s = run_result;
        //    // ViewBag.flows = CWFEngine.ListAllWFDefine();
        //    ViewBag.run_result = run_result;
        //    ViewBag.jobtitle = job_title;
        //    return View();
        //}





    }
}
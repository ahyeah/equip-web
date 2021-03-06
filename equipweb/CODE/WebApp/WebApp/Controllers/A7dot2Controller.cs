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

namespace WebApp.Controllers
{
    public class A7dot2Controller : CommonController
    {
        //
        // GET: /A7dot2/
        public ActionResult Index()
        {
            return View(getIndexListRecords("A7dot2", (Session["User"] as EquipModel.Entities.Person_Info).Person_Name));            
        }
        // GET: /A7dot2/DRBPM系统输入（这里给一个人工提报功能；从DRBPM获取时，这步跳过）
        public ActionResult ZzSubmit(string wfe_id)
        {
            return View(getSubmitModel(wfe_id));
        }

        // GET: /A7dot2/DRBMP平台数据筛选
        public ActionResult DrbpmSelect(string flowname)
        {
            //return View(getA7dot2_Model()); //2016.7.25
            string strTableName = "SbGyAnalysis";
            string strWhere = "1=1";//
            //页面权限，现场工程师能处理
            int cur_PersionID = (Session["User"] as EquipModel.Entities.Person_Info).Person_Id;
            PersonManagment pm = new PersonManagment();
            EquipBLL.AdminManagment.PersonManagment.P_viewModal pv = pm.Get_PersonModal(cur_PersionID);
            ViewBag.user_Role = pv.Role_Names;
            return View(getDRBPM_SbGyAnalysisList(strTableName, strWhere));//2016.7.25 
        }
        // GET: /A7dot2/可靠性工程师确认
        public ActionResult PqConfirm(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }

        // GET: /A7dot2/专业团队审核
        public ActionResult ZytdConfirm(string  wfe_id)
        {         
            return View(getWFDetail_Model(wfe_id));
        }

        // GET: /A7dot2/车间提出改造方案
        public ActionResult CjCreatePlan(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }

        // GET: /A7dot2/跳转到A12.2，工艺变更管理
        public ActionResult JumpToA12dot2(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }

        // GET: /A7dot2/跳转到A4.1，重新选型（设计）
        public ActionResult JumpToA4dot1(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }                      
       
        //GET: /A7dot2/工作流详情
        public ActionResult WorkFolw_Detail(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
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
                signal["Zy_Type"] = item["Zy_Type"].ToString();
                signal["Zy_SubType"] = item["Zy_SubType"].ToString();
                signal["Data_Src"] = "";
                signal["Problem_Desc"] = item["Problem_Desc"].ToString();  
                EquipManagment em = new EquipManagment();
                signal["Equip_ABCMark"] = em.getEquip_Info(item["Equip_Code"].ToString()).Equip_ABCmark;
                //signal["Equip_ABCMark"] = "A";//for test
               
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
            return ("/A7dot2/Index");
        }
        
        //DRBPM平台低能效机泵-筛选-自动提报-2016.07.25
        public string Auto_ZzSubmit_submitsignal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string Equip_Code = item["Equip_Code"].ToString();
                //string Problem_Desc = item["gyState_PMGList"].ToString();
                string flowname = "A7dot2";
                UI_WorkFlow_Entity wfe = CWFEngine.CreateAWFEntityByName(flowname);
                if (wfe != null)
                {
                    EquipManagment em = new EquipManagment();
                    Equip_Info eqinfo = em.getEquip_Info(Equip_Code);
                    List<Equip_Archi> Equip_ZzBelong = em.getEquip_ZzBelong(eqinfo.Equip_Id);
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
                    signal["Cj_Name"] = Equip_ZzBelong[1].EA_Name; //Cj_Name
                    signal["Zz_Name"] = Equip_ZzBelong[0].EA_Name; //Zz_Name
                    signal["Equip_GyCode"] = eqinfo.Equip_GyCode;
                    signal["Equip_Code"] = eqinfo.Equip_Code;
                    signal["Equip_Type"] = eqinfo.Equip_Type;
                    signal["Zy_Type"] = eqinfo.Equip_Specialty;
                    signal["Zy_SubType"] = eqinfo.Equip_PhaseB;
                    signal["Equip_ABCMark"] = eqinfo.Equip_ABCmark;
                    signal["Data_Src"] = "DRBPM平台低能效机泵";
                    signal["Problem_Desc"] = GetGyStateDescription(item["gyState_PMGList"].ToString(), true); 
                    
                    //record                    
                    record["username"] = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
                    record["time"] = DateTime.Now.ToString();
                    //submit
                    CWFEngine.SubmitSignal(flow_id, signal, record);
                    return ("/A7dot2/Index");
                }

            }
            catch (Exception e)
            {
                return "";
            }
            return ("/A7dot2/Index");
        }
        public string PqConfirm_submitsignal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string flowname = item["Flow_Name"].ToString();
                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();
                signal["PqConfirm_Result"] = item["PqConfirm_Result"].ToString();
                signal["PqConfirm_Reason"] = item["PqConfirm_Reason"].ToString();
                //record
                Dictionary<string, string> record = new Dictionary<string, string>();
                record["username"] = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
                record["time"] = DateTime.Now.ToString();
                //submit
                CWFEngine.SubmitSignal(Convert.ToInt32(flowname), signal, record);
            }
            catch (Exception e)
            {
                return "";
            }
            return ("/A7dot2/Index");
        }

        public string ZytdConfirm_submitsignal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string flowname = item["Flow_Name"].ToString();
                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();
                signal["ZytdConfirm_Result"] = item["ZytdConfirm_Result"].ToString();
                signal["ZytdConfirm_Reason"] = item["ZytdConfirm_Reason"].ToString();
                //record
                Dictionary<string, string> record = new Dictionary<string, string>();
                record["username"] = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
                record["time"] = DateTime.Now.ToString();
                //submit
                CWFEngine.SubmitSignal(Convert.ToInt32(flowname), signal, record);
            }
            catch (Exception e)
            {
                return "";
            }
            return ("/A7dot2/Index");
        }

        public string CjCreatePlan_submitsignal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string flowname = item["Flow_Name"].ToString();
                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();
                signal["CjCreatePlan_done"] = "true";                 
                signal["Plan_Desc"] = item["Plan_Desc"].ToString();
                signal["Plan_DescFilePath"] = item["Plan_DescFilePath"].ToString();
                //record
                Dictionary<string, string> record = new Dictionary<string, string>();
                record["username"] = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
                record["time"] = DateTime.Now.ToString();
                //submit
                CWFEngine.SubmitSignal(Convert.ToInt32(flowname), signal, record);
            }
            catch (Exception e)
            {
                return "";
            }
            return ("/A7dot2/Index");
        }

        // GET: /A7dot2/装置申请工艺变更（补充变更方案）
        public ActionResult ZzAddPlan(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }

        public string ZzAddPlan_submitsignal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string flowname = item["Flow_Name"].ToString();
                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();
                signal["ZzAddPlan_done"] = "true";
                signal["GyChangePlan_Desc"] = item["Plan_Desc"].ToString();
                signal["GyChangePlan_DescFilePath"] = item["Plan_DescFilePath"].ToString();
                //record
                Dictionary<string, string> record = new Dictionary<string, string>();
                record["username"] = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
                record["time"] = DateTime.Now.ToString();
                //submit
                CWFEngine.SubmitSignal(Convert.ToInt32(flowname), signal, record);
            }
            catch (Exception e)
            {
                return "";
            }
            return ("/A7dot2/Index");
        }

    }
}
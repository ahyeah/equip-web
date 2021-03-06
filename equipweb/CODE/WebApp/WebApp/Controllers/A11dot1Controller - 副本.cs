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
    public class A11dot1Controller : CommonController
    {
        //
        // GET: /A11dot1/
        public ActionResult Index()
        {
            return View(getIndexListRecords("A11dot1", (Session["User"] as EquipModel.Entities.Person_Info).Person_Name));    
        }

        // GET: /A11dot1/提报
        public ActionResult ZzSubmit(string wfe_id)
        {
            return View(getSubmitModel(wfe_id));
        }
        // GET: /A11dot1/成立风险评估小组
        public ActionResult CreateAssessGroup(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }

        // GET: /A11dot1/危害识别
        public ActionResult RiskRecognition(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }

        // GET: /A11dot1/车间确立管控措施
        public ActionResult CjMngCtlPlan(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }
        // GET: /A11dot1/机动处（专业团队）审核下达
        public ActionResult ZytdConfirm(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }


        // GET: /A11dot1/可靠性工程师审核车间管控措施
        public ActionResult PqConfirm(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }

        // GET: /A11dot1/车间组织监督实施
        public ActionResult CjSupervise(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }

        // GET: /A11dot1/评估小组确立管控措施
        public ActionResult PgxzMngCtlPlan(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }

        // GET: /A11dot1/机动处组织监督实施
        public ActionResult JdcSupervise(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }

        // GET: /A11dot1/确认风险是否可控，并进行风险登记
        public ActionResult RiskAcceptRecord(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }

        // GET: /A8dot2/跳转到[A11.3]风险管控模块
        public ActionResult JumpToA11dot3(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }
        
        //工作流详情
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
                signal["Submit_done"] = "true";
                signal["Cj_Name"] = item["Cj_Name"].ToString();
                signal["Zz_Name"] = item["Zz_Name"].ToString();
                signal["Equip_GyCode"] = item["Equip_GyCode"].ToString();
                signal["Equip_Code"] = item["Equip_Code"].ToString();
                signal["Equip_Type"] = item["Equip_Type"].ToString();
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
            return ("/A11dot1/Index");
        }
        public string CreateAssessGroup_submitsignal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string flowname = item["Flow_Name"].ToString();
                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();
                signal["CreateAssessGroup_done"] = item["CreateAssessGroup_done"].ToString();
                signal["Group_Header"] = item["Group_Header"].ToString();
                signal["Group_Member"] = item["Group_Member"].ToString();
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
            return ("/A11dot1/Index");
        }

               
        public string RiskRecognition_submitsignal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string flowname = item["Flow_Name"].ToString();
                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();
                signal["Risk_Type"] = item["Risk_Type"].ToString();
                signal["RiskRecognition_done"] = item["RiskRecognition_done"].ToString();
                signal["Severity"] = item["Severity"].ToString();
                signal["Probability"] = item["Probability"].ToString();  
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
            return ("/A11dot1/Index");
        }

        public string CjMngCtlPlan_submitsignal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string flowname = item["Flow_Name"].ToString();
                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();
                signal["CreatePlan_done"] = item["CreatePlan_done"].ToString();
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
            return ("/A11dot1/Index");
        }

        public string PqConfirm_submitsignal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string flowname = item["Flow_Name"].ToString();
                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();
                signal["PqConfirm_done"] = item["PqConfirm_done"].ToString();      
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
            return ("/A11dot1/Index");
        }

        public string CjSupervise_submitsignal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string flowname = item["Flow_Name"].ToString();
                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();
                signal["ImplementPlan_done"] = item["ImplementPlan_done"].ToString();
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
            return ("/A11dot1/Index");
        }

        public string PgxzMngCtlPlan_submitsignal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string flowname = item["Flow_Name"].ToString();
                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();
                signal["CreatePlan_done"] = item["CreatePlan_done"].ToString();
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
            return ("/A11dot1/Index");
        }

        public string JdcSupervise_submitsignal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string flowname = item["Flow_Name"].ToString();
                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();
                signal["ImplementPlan_done"] = item["ImplementPlan_done"].ToString();
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
            return ("/A11dot1/Index");
        }

        public string RiskAcceptRecord_submitsignal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string flowname = item["Flow_Name"].ToString();
                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();
                signal["RiskAcceptRecord_done"] = item["RiskAcceptRecord_done"].ToString();
                signal["Risk_IsAcceptable"] = item["Risk_IsAcceptable"].ToString();
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
            return ("/A11dot1/Index");
        }


	}
}
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
    public class A12dot2Controller : CommonController
    {
        //
        // GET: /A12dot2/
        public ActionResult Index()
        {
            return View(getIndexListRecords("A12dot2", (Session["User"] as EquipModel.Entities.Person_Info).Person_Name));            
        }
        // GET: /A12dot2/装置提报
        public ActionResult ZzSubmit(string wfe_id)
        {
            return View(getSubmitModel(wfe_id));
        }
        // GET: /A12dot2/可靠性工程师审核提报
        public ActionResult PqConfirm(string  wfe_id)
        {         
            return View(getWFDetail_Model(wfe_id));
        }
        // GET: /A12dot2/调度处判断能否调整工艺避免变更
        public ActionResult DdcJudge(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }
        // GET: /A12dot2/跳转到A11.1风险评估
        public ActionResult JumpToA11dot1(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }
                       
        // GET: /A12dot2/专业团队审核是否执行
        public ActionResult ZytdConfirm(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }

        // GET: /A12dot2/可靠性工程师确定是否需要设计委托
        public ActionResult PqConfirmDesign(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }

        // GET: /A12dot2/跳转到A4.1设备前期管理
        public ActionResult JumpToA4dot1(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }
        // GET: /A12dot2/跳转到A12.1本体改造
        public ActionResult JumpToA12dot1(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }
        // GET: /A12dot2/跳转到A3.3文件管理
        public ActionResult JumpToA3dot3(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }
        //GET: /A12dot2/工作流详情
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
                //signal["Data_Src"] = item["Data_Src"].ToString();         
                signal["Plan_Desc"] = item["Plan_Desc"].ToString();               
                //string  filename = Path.Combine(Request.MapPath("~/Upload"),item["Plan_DescFilePath"].ToString());
                signal["Plan_DescFilePath"] = item["Plan_DescFilePath"].ToString();
                signal["Zy_Type"] = item["Zy_Type"].ToString();
                signal["Zy_SubType"] = item["Zy_SubType"].ToString();
                EquipManagment em = new EquipManagment();
                signal["Equip_ABCMark"] = em.getEquip_Info(item["Equip_Code"].ToString()).Equip_ABCmark;
                signal["Nature"] = item["Nature"].ToString();
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
            return ("/A12dot2/Index");
        }

        public string DdcJudge_submitsignal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string flowname = item["Flow_Name"].ToString();
                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();
                signal["DdcJudge_Result"] = item["DdcJudge_Result"].ToString();        
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
            return ("/A12dot2/Index");
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
            return ("/A12dot2/Index");
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
            return ("/A12dot2/Index");
        }

        public string PqConfirmDesign_submitsignal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string flowname = item["Flow_Name"].ToString();
                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();

                signal["PqConfirmDesign_Result"] = item["PqConfirmDesign_Result"].ToString();
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
            return ("/A12dot2/Index");
        }

        // GET: /A12dot2/装置申请设备本体改造（补充改造方案）
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
                signal["ModifyPlan_Desc"] = item["Plan_Desc"].ToString();
                signal["ModifyPlan_DescFilePath"] = item["Plan_DescFilePath"].ToString();
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
            return ("/A12dot2/Index");
        }

    }
}
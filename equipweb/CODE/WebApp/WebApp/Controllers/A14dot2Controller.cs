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
    public class A14dot2Controller : CommonController
    {
        //
        // GET: /A14dot2/
        public ActionResult Index()
        {
            return View(getIndexListRecords("A14dot2", (Session["User"] as EquipModel.Entities.Person_Info).Person_Name));            
        }
        // GET: /A14dot2/维修单位填写维保内容
        public ActionResult JxdwSubmit(string wfe_id)
        {
            return View(getSubmitModel(wfe_id));
        }
        // GET: /A14dot2/装置确认
        public ActionResult ZzConfirm(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }
        // GET: /A14dot2/可靠性工程师检查确认 
        public ActionResult PqConfirm(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }

        // GET: /A14dot2/机动处检查确认
        public ActionResult JdcConfirm(string  wfe_id)
        {         
            return View(getWFDetail_Model(wfe_id));
        }
        // GET: /A14dot2/跳转到A3.3
        public ActionResult JumpToA3dot3(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }                     
       
        //GET: /A14dot2/工作流详情
        public ActionResult WorkFolw_Detail(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }
        
        public string JxdwSubmit_submitsignal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string flowname = item["Flow_Name"].ToString();
                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();               
                signal["JxdwSubmit_done"] = "true";               
                signal["Cj_Name"] = item["Cj_Name"].ToString();
                signal["Zz_Name"] = item["Zz_Name"].ToString();              
                signal["Equip_GyCode"] = item["Equip_GyCode"].ToString();
                signal["Equip_Code"] = item["Equip_Code"].ToString();
                signal["Equip_Type"] = item["Equip_Type"].ToString();
                signal["EquipMaintain_Time"] = item["EquipMaintain_Time"].ToString();
                signal["Content_Desc"] = item["Content_Desc"].ToString();               
                //string  filename = Path.Combine(Request.MapPath("~/Upload"),item["Plan_DescFilePath"].ToString());
                signal["Content_File"] = item["Content_File"].ToString();          
                
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
            return ("/A14dot2/Index");
        }

        public string ZzConfirm_submitsignal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string flowname = item["Flow_Name"].ToString();
                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();
                signal["ZzConfirm_done"] = item["ZzConfirm_done"].ToString();               
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
            return ("/A14dot2/Index");
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
            return ("/A14dot2/Index");
        }
        public string JdcConfirm_submitsignal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string flowname = item["Flow_Name"].ToString();
                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();
                signal["JdcConfirm_done"] = item["JdcConfirm_done"].ToString();       
                 
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
            return ("/A14dot2/Index");
        }
    }
}
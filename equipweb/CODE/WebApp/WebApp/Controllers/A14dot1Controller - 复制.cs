using EquipModel.Entities;
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
    public class A14dot1Controller : CommonController
    {
        //
        // GET: /A14dot1/
        public ActionResult Index()
        {
            return View(getIndexListRecords("A14dot1", (Session["User"] as EquipModel.Entities.Person_Info).Person_Name));            
        }
        // GET: /A14dot1/可靠性工程师提报DRBPM计划
        public ActionResult PqSubmitDRBPM(string wfe_id)
        {
            return View(getSubmitModel(wfe_id));
        }
        // GET: /A14dot1/维修单位审核，提出检修方案
        public ActionResult JxdwConfirm(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }

        // GET: /A14dot1/机动处批准
        public ActionResult JdcConfirm(string  wfe_id)
        {         
            return View(getWFDetail_Model(wfe_id));
        }
        // GET: /A14dot1/跳转到A8.2
        public ActionResult JumpToA8dot2(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }
                      
       
        //GET: /A14dot1/工作流详情
        public ActionResult WorkFolw_Detail(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }
        
        public string PqSubmitDRBPM_submitsignal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string flowname = item["Flow_Name"].ToString();
                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();               
                signal["PqSubmitDRBPM_done"] = "true";
                signal["PqConfirm_Result"] = "是"; 
                signal["Cj_Name"] = item["Cj_Name"].ToString();
                signal["Zz_Name"] = item["Zz_Name"].ToString();              
                signal["Equip_GyCode"] = item["Equip_GyCode"].ToString();
                signal["Equip_Code"] = item["Equip_Code"].ToString();
                signal["Equip_Type"] = item["Equip_Type"].ToString();
                signal["Data_Src"] = item["Data_Src"].ToString();         
                signal["Plan_Desc"] = item["Plan_Desc"].ToString();               
                //string  filename = Path.Combine(Request.MapPath("~/Upload"),item["Plan_DescFilePath"].ToString());
                signal["Plan_DescFilePath"] = item["Plan_DescFilePath"].ToString();
                signal["Zy_Type"] = item["Zy_Type"].ToString();
                signal["Zy_SubType"] = item["Zy_SubType"].ToString();                
                
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
            return ("/A14dot1/Index");
        }

        public string JxdwConfirm_submitsignal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string flowname = item["Flow_Name"].ToString();
                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();
                signal["JxdwConfirm_done"] = item["JxdwConfirm_done"].ToString();
                signal["JxdwConfirm_Result"] = item["JxdwConfirm_Result"].ToString();
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
            return ("/A14dot1/Index");
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
                signal["JdcConfirm_Result"] = item["JdcConfirm_Result"].ToString();               
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
            return ("/A14dot1/Index");
        }
    }
}
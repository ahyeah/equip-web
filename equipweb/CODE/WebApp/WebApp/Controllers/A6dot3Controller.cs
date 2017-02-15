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
    public class A6dot3Controller : CommonController
    {
        //
        // GET: /A6dot3/
        public ActionResult Index()
        {
            return View(getIndexListRecords("A6dot3", (Session["User"] as EquipModel.Entities.Person_Info).Person_Name));            
        }
        // GET: /A6dot3/不具备备用条件设备装置提报
        public ActionResult Ineligible_Submit(string wfe_id)
        {
            return View(getSubmitModel(wfe_id));
        }
        // GET: /A6dot3/现场工程师确认
        public ActionResult Xcgcs_Confirm(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }

        // GET: /A6dot3/可靠性工程师确认
        public ActionResult Kkxgcs_Confirm(string  wfe_id)
        {         
            return View(getWFDetail_Model(wfe_id));
        }
        public ActionResult WorkFolw_Detail(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }
        public string Ineligible_submitsignal(string json1)
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
            //    signal["Data_Src"] = "人工提报";       
                signal["Ineligible_Reason"] = item["Ineligible_Reason"].ToString();     
                //record
                Dictionary<string, string> record =  new Dictionary<string, string>();
                record["username"] = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
                record["time"] = DateTime.Now.ToString();
                //submit
                CWFEngine.SubmitSignal(Convert.ToInt32(flowname), signal,record);
                //SubmitDSEventDetails("A6.3", "备用设备管理");
            }
            catch (Exception e)
            {
                return "";
            }
            return ("/A6dot3/Index");
        }


        public string XcgcsConfirm_submitsignal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string flowname = item["Flow_Name"].ToString();
                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();

                signal["Xcgcs_Confirm"] = item["CMConfirmC_Done"].ToString();
           
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
            return ("/A6dot3/Index");
        }

        public string kkxgcsConfirm_submitsignal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string flowname = item["Flow_Name"].ToString();
                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();

                signal["Kkxgcs_Confirm"] = item["KkxgcsConfirm"].ToString();
           
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
            return ("/A6dot3/Index");
        }
    }
}
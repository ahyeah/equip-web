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

using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;

using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;

namespace WebApp.Controllers
{
    public class A4dot1Controller : CommonController
    {
        public ActionResult Index()
        {
            return View(getIndexListRecords("A4dot1", (Session["User"] as EquipModel.Entities.Person_Info).Person_Name));
        }
        public ActionResult WorkFolw_Detail(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }

        public ActionResult Design(string wfe_id)
        {
            //find cj zz namme
            UI_MISSION miss = CWFEngine.GetActiveMission<Person_Info>(int.Parse(wfe_id), ((IObjectContextAdapter)(new EquipWebContext())).ObjectContext);
            //miss.Miss_Params
            ViewBag.flag = 0;
            if (miss.Miss_Params["Cj_Name"].ToString() != "")
            {
                ViewBag.CjName = miss.Miss_Params["Cj_Name"].ToString();
                ViewBag.ZzName = miss.Miss_Params["Zz_Name"].ToString();
                ViewBag.flag = 1;
            }


            //  ViewBag.CjName = "CCCJJJ";
            // ViewBag.ZzName = "ZZZZZZ";
            return View(getSubmitModel(wfe_id));//
        }

        public ActionResult PqConfirm(string wfe_id)
        {

            return View(getWFDetail_Model(wfe_id));
        }
        public ActionResult ZytdConfirm(string wfe_id)
        {

            return View(getWFDetail_Model(wfe_id));
        }
        public ActionResult UploadBlueprint(string wfe_id)
        {

            return View(getWFDetail_Model(wfe_id));
        }
        public ActionResult ZytdConfirm2(string wfe_id)
        {

            return View(getWFDetail_Model(wfe_id));
        }
        public ActionResult OrderForm(string wfe_id)
        {

            return View(getWFDetail_Model(wfe_id));
        }
        public ActionResult ZytdConfirm3(string wfe_id)
        {

            return View(getWFDetail_Model(wfe_id));
        }
        public ActionResult JscSave(string wfe_id)
        {

            return View(getWFDetail_Model(wfe_id));
        }
        public ActionResult WzcConfirm(string wfe_id)
        {

            return View(getWFDetail_Model(wfe_id));
        }

        public string Design_submitsignal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string flowname = item["Flow_Name"].ToString();
                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();
                signal["Design_Done"] = "true";
                signal["Cj_Name"] = item["Cj_Name"].ToString();
                signal["Zz_Name"] = item["Zz_Name"].ToString();
                signal["Client"] = item["Client"].ToString();
                signal["Design_Description"] = item["Design_Description"].ToString();
                signal["Plan_Order"] = item["Plan_Order"].ToString();
                signal["Plan_Name"] = item["Plan_Name"].ToString();
                signal["CM_Department"] = item["CM_Department"].ToString();
                signal["Equip_GyCode"] = "";//业务流水号需要正常工作必须有这一字段？
                signal["start_done"] = "true";
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
            return ("/A4dot1/Index");
        }

        public string PqConfirm_submitsignal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string flowname = item["Flow_Name"].ToString();
                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();
                signal["PqSuggestion"] = item["Pqsuggestion"].ToString();
                signal["PqConfirm_Result"] = item["PqConfirm_Result"].ToString();

                //record
                Dictionary<string, string> record = new Dictionary<string, string>();
                record["username"] = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
                record["time"] = DateTime.Now.ToString();
                //submit
                CWFEngine.SubmitSignal(Convert.ToInt32(flowname), signal, record);
            }
            catch (Exception e)
            { return ""; }
            return ("/A4dot1/Index");
        }
        public string ZytdConfirm_submitsignal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string flowname = item["Flow_Name"].ToString();
                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();
                signal["ZytdSuggestion"] = item["Zytdsuggestion"].ToString();
                signal["ZytdConfirm_Result"] = item["ZytdConfirm_Result"].ToString();

                //record
                Dictionary<string, string> record = new Dictionary<string, string>();
                record["username"] = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
                record["time"] = DateTime.Now.ToString();
                //submit
                CWFEngine.SubmitSignal(Convert.ToInt32(flowname), signal, record);
            }
            catch (Exception e)
            { return ""; }
            return ("/A4dot1/Index");
        }
        public string UploadBlueprint_submitsignal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string flowname = item["Flow_Name"].ToString();
                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();
                signal["Blueprint"] = item["Blueprint"].ToString();

                signal["UploadBlueprint_Done"] = "true";

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
            return ("/A4dot1/Index");
        }
        public string ZytdConfirm2_submitsignal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string flowname = item["Flow_Name"].ToString();
                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();
                signal["ZytdReason"] = item["ZytdReason"].ToString();
                signal["ZytdConfirm2_Result"] = item["ZytdConfirm2_Result"].ToString();

                //record
                Dictionary<string, string> record = new Dictionary<string, string>();
                record["username"] = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
                record["time"] = DateTime.Now.ToString();
                //submit
                CWFEngine.SubmitSignal(Convert.ToInt32(flowname), signal, record);
            }
            catch (Exception e)
            { return ""; }
            return ("/A4dot1/Index");
        }
        public string OrderForm_submitsignal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string flowname = item["Flow_Name"].ToString();
                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();
                signal["Order_Form"] = item["Order_Form"].ToString();
                signal["Fittings_Name"] = item["Fittings_Name"].ToString();
                signal["Fittings_Code"] = item["Fittings_Code"].ToString();
                signal["Equip_GyCode"] = item["Equip_GyCode"].ToString();
                signal["Equip_Code"] = "";
                signal["OrderForm_Done"] = "true";

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
            return ("/A4dot1/Index");
        }
        public string ZytdConfirm3_submitsignal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string flowname = item["Flow_Name"].ToString();
                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();
                signal["ZytdReason2"] = item["ZytdReason2"].ToString();
                signal["ZytdConfirm3_Result"] = item["ZytdConfirm3_Result"].ToString();

                //record
                Dictionary<string, string> record = new Dictionary<string, string>();
                record["username"] = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
                record["time"] = DateTime.Now.ToString();
                //submit
                CWFEngine.SubmitSignal(Convert.ToInt32(flowname), signal, record);
            }
            catch (Exception e)
            { return ""; }
            return ("/A4dot1/Index");
        }
        public string JscSave_submitsignal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string flowname = item["Flow_Name"].ToString();
                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();
                signal["JscSave_Done"] = "true";
               

                //record
                Dictionary<string, string> record = new Dictionary<string, string>();
                record["username"] = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
                record["time"] = DateTime.Now.ToString();
                //submit
                CWFEngine.SubmitSignal(Convert.ToInt32(flowname), signal, record);
            }
            catch (Exception e)
            { return ""; }
            return ("/A4dot1/Index");
        }
        public string WzcConfirm_submitsignal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string flowname = item["Flow_Name"].ToString();
                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();
                signal["WzcConfirm_Done"] = "true";


                //record
                Dictionary<string, string> record = new Dictionary<string, string>();
                record["username"] = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
                record["time"] = DateTime.Now.ToString();
                //submit
                CWFEngine.SubmitSignal(Convert.ToInt32(flowname), signal, record);
            }
            catch (Exception e)
            { return ""; }
            return ("/A4dot1/Index");
        }
    }
}
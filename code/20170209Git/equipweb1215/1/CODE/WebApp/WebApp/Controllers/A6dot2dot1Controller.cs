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
    public class A6dot2dot1Controller : CommonController
    {
        public ActionResult Index()
        {
            return View(getIndexListRecords("A6dot2dot1", (Session["User"] as EquipModel.Entities.Person_Info).Person_Name));
        }
        public ActionResult WorkFolw_Detail(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }
        public ActionResult Submit(string wfe_id)
        {

            return View(getRunhuaSubmitModel(wfe_id));
        }
        public ActionResult ConfirmRectified(string wfe_id)
        {

            return View(getWFDetail_Model(wfe_id));
        }
        public ActionResult PqDirectorConfirm(string wfe_id)
        {

            return View(getWFDetail_Model(wfe_id));
        }
        /// <summary>
        /// 提报不完好润滑间
        /// </summary>
        /// <param name="json1"></param>
        /// <returns></returns>
        public string Submit_submitsignal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string flowname = item["Flow_Name"].ToString();
                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();
                signal["RH_Name"] = item["Cj_Name"].ToString();
                signal["ProblemDesc"] = item["Problem_Desc"].ToString();
                signal["Equip_GyCode"] = "";
                signal["Submit_Done"] = "true";               
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
            return ("/A6dot2dot1/Index");
        }
        /// <summary>
        /// 润滑间确认整改提交
        /// </summary>
        /// <param name="json1"></param>
        /// <returns></returns>
        public string ConfirmRectified_submitsignal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string flowname = item["Flow_Name"].ToString();
                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();
                signal["ConfirmRectified_Done"] = "true";

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
            return ("/A6dot2dot1/Index");
        }
        /// <summary>
        /// 片区主任核实提交
        /// </summary>
        /// <param name="json1"></param>
        /// <returns></returns>
        public string PqDirectorConfirm_submitsignal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string flowname = item["Flow_Name"].ToString();
                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();
                signal["PqDirectorConfirm_Done"] = "true";

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
            return ("/A6dot2dot1/Index");
        }      
    }
}
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
    public class A4dot3Controller : CommonController
    {


       
        
        // GET: /A4dot3/
        public ActionResult Index()
        {
            return View(getIndexListRecords("A4dot3", (Session["User"] as EquipModel.Entities.Person_Info).Person_Name));
        }
        // GET: /A4dot3/工程计划提报，确认工程管理单位、质量管理单位
        public ActionResult PlanSubmit(string wfe_id)
        {
            return View(getSubmitModel(wfe_id));
        }
        // GET: /A4dot3/工程管理单位确认时间
        public ActionResult CMConfirmSchedule(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }
        // GET: /A4dot3/ 工程管理单位三查四定
        public ActionResult CMZySCSD(string wfe_id)
        {
            UI_MISSION miss = CWFEngine.GetActiveMission<Person_Info>(int.Parse(wfe_id), ((IObjectContextAdapter)(new EquipWebContext())).ObjectContext);
            ViewBag.currentMiss = miss;
            return View(getWFDetail_Model(wfe_id));
        }
        // GET: /A4dot3/相关单位确认三查四定
        public ActionResult SCSDConfirm(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }
        // GET: /A4dot3/工程管理单位组织中交
        public ActionResult Zj(string wfe_id)
        {

            UI_MISSION miss = CWFEngine.GetActiveMission<Person_Info>(int.Parse(wfe_id), ((IObjectContextAdapter)(new EquipWebContext())).ObjectContext);
            ViewBag.currentMiss = miss;
            return View(getWFDetail_Model(wfe_id));


        }
        // GET: /A4dot3/相关单位确认中交
        public ActionResult ZjConfirm(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }
        // GET: /A4dot3/确认是否合格
        public ActionResult ZytdConfirmC(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }
        // GET: /A4dot3/工程管理单位通知施工方整改
        public ActionResult CMConfirmC(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }
        // GET: /A4dot3/开箱验收，工程管理组织验收
        public ActionResult YanShou(string wfe_id)
        {
            return View();
        }
      
        // GET: /A4dot3/随机专用工具及配件交车间保留、资料交技术档案室
        public ActionResult CjJsQueRen(string wfe_id)
        {
            return View();
        }
        // GET: /A4dot3/物资处联系供应商补全
        public ActionResult WuZiChu(string wfe_id)
        {
            return View();
        }
        // GET: /A4dot3/施工单位编制施工方案,工程管理单位确认
        public ActionResult shigongPlan(string wfe_id)
        {
            return View();
        }
      
        // GET: /A4dot3/确认是否完工
        public ActionResult IfSuccess(string wfe_id)
        {
            return View();
        }
       
        public ActionResult WorkFolw_Detail(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }


        public string Plan_submitsignal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string flowname = item["Flow_Name"].ToString();
                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();
                signal["PlanSubmit_done"] = "true";
                signal["Cj_Name"] = item["Cj_Name"].ToString();
                signal["Zz_Name"] = item["Zz_Name"].ToString();
                signal["Plan_Order"] = item["Plan_Order"].ToString();
                signal["Plan_Name"] = item["Plan_Name"].ToString();
                signal["Equip_GyCode"] = item["Equip_GyCode"].ToString();
                signal["Equip_Code"] = item["Equip_Code"].ToString();

                signal["CM_Department"] = item["CM_Department"].ToString();
            
             
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
            return ("/A4dot3/Index");
        }

        public string ScheduleTimeSubmit(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string flowname = item["Flow_Name"].ToString();
                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();
                signal["ScheduleConfirm_Done"] = "true";
                signal["ScheduleTime_Begin"] = item["ScheduleTime_Begin"].ToString();
                signal["ScheduleTime_End"] = item["ScheduleTime_End"].ToString();
              //record
                Dictionary<string, string> record = new Dictionary<string, string>();
                record["username"] = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
                record["time"] = DateTime.Now.ToString();
                //submit
                CWFEngine.SubmitSignal(Convert.ToInt32(flowname), signal, record);
            }
            catch (Exception e)
            { return ""; }
            return ("/A4dot3/Index");
        }

        public string CMZySCSD_submitsignal(string json1)
        {
            JObject item = (JObject)JsonConvert.DeserializeObject(json1);
            string flowname = item["Flow_Name"].ToString();
            Dictionary<string, string> signal = new Dictionary<string, string>();
            //new input paras
            signal["SCSD_Time"] = item["SCSD_Time"].ToString();
            signal["CMZySCSD_Done"] = "true";
            signal["SCSD_Place"] = item["SCSD_Place"].ToString();
            signal["SCSD_Speci"] = item["SCSD_Speci"].ToString();


            Dictionary<string, string> record = new Dictionary<string, string>();
            record["username"] = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
            record["time"] = DateTime.Now.ToString();
            CWFEngine.SubmitSignal(Convert.ToInt32(flowname), signal, record);
            return ("/A4dot3/Index");
        }
        public string SCSDConfirm_submitsignal(string json1)
        {
            JObject item = (JObject)JsonConvert.DeserializeObject(json1);
            string flowname = item["Flow_Name"].ToString();
            Dictionary<string, string> signal = new Dictionary<string, string>();
            //new input paras
            signal["SCSDConfirm_Done"] = item["SCSDConfirm_Done"].ToString();
       
            Dictionary<string, string> record = new Dictionary<string, string>();
            record["username"] = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
            record["time"] = DateTime.Now.ToString();
            CWFEngine.SubmitSignal(Convert.ToInt32(flowname), signal, record);
            return ("/A4dot3/Index");
        }


        




        public string ZJ_submitsignal(string json1)
        {
            JObject item = (JObject)JsonConvert.DeserializeObject(json1);
            string flowname = item["Flow_Name"].ToString();
            Dictionary<string, string> signal = new Dictionary<string, string>();
            //new input paras
            signal["Zj_Done"] = "true";
            signal["Zj_Time"] = item["ZJ_Time"].ToString();
            signal["Zj_Place"] = item["ZJ_Place"].ToString();
            Dictionary<string, string> record = new Dictionary<string, string>();
            record["username"] = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
            record["time"] = DateTime.Now.ToString();
            CWFEngine.SubmitSignal(Convert.ToInt32(flowname), signal, record);
            return ("/A4dot3/Index");
        }



       public string ZjConfirm_submitsignal(string json1)
        {
            JObject item = (JObject)JsonConvert.DeserializeObject(json1);
            string flowname = item["Flow_Name"].ToString();
            Dictionary<string, string> signal = new Dictionary<string, string>();
            //new input paras
            signal["ZjConfirm_Done"] = item["ZjConfirm_Done"].ToString();
       
            Dictionary<string, string> record = new Dictionary<string, string>();
            record["username"] = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
            record["time"] = DateTime.Now.ToString();
            CWFEngine.SubmitSignal(Convert.ToInt32(flowname), signal, record);
            return ("/A4dot3/Index");
        }



       public string ZytdConfirmC_submitsignal(string json1)
       {
           JObject item = (JObject)JsonConvert.DeserializeObject(json1);
           string flowname = item["Flow_Name"].ToString();
           Dictionary<string, string> signal = new Dictionary<string, string>();
           //new input paras
           signal["ZytdConfirmC_Result"] = item["ZytdConfirmC_Result"].ToString();
           signal["Equip_Type"] = "无";
           Dictionary<string, string> record = new Dictionary<string, string>();
           record["username"] = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
           record["time"] = DateTime.Now.ToString();
           CWFEngine.SubmitSignal(Convert.ToInt32(flowname), signal, record);
           return ("/A4dot3/Index");
       }



       public string CMConfirmC_submitsignal(string json1)
       {
           JObject item = (JObject)JsonConvert.DeserializeObject(json1);
           string flowname = item["Flow_Name"].ToString();
           Dictionary<string, string> signal = new Dictionary<string, string>();
           //new input paras
           signal["CMConfirmC_Done"] = item["CMConfirmC_Done"].ToString();

           Dictionary<string, string> record = new Dictionary<string, string>();
           record["username"] = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
           record["time"] = DateTime.Now.ToString();
           CWFEngine.SubmitSignal(Convert.ToInt32(flowname), signal, record);
           return ("/A4dot3/Index");
       }







    }
}
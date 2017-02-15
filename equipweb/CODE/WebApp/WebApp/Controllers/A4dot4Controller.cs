using EquipModel.Entities;
using FlowEngine;
using FlowEngine.Modals;
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
using System.Xml;
using FlowEngine.Event;
using FlowEngine.DAL;


namespace WebApp.Controllers
{
    public class A4dot4Controller : CommonController
    {
        
       
       
        //

        // GET: /A4dot4/
        public ActionResult Index()
        {
            return View(getIndexListRecords("A4dot4", (Session["User"] as EquipModel.Entities.Person_Info).Person_Name)); 
           
        }
        // GET: /A4dot4/装置提报
        public ActionResult SySubmit(string wfe_id)
        {
            return View(getSubmitModel(wfe_id));
        }//车间现场工程师编写设备试运方案

        public ActionResult WriteSyPlan(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }//编写设备试运方案

        public ActionResult ZytdConfirmPlan(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }//专业团队组织审核方案
       

        public ActionResult ZzSy(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }//现场工程师组织设备试运

        public ActionResult SyReform(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }//设备试运整改确认

        public ActionResult PqConfirmSy(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }
        //可靠性工程师组织设备试运

       
        public ActionResult CMUploadSy(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }
        //工程管理单位上传试运记录

        public ActionResult ZytdConfirmSyRecord(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }//  专业团队审核试运记录

        public ActionResult CMUpload(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }//工程单位
        public ActionResult JscConfirm(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }//技术处确认竣工资料
        //erp进入
        public ActionResult ERP(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }



        public ActionResult JumpToA3dot3(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }



        public ActionResult WorkFolw_Detail(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }

        // GET: /A4dot4/跳转到其它模块--仅用于测试
        /*       public ActionResult JumpToAx(string wfe_id)
               {       
                   UI_MISSION miModel = CWFEngine.GetActiveMission<Person_Info>(int.Parse(wfe_id), ((IObjectContextAdapter)(new EquipWebContext())).ObjectContext);
                   ViewBag.MODULE_NAME = miModel.Miss_Params["temp_ModuleName"].ToString();//根据可靠性工程师分类结果，得到跳转的模块名，并传参数
                   return View(getWFDetail_Model(wfe_id));
               }
       */

        //  人工提报
        public string SySubmit_signal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string flowname = item["Flow_Name"].ToString();
                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();
                //     signal["submit_user"] = item["submit_user"].ToString();
                signal["SySubmit_Done"] = "true";
                signal["Plan_Name"] = item["Plan_Name"].ToString();
                signal["Plan_Order"] = item["Plan_Order"].ToString();
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
            return ("/A4dot4/Index");
        }
        //  设备试运方案
        public string WriteSyPlan_signal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string flowname = item["Flow_Name"].ToString();
                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();
                //     signal["submit_user"] = item["submit_user"].ToString();
                signal["WriteSyPlan_Done"] = "true";
                signal["SyPlan"] = item["SyPlan"].ToString();
               signal["SyPlan_File"] = item["SyPlan_File"].ToString();

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
            return ("/A4dot4/Index");
        }


        //试运方案
        public string ZytdConfirmPlan_signal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string flowname = item["Flow_Name"].ToString();
                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();
                signal["ZytdConfirmPlan_Result"] = item["ZytdConfirmPlan_Result"].ToString();
                //signal["SyfaConfirm_done"] = "true";

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
            return ("/A4dot4/Index");
        }

        // 试运条件
        public string ZzSy_signal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string flowname = item["Flow_Name"].ToString();
                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();
                //     signal["Confirm_result"] = item["Confirm_result"].ToString();
              //  signal["ZzSy_Done"] = "true";
                signal["ZzSy_Done"] = item["ZzSy_Done"].ToString();
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
            return ("/A4dot4/Index");
        }
        // 试运结果
        public string PqConfirmSy_signal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string flowname = item["Flow_Name"].ToString();
                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();
                signal["PqConfirmSy_Result"] = item["PqConfirmSy_Result"].ToString();
               

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
            return ("/A4dot4/Index");
        }
        //  设备试运整改
        public string SyReform_signal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string flowname = item["Flow_Name"].ToString();
                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();
                signal["SyReform_Done"] = item["SyReform_Done"].ToString();
              //  signal["SyReform_Done"] = "true";
                ////DangerType_isgreen:需要根据逻辑判断
                //UI_MISSION miModel = CWFEngine.GetActiveMission<Person_Info>(int.Parse(flowname), ((IObjectContextAdapter)(new EquipWebContext())).ObjectContext);
                //RiskMatrixElement rme = riskMatrixAnalysis(miModel.Miss_Params["Danger_Intensity"].ToString(), miModel.Miss_Params["Time_Level"].ToString());
                //signal["DangerType_isgreen"] = rme.DangerType_isgreen;
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
            return ("/A4dot4/Index");
        }

        //  上传试运记录
        public string CMUploadSy_signal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string flowname = item["Flow_Name"].ToString();
                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();
                //     signal["submit_user"] = item["submit_user"].ToString();
                signal["UploadSy_Done"] = "true";
                signal["SyRecord_File"] = item["SyRecord_File"].ToString();
               

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
            return ("/A4dot4/Index");
        }
 

        //  专业团队审核试运记录
        public string ZytdConfirmSyRecord_signal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string flowname = item["Flow_Name"].ToString();
                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();
                signal["ZytdConfirmSyRecord_Result"] = item["ZytdConfirmSyRecord_Result"].ToString();
                //   signal["ZytdConfirmSyRecord_Result"] = "true";   
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
            return ("/A4dot4/Index");
        }
        //  工程管理单位上传竣工资料
        public string CMUpload_signal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string flowname = item["Flow_Name"].ToString();
                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();
                //     signal["submit_user"] = item["submit_user"].ToString();
                signal["CMUpload_Done"] = "true";

                signal["Js_File"] = item["Js_File"].ToString();

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
            return ("/A4dot4/Index");
        }
        //技术处确认存档
        public string JscConfirm_signal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string flowname = item["Flow_Name"].ToString();
                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();
                signal["JscConfirm_Result"] = item["JscConfirm_Result"].ToString();
              //  signal["JscConfirm_done"] = "true";
                ////DangerType_isgreen:需要根据逻辑判断
                //UI_MISSION miModel = CWFEngine.GetActiveMission<Person_Info>(int.Parse(flowname), ((IObjectContextAdapter)(new EquipWebContext())).ObjectContext);
                //RiskMatrixElement rme = riskMatrixAnalysis(miModel.Miss_Params["Danger_Intensity"].ToString(), miModel.Miss_Params["Time_Level"].ToString());
                //signal["DangerType_isgreen"] = rme.DangerType_isgreen;
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
            return ("/A4dot4/Index");
        }
        public string JumpToA3dot3_submitsignal(string json1)
        {


            JObject item = (JObject)JsonConvert.DeserializeObject(json1);
            string flowname = item["Flow_Name"].ToString();
            //paras
            Dictionary<string, string> signal = new Dictionary<string, string>();
            string mname = item["module_name"].ToString();
              signal["JumpToA3dot3_Done"] = item["JumpToA3dot3_Done"].ToString();
            //record
            Dictionary<string, string> record = new Dictionary<string, string>();
            record["username"] = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
            record["time"] = DateTime.Now.ToString();
            //submit
            CWFEngine.SubmitSignal(Convert.ToInt32(flowname), signal, record);


            return ("/A4dot4/Index");
        }

    }
}
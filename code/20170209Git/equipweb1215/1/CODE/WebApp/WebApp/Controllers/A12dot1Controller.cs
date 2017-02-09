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
using System.Threading;

namespace WebApp.Controllers
{
    public class A12dot1Controller : CommonController
    {
        //
        // GET: /A12dot1/
        public ActionResult Index()
        {
            return View(getIndexListRecords("A12dot1", (Session["User"] as EquipModel.Entities.Person_Info).Person_Name));            
        }
        // GET: /A12dot1/装置提报
        public ActionResult ZzSubmit(string wfe_id)
        {
            return View(getSubmitModel(wfe_id));
        }
        // GET: /A12dot1/维修单位给出具体建议
        public ActionResult JxdwAdvise(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }

        // GET: /A12dot1/可靠性工程师审核提报
        public ActionResult PqConfirm(string  wfe_id)
        {         
            return View(getWFDetail_Model(wfe_id));
        }
        // GET: /A12dot1/跳转到A11.1风险评估
        public ActionResult JumpToA11dot1(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }
                       
        // GET: /A12dot1/专业团队审核是否执行
        public ActionResult ZytdConfirm(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }

        // GET: /A12dot1/专业团队确认是否上报厂部领导
        public ActionResult ZytdConfirmToLeader(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }

        // GET: /A12dot1/厂部领导审核是否执行
        public ActionResult LeaderConfirm(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }

        //GET: /A12dot1/工作流详情
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
                signal["Data_Src"] = "设备本体改造";         
                signal["Plan_Desc"] = item["Plan_Desc"].ToString();               
                //string  filename = Path.Combine(Request.MapPath("~/Upload"),item["Plan_DescFilePath"].ToString());
                signal["Plan_DescFilePath"] = item["Plan_DescFilePath"].ToString();
                signal["Zy_Type"] = item["Zy_Type"].ToString();
                signal["Zy_SubType"] = item["Zy_SubType"].ToString();
                EquipManagment em = new EquipManagment();
                signal["Equip_ABCMark"] = em.getEquip_Info(item["Equip_Code"].ToString()).Equip_ABCmark;
               
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
            return ("/A12dot1/Index");
        }

        public string JxdwAdvise_submitsignal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string flowname = item["Flow_Name"].ToString();
                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();
                signal["JxdwAdvise_done"] = item["JxdwAdvise_done"].ToString();
                signal["JxdwAdvise_Desc"] = item["JxdwAdvise_Desc"].ToString();
                signal["JxdwAdvise_File"] = item["JxdwAdvise_File"].ToString();
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
            return ("/A12dot1/Index");
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
            return ("/A12dot1/Index");
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
            return ("/A12dot1/Index");
        }

        public string ZytdConfirmToLeader_submitsignal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string flowname = item["Flow_Name"].ToString();
                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();

                signal["ZytdConfirmToLeader_Result"] = item["ZytdConfirmToLeader_Result"].ToString();
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
            return ("/A12dot1/Index");
        }

        public string LeaderConfirm_submitsignal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string flowname = item["Flow_Name"].ToString();
                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();

                signal["LeaderConfirm_Result"] = item["LeaderConfirm_Result"].ToString();
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
            return ("/A12dot1/Index");
        }
        public static void PushWorkFlowRun(object wfe_id)
        {
            //等待sendtoDRBPMsystem函数返回
            Thread.Sleep(2000);

            int i_wfe_id = Convert.ToInt32(wfe_id);
            CWFEngine.SubmitSignal(i_wfe_id, new Dictionary<string, string>());
        }
        //  sendToDRBPMsystem
        public string sendToDRBPMsystem(string param)
        {
            JObject item = (JObject)JsonConvert.DeserializeObject(param);
            string wfe_id = item["wfe_id"].ToString();
            //======================================2016.7.22=======================================
            //DRBPM预防性计划 : writeDRBPM(string sbcode,string reason, bool isUrgent)

            writeDRBPM(item["E_Code"].ToString(), "设备本体改造", false); // write to drbpm
            //======================================================================================
            Thread th1 = new Thread(new ParameterizedThreadStart(PushWorkFlowRun));
            th1.Start(wfe_id);
            return "";
        }

    }
}
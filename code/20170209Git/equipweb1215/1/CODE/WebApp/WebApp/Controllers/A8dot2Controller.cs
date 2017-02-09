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
using FlowEngine.Param;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using FlowEngine.DAL;
using FlowEngine.Modals;

namespace WebApp.Controllers
{
    public class A8dot2Controller : CommonController
    {
        //
        // GET: /A8dot2/
        public ActionResult A8()
        {
            return View();
        }
        public ActionResult Index()
        {
            return View(getIndexListRecords("A8dot2", (Session["User"] as EquipModel.Entities.Person_Info).Person_Name));    
        }

        // GET: /A8dot2/检修单位提报（直接从DRBPM当月计划中提取后确认提交！）
        public ActionResult JxSubmit(string wfe_id)
        {
            return View(getSubmitModel(wfe_id));
        }

        public ActionResult JxdwCreateOrder (string wfe_id)
        {
            Dictionary<string, object> paras1 = new Dictionary<string, object>();
            paras1["Job_Name"] = null;           
            paras1["Job_Order"] = null;
            paras1["Data_Src"] = null;
            UI_WFEntity_Info wfei = CWFEngine.GetWorkFlowEntityWithParams(Convert.ToInt32(wfe_id), paras1);
            ViewBag.GD_Id = paras1["Job_Order"].ToString();
            ViewBag.Plan_Name = paras1["Job_Name"].ToString();
            ViewBag.Data_Src = paras1["Data_Src"].ToString();
            ERPInfoManagement erp = new ERPInfoManagement();
            GD_InfoModal res = erp.getGD_Modal_GDId(paras1["Job_Order"].ToString());
            if (res != null)
                ViewBag.GD_State = res.GD_State;
            return View(getWFDetail_Model(wfe_id));
        }
        public ActionResult ZzConfirmJobOrder(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }
        public ActionResult ZytdConfirm1(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }
        public ActionResult PqConfirm1(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }
        public ActionResult JhkConfirm2(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }
        public ActionResult CaiGouConfirm(string wfe_id)
        {
            Dictionary<string, object> paras1 = new Dictionary<string, object>();
            paras1["Job_OrderState"] = null;
           
            UI_WFEntity_Info wfei = CWFEngine.GetWorkFlowEntityWithParams(Convert.ToInt32(wfe_id), paras1);
            ViewBag.Job_OrderState = paras1["Job_OrderState"].ToString();
            int cur_PersionID = (Session["User"] as EquipModel.Entities.Person_Info).Person_Id;
            PersonManagment pm = new PersonManagment();
            EquipBLL.AdminManagment.PersonManagment.P_viewModal pv = pm.Get_PersonModal(cur_PersionID);
            ViewBag.user_Role = pv.Role_Names;
            ViewBag.user_Depart = pv.Department_Name;
            UI_MISSION miss = CWFEngine.GetActiveMission<Person_Info>(int.Parse(wfe_id), ((IObjectContextAdapter)(new EquipWebContext())).ObjectContext);
            ViewBag.currentMiss = miss;
            return View(getWFDetail_Model(wfe_id));
        }
        public ActionResult WzcCaiGou(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }
        public ActionResult WzcNoticeJxdw(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }
        public ActionResult UploadJxPlan(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }
        public ActionResult WriteJxContent(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }
        public ActionResult ZytdConfirmPlan(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }
        public ActionResult PqConfirmContent(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }
        public ActionResult ZzConfirmPlan(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }
        public ActionResult ZzConfirmCondition(string wfe_id)
        {
            UI_MISSION miss = CWFEngine.GetActiveMission<Person_Info>(int.Parse(wfe_id), ((IObjectContextAdapter)(new EquipWebContext())).ObjectContext);
            ViewBag.currentMiss = miss;
            return View(getWFDetail_Model(wfe_id));
        }
        public ActionResult UploadZjRecord(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }
        public ActionResult UploadBCdot17(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }
        public ActionResult ZzPqZytdConfirm(string wfe_id)
        {
            int cur_PersionID = (Session["User"] as EquipModel.Entities.Person_Info).Person_Id;
            PersonManagment pm = new PersonManagment();
            EquipBLL.AdminManagment.PersonManagment.P_viewModal pv = pm.Get_PersonModal(cur_PersionID);
            ViewBag.user_Role = pv.Role_Names;
            UI_MISSION miss = CWFEngine.GetActiveMission<Person_Info>(int.Parse(wfe_id), ((IObjectContextAdapter)(new EquipWebContext())).ObjectContext);
            ViewBag.currentMiss = miss;
            return View(getWFDetail_Model(wfe_id));
        }
        public ActionResult UploadTryRunRecord(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }
        public ActionResult ZzPqConfirm(string wfe_id)
        {
            int cur_PersionID = (Session["User"] as EquipModel.Entities.Person_Info).Person_Id;
            PersonManagment pm = new PersonManagment();
            EquipBLL.AdminManagment.PersonManagment.P_viewModal pv = pm.Get_PersonModal(cur_PersionID);
            ViewBag.user_Role = pv.Role_Names;
            UI_MISSION miss = CWFEngine.GetActiveMission<Person_Info>(int.Parse(wfe_id), ((IObjectContextAdapter)(new EquipWebContext())).ObjectContext);
            ViewBag.currentMiss = miss;
            return View(getWFDetail_Model(wfe_id));
        }
        public ActionResult JxdwConfirmEnd(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }


        public string JxSubmit_submitsignal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string flowname = item["Flow_Name"].ToString();
                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();
                signal["JxSubmit_done"] = "true";
                signal["Cj_Name"] = item["Cj_Name"].ToString();
                signal["Zz_Name"] = item["Zz_Name"].ToString();
                signal["Equip_GyCode"] = item["Equip_GyCode"].ToString();
                signal["Equip_Code"] = item["Equip_Code"].ToString();
                signal["Equip_Type"] = item["Equip_Type"].ToString();
                signal["Jx_Reason"] = item["Jx_Reason"].ToString();
                //signal["Data_Src"] = item["Data_Src"].ToString();                
                signal["Zy_Type"] = item["Zy_Type"].ToString();
                signal["Zy_SubType"] = item["Zy_SubType"].ToString();
                EquipManagment em = new EquipManagment();
                signal["Equip_ABCMark"] = em.getEquip_Info(item["Equip_Code"].ToString()).Equip_ABCmark;
                signal["Job_Name"] = "";
                signal["Job_Order"] = "";   
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
            return ("/A8dot2/Index");
        }
       
        public string JxdwCreateOrder_submitsignal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                //JObject item2 = (JObject)JsonConvert.DeserializeObject(item["plan_data"].ToString());
                string flowname = item["Flow_Name"].ToString();
                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();
                //signal["Job_Order"] = item2["Plan_num"].ToString();
                signal["Job_Name"] = item["Job_Name"].ToString();
                signal["Job_Order"] = item["Job_Order"].ToString();
                ERPInfoManagement erp = new ERPInfoManagement();
                GD_InfoModal res = erp.getGD_Modal_GDId("00"+item["Job_Order"].ToString());

                signal["Job_OrderState"] = res.GD_State;
                //signal["Job_OrderState"] = item["Job_OrderState"].ToString();
                //signal["job_Name"] = item2["Plan_name"].ToString();
                signal["ZjGxIsOK"] = "是";
                //signal["Equip_GyCode"] = "8";
                //signal["Equip_Code"] = "7";
                //signal["Equip_ABCMark"] = "A"; 
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
            return ("/A8dot2/Index");
        }
        public string ZzConfirmJobOrder_submitsignal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string flowname = item["Flow_Name"].ToString();
                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();
                signal["ZzConfirmJobOrder_done"] = "是";
                signal["Job_OrderState"] = "待审";//for test
                //signal["faulty_intensity"] = item["faulty_intensity"].ToString();
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
            return ("/A8dot2/Index");
        }
        public string ZytdConfirm1_submitsignal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string flowname = item["Flow_Name"].ToString();

                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();
                signal["ZytdConfirm1_done"] = "是";
                
              //  signal["faulty_intensity"] = item["faulty_intensity"].ToString();
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
            return ("/A8dot2/Index");
        }
        public string PqConfirm1_submitsignal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string flowname = item["Flow_Name"].ToString();
                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();
                signal["PqConfirm1_done"] = "是";
             //   signal["faulty_intensity"] = item["faulty_intensity"].ToString();
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
            return ("/A8dot2/Index");
        }
        public string JhkConfirm2_submitsignal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string flowname = item["Flow_Name"].ToString();
                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();
                signal["JhkConfirm2_done"] = "是";
                signal["Job_OrderState"] = "下达";
              //  signal["faulty_intensity"] = item["faulty_intensity"].ToString();
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
            return ("/A8dot2/Index");
        }
        public string CaiGouConfirm_submitsignal(string json1)//否否走不通
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string flowname = item["Flow_Name"].ToString();
                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();
                
                signal["IsCaiGou_Wzc"] = item["IsCaiGou_Wzc"].ToString();
                signal["IsCaiGou_Jxdw"] = item["IsCaiGou_Jxdw"].ToString();
                //  signal["faulty_intensity"] = item["faulty_intensity"].ToString();
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
            return ("/A8dot2/Index");
        }
        public string WzcCaiGou_submitsignal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string flowname = item["Flow_Name"].ToString();
                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();
                signal["Daohuo_Time"] = item["Daohuo_Time"].ToString();
                signal["IsDaohuo_All"] = "Unconfirmed";
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
            return ("/A8dot2/Index");
        }
        public string WzcNoticeJxdw_submitsignal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string flowname = item["Flow_Name"].ToString();
                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();
                signal["IsDaohuo_All"] = "是";
             //   signal["faulty_intensity"] = item["faulty_intensity"].ToString();
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
            return ("/A8dot2/Index");
        }
        public string UploadJxPlan_submitsignal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string flowname = item["Flow_Name"].ToString();
                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();
                signal["Plan_DescFilePath"] = item["Plan_DescFilePath"].ToString();
                signal["UploadJxPlan_done"] = "true";
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
            return ("/A8dot2/Index");
        }
        public string WriteJxContent_submitsignal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string flowname = item["Flow_Name"].ToString();
                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();
                signal["Jx_Content"] = item["Jx_Content"].ToString();
                signal["Job_Guidebook"] = item["Job_Guidebook"].ToString();
                signal["WriteJxContent_done"] = "true";
                
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
            return ("/A8dot2/Index");
        }
        public string ZytdConfirmPlan_submitsignal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string flowname = item["Flow_Name"].ToString();
                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();
                signal["ZytdConfirmPlan_Result"] = item["ZytdConfirmPlan_Result"].ToString(); 
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
            return ("/A8dot2/Index");
        }
        public string PqConfirmContent_submitsignal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string flowname = item["Flow_Name"].ToString();
                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();
                signal["PqConfirmContent_Result"] = item["PqConfirmContent_Result"].ToString();
             //   signal["faulty_intensity"] = item["faulty_intensity"].ToString();
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
            return ("/A8dot2/Index");
        }

        public string ZzConfirmPlan_submitsignal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string flowname = item["Flow_Name"].ToString();
                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();
                signal["ZzConfirmPlan_Result"] = item["ZzConfirmPlan_Result"].ToString(); ;
                signal["Delay_Reason"] = item["Delay_Reason"].ToString();
             //   signal["faulty_intensity"] = item["faulty_intensity"].ToString();
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
            return ("/A8dot2/Index");
        }
        public string ZzConfirmCondition_submitsignal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string flowname = item["Flow_Name"].ToString();
                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();
                signal["ZzConfirmCondition_Result"] = item["ZzConfirmCondition_Result"].ToString();
                
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
            return ("/A8dot2/Index");
        }
        public string UploadZjRecord_submitsignal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string flowname = item["Flow_Name"].ToString();
                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();
                signal["ZjRecord_File"] = item["ZjRecord_File"].ToString();
               
                signal["UploadZjRecord_done"] = "true";

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
            return ("/A8dot2/Index");
        }

        public string ZzPqZytdConfirm_submitsignal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string flowname = item["Flow_Name"].ToString();
                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();
                signal["ZzConfirmZjRecord_Result"] = item["ZzConfirmZjRecord_Result"].ToString();
                signal["PqConfirmZjRecord_Result"] = item["PqConfirmZjRecord_Result"].ToString();
                signal["ZytdConfirmZjRecord_Result"] = item["ZytdConfirmZjRecord_Result"].ToString();

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
            return ("/A8dot2/Index");
        }
        public string ZzPqConfirm_submitsignal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string flowname = item["Flow_Name"].ToString();
                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();
                signal["ZzConfirmZjRecord_Result"] = item["ZzConfirmZjRecord_Result"].ToString();
                signal["PqConfirmZjRecord_Result"] = item["PqConfirmZjRecord_Result"].ToString();
               
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
            return ("/A8dot2/Index");
        }
        public string UploadTryRunRecord_submitsignal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string flowname = item["Flow_Name"].ToString();
                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();
                signal["TryRunRecord_ConfirmResult"] = "是";
                signal["TryRunRecord_File"] = item["TryRunRecord_File"].ToString();
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
            return ("/A8dot2/Index");
        }
        public string JxdwConfirmEnd_submitsignal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string flowname = item["Flow_Name"].ToString();
                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();
                signal["JxdwConfirmEnd_done"] = "true";
                signal["JobFinish_File"] = item["JobFinish_File"].ToString();
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
            return ("/A8dot2/Index");
        }
        public ActionResult WorkFolw_Detail(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }
        public ActionResult WorkFolw_DetailforA8 (string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }
        public class Gxqmodel
        {
            public string WE_Ser;
            public int WE_Id;
        }
        public string A8ActiveList(string WorkFlow_Name)
        {
            string username = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;


            string WE_Status = "0";
            string query_list = "distinct E.WE_Ser, E.WE_Id, R.username";
            string query_condition = "E.W_Name='" + WorkFlow_Name + "' and E.WE_Status='" + WE_Status + "' and R.username is not null";
            string record_filter = "username is not null";
            DataTable dt = CWFEngine.QueryAllInformation(query_list, query_condition, record_filter);
            List<Gxqmodel> Gmlist = new List<Gxqmodel>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Gxqmodel Gm = new Gxqmodel();
                Gm.WE_Id = Convert.ToInt16(dt.Rows[i]["WE_Id"]);
                Gm.WE_Ser = dt.Rows[i]["WE_Ser"].ToString();
                Gmlist.Add(Gm);

            }
            List<A8Model> Hm = new List<A8Model>();
            ERPInfoManagement erp = new ERPInfoManagement();

            foreach (var item in Gmlist)
            {
                A8Model h = new A8Model();

                Dictionary<string, object> paras = new Dictionary<string, object>();
                paras["Equip_GyCode"] = null;
                paras["Job_Order"] = null;//通过工单找通知单号
                paras["Zz_Name"] = null;
                paras["Job_Name"] = null;//计划名称

                UI_WFEntity_Info wfei = CWFEngine.GetWorkFlowEntityWithParams(item.WE_Id, paras);
                h.zz_name = paras["Zz_Name"].ToString();
                h.sb_gycode = paras["Equip_GyCode"].ToString();
                h.job_order = paras["Job_Order"].ToString();
                h.plan_name = paras["Job_Name"].ToString();
                GD_InfoModal res = erp.getGD_Modal_GDId(paras["Job_Order"].ToString());
                if (res != null)
                    h.notice_order = res.GD_Notice_Id;
                h.gd_state = "检修中";
                WorkFlows wfsd = new WorkFlows();
                Mission db_missA8dot2 = wfsd.GetWFEntityMissions(item.WE_Id).Last();
                //写if而不写else if是因为13.1和8.2是断开的，跳转8.2仍满足db_miss.Miss_Desc == "专业团队审核"
                if (db_missA8dot2.Miss_Desc == "检修单位按计划建立工单，完善工序、组件")
                {
                    if(res!=null)
                    h.gd_state = res.GD_State;
                }
                else if (db_missA8dot2.Miss_Desc == "检修计划提报")
                {
                    if (res != null)
                    h.gd_state = res.GD_State;
                }
                else if (db_missA8dot2.Miss_Desc == "现场工程师审核工单")
                {
                    if (res != null)
                    h.gd_state = res.GD_State;
                }
                else if (db_missA8dot2.Miss_Desc == "专业团队审1")
                {
                    if (res != null)
                    h.gd_state = res.GD_State;
                }
                else if (db_missA8dot2.Miss_Desc == "可靠性工程师审1")
                {
                    if (res != null)
                    h.gd_state = res.GD_State;
                }
                else if (db_missA8dot2.Miss_Desc == "机动处计划科审2")
                {
                    if (res != null)
                    h.gd_state = res.GD_State;
                }
                else if (db_missA8dot2.Miss_Desc == "物资处采购，填写到货时间")
                {
                    h.gd_state = "物资采购中";
                }
                else if (db_missA8dot2.Miss_Desc == "物资处确认到货并通知检修单位")
                {
                    h.gd_state = "物资已到货";
                }
                else if (db_missA8dot2.Miss_Desc == "检修单位上传检修方案" || db_missA8dot2.Miss_Desc == "专业团队审批" || db_missA8dot2.Miss_Desc == "检修单位填写检修内容及关键工序，关联作业指导书" || db_missA8dot2.Miss_Desc == "可靠性工程师审批")
                {
                    h.gd_state = "检修方案制定与审判";
                }
                else if (db_missA8dot2.Miss_Desc == "现场工程师确认是否可实施计划")
                {
                    UI_MISSION ui = new UI_MISSION();
                    List<Mission_Param> mis_pars = wfsd.GetMissParams(db_missA8dot2.Miss_Id);//获取当前任务参数
                    foreach (var par in mis_pars)
                    {
                        CParam cp = new CParam();
                        ui.Miss_Params[cp.name] = cp.value;
                    }
                    if (ui.Miss_Params["ZzConfirmPlan_Result"].ToString() == "是")
                    {
                        h.gd_state = "检修计划实施中";
                    }
                    else if (ui.Miss_Params["ZzConfirmPlan_Result"].ToString() == "否")
                    {
                        h.gd_state = "检修计划延期";
                    }
                }
                else if (db_missA8dot2.Miss_Desc == "检修单位确认施工完毕，上传交工资料")
                {
                    h.gd_state = "处理完成";

                }
                h.Data_Src = "待定";
                h.detail = "待定";
                Hm.Add(h);
            }
            List<object> or = new List<object>();
            for (int i = 0; i < Hm.Count; i++)
            {
                object o = new
                {
                    index = i + 1,
                    equip_gycode = Hm[i].sb_gycode,
                    job_order = Hm[i].job_order,
                    notice_order = Hm[i].notice_order,
                    gd_state = Hm[i].gd_state,
                    datasrc = Hm[i].Data_Src,
                    detail = Hm[i].detail,
                    zzname = Hm[i].zz_name,
                    planname = Hm[i].plan_name
                };
                or.Add(o);

            }
            string str = JsonConvert.SerializeObject(or);
            return ("{" + "\"data\": " + str + "}");
        }
        //public string A8ActiveList(string WorkFlow_Name)
        //{
        //    string username = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
            
           
        //    string WE_Status = "0";
        //    string query_list = "distinct E.WE_Ser, E.WE_Id, R.username";
        //    string query_condition = "E.W_Name='" + WorkFlow_Name + "' and E.WE_Status='" + WE_Status + "' and R.username is not null";
        //    string record_filter = "username is not null";
        //    DataTable dt = CWFEngine.QueryAllInformation(query_list, query_condition, record_filter);
        //    List<Gxqmodel> Gmlist = new List<Gxqmodel>();
        //    for (int i = 0; i < dt.Rows.Count; i++)
        //    {
        //        Gxqmodel Gm = new Gxqmodel();
        //        Gm.WE_Id = Convert.ToInt16(dt.Rows[i]["WE_Id"]);
        //        Gm.WE_Ser = dt.Rows[i]["WE_Ser"].ToString();
        //        Gmlist.Add(Gm);

        //    }
        //    List<A8Model> Hm = new List<A8Model>();
        //    ERPInfoManagement erp = new ERPInfoManagement();
        //    foreach (var item in Gmlist)
        //    {
        //        A8Model h = new A8Model();
        //        Dictionary<string, object> paras = new Dictionary<string, object>();
        //        paras["Equip_GyCode"] = null;
        //        paras["Job_Order"] = null;//通过工单找通知单号
        //        paras["Zz_Name"] = null;
        //        paras["Job_Name"] = null;//计划名称

        //        UI_WFEntity_Info wfei = CWFEngine.GetWorkFlowEntityWithParams(item.WE_Id, paras);
        //        h.zz_name = paras["Zz_Name"].ToString();
        //        h.sb_gycode = paras["Equip_GyCode"].ToString();
        //        h.job_order = paras["Job_Order"].ToString();
        //        h.plan_name = paras["Job_Name"].ToString();
        //        GD_InfoModal res = erp.getGD_Modal_GDId(paras["Job_Order"].ToString());
        //        if (res != null)
        //        { 
        //            h.notice_order = res.GD_Notice_Id;
        //        //h.gd_state = res.GD_UserState;
        //        h.gd_state = res.GD_State;
        //        }
        //        h.Data_Src = "待定";
        //        h.detail = "待定";
        //        Hm.Add(h);
        //    }
        //    List<object> or = new List<object>();
        //    for (int i = 0; i < Hm.Count; i++)
        //    {
        //        object o = new
        //        {
        //            index=i+1,
        //            equip_gycode = Hm[i].sb_gycode,
        //            job_order = Hm[i].job_order,
        //            notice_order = Hm[i].notice_order,
        //            gd_state=Hm[i].gd_state,
        //            datasrc = Hm[i].Data_Src,
        //            detail=Hm[i].detail,
        //            zzname=Hm[i].zz_name,
        //            planname=Hm[i].plan_name
        //        };
        //        or.Add(o);

        //    }
        //    string str = JsonConvert.SerializeObject(or);
        //    return ("{" + "\"data\": " + str + "}");
        //}


        public string A8HistoryList(string WorkFlow_Name)
        {
            string username = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;


            string WE_Status = "3";
            string query_list = "distinct E.WE_Ser, E.WE_Id, R.username";
            string query_condition = "E.W_Name='" + WorkFlow_Name + "' and E.WE_Status='" + WE_Status + "' and R.username is not null";
            string record_filter = "username is not null";
            DataTable dt = CWFEngine.QueryAllInformation(query_list, query_condition, record_filter);
            List<Gxqmodel> Gmlist = new List<Gxqmodel>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Gxqmodel Gm = new Gxqmodel();
                Gm.WE_Id = Convert.ToInt16(dt.Rows[i]["WE_Id"]);
                Gm.WE_Ser = dt.Rows[i]["WE_Ser"].ToString();
                Gmlist.Add(Gm);

            }
            List<HistroyModel> Hm = new List<HistroyModel>();
            foreach (var item in Gmlist)
            {
                HistroyModel h = new HistroyModel();
                Dictionary<string, object> paras = new Dictionary<string, object>();
                paras["Equip_GyCode"] = null;
                UI_WFEntity_Info wfei = CWFEngine.GetWorkFlowEntityWithParams(item.WE_Id, paras);
                List<UI_MISSION> AllHistoryMiss = CWFEngine.GetHistoryMissions(item.WE_Id);
                int Miss_Id = AllHistoryMiss[0].Miss_Id;
                h.miss_LastStatusdesc = AllHistoryMiss[AllHistoryMiss.Count - 1].WE_Event_Desc;
                IDictionary<string, string> r = CWFEngine.GetMissionRecordInfo(Miss_Id);
                if (r.Count > 0)
                {
                    h.missStartName = r["username"];
                    h.missStartTime = r["time"];
                }
                else
                {
                    h.missStartName = "";
                    h.missStartTime = "";
                }

                h.wfe_serial = item.WE_Ser;
                // h.miss_wfe_Id = wfei.EntityID;
                h.miss_wfe_Id = item.WE_Id;
                h.workFlowName = wfei.description;
                h.sbGycode = paras["Equip_GyCode"].ToString();
                Hm.Add(h);
            }
            List<object> or = new List<object>();
            for (int i = 0; i < Hm.Count; i++)
            {
                object o = new
                {
                    wfe_serial = Hm[i].wfe_serial,
                    equip_gycode = Hm[i].sbGycode,
                    workflow_name = Hm[i].workFlowName,
                    missStartName = Hm[i].missStartName,
                    missStartTime = Hm[i].missStartTime,
                    miss_LastStatusdesc = Hm[i].miss_LastStatusdesc,
                    miss_wfe_Id = Hm[i].miss_wfe_Id

                };
                or.Add(o);

            }
            string str = JsonConvert.SerializeObject(or);
            return ("{" + "\"data\": " + str + "}");
        }
	}
}
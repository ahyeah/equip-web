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
    public class A4dot2Controller : CommonController
    {
        public ActionResult Index()
        {
            return View(getIndexListRecords("A4dot2", (Session["User"] as EquipModel.Entities.Person_Info).Person_Name));
        }
        public ActionResult CaiGouSubmit(string wfe_id)
        {
            return View(getSubmitModel(wfe_id));//
        }
        public ActionResult WzcPriceBatch(string wfe_id)
        {
           
            return View(getWFDetail_Model(wfe_id));
        }
        public ActionResult ZytdConfirmEquip(string wfe_id)
        {

            return View(getWFDetail_Model(wfe_id));
        }
        public ActionResult ZytdTechSign(string wfe_id)
        {
            UI_MISSION miss = CWFEngine.GetActiveMission<Person_Info>(int.Parse(wfe_id), ((IObjectContextAdapter)(new EquipWebContext())).ObjectContext);
            ViewBag.currentMiss = miss;
            return View(getWFDetail_Model(wfe_id));
        }
        public ActionResult PqTechSign(string wfe_id)
        {
            UI_MISSION miss = CWFEngine.GetActiveMission<Person_Info>(int.Parse(wfe_id), ((IObjectContextAdapter)(new EquipWebContext())).ObjectContext);
            ViewBag.currentMiss = miss;
            return View(getWFDetail_Model(wfe_id));
        }
        public ActionResult ZytdAppoint(string wfe_id)
        {

            return View(getWFDetail_Model(wfe_id));
        }
        public ActionResult UploadBidFile(string wfe_id)
        {

            UI_MISSION miss = CWFEngine.GetActiveMission<Person_Info>(int.Parse(wfe_id), ((IObjectContextAdapter)(new EquipWebContext())).ObjectContext);
            ViewBag.currentMiss = miss;
            return View(getWFDetail_Model(wfe_id));
        }
        public ActionResult WorkFolw_Detail(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }
        // GET: /A4dot2/确定中标单位
        public ActionResult WzcConfirmSupplier(string wfe_id)
        {

            UI_MISSION miss = CWFEngine.GetActiveMission<Person_Info>(int.Parse(wfe_id), ((IObjectContextAdapter)(new EquipWebContext())).ObjectContext);
            ViewBag.currentMiss = miss;
            return View(getWFDetail_Model(wfe_id));
        }
        // GET: /A4dot2/专业团队确定是否需要监造明细
        public ActionResult ZytdConfirmJZ(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }
        // GET: /A4dot2/上传监造反馈文件
        public ActionResult JZEquipDetail(string wfe_id)
        {
            UI_MISSION miss = CWFEngine.GetActiveMission<Person_Info>(int.Parse(wfe_id), ((IObjectContextAdapter)(new EquipWebContext())).ObjectContext);
            ViewBag.currentMiss = miss;
            return View(getWFDetail_Model(wfe_id));
        }
        // GET: /A4dot2/ 物资处通知验收
        public ActionResult WzcNoticeCheck(string wfe_id)
        {

            return View(getWFDetail_Model(wfe_id));
        }
        // GET: /A4dot2/专业团队指定验收人员
        public ActionResult ZytdAppointCheck(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }
        // GET: /A4dot2/验收人员确定已验收
        public ActionResult CheckPersonConfirm(string wfe_id)
        {


            return View(getWFDetail_Model(wfe_id));


        }
        // GET: /A4dot2/物资处发出到货通知
        public ActionResult WzcNoticeArrived(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }
        // GET: /A4dot2/工程管理开箱验收
        public ActionResult CMWriConKXD(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }
        // GET: /A4dot2/工程管理确认移交是否完成
        public ActionResult CMConfirmHandover(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }
      
        public string CaiGouSubmit_submitsignal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string flowname = item["Flow_Name"].ToString();
                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();
                signal["CaiGouSubmit_Done"] = "true";
                signal["Cj_Name"] = item["Cj_Name"].ToString();
                signal["Zz_Name"] = item["Zz_Name"].ToString();
                signal["Equip_GyCode"] = item["Equip_GyCode"].ToString();
                signal["Equip_Code"] = "";
                signal["Plan_Order"] = item["Plan_Order"].ToString();
                signal["Plan_Name"] = item["Plan_Name"].ToString();
                signal["CM_Department"] = item["CM_Department"].ToString();
                signal["Fittings_Name"] = item["Fittings_Name"].ToString();
                signal["Fittings_Code"] = item["Fittings_Code"].ToString();
                //EquipManagment em = new EquipManagment();
                //Equip_Info eqinfo = em.getEquip_Info(item["Equip_Code"].ToString());
               
                //record
                Dictionary<string, string> record = new Dictionary<string, string>();
                record["username"] = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
                record["time"] = DateTime.Now.ToString();
                //submit
                //由于DongZyConfirm_done 等变量未与该Event关联， 所以submitSignal不会将确认信息提交到工作流
                CWFEngine.SubmitSignal(Convert.ToInt32(flowname), signal, record);
            }
            catch (Exception e)
            {
                return "";
            }
            return ("/A4dot2/Index");
        }
        public string WzcPriceBatch_submitsignal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
               
                string flowname = item["Flow_Name"].ToString();
                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();
                //signal["Job_Order"] = item2["Plan_num"].ToString();
                signal["Purchase_Batch"] = item["Purchase_Batch"].ToString();
                signal["Budgeted_Price"] = item["Budgeted_Price"].ToString();
                signal["IsMoreThanFifty_Result"] = item["IsMoreThanFifty_Result"].ToString();
                              
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
            return ("/A4dot2/Index");
        }
        public string ZytdConfirmEquip_submitsignal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);

                string flowname = item["Flow_Name"].ToString();
                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();
                //signal["Job_Order"] = item2["Plan_num"].ToString();
                signal["TechSign_Person"] = item["TechSign_Person"].ToString();
                signal["Supplier_List"] = item["Supplier_List"].ToString();
                signal["IsVIPEquip_Result"] = item["IsVIPEquip_Result"].ToString();

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
            return ("/A4dot2/Index");
        }
        public string ZytdTechSign_submitsignal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);

                string flowname = item["Flow_Name"].ToString();
                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();
                //signal["Job_Order"] = item2["Plan_num"].ToString();
                signal["TechAgreement_File"] = item["TechAgreement_File"].ToString();
               
                signal["ZytdTechSign_Done"] = "true";

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
            return ("/A4dot2/Index");
        }
        public string PqTechSign_submitsignal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);

                string flowname = item["Flow_Name"].ToString();
                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();
                //signal["Job_Order"] = item2["Plan_num"].ToString();
                signal["TechAgreement_File"] = item["TechAgreement_File"].ToString();

                signal["PqTechSign_Done"] = "true";

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
            return ("/A4dot2/Index");
        }
        public string ZytdAppoint_submitsignal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);

                string flowname = item["Flow_Name"].ToString();
                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();
                //signal["Job_Order"] = item2["Plan_num"].ToString();
                signal["TechDemandMake_Header"] = item["TechDemandMake_Header"].ToString();
                signal["TechDemandMake_Person"] = item["TechDemandMake_Person"].ToString();
                signal["Supplier_List"] = item["Supplier_List"].ToString();
                signal["ZytdAppoint_Done"] = "true";

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
            return ("/A4dot2/Index");
        }
        public string UploadBidFile_submitsignal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);

                string flowname = item["Flow_Name"].ToString();
                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();
                //signal["Job_Order"] = item2["Plan_num"].ToString();
                
                signal["Bid_File"] = item["Bid_File"].ToString();
                signal["UploadBidFile_Done"] = "true";

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
            return ("/A4dot2/Index");
        }

        public string BidSupplier_submitsignal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string flowname = item["Flow_Name"].ToString();
                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();
                signal["WzcConfirmSupplier_Done"] = "true";


                signal["Bid_Supplier"] = item["Bid_Supplier"].ToString();


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
            return ("/A4dot2/Index");
        }

        public string ZytdConfirm_submitsignal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string flowname = item["Flow_Name"].ToString();
                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();
                signal["JZEquip_Detail"] = item["JZEquip_Detail"].ToString();
                signal["ZytdConfirmJZ_Result"] = item["ZytdConfirmJZ_Result"].ToString();

                //record
                Dictionary<string, string> record = new Dictionary<string, string>();
                record["username"] = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
                record["time"] = DateTime.Now.ToString();
                //submit
                CWFEngine.SubmitSignal(Convert.ToInt32(flowname), signal, record);
            }
            catch (Exception e)
            { return ""; }
            return ("/A4dot2/Index");
        }


        //上传明细文件JZEquipDetail_submitsignal

        public string JZEquipDetail_submitsignal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string flowname = item["Flow_Name"].ToString();
                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();
                //signal["JZEquip_Detail"] = item["JZEquip_Detail"].ToString();
                signal["JZEquip_File"] = item["JZEquip_File"].ToString();
                signal["JZEquipDetail_Done"] = "true";
                //record
                Dictionary<string, string> record = new Dictionary<string, string>();
                record["username"] = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
                record["time"] = DateTime.Now.ToString();
                //submit
                CWFEngine.SubmitSignal(Convert.ToInt32(flowname), signal, record);
            }
            catch (Exception e)
            { return ""; }
            return ("/A4dot2/Index");
        }

        public string WZC_submitsignal(string json1)
        {
            JObject item = (JObject)JsonConvert.DeserializeObject(json1);
            string flowname = item["Flow_Name"].ToString();
            Dictionary<string, string> signal = new Dictionary<string, string>();
            //new input paras
            // signal["ZytdConfirm_Result"] = item["ZytdConfirm_Result"].ToString();
            signal["WzcNoticeCheck_Done"] = "true";



            Dictionary<string, string> record = new Dictionary<string, string>();
            record["username"] = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
            record["time"] = DateTime.Now.ToString();
            CWFEngine.SubmitSignal(Convert.ToInt32(flowname), signal, record);
            return ("/A4dot2/Index");
        }
        public string Check_Person_submitsignal(string json1)
        {
            JObject item = (JObject)JsonConvert.DeserializeObject(json1);
            string flowname = item["Flow_Name"].ToString();
            Dictionary<string, string> signal = new Dictionary<string, string>();
            //new input paras
            signal["Check_Person"] = item["Check_Person"].ToString();
            signal["Check_Header"] = item["Check_Header"].ToString();
            signal["ZytdAppointCheck_Done"] = "true";
            Dictionary<string, string> record = new Dictionary<string, string>();
            record["username"] = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
            record["time"] = DateTime.Now.ToString();
            CWFEngine.SubmitSignal(Convert.ToInt32(flowname), signal, record);
            return ("/A4dot2/Index");
        }







        public string CheckPersonConfirm_submitsignal(string json1)
        {
            JObject item = (JObject)JsonConvert.DeserializeObject(json1);
            string flowname = item["Flow_Name"].ToString();
            Dictionary<string, string> signal = new Dictionary<string, string>();
            //new input paras
            signal["CheckPersonConfirm_Result"] = "是";
            Dictionary<string, string> record = new Dictionary<string, string>();
            record["username"] = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
            record["time"] = DateTime.Now.ToString();
            CWFEngine.SubmitSignal(Convert.ToInt32(flowname), signal, record);
            return ("/A4dot2/Index");
        }



        public string WzcNoticeArrived_submitsignal(string json1)
        {
            JObject item = (JObject)JsonConvert.DeserializeObject(json1);
            string flowname = item["Flow_Name"].ToString();
            Dictionary<string, string> signal = new Dictionary<string, string>();
            //new input paras
            signal["WzcNoticeArrived_Done"] = "true";

            Dictionary<string, string> record = new Dictionary<string, string>();
            record["username"] = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
            record["time"] = DateTime.Now.ToString();
            CWFEngine.SubmitSignal(Convert.ToInt32(flowname), signal, record);
            return ("/A4dot2/Index");
        }



        public string CMWriConKXD_submitsignal(string json1)
        {
            JObject item = (JObject)JsonConvert.DeserializeObject(json1);
            string flowname = item["Flow_Name"].ToString();
            Dictionary<string, string> signal = new Dictionary<string, string>();
            //new input paras
            signal["CMConfirmKXD_Result"] = item["CMConfirmKXD_Result"].ToString();

            Dictionary<string, string> record = new Dictionary<string, string>();
            record["username"] = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
            record["time"] = DateTime.Now.ToString();
            CWFEngine.SubmitSignal(Convert.ToInt32(flowname), signal, record);
            return ("/A4dot2/Index");
        }



        public string CMConfirmHandover_submitsignal(string json1)
        {
            JObject item = (JObject)JsonConvert.DeserializeObject(json1);
            string flowname = item["Flow_Name"].ToString();
            Dictionary<string, string> signal = new Dictionary<string, string>();
            //new input paras
            signal["CMConfirmHandover_Done"] = "true";

            Dictionary<string, string> record = new Dictionary<string, string>();
            record["username"] = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
            record["time"] = DateTime.Now.ToString();
            CWFEngine.SubmitSignal(Convert.ToInt32(flowname), signal, record);
            return ("/A4dot2/Index");
        }


    }
}
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
    public class A11dot4Controller : CommonController
    {
        //
        // GET: /A11dot4/

        public JsonResult List_AccessGroup()
        {
            BaseDataManagment DM = new BaseDataManagment();

            List<TreeListNode> AccessGroup_obj = DM.BuildAccessGroupPersonList();
            return Json(AccessGroup_obj.ToArray());
        }
        public ActionResult Index()
        {
            return View(getIndexListRecords("A11dot4", (Session["User"] as EquipModel.Entities.Person_Info).Person_Name));
        }
        public JsonResult List_Workflow()
        {
            BaseDataManagment DM = new BaseDataManagment();

            List<TreeListNode> Menu_obj = DM.BuildMenuList("3");
            return Json(Menu_obj.ToArray());
        }
        // GET: /A11dot4/提报无页面，由其他流程转入并自动完成
        public ActionResult PreSubmit(string wfe_id)
        {
            return View(getSubmitModel(wfe_id));
        }
        // GET: /A11dot4/成立风险评估小组
        public ActionResult CreateAssessGroup(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }
        // GET: /A11dot4/风险评估小组确定并上传评估报告
        public ActionResult ConfirmAndUpload(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }  
        //工作流详情
        public ActionResult WorkFolw_Detail(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }     
        public string CreateAssessGroup_submitsignal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string flowname = item["Flow_Name"].ToString();
                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();
                signal["CreateAssessGroup_done"] = item["CreateAssessGroup_done"].ToString();
                signal["Group_Header"] = item["Group_Header"].ToString();
                signal["Group_Member"] = item["Group_Member"].ToString();
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
            return ("/A11dot4/Index");
        }
        public string ConfirmAndUpload_submitsignal(string json1)
        {
            JObject item = (JObject)JsonConvert.DeserializeObject(json1);
            string flowname = item["Flow_Name"].ToString();
            Dictionary<string, string> signal = new Dictionary<string, string>();
            //new input paras
            signal["Plan_Desc"] = item["Plan_Desc"].ToString();            
            signal["Plan_DescFilePath"] = item["Plan_DescFilePath"].ToString();
            signal["Risk_IsAcceptable"] = item["Risk_IsAcceptable"].ToString();        
            signal["ConfirmAndUpload_done"] = "true";

            Dictionary<string, string> record = new Dictionary<string, string>();
            record["username"] = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
            record["time"] = DateTime.Now.ToString();
            CWFEngine.SubmitSignal(Convert.ToInt32(flowname), signal, record);
            return ("/A11dot4/Index");
        }
        
    }
}
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
    public class A7dot3Controller : CommonController
    {
        //
        // GET: /A7dot3/
        public ActionResult Index()
        {
            return View(getIndexListRecords("A7dot3", (Session["User"] as EquipModel.Entities.Person_Info).Person_Name));            
        }
        // GET: /A7dot3/装置提报
        public ActionResult ZzSubmit(string wfe_id)
        {
            return View(getSubmitModel(wfe_id));
        }
        
        // GET: /A7dot3/跳转到A12.2工艺变更管理
        public ActionResult JumpToA12dot2(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }
                       
        
        //GET: /A7dot3/工作流详情
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
                //signal["Data_Src"] = "";         
                signal["Plan_Desc"] = item["Plan_Desc"].ToString();               
                //string  filename = Path.Combine(Request.MapPath("~/Upload"),item["Plan_DescFilePath"].ToString());
                signal["Plan_DescFilePath"] = item["Plan_DescFilePath"].ToString();
                signal["Zy_Type"] = item["Zy_Type"].ToString();
                signal["Zy_SubType"] = item["Zy_SubType"].ToString();                
                EquipManagment em = new EquipManagment();
                signal["Equip_ABCMark"] = em.getEquip_Info(item["Equip_Code"].ToString()).Equip_ABCmark;
                //signal["Equip_ABCMark"] = "A";//for test
               
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
            return ("/A7dot3/Index");
        }

    }
}
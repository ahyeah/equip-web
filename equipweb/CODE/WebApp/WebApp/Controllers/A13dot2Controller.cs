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
    public class A13dot2Controller :CommonController
    {
        //
        // GET: /A13dot2/
        public ActionResult Index()
        {
            return View(getA13dot2_Model());            
        }


        
        public string click_submitsignal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string Equip_Code = item["Equip_Code"].ToString();
                string Jx_Reason = item["Jx_Reason"].ToString();               
                string flowname = "A8dot2"; 
                UI_WorkFlow_Entity wfe = CWFEngine.CreateAWFEntityByName(flowname);
                if (wfe != null)
                {
                    EquipManagment em = new EquipManagment();
                    Equip_Info eqinfo = em.getEquip_Info(Equip_Code);                    
                    List<Equip_Archi> Equip_ZzBelong = em.getEquip_ZzBelong(eqinfo.Equip_Id);               
                    Dictionary<string, string> record = wfe.GetRecordItems();
                    record["username"] = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
                    record["time"] = DateTime.Now.ToString();
                    wfe.Start(record);
                    int flow_id = wfe.EntityID;
                    //paras
                    Dictionary<string, string> signal = new Dictionary<string, string>();
                    signal["JxSubmit_done"] = "true";
                    signal["Cj_Name"] = Equip_ZzBelong[1].EA_Name; //Cj_Name
                    signal["Zz_Name"] = Equip_ZzBelong[0].EA_Name; //Zz_Name
                    signal["Equip_GyCode"] = eqinfo.Equip_GyCode;
                    signal["Equip_Code"] = eqinfo.Equip_Code;
                    signal["Equip_Type"] = eqinfo.Equip_Type;                   
                    signal["Zy_Type"] = eqinfo.Equip_Specialty;
                    signal["Zy_SubType"] = eqinfo.Equip_PhaseB;
                    signal["Equip_ABCMark"] = eqinfo.Equip_ABCmark;
                    signal["Jx_Reason"] = Jx_Reason;// "一级缺陷";//计划检修原因 PM？ 
                    signal["Data_Src"] = "A13dot2";
                    //record                    
                    record["username"] = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
                    record["time"] = DateTime.Now.ToString();
                    //submit
                    CWFEngine.SubmitSignal(flow_id, signal, record);
                    return ("/A8dot2/Index");
                }
                else
                    return "/A13dot2/Index";
        
            }
            catch (Exception e)
            {
                return "";
            }

            //return ("/A13dot2/Index");
        }

    }
}
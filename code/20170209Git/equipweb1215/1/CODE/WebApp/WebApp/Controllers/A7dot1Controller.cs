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
    public class A7dot1Controller : CommonController
    {
        //
        // GET: /A7dot1/
        public ActionResult Index()
        {
            return View(getIndexListRecords("A7dot1", (Session["User"] as EquipModel.Entities.Person_Info).Person_Name));            
        }
        // GET: /A7dot1/装置提报
        public ActionResult ZzSubmit(string wfe_id)
        {
            return View(getSubmitModel(wfe_id));
        }

        // GET: /A7dot1/美迅平台数据筛选
        public ActionResult MxptSelect(string flowname)
        {
            //页面权限，现场工程师能处理
            int cur_PersionID = (Session["User"] as EquipModel.Entities.Person_Info).Person_Id;
            PersonManagment pm = new PersonManagment();
            EquipBLL.AdminManagment.PersonManagment.P_viewModal pv = pm.Get_PersonModal(cur_PersionID);
            ViewBag.user_Role = pv.Role_Names;
            return View(getA7dot1MxAlarm_Model()); //2016.6.22 
        }

        //装置筛选美迅报警设备-自动提报到A13.1进行片区分类-2016.07.27
        public string Auto_MxptSelect_submitsignalToA13dot1(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string Equip_Code = item["Equip_Code"].ToString();
                string AlarmState = item["AlarmState"].ToString();
                string flowname = "A13dot1";
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
                    signal["ZzSubmit_done"] = "true";
                    signal["Cj_Name"] = Equip_ZzBelong[1].EA_Name; //Cj_Name
                    signal["Zz_Name"] = Equip_ZzBelong[0].EA_Name; //Zz_Name
                    signal["Equip_GyCode"] = eqinfo.Equip_GyCode;
                    signal["Equip_Code"] = eqinfo.Equip_Code;
                    signal["Equip_Type"] = eqinfo.Equip_Type;
                    signal["Zy_Type"] = eqinfo.Equip_Specialty;
                    signal["Zy_SubType"] = eqinfo.Equip_PhaseB;
                    signal["Equip_ABCMark"] = eqinfo.Equip_ABCmark;
                    signal["Data_Src"] = "A7.1.3-美迅平台报警";
                    signal["Problem_Desc"] = "美迅离线巡检平台显示设备报警状态等级：" + AlarmState; 
                    signal["Problem_DescFilePath"] = "";

                    //record                    
                    record["username"] = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
                    record["time"] = DateTime.Now.ToString();
                    //submit
                    CWFEngine.SubmitSignal(flow_id, signal, record);
                    return ("/A7dot1/Index");
                }

            }
            catch (Exception e)
            {
                return "";
            }
            return ("/A7dot1/Index");
        }

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
                signal["Problem_Desc"] = item["Problem_Desc"].ToString();  
                signal["Problem_DescFilePath"] = item["Problem_DescFilePath"].ToString();
                signal["Zy_Type"] = item["Zy_Type"].ToString();
                signal["Zy_SubType"] = item["Zy_SubType"].ToString();
                EquipManagment em = new EquipManagment();
                signal["Equip_ABCMark"] = em.getEquip_Info(item["Equip_Code"].ToString()).Equip_ABCmark;             
                signal["Data_Src"] = "S8000";
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
            return ("/A7dot1/Index");
        }


    }
}
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
using FlowEngine.Modals;
using FlowEngine.DAL;
using FlowEngine.Param;

namespace WebApp.Controllers
{
    public class A14dot3dot1Controller : CommonController
    {

        public ActionResult WorkFolw_Detail(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }
        public ActionResult PlanSubmit(string wfe_id)
        {

            return View(getSubmitModel(wfe_id));
        }
        public ActionResult EquipFilter(string wfe_id)
        {

            return View(getWFDetail_Model(wfe_id));
        }
        public string PlanSubmit_submitsignal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string flowname = item["Flow_Name"].ToString();
                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();
                signal["Cj_Name"] = item["Cj_Name"].ToString();
                signal["Zz_Name"] = item["Zz_Name"].ToString();
                signal["WorkDetail"] = item["WorkDetail"].ToString();
                signal["Equip_GyCode"] = "";
                signal["PlanSubmit_Done"] = "true";
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
            return ("/A14dot3/Index");
        }
        public class EquipListModel
        {
            public int Equip_Id;
            public string Equip_GyCode;
            public string Equip_Code;
            public string Equip_Type;
            public string Equip_Specialty;
            public string Equip_ABCMark;
        }
        public string get_equip_info(string wfe_id)
        {
            WorkFlows wfsd = new WorkFlows();
            Mission db_miss = wfsd.GetWFEntityMissions(Convert.ToInt16(wfe_id)).Last();//获取该实体最后一个任务        


            WorkFlow_Entity wfe = wfsd.GetWorkFlowEntity(Convert.ToInt16(wfe_id));
            //CWorkFlow wf = new CWorkFlow();
            WorkFlows wfs = new WorkFlows();
            UI_MISSION ui = new UI_MISSION();
            ui.WE_Entity_Ser = wfe.WE_Ser;
            ui.WE_Event_Desc = db_miss.Miss_Desc;
            ui.WE_Event_Name = db_miss.Event_Name;
            ui.WE_Name = db_miss.Miss_Name;
            ui.Mission_Url = ""; //历史任务的页面至空
            ui.Miss_Id = db_miss.Miss_Id;
            List<Mission_Param> mis_pars = wfsd.GetMissParams(db_miss.Miss_Id);//获取当前任务参数
            foreach (var par in mis_pars)
            {
                CParam cp = new CParam();
                cp.type = par.Param_Type;
                cp.name = par.Param_Name;
                cp.value = par.Param_Value;
                cp.description = par.Param_Desc;
                ui.Miss_Params[cp.name] = cp.value;
                ui.Miss_ParamsDesc[cp.name] = cp.description;
            }
            List<EquipListModel> EquipList = Zz_Equips(ui.Miss_Params["Zz_Name"].ToString());
            List<object> miss_obj = new List<object>();
            int i = 1;
            foreach (var item in EquipList)
            {
                object m = new
                {
                    index = i,
                    Equip_Id = item.Equip_Id,
                    Equip_GyCode = item.Equip_GyCode,
                    Equip_Code = item.Equip_Code,
                    Equip_Type = item.Equip_Type,
                    Equip_Specialty = item.Equip_Specialty,
                    Equip_ABCMark = item.Equip_ABCMark
                };
                miss_obj.Add(m);
                i++;
            }

            string str = JsonConvert.SerializeObject(miss_obj);
            return ("{" + "\"data\": " + str + "}");


        }
        public List<EquipListModel> Zz_Equips(string Zz_id)
        {
            EquipManagment pm = new EquipManagment();
            List<Equip_Info> Equips_of_Zz = new List<Equip_Info>();
            List<EquipListModel> Equip_obj = new List<EquipListModel>();
            string[] Zhuangzhi_id = Zz_id.Split(',');
            for (int i = 0; i < Zhuangzhi_id.Length; i++)
            {
                Equips_of_Zz = pm.getEquips_OfZz(Convert.ToInt32(Zhuangzhi_id[i]));
                foreach (var item in Equips_of_Zz)
                {
                    EquipListModel io = new EquipListModel();
                    io.Equip_Id = item.Equip_Id;
                    io.Equip_GyCode = item.Equip_GyCode;
                    io.Equip_Code = item.Equip_Code;
                    io.Equip_Type = item.Equip_Type;
                    io.Equip_Specialty = item.Equip_Specialty;
                    io.Equip_ABCMark = item.Equip_ABCmark;
                    Equip_obj.Add(io);
                }
            }
            return Equip_obj;

        }
        public string EquipFilter_submitsignal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string flowname = item["Flow_Name"].ToString();
                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();
                signal["EquipFilter_Done"] = "true";

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
            return ("/A14dot3/Index");
        }
    }
}
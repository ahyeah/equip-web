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
using System.Collections.Specialized;
using WebApp.Models.DateTables;
using FlowEngine.DAL;
using FlowEngine.Modals;

namespace WebApp.Controllers
{
    public class A14dot3dot3Controller : CommonController
    {
        //月度计划人工提报
       
        public ActionResult SubmitJxPlan(string wfe_id)
        {           
            return View(getSubmitModel(wfe_id));
        }
        public ActionResult WorkFolw_Detail(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }

        public string submitsignal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string flowname = item["Flow_Name"].ToString();
                Dictionary<string, string> signal = new Dictionary<string, string>();
                signal["SubmitJxPlan_Done"] = "true";
                signal["Cj_Name"] = item["Cj_Name"].ToString();
                signal["Zz_Name"] = item["Zz_Name"].ToString();
                signal["Equip_GyCode"] = item["Equip_GyCode"].ToString();
                signal["Equip_Code"] = item["Equip_Code"].ToString();
                signal["Equip_Type"] = item["Equip_Type"].ToString();              
                signal["Zy_Type"] = item["Zy_Type"].ToString();
                signal["Zy_SubType"] = item["Zy_SubType"].ToString();
                EquipManagment em = new EquipManagment();
                signal["Equip_ABCMark"] = em.getEquip_Info(item["Equip_Code"].ToString()).Equip_ABCmark;
                signal["Plan_Name"] = item["Plan_Name"].ToString();
                signal["JxCauseDesc"] = item["JxCauseDesc"].ToString();
              
                Dictionary<string, string> record = new Dictionary<string, string>();
                record["username"] = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
                record["time"] = DateTime.Now.ToString();
                CWFEngine.SubmitSignal(Convert.ToInt32(flowname), signal, record);
            }
            catch (Exception e)
            {
                return "";
            }
            return ("/A14dot3/Index");
        }
        private List<KeyValuePair<string, string>> ParsePostForm(NameValueCollection Form)
        {
            var list = new List<KeyValuePair<string, string>>();
            if (Form != null)
            {
                foreach (var f in Form.AllKeys)
                {
                    list.Add(new KeyValuePair<string, string>(f, Form[f]));
                }
            }
            return list;
        }

        private DtResponse ProcessRequest(List<KeyValuePair<string, string>> data)
        {
            DtResponse dt = new DtResponse();
            var http = DtRequest.HttpData(data);
            var Data = http["data"] as Dictionary<string, object>;
            int wfe_id = -1;
            foreach (var d in Data)
            {
                wfe_id = Convert.ToInt32(d.Key);
            }
            foreach (var d in Data)
            {
                int id = Convert.ToInt32(d.Key);
                foreach (var dd in d.Value as Dictionary<string, object>)
                {
                    Dictionary<string, string> signal = new Dictionary<string, string>();
                    ERPInfoManagement erp = new ERPInfoManagement();
                    if (dd.Key == "kkconfirm1")
                    {
                        if (dd.Value.ToString() == "")
                            break;
                        string[] result = dd.Value.ToString().Split(new char[] { '$' });
                        signal["KkConfirm_Result"] = result[0];
                        if (result.Length > 1)
                            signal["KkConfirm_Reason"] = result[1];
                        //record
                        Dictionary<string, string> record1 = new Dictionary<string, string>();
                        record1["username"] = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
                        record1["time"] = DateTime.Now.ToString();
                        //submit
                        CWFEngine.SubmitSignal(Convert.ToInt32(id), signal, record1);
                    }
                    if (dd.Key == "zytdconfirm1")
                    {
                        if (dd.Value.ToString() == "")
                            continue;
                        string[] result = dd.Value.ToString().Split(new char[] { '$' });
                        signal["ZytdConfirm_Result"] = result[0];
                        if (result.Length > 1)
                            signal["ZytdConfirm_Reason"] = result[1];


                        //record
                        Dictionary<string, string> record1 = new Dictionary<string, string>();
                        record1["username"] = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
                        record1["time"] = DateTime.Now.ToString();
                        //submit
                        CWFEngine.SubmitSignal(Convert.ToInt32(id), signal, record1);
                    }
                    if (dd.Key == "notice_order1")
                    {
                        if (dd.Value.ToString() == "")
                            continue;
                        signal["NoticeOrder"] = "00" + dd.Value.ToString();
                        GD_InfoModal res = erp.getGD_Modal_Notice("00" + dd.Value.ToString());
                        if (res != null)
                            signal["JobOrder"] = res.GD_Id;
                            
                        //record
                        Dictionary<string, string> record1 = new Dictionary<string, string>();
                        record1["username"] = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
                        record1["time"] = DateTime.Now.ToString();
                        //submit
                        CWFEngine.SubmitSignal(Convert.ToInt32(id), signal, record1);
                    }
                    if (dd.Key == "job_order1")
                    {
                        if (dd.Value.ToString() == "")
                            continue;
                        signal["JobOrder"] = "00" + dd.Value.ToString();
                        GD_InfoModal res = erp.getGD_Modal_GDId("00" + dd.Value.ToString());
                        if (res != null)
                            signal["NoticeOrder"] = res.GD_Notice_Id;
                       
                        //record
                        Dictionary<string, string> record1 = new Dictionary<string, string>();
                        record1["username"] = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
                        record1["time"] = DateTime.Now.ToString();
                        //submit
                        CWFEngine.SubmitSignal(Convert.ToInt32(id), signal, record1);
                    }
                    //if (dd.Key == "JumpA8dot2")
                    //{
                    //    //补充跳转A8dot2的变量，Cj_Name，Zy_Type，Zy_SubType
                    //    Dictionary<string, object> paras1 = new Dictionary<string, object>();
                    //    paras1["Zz_Name"] = null;
                    //    paras1["Equip_GyCode"] = null;
                    //    paras1["Equip_Code"] = null;
                    //    paras1["Plan_Name"] = null;
                    //    UI_WFEntity_Info wfei = CWFEngine.GetWorkFlowEntityWithParams(id, paras1);
                    //    //获取设备专业类别和子类别及设备所属车间
                    //    EquipManagment tm = new EquipManagment();
                    //    Equip_Info getZy = tm.getEquip_ByGyCode(paras1["Equip_GyCode"].ToString());
                    //    signal["Zy_Type"] = getZy.Equip_Specialty;
                    //    signal["Zy_SubType"] = getZy.Equip_PhaseB;
                    //    signal["Equip_Type"] = getZy.Equip_Type;
                    //    //EA_Name_EA_Id= tm.getEquip(paras1["Zz_Name"].ToString()).EA_Parent.EA_Id;
                    //    signal["Cj_Name"] = tm.getEquip(paras1["Zz_Name"].ToString());
                    //    signal["Plan_Name"] = paras1["Plan_Name"].ToString();
                    //    signal["JxdwAttachPlanOrder_Done"] = dd.Value.ToString();
                    //    //record
                    //    Dictionary<string, string> record1 = new Dictionary<string, string>();
                    //    record1["username"] = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
                    //    record1["time"] = DateTime.Now.ToString();
                    //    //submit
                    //    CWFEngine.SubmitSignal(Convert.ToInt32(id), signal, record1);
                    //}
                }
            }


            Dictionary<string, object> paras = new Dictionary<string, object>();
            paras["Zz_Name"] = null;
            paras["Equip_GyCode"] = null;
            paras["Equip_Code"] = null;
            paras["Equip_Type"] = null;
            paras["Equip_ABCMark"] = null;
            paras["Plan_Name"] = null;
            paras["JxCauseDesc"] = null;
           
            paras["KkConfirm_Result"] = null;
            paras["ZytdConfirm_Result"] = null;
            paras["JobOrder"] = null;
            paras["NoticeOrder"] = null;
            if (wfe_id != -1)
            {
                WorkFlows wfsd = new WorkFlows();
                UI_WFEntity_Info wfei = CWFEngine.GetWorkFlowEntityWithParams(wfe_id, paras);
                Dictionary<string, object> m_kv = new Dictionary<string, object>();
                Mission db_miss = wfsd.GetWFEntityMissions(wfe_id).Last();//获取该实体最后一个任务
                int UserId = (Session["User"] as EquipModel.Entities.Person_Info).Person_Id;
                PersonManagment pm = new PersonManagment();
                EquipBLL.AdminManagment.PersonManagment.P_viewModal pv = pm.Get_PersonModal(UserId);

                m_kv["index_Id"] = wfe_id;
                m_kv["zz_name"] = paras["Zz_Name"].ToString();
                m_kv["sb_gycode"] = paras["Equip_GyCode"].ToString();
                m_kv["sb_code"] = paras["Equip_Code"].ToString();
                m_kv["sb_type"] = paras["Equip_Type"].ToString();
                m_kv["sb_ABCMark"] = paras["Equip_ABCMark"].ToString();
                m_kv["plan_name"] = paras["Plan_Name"].ToString();
                m_kv["jxreason"] = paras["JxCauseDesc"].ToString();
               
                m_kv["kkconfirm1"] = paras["KkConfirm_Result"].ToString();
                m_kv["zytdconfirm1"] = paras["ZytdConfirm_Result"].ToString();
                m_kv["job_order1"] = paras["JobOrder"].ToString();
                m_kv["notice_order1"] = paras["NoticeOrder"].ToString();
                m_kv["missionname"] = db_miss.Miss_Desc;
                m_kv["role"] = pv.Role_Names;
                dt.data.Add(m_kv);

            }
            return dt;
        }
        public JsonResult PostChanges()
        {

            var request = System.Web.HttpContext.Current.Request;
            var list = ParsePostForm(request.Form);
            var dtRes = ProcessRequest(list);


            return Json(dtRes);
        }

    }
}
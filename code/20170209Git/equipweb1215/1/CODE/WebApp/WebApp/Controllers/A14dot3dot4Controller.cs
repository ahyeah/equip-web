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
    public class A14dot3dot4Controller : CommonController
    {
        //月度计划DRBPM
       
        public ActionResult SubmitJxPlan(string wfe_id)
        {
            return View(getSubmitModel(wfe_id));
        }
        public ActionResult WorkFolw_Detail(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }

        public string getA14dot3dot4dcl_list(string WorkFlow_Name)
        {
            List<A14dot3dot4Model> Am = new List<A14dot3dot4Model>();            
            
            List<A14dot3dot4_ModelInfo> YueduDRBPM = new List<A14dot3dot4_ModelInfo>();
            YueduDRBPM=getA14dot3dot4_Model().A14dot3dot4_ModelInfoList;
            
            for (int j = 0; j < YueduDRBPM.Count; j++)
            {
                A14dot3dot4Model o = new A14dot3dot4Model();
                o.index_Id = j+1;
                o.zz_name = YueduDRBPM[j].Zz_Name;
                o.sb_gycode = YueduDRBPM[j].Equip_GyCode;
                o.sb_code = YueduDRBPM[j].Equip_Code;
                o.sb_type = YueduDRBPM[j].Equip_Type;
                o.sb_ABCMark = YueduDRBPM[j].Equip_ABCMark;
                o.plan_name = "来自DRBPM";
                o.jxreason = YueduDRBPM[j].Jx_Reason;
                o.xcconfirm = "同意";
                o.kkconfirm2 = "同意";
                o.zytdconfirm2 = "同意";
                o.job_order2 = YueduDRBPM[j].Job_Order;
                o.notice_order2 = "";//通过工单号获取通知单号 Q1:会有无法获取通知单号的情况吗，是否允许手动填写
                
                    if (!string.IsNullOrEmpty(YueduDRBPM[j].JxSubmit_done.Trim()))
                    {
                        o.missionname = "已跳转至A8.2";
                    }
                    else
                o.missionname = "审核已完成，工单待填写";
                int UserId = (Session["User"] as EquipModel.Entities.Person_Info).Person_Id;
                PersonManagment pm = new PersonManagment();
                EquipBLL.AdminManagment.PersonManagment.P_viewModal pv = pm.Get_PersonModal(UserId);
                o.role = pv.Role_Names;

                Am.Add(o);
            }
            string str = JsonConvert.SerializeObject(Am);
            return ("{" + "\"data\": " + str + "}");
        }
        public string testgetA14dot3dot4dcl_list(string WorkFlow_Name)
        {
            List<A14dot3dot4Model> Am = new List<A14dot3dot4Model>();
                            
                A14dot3dot4Model o = new A14dot3dot4Model();
                o.index_Id =1;
                o.zz_name = "1#常压装置";
                o.sb_gycode = "1#常压常一中泵P112A";
                o.sb_code = "210000034";
                o.sb_type = "150HDS-91";
                o.sb_ABCMark ="C";
                o.plan_name = "来自DRBPM";
                o.jxreason = "就是想检修";
                o.xcconfirm = "同意";
                o.kkconfirm2 = "同意";
                o.zytdconfirm2 = "同意";
                o.job_order2 = "23333";
                o.notice_order2 = "";//通过工单号获取通知单号 Q1:会有无法获取通知单号的情况吗，是否允许手动填写
                o.missionname = "";
                int UserId = (Session["User"] as EquipModel.Entities.Person_Info).Person_Id;
                PersonManagment pm = new PersonManagment();
                EquipBLL.AdminManagment.PersonManagment.P_viewModal pv = pm.Get_PersonModal(UserId);
                o.role = pv.Role_Names;

                Am.Add(o);
            
            string str = JsonConvert.SerializeObject(Am);
            return ("{" + "\"data\": " + str + "}");
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
                signal["Data_Src"] = "月度计划DRBPM";
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
        public string click_submitsignal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                ERPInfoManagement erp = new ERPInfoManagement();
                GD_InfoModal res = erp.getGD_Modal_GDId(item["Job_Order"].ToString());
                if (res != null)
                {
                    if (String.Compare(res.GD_EquipCode.Trim(), item["Equip_Code"].ToString().Trim()) != 0)
                    {
                        return "工单号与设备不匹配";
                    }
                }
                else
                {
                    return "系统中无此工单";
                }

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
                    signal["Jx_Reason"] = Jx_Reason;//计划检修原因 PM？ 
                    signal["Data_Src"] = "月度计划DRBPM";
                    signal["Job_Name"] = "来自DRBPM";
                    signal["Job_Order"] = item["Job_Order"].ToString(); 
                    //record                    
                    record["username"] = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
                    record["time"] = DateTime.Now.ToString();
                    //submit
                    CWFEngine.SubmitSignal(flow_id, signal, record);
                    return ("/A8dot2/Index");
                }
                else
                    return "/A14dot1/Index";

            }
            catch (Exception e)
            {
                return "";
            }

            //return ("/A14dot1/Index");
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
            string jx_reason="";
            string E_code="";
            string job_order="";
            string notice_order="";
            foreach (var d in Data)
            {
                int id = Convert.ToInt32(d.Key);
                foreach (var dd in d.Value as Dictionary<string, object>)
                {
                    ERPInfoManagement erp = new ERPInfoManagement();
                   
                    //sb_code、jxreason与设备绑定在一起传过来，当通知单号工单号填完后满足向A8.2跳转条件
                    if (dd.Key == "sb_code")
                    {
                        E_code=dd.Value.ToString();
                    }
                    if (dd.Key == "jxreason")
                    {
                        jx_reason=dd.Value.ToString();
                    }
                    if (dd.Key == "notice_order2")
                    {
                        if (dd.Value.ToString() == "")
                            continue;
                        notice_order="00"+dd.Value.ToString();
                        GD_InfoModal res = erp.getGD_Modal_Notice(notice_order);
                        if (res != null)
                        job_order=res.GD_Id;
                        

                    }
                    if (dd.Key == "job_order2")
                    {
                        if (dd.Value.ToString() == "")
                            continue;
                        job_order = "00" + dd.Value.ToString();
                        GD_InfoModal res = erp.getGD_Modal_GDId(job_order);
                        if (res != null)
                            notice_order = res.GD_Notice_Id;
                       
                    }
                    //if (dd.Key == "JumpA8dot2DR")
                    //{
                    //    string Equip_Code = E_code;
                    //    string Jx_Reason = jx_reason;
                    //    string flowname = "A8dot2";
                    //    UI_WorkFlow_Entity wfe = CWFEngine.CreateAWFEntityByName(flowname);
                    //    if (wfe != null)
                    //    {
                    //        EquipManagment em = new EquipManagment();
                    //        Equip_Info eqinfo = em.getEquip_Info(Equip_Code);
                    //        List<Equip_Archi> Equip_ZzBelong = em.getEquip_ZzBelong(eqinfo.Equip_Id);
                    //        Dictionary<string, string> record = wfe.GetRecordItems();
                    //        record["username"] = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
                    //        record["time"] = DateTime.Now.ToString();
                    //        wfe.Start(record);
                    //        int flow_id = wfe.EntityID;
                    //        //paras
                    //        Dictionary<string, string> signal = new Dictionary<string, string>();
                    //        signal["JxSubmit_done"] = "true";
                    //        signal["Cj_Name"] = Equip_ZzBelong[1].EA_Name; //Cj_Name
                    //        signal["Zz_Name"] = Equip_ZzBelong[0].EA_Name; //Zz_Name
                    //        signal["Equip_GyCode"] = eqinfo.Equip_GyCode;
                    //        signal["Equip_Code"] = eqinfo.Equip_Code;
                    //        signal["Equip_Type"] = eqinfo.Equip_Type;
                    //        signal["Zy_Type"] = eqinfo.Equip_Specialty;
                    //        signal["Zy_SubType"] = eqinfo.Equip_PhaseB;
                    //        signal["Equip_ABCMark"] = eqinfo.Equip_ABCmark;
                    //        signal["Jx_Reason"] = Jx_Reason;//计划检修原因 PM？ 
                    //        signal["Job_Name"] = "来自DRBPM";
                    //        signal["Job_Order"] = job_order;
                    //        //record                    
                    //        record["username"] = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
                    //        record["time"] = DateTime.Now.ToString();
                    //        //submit
                    //        CWFEngine.SubmitSignal(flow_id, signal, record);
                            
                    //    }
                    //}
                }
            }

            
             Dictionary<string, object> m_kv = new Dictionary<string, object>();
             EquipManagment em1 = new EquipManagment();
             Equip_Info eqinfo1 = em1.getEquip_Info(E_code);
             List<Equip_Archi> Equip_ZzBelong1 = em1.getEquip_ZzBelong(eqinfo1.Equip_Id);  
 

                int UserId = (Session["User"] as EquipModel.Entities.Person_Info).Person_Id;
                PersonManagment pm = new PersonManagment();
                EquipBLL.AdminManagment.PersonManagment.P_viewModal pv = pm.Get_PersonModal(UserId);

                m_kv["index_Id"] = wfe_id;
                m_kv["zz_name"] =Equip_ZzBelong1[0].EA_Name; //Zz_Name 
                m_kv["sb_gycode"] = eqinfo1.Equip_GyCode;
                m_kv["sb_code"] = E_code;
                m_kv["sb_type"] = eqinfo1.Equip_Type;
                m_kv["sb_ABCMark"] = eqinfo1.Equip_ABCmark;
                m_kv["plan_name"] ="来自DRBPM";
                m_kv["jxreason"] =jx_reason;
                m_kv["kkconfirm2"] = "同意";
                m_kv["zytdconfirm2"] ="同意";
                m_kv["job_order2"] = job_order;
                m_kv["notice_order2"] = notice_order;
                m_kv["missionname"] = "完善工单与通知单后跳转";
                m_kv["role"] = pv.Role_Names;
                dt.data.Add(m_kv);

            
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
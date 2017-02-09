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
using System.Collections;

namespace WebApp.Controllers
{
    public class A14dot1Controller : CommonController
    {

        //
        // GET: /A14dot1/
        public ActionResult Index()
        {
            return View(getA14dot1_Model());
        }
        public ActionResult A14()
        {
            return View();
        }
        public ActionResult WorkFolw_DetailforA14(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
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
                    signal["Jx_Reason"] = Jx_Reason;//计划检修原因 PM？ 
                    signal["Data_Src"] = "A14dot1";
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
        public class Gxqmodel
        {
            public string WE_Ser;
            public int WE_Id;
        }
        //=========================2016.8.7by XuS===========================================================
        /// <summary>
        /// 删除重复行，若查询条件有时间，则会将一个流水号的所有事件查询出来，下面两个函数可将流水号筛选一遍剔除重复
        /// </summary>
        /// <param name="indexList"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static bool IsContain(ArrayList indexList, int index)
        {
            for (int i = 0; i < indexList.Count; i++)
            {
                int tempIndex = Convert.ToInt32(indexList[i]);
                if (tempIndex == index)
                {
                    return true;
                }
            }
            return false;
        }
        public static DataTable DeleteSameRow(DataTable dt, string Field)
        {
            ArrayList indexList = new ArrayList();
            // 找出待删除的行索引   
           
            if (dt.Rows.Count>1)
            { 
            for (int i = 0; i < dt.Rows.Count - 1; i++)
            {
                if (!IsContain(indexList, i))
                {
                    for (int j = i + 1; j < dt.Rows.Count; j++)
                    {
                        if (dt.Rows[i][Field].ToString() == dt.Rows[j][Field].ToString())
                        {
                            indexList.Add(j);
                        }
                    }
                }
            }
            // 根据待删除索引列表删除行   
            for (int i = indexList.Count - 1; i >= 0; i--)
            {
                int index = Convert.ToInt32(indexList[i]);
                dt.Rows.RemoveAt(index);
            }
            }
            
            return dt;

        }
        //========================================================================================================
        public string A14ActiveList(string WorkFlow_Name,string yearandmonth)
        {
            string username = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;


            string WE_Status = "0";
            string query_list = "distinct E.WE_Ser, E.WE_Id, R.username,R.time";
            string query_condition = "E.W_Name='" + WorkFlow_Name + "' and E.WE_Status='" + WE_Status + "' and R.username is not null";
            string record_filter;
            string nexttimepoint;
            string[] month = yearandmonth.Split('/');
            nexttimepoint=month[0]+"/"+(Convert.ToInt16(month[1])+1).ToString();
            record_filter = "time >= '" + yearandmonth + "/" + "1" + " 00:00:00'" + "and  time <= '" + nexttimepoint + "/" + "1" + " 00:00:00'";
            DataTable dt = CWFEngine.QueryAllInformation(query_list, query_condition, record_filter);
            DataTable dt2 = DeleteSameRow(dt, "WE_Ser");
            string str = "";//存返回结果
           
                List<Gxqmodel> Gmlist = new List<Gxqmodel>();
                for (int i = 0; i < dt2.Rows.Count; i++)
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
                str = JsonConvert.SerializeObject(or);
            
            return ("{" + "\"data\": " + str + "}");
        }

        public string A14HistoryList(string WorkFlow_Name, string yearandmonth)
        {
            string username = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;


            string WE_Status = "3";
            string query_list = "distinct E.WE_Ser, E.WE_Id, R.username,R.time";
            string query_condition = "E.W_Name='" + WorkFlow_Name + "' and E.WE_Status='" + WE_Status + "' and R.username is not null";
            string record_filter;
            string nexttimepoint;
            string[] month = yearandmonth.Split('/');
            nexttimepoint = month[0] + "/" + (Convert.ToInt16(month[1]) + 1).ToString();
            record_filter = "time >= '" + yearandmonth + "/" + "1" + " 00:00:00'" + "and  time <= '" + nexttimepoint + "/" + "1" + " 00:00:00'";
            DataTable dt = CWFEngine.QueryAllInformation(query_list, query_condition, record_filter);
            DataTable dt2 = DeleteSameRow(dt, "WE_Ser");
            string str="";//存返回结果
           
            List<Gxqmodel> Gmlist = new List<Gxqmodel>();
            for (int i = 0; i < dt2.Rows.Count; i++)
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
             str = JsonConvert.SerializeObject(or);
            
            return ("{" + "\"data\": " + str + "}");
        }


    }
}
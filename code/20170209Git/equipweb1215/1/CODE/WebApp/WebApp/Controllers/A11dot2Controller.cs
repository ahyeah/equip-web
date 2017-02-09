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
using FlowEngine.DAL;
using FlowEngine.Modals;

namespace WebApp.Controllers
{
    public class A11dot2Controller : CommonController
    {
        EquipManagment Em = new EquipManagment();
        //
        // GET: /A11dot2/
        public ActionResult Index()
        {
            return View(getIndexListRecords("A11dot2", (Session["User"] as EquipModel.Entities.Person_Info).Person_Name));          
        }
        // GET: /A11dot2/装置提报
        public ActionResult ZzSubmit(string wfe_id)
        {
           

            return View(getSubmitModel(wfe_id));
        }

        // GET: /A11dot2/可靠性工程师风险矩阵评估
        public ActionResult PqAssess(string wfe_id)
        { 
            //判断是否为专家团队不同意，需要重新评估的
            int num_HistoryMissions = CWFEngine.GetHistoryMissions(int.Parse(wfe_id)).Count;
            if(num_HistoryMissions>=4)
                ViewBag.zjtdOpinion = 0;

            return View(getWFDetail_Model(wfe_id));
        }
        // GET: /A11dot2/专业团队审核
        public ActionResult ZytdConfirm(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }
        // GET: /A11dot2/车间确立应急预案
        public ActionResult CjCreatePlan(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }
        // GET: /A11dot2/车间组织和实施应急方案
        public ActionResult CjImplementPlan(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }
        // GET: /A11dot2/可靠性工程师监督执行
        public ActionResult PqSupervise(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }
        // GET: /A11dot2/车间确立管控措施
        public ActionResult CjMngCtlPlan(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }
        // GET: /A11dot2/可靠性工程师审核车间管控措施
        public ActionResult PqConfirm(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }
        // GET: /A11dot2/车间组织监督实施
        public ActionResult CjSupervise(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }

        // GET: /A11dot2/专业团队确立管控措施
        public ActionResult PgxzMngCtlPlan(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }

        // GET: /A11dot2/机动处组织监督实施
        public ActionResult JdcSupervise(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }

        // GET: /A11dot2/确认风险是否可控，并进行风险登记
        public ActionResult RiskAcceptRecord(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }
        // GET: /A11dot2/确认风险是否消除，并进行风险登记
        public ActionResult RiskCleanRecord(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }
        // GET: /A11dot2/跳转到[A11.3]风险管控模块--仅用于测试
        public ActionResult JumpToA11dot3(string wfe_id)
        {
            ViewBag.WF_NAME = wfe_id;
            ViewBag.MODULE_NAME = "【A11.3】风险管控模块";           
            return View(getWFDetail_Model(wfe_id));
        }

        public ActionResult WorkFolw_Detail(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }
        //一级菜单
        public ActionResult A11()
        {
            return View();
        }
        public ActionResult fengxiandengji()
        {
            return View();
        }
        
        public string submitsignal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string flowname = item["Flow_Name"].ToString();
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
                //signal["Equip_ABCMark"] = "A";//for test
                signal["Data_Src"] = "人工提报"; //格式统一
                Dictionary<string, string> record = new Dictionary<string, string>();
                record["username"] = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
                record["time"] = DateTime.Now.ToString();
                CWFEngine.SubmitSignal(Convert.ToInt32(flowname), signal, record);
            }
            catch (Exception e)
            {
                return "";
            }
            return ("/A11dot2/Index");
        }

        public string submitAssess_signal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string flowname = item["Flow_Name"].ToString();
                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();                
                string Danger_Intensity = item["Danger_Intensity"].ToString();
                string Time_Level = item["Time_Level"].ToString();
                signal["Danger_Intensity"] = Danger_Intensity;
                signal["Time_Level"] = Time_Level;                
                //RiskMatrix_Color,DangerType_isgreen:根据逻辑判断   
                RiskMatrixElement rme = riskMatrixAnalysis(Danger_Intensity, Time_Level);
                signal["RiskMatrix_Color"] = rme.color;
                signal["DangerType_isgreen"] = rme.DangerType_isgreen;
                signal["Assess_done"] = "true";
                //record:
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
            return ("/A11dot2/Index");
        }

        public string submitZytdConfirm_signal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string flowname = item["Flow_Name"].ToString();
                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();
                signal["ZytdConfirm_Result"] = item["ZytdConfirm_Result"].ToString();
                signal["ZytdConfirm_Reason"] = item["ZytdConfirm_Reason"].ToString();
                ////DangerType_isgreen:需要根据逻辑判断
                //UI_MISSION miModel = CWFEngine.GetActiveMission<Person_Info>(int.Parse(flowname), ((IObjectContextAdapter)(new EquipWebContext())).ObjectContext);
                //RiskMatrixElement rme = riskMatrixAnalysis(miModel.Miss_Params["Danger_Intensity"].ToString(), miModel.Miss_Params["Time_Level"].ToString());
                //signal["DangerType_isgreen"] = rme.DangerType_isgreen;
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
            return ("/A11dot2/Index");
        }

        //风险矩阵分析
        public RiskMatrixElement riskMatrixAnalysis(string Danger_Intensity, string Time_Level)
        {
            
            string DangerType_isgreen = "";
            string color = "";
            int danger_intensity=int.Parse(Danger_Intensity.Substring(1,1));
            int time_level=int.Parse(Time_Level.Substring(1,1));
            if ((danger_intensity == 1 && time_level <= 3) || (danger_intensity == 2 && time_level <= 2))
            {
                color = "red";
                DangerType_isgreen = "false";
            }
            else if (danger_intensity >= 5 || (danger_intensity == 4 && time_level >= 2) || (danger_intensity == 3 && time_level >= 3))
            {
                color = "green";
                DangerType_isgreen = "true";
            }
            else
            {
                color = "yellow";
                DangerType_isgreen = "false";
            }
            //
            RiskMatrixElement rme = new RiskMatrixElement();
            rme.danger_intensity = danger_intensity;
            rme.time_level = time_level;
            rme.color = color;
            rme.DangerType_isgreen = DangerType_isgreen;
            return rme;
        }

        public string submitCreatePlan_signal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string flowname = item["Flow_Name"].ToString();
                Dictionary<string, string> signal = new Dictionary<string, string>();
                //paras
                signal["Plan_Desc"] = item["Plan_Desc"].ToString();
                signal["Plan_DescFilePath"] = item["Plan_DescFilePath"].ToString();
                signal["CreatePlan_done"] = "true";
                Dictionary<string, string> record = new Dictionary<string, string>();


                record["username"] = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
                record["time"] = DateTime.Now.ToString();

                CWFEngine.SubmitSignal(Convert.ToInt32(flowname), signal, record);
            }
            catch (Exception e)
            {
                return "";
            }
            return ("/A11dot2/Index");
        }

        public string submitImplementPlan_signal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string flowname = item["Flow_Name"].ToString();
                Dictionary<string, string> signal = new Dictionary<string, string>();
                //paras
                signal["ImplementPlan_done"] = item["ImplementPlan_done"].ToString();

                Dictionary<string, string> record = new Dictionary<string, string>();


                record["username"] = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
                record["time"] = DateTime.Now.ToString();

                CWFEngine.SubmitSignal(Convert.ToInt32(flowname), signal, record);
            }
            catch (Exception e)
            {
                return "";
            }
            return ("/A11dot2/Index");
        }

        public string submitSupervise_signal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string flowname = item["Flow_Name"].ToString();
                Dictionary<string, string> signal = new Dictionary<string, string>();
                //paras
                signal["Supervise_done"] = item["Supervise_done"].ToString();
                Dictionary<string, string> record = new Dictionary<string, string>();
                record["username"] = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
                record["time"] = DateTime.Now.ToString();

                CWFEngine.SubmitSignal(Convert.ToInt32(flowname), signal, record);
            }
            catch (Exception e)
            {
                return "";
            }
            return ("/A11dot2/Index");
        }
        public string CjMngCtlPlan_submitsignal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string flowname = item["Flow_Name"].ToString();
                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();
                signal["CreatePlan_done"] = item["CreatePlan_done"].ToString();
                signal["Plan_Desc"] = item["Plan_Desc"].ToString();
                signal["Plan_DescFilePath"] = item["Plan_DescFilePath"].ToString();
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
            return ("/A11dot2/Index");
        }

        public string PqConfirm_submitsignal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string flowname = item["Flow_Name"].ToString();
                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();
                signal["PqConfirm_done"] = item["PqConfirm_done"].ToString();
                signal["PqConfirm_Result"] = item["PqConfirm_Result"].ToString();
                signal["PqConfirm_Reason"] = item["PqConfirm_Reason"].ToString();
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
            return ("/A11dot2/Index");
        }

        public string CjSupervise_submitsignal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string flowname = item["Flow_Name"].ToString();
                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();
                signal["ImplementPlan_done"] = item["ImplementPlan_done"].ToString();
                signal["flag_NandD"] = "一般风险";

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
            return ("/A11dot2/Index");
        }

        public string PgxzMngCtlPlan_submitsignal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string flowname = item["Flow_Name"].ToString();
                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();
                signal["CreatePlan_done"] = item["CreatePlan_done"].ToString();
                signal["Plan_Desc"] = item["Plan_Desc"].ToString();
                signal["Plan_DescFilePath"] = item["Plan_DescFilePath"].ToString();
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
            return ("/A11dot2/Index");
        }

        public string JdcSupervise_submitsignal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string flowname = item["Flow_Name"].ToString();
                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();
                signal["ImplementPlan_done"] = item["ImplementPlan_done"].ToString();
                signal["flag_NandD"] = "重大风险";
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
            return ("/A11dot2/Index");
        }

        public string RiskAcceptRecord_submitsignal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string flowname = item["Flow_Name"].ToString();
                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();
                signal["RiskAcceptRecord_done"] = item["RiskAcceptRecord_done"].ToString();
                signal["RiskIsAcceptable"] = item["Risk_IsAcceptable"].ToString();

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
            return ("/A11dot2/Index");
        }
      
        //test
        public string submitA11dot3_signal(string json1)
        {
            JObject item = (JObject)JsonConvert.DeserializeObject(json1);
            string flowname = item["Flow_Name"].ToString();
            Dictionary<string, string> signal = new Dictionary<string, string>();
             //paras
            signal["temp_A11dot3"] = item["temp_A11dot3"].ToString();
            Dictionary<string, string> record = new Dictionary<string, string>();
            record["username"] = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
            record["time"] = DateTime.Now.ToString();
            CWFEngine.SubmitSignal(Convert.ToInt32(flowname), signal, record);
                return ("/A11dot2/Index");
        }

        public ActionResult WorkFolw_DetailforA11(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
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
            if (dt.Rows.Count > 1)
            {
                // 找出待删除的行索引   
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
        public string A11MissList(string WorkFlow_Name)
        {
            string username = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;

            DateTime nowtime = DateTime.Now;
            string record_filter;
            string WE_Status = "0";
            string query_list = "distinct E.WE_Ser, E.WE_Id, R.username,R.time";
            string query_condition = "E.W_Name='" + WorkFlow_Name  + "' and R.username is not null";
            record_filter = "time >= '" + nowtime.Year + "/" + nowtime.Month + "/" + "1" + " 00:00:00'" + "and  time <= '" + nowtime.AddMonths(1).Year + "/" + nowtime.AddMonths(1).Month + "/" + "1" + " 00:00:00'";
            
            DataTable dt = CWFEngine.QueryAllInformation(query_list, query_condition, record_filter);
            DataTable dt2= DeleteSameRow( dt,"WE_Ser");
            //DataView dv = new DataView(dt); //虚拟视图吧，我这么认为
            //DataTable dt2 = dv.ToTable(true, "WE_Ser","WE_Id");
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
            string str = JsonConvert.SerializeObject(or);
            return ("{" + "\"data\": " + str + "}");
        }
	}
}
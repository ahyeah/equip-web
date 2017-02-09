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
using WebApp.Models.DateTables;
using System.Collections.Specialized;
using EquipBLL.AdminManagment.MenuConfig;
using FlowEngine.TimerManage;
using FlowEngine.Modals;

namespace WebApp.Controllers
{
    public class DSA5dot1Controller : Controller
    {
        //
        // GET: /DSA5dot1/
        //
        // GET: /LSA5dot1/
        PersonManagment pm = new PersonManagment();
        TablesManagment tm = new TablesManagment();
        EquipManagment Em = new EquipManagment();
        EquipArchiManagment Eam = new EquipArchiManagment();
        public ActionResult ZzSubmit(string wfe_id)
        {
            UI_MISSION mi = CWFEngine.GetActiveMission<Person_Info>(int.Parse(wfe_id), ((IObjectContextAdapter)(new EquipWebContext())).ObjectContext);
            Dictionary<string, object> mi_params = mi.Miss_Params;
            EquipArchiManagment em = new EquipArchiManagment();
            int Zz_id = em.getEa_idbyname(mi.Miss_Params["Zz_Name"].ToString());
            ViewBag.Zz_id = Zz_id;
            ViewBag.Zz_name = mi.Miss_Params["Zz_Name"].ToString();
            ViewBag.wfe_id = wfe_id;
            return View();
        }
        public string ZzSubmit_Bcsignal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);



                CTimerTimeout timeout_job = new CTimerTimeout();
                timeout_job.CreateTime = DateTime.Now;
                timeout_job.for_using = TIMER_USING.FOR_SYSTEM;
                timeout_job.mission_name = "";
                timeout_job.status = TM_STATUS.TM_STATUS_ACTIVE;
                timeout_job.SetActionForWF("INVILID");
                timeout_job.AttachWFEntityID = Convert.ToInt32(item["wfe_id"]);
                timeout_job.CustomAction = "http://localhost/CallBack/testCallBack";
                timeout_job.EventName = "PqAessess";
                timeout_job.SetTriggerTiming("0 0 0 20 * ?");
                //保存该定时任务，并将其添加到激活任务列表 
                timeout_job.Save();
                int m_timerMissionID = timeout_job.ID;
                CTimerManage.AppendMissionToActiveList(timeout_job);

              
                string[] notgood = item["Incomplete_content"].ToString().Split('|');
                A5dot1Tab1 a5dot1Tab1 = new A5dot1Tab1();
                DateTime my = DateTime.Now;
                string yearmonth = "";
                int cjid = Em.getEA_parentid(Convert.ToInt32(item["Zz_Id"]));
                string cjname = Eam.getEa_namebyId(cjid);
                string Username = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
                if (my.Day >= 15)
                {
                    yearmonth = my.Year.ToString() + my.AddMonths(1).Month.ToString();
                }
                else
                {
                    yearmonth = my.Year.ToString() + my.Month.ToString();
                }

                if (notgood[notgood.Count() - 1] == "")
                {
                    for (int i = 0; i < notgood.Count() - 1; i++)
                    {
                        a5dot1Tab1.cjName = cjname;
                        a5dot1Tab1.zzName = item["Zz_Name"].ToString();
                        a5dot1Tab1.sbGyCode = item["Equip_GyCode"].ToString();
                        a5dot1Tab1.sbCode = item["Equip_Code"].ToString();
                        a5dot1Tab1.sbType = item["Equip_Type"].ToString();
                        a5dot1Tab1.zyType = item["Zy_Type"].ToString();
                        a5dot1Tab1.notGoodContent = notgood[i];
                        a5dot1Tab1.isRectified = 0;
                        a5dot1Tab1.zzSubmitTime = DateTime.Now;
                        a5dot1Tab1.zzUserName = Username;
                        a5dot1Tab1.yearMonthForStatistic = yearmonth;
                        a5dot1Tab1.temp3 = m_timerMissionID.ToString();
                        if (item["wfe_id"].ToString() != "")
                            a5dot1Tab1.dataSource = item["wfe_id"].ToString();
                        if (a5dot1Tab1.cjName == "消防队" || a5dot1Tab1.cjName == "计量站")
                            a5dot1Tab1.pqName = a5dot1Tab1.cjName;
                        else
                            a5dot1Tab1.pqName = pm.Get_PqnamebyCjname(a5dot1Tab1.cjName.ToString());
                        tm.Zzsubmit(a5dot1Tab1);
                    }
                }
                else
                {
                    for (int i = 0; i < notgood.Count(); i++)
                    {
                        a5dot1Tab1.cjName = cjname;
                        a5dot1Tab1.zzName = item["Zz_Name"].ToString();
                        a5dot1Tab1.sbGyCode = item["Equip_GyCode"].ToString();
                        a5dot1Tab1.sbCode = item["Equip_Code"].ToString();
                        a5dot1Tab1.sbType = item["Equip_Type"].ToString();
                        a5dot1Tab1.zyType = item["Zy_Type"].ToString();
                        a5dot1Tab1.notGoodContent = notgood[i];
                        a5dot1Tab1.isRectified = 0;
                        a5dot1Tab1.zzSubmitTime = DateTime.Now;
                        a5dot1Tab1.zzUserName = Username;
                        a5dot1Tab1.yearMonthForStatistic = yearmonth;
                        a5dot1Tab1.temp3 = m_timerMissionID.ToString();
                        if (item["wfe_id"].ToString() != "")
                            a5dot1Tab1.dataSource = item["wfe_id"].ToString();
                        if (a5dot1Tab1.cjName == "消防队" || a5dot1Tab1.cjName == "计量站")
                            a5dot1Tab1.pqName = a5dot1Tab1.cjName;
                        else
                            a5dot1Tab1.pqName = pm.Get_PqnamebyCjname(a5dot1Tab1.cjName.ToString());
                        tm.Zzsubmit(a5dot1Tab1);
                    }
                }
                string wfe_id = item["wfe_id"].ToString();
                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();
                signal["ZzSubmit_done"] = "true";

                //record
                Dictionary<string, string> record = new Dictionary<string, string>();
                record["username"] = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
                record["time"] = DateTime.Now.ToString();
                //submit
                CWFEngine.SubmitSignal(Convert.ToInt32(wfe_id), signal, record);
            }
            catch (Exception e)
            {
                return "";
            }
            return ("/A5dot1/Index");
        }

        //public string ZzSubmit_submitsignal(string json1)
        //{
        //    try
        //    {
        //        JObject item = (JObject)JsonConvert.DeserializeObject(json1);
        //        string wfe_id = item["wfe_id"].ToString();
        //        //paras
        //        Dictionary<string, string> signal = new Dictionary<string, string>();
        //        signal["ZzSubmit_done"] = "true";

        //        //record
        //        Dictionary<string, string> record = new Dictionary<string, string>();
        //        record["username"] = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
        //        record["time"] = DateTime.Now.ToString();
        //        //submit
        //        CWFEngine.SubmitSignal(Convert.ToInt32(wfe_id), signal, record);
        //    }
        //    catch (Exception e)
        //    {
        //        return "";
        //    }
        //    return ("/A5dot1/Index");
        //}
    }
}
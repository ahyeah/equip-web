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
using FlowEngine.DAL;
using FlowEngine.Modals;
using FlowEngine.TimerManage;

namespace WebApp.Controllers
{
    public class CallBackController : Controller
    {
        //
        // GET: /CallBack/
        Jobs js = new Jobs();
        DsWorkManagment dm = new DsWorkManagment();
        public ActionResult Index()
        {
            return View();
        }
        public string GetDSRemind()
        {
            List<Timer_Jobs> Tjs = js.GetDSRemind();
            List<string> Remind_list = new List<string>();
           
            for (int i = 0; i < Tjs.Count;i++)
            {
                //TriggerTiming TT = new TriggerTiming();
                //TT.FromString(Tjs[i].corn_express);
                //TT.RefreshNextTiming(DateTime.Now);
                //DateTime next_dt = TT.NextTiming.Value;
                UI_MISSION u = CWFEngine.GetActiveMission<Person_Info>(Tjs[i].workflow_ID, null, false);
                string work_name = u.WE_Name;
                string event_desc = u.WE_Event_Desc;
                string event_name = work_name + ":" + event_desc;
                Remind_list.Add(event_name);
            }
            string Remind_Str = "定时任务提醒！！！请按时完成以下流程：";
            for (int i = 0; i < Remind_list.Count; i++)
            {
                Remind_Str += Remind_list[i] + "、";
            
            }

            return Remind_Str;
        }
        public static int WeekOfMonth(DateTime day, int WeekStart)
        {
            //WeekStart                                                                      
            //1表示 周一至周日 为一周                                                        
            //2表示 周日至周六 为一周                                                        
            DateTime FirstofMonth;
            FirstofMonth = Convert.ToDateTime(day.Date.Year + "-" + day.Date.Month + "-" + 1);
            int i = (int)FirstofMonth.Date.DayOfWeek;
            if (i == 0)
            {
                i = 7;
            }
            if (WeekStart == 1)
            {
                return (day.Date.Day + i - 2) / 7 + 1;
            }
            if (WeekStart == 2)
            {
                return (day.Date.Day + i - 1) / 7;
            }
            return 0;
            //错误返回值0                                                                    
        } 
        public void testCallBack(int timer_id, string user_params)
        {
            JObject item = (JObject)JsonConvert.DeserializeObject(user_params);
            int eneity_id = Convert.ToInt32(item["entity_id"]);
            string event_name = item["event_name"].ToString();
            DSEventDetail ds = new DSEventDetail();
            DateTime now = DateTime.Now;
            ds.year = now.Year;
            ds.month = now.Month;
            ds.day = now.Day;
            ds.week = WeekOfMonth(now,1);
            ds.state =0;
            ds.entity_id = eneity_id;
            //List<UI_MISSION> AllHistoryMiss = CWFEngine.GetHistoryMissions(eneity_id);
            UI_MISSION u = CWFEngine.GetActiveMission<Person_Info>(eneity_id, null, false);
            string work_name = u.WE_Name; 
            string event_desc = u.WE_Event_Desc;
            ds.event_name = work_name + ":" + event_desc;
            
            Timer_Jobs tj = js.GetTimerJob(timer_id);
            ds.DSTime_Desc = tj.STR_RES_2;
            //if (dm.getdetailbyE_id(eneity_id) == null)
            dm.AddDsEvent(ds);   
        }


        public void CSA11dot2ZzSubmit(string param)
        {
            JObject item = (JObject)JsonConvert.DeserializeObject(param);
            int entity_id = Convert.ToInt32 (item["Entity_id"]);
            UI_MISSION u = CWFEngine.GetActiveMission<Person_Info>(entity_id, null, false);
            string work_name = u.WE_Name;
            string event_desc = u.WE_Event_Desc;
            string event_name = work_name + ":" + event_desc;
            dm.modifystate(entity_id, event_name);
        }

        public void A5dot1ZzSubmit()
        { 
        
        
        }

        /// <summary>
        /// 有以前的通过Excel表读取数据模式改为现在的直接读取ERP中数据，通过周期调用此函数，周期存库
        /// </summary>
        /// <param name="json1"></param>
        /// <returns></returns>
        public void ERPSubmit()
        {
            try
            {
                PersonManagment PM = new PersonManagment();
                A6dot2Managment WM = new A6dot2Managment();
                A6dot2Tab1 WDT_list = new A6dot2Tab1();
                ERPInfoManagement erp = new ERPInfoManagement();
                EquipManagment EM = new EquipManagment();

                string EquipPhaseB;
                WDT_list.uploadDesc = "";//字段保留，（以前用于上传五定表的描述）
                WDT_list.uuploadFileName = "";//字段保留，（以前用于上传五定表的名字）
                //string[] savedFileName = WDT_list.uuploadFileName.Split(new char[] { '$' });
                //string wdt_filename = Path.Combine(Request.MapPath("~/Upload"), savedFileName[1]);//没有用到的变量
                //WDT_list.userName = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
                WDT_list.userName = "自动提取自ERP";
                WDT_list.uploadtime = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
                //WDT_list.pqName = PM.Get_Person_Depart((Session["User"] as EquipModel.Entities.Person_Info).Person_Id).Depart_Name;
                WDT_list.pqName = "待定";
                int WDT_ID = WM.add_WDT_list(WDT_list);

                List<OilInfo> OilInfo_Overdue = erp.getOilInfo_overdue();

                List<A6dot2Tab2> wdt_list = new List<A6dot2Tab2>();
                foreach (var i in OilInfo_Overdue)
                {

                    A6dot2Tab2 tmp = new A6dot2Tab2();
                    tmp.isValid = 1;
                    tmp.equipCode = i.oil_EquipCode;
                    tmp.equipDesc = i.oil_EquipDesc;
                    tmp.funLoc = i.oil_Fun_Loc;
                    tmp.funLoc_desc = i.oil_Fun_LocDesc;
                    tmp.oilLoc = i.oil_Loc;
                    tmp.oilLoc_desc = i.oil_Loc;
                    tmp.oilInterval = Convert.ToInt32(i.oil_Interval);
                    tmp.unit = i.oil_Unit;
                    tmp.lastOilTime = i.oil_LastDate;
                    tmp.lastOilNumber = Convert.ToDouble(i.oil_LastNum);
                    tmp.lastOilUnit = i.oil_Unit;
                    tmp.NextOilTime = i.oil_NextDate.ToString();
                    tmp.NextOilNumber = Convert.ToDouble(i.oil_NextNum);
                    tmp.NextOilUnit = i.oil_Unit2;
                    tmp.oilCode = i.oil_Code;
                    tmp.oilCode_desc = i.oil_Desc;
                    tmp.substiOilCode = "";
                    tmp.substiOilCode_desc = "";
                    if (EM.getEquip_Info(tmp.equipCode) != null)
                    {
                        EquipPhaseB = EM.getEquip_Info(tmp.equipCode).Equip_PhaseB;
                        if (EquipPhaseB==null)
                            tmp.isOilType = 0;
                        else
                        { 
                        if (EquipPhaseB.Equals("机泵") || EquipPhaseB.Equals("风机"))
                            tmp.isOilType = 1;
                        else
                            tmp.isOilType = 0;
                        }
                        List<Equip_Archi> ZzCj = EM.getEquip_ZzBelong(EM.getEquip_Info(tmp.equipCode).Equip_Id);
                        tmp.equip_ZzName = ZzCj.First().EA_Name;
                        tmp.equip_CjName = ZzCj.Last().EA_Name;
                        tmp.equip_PqName = EM.GetPqofZz(tmp.equip_ZzName).Pq_Name;
                    }

                    tmp.isExceed = 1;                   
                    tmp.Tab1_Id = WDT_ID;
                    wdt_list.Add(tmp);
                }
                WM.add_WDT_content(WDT_ID, wdt_list);
                /*  foreach(var i in wdt_content)
                  {
                      return i.equip_CjName;
                  }
                 * */

               // return "/A6dot2/Index_Tj";
            }
            catch { }

        }
	}
}
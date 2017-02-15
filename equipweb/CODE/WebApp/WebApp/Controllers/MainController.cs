using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EquipBLL.AdminManagment.MenuConfig;
using EquipBLL.AdminManagment;
using EquipModel.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using FlowEngine;
using FlowEngine.UserInterface;
using EquipModel.Context;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data;
using FlowEngine.Modals;
using FlowEngine.DAL;
using FlowEngine.TimerManage;
using System.Xml;

namespace WebApp.Controllers
{
    public class MainController :CommonController
    {
        // GET: Main
        private SysMenuManagment menuconfig = new SysMenuManagment();
        private PersonManagment PM = new PersonManagment();

        public class getIndexmodel
        {
            public List<MenuListNode> mt;
            public int isManager;
            public string username;
            public int userid;
            public string belong_depart;
            
        }
        public ActionResult Index1()
        {
            MenuTree mtree = menuconfig.BuildMenuTree();            
            return View(mtree);
        }
        public ActionResult Index()
        {
            //CWFEngine.CreateAWFEntityByName("");
            List<MenuListNode> mt = menuconfig.BuildMenuList();
            int a = mt.Count();
            return View(mt);
        }
        public ActionResult Index0()
        {
            if(Session["User"]==null)
            {
                Response.Redirect("/Login/chaoshi");
                return View();
            }
            else
            {

            
            getIndexmodel model = new getIndexmodel();

            model.mt = menuconfig.BuildMenuList();
            model.username = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
            model.userid = (Session["User"] as EquipModel.Entities.Person_Info).Person_Id;
            PersonManagment PM = new PersonManagment();
            EquipBLL.AdminManagment.PersonManagment.P_viewModal pm_info = PM.Get_PersonModal(model.userid);
            model.belong_depart = pm_info.Department_Name;

            model.isManager = PM.is_Role_admin(model.username); 

            //int a = mt.Count();
            return View(model);
            }
        }
        // Get: Home
        public class homemodel
        {
            public int[] q_cunzai;
            public string userName;
            public int userId;
            public string A7dot1Isdone;
            public string A6dot2Isdone;
            public string A6dot2SjFile;
        }

        public ActionResult Home()
        {
            homemodel hm = new homemodel();
            hm.userName = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name.ToString();
            hm.userId = (Session["User"] as EquipModel.Entities.Person_Info).Person_Id;
            PersonManagment pm = new PersonManagment();
            //string cj = pm.Get_Person_Cj(hm.userId).First().EA_Name;
            string cj = "联合一车间";
            EquipBLL.AdminManagment.PersonManagment.P_viewModal pv = pm.Get_PersonModal(hm.userId);
            QEntranceManagment qem = new QEntranceManagment();
            List<Quick_Entrance> qe = qem.GetQ_EbyP_Id(hm.userId);
            hm.q_cunzai = new int[8] { 0, 0, 0, 0, 0, 0, 0, 0 };
            if (qe != null)
            {
                foreach (var q in qe)
                {
                    hm.q_cunzai[q.xuhao - 1] = 1;
                }
            }
            //if (pv.Role_Names == "现场工程师")
            if (pv.Role_Names.Contains("现场工程师"))
            {



                DateTime now = DateTime.Now;
                DateTime LastThursday = now;
                DateTime Day20 = now;
                string a = now.DayOfWeek.ToString("d");
                if (a == "1")
                {
                    LastThursday = now.AddDays(-4).AddHours(-now.Hour).AddMinutes(-now.Minute).AddSeconds(-now.Second);
                }
                if (a == "2")
                {
                    LastThursday = now.AddDays(-5).AddHours(-now.Hour).AddMinutes(-now.Minute).AddSeconds(-now.Second);
                }
                if (a == "3")
                {
                    LastThursday = now.AddDays(-6).AddHours(-now.Hour).AddMinutes(-now.Minute).AddSeconds(-now.Second);
                }
                if (a == "4")
                {
                    LastThursday = now.AddHours(-now.Hour).AddMinutes(-now.Minute).AddSeconds(-now.Second);

                }
                if (a == "5")
                {
                    LastThursday = now.AddDays(-1).AddHours(-now.Hour).AddMinutes(-now.Minute).AddSeconds(-now.Second);

                }
                if (a == "6")
                {
                    LastThursday = now.AddDays(-2).AddHours(-now.Hour).AddMinutes(-now.Minute).AddSeconds(-now.Second);

                }
                if (a == "0")
                {
                    LastThursday = now.AddDays(-3).AddHours(-now.Hour).AddMinutes(-now.Minute).AddSeconds(-now.Second);

                }
                if (now.Day >= 20)
                {
                    Day20 = now.AddDays(20 - now.Day).AddHours(-now.Hour).AddMinutes(-now.Minute).AddSeconds(-now.Second);
                    string query_list1 = "E.WE_Ser,R.time";
                    string query_condition1 = " P.Cj_Name ='" + cj + "' and E.W_Name='A6dot2' and M.Miss_Name ='Xc_Sample' ";
                    string record_filter1 = "Convert(datetime,time)>'" + Day20.ToString() + "' and Convert(datetime,time)<'" + now.ToString() + "'";
                    DataTable dt1 = CWFEngine.QueryAllInformation(query_list1, query_condition1, record_filter1);
                    if (dt1.Rows.Count != 0)
                        hm.A6dot2Isdone = "false";
                    else
                        hm.A6dot2Isdone = "true";
                }
                else
                {
                    hm.A6dot2Isdone = "false";
                }
                string query_list = "E.WE_Ser,R.time";
                string query_condition = " P.Cj_Name ='" + cj + "' and E.W_Name='A7dot1dot1'";
                string record_filter = "Convert(datetime,time)>'" + LastThursday.ToString() + "' and Convert(datetime,time)<'" + now.ToString() + "'";
                DataTable dt = CWFEngine.QueryAllInformation(query_list, query_condition, record_filter);
               if(dt!=null)
               {
                   if (dt.Rows.Count != 0)
                       hm.A7dot1Isdone = "true";
                   else
                       hm.A7dot1Isdone = "false";
               }
               
            }
            else
            {
                hm.A7dot1Isdone = "true";

            }
            //注：这里判断的是部门为设监中心的
            if (pv.Department_Name.Contains("设监中心"))
            {
                DateTime now = DateTime.Now;
                DateTime Day27 = now;
                if (now.Day >= 27)
                {
                    Day27 = now.AddDays(27 - now.Day).AddHours(-now.Hour).AddMinutes(-now.Minute).AddSeconds(-now.Second);
                    string query_list1 = "E.WE_Ser,R.time";
                    string query_condition1 = "E.W_Name='A6dot2' and M.Miss_Name ='Sj_Result' ";
                    string record_filter1 = "Convert(datetime,time)>'" + Day27.ToString() + "' and Convert(datetime,time)<'" + now.ToString() + "'";
                    DataTable dt1 = CWFEngine.QueryAllInformation(query_list1, query_condition1, record_filter1);
                    if (dt1.Rows.Count != 0)
                        hm.A6dot2SjFile = "false";
                    else
                        hm.A6dot2SjFile = "true";
                }
                else
                {
                    hm.A6dot2SjFile = "false";
                }

            }
            else
            {
                hm.A6dot2SjFile = "false";

            }



            return View(hm);
        }

        //public string CreateFlow(string flowname)
        //{
        //    UI_WorkFlow_Entity wfe = CWFEngine.CreateAWFEntityByName(flowname);
        //    if (wfe != null)
        //    {
        //        Dictionary<string, string> record = wfe.GetRecordItems();
        //            record["username"] = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
        //            record["time"] = DateTime.Now.ToString();
        //        return wfe.Start(record);
        //        //Json(new { url = wfe.Start(record), wfe_id = wfe.EntityID }); 
        //        //"{url:'" + wfe.Start(record) + "', wfe_id:'" + wfe.EntityID + "'}";

        //    }
        //    else
        //        return null;
        //}
        public string CreateFlow(string flowname)
        {
            UI_WorkFlow_Entity wfe = CWFEngine.CreateAWFEntityByName(flowname);
            if (wfe != null)
            {
                Dictionary<string, string> record = wfe.GetRecordItems();
                record["username"] = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
                record["time"] = DateTime.Now.ToString();
                wfe.Start(record);
                Dictionary<string, string> signal = new Dictionary<string, string>();

                signal["currentuser"] = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
                signal["start_done"] = "true";
                //submit
                CWFEngine.SubmitSignal(wfe.EntityID, signal, record);



                CWorkFlow wf = new CWorkFlow();
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(CWFEngine.GetWorkFlowEntiy(wfe.EntityID, true).Binary);
                wf.InstFromXmlNode(doc.DocumentElement);

                string returl = "";
                if (wf.GetCurrentEvent().CheckAuthority<Person_Info>((Dictionary<string, object>)Session[CWFEngine.authority_params], ((IObjectContextAdapter)(new EquipWebContext())).ObjectContext))
                {
                    returl = wf.GetCurrentEvent().currentaction + "?wfe_id=" + wfe.EntityID.ToString();
                    //如果权限认证通过则返回正确的页面URL
                    return returl;
                    //return returl;
                }
                else
                {
                    //如果权限认证不通过， 则删除刚创建的工作流实体， 并返回 -1
                    CWFEngine.RemoveWFEntity(wfe.EntityID);
                    return "-1";
                }
                //Json(new { url = wfe.Start(record), wfe_id = wfe.EntityID }); 
                //"{url:'" + wfe.Start(record) + "', wfe_id:'" + wfe.EntityID + "'}";

            }
            else
                return null;
        }
        /// <summary>
        /// 获得当前的任务列表
        /// </summary>
        /// <returns>前台JS解析的任务JSon数据</returns>
        [HttpPost]
        public JsonResult ListMission()
        {
            IObjectContextAdapter IOca = new EquipWebContext();

            List<UI_MISSION> miss = CWFEngine.GetActiveMissions<Person_Info>(IOca.ObjectContext);
            
            List<Object> miss_obj = new List<object>();

            foreach(UI_MISSION item in miss)
            {
                UI_MISSION mi = CWFEngine.GetHistoryMissions(item.WE_Entity_Id).Last();
                IDictionary<string, string> record = CWFEngine.GetMissionRecordInfo(mi.Miss_Id);
                DateTime dt = DateTime.Parse(!record.ContainsKey("time") ? DateTime.Now.ToString() : record["time"]);
                TimeSpan ts = DateTime.Now - dt;
                string time_last = "";
                if (ts.TotalDays > 1)
                    time_last += (((int)ts.TotalDays).ToString() + "天之前");
                else if (ts.TotalHours > 1)
                    time_last += (((int)ts.TotalHours).ToString() + "小时之前");
                else
                    time_last += (((int)ts.TotalMinutes).ToString() + "分钟之前");
                object o = new  {
                                WF_ICON = "fa fa-flash text-text-aqua",
                                MISS_Url = item.Mission_Url,
                                WF_Name = CWFEngine.GetWorkFlowEntiy(item.WE_Entity_Id, true).description,
                                MISS_Name = item.WE_Event_Desc,
                                TIME_Last = time_last};
                
                miss_obj.Add(o);
            }
            return Json(miss_obj.ToArray());
        }

        public bool checktimeout()
        {
            bool result = false;
            if (Session["User"] != null)
                result = true;
            return result;
        }
        //菜单栏的提示任务个数
        public JsonResult task_mission()
        {
            IObjectContextAdapter IOca = new EquipWebContext();
            List<UI_MISSION> miss = CWFEngine.GetActiveMissions<Person_Info>(IOca.ObjectContext);
            List<Object> Url_List = new List<object>();
            List<Object> urlobj = new List<object>();
            foreach (UI_MISSION item in miss)//本模块的作用是：将MISS_Url中截取前面的字段；例如“/A4dot4/Zzsubmit?wef=123”截取“A4dot4”，并存入数组str。注：这些MISS_Url就是待处理任务的跳转路径，故与本模块待处理任务个数提示可以关联
            {
                UI_MISSION mi = CWFEngine.GetHistoryMissions(item.WE_Entity_Id).Last();
                string MISS_Url = item.Mission_Url;
                string[] s = MISS_Url.Split(new char[] { '/' });
                string str = s[1];
                Url_List.Add(str);
            }
            //判断数组str中的相同元素的个数，并将结果转为json格式，返回前台
            var vs = from string p in Url_List group p by p into g select new { g, num = g.Count() };
            foreach (var v in vs)
            {

                object m = new
                {
                    url_name = v.g.Key,
                    url_count = v.num
                };
                urlobj.Add(m);

            }

            return Json(urlobj.ToArray());
        }

        //zxh

        public string ListMission_zxh()
        {

            try
            {
                IObjectContextAdapter IOca = new EquipWebContext();

                List<UI_MISSION> miss = CWFEngine.GetActiveMissions<Person_Info>(IOca.ObjectContext);


                List<Object> miss_obj = new List<object>();
                string userName = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name.ToString();
                foreach (UI_MISSION item in miss)
                {
                    MainMissionsModel mm = GetMainMissionsInfo(item.WE_Entity_Id);
   

                    Jobs js = new Jobs();
                    string endtime = "";
                    string lsh_xy = "1";


                    if (js.GetDSnexttime(mm.jobname, item.WE_Entity_Id) != null)
                    {
                        if (js.GetDSnexttime(mm.jobname, item.WE_Entity_Id).PreTime == null)
                        {

                            string end_corn = js.GetDSnexttime(mm.jobname, item.WE_Entity_Id).corn_express;
                            TriggerTiming TT = new TriggerTiming();
                            TT.FromString(end_corn);
                            TT.RefreshNextTiming(DateTime.Now);
                            endtime = TT.NextTiming.ToString();
                        }
                        else
                        {

                            endtime = js.GetDSnexttime(mm.jobname, item.WE_Entity_Id).PreTime.ToString();
                        }
                    }
         

                    if (mm.Equip_GyCode == null || mm.Equip_GyCode == "")
                        lsh_xy = "0";
                    object o = new
                    {
                        WF_ICON = "fa fa-flash text-text-aqua",
                        MISS_Url = item.Mission_Url,
                        WF_Name = mm.WF_Name,
                        MISS_Name = item.WE_Event_Desc,
                        wfe_serial = mm.wfe_serial,
                        sbCode = mm.Equip_GyCode,
                        time = mm.time,
                        endtime = endtime,
                        lsh_xy = lsh_xy

                    };
                    EquipManagment em=new EquipManagment ();
                    if (mm.WF_Name.Contains("定时KPI月报"))
                    {
                        if (userName == "龚海桥" && mm.WF_Name.Contains("联合一片区"))
                            miss_obj.Add(o);
                        else if (userName == "丁一刚" && mm.WF_Name.Contains("联合二片区"))
                            miss_obj.Add(o);
                        else if (userName == "邓杰" && (mm.WF_Name.Contains("联合三片区") || mm.WF_Name.Contains("化工片区")))
                            miss_obj.Add(o);
                        else if (userName == "杨书毅" && mm.WF_Name.Contains("联合四片区"))
                            miss_obj.Add(o);
                        else if (userName == "武文斌" && (mm.WF_Name.Contains("综合片区") || mm.WF_Name.Contains("系统片区")))
                            miss_obj.Add(o);
                    }
                    else
                    {
                        if (userName == "sa" || userName == "程聂")
                            miss_obj.Add(o);
                        else
                        {
                            if(lsh_xy=="1")
                            {
                                if(em.getEquip_ByGyCode(mm.Equip_GyCode).Equip_Specialty=="动")
                                    miss_obj.Add(o);
                            }
                            else
                                miss_obj.Add(o);
                        }
                    }
                        

                }

                TablesManagment tm = new TablesManagment();
                int UserId = (Session["User"] as EquipModel.Entities.Person_Info).Person_Id;
                PersonManagment pm = new PersonManagment();
                EquipBLL.AdminManagment.PersonManagment.P_viewModal pv = pm.Get_PersonModal(UserId);
                if (pv.Role_Names.Contains("可靠性工程师"))
                {
                    string wfe_ser = "";
                    List<string> cjname = new List<string>();
                    List<Equip_Archi> EA = pm.Get_Person_Cj(UserId);
                    foreach (var ea in EA)
                    {
                        cjname.Add(ea.EA_Name);
                    }
                    List<A5dot1Tab1> E = tm.Getdcl_listbyisZG(0, cjname);

                    for (int i = 0; i < E.Count; i++)
                    {
                        if (E[i].dataSource != null)
                            wfe_ser = E[i].dataSource;
                        object o = new
                        {

                            WF_ICON = "fa fa-flash text-text-aqua",
                            MISS_Url = "/A5dot1/Index",
                            WF_Name = "设备完好",
                            MISS_Name = "可靠性工程师确认整改",
                            wfe_serial = wfe_ser,
                            sbCode = E[i].sbGyCode.ToString(),
                            time = E[i].zzSubmitTime.ToString(),
                            endtime = "",
                            lsh_xy = 1
                        };
                        miss_obj.Add(o);
                    }
                    SxglManagment Sx = new SxglManagment();
                    List<A5dot2Tab1> EE = Sx.GetSxItem(cjname);
                    foreach (var item in EE)
                    {
                        if (item.temp2 != null)
                            wfe_ser = item.temp2;
                        object o = new
                        {
                            WF_ICON = "fa fa-flash text-text-aqua",
                            MISS_Url = "/A5dot2/Index",
                            WF_Name = "竖向问题",
                            MISS_Name = "可靠性工程师确认整改",
                            wfe_serial = wfe_ser,
                            sbCode = item.sbGyCode.ToString(),
                            time = item.jxSubmitTime.ToString(),
                            endtime = "",
                            lsh_xy = 1
                        };
                        miss_obj.Add(o);
                    }
                }
               
                string str = JsonConvert.SerializeObject(miss_obj);
                return ("{" + "\"data\": " + str + "}");
            }
            catch (Exception e)
            {
                return null;
            }




        }


        //zxh
        //退出登录
        public string login_out()
        {
            Session.RemoveAll();

            Session.Clear();
          


            return("/Login/Index");



        }
        /// <summary>
        /// 获得工作流进度信息
        /// </summary>
        /// <returns>前台JS解析的JSon数据</returns>
        [HttpPost]
        public JsonResult ListProcess()
        {
            List<UI_WFEntity_Info> start_entities = CWFEngine.GetWFEntityByHistoryStatus(t => t.Record_Info.Count(q => q.Re_Name=="username" && q.Re_Value=="cb") > 0);

            List<object> proc_list = new List<object>();

            IObjectContextAdapter IOca = new EquipWebContext();

            foreach (var en in start_entities)
            {
                string startUser = CWFEngine.GetMissionRecordInfo(CWFEngine.GetHistoryMissions(en.EntityID).First().Miss_Id)["username"];
                if (startUser != (Session["User"] as Person_Info).Person_Name)
                    continue;

                int total = CWFEngine.GetWorkFlowAvgSteps(en.name);
                int curr = CWFEngine.GetWFEntityFinishSteps(en.EntityID);

                if (total == 0)
                    total = 5;
                if (curr == 0)
                    curr = 1;
                object o = new
                {
                    WF_Name = en.description,
                    MISS_Name = CWFEngine.GetActiveMission<Person_Info>(en.EntityID, IOca.ObjectContext).WE_Event_Desc,
                    MISS_Present = (double)curr/(double)total * 100
                };
                proc_list.Add(o);
            }
            return Json(proc_list.ToArray());
        }
    }
}
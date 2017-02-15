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
using FlowEngine.TimerManage;
using FlowEngine.Modals;

namespace WebApp.Controllers
{
    public class A5dot2Controller : CommonController
    {
        private SxglManagment Sx = new SxglManagment();
        public ActionResult Index1()
        {
            DateTime time = DateTime.Now;
            string morentime = time.Year.ToString() + "-" + time.Month.ToString();
            ViewBag.currenttime = morentime;
            return View();
        }  
        public ActionResult Index()
        {
            return View(getRecord());
        }
        //GET: /A5dot1/装置提报
        public ActionResult ShuxiangCheck(string wfe_id)
        {
            return View(getSubmitModel(wfe_id));
        }
        public ActionResult PqConfirm(string id)
        {
            return View(getRecord_detail(id));
        }
        public ActionResult SBShuxiang(DateTime time)
        {

            return View(getRecord_detailTab2(time));
        }
        public ActionResult JdcSummary(DateTime time)
        {

            return View(jdcSummy(time));
        }
        public class Index_ModelforA5dot2
        {
            public List<A5dot2Tab1> Am;
            public List<A5dot2Tab1> Hm;
            public string time;
            public int jwxry;
            public int kxxgcs;
            public int jdc;
        }
        public class Index_ModelforA5dot2Tab2
        {
            public List<A5dot2Tab2> Am;
            public List<A5dot2Tab2> Hm;
            public string title;
        }
        public Index_ModelforA5dot2 getRecord()
        {
            Index_ModelforA5dot2 RecordforA5dot2 = new Index_ModelforA5dot2();
            RecordforA5dot2.time = DateTime.Now.ToString();
            //ViewBag.curuser = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
            RecordforA5dot2.Am = new List<A5dot2Tab1>();
            int UserId = (Session["User"] as EquipModel.Entities.Person_Info).Person_Id;
            PersonManagment pm = new PersonManagment();
            EquipBLL.AdminManagment.PersonManagment.P_viewModal pv = pm.Get_PersonModal(UserId);
            if (pv.Role_Names.Contains("检维修人员") || pv.Role_Names.Contains("现场工程师"))
                RecordforA5dot2.jwxry = 1;
            else
                RecordforA5dot2.jwxry = 0;
            if (pv.Role_Names.Contains("机动处"))
                RecordforA5dot2.jdc = 1;
            else
                RecordforA5dot2.kxxgcs = 0;
            if (pv.Department_Name.Contains("机动处"))
                RecordforA5dot2.jdc = 1;
            else
                RecordforA5dot2.jdc = 0;
            if (pv.Role_Names.Contains("可靠性工程师"))
            {
                List<string> cjname = new List<string>();
                List<Equip_Archi> EA = pm.Get_Person_Cj(UserId);
                foreach (var ea in EA)
                {
                    cjname.Add(ea.EA_Name);
                }
                List<A5dot2Tab1> miss = Sx.GetSxItem(cjname);
                foreach (var item in miss)
                {
                    A5dot2Tab1 a = new A5dot2Tab1();
                    a.zzName = item.zzName;
                    a.sbGyCode = item.sbGyCode;
                    a.sbCode = item.sbCode;
                    a.jxUserName = item.jxUserName;
                    a.jxSubmitTime = item.jxSubmitTime;
                    a.temp1 = Convert.ToString(miss.IndexOf(item) + 1);
                    a.Id = item.Id;
                    a.temp2 = DateTime.Now.ToString();
                    RecordforA5dot2.Am.Add(a);
                }
                RecordforA5dot2.Hm = new List<A5dot2Tab1>();
                // List<A5dot2Tab1> His = Jx.GetHisJxItem();
                // foreach (var item in His)
                // {
                //     A5dot2Tab1 a = new A5dot2Tab1();
                //     a.Id = item.Id;
                //     a.state = item.state;
                //     
                //     a.temp1 = Convert.ToString(miss.IndexOf(item) + 1);
                //    RecordforA5dot2.Hm.Add(a);
                // }

                return RecordforA5dot2;
            }
            return RecordforA5dot2;
        }
        public Index_ModelforA5dot2 getRecord_detail(string id)
        {
            Index_ModelforA5dot2 RecordforA5dot2 = new Index_ModelforA5dot2();
            RecordforA5dot2.Am = new List<A5dot2Tab1>();
            RecordforA5dot2.Hm = new List<A5dot2Tab1>();
            ViewBag.curtime = DateTime.Now.ToString();
            ViewBag.curuser = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
            int IntId = Convert.ToInt32(id);
            List<A5dot2Tab1> miss = Sx.GetSxItem_detail(IntId);
            foreach (var item in miss)
            {
                A5dot2Tab1 a = new A5dot2Tab1();
                a.cjName = item.cjName;
                a.zzName = item.zzName;
                a.sbGyCode = item.sbGyCode;
                a.sbCode = item.sbCode;
                a.sbType = item.sbType;
                a.zyType = item.zyType;
                string[] str = (item.problemDescription).Split(new[] { "(未整改)" }, StringSplitOptions.None);
                a.problemDescription = str[0];
                a.temp1 = Convert.ToString(miss.IndexOf(item) + 1);              
                a.state = item.state;
                a.Id = item.Id;
                RecordforA5dot2.Am.Add(a);
            }

            return RecordforA5dot2;
        }
        public Index_ModelforA5dot2Tab2 getRecordforIndex()
        {
            Index_ModelforA5dot2Tab2 record = new Index_ModelforA5dot2Tab2();
            record.Am = new List<A5dot2Tab2>();
            ViewBag.curtime = DateTime.Now.ToString();
            ViewBag.curuser = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
            DateTime time = DateTime.Now;
            string yearandmonth = "";
            if (time.Day > 15)
            {
                time = time.AddMonths(+1);
                yearandmonth = time.Year + "年" + time.Month;
                //record.title = time.Year.ToString() + "-" + time.AddMonths(1).Month.ToString() + "月" + "最脏十台机泵";
            }
            else if (time.Day < 15)
            {
                yearandmonth = time.Year + "年" + time.Month;
                //record.title = time.Year.ToString() + "-" + time.Month.ToString() + "月" + "最脏十台机泵";
            }
            record.title = "2016-8月最脏十台机泵";
            List<A5dot2Tab2> miss = Sx.jdcsum("2016年8");
            foreach (var item in miss)
            {
                A5dot2Tab2 a = new A5dot2Tab2();
                //List<A5dot2Tab1> uncomplete = Sx.uncom(item.sbCode,time);
                a.cjName = item.cjName;
                a.zzName = item.zzName;
                a.sbGyCode = item.sbGyCode;
                a.sbCode = item.sbCode;
                a.sbType = item.sbType;
                a.nProblemsInCurMonth = item.nProblemsInCurMonth;
                a.nProblemsInCurYear = item.nProblemsInCurYear;//这台最脏设备累计未整改数     
                a.problemDescription = item.problemDescription;
                //string lastmonth = time.Year + "年" + (time.Month-1);
                //string lastmp = Sx.lastmonthproblem(lastmonth, item.sbCode);
                //if(lastmp!=null)
                //{

               
                //string[] distiproblem = lastmp.Split(new char[] { '$' });
                //for (int i = 0; i < distiproblem.Length;i++ )
                //{
                //    if(distiproblem[i].Contains("(未整改)"))
                //    {
                //        string[] str = distiproblem[i].Split(new[] { "(未整改)" }, StringSplitOptions.None);
                //        a.problemDescription= a.problemDescription+"$"+str[0] + "(上月未整改)";
                //    }
                //}
                //}
                   
                a.Id = item.Id;
                record.Am.Add(a);
            }
            return record;
        }
        //A5最脏十台
        public string A5zuizang()
        {
            List<object> r = new List<object>();
            
            ViewBag.curtime = DateTime.Now.ToString();
            ViewBag.curuser = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
            
            DateTime time = DateTime.Now;
            string yearandmonth = "";
            if(time.Day>15)
            {
                time = time.AddMonths(+1);
                yearandmonth = time.Year + "年" + time.Month;
            }
            else if(time.Day<15)
            {
                yearandmonth = time.Year + "年" + time.Month;
            }
            
            

            List<A5dot2Tab2> miss = Sx.jdcsum(yearandmonth);
            for (int i = 0; i < miss.Count;i++ )
            {
                List<A5dot2Tab1> uncomplete = Sx.uncom(miss[i].sbCode, time);
                object o = new
                {
                    index = i + 1,
                    cjname = miss[i].cjName,
                    zzname = miss[i].zzName,
                    sbGyCode = miss[i].sbGyCode,
                    sbCode = miss[i].sbCode,
                    sbType = miss[i].sbType,
                    nProblemsInCurMonth = miss[i].nProblemsInCurMonth,
                    AllProblem = (uncomplete.Count).ToString()//这台最脏设备累计未整改数 
                };
                r.Add(o);
            }         
                //foreach (var item in miss)
                //{
                //    A5dot2Tab2 a = new A5dot2Tab2();
                //    List<A5dot2Tab1> uncomplete = Sx.uncom(item.sbCode, time);
                //    a.cjName = item.cjName;
                //    a.zzName = item.zzName;
                //    a.sbGyCode = item.sbGyCode;
                //    a.sbCode = item.sbCode;
                //    a.sbType = item.sbType;
                //    a.nProblemsInCurMonth = item.nProblemsInCurMonth;
                //    a.temp3 = (uncomplete.Count).ToString();//这台最脏设备累计未整改数     
                //    a.problemDescription = item.problemDescription;
                //    string lastmonth = time.Year + "年" + (time.Month - 1);
                //    string lastmp = Sx.lastmonthproblem(lastmonth, item.sbCode);
                //    string[] distiproblem = lastmp.Split(new char[] { '$' });
                //    for (int i = 0; i < distiproblem.Length; i++)
                //    {
                //        if (distiproblem[i].Contains("(未整改)"))
                //        {
                //            string[] str = distiproblem[i].Split(new[] { "(未整改)" }, StringSplitOptions.None);
                //            a.problemDescription = a.problemDescription + "$" + str[0] + "(上月未整改)";
                //        }
                //    }


                //    a.Id = item.Id;
                //    record.Am.Add(a);
                //}
            //return record;
            string str = JsonConvert.SerializeObject(r);
            return ("{" + "\"data\": " + str + "}");
        }
        public string zuizhang10bytime(string time)
        {
            DateTime morentime = DateTime.Now;
            if(time==null)
            {
                if (morentime.Day>=15)
                {
                    time = morentime.Year.ToString() + "-" + morentime.AddMonths(1).Month.ToString();
                }
                else
                {
                    time = morentime.Year.ToString() + "-" + morentime.Month.ToString();
                }
            }
            List<object> r = new List<object>();
            List<A5dot2Tab2> zuizang10 = new List<A5dot2Tab2>();
            string[] s = time.Split(new char[] { '-' });
            string ym = s[0] + "年" + s[1];
            DateTime time1 = DateTime.Now;
            zuizang10 = Sx.jdcsum(ym);
            for (int i = 0; i < zuizang10.Count; i++)
            {
                //List<A5dot2Tab1> uncomplete = Sx.uncom(zuizang10[i].sbCode, time1);
                string problemDescription = zuizang10[i].problemDescription;
                //string lastmonth = time1.Year + "年" + (time1.Month - 1);
                //string lastmp = Sx.lastmonthproblem(lastmonth, zuizang10[i].sbCode);
                //if (lastmp != null)
                //{


                //    string[] distiproblem = lastmp.Split(new char[] { '$' });
                //    for (int k = 0; k < distiproblem.Length; i++)
                //    {
                //        if (distiproblem[k].Contains("(未整改)"))
                //        {
                //            string[] str1 = distiproblem[k].Split(new[] { "(未整改)" }, StringSplitOptions.None);
                //            problemDescription = problemDescription + "$" + str1[0] + "(上月未整改)";
                //        }
                //    }
                //}
                object o = new
                {
                    //index = i + 1,
                    cjname = zuizang10[i].cjName,
                    zzname = zuizang10[i].zzName,
                    sbGyCode = zuizang10[i].sbGyCode,
                    sbCode = zuizang10[i].sbCode,
                    sbType = zuizang10[i].sbType,
                    nProblemsInCurMonth = zuizang10[i].nProblemsInCurMonth,
                    AllProblem = zuizang10[i].nProblemsInCurYear,
                    //AllProblem = (uncomplete.Count).ToString(),//这台最脏设备累计未整改数 
                    problemDescription = problemDescription
                };
                r.Add(o);
            }
            string str = JsonConvert.SerializeObject(r);
            return ("{" + "\"data\": " + str + "}");

        }
        public JsonResult ShuxiangCheck_submitsignal(string json1)
        {

            GetNWorkSerManagment gnm = new GetNWorkSerManagment();
            JObject item = (JObject)JsonConvert.DeserializeObject(json1);
            A5dot2Tab1 new_5dot2 = new A5dot2Tab1();
            new_5dot2.cjName = item["cjName"].ToString();
            new_5dot2.zzName = item["zzName"].ToString();
            new_5dot2.sbGyCode = item["sbGyCode"].ToString();
            new_5dot2.sbCode = item["sbCode"].ToString();
            new_5dot2.sbType = item["sbType"].ToString();
            new_5dot2.zyType = item["zyType"].ToString();
            new_5dot2.problemDescription = item["problemDescription"].ToString();
            new_5dot2.jxSubmitTime = DateTime.Now;
            new_5dot2.jxUserName = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
            new_5dot2.isRectified = 0;
            new_5dot2.state = 0;
            new_5dot2.temp2 = gnm.AddNWorkEntity("A5dot2").WE_Ser; ;
            bool res = Sx.AddSxItem(new_5dot2);
            
          
            return Json(new { result = res });
        }
        public JsonResult Pq_submitsignal(string json1)
        {

            JObject item = (JObject)JsonConvert.DeserializeObject(json1);

            A5dot2Tab1 new_5dot21 = new A5dot2Tab1();
            new_5dot21.pqCheckTime = DateTime.Now;
            new_5dot21.pqUserName = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;

            new_5dot21.isRectified = Convert.ToInt32(item["isRectified"]);
            new_5dot21.state = 1;
            if (Convert.ToInt32(item["isRectified"])==1)
            {
                //string[] str = (item["problemDescription"].ToString()).Split(new[] { "(未整改)" }, StringSplitOptions.None);//以字符串作为分割条件
                new_5dot21.problemDescription = item["problemDescription"].ToString() + "(已整改)";
            }
            if (Convert.ToInt32(item["isRectified"]) == 0)
            {
                new_5dot21.problemDescription = item["problemDescription"].ToString() + "(未整改)";
            }
            new_5dot21.Id = Convert.ToInt32(item["A5dot2Tab1_id"].ToString());
            string res = Sx.ModifySxItem(new_5dot21);



            int a_id = Convert.ToInt32(item["A5dot2Tab1_id"].ToString());
            if (Sx.getAllbyid(a_id).temp3 != null)
            {
                int missId = Convert.ToInt32(Sx.getAllbyid(a_id).temp3);
                var t = CTimerManage.LoadTimerMission(missId);

                if (t != null)
                {
                    t.status = TM_STATUS.TM_FINISH;
                    t.Save();
                    CTimerManage.RemoveFromActiveList(t.ID);
                }
            }
            

            //接收返回的设备工艺号

            A5dot2Tab2 new_5dot22 = new A5dot2Tab2();
            List<A5dot2Tab2> IsExist = Sx.GetA5dot2Tab2Item(res);
            if(IsExist.Count==0)
            { 
            //传到表2若没有则添加一条数据，如果有则修改该条数据
                DateTime i = DateTime.Now;
                string yearmonth = "";
                if (i.Day >= 15)
                {
                    yearmonth = i.Year.ToString() + "年" + i.AddMonths(1).Month.ToString();
                }
                else
                {
                    yearmonth = i.Year.ToString() + "年"+ i.Month.ToString();
                }
                new_5dot22.cjName = item["cjName"].ToString();
                new_5dot22.zzName = item["zzName"].ToString();
                new_5dot22.sbGyCode = item["sbGyCode"].ToString();
                new_5dot22.sbCode = item["sbCode"].ToString();
                new_5dot22.sbType = item["sbType"].ToString();
                new_5dot22.zyType = item["zyType"].ToString();
                if (Convert.ToInt32(item["isRectified"]) == 1)
                {
                    new_5dot22.problemDescription = item["problemDescription"].ToString() + "(已整改)";
                }
                if (Convert.ToInt32(item["isRectified"]) == 0)
                {
                    new_5dot22.problemDescription = item["problemDescription"].ToString() + "(未整改)";
                }
              
                new_5dot22.state = 1;
                new_5dot22.nProblemsInCurMonth = 1;
                new_5dot22.nProblemsInCurYear = 1;
                new_5dot22.isSetAsTop10Worst = 0;
                new_5dot22.yearMonthForStatistic = yearmonth;
            bool has=Sx.AddA5dot2Tab2Item(new_5dot22);
            }
                //修改该条数据
            else
            {
                DateTime i = DateTime.Now;
                string yearmonth = "";
                if (i.Day >= 15)
                {
                    yearmonth = i.Year.ToString() + "年" + i.AddMonths(1).Month.ToString();
                }
                else
                {
                    yearmonth = i.Year.ToString() + "年" + i.Month.ToString();
                }
                new_5dot22.cjName = item["cjName"].ToString();
                new_5dot22.zzName = item["zzName"].ToString();
                new_5dot22.sbGyCode = item["sbGyCode"].ToString();
                new_5dot22.sbCode = item["sbCode"].ToString();
                new_5dot22.sbType = item["sbType"].ToString();
                if (Convert.ToInt32(item["isRectified"]) == 1)
                {
                    new_5dot22.problemDescription = item["problemDescription"].ToString() + "(已整改)";
                }
                if (Convert.ToInt32(item["isRectified"]) == 0)
                {
                    new_5dot22.problemDescription = item["problemDescription"].ToString() + "(未整改)";
                }
              
                new_5dot22.zyType = item["zyType"].ToString();
                new_5dot22.sbCode = item["sbCode"].ToString();
                new_5dot22.state = 1;
                new_5dot22.nProblemsInCurMonth = 1;
                new_5dot22.nProblemsInCurYear = 1;
                new_5dot22.isSetAsTop10Worst = 0;
                new_5dot22.yearMonthForStatistic = yearmonth;
                bool has = Sx.ModifyA5dot2Tab2Item(new_5dot22);
            }

            


            return Json(new { result = res });
        }

        public Index_ModelforA5dot2Tab2 getRecord_detailTab2(DateTime time)
        {
            Index_ModelforA5dot2Tab2 RecordforA5dot2Tab2 = new Index_ModelforA5dot2Tab2();
            RecordforA5dot2Tab2.Am = new List<A5dot2Tab2>();
            RecordforA5dot2Tab2.Hm = new List<A5dot2Tab2>();
            ViewBag.curtime = DateTime.Now.ToString();
            ViewBag.curuser = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
            string yearandmonth = time.Year + "年" + time.Month;
            
            List<A5dot2Tab2> miss = Sx.GetA5dot2Tab2ItemForPq(yearandmonth);
           foreach(var item in miss)
            {
                A5dot2Tab2 a = new A5dot2Tab2();
                a.cjName = item.cjName;
                a.zzName = item.zzName;
                a.sbGyCode = item.sbGyCode;
                a.sbCode = item.sbCode;
                a.nProblemsInCurMonth = item.nProblemsInCurMonth;
                a.nProblemsInCurYear = Sx.GetnProblemsInYearItem(item.sbCode);
                a.Id = item.Id;
                RecordforA5dot2Tab2.Am.Add(a);
            }
            //List<A5dot2Tab1> miss = Sx.GetSxItem_detail(IntId);
            //foreach (var item in miss)
            //{
            //    A5dot2Tab1 a = new A5dot2Tab1();
            //    a.cjName = item.cjName;
            //    a.zzName = item.zzName;
            //    a.sbGyCode = item.sbGyCode;
            //    a.sbCode = item.sbCode;
            //    a.sbType = item.sbType;
            //    a.zyType = item.zyType;
            //    a.problemDescription = item.problemDescription;
            //    a.temp1 = Convert.ToString(miss.IndexOf(item) + 1);
            //    a.state = item.state;
            //    a.Id = item.Id;
            //    RecordforA5dot2.Am.Add(a);
            //}

            return RecordforA5dot2Tab2;
        }
        public JsonResult SBShuxiang_submitsignal(string json1)
        {

            JObject item = (JObject)JsonConvert.DeserializeObject(json1);
            bool res = false;
            string jdcuser = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
            string IDArray = item["IDArray"].ToString();
            string[] ID = IDArray.Split(new char[] { ',' });
            for (int i = 0; i < ID.Length-1;i++ )
            {
                res = Sx.IsSetWorse(ID[i], jdcuser);
            }


            return Json(new { result = res });
        }
        public Index_ModelforA5dot2Tab2 jdcSummy(DateTime time)
        {
            Index_ModelforA5dot2Tab2 jdcSummy = new Index_ModelforA5dot2Tab2();
            jdcSummy.Am = new List<A5dot2Tab2>();
            jdcSummy.Hm = new List<A5dot2Tab2>();
            ViewBag.curtime = DateTime.Now.ToString();
            ViewBag.curuser = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
            string yearandmonth = time.Year + "年" + time.Month;
           
            List<A5dot2Tab2> miss = Sx.jdcsum(yearandmonth);
            foreach (var item in miss)
            {
                A5dot2Tab2 a = new A5dot2Tab2();
                a.cjName = item.cjName;
                a.zzName = item.zzName;
                a.sbGyCode = item.sbGyCode;
                a.sbCode = item.sbCode;
                a.problemDescription = item.problemDescription;
                a.Id = item.Id;
                jdcSummy.Am.Add(a);
            }
            return jdcSummy;
        }
    }
}
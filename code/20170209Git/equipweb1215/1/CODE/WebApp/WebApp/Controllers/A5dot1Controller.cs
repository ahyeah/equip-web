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
using FlowEngine.TimerManage;
using FlowEngine.Modals;

namespace WebApp.Controllers
{
    public class A5dot1Controller : CommonController
    {
        //
        // GET: /A5dot1/
        public ActionResult A5()
        {
            return View();
        }
        public class NameandNum
        {
            public int Equip_Num;
            public string name;
        }
        public class PqcheckModel
        {
            public A5dot1Tab1 a5dot1Tab1;
            public string UserName;
            public DateTime currenttime;

        }
        public class indexmodel
        {
            public int xcgcs;
            public int kxxgcs;
            public int jxdw;
            public int jdc;
        }
        public class index0model
        {
            public string title;
            public List<A5dot1Tab2> worst10;
            public List<double> cj_bwh;
            public List<double> pq_bwh;
            public double qc_bwh;
        }
        //A5最差十台表
        public string A5zuicha()
        {
            List<object> r = new List<object>();
            index0model i0m = new index0model();
            DateTime my = DateTime.Now;
            string ym;
            if (my.Day >= 15)
            {
                ym = my.Year.ToString() + my.AddMonths(1).Month.ToString();
            }
            else
            {
                ym = my.Year.ToString() + my.Month.ToString();
            }
            TablesManagment tm = new TablesManagment();
            i0m.worst10 = tm.get_worst10byym(ym);
            for (int i = 0; i < i0m.worst10.Count; i++)
            {
                object o = new
                {
                    index = i + 1,
                    cjname = i0m.worst10[i].cjName,
                    zzname = i0m.worst10[i].zzName,
                    sbGyCode = i0m.worst10[i].sbGyCode,
                    sbCode = i0m.worst10[i].sbCode,
                    timesNotGood = i0m.worst10[i].timesNotGood,
                    countAllNoRectifed = i0m.worst10[i].timesNotGood,//这台最差设备当月累计未整改数 
                    notGoodContents = i0m.worst10[i].notGoodContents
                };
                r.Add(o);
            }
            string str = JsonConvert.SerializeObject(r);
            return ("{" + "\"data\": " + str + "}");
        }
        public index0model getindex0()
        {
            index0model i0m = new index0model();
            DateTime my = DateTime.Now;
            string title = "";
            string ym;
            if (my.Day >= 15)
            {
                title = my.Year.ToString() + "-" + my.AddMonths(1).Month.ToString() + "月" + "最差十台机泵";
                ym = my.Year.ToString() + my.AddMonths(1).Month.ToString();
            }
            else
            {
                title = my.Year.ToString() + "-" + my.Month.ToString() + "月" + "最差十台机泵";
                ym = my.Year.ToString() + my.Month.ToString();
            }
            i0m.title = title;
            TablesManagment tm = new TablesManagment();
            i0m.worst10 = tm.get_worst10byym(ym);



            EquipManagment Em = new EquipManagment();
            List<EANummodel> E = Em.getequipnum_byarchi();
            List<Equip_Archi> AllCj_List = Em.GetAllCj();

            List<NameandNum> cj = new List<NameandNum>();
            List<NameandNum> pq = new List<NameandNum>();




            for (int i = 0; i < AllCj_List.Count; i++)
            {
                int count = 0;
                NameandNum temp1 = new NameandNum();
                temp1.name = AllCj_List[i].EA_Name;
                for (int j = 0; j < E.Count; j++)
                {
                    if (AllCj_List[i].EA_Id == Em.getEA_parentid(E[j].EA_Id))
                        count += E[j].Equip_Num;
                }
                temp1.Equip_Num = count;
                cj.Add(temp1);
                count = 0;
            }

            List<string> pq_name_list = new List<string>();
            pq_name_list.Add("联合一片区");
            pq_name_list.Add("联合二片区");
            pq_name_list.Add("联合三片区");
            pq_name_list.Add("联合四片区");
            pq_name_list.Add("化工片区");
            pq_name_list.Add("综合片区");
            pq_name_list.Add("其他");
            for (int i = 0; i < pq_name_list.Count; i++)
            {
                int count = 0;
                NameandNum temp1 = new NameandNum();
                temp1.name = pq_name_list[i];
                List<Pq_Zz_map> Pq_Zz_map = Em.GetZzsofPq(pq_name_list[i]);
                for (int j = 0; j < Pq_Zz_map.Count;j++ )
                {
                    for (int z = 0; z < E.Count; z++)
                    {
                        if (Pq_Zz_map[j].Zz_Name == Em.getEa_namebyid(E[z].EA_Id))
                            count += E[z].Equip_Num;
                    }
                }
                temp1.Equip_Num = count;
                pq.Add(temp1);
                count = 0;

            }
            //全场不完好
            int qc_count = 0;
            for (int i = 0; i < pq.Count; i++)
            {
                qc_count += pq[i].Equip_Num;
            }
            List<A5dot1Tab1> qc_list = tm.get_All();
            double qcbwh = 0.000;
            double qc_bxhcount = 0;
            int wzgcount = 0;
            if (qc_list.Count > 0)
            {
                qc_bxhcount = 0;
                wzgcount = 0;
                string sbcode_temp = qc_list[0].sbCode;
                for (int j = 0; j < qc_list.Count; j++)
                {
                    qc_list = tm.get_All();
                    if (qc_list[j].temp1 == null)
                    {
                        List<A5dot1Tab1> cj_bycode = tm.GetAll1_bycode(qc_list[j].sbCode);
                        for (int k = 0; k < cj_bycode.Count; k++)
                        {
                            if (cj_bycode[k].isRectified == 0)
                            {
                                wzgcount++;
                            }
                            tm.modifytemp1_byid(cj_bycode[k].Id, "已合并");

                        }
                        if (wzgcount > 0)
                        {
                            qc_bxhcount++;
                        }
                        wzgcount = 0;
                    }

                    // cjbwh.Add(f);
                }

            }
            for (int n = 0; n < qc_list.Count; n++)
            {
                tm.modifytemp1_byid(qc_list[n].Id, null);
            }
            qcbwh = Math.Round(((double)qc_bxhcount / qc_count), 6);

            i0m.qc_bwh = qcbwh;




            List<double> cjbwh = new List<double>();
            List<double> pqbwh = new List<double>();
            //车间不完好 

            for (int i = 0; i < cj.Count; i++)
            {
                List<A5dot1Tab1> cj_list = tm.get_cj_bwh(cj[i].name, cj[i].Equip_Num);
                double cj_bxhcount = 0;
                int wzg_count = 0;
                if (cj_list.Count > 0)
                {
                    cj_bxhcount = 0;
                    wzg_count = 0;
                    string sbcode_temp = cj_list[0].sbCode;
                    for (int j = 0; j < cj_list.Count; j++)
                    {
                        cj_list = tm.get_cj_bwh(cj[i].name, cj[i].Equip_Num);
                        if (cj_list[j].temp1 == null)
                        {
                            List<A5dot1Tab1> cj_bycode = tm.GetAll1_bycode(cj_list[j].sbCode);
                            for (int k = 0; k < cj_bycode.Count; k++)
                            {
                                if (cj_bycode[k].isRectified == 0)
                                {
                                    wzg_count++;
                                }
                                tm.modifytemp1_byid(cj_bycode[k].Id, "已合并");

                            }
                            if (wzg_count > 0)
                            {
                                cj_bxhcount++;
                            }
                            wzg_count = 0;
                        }

                        // cjbwh.Add(f);
                    }

                }
                for (int n = 0; n < cj_list.Count; n++)
                {
                    tm.modifytemp1_byid(cj_list[n].Id, null);
                }
                if (cj[i].Equip_Num != 0)
                    cjbwh.Add(Math.Round(((double)cj_bxhcount / cj[i].Equip_Num), 6));
                else
                    cjbwh.Add(0.0);
            }

            i0m.cj_bwh = cjbwh;

            //片区不完好
            for (int i = 0; i < pq.Count; i++)
            {
                List<A5dot1Tab1> pq_list = tm.get_pq_bwh(pq[i].name, pq[i].Equip_Num);
                double pq_bxhcount = 0;
                int wzg_count = 0;
                if (pq_list.Count > 0)
                {
                    pq_bxhcount = 0;
                    wzg_count = 0;
                    string sbcode_temp = pq_list[0].sbCode;
                    for (int j = 0; j < pq_list.Count; j++)
                    {
                        pq_list = tm.get_cj_bwh(cj[i].name, cj[i].Equip_Num);
                        if (pq_list[j].temp1 == null)
                        {
                            List<A5dot1Tab1> cj_bycode = tm.GetAll1_bycode(pq_list[j].sbCode);
                            for (int k = 0; k < cj_bycode.Count; k++)
                            {
                                if (cj_bycode[k].isRectified == 0)
                                {
                                    wzg_count++;
                                }
                                tm.modifytemp1_byid(cj_bycode[k].Id, "已合并");

                            }
                            if (wzg_count > 0)
                            {
                                pq_bxhcount++;
                            }
                            wzg_count = 0;
                        }

                        // cjbwh.Add(f);
                    }
                }

                for (int n = 0; n < pq_list.Count; n++)
                {
                    tm.modifytemp1_byid(pq_list[n].Id, null);
                }
                pqbwh.Add(Math.Round(((double)pq_bxhcount / pq[i].Equip_Num), 6));
            }
            i0m.pq_bwh = pqbwh;

            return i0m;
        }
        public PqcheckModel getPqcheckModel(int a_id)
        {
            PqcheckModel mm = new PqcheckModel();
            mm.UserName = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
            TablesManagment tm = new TablesManagment();
            mm.a5dot1Tab1 = tm.GetA_byid(a_id);
            mm.currenttime = DateTime.Now;
            return mm;
        }
        public ActionResult Index()
        {
            int UserId = (Session["User"] as EquipModel.Entities.Person_Info).Person_Id;
            PersonManagment pm = new PersonManagment();
            EquipBLL.AdminManagment.PersonManagment.P_viewModal pv = pm.Get_PersonModal(UserId);
            indexmodel im = new indexmodel();
            if (pv.Role_Names.Contains("现场工程师"))
                im.xcgcs = 1;
            else
                im.xcgcs = 0;
            if (pv.Role_Names.Contains("可靠性工程师"))
                im.kxxgcs = 1;
            else
                im.kxxgcs = 0;
            if (pv.Role_Names.Contains("检维修人员"))
                im.jxdw = 1;
            else
                im.jxdw = 0;
            if (pv.Department_Name.Contains("机动处") && pv.Role_Names.Contains("专业团队"))
                im.jdc = 1;
            else
                im.jdc = 0;
            return View(im);
        }
        public ActionResult Index0()
        {

            return View(getindex0());
        }
        //GET: /A5dot1/装置提报
        public ActionResult ZzSubmit(string wfe_id)
        {
            return View(getSubmitModel(wfe_id));
        }
        public ActionResult Pqcheck(int a_id)
        {
            return View(getPqcheckModel(a_id));
        }
        public ActionResult HistoryCheck(int a_id)
        {
            return View(getPqcheckModel(a_id));
        }

        // GET: /A5dot1/可靠性工程师汇总
        public ActionResult PqSummary()
        {
            return View();
        }

        // GET: /A5dot1/机动处汇总
        public ActionResult JdcSummary(string flowname)
        {
            return View(getA5_Model(flowname));
        }

        public string ZzSubmit_submitsignal(string json1)
        {
            try
            {

                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string[] notgood = item["Incomplete_content"].ToString().Split('|');
                A5dot1Tab1 a5dot1Tab1 = new A5dot1Tab1();
                DateTime my = DateTime.Now;
                string yearmonth = "";
                EquipManagment Em = new EquipManagment();
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
                        a5dot1Tab1.cjName = item["Cj_Name"].ToString();
                        a5dot1Tab1.zzName = item["Zz_Name"].ToString();
                        a5dot1Tab1.sbGyCode = item["Equip_GyCode"].ToString();
                        a5dot1Tab1.sbCode = item["Equip_Code"].ToString();
                        a5dot1Tab1.sbType = item["Equip_Type"].ToString();
                        a5dot1Tab1.zyType = item["Zy_Type"].ToString();
                        a5dot1Tab1.notGoodContent = notgood[i];
                        a5dot1Tab1.isRectified = 0;
                        a5dot1Tab1.zzSubmitTime = DateTime.Now;
                        a5dot1Tab1.zzUserName = item["ZzsubmitName"].ToString();
                        a5dot1Tab1.yearMonthForStatistic = yearmonth;
                        a5dot1Tab1.pqName = Em.GetPqofZz(item["Zz_Name"].ToString()).Pq_Name;
                        TablesManagment tm = new TablesManagment();
                        tm.Zzsubmit(a5dot1Tab1);
                    }
                }
                else
                {
                    for (int i = 0; i < notgood.Count(); i++)
                    {
                        a5dot1Tab1.cjName = item["Cj_Name"].ToString();
                        a5dot1Tab1.zzName = item["Zz_Name"].ToString();
                        a5dot1Tab1.sbGyCode = item["Equip_GyCode"].ToString();
                        a5dot1Tab1.sbCode = item["Equip_Code"].ToString();
                        a5dot1Tab1.sbType = item["Equip_Type"].ToString();
                        a5dot1Tab1.zyType = item["Zy_Type"].ToString();
                        a5dot1Tab1.notGoodContent = notgood[i];
                        a5dot1Tab1.isRectified = 0;
                        a5dot1Tab1.zzSubmitTime = DateTime.Now;
                        a5dot1Tab1.zzUserName = item["ZzsubmitName"].ToString();
                        a5dot1Tab1.yearMonthForStatistic = yearmonth;
                        a5dot1Tab1.pqName = Em.GetPqofZz(item["Zz_Name"].ToString()).Pq_Name;
                        TablesManagment tm = new TablesManagment();
                        tm.Zzsubmit(a5dot1Tab1);
                    }
                }
                //SubmitDSEventDetails("A5.1","设备完好提报");
            }
            catch (Exception e)
            {
                return "";
            }
            return ("/A5dot1/Index");
        }

        public string JxSubmit(string json1)
        {
            try
            {

                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                A5dot1Tab2 a5dot1Tab2 = new A5dot1Tab2();
                DateTime my = DateTime.Now;
                string yearmonth = "";
                if (my.Day >= 15)
                {
                    yearmonth = my.Year.ToString() + my.AddMonths(1).Month.ToString();
                }
                else
                {
                    yearmonth = my.Year.ToString() + my.Month.ToString();
                }
                int userid = (Session["User"] as EquipModel.Entities.Person_Info).Person_Id;
                PersonManagment pm = new PersonManagment();
                string pqname = "";
                int pqid = pm.Get_Person_Depart(userid).Depart_Id;
                if (2 <= pqid && pqid <= 12)
                {
                    pqname = pm.Get_Person_Depart(userid).Depart_Name;
                }
                else
                {
                    pqname = pm.Get_Person_DepartOfParent(userid).Depart_Name;
                }
                a5dot1Tab2.cjName = item["Cj_Name"].ToString();
                a5dot1Tab2.zzName = item["Zz_Name"].ToString();
                a5dot1Tab2.sbGyCode = item["Equip_GyCode"].ToString();
                a5dot1Tab2.sbCode = item["Equip_Code"].ToString();
                a5dot1Tab2.sbType = item["Equip_Type"].ToString();
                a5dot1Tab2.zyType = item["Zy_Type"].ToString();
                a5dot1Tab2.notGoodContents = item["Incomplete_contents"].ToString();
                a5dot1Tab2.problemAnalysisAdvise = item["wtfx"].ToString();
                a5dot1Tab2.processMeansMethods = item["chlsd"].ToString();
                a5dot1Tab2.processReuslt = item["cljg"].ToString();
                a5dot1Tab2.yearMonthForStatistic = yearmonth;
                a5dot1Tab2.countAllNoRectifed = 1;
                a5dot1Tab2.timesNotGood = 1;
                a5dot1Tab2.state = 1;
                a5dot1Tab2.jxdwUserName = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
                a5dot1Tab2.isSetAsTop5Worst = 1;
                a5dot1Tab2.pqName = pqname;
                //a5dot1Tab2.D = item["Data_Src"].ToString();
                a5dot1Tab2.yearMonthForStatistic = yearmonth;
                TablesManagment tm = new TablesManagment();
                tm.savefivesb(a5dot1Tab2);
            }
            catch (Exception e)
            {
                return "";
            }
            return ("/A5dot1/JxEdit");
        }
        public string JxEditSubmit()
        {
            string userName = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
            DateTime my = DateTime.Now;
            string yearmonth = "";
            if (my.Day >= 15)
            {
                yearmonth = my.Year.ToString() + my.AddMonths(1).Month.ToString();
            }
            else
            {
                yearmonth = my.Year.ToString() + my.Month.ToString();
            }
            TablesManagment tm = new TablesManagment();
            List<A5dot1Tab2> E = tm.GetAll2_byymandstate(yearmonth, 1);
            for (int i = 0; i < E.Count; i++)
            {
                tm.setstate_byid(E[i].Id, 2, userName);
            }
            return ("/A5dot1/Index");
        }

        public string dcl_list()
        {
            TablesManagment tm = new TablesManagment();
            int UserId = (Session["User"] as EquipModel.Entities.Person_Info).Person_Id;
            PersonManagment pm = new PersonManagment();
            EquipBLL.AdminManagment.PersonManagment.P_viewModal pv = pm.Get_PersonModal(UserId);
            List<object> r = new List<object>();
            if (pv.Role_Names.Contains("可靠性工程师"))
            {
                List<string> cjname = new List<string>();
                List<Equip_Archi> EA = pm.Get_Person_Cj(UserId);
                foreach (var ea in EA)
                {
                    cjname.Add(ea.EA_Name);
                }
                List<A5dot1Tab1> E = tm.Getdcl_listbyisZG(0, cjname);

                for (int i = 0; i < E.Count; i++)
                {
                    object o = new
                    {
                        index = i + 1,
                        zzname = E[i].zzName.ToString(),
                        sbgybh = E[i].sbGyCode.ToString(),
                        sbcode = E[i].sbCode.ToString(),
                        notgood = E[i].notGoodContent.ToString(),
                        a_id = E[i].Id
                    };
                    r.Add(o);
                }
            }
            string str = JsonConvert.SerializeObject(r);
            return ("{" + "\"data\": " + str + "}");


        }
        public string ycl_list()
        {
            TablesManagment tm = new TablesManagment();
            int UserId = (Session["User"] as EquipModel.Entities.Person_Info).Person_Id;
            PersonManagment pm = new PersonManagment();
            EquipBLL.AdminManagment.PersonManagment.P_viewModal pv = pm.Get_PersonModal(UserId);
            List<object> r = new List<object>();
            if (pv.Role_Names.Contains("可靠性工程师"))
            {
                List<string> cjname = new List<string>();
                List<Equip_Archi> EA = pm.Get_Person_Cj(UserId);
                foreach (var ea in EA)
                {
                    cjname.Add(ea.EA_Name);
                }
                List<A5dot1Tab1> E = tm.Getdcl_listbyisZG(1, cjname);
                //List<object> r = new List<object>();
                for (int i = 0; i < E.Count; i++)
                {
                    object o = new
                    {
                        index = i + 1,
                        zzname = E[i].zzName.ToString(),
                        sbgybh = E[i].sbGyCode.ToString(),
                        sbcode = E[i].sbCode.ToString(),
                        notgood = E[i].notGoodContent.ToString(),
                        a_id = E[i].Id
                    };
                    r.Add(o);
                }
            }
            string str = JsonConvert.SerializeObject(r);
            return ("{" + "\"data\": " + str + "}");


        }
        public string list()
        {
            DateTime my = DateTime.Now;
            DateTime lastmonth = DateTime.Now.AddMonths(-1);
            string yearmonth = "";
            string last = "";
            if (my.Day >= 15)
            {
                yearmonth = my.Year.ToString() + my.AddMonths(1).Month.ToString();
                last = lastmonth.Year.ToString() + lastmonth.AddMonths(1).Month.ToString();
            }
            else
            {
                yearmonth = my.Year.ToString() + my.Month.ToString();
                last = lastmonth.Year.ToString() + lastmonth.Month.ToString();
            }
            TablesManagment tm = new TablesManagment();
            List<string> cjname = new List<string>();
            PersonManagment pm = new PersonManagment();

            int UserId = (Session["User"] as EquipModel.Entities.Person_Info).Person_Id;
            //Depart_Archi da= pm.Get_Person_Depart(UserId);
            //string pq = da.Depart_Name;
            List<Equip_Archi> EA = pm.Get_Person_Cj(UserId);
            foreach (var ea in EA)
            {
                cjname.Add(ea.EA_Name);
            }
            List<A5dot1Tab1> e = tm.GetAll1_byym(yearmonth, cjname);
            //List<A5dot1Tab1> laste = tm.GetAll1_byym(last, cjname);
            List<object> r = new List<object>();
            string temp = e[0].sbCode;
            string s = "";
            string lasts = "";
            string isZG = "";
            string problemDescription = "";
            int bwhcs = 0;
            int bwhxs = 0;
            for (int j = 0; j < e.Count; j++)
            {
                e = tm.GetAll1_byym(yearmonth, cjname);
                bwhcs = 0;
                bwhxs = 0;
                if (e[j].temp1 == null)
                {
                    List<A5dot1Tab1> E = tm.GetAll1_byymandcode(yearmonth, e[j].sbCode);
                    for (int i = 0; i < E.Count; i++)
                    {
                        if (E[i].isRectified == 0)
                        {
                            isZG = "未整改";
                            bwhxs++;
                        }
                        else
                            isZG = "已整改";
                        s += E[i].notGoodContent + " (" + isZG + ") ";
                        problemDescription += E[i].notGoodContent;
                        //if (i > 0&&E[i].zzSubmitTime.Value.Second - E[i - 1].zzSubmitTime.Value.Second >= 2)
                        bwhcs++;
                        tm.modifytemp1_byid(E[i].Id, "已合并");
                    }
                    List<A5dot1Tab1> lastE = tm.GetAll1_byymandcode(last, e[j].sbCode);
                    for (int k = 0; k < lastE.Count; k++)
                    {
                        if (lastE[k].isRectified == 0)
                        {

                            lasts += lastE[k].notGoodContent;
                        }

                    }
                    if (lasts != "")
                        s = s + " 上月未整改内容：" + lasts;
                    else
                        s = s + " 上月无未整改内容。";
                    if (e[j].temp2 == null)
                        e[j].temp2 = "";

                    //string pq = "";
                    //if (e[j].zzName.ToString() == "消防队" || e[j].zzName.ToString() == "计量站")
                    //    pq = e[j].zzName.ToString();
                    //else
                    //    pq = pm.Get_PqnamebyCjname(e[j].zzName.ToString());

                    object o = new
                    {
                        //index = count++,
                        zzname = e[j].zzName.ToString(),
                        sbgybh = e[j].sbGyCode.ToString(),
                        sbcode = e[j].sbCode.ToString(),
                        notgood = s,
                        cjName = e[j].cjName.ToString(),
                        sbType = e[j].sbType.ToString(),
                        zyType = e[j].zyType.ToString(),
                        a_id = e[j].Id,
                        ym = e[j].yearMonthForStatistic,
                        entityid = e[j].temp2.ToString(),
                        wtfx = "",
                        clsd = "",
                        cljg = "",
                        wtms = problemDescription,
                        bwhxs = bwhxs,
                        bwhcs = bwhcs,
                        //pq=pq

                    };
                    r.Add(o);
                    s = "";
                }
            }
            for (int k = 0; k < e.Count; k++)
            {
                tm.modifytemp1_byid(e[k].Id, null);
            }
            string str = JsonConvert.SerializeObject(r);
            return ("{" + "\"data\": " + str + "}");

        }
        public string list2()
        {
            DateTime my = DateTime.Now;
            string yearmonth = "";
            if (my.Day >= 15)
            {
                yearmonth = my.Year.ToString() + my.AddMonths(1).Month.ToString();
            }
            else
            {
                yearmonth = my.Year.ToString() + my.Month.ToString();
            }
            TablesManagment tm = new TablesManagment();
            List<A5dot1Tab2> E = tm.GetAll2_byymandstate(yearmonth, 1);
            List<object> r = new List<object>();
            for (int i = 0; i < E.Count; i++)
            {
                object o = new
                {
                    //index = i + 1,
                    zzname = E[i].zzName.ToString(),
                    sbgybh = E[i].sbGyCode.ToString(),
                    sbcode = E[i].sbCode.ToString(),
                    notgood = E[i].notGoodContents.ToString(),
                    a_id = E[i].Id

                };
                r.Add(o);
            }



            string str = JsonConvert.SerializeObject(r);
            return ("{" + "\"data\": " + str + "}");
        }
        public string list3()
        {
            DateTime my = DateTime.Now;
            string yearmonth = "";
            if (my.Day >= 15)
            {
                yearmonth = my.Year.ToString() + my.AddMonths(1).Month.ToString();
            }
            else
            {
                yearmonth = my.Year.ToString() + my.Month.ToString();
            }
            TablesManagment tm = new TablesManagment();
            List<A5dot1Tab2> E = tm.GetAll2_byymandstate(yearmonth, 2);
            List<object> r = new List<object>();
            for (int i = 0; i < E.Count; i++)
            {
                object o = new
                {
                    //index = i + 1,
                    zzname = E[i].zzName.ToString(),
                    sbgybh = E[i].sbGyCode.ToString(),
                    sbcode = E[i].sbCode.ToString(),
                    notgood = E[i].notGoodContents.ToString(),
                    a_id = E[i].Id

                };
                r.Add(o);
            }



            string str = JsonConvert.SerializeObject(r);
            return ("{" + "\"data\": " + str + "}");
        }
        public string Pqcheck_submitsignal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string pqname = item["pqUserName"].ToString();
                int a_id = Convert.ToInt32(item["a_id"]);
                DateTime time = DateTime.Now;
                TablesManagment tm = new TablesManagment();
                tm.Pqcheck_byid(a_id, pqname, time);
                if (tm.GetA_byid(a_id).temp3 != null)
                {
                    int missId = Convert.ToInt32(tm.GetA_byid(a_id).temp3);
                    var t = CTimerManage.LoadTimerMission(missId);

                    if (t != null)
                    {
                        t.status = TM_STATUS.TM_FINISH;
                        t.Save();
                        CTimerManage.RemoveFromActiveList(t.ID);
                    }
                }
            }
            catch (Exception e)
            {
                return "";
            }
            return ("/A5dot1/Index");
        }

        public string save5(string json1)
        {
            //List<string> json = new List<string>();
            JArray jsonVal = JArray.Parse(json1) as JArray;
            dynamic table2 = jsonVal;
            string userName = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
            int userid = (Session["User"] as EquipModel.Entities.Person_Info).Person_Id;
            PersonManagment pm = new PersonManagment();

            TablesManagment tm = new TablesManagment();
            A5dot1Tab2 a = new A5dot1Tab2();
            // List<A5dot1Tab2> temp = new List<A5dot1Tab2>();
            foreach (dynamic T in table2)
            {
                a.cjName = T.CjName;
                a.zzName = T.ZzName;
                a.sbGyCode = T.sbGyCode;
                a.sbCode = T.sbCode;
                a.notGoodContents = T.notGoodContents;
                a.zyType = T.zyType;
                a.sbType = T.sbType;
                a.yearMonthForStatistic = T.ym;
                a.isSetAsTop5Worst = 1;
                a.state = 1;
                a.pqUserName = userName;
                a.problemAnalysisAdvise = T.wtfx;
                a.processMeansMethods = T.clsd;
                a.processReuslt = T.cljg;
                a.problemDescription = T.wtms;
                a.timesNotGood = T.bwhcs;
                a.countAllNoRectifed = T.bwhxs;
                if (a.cjName == "消防队" || a.cjName == "计量站")
                    a.pqName = a.cjName;
                else
                    a.pqName = pm.Get_PqnamebyCjname(a.cjName.ToString());
                a.temp2 = T.entityid;
                tm.savefivesb(a);

            }
            return null;
        }
        public string save10(string json1)
        {
            //List<string> json = new List<string>();
            string userName = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
            DateTime my = DateTime.Now;
            string yearmonth = "";
            if (my.Day >= 15)
            {
                yearmonth = my.Year.ToString() + my.AddMonths(1).Month.ToString();
            }
            else
            {
                yearmonth = my.Year.ToString() + my.Month.ToString();
            }
            TablesManagment tm = new TablesManagment();
            A5dot1Tab2 a = new A5dot1Tab2();
            JObject item = (JObject)JsonConvert.DeserializeObject(json1);
            string[] a_id = item["a_id"].ToString().Split('|');
            List<A5dot1Tab2> E = tm.GetAll2_byymandstate(yearmonth, 2);
            for (int i = 0; i < E.Count; i++)
            {
                tm.setstate_byid(E[i].Id, 3, userName);
            }
            for (int j = 0; j < a_id.Count(); j++)
            {
                tm.setisworst10_byid(Convert.ToInt16(a_id[j]), 1);
            }
            return ("/A5dot1/Index");
        }

        public ActionResult JxEdit(string wfe_id)
        {
            return View(getSubmitModel(wfe_id));
        }
        public string delete_worst5(string json1)
        {
            JObject item = (JObject)JsonConvert.DeserializeObject(json1);
            int a_id = Convert.ToInt32(item["a_id"]);
            TablesManagment tm = new TablesManagment();
            tm.delete_byid(a_id);
            return "删除成功";
        }
        public string OutputExcel(string time)
        {
            try
            {
                DateTime my = DateTime.Now;
                string title = "";
                string ym;
                if (my.Day >= 15)
                {
                    title = my.Year.ToString() + "年" + my.AddMonths(1).Month.ToString() + "月" + "最差十台机泵";
                    //ym = my.Year.ToString() + my.AddMonths(1).Month.ToString();
                }
                else
                {
                    title = my.Year.ToString() + "年" + my.Month.ToString() + "月" + "最差十台机泵";
                    // ym = my.Year.ToString() + my.Month.ToString();
                }
                if (time == "")
                {
                    if (my.Day >= 15)
                    {
                        ym = my.Year.ToString() + my.AddMonths(1).Month.ToString();
                    }
                    else
                    {
                        ym = my.Year.ToString() + my.Month.ToString();
                    }

                }
                else
                {

                    string[] s = time.Split(new char[] { '-' });
                    ym = s[0] + s[1];
                }

                TablesManagment tm = new TablesManagment();
                List<A5dot1Tab2> worst10 = tm.get_worst10byym(ym);
                // 创建Excel 文档

                HSSFWorkbook wk = new HSSFWorkbook();

                ISheet tb = wk.CreateSheet("sheet1");

                //设置单元的宽度  
                tb.SetColumnWidth(0, 25 * 256);
                tb.SetColumnWidth(1, 20 * 256);
                tb.SetColumnWidth(2, 20 * 256);
                tb.SetColumnWidth(3, 20 * 256);
                tb.SetColumnWidth(4, 20 * 256);
                tb.SetColumnWidth(5, 20 * 256);
                tb.SetColumnWidth(6, 45 * 256);


                //合并标题头，该方法的参数次序是：开始行号，结束行号，开始列号，结束列号。
                tb.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 0, 0, 6));

                IRow head = tb.CreateRow(0);

                head.Height = 20 * 30;
                ICell head_first_cell = head.CreateCell(0);
                ICellStyle cellStyle_head = wk.CreateCellStyle();

                //对齐
                cellStyle_head.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                // 边框
                /*
                cellStyle_head.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                cellStyle_head.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                cellStyle_head.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                cellStyle_head.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                 * */

                // 字体
                IFont font = wk.CreateFont();
                font.FontName = "微软雅黑";
                font.Boldweight = 8;
                font.FontHeightInPoints = 16;
                cellStyle_head.SetFont(font);

                head_first_cell.CellStyle = head.CreateCell(1).CellStyle
                                          = head.CreateCell(2).CellStyle
                                          = head.CreateCell(3).CellStyle
                                          = head.CreateCell(4).CellStyle
                                          = head.CreateCell(5).CellStyle
                                          = head.CreateCell(6).CellStyle
                                          = cellStyle_head;


                head_first_cell.SetCellValue(title);


                IRow table_head = tb.CreateRow(1);

                ICellStyle cellStyle_normal = wk.CreateCellStyle();

                cellStyle_normal.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                cellStyle_normal.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                cellStyle_normal.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                cellStyle_normal.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;

                ICell table_head_cj = table_head.CreateCell(0);
                table_head_cj.CellStyle = cellStyle_normal;
                table_head_cj.SetCellValue("车间");

                ICell table_head_zz = table_head.CreateCell(1);
                table_head_zz.CellStyle = cellStyle_normal;
                table_head_zz.SetCellValue("装置");

                ICell table_head_sbgycode = table_head.CreateCell(2);
                table_head_sbgycode.CellStyle = cellStyle_normal;
                table_head_sbgycode.SetCellValue("设备位号");

                ICell table_head_sbcode = table_head.CreateCell(3);
                table_head_sbcode.CellStyle = cellStyle_normal;
                table_head_sbcode.SetCellValue("设备编号");

                ICell table_head_bwh = table_head.CreateCell(4);
                table_head_bwh.CellStyle = cellStyle_normal;
                table_head_bwh.SetCellValue("当月不完好数");

                ICell table_head_bwhxs = table_head.CreateCell(5);
                table_head_bwhxs.CellStyle = cellStyle_normal;
                table_head_bwhxs.SetCellValue("当前累计未整改不完好数");

                ICell table_head_bhwxq = table_head.CreateCell(6);
                table_head_bhwxq.CellStyle = cellStyle_normal;
                table_head_bhwxq.SetCellValue("不完好内容详情");


                ICellStyle thjl_record_cellStyle = wk.CreateCellStyle();

                thjl_record_cellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                thjl_record_cellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                thjl_record_cellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                thjl_record_cellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                //水平对齐  
                thjl_record_cellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Left;
                //垂直对齐  
                thjl_record_cellStyle.VerticalAlignment = VerticalAlignment.Top;
                //自动换行  
                thjl_record_cellStyle.WrapText = true;

                int row_index = 2;

                for (int i = 0; i < worst10.Count; i++)
                {



                    IRow thjl_row = tb.CreateRow(row_index);

                    ICell cj = thjl_row.CreateCell(0);
                    cj.CellStyle = thjl_record_cellStyle;
                    cj.SetCellValue(worst10[i].cjName.ToString());

                    ICell zz = thjl_row.CreateCell(1);
                    zz.CellStyle = thjl_record_cellStyle;
                    zz.SetCellValue(worst10[i].zzName.ToString());

                    ICell sbgycode = thjl_row.CreateCell(2);
                    sbgycode.CellStyle = thjl_record_cellStyle;
                    sbgycode.SetCellValue(worst10[i].sbGyCode.ToString());

                    ICell sbcode = thjl_row.CreateCell(3);
                    sbcode.CellStyle = thjl_record_cellStyle;
                    sbcode.SetCellValue(worst10[i].sbCode.ToString());

                    ICell bwh = thjl_row.CreateCell(4);
                    bwh.CellStyle = thjl_record_cellStyle;
                    bwh.SetCellValue(worst10[i].timesNotGood.ToString());

                    ICell bwhxs = thjl_row.CreateCell(5);
                    bwhxs.CellStyle = thjl_record_cellStyle;
                    bwhxs.SetCellValue(worst10[i].countAllNoRectifed.ToString());

                    ICell bwhxq = thjl_row.CreateCell(6);
                    bwhxq.CellStyle = thjl_record_cellStyle;
                    bwhxq.SetCellValue(worst10[i].notGoodContents.ToString());

                    row_index++;

                }





                string path = Server.MapPath("~/Upload//最差十台机泵.xls");
                using (FileStream fs = System.IO.File.OpenWrite(path)) //打开一个xls文件，如果没有则自行创建，如果存在myxls.xls文件则在创建是不要打开该文件！
                {
                    wk.Write(fs);   //向打开的这个xls文件中写入mySheet表并保存。
                    Console.WriteLine("提示：创建成功！");
                }
                return Path.Combine("/Upload", "最差十台机泵.xls");

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return "";
            }


        }

        public string OutputExcel_2()
        {
            try
            {
                DateTime my = DateTime.Now;
                string ym;
                TablesManagment tm = new TablesManagment();
                if (my.Day >= 15)
                {
                    ym = my.Year.ToString() + my.AddMonths(1).Month.ToString();
                }
                else
                {
                    ym = my.Year.ToString() + my.Month.ToString();
                }
                EquipManagment Em = new EquipManagment();
                List<EANummodel> E = Em.getequipnum_byarchi();
                List<NameandNum> cj = new List<NameandNum>();
                List<NameandNum> pq = new List<NameandNum>();

                int count = 0;
                for (int i = 0; i < 8; i++)
                {
                    count += E[i].Equip_Num;
                }
                NameandNum temp = new NameandNum();
                temp.Equip_Num = count;
                temp.name = "联合一车间";
                cj.Add(temp);
                count = 0;
                for (int i = 8; i < 11; i++)
                {
                    count += E[i].Equip_Num;
                }
                temp = new NameandNum();
                temp.Equip_Num = count;
                temp.name = "联合二车间";
                cj.Add(temp);
                count = 0;
                for (int i = 11; i < 15; i++)
                {
                    count += E[i].Equip_Num;
                }
                temp = new NameandNum();
                temp.Equip_Num = count;
                temp.name = "联合三车间";
                cj.Add(temp);
                count = 0;
                for (int i = 15; i < 17; i++)
                {
                    count += E[i].Equip_Num;
                }
                temp = new NameandNum();
                temp.Equip_Num = count;
                temp.name = "焦化车间";
                cj.Add(temp);
                count = 0;
                for (int i = 17; i < 18; i++)
                {
                    count += E[i].Equip_Num;
                }
                temp = new NameandNum();
                temp.Equip_Num = count;
                temp.name = "铁路车间";
                cj.Add(temp);
                count = 0;
                for (int i = 18; i < 22; i++)
                {
                    count += E[i].Equip_Num;
                }
                temp = new NameandNum();
                temp.Equip_Num = count;
                temp.name = "气加车间";
                cj.Add(temp);
                count = 0;
                for (int i = 22; i < 24; i++)
                {
                    count += E[i].Equip_Num;
                }
                temp = new NameandNum();
                temp.Equip_Num = count;
                temp.name = "聚丙烯车间";
                cj.Add(temp);
                count = 0;
                for (int i = 24; i < 26; i++)
                {
                    count += E[i].Equip_Num;
                }
                temp = new NameandNum();
                temp.Equip_Num = count;
                temp.name = "综合车间";
                cj.Add(temp);
                count = 0;
                List<string> s = new List<string>();
                s.Add("油品车间");
                s.Add("排水车间");
                s.Add("供水车间");
                s.Add("热点车间");
                s.Add("码头车间");
                s.Add("消防队");
                s.Add("计量站");
                for (int i = 26; i < 33; i++)
                {
                    count = E[i].Equip_Num;
                    temp = new NameandNum();
                    temp.Equip_Num = count;
                    temp.name = s[i - 26];
                    cj.Add(temp);
                    count = 0;
                }


                for (int i = 0; i < 1; i++)
                {
                    count += cj[i].Equip_Num;
                    temp = new NameandNum();
                    temp.Equip_Num = count;
                    temp.name = "联合一片区";
                    pq.Add(temp);
                    count = 0;
                }
                for (int i = 1; i < 2; i++)
                {
                    count += cj[i].Equip_Num;
                    temp = new NameandNum();
                    temp.Equip_Num = count;
                    temp.name = "联合二片区";
                    pq.Add(temp);
                    count = 0;
                }
                for (int i = 2; i < 3; i++)
                {
                    count += cj[i].Equip_Num;
                    temp = new NameandNum();
                    temp.Equip_Num = count;
                    temp.name = "联合三片区";
                    pq.Add(temp);
                    count = 0;
                }
                for (int i = 3; i < 5; i++)
                {
                    count += cj[i].Equip_Num;

                }
                temp = new NameandNum();
                temp.Equip_Num = count;
                temp.name = "焦化片区";
                pq.Add(temp);
                count = 0;
                for (int i = 5; i < 7; i++)
                {
                    count += cj[i].Equip_Num;

                }
                temp = new NameandNum();
                temp.Equip_Num = count;
                temp.name = "化工片区";
                pq.Add(temp);
                count = 0;
                for (int i = 7; i < 8; i++)
                {
                    count += cj[i].Equip_Num;

                }
                temp = new NameandNum();
                temp.Equip_Num = count;
                temp.name = "综合片区";
                pq.Add(temp);
                count = 0;
                for (int i = 8; i < 13; i++)
                {
                    count += cj[i].Equip_Num;

                }
                temp = new NameandNum();
                temp.Equip_Num = count;
                temp.name = "系统片区";
                pq.Add(temp);
                count = 0;
                for (int i = 13; i < 15; i++)
                {
                    count += cj[i].Equip_Num;

                }
                temp = new NameandNum();
                temp.Equip_Num = count;
                temp.name = "其他";
                pq.Add(temp);
                count = 0;

                for (int i = 0; i < pq.Count; i++)
                {
                    count += pq[i].Equip_Num;
                }
                List<A5dot1Tab1> qc_list = tm.get_All();
                double qcbwh = 0.000;
                double qc_bxhcount = 0;
                int wzgcount = 0;
                if (qc_list.Count > 0)
                {
                    qc_bxhcount = 0;
                    wzgcount = 0;
                    string sbcode_temp = qc_list[0].sbCode;
                    for (int j = 0; j < qc_list.Count; j++)
                    {
                        qc_list = tm.get_All();
                        if (qc_list[j].temp1 == null)
                        {
                            List<A5dot1Tab1> cj_bycode = tm.GetAll1_bycode(qc_list[j].sbCode);
                            for (int k = 0; k < cj_bycode.Count; k++)
                            {
                                if (cj_bycode[k].isRectified == 0)
                                {
                                    wzgcount++;
                                }
                                tm.modifytemp1_byid(cj_bycode[k].Id, "已合并");

                            }
                            if (wzgcount > 0)
                            {
                                qc_bxhcount++;
                            }
                            wzgcount = 0;
                        }

                        // cjbwh.Add(f);
                    }

                }
                for (int n = 0; n < qc_list.Count; n++)
                {
                    tm.modifytemp1_byid(qc_list[n].Id, null);
                }
                qcbwh = Math.Round(((double)qc_bxhcount / count), 6);
                List<double> cjbwh = new List<double>();
                List<double> pqbwh = new List<double>();


                for (int i = 0; i < cj.Count; i++)
                {
                    List<A5dot1Tab1> cj_list = tm.get_cj_bwh(cj[i].name, cj[i].Equip_Num);
                    double cj_bxhcount = 0;
                    int wzg_count = 0;
                    if (cj_list.Count > 0)
                    {
                        cj_bxhcount = 0;
                        wzg_count = 0;
                        string sbcode_temp = cj_list[0].sbCode;
                        for (int j = 0; j < cj_list.Count; j++)
                        {
                            cj_list = tm.get_cj_bwh(cj[i].name, cj[i].Equip_Num);
                            if (cj_list[j].temp1 == null)
                            {
                                List<A5dot1Tab1> cj_bycode = tm.GetAll1_bycode(cj_list[j].sbCode);
                                for (int k = 0; k < cj_bycode.Count; k++)
                                {
                                    if (cj_bycode[k].isRectified == 0)
                                    {
                                        wzg_count++;
                                    }
                                    tm.modifytemp1_byid(cj_bycode[k].Id, "已合并");
                                }

                            }
                        }
                        if (wzg_count > 0)
                        {
                            cj_bxhcount++;
                        }
                    }
                    else
                    {
                        cjbwh.Add(0.000);
                    }
                    for (int n = 0; n < cj_list.Count; n++)
                    {
                        tm.modifytemp1_byid(cj_list[n].Id, null);
                    }
                    cjbwh.Add(Math.Round(((double)cj_bxhcount / cj[i].Equip_Num), 6));
                }


                for (int i = 0; i < pq.Count; i++)
                {
                    List<A5dot1Tab1> pq_list = tm.get_pq_bwh(pq[i].name, pq[i].Equip_Num);
                    double pq_bxhcount = 0;
                    int wzg_count = 0;
                    if (pq_list.Count > 0)
                    {
                        pq_bxhcount = 0;
                        wzg_count = 0;
                        string sbcode_temp = pq_list[0].sbCode;
                        for (int j = 0; j < pq_list.Count; j++)
                        {
                            pq_list = tm.get_cj_bwh(cj[i].name, cj[i].Equip_Num);
                            if (pq_list[j].temp1 == null)
                            {
                                List<A5dot1Tab1> cj_bycode = tm.GetAll1_bycode(pq_list[j].sbCode);
                                for (int k = 0; k < cj_bycode.Count; k++)
                                {
                                    if (cj_bycode[k].isRectified == 0)
                                    {
                                        wzg_count++;
                                    }
                                    tm.modifytemp1_byid(cj_bycode[k].Id, "已合并");

                                }
                                if (wzg_count > 0)
                                {
                                    pq_bxhcount++;
                                }
                                wzg_count = 0;
                            }

                        }
                    }

                    for (int n = 0; n < pq_list.Count; n++)
                    {
                        tm.modifytemp1_byid(pq_list[n].Id, null);
                    }
                    pqbwh.Add(Math.Round(((double)pq_bxhcount / pq[i].Equip_Num), 6));
                }



                // 创建Excel 文档

                HSSFWorkbook wk = new HSSFWorkbook();

                ISheet tb = wk.CreateSheet("sheet1");

                //设置单元的宽度  
                tb.SetColumnWidth(0, 25 * 256);
                tb.SetColumnWidth(1, 20 * 256);
                tb.SetColumnWidth(2, 20 * 256);
                tb.SetColumnWidth(3, 20 * 256);
                tb.SetColumnWidth(4, 20 * 256);



                //合并标题头，该方法的参数次序是：开始行号，结束行号，开始列号，结束列号。
                tb.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 0, 0, 4));

                tb.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(5, 6, 0, 0));

                tb.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(5, 6, 1, 1));

                tb.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(7, 8, 0, 0));

                tb.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(7, 8, 1, 1));

                tb.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(10, 14, 0, 0));

                tb.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(10, 14, 1, 1));

                tb.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(15, 16, 0, 0));

                tb.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(15, 16, 1, 1));

                tb.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(2, 16, 4, 4));

                IRow head = tb.CreateRow(0);

                head.Height = 20 * 30;
                ICell head_first_cell = head.CreateCell(0);
                ICellStyle cellStyle_head = wk.CreateCellStyle();

                //对齐
                cellStyle_head.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                // 边框
                /*
                cellStyle_head.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                cellStyle_head.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                cellStyle_head.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                cellStyle_head.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                 * */

                // 字体
                IFont font = wk.CreateFont();
                font.FontName = "微软雅黑";
                font.Boldweight = 8;
                font.FontHeightInPoints = 16;
                cellStyle_head.SetFont(font);

                head_first_cell.CellStyle = head.CreateCell(1).CellStyle
                                          = head.CreateCell(2).CellStyle
                                          = head.CreateCell(3).CellStyle
                                          = head.CreateCell(4).CellStyle
                                          = cellStyle_head;


                head_first_cell.SetCellValue("不完好率统计表");

                //1
                IRow table_head = tb.CreateRow(1);

                ICellStyle cellStyle_normal = wk.CreateCellStyle();

                cellStyle_normal.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                cellStyle_normal.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                cellStyle_normal.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                cellStyle_normal.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;

                ICell table_head_cj = table_head.CreateCell(0);
                table_head_cj.CellStyle = cellStyle_normal;
                table_head_cj.SetCellValue("片区名称");

                ICell table_head_zz = table_head.CreateCell(1);
                table_head_zz.CellStyle = cellStyle_normal;
                table_head_zz.SetCellValue("片区不完好率");

                ICell table_head_sbgycode = table_head.CreateCell(2);
                table_head_sbgycode.CellStyle = cellStyle_normal;
                table_head_sbgycode.SetCellValue("车间名称");

                ICell table_head_sbcode = table_head.CreateCell(3);
                table_head_sbcode.CellStyle = cellStyle_normal;
                table_head_sbcode.SetCellValue("车间不完好率");

                ICell table_head_bwh = table_head.CreateCell(4);
                table_head_bwh.CellStyle = cellStyle_normal;
                table_head_bwh.SetCellValue("全场不完好率");





                ICellStyle thjl_record_cellStyle = wk.CreateCellStyle();

                thjl_record_cellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                thjl_record_cellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                thjl_record_cellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                thjl_record_cellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                //水平对齐  
                thjl_record_cellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Left;
                //垂直对齐  
                thjl_record_cellStyle.VerticalAlignment = VerticalAlignment.Top;
                //自动换行  
                thjl_record_cellStyle.WrapText = true;
                //2

                IRow row_2 = tb.CreateRow(2);

                ICell qp_1 = row_2.CreateCell(0);
                qp_1.CellStyle = thjl_record_cellStyle;
                qp_1.SetCellValue("联合一片区");

                ICell pq_bwh = row_2.CreateCell(1);
                pq_bwh.CellStyle = thjl_record_cellStyle;
                pq_bwh.SetCellValue(pqbwh[0].ToString());

                ICell cj_1 = row_2.CreateCell(2);
                cj_1.CellStyle = thjl_record_cellStyle;
                cj_1.SetCellValue("联合一车间");

                ICell cj_bwh = row_2.CreateCell(3);
                cj_bwh.CellStyle = thjl_record_cellStyle;
                cj_bwh.SetCellValue(cjbwh[0].ToString());

                ICell qc_bwh = row_2.CreateCell(4);
                qc_bwh.CellStyle = thjl_record_cellStyle;
                qc_bwh.SetCellValue(qcbwh.ToString());
                //3

                IRow row_3 = tb.CreateRow(3);

                ICell qp_2 = row_3.CreateCell(0);
                qp_2.CellStyle = thjl_record_cellStyle;
                qp_2.SetCellValue("联合二片区");

                ICell pq2_bwh = row_3.CreateCell(1);
                pq2_bwh.CellStyle = thjl_record_cellStyle;
                pq2_bwh.SetCellValue(pqbwh[1].ToString());

                ICell cj_2 = row_3.CreateCell(2);
                cj_2.CellStyle = thjl_record_cellStyle;
                cj_2.SetCellValue("联合二车间");

                ICell cj2_bwh = row_3.CreateCell(3);
                cj2_bwh.CellStyle = thjl_record_cellStyle;
                cj2_bwh.SetCellValue(cjbwh[1].ToString());

                //4
                IRow row_4 = tb.CreateRow(4);

                ICell qp_3 = row_4.CreateCell(0);
                qp_3.CellStyle = thjl_record_cellStyle;
                qp_3.SetCellValue("联合三片区");

                ICell pq3_bwh = row_4.CreateCell(1);
                pq3_bwh.CellStyle = thjl_record_cellStyle;
                pq3_bwh.SetCellValue(pqbwh[2].ToString());

                ICell cj_3 = row_4.CreateCell(2);
                cj_3.CellStyle = thjl_record_cellStyle;
                cj_3.SetCellValue("联合三车间");

                ICell cj3_bwh = row_4.CreateCell(3);
                cj3_bwh.CellStyle = thjl_record_cellStyle;
                cj3_bwh.SetCellValue(cjbwh[2].ToString());

                //5

                IRow row_5 = tb.CreateRow(5);

                ICell qp_4 = row_5.CreateCell(0);
                qp_4.CellStyle = thjl_record_cellStyle;
                qp_4.SetCellValue("焦化片区");

                ICell pq4_bwh = row_5.CreateCell(1);
                pq4_bwh.CellStyle = thjl_record_cellStyle;
                pq4_bwh.SetCellValue(pqbwh[3].ToString());

                ICell cj_4 = row_5.CreateCell(2);
                cj_4.CellStyle = thjl_record_cellStyle;
                cj_4.SetCellValue("焦化车间");

                ICell cj4_bwh = row_5.CreateCell(3);
                cj4_bwh.CellStyle = thjl_record_cellStyle;
                cj4_bwh.SetCellValue(cjbwh[3].ToString());

                //6

                IRow row_6 = tb.CreateRow(6);


                ICell cj_5 = row_6.CreateCell(2);
                cj_5.CellStyle = thjl_record_cellStyle;
                cj_5.SetCellValue("铁路车间");

                ICell cj5_bwh = row_6.CreateCell(3);
                cj5_bwh.CellStyle = thjl_record_cellStyle;
                cj5_bwh.SetCellValue(cjbwh[4].ToString());

                //7
                IRow row_7 = tb.CreateRow(7);

                ICell qp_6 = row_7.CreateCell(0);
                qp_6.CellStyle = thjl_record_cellStyle;
                qp_6.SetCellValue("化工片区");

                ICell pq6_bwh = row_7.CreateCell(1);
                pq6_bwh.CellStyle = thjl_record_cellStyle;
                pq6_bwh.SetCellValue(pqbwh[4].ToString());

                ICell cj_6 = row_7.CreateCell(2);
                cj_6.CellStyle = thjl_record_cellStyle;
                cj_6.SetCellValue("气加车间");

                ICell cj6_bwh = row_7.CreateCell(3);
                cj6_bwh.CellStyle = thjl_record_cellStyle;
                cj6_bwh.SetCellValue(cjbwh[5].ToString());

                //8
                IRow row_8 = tb.CreateRow(8);


                ICell cj_7 = row_8.CreateCell(2);
                cj_7.CellStyle = thjl_record_cellStyle;
                cj_7.SetCellValue("聚丙烯车间");

                ICell cj7_bwh = row_8.CreateCell(3);
                cj7_bwh.CellStyle = thjl_record_cellStyle;
                cj7_bwh.SetCellValue(cjbwh[6].ToString());
                //9

                IRow row_9 = tb.CreateRow(9);

                ICell qp_8 = row_9.CreateCell(0);
                qp_8.CellStyle = thjl_record_cellStyle;
                qp_8.SetCellValue("综合片区");

                ICell pq8_bwh = row_9.CreateCell(1);
                pq8_bwh.CellStyle = thjl_record_cellStyle;
                pq8_bwh.SetCellValue(pqbwh[5].ToString());

                ICell cj_8 = row_9.CreateCell(2);
                cj_8.CellStyle = thjl_record_cellStyle;
                cj_8.SetCellValue("综合车间");

                ICell cj8_bwh = row_9.CreateCell(3);
                cj8_bwh.CellStyle = thjl_record_cellStyle;
                cj8_bwh.SetCellValue(cjbwh[7].ToString());
                //10

                IRow row_10 = tb.CreateRow(10);

                ICell qp_9 = row_10.CreateCell(0);
                qp_9.CellStyle = thjl_record_cellStyle;
                qp_9.SetCellValue("系统片区");

                ICell pq9_bwh = row_10.CreateCell(1);
                pq9_bwh.CellStyle = thjl_record_cellStyle;
                pq9_bwh.SetCellValue(pqbwh[6].ToString());

                ICell cj_9 = row_10.CreateCell(2);
                cj_9.CellStyle = thjl_record_cellStyle;
                cj_9.SetCellValue("油品车间");

                ICell cj9_bwh = row_10.CreateCell(3);
                cj9_bwh.CellStyle = thjl_record_cellStyle;
                cj9_bwh.SetCellValue(cjbwh[8].ToString());

                //11
                IRow row_11 = tb.CreateRow(11);


                ICell cj_10 = row_11.CreateCell(2);
                cj_10.CellStyle = thjl_record_cellStyle;
                cj_10.SetCellValue("排水车间");

                ICell cj10_bwh = row_11.CreateCell(3);
                cj10_bwh.CellStyle = thjl_record_cellStyle;
                cj10_bwh.SetCellValue(cjbwh[9].ToString());
                //12
                IRow row_12 = tb.CreateRow(12);


                ICell cj_11 = row_12.CreateCell(2);
                cj_11.CellStyle = thjl_record_cellStyle;
                cj_11.SetCellValue("供水车间");

                ICell cj11_bwh = row_12.CreateCell(3);
                cj11_bwh.CellStyle = thjl_record_cellStyle;
                cj11_bwh.SetCellValue(cjbwh[10].ToString());

                //13
                IRow row_13 = tb.CreateRow(13);


                ICell cj_12 = row_13.CreateCell(2);
                cj_12.CellStyle = thjl_record_cellStyle;
                cj_12.SetCellValue("热电车间");

                ICell cj12_bwh = row_13.CreateCell(3);
                cj12_bwh.CellStyle = thjl_record_cellStyle;
                cj12_bwh.SetCellValue(cjbwh[11].ToString());

                //14
                IRow row_14 = tb.CreateRow(14);


                ICell cj_13 = row_14.CreateCell(2);
                cj_13.CellStyle = thjl_record_cellStyle;
                cj_13.SetCellValue("热电车间");

                ICell cj13_bwh = row_14.CreateCell(3);
                cj13_bwh.CellStyle = thjl_record_cellStyle;
                cj13_bwh.SetCellValue(cjbwh[12].ToString());

                //15

                IRow row_15 = tb.CreateRow(15);

                ICell qp_14 = row_15.CreateCell(0);
                qp_14.CellStyle = thjl_record_cellStyle;
                qp_14.SetCellValue("其他");

                ICell pq14_bwh = row_15.CreateCell(1);
                pq14_bwh.CellStyle = thjl_record_cellStyle;
                pq14_bwh.SetCellValue(pqbwh[7].ToString());

                ICell cj_14 = row_15.CreateCell(2);
                cj_14.CellStyle = thjl_record_cellStyle;
                cj_14.SetCellValue("消防队");

                ICell cj14_bwh = row_15.CreateCell(3);
                cj14_bwh.CellStyle = thjl_record_cellStyle;
                cj14_bwh.SetCellValue(cjbwh[13].ToString());

                //16
                IRow row_16 = tb.CreateRow(16);


                ICell cj_15 = row_16.CreateCell(2);
                cj_15.CellStyle = thjl_record_cellStyle;
                cj_15.SetCellValue("计量站");

                ICell cj15_bwh = row_16.CreateCell(3);
                cj15_bwh.CellStyle = thjl_record_cellStyle;
                cj15_bwh.SetCellValue(cjbwh[14].ToString());

                string path = Server.MapPath("~/Upload//不完好率统计表.xls");
                using (FileStream fs = System.IO.File.OpenWrite(path)) //打开一个xls文件，如果没有则自行创建，如果存在myxls.xls文件则在创建是不要打开该文件！
                {
                    wk.Write(fs);   //向打开的这个xls文件中写入mySheet表并保存。
                    Console.WriteLine("提示：创建成功！");
                }
                return Path.Combine("/Upload", "不完好率统计表.xls");

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return "";
            }


        }

        public double getqcbwh_bytime(DateTime time)
        {
            EquipManagment Em = new EquipManagment();
            TablesManagment tm = new TablesManagment();
            List<EANummodel> E = Em.getequipnum_byarchi();
            int qcsb_count = 0;
            for (int i = 0; i < E.Count; i++)
            {
                qcsb_count += E[i].Equip_Num;

            }
            List<A5dot1Tab1> qc_list = tm.get_Allbytime(time);
            double qcbwh = 0.000;
            double qc_bxhcount = 0;
            int wzgcount = 0;
            if (qc_list.Count > 0)
            {
                qc_bxhcount = 0;
                wzgcount = 0;
                string sbcode_temp = qc_list[0].sbCode;
                for (int j = 0; j < qc_list.Count; j++)
                {
                    qc_list = tm.get_Allbytime(time);
                    if (qc_list[j].temp1 == null)
                    {
                        List<A5dot1Tab1> cj_bycode = tm.GetAll1_bycode(qc_list[j].sbCode);
                        for (int k = 0; k < cj_bycode.Count; k++)
                        {
                            if (cj_bycode[k].isRectified == 0 || cj_bycode[k].pqCheckTime == null || cj_bycode[k].pqCheckTime > time)
                            {
                                wzgcount++;
                            }
                            tm.modifytemp1_byid(cj_bycode[k].Id, "已合并");

                        }
                        if (wzgcount > 0)
                        {
                            qc_bxhcount++;
                        }
                        wzgcount = 0;
                    }

                    // cjbwh.Add(f);
                }

            }
            for (int n = 0; n < qc_list.Count; n++)
            {
                tm.modifytemp1_byid(qc_list[n].Id, null);
            }
            qcbwh = Math.Round(((double)qc_bxhcount / qcsb_count), 6);
            return qcbwh;
        }

        public string Incomplete()
        {
            DateTime my = DateTime.Now;
            Double incomplete_1;
            Double incomplete_2;
            Double incomplete_3;
            Double incomplete_4;
            Double incomplete_5;
            Double incomplete_6;

            string nowmonth;//获取当前月份
            if (my.Day >= 15)
            {
                nowmonth = my.AddMonths(1).Month.ToString();
            }
            else
            {
                nowmonth = my.Month.ToString();
            }
            Double[] incomplete = new Double[Convert.ToInt32(nowmonth)];//创建一个大小根据月份来确定对的数组如9月,数组长度为9
            incomplete[Convert.ToInt32(nowmonth) - 1] = 1 - getqcbwh_bytime(my);//1-getqcbwh_bytime(my)为当月完好率，是完好
            for (int i = Convert.ToInt32(nowmonth); i > 1; i--)
            {
                DateTime lasttime = Convert.ToDateTime(my.Year + "/" + (i - 1) + "/" + 15 + " 00:00:00");
                incomplete[i - 2] = 1 - getqcbwh_bytime(lasttime);//完好率
            }
            ////前一个月不完好率根据nowmonth取


            //DateTime lasttime2 = Convert.ToDateTime(my.Year + "/" + (Convert.ToInt32(nowmonth) - 2) + "/" + 15 + " 00:00:00");
            //incomplete_3 = getqcbwh_bytime(lasttime2);

            //DateTime lasttime3 = Convert.ToDateTime(my.Year + "/" + (Convert.ToInt32(nowmonth) - 3) + "/" + 15 + " 00:00:00");
            //incomplete_4 = getqcbwh_bytime(lasttime3);

            //DateTime lasttime4 = Convert.ToDateTime(my.Year + "/" + (Convert.ToInt32(nowmonth) - 4) + "/" + 15 + " 00:00:00");
            //incomplete_5 = getqcbwh_bytime(lasttime4);

            //DateTime lasttime5 = Convert.ToDateTime(my.Year + "/" + (Convert.ToInt32(nowmonth) - 5) + "/" + 15 + " 00:00:00");
            //incomplete_6 = getqcbwh_bytime(lasttime5);

            // string result=incomplete_1+"$"+incomplete_2+"$"+incomplete_3+"$"+incomplete_4+"$"+incomplete_5+"$"+incomplete_6+"$"+nowmonth;
            string result = "";
            for (int k = 0; k < Convert.ToInt32(nowmonth); k++)
            {
                result += incomplete[k] + "$";
            }
            return result;
        }
        public string Query_worst10bytime(string time)
        {
            List<object> r = new List<object>();
            List<A5dot1Tab2> worst10 = new List<A5dot1Tab2>();
            string[] s = time.Split(new char[] { '-' });
            string ym = s[0] + s[1];
            TablesManagment tm = new TablesManagment();
            worst10 = tm.get_worst10byym(ym);
            for (int i = 0; i < worst10.Count; i++)
            {
                object o = new
                {
                    // index = i + 1,
                    cjname = worst10[i].cjName,
                    zzname = worst10[i].zzName,
                    sbGyCode = worst10[i].sbGyCode,
                    sbCode = worst10[i].sbCode,
                    timesNotGood = worst10[i].timesNotGood,
                    countAllNoRectifed = worst10[i].timesNotGood,//这台最差设备当月累计未整改数 
                    notGoodContents = worst10[i].notGoodContents
                };
                r.Add(o);
            }
            string str = JsonConvert.SerializeObject(r);
            return ("{" + "\"data\": " + str + "}");

        }
    }
}
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
using EquipBLL;



namespace WebApp.Controllers
{
    public class A2Controller : CommonController
    {
        private JxpgManagment Jx = new JxpgManagment();
        private KpiManagement Jx1 = new KpiManagement();//kpi的bll层代码调用
        public JsonResult A2Cj_Zzs(int cj_id,string zy)
        {
            //JObject item = (JObject)JsonConvert.DeserializeObject(json1);
            //A15dot1TabDong new_15dot1dong = new A15dot1TabDong();
            //int cj_id = Convert.ToInt32(item["cj_id"]);
            //string zy = item["zy"].ToString();
            PersonManagment pm = new PersonManagment();
            Dictionary<string, string> ZY = new Dictionary<string, string>();
            DateTime Cxtime = DateTime.Now;
            string stime;
            string etime;//设置查询月份的格式，eg：1月15日0：00：00到2月14日23：59：59查询的是2月的数据
            if (Cxtime.Month > 1)
            {
                string Reducetime = (Cxtime.Month - 1).ToString();
                etime = Cxtime.Year.ToString() + '/' + Cxtime.Month + '/' + "14" + " 23:59";
                stime = Cxtime.Year.ToString() + '/' + Reducetime + '/' + "15" + " 00:00";
            }
            else
            {
                string Addtime = (Cxtime.Month + 1).ToString();
                stime = (Cxtime.Year - 1).ToString() + '/' + "12" + '/' + "15" + " 00:00";
                etime = Cxtime.Year.ToString() + '/' + "01" + '/' + "14" + " 23:59";
            }
            DateTime astime = Convert.ToDateTime(stime);
            DateTime aetime = Convert.ToDateTime(etime);
            ZY.Add("zy_name", "企业");
            ZY.Add("zy_name", "动");
            ZY.Add("zy_name", "静");
            ZY.Add("zy_name", "电");
            ZY.Add("zy_name", "仪");
            // EquipManagment em = new EquipManagment();
            int p_id = (Session["User"] as EquipModel.Entities.Person_Info).Person_Id;
            List<Equip_Archi> Zz = pm.Get_Person_Zz_ofCj(p_id, cj_id);

            List<object> Zz_obj = new List<object>();
            if (zy == "企业")
            {
                foreach (var i in Zz)
                { 
                    List<A15dot1TabQiYe> res = Jx1.GetJxByA2(astime,aetime, i.EA_Name, zy);
                    int issubmit =0;
                    if(res.Count>0)
                        issubmit=1;
                    object o = new
                    {
                        Zz_Id = i.EA_Id,
                        Zz_Name = i.EA_Name,
                        issubmit = issubmit
                    };
                    Zz_obj.Add(o);

                }
                return Json(Zz_obj.ToArray());
            }
  
            return Json(Zz_obj.ToArray());

        }
        public ActionResult Index(string time)
        {
            return View(getRecord(time));
        }
        //public ActionResult Index1()
        //{
        //    return View();
        //}
        public ActionResult Index2()
        {
            int cur_PersionID = (Session["User"] as EquipModel.Entities.Person_Info).Person_Id;
            PersonManagment pm = new PersonManagment();
            EquipBLL.AdminManagment.PersonManagment.P_viewModal pv = pm.Get_PersonModal(cur_PersionID);
            ViewBag.user_specis = pv.Speciaty_Names;
            ViewBag.rolename=pv.Role_Names;
            DateTime Cxtime = DateTime.Now;
            if (Cxtime.Day > 15)
                ViewBag.Month = Cxtime.AddMonths(1).Month;
            else
                ViewBag.Month = Cxtime.Month;
            index2model index2model = new index2model();
            //ViewBag.curtime = DateTime.Now.ToString();
            int UserId = (Session["User"] as EquipModel.Entities.Person_Info).Person_Id;
            

            ViewBag.curuser = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name.ToString();//获得当前用户的名字
            index2model.UserHasEquips = pm.Get_Person_Cj((Session["User"] as EquipModel.Entities.Person_Info).Person_Id);
            

            if (pv.Role_Names.Contains("可靠性工程师") || pv.Role_Names.Contains("检维修人员"))
                index2model.isSubmit = 1;
            else
                index2model.isSubmit = 0;
            if (ViewBag.Month == 1)
                ViewBag.Monthname = "一月（12月15至1月15）";
            if (ViewBag.Month == 2)
                ViewBag.Monthname = "二月（1月15至2月15）";
            if (ViewBag.Month == 3)
                ViewBag.Monthname = "三月（2月15至3月15）";
            if (ViewBag.Month == 4)
                ViewBag.Monthname = "四月（3月15至4月15）";
            if (ViewBag.Month == 5)
                ViewBag.Monthname = "五月（4月15至5月15）";
            if (ViewBag.Month == 6)
                ViewBag.Monthname = "六月（5月15至6月15）";
            if (ViewBag.Month == 7)
                ViewBag.Monthname = "七月（6月15至7月15）";
            if (ViewBag.Month == 8)
                ViewBag.Monthname = "八月（7月15至8月15）";
            if (ViewBag.Month == 9)
                ViewBag.Monthname = "九月（8月15至9月15）";
            if (ViewBag.Month == 10)
                ViewBag.Monthname = "十月（9月15至10月15）";
            if (ViewBag.Month == 11)
                ViewBag.Monthname = "十一月（10月15至11月15）";
            if (ViewBag.Month == 12)
                ViewBag.Monthname = "十二月（11月15至12月15）";
            return View(index2model);
        }
        public ActionResult yearreport(string time)
        {
            return View(getRecordforyear(time));
        }
        public class index2model
        {
            public int isSubmit;
            //  public string title;
            public List<Equip_Archi> UserHasEquips;
        }
        public class Index_ModelforA2
        {
            public List<A15dot1Tab> Am;
            public List<A15dot1Tab> Bm;
            public List<A15dot1Tab> Cm;
            public List<A15dot1Tab> Dm;
            public List<A15dot1Tab> Em;
            public List<A15dot1Tab> Fm;
            public List<A15dot1Tab> Gm;
            public List<A15dot1Tab> Hm;
            public List<A15dot1Tab> Qc;//全厂指标

        }
        public class submitmodel
        {
            public string equipReliableExponent;//装置可靠性指数
            public string maintainChargeExponent;//维修费用指数
            public string thousandLXBrate;//千台离心泵（机泵）密封消耗率
            public string thousandColdChangeRate;//千台冷换设备管束（含整台）更换率
            public string instrumentControlRate;//仪表实际控制率
            public string eventNumber;//事件数
            public string troubleKoufen;//故障强度扣分
            public string projectLateRate;//项目逾期率
            public string trainAndAbility;//培训及能力
            //public int kkxgcs;
            //public int jwxry;
            public List<Equip_Archi> UserHasEquips;
        }

        public Index_ModelforA2 getRecord(string time)
        {
            Index_ModelforA2 RecordforA2 = new Index_ModelforA2();
            //ViewBag.curtime = DateTime.Now.ToString();
            int UserId = (Session["User"] as EquipModel.Entities.Person_Info).Person_Id;
            DateTime Cxtime = DateTime.Now;
            List<A15dot1Tab> miss = new List<A15dot1Tab>();
            RecordforA2.Am = new List<A15dot1Tab>();
            RecordforA2.Bm = new List<A15dot1Tab>();
            RecordforA2.Cm = new List<A15dot1Tab>();
            RecordforA2.Dm = new List<A15dot1Tab>();
            RecordforA2.Em = new List<A15dot1Tab>();
            RecordforA2.Fm = new List<A15dot1Tab>();
            RecordforA2.Gm = new List<A15dot1Tab>();
            RecordforA2.Qc = new List<A15dot1Tab>();
            if (time == null)
            {
                miss = Jx.GetJxItemforA2(Cxtime, Cxtime.AddMonths(-1));
            }
            else
            {
                string[] strtime = time.Split(new char[] { '-', ' ' });
                string[] starttime = strtime[0].Split(new char[] { '/' });
                string[] endtime = strtime[3].Split(new char[] { '/' });
                string stime = starttime[2] + '/' + starttime[0] + '/' + starttime[1];
                string etime = endtime[2] + '/' + endtime[0] + '/' + endtime[1];
                miss = Jx.GetJxItemforA2(Convert.ToDateTime(etime), Convert.ToDateTime(stime));
            }
            foreach (var item in miss)
            {
                A15dot1Tab a = new A15dot1Tab();
                if (item.submitDepartment == "联合一片区")
                {
                    a.timesNonPlanStop = item.timesNonPlanStop;
                    a.scoreDeductFaultIntensity = item.scoreDeductFaultIntensity;
                    a.rateBigUnitFault = item.rateBigUnitFault;
                    a.rateFaultMaintenance = item.rateFaultMaintenance;
                    a.MTBF = item.MTBF;
                    a.rateEquipUse = item.rateEquipUse;
                    a.avgLifeSpanSeal = item.avgLifeSpanSeal;
                    a.avgLifeSpanAxle = item.avgLifeSpanAxle;
                    a.percentEquipAvailability = item.percentEquipAvailability;
                    a.percentPassOnetimeRepair = item.percentPassOnetimeRepair;

                    a.Count_timesNonPlanStop = item.Count_timesNonPlanStop;
                    a.Count_scoreDeductFaultIntensity = item.Count_scoreDeductFaultIntensity;
                    a.Count_rateBigUnitFault = item.Count_rateBigUnitFault;
                    a.Count_rateFaultMaintenance = item.Count_rateFaultMaintenance;
                    a.Count_MTBF = item.Count_MTBF;
                    a.Count_rateEquipUse = item.Count_rateEquipUse;
                    a.Count_avgLifeSpanSeal = item.Count_avgLifeSpanSeal;
                    a.Count_avgLifeSpanAxle = item.Count_avgLifeSpanAxle;
                    a.Count_percentEquipAvailability = item.Count_percentEquipAvailability;
                    a.Count_percentPassOnetimeRepair = item.Count_percentPassOnetimeRepair;
                    RecordforA2.Am.Add(a);
                }
                if (item.submitDepartment == "联合二片区")
                {
                    a.timesNonPlanStop = item.timesNonPlanStop;
                    a.scoreDeductFaultIntensity = item.scoreDeductFaultIntensity;
                    a.rateBigUnitFault = item.rateBigUnitFault;
                    a.rateFaultMaintenance = item.rateFaultMaintenance;
                    a.MTBF = item.MTBF;
                    a.rateEquipUse = item.rateEquipUse;
                    a.avgLifeSpanSeal = item.avgLifeSpanSeal;
                    a.avgLifeSpanAxle = item.avgLifeSpanAxle;
                    a.percentEquipAvailability = item.percentEquipAvailability;
                    a.percentPassOnetimeRepair = item.percentPassOnetimeRepair;
                    //参与统计的台数
                    a.Count_timesNonPlanStop = item.Count_timesNonPlanStop;
                    a.Count_scoreDeductFaultIntensity = item.Count_scoreDeductFaultIntensity;
                    a.Count_rateBigUnitFault = item.Count_rateBigUnitFault;
                    a.Count_rateFaultMaintenance = item.Count_rateFaultMaintenance;
                    a.Count_MTBF = item.Count_MTBF;
                    a.Count_rateEquipUse = item.Count_rateEquipUse;
                    a.Count_avgLifeSpanSeal = item.Count_avgLifeSpanSeal;
                    a.Count_avgLifeSpanAxle = item.Count_avgLifeSpanAxle;
                    a.Count_percentEquipAvailability = item.Count_percentEquipAvailability;
                    a.Count_percentPassOnetimeRepair = item.Count_percentPassOnetimeRepair;
                    RecordforA2.Bm.Add(a);
                }
                if (item.submitDepartment == "联合三片区")
                {
                    a.timesNonPlanStop = item.timesNonPlanStop;
                    a.scoreDeductFaultIntensity = item.scoreDeductFaultIntensity;
                    a.rateBigUnitFault = item.rateBigUnitFault;
                    a.rateFaultMaintenance = item.rateFaultMaintenance;
                    a.MTBF = item.MTBF;
                    a.rateEquipUse = item.rateEquipUse;
                    a.avgLifeSpanSeal = item.avgLifeSpanSeal;
                    a.avgLifeSpanAxle = item.avgLifeSpanAxle;
                    a.percentEquipAvailability = item.percentEquipAvailability;
                    a.percentPassOnetimeRepair = item.percentPassOnetimeRepair;

                    a.Count_timesNonPlanStop = item.Count_timesNonPlanStop;
                    a.Count_scoreDeductFaultIntensity = item.Count_scoreDeductFaultIntensity;
                    a.Count_rateBigUnitFault = item.Count_rateBigUnitFault;
                    a.Count_rateFaultMaintenance = item.Count_rateFaultMaintenance;
                    a.Count_MTBF = item.Count_MTBF;
                    a.Count_rateEquipUse = item.Count_rateEquipUse;
                    a.Count_avgLifeSpanSeal = item.Count_avgLifeSpanSeal;
                    a.Count_avgLifeSpanAxle = item.Count_avgLifeSpanAxle;
                    a.Count_percentEquipAvailability = item.Count_percentEquipAvailability;
                    a.Count_percentPassOnetimeRepair = item.Count_percentPassOnetimeRepair;
                    RecordforA2.Cm.Add(a);
                }
                if (item.submitDepartment == "联合四片区")
                {
                    a.timesNonPlanStop = item.timesNonPlanStop;
                    a.scoreDeductFaultIntensity = item.scoreDeductFaultIntensity;
                    a.rateBigUnitFault = item.rateBigUnitFault;
                    a.rateFaultMaintenance = item.rateFaultMaintenance;
                    a.MTBF = item.MTBF;
                    a.rateEquipUse = item.rateEquipUse;
                    a.avgLifeSpanSeal = item.avgLifeSpanSeal;
                    a.avgLifeSpanAxle = item.avgLifeSpanAxle;
                    a.percentEquipAvailability = item.percentEquipAvailability;
                    a.percentPassOnetimeRepair = item.percentPassOnetimeRepair;

                    a.Count_timesNonPlanStop = item.Count_timesNonPlanStop;
                    a.Count_scoreDeductFaultIntensity = item.Count_scoreDeductFaultIntensity;
                    a.Count_rateBigUnitFault = item.Count_rateBigUnitFault;
                    a.Count_rateFaultMaintenance = item.Count_rateFaultMaintenance;
                    a.Count_MTBF = item.Count_MTBF;
                    a.Count_rateEquipUse = item.Count_rateEquipUse;
                    a.Count_avgLifeSpanSeal = item.Count_avgLifeSpanSeal;
                    a.Count_avgLifeSpanAxle = item.Count_avgLifeSpanAxle;
                    a.Count_percentEquipAvailability = item.Count_percentEquipAvailability;
                    a.Count_percentPassOnetimeRepair = item.Count_percentPassOnetimeRepair;
                    RecordforA2.Dm.Add(a);
                }
                if (item.submitDepartment == "化工片区")
                {
                    a.timesNonPlanStop = item.timesNonPlanStop;
                    a.scoreDeductFaultIntensity = item.scoreDeductFaultIntensity;
                    a.rateBigUnitFault = item.rateBigUnitFault;
                    a.rateFaultMaintenance = item.rateFaultMaintenance;
                    a.MTBF = item.MTBF;
                    a.rateEquipUse = item.rateEquipUse;
                    a.avgLifeSpanSeal = item.avgLifeSpanSeal;
                    a.avgLifeSpanAxle = item.avgLifeSpanAxle;
                    a.percentEquipAvailability = item.percentEquipAvailability;
                    a.percentPassOnetimeRepair = item.percentPassOnetimeRepair;

                    a.Count_timesNonPlanStop = item.Count_timesNonPlanStop;
                    a.Count_scoreDeductFaultIntensity = item.Count_scoreDeductFaultIntensity;
                    a.Count_rateBigUnitFault = item.Count_rateBigUnitFault;
                    a.Count_rateFaultMaintenance = item.Count_rateFaultMaintenance;
                    a.Count_MTBF = item.Count_MTBF;
                    a.Count_rateEquipUse = item.Count_rateEquipUse;
                    a.Count_avgLifeSpanSeal = item.Count_avgLifeSpanSeal;
                    a.Count_avgLifeSpanAxle = item.Count_avgLifeSpanAxle;
                    a.Count_percentEquipAvailability = item.Count_percentEquipAvailability;
                    a.Count_percentPassOnetimeRepair = item.Count_percentPassOnetimeRepair;
                    RecordforA2.Em.Add(a);
                }
                if (item.submitDepartment == "综合片区")
                {
                    a.timesNonPlanStop = item.timesNonPlanStop;
                    a.scoreDeductFaultIntensity = item.scoreDeductFaultIntensity;
                    a.rateBigUnitFault = item.rateBigUnitFault;
                    a.rateFaultMaintenance = item.rateFaultMaintenance;
                    a.MTBF = item.MTBF;
                    a.rateEquipUse = item.rateEquipUse;
                    a.avgLifeSpanSeal = item.avgLifeSpanSeal;
                    a.avgLifeSpanAxle = item.avgLifeSpanAxle;
                    a.percentEquipAvailability = item.percentEquipAvailability;
                    a.percentPassOnetimeRepair = item.percentPassOnetimeRepair;

                    a.Count_timesNonPlanStop = item.Count_timesNonPlanStop;
                    a.Count_scoreDeductFaultIntensity = item.Count_scoreDeductFaultIntensity;
                    a.Count_rateBigUnitFault = item.Count_rateBigUnitFault;
                    a.Count_rateFaultMaintenance = item.Count_rateFaultMaintenance;
                    a.Count_MTBF = item.Count_MTBF;
                    a.Count_rateEquipUse = item.Count_rateEquipUse;
                    a.Count_avgLifeSpanSeal = item.Count_avgLifeSpanSeal;
                    a.Count_avgLifeSpanAxle = item.Count_avgLifeSpanAxle;
                    a.Count_percentEquipAvailability = item.Count_percentEquipAvailability;
                    a.Count_percentPassOnetimeRepair = item.Count_percentPassOnetimeRepair;
                    RecordforA2.Fm.Add(a);
                }
                if (item.submitDepartment == "系统片区")
                {
                    a.timesNonPlanStop = item.timesNonPlanStop;
                    a.scoreDeductFaultIntensity = item.scoreDeductFaultIntensity;
                    a.rateBigUnitFault = item.rateBigUnitFault;
                    a.rateFaultMaintenance = item.rateFaultMaintenance;
                    a.MTBF = item.MTBF;
                    a.rateEquipUse = item.rateEquipUse;
                    a.avgLifeSpanSeal = item.avgLifeSpanSeal;
                    a.avgLifeSpanAxle = item.avgLifeSpanAxle;
                    a.percentEquipAvailability = item.percentEquipAvailability;
                    a.percentPassOnetimeRepair = item.percentPassOnetimeRepair;

                    a.Count_timesNonPlanStop = item.Count_timesNonPlanStop;
                    a.Count_scoreDeductFaultIntensity = item.Count_scoreDeductFaultIntensity;
                    a.Count_rateBigUnitFault = item.Count_rateBigUnitFault;
                    a.Count_rateFaultMaintenance = item.Count_rateFaultMaintenance;
                    a.Count_MTBF = item.Count_MTBF;
                    a.Count_rateEquipUse = item.Count_rateEquipUse;
                    a.Count_avgLifeSpanSeal = item.Count_avgLifeSpanSeal;
                    a.Count_avgLifeSpanAxle = item.Count_avgLifeSpanAxle;
                    a.Count_percentEquipAvailability = item.Count_percentEquipAvailability;
                    a.Count_percentPassOnetimeRepair = item.Count_percentPassOnetimeRepair;
                    RecordforA2.Gm.Add(a);
                }

            }
            /***************************计算全厂KPI值****************************************************/
            A15dot1Tab QcKPI = new A15dot1Tab();
            if (RecordforA2.Am.Count != 0 && RecordforA2.Bm.Count != 0 && RecordforA2.Cm.Count != 0 && RecordforA2.Dm.Count != 0 && RecordforA2.Em.Count != 0 && RecordforA2.Fm.Count != 0 && RecordforA2.Gm.Count != 0)
            {//一个月一个片区只存一次因此为Am[0]
                QcKPI.timesNonPlanStop = RecordforA2.Am[0].timesNonPlanStop
                                        + RecordforA2.Bm[0].timesNonPlanStop
                                        + RecordforA2.Cm[0].timesNonPlanStop
                                        + RecordforA2.Dm[0].timesNonPlanStop
                                        + RecordforA2.Em[0].timesNonPlanStop
                                        + RecordforA2.Fm[0].timesNonPlanStop
                                        + RecordforA2.Gm[0].timesNonPlanStop;

                QcKPI.scoreDeductFaultIntensity = RecordforA2.Am[0].scoreDeductFaultIntensity
                                                 + RecordforA2.Bm[0].scoreDeductFaultIntensity
                                                 + RecordforA2.Cm[0].scoreDeductFaultIntensity
                                                 + RecordforA2.Dm[0].scoreDeductFaultIntensity
                                                 + RecordforA2.Em[0].scoreDeductFaultIntensity
                                                 + RecordforA2.Fm[0].scoreDeductFaultIntensity
                                                 + RecordforA2.Gm[0].scoreDeductFaultIntensity;

                QcKPI.rateBigUnitFault = RecordforA2.Am[0].rateBigUnitFault
                                       + RecordforA2.Bm[0].rateBigUnitFault
                                       + RecordforA2.Cm[0].rateBigUnitFault
                                       + RecordforA2.Dm[0].rateBigUnitFault
                                       + RecordforA2.Em[0].rateBigUnitFault
                                       + RecordforA2.Fm[0].rateBigUnitFault
                                       + RecordforA2.Gm[0].rateBigUnitFault;

                QcKPI.rateFaultMaintenance = (RecordforA2.Am[0].rateFaultMaintenance
                                            + RecordforA2.Bm[0].rateFaultMaintenance
                                            + RecordforA2.Cm[0].rateFaultMaintenance
                                            + RecordforA2.Dm[0].rateFaultMaintenance
                                            + RecordforA2.Em[0].rateFaultMaintenance
                                            + RecordforA2.Fm[0].rateFaultMaintenance
                                            + RecordforA2.Gm[0].rateFaultMaintenance)
                                           / (RecordforA2.Am[0].Count_rateFaultMaintenance
                                            + RecordforA2.Bm[0].Count_rateFaultMaintenance
                                            + RecordforA2.Cm[0].Count_rateFaultMaintenance
                                            + RecordforA2.Dm[0].Count_rateFaultMaintenance
                                            + RecordforA2.Em[0].Count_rateFaultMaintenance
                                            + RecordforA2.Fm[0].Count_rateFaultMaintenance
                                            + RecordforA2.Gm[0].Count_rateFaultMaintenance);

                QcKPI.MTBF = (RecordforA2.Am[0].Count_MTBF
                                       + RecordforA2.Bm[0].Count_MTBF
                                       + RecordforA2.Cm[0].Count_MTBF
                                       + RecordforA2.Dm[0].Count_MTBF
                                       + RecordforA2.Em[0].Count_MTBF
                                       + RecordforA2.Fm[0].Count_MTBF
                                       + RecordforA2.Gm[0].Count_MTBF) * 12 /
                                      (RecordforA2.Am[0].MTBF
                                       + RecordforA2.Bm[0].MTBF
                                       + RecordforA2.Cm[0].MTBF
                                       + RecordforA2.Dm[0].MTBF
                                       + RecordforA2.Em[0].MTBF
                                       + RecordforA2.Fm[0].MTBF
                                       + RecordforA2.Gm[0].MTBF);

                QcKPI.rateEquipUse = (RecordforA2.Am[0].rateEquipUse
                                           + RecordforA2.Bm[0].rateEquipUse
                                           + RecordforA2.Cm[0].rateEquipUse
                                           + RecordforA2.Dm[0].rateEquipUse
                                           + RecordforA2.Em[0].rateEquipUse
                                           + RecordforA2.Fm[0].rateEquipUse
                                           + RecordforA2.Gm[0].rateEquipUse)
                                       / (RecordforA2.Am[0].rateEquipUse
                                           + RecordforA2.Bm[0].Count_rateEquipUse
                                           + RecordforA2.Cm[0].Count_rateEquipUse
                                           + RecordforA2.Dm[0].Count_rateEquipUse
                                           + RecordforA2.Em[0].Count_rateEquipUse
                                           + RecordforA2.Fm[0].Count_rateEquipUse
                                           + RecordforA2.Gm[0].Count_rateEquipUse);

                QcKPI.avgLifeSpanSeal = (RecordforA2.Am[0].avgLifeSpanSeal
                                          + RecordforA2.Bm[0].avgLifeSpanSeal
                                          + RecordforA2.Cm[0].avgLifeSpanSeal
                                          + RecordforA2.Dm[0].avgLifeSpanSeal
                                          + RecordforA2.Em[0].avgLifeSpanSeal
                                          + RecordforA2.Fm[0].avgLifeSpanSeal
                                          + RecordforA2.Gm[0].avgLifeSpanSeal)
                                      / (RecordforA2.Am[0].Count_avgLifeSpanSeal
                                          + RecordforA2.Bm[0].Count_avgLifeSpanSeal
                                          + RecordforA2.Cm[0].Count_avgLifeSpanSeal
                                          + RecordforA2.Dm[0].Count_avgLifeSpanSeal
                                          + RecordforA2.Em[0].Count_avgLifeSpanSeal
                                          + RecordforA2.Fm[0].Count_avgLifeSpanSeal
                                          + RecordforA2.Gm[0].Count_avgLifeSpanSeal);

                QcKPI.avgLifeSpanAxle = (RecordforA2.Am[0].avgLifeSpanAxle
                                         + RecordforA2.Bm[0].avgLifeSpanAxle
                                         + RecordforA2.Cm[0].avgLifeSpanAxle
                                         + RecordforA2.Dm[0].avgLifeSpanAxle
                                         + RecordforA2.Em[0].avgLifeSpanAxle
                                         + RecordforA2.Fm[0].avgLifeSpanAxle
                                         + RecordforA2.Gm[0].avgLifeSpanAxle)
                                     / (RecordforA2.Am[0].Count_avgLifeSpanAxle
                                         + RecordforA2.Bm[0].Count_avgLifeSpanAxle
                                         + RecordforA2.Cm[0].Count_avgLifeSpanAxle
                                         + RecordforA2.Dm[0].Count_avgLifeSpanAxle
                                         + RecordforA2.Em[0].Count_avgLifeSpanAxle
                                         + RecordforA2.Fm[0].Count_avgLifeSpanAxle
                                         + RecordforA2.Gm[0].Count_avgLifeSpanAxle);

                QcKPI.percentEquipAvailability = (RecordforA2.Am[0].percentEquipAvailability
                                                    + RecordforA2.Bm[0].percentEquipAvailability
                                                    + RecordforA2.Cm[0].percentEquipAvailability
                                                    + RecordforA2.Dm[0].percentEquipAvailability
                                                    + RecordforA2.Em[0].percentEquipAvailability
                                                    + RecordforA2.Fm[0].percentEquipAvailability
                                                    + RecordforA2.Gm[0].percentEquipAvailability)
                                                / (RecordforA2.Am[0].Count_percentEquipAvailability
                                                    + RecordforA2.Bm[0].Count_percentEquipAvailability
                                                    + RecordforA2.Cm[0].Count_percentEquipAvailability
                                                    + RecordforA2.Dm[0].Count_percentEquipAvailability
                                                    + RecordforA2.Em[0].Count_percentEquipAvailability
                                                    + RecordforA2.Fm[0].Count_percentEquipAvailability
                                                    + RecordforA2.Gm[0].Count_percentEquipAvailability);

                QcKPI.percentPassOnetimeRepair = (RecordforA2.Am[0].percentPassOnetimeRepair
                                                   + RecordforA2.Bm[0].percentPassOnetimeRepair
                                                   + RecordforA2.Cm[0].percentPassOnetimeRepair
                                                   + RecordforA2.Dm[0].percentPassOnetimeRepair
                                                   + RecordforA2.Em[0].percentPassOnetimeRepair
                                                   + RecordforA2.Fm[0].percentPassOnetimeRepair
                                                   + RecordforA2.Gm[0].percentPassOnetimeRepair)
                                               / (RecordforA2.Am[0].Count_percentPassOnetimeRepair
                                                   + RecordforA2.Bm[0].Count_percentPassOnetimeRepair
                                                   + RecordforA2.Cm[0].Count_percentPassOnetimeRepair
                                                   + RecordforA2.Dm[0].Count_percentPassOnetimeRepair
                                                   + RecordforA2.Em[0].Count_percentPassOnetimeRepair
                                                   + RecordforA2.Fm[0].Count_percentPassOnetimeRepair
                                                   + RecordforA2.Gm[0].Count_percentPassOnetimeRepair);
                RecordforA2.Qc.Add(QcKPI);
            }
            return RecordforA2;
        }
        public Index_ModelforA2 getRecordforyear(string time)
        {
            Index_ModelforA2 RecordforA2 = new Index_ModelforA2();
            Index_ModelforA2 Record = new Index_ModelforA2();
            //ViewBag.curtime = DateTime.Now.ToString();
            int UserId = (Session["User"] as EquipModel.Entities.Person_Info).Person_Id;
            string Cxtime = Convert.ToString(DateTime.Now.Year) + "/01/01 00:00:00";
            string CxEndtime = Convert.ToString(DateTime.Now.Year + 1) + "/01/01 00:00:00";
            List<A15dot1Tab> miss = new List<A15dot1Tab>();
            RecordforA2.Am = new List<A15dot1Tab>();
            RecordforA2.Bm = new List<A15dot1Tab>();
            RecordforA2.Cm = new List<A15dot1Tab>();
            RecordforA2.Dm = new List<A15dot1Tab>();
            RecordforA2.Em = new List<A15dot1Tab>();
            RecordforA2.Fm = new List<A15dot1Tab>();
            RecordforA2.Gm = new List<A15dot1Tab>();
            RecordforA2.Hm = new List<A15dot1Tab>();

            Record.Am = new List<A15dot1Tab>();
            Record.Bm = new List<A15dot1Tab>();
            Record.Cm = new List<A15dot1Tab>();
            Record.Dm = new List<A15dot1Tab>();
            Record.Em = new List<A15dot1Tab>();
            Record.Fm = new List<A15dot1Tab>();
            Record.Gm = new List<A15dot1Tab>();
            Record.Hm = new List<A15dot1Tab>();
            if (time == null)
            {
                miss = Jx.GetJxItemforA2(Convert.ToDateTime(CxEndtime), Convert.ToDateTime(Cxtime));
            }
            else
            {
                string stime = time + "/01/01 00:00:00";
                string etime = Convert.ToInt32(time) + 1 + "/01/01 00:00:00";
                miss = Jx.GetJxItemforA2(Convert.ToDateTime(etime), Convert.ToDateTime(stime));
            }
            foreach (var item in miss)
            {
                A15dot1Tab a = new A15dot1Tab();
                if (item.submitDepartment == "联合一片区")
                {
                    a.timesNonPlanStop = item.timesNonPlanStop;
                    a.scoreDeductFaultIntensity = item.scoreDeductFaultIntensity;
                    a.rateBigUnitFault = item.rateBigUnitFault;
                    a.rateFaultMaintenance = item.rateFaultMaintenance;
                    a.MTBF = item.MTBF;
                    a.rateEquipUse = item.rateEquipUse;
                    a.avgLifeSpanSeal = item.avgLifeSpanSeal;
                    a.avgLifeSpanAxle = item.avgLifeSpanAxle;
                    a.percentEquipAvailability = item.percentEquipAvailability;
                    a.percentPassOnetimeRepair = item.percentPassOnetimeRepair;
                    RecordforA2.Am.Add(a);
                }
                if (item.submitDepartment == "联合二片区")
                {
                    a.timesNonPlanStop = item.timesNonPlanStop;
                    a.scoreDeductFaultIntensity = item.scoreDeductFaultIntensity;
                    a.rateBigUnitFault = item.rateBigUnitFault;
                    a.rateFaultMaintenance = item.rateFaultMaintenance;
                    a.MTBF = item.MTBF;
                    a.rateEquipUse = item.rateEquipUse;
                    a.avgLifeSpanSeal = item.avgLifeSpanSeal;
                    a.avgLifeSpanAxle = item.avgLifeSpanAxle;
                    a.percentEquipAvailability = item.percentEquipAvailability;
                    a.percentPassOnetimeRepair = item.percentPassOnetimeRepair;
                    RecordforA2.Bm.Add(a);
                }
                if (item.submitDepartment == "联合三片区")
                {
                    a.timesNonPlanStop = item.timesNonPlanStop;
                    a.scoreDeductFaultIntensity = item.scoreDeductFaultIntensity;
                    a.rateBigUnitFault = item.rateBigUnitFault;
                    a.rateFaultMaintenance = item.rateFaultMaintenance;
                    a.MTBF = item.MTBF;
                    a.rateEquipUse = item.rateEquipUse;
                    a.avgLifeSpanSeal = item.avgLifeSpanSeal;
                    a.avgLifeSpanAxle = item.avgLifeSpanAxle;
                    a.percentEquipAvailability = item.percentEquipAvailability;
                    a.percentPassOnetimeRepair = item.percentPassOnetimeRepair;
                    RecordforA2.Cm.Add(a);
                }
                if (item.submitDepartment == "焦化片区")
                {
                    a.timesNonPlanStop = item.timesNonPlanStop;
                    a.scoreDeductFaultIntensity = item.scoreDeductFaultIntensity;
                    a.rateBigUnitFault = item.rateBigUnitFault;
                    a.rateFaultMaintenance = item.rateFaultMaintenance;
                    a.MTBF = item.MTBF;
                    a.rateEquipUse = item.rateEquipUse;
                    a.avgLifeSpanSeal = item.avgLifeSpanSeal;
                    a.avgLifeSpanAxle = item.avgLifeSpanAxle;
                    a.percentEquipAvailability = item.percentEquipAvailability;
                    a.percentPassOnetimeRepair = item.percentPassOnetimeRepair;
                    RecordforA2.Dm.Add(a);
                }
                if (item.submitDepartment == "化工片区")
                {
                    a.timesNonPlanStop = item.timesNonPlanStop;
                    a.scoreDeductFaultIntensity = item.scoreDeductFaultIntensity;
                    a.rateBigUnitFault = item.rateBigUnitFault;
                    a.rateFaultMaintenance = item.rateFaultMaintenance;
                    a.MTBF = item.MTBF;
                    a.rateEquipUse = item.rateEquipUse;
                    a.avgLifeSpanSeal = item.avgLifeSpanSeal;
                    a.avgLifeSpanAxle = item.avgLifeSpanAxle;
                    a.percentEquipAvailability = item.percentEquipAvailability;
                    a.percentPassOnetimeRepair = item.percentPassOnetimeRepair;
                    RecordforA2.Em.Add(a);
                }
                if (item.submitDepartment == "综合片区")
                {
                    a.timesNonPlanStop = item.timesNonPlanStop;
                    a.scoreDeductFaultIntensity = item.scoreDeductFaultIntensity;
                    a.rateBigUnitFault = item.rateBigUnitFault;
                    a.rateFaultMaintenance = item.rateFaultMaintenance;
                    a.MTBF = item.MTBF;
                    a.rateEquipUse = item.rateEquipUse;
                    a.avgLifeSpanSeal = item.avgLifeSpanSeal;
                    a.avgLifeSpanAxle = item.avgLifeSpanAxle;
                    a.percentEquipAvailability = item.percentEquipAvailability;
                    a.percentPassOnetimeRepair = item.percentPassOnetimeRepair;
                    RecordforA2.Fm.Add(a);
                }
                if (item.submitDepartment == "系统片区")
                {
                    a.timesNonPlanStop = item.timesNonPlanStop;
                    a.scoreDeductFaultIntensity = item.scoreDeductFaultIntensity;
                    a.rateBigUnitFault = item.rateBigUnitFault;
                    a.rateFaultMaintenance = item.rateFaultMaintenance;
                    a.MTBF = item.MTBF;
                    a.rateEquipUse = item.rateEquipUse;
                    a.avgLifeSpanSeal = item.avgLifeSpanSeal;
                    a.avgLifeSpanAxle = item.avgLifeSpanAxle;
                    a.percentEquipAvailability = item.percentEquipAvailability;
                    a.percentPassOnetimeRepair = item.percentPassOnetimeRepair;
                    RecordforA2.Gm.Add(a);
                }
                if (item.submitDepartment == "检修单位")
                {
                    a.timesNonPlanStop = item.timesNonPlanStop;
                    a.scoreDeductFaultIntensity = item.scoreDeductFaultIntensity;
                    a.rateBigUnitFault = item.rateBigUnitFault;
                    a.rateFaultMaintenance = item.rateFaultMaintenance;
                    a.MTBF = item.MTBF;
                    a.rateEquipUse = item.rateEquipUse;
                    a.rateUrgentRepairWorkHour = item.rateUrgentRepairWorkHour;
                    a.hourWorkOrderFinish = item.hourWorkOrderFinish;
                    a.avgLifeSpanSeal = item.avgLifeSpanSeal;
                    a.avgLifeSpanAxle = item.avgLifeSpanAxle;
                    a.percentEquipAvailability = item.percentEquipAvailability;
                    a.percentPassOnetimeRepair = item.percentPassOnetimeRepair;
                    a.avgEfficiencyPump = item.avgEfficiencyPump;
                    a.avgEfficiencyUnit = item.avgEfficiencyUnit;
                    RecordforA2.Hm.Add(a);
                }

            }
            A15dot1Tab aa = new A15dot1Tab();
            foreach (var ruo in RecordforA2.Am)//联合一
            {
                aa.timesNonPlanStop += ruo.timesNonPlanStop;
                aa.scoreDeductFaultIntensity += ruo.scoreDeductFaultIntensity;
                aa.rateBigUnitFault += ruo.rateBigUnitFault;
                aa.rateFaultMaintenance += ruo.rateFaultMaintenance;//jun
                aa.MTBF += ruo.MTBF;//jun
                aa.rateEquipUse += ruo.rateEquipUse;//jun
                aa.avgLifeSpanSeal = ruo.avgLifeSpanSeal;
                aa.avgLifeSpanAxle = ruo.avgLifeSpanAxle;
                aa.percentEquipAvailability += ruo.percentEquipAvailability;//jun
                aa.percentPassOnetimeRepair += ruo.percentPassOnetimeRepair;//jun
            }
            aa.rateFaultMaintenance = aa.rateFaultMaintenance / 12;
            aa.MTBF = aa.MTBF / 12;
            aa.rateEquipUse = aa.rateEquipUse / 12;
            aa.percentEquipAvailability = aa.percentEquipAvailability / 12;
            aa.percentPassOnetimeRepair = aa.percentPassOnetimeRepair / 12;
            Record.Am.Add(aa);
            A15dot1Tab bb = new A15dot1Tab();
            foreach (var ruo in RecordforA2.Bm)//联合二
            {
                bb.timesNonPlanStop += ruo.timesNonPlanStop;
                bb.scoreDeductFaultIntensity += ruo.scoreDeductFaultIntensity;
                bb.rateBigUnitFault += ruo.rateBigUnitFault;
                bb.rateFaultMaintenance += ruo.rateFaultMaintenance;//jun
                bb.MTBF += ruo.MTBF;//jun
                bb.rateEquipUse += ruo.rateEquipUse;//jun
                bb.avgLifeSpanSeal = ruo.avgLifeSpanSeal;
                bb.avgLifeSpanAxle = ruo.avgLifeSpanAxle;
                bb.percentEquipAvailability += ruo.percentEquipAvailability;//jun
                bb.percentPassOnetimeRepair += ruo.percentPassOnetimeRepair;//jun
            }
            bb.rateFaultMaintenance = bb.rateFaultMaintenance / 12;
            bb.MTBF = bb.MTBF / 12;
            bb.rateEquipUse = bb.rateEquipUse / 12;
            bb.percentEquipAvailability = bb.percentEquipAvailability / 12;
            bb.percentPassOnetimeRepair = bb.percentPassOnetimeRepair / 12;
            Record.Bm.Add(bb);
            A15dot1Tab cc = new A15dot1Tab();
            foreach (var ruo in RecordforA2.Cm)//联合三
            {
                cc.timesNonPlanStop += ruo.timesNonPlanStop;
                cc.scoreDeductFaultIntensity += ruo.scoreDeductFaultIntensity;
                cc.rateBigUnitFault += ruo.rateBigUnitFault;
                cc.rateFaultMaintenance += ruo.rateFaultMaintenance;//jun
                cc.MTBF += ruo.MTBF;//jun
                cc.rateEquipUse += ruo.rateEquipUse;//jun
                cc.avgLifeSpanSeal = ruo.avgLifeSpanSeal;
                cc.avgLifeSpanAxle = ruo.avgLifeSpanAxle;
                cc.percentEquipAvailability += ruo.percentEquipAvailability;//jun
                cc.percentPassOnetimeRepair += ruo.percentPassOnetimeRepair;//jun
            }
            cc.rateFaultMaintenance = cc.rateFaultMaintenance / 12;
            cc.MTBF = cc.MTBF / 12;
            cc.rateEquipUse = cc.rateEquipUse / 12;
            cc.percentEquipAvailability = cc.percentEquipAvailability / 12;
            cc.percentPassOnetimeRepair = cc.percentPassOnetimeRepair / 12;
            Record.Cm.Add(cc);
            A15dot1Tab dd = new A15dot1Tab();
            foreach (var ruo in RecordforA2.Dm)//焦化
            {
                dd.timesNonPlanStop += ruo.timesNonPlanStop;
                dd.scoreDeductFaultIntensity += ruo.scoreDeductFaultIntensity;
                dd.rateBigUnitFault += ruo.rateBigUnitFault;
                dd.rateFaultMaintenance += ruo.rateFaultMaintenance;//jun
                dd.MTBF += ruo.MTBF;//jun
                dd.rateEquipUse += ruo.rateEquipUse;//jun
                dd.avgLifeSpanSeal = ruo.avgLifeSpanSeal;
                dd.avgLifeSpanAxle = ruo.avgLifeSpanAxle;
                dd.percentEquipAvailability += ruo.percentEquipAvailability;//jun
                dd.percentPassOnetimeRepair += ruo.percentPassOnetimeRepair;//jun
            }
            dd.rateFaultMaintenance = dd.rateFaultMaintenance / 12;
            dd.MTBF = dd.MTBF / 12;
            dd.rateEquipUse = dd.rateEquipUse / 12;
            dd.percentEquipAvailability = dd.percentEquipAvailability / 12;
            dd.percentPassOnetimeRepair = dd.percentPassOnetimeRepair / 12;
            Record.Dm.Add(dd);
            A15dot1Tab ee = new A15dot1Tab();
            foreach (var ruo in RecordforA2.Em)//化工
            {
                ee.timesNonPlanStop += ruo.timesNonPlanStop;
                ee.scoreDeductFaultIntensity += ruo.scoreDeductFaultIntensity;
                ee.rateBigUnitFault += ruo.rateBigUnitFault;
                ee.rateFaultMaintenance += ruo.rateFaultMaintenance;//jun
                ee.MTBF += ruo.MTBF;//jun
                ee.rateEquipUse += ruo.rateEquipUse;//jun
                ee.avgLifeSpanSeal = ruo.avgLifeSpanSeal;
                ee.avgLifeSpanAxle = ruo.avgLifeSpanAxle;
                ee.percentEquipAvailability += ruo.percentEquipAvailability;//jun
                ee.percentPassOnetimeRepair += ruo.percentPassOnetimeRepair;//jun
            }
            ee.rateFaultMaintenance = ee.rateFaultMaintenance / 12;
            ee.MTBF = ee.MTBF / 12;
            ee.rateEquipUse = ee.rateEquipUse / 12;
            ee.percentEquipAvailability = ee.percentEquipAvailability / 12;
            ee.percentPassOnetimeRepair = ee.percentPassOnetimeRepair / 12;
            Record.Em.Add(ee);
            A15dot1Tab ff = new A15dot1Tab();
            foreach (var ruo in RecordforA2.Fm)//综合
            {
                ff.timesNonPlanStop += ruo.timesNonPlanStop;
                ff.scoreDeductFaultIntensity += ruo.scoreDeductFaultIntensity;
                ff.rateBigUnitFault += ruo.rateBigUnitFault;
                ff.rateFaultMaintenance += ruo.rateFaultMaintenance;//jun
                ff.MTBF += ruo.MTBF;//jun
                ff.rateEquipUse += ruo.rateEquipUse;//jun
                ff.avgLifeSpanSeal = ruo.avgLifeSpanSeal;
                ff.avgLifeSpanAxle = ruo.avgLifeSpanAxle;
                ff.percentEquipAvailability += ruo.percentEquipAvailability;//jun
                ff.percentPassOnetimeRepair += ruo.percentPassOnetimeRepair;//jun
            }
            ff.rateFaultMaintenance = ff.rateFaultMaintenance / 12;
            ff.MTBF = ff.MTBF / 12;
            ff.rateEquipUse = ff.rateEquipUse / 12;
            ff.percentEquipAvailability = ff.percentEquipAvailability / 12;
            ff.percentPassOnetimeRepair = ff.percentPassOnetimeRepair / 12;
            Record.Fm.Add(ff);
            A15dot1Tab gg = new A15dot1Tab();
            foreach (var ruo in RecordforA2.Gm)//系统
            {
                gg.timesNonPlanStop += ruo.timesNonPlanStop;
                gg.scoreDeductFaultIntensity += ruo.scoreDeductFaultIntensity;
                gg.rateBigUnitFault += ruo.rateBigUnitFault;
                gg.rateFaultMaintenance += ruo.rateFaultMaintenance;//jun
                gg.MTBF += ruo.MTBF;//jun
                gg.rateEquipUse += ruo.rateEquipUse;//jun
                gg.avgLifeSpanSeal = ruo.avgLifeSpanSeal;
                gg.avgLifeSpanAxle = ruo.avgLifeSpanAxle;
                gg.percentEquipAvailability += ruo.percentEquipAvailability;//jun
                gg.percentPassOnetimeRepair += ruo.percentPassOnetimeRepair;//jun
            }
            gg.rateFaultMaintenance = gg.rateFaultMaintenance / 12;
            gg.MTBF = gg.MTBF / 12;
            gg.rateEquipUse = gg.rateEquipUse / 12;
            gg.percentEquipAvailability = gg.percentEquipAvailability / 12;
            gg.percentPassOnetimeRepair = gg.percentPassOnetimeRepair / 12;
            Record.Gm.Add(gg);
            A15dot1Tab hh = new A15dot1Tab();
            foreach (var ruo in RecordforA2.Hm)//检修单位
            {
                hh.timesNonPlanStop += ruo.timesNonPlanStop;
                hh.scoreDeductFaultIntensity += ruo.scoreDeductFaultIntensity;
                hh.rateBigUnitFault += ruo.rateBigUnitFault;
                hh.rateFaultMaintenance += ruo.rateFaultMaintenance;//jun
                hh.MTBF += ruo.MTBF;//jun
                hh.rateEquipUse += ruo.rateEquipUse;//jun
                hh.rateUrgentRepairWorkHour += ruo.rateUrgentRepairWorkHour;
                hh.hourWorkOrderFinish += hh.hourWorkOrderFinish;//jun
                hh.avgLifeSpanSeal = ruo.avgLifeSpanSeal;
                hh.avgLifeSpanAxle = ruo.avgLifeSpanAxle;
                hh.percentEquipAvailability += ruo.percentEquipAvailability;//jun
                hh.percentPassOnetimeRepair += ruo.percentPassOnetimeRepair;//jun
                hh.avgEfficiencyPump = ruo.avgEfficiencyPump;
                hh.avgEfficiencyUnit = ruo.avgEfficiencyUnit;
            }
            hh.rateFaultMaintenance = hh.rateFaultMaintenance / 12;
            hh.MTBF = hh.MTBF / 12;
            hh.rateEquipUse = hh.rateEquipUse / 12;
            hh.hourWorkOrderFinish = hh.hourWorkOrderFinish / 12;
            hh.percentEquipAvailability = hh.percentEquipAvailability / 12;
            hh.percentPassOnetimeRepair = hh.percentPassOnetimeRepair / 12;
            Record.Hm.Add(hh);
            return Record;
        }
        public string OutputExcel(string time)
        {
            DateTime Cxtime = DateTime.Now;
            List<A15dot1Tab> miss = new List<A15dot1Tab>();
            Index_ModelforA2 RecordforA2 = new Index_ModelforA2();
            RecordforA2.Am = new List<A15dot1Tab>();
            RecordforA2.Bm = new List<A15dot1Tab>();
            RecordforA2.Cm = new List<A15dot1Tab>();
            RecordforA2.Dm = new List<A15dot1Tab>();
            RecordforA2.Em = new List<A15dot1Tab>();
            RecordforA2.Fm = new List<A15dot1Tab>();
            RecordforA2.Gm = new List<A15dot1Tab>();
            if (time == "")
            {
                miss = Jx.GetJxItemforA2(Cxtime, Cxtime.AddMonths(-1));
            }
            else
            {
                string[] strtime = time.Split(new char[] { '-', ' ' });
                string[] starttime = strtime[0].Split(new char[] { '/' });
                string[] endtime = strtime[3].Split(new char[] { '/' });
                string stime = starttime[2] + '/' + starttime[0] + '/' + starttime[1];
                string etime = endtime[2] + '/' + endtime[0] + '/' + endtime[1];
                miss = Jx.GetJxItemforA2(Convert.ToDateTime(etime), Convert.ToDateTime(stime));
            }
            foreach (var item in miss)
            {
                A15dot1Tab a = new A15dot1Tab();
                if (item.submitDepartment == "联合一片区")
                {
                    a.timesNonPlanStop = item.timesNonPlanStop;
                    a.scoreDeductFaultIntensity = item.scoreDeductFaultIntensity;
                    a.rateBigUnitFault = item.rateBigUnitFault;
                    a.rateFaultMaintenance = item.rateFaultMaintenance;
                    a.MTBF = item.MTBF;
                    a.rateEquipUse = item.rateEquipUse;
                    a.avgLifeSpanSeal = item.avgLifeSpanSeal;
                    a.avgLifeSpanAxle = item.avgLifeSpanAxle;
                    a.percentEquipAvailability = item.percentEquipAvailability;
                    a.percentPassOnetimeRepair = item.percentPassOnetimeRepair;
                    RecordforA2.Am.Add(a);
                }
                if (item.submitDepartment == "联合二片区")
                {
                    a.timesNonPlanStop = item.timesNonPlanStop;
                    a.scoreDeductFaultIntensity = item.scoreDeductFaultIntensity;
                    a.rateBigUnitFault = item.rateBigUnitFault;
                    a.rateFaultMaintenance = item.rateFaultMaintenance;
                    a.MTBF = item.MTBF;
                    a.rateEquipUse = item.rateEquipUse;
                    a.avgLifeSpanSeal = item.avgLifeSpanSeal;
                    a.avgLifeSpanAxle = item.avgLifeSpanAxle;
                    a.percentEquipAvailability = item.percentEquipAvailability;
                    a.percentPassOnetimeRepair = item.percentPassOnetimeRepair;
                    RecordforA2.Bm.Add(a);
                }
                if (item.submitDepartment == "联合三片区")
                {
                    a.timesNonPlanStop = item.timesNonPlanStop;
                    a.scoreDeductFaultIntensity = item.scoreDeductFaultIntensity;
                    a.rateBigUnitFault = item.rateBigUnitFault;
                    a.rateFaultMaintenance = item.rateFaultMaintenance;
                    a.MTBF = item.MTBF;
                    a.rateEquipUse = item.rateEquipUse;
                    a.avgLifeSpanSeal = item.avgLifeSpanSeal;
                    a.avgLifeSpanAxle = item.avgLifeSpanAxle;
                    a.percentEquipAvailability = item.percentEquipAvailability;
                    a.percentPassOnetimeRepair = item.percentPassOnetimeRepair;
                    RecordforA2.Cm.Add(a);
                }
                if (item.submitDepartment == "焦化片区")
                {
                    a.timesNonPlanStop = item.timesNonPlanStop;
                    a.scoreDeductFaultIntensity = item.scoreDeductFaultIntensity;
                    a.rateBigUnitFault = item.rateBigUnitFault;
                    a.rateFaultMaintenance = item.rateFaultMaintenance;
                    a.MTBF = item.MTBF;
                    a.rateEquipUse = item.rateEquipUse;
                    a.avgLifeSpanSeal = item.avgLifeSpanSeal;
                    a.avgLifeSpanAxle = item.avgLifeSpanAxle;
                    a.percentEquipAvailability = item.percentEquipAvailability;
                    a.percentPassOnetimeRepair = item.percentPassOnetimeRepair;
                    RecordforA2.Dm.Add(a);
                }
                if (item.submitDepartment == "化工片区")
                {
                    a.timesNonPlanStop = item.timesNonPlanStop;
                    a.scoreDeductFaultIntensity = item.scoreDeductFaultIntensity;
                    a.rateBigUnitFault = item.rateBigUnitFault;
                    a.rateFaultMaintenance = item.rateFaultMaintenance;
                    a.MTBF = item.MTBF;
                    a.rateEquipUse = item.rateEquipUse;
                    a.avgLifeSpanSeal = item.avgLifeSpanSeal;
                    a.avgLifeSpanAxle = item.avgLifeSpanAxle;
                    a.percentEquipAvailability = item.percentEquipAvailability;
                    a.percentPassOnetimeRepair = item.percentPassOnetimeRepair;
                    RecordforA2.Em.Add(a);
                }
                if (item.submitDepartment == "综合片区")
                {
                    a.timesNonPlanStop = item.timesNonPlanStop;
                    a.scoreDeductFaultIntensity = item.scoreDeductFaultIntensity;
                    a.rateBigUnitFault = item.rateBigUnitFault;
                    a.rateFaultMaintenance = item.rateFaultMaintenance;
                    a.MTBF = item.MTBF;
                    a.rateEquipUse = item.rateEquipUse;
                    a.avgLifeSpanSeal = item.avgLifeSpanSeal;
                    a.avgLifeSpanAxle = item.avgLifeSpanAxle;
                    a.percentEquipAvailability = item.percentEquipAvailability;
                    a.percentPassOnetimeRepair = item.percentPassOnetimeRepair;
                    RecordforA2.Fm.Add(a);
                }
                if (item.submitDepartment == "系统片区")
                {
                    a.timesNonPlanStop = item.timesNonPlanStop;
                    a.scoreDeductFaultIntensity = item.scoreDeductFaultIntensity;
                    a.rateBigUnitFault = item.rateBigUnitFault;
                    a.rateFaultMaintenance = item.rateFaultMaintenance;
                    a.MTBF = item.MTBF;
                    a.rateEquipUse = item.rateEquipUse;
                    a.avgLifeSpanSeal = item.avgLifeSpanSeal;
                    a.avgLifeSpanAxle = item.avgLifeSpanAxle;
                    a.percentEquipAvailability = item.percentEquipAvailability;
                    a.percentPassOnetimeRepair = item.percentPassOnetimeRepair;
                    RecordforA2.Gm.Add(a);
                }
            }

            try
            {
                // 创建Excel 文档

                HSSFWorkbook wk = new HSSFWorkbook();

                ISheet tb = wk.CreateSheet("sheet1");

                //设置单元的宽度  
                tb.SetColumnWidth(0, 15 * 256);
                tb.SetColumnWidth(1, 25 * 256);
                tb.SetColumnWidth(2, 25 * 256);
                tb.SetColumnWidth(3, 11 * 256);
                tb.SetColumnWidth(4, 15 * 256);
                tb.SetColumnWidth(5, 15 * 256);
                tb.SetColumnWidth(6, 15 * 256);
                tb.SetColumnWidth(7, 15 * 256);
                tb.SetColumnWidth(8, 15 * 256);
                tb.SetColumnWidth(9, 15 * 256);
                tb.SetColumnWidth(10, 15 * 256);




                //合并标题头，该方法的参数次序是：开始行号，结束行号，开始列号，结束列号。
                tb.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 0, 0, 10));

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
                                          = cellStyle_head;


                head_first_cell.SetCellValue("各片区KPI指标统计表");
                IRow table_head = tb.CreateRow(1);

                ICellStyle cellStyle_normal = wk.CreateCellStyle();

                cellStyle_normal.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                cellStyle_normal.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                cellStyle_normal.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                cellStyle_normal.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;

                ICell table_head_zhibiaoleixing = table_head.CreateCell(0);
                table_head_zhibiaoleixing.CellStyle = cellStyle_normal;
                table_head_zhibiaoleixing.SetCellValue("指标类型");

                ICell table_head_KPIzhibiao = table_head.CreateCell(1);
                table_head_KPIzhibiao.CellStyle = cellStyle_normal;
                table_head_KPIzhibiao.SetCellValue("KPI指标");

                ICell table_head_zhibiaoshuoming = table_head.CreateCell(2);
                table_head_zhibiaoshuoming.CellStyle = cellStyle_normal;
                table_head_zhibiaoshuoming.SetCellValue("指标说明");

                ICell table_head_mubiaozhi = table_head.CreateCell(3);
                table_head_mubiaozhi.CellStyle = cellStyle_normal;
                table_head_mubiaozhi.SetCellValue("目标值");

                ICell table_head_lianheyi = table_head.CreateCell(4);
                table_head_lianheyi.CellStyle = cellStyle_normal;
                table_head_lianheyi.SetCellValue("联合一片区");

                ICell table_head_lianheer = table_head.CreateCell(5);
                table_head_lianheer.CellStyle = cellStyle_normal;
                table_head_lianheer.SetCellValue("联合二片区");

                ICell table_head_lianhesan = table_head.CreateCell(6);
                table_head_lianhesan.CellStyle = cellStyle_normal;
                table_head_lianhesan.SetCellValue("联合三片区");

                ICell table_head_jiaohua = table_head.CreateCell(7);
                table_head_jiaohua.CellStyle = cellStyle_normal;
                table_head_jiaohua.SetCellValue("焦化片区");

                ICell table_head_huagong = table_head.CreateCell(8);
                table_head_huagong.CellStyle = cellStyle_normal;
                table_head_huagong.SetCellValue("化工片区");

                ICell table_head_zonghe = table_head.CreateCell(9);
                table_head_zonghe.CellStyle = cellStyle_normal;
                table_head_zonghe.SetCellValue("综合片区");

                ICell table_head_xitong = table_head.CreateCell(10);
                table_head_xitong.CellStyle = cellStyle_normal;
                table_head_xitong.SetCellValue("系统片区");


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


                IRow table_row1 = tb.CreateRow(2);
                ICell table_row1_zhibiaoleixing = table_row1.CreateCell(0);
                table_row1_zhibiaoleixing.CellStyle = thjl_record_cellStyle;
                table_row1_zhibiaoleixing.SetCellValue("可靠性指标");

                ICell table_row1_KPIzhibiao = table_row1.CreateCell(1);
                table_row1_KPIzhibiao.CellStyle = thjl_record_cellStyle;
                table_row1_KPIzhibiao.SetCellValue("非计划停工次数");

                ICell table_row1_zhibiaoshuoming = table_row1.CreateCell(2);
                table_row1_zhibiaoshuoming.CellStyle = thjl_record_cellStyle;
                table_row1_zhibiaoshuoming.SetCellValue("由设备机械原因导致生产装置非计划停工");

                ICell table_row1_KPImubiaozhi = table_row1.CreateCell(3);
                table_row1_KPImubiaozhi.CellStyle = thjl_record_cellStyle;
                table_row1_KPImubiaozhi.SetCellValue("0次");

                ICell table_row1_KPIlianheyi = table_row1.CreateCell(4);
                table_row1_KPIlianheyi.CellStyle = cellStyle_normal;
                if (RecordforA2.Am.Count != 0)
                {
                    table_row1_KPIlianheyi.SetCellValue(RecordforA2.Am[0].timesNonPlanStop);
                }

                ICell table_row1_KPIlianheer = table_row1.CreateCell(5);
                table_row1_KPIlianheer.CellStyle = cellStyle_normal;
                if (RecordforA2.Bm.Count != 0)
                {
                    table_row1_KPIlianheer.SetCellValue(RecordforA2.Bm[0].timesNonPlanStop);
                }

                ICell table_row1_KPIlianhesan = table_row1.CreateCell(6);
                table_row1_KPIlianhesan.CellStyle = cellStyle_normal;
                if (RecordforA2.Cm.Count != 0)
                {
                    table_row1_KPIlianhesan.SetCellValue(RecordforA2.Cm[0].timesNonPlanStop);
                }
                ICell table_row1_KPIjiaohua = table_row1.CreateCell(7);
                table_row1_KPIjiaohua.CellStyle = cellStyle_normal;
                if (RecordforA2.Dm.Count != 0)
                {
                    table_row1_KPIjiaohua.SetCellValue(RecordforA2.Dm[0].timesNonPlanStop);
                }
                ICell table_row1_KPIhuagong = table_row1.CreateCell(8);
                table_row1_KPIhuagong.CellStyle = cellStyle_normal;
                if (RecordforA2.Em.Count != 0)
                {
                    table_row1_KPIhuagong.SetCellValue(RecordforA2.Em[0].timesNonPlanStop);
                }
                ICell table_row1_KPIzonghe = table_row1.CreateCell(9);
                table_row1_KPIzonghe.CellStyle = cellStyle_normal;
                if (RecordforA2.Fm.Count != 0)
                {
                    table_row1_KPIzonghe.SetCellValue(RecordforA2.Fm[0].timesNonPlanStop);
                }

                ICell table_row1_KPIxitong = table_row1.CreateCell(10);
                table_row1_KPIxitong.CellStyle = cellStyle_normal;
                if (RecordforA2.Gm.Count != 0)
                {
                    table_row1_KPIxitong.SetCellValue(RecordforA2.Gm[0].timesNonPlanStop);
                }

                IRow table_row2 = tb.CreateRow(3);
                ICell table_row2_zhibiaoleixing = table_row2.CreateCell(0);
                table_row2_zhibiaoleixing.CellStyle = cellStyle_normal;
                table_row2_zhibiaoleixing.SetCellValue("");

                ICell table_row2_KPIzhibiao = table_row2.CreateCell(1);
                table_row2_KPIzhibiao.CellStyle = thjl_record_cellStyle;
                table_row2_KPIzhibiao.SetCellValue("四类以上故障强度扣分");

                ICell table_row2_zhibiaoshuoming = table_row2.CreateCell(2);
                table_row2_zhibiaoshuoming.CellStyle = thjl_record_cellStyle;
                table_row2_zhibiaoshuoming.SetCellValue("由设备机械原因导致发生四类以上强度故障扣分");

                ICell table_row2_KPImubiaozhi = table_row2.CreateCell(3);
                table_row2_KPImubiaozhi.CellStyle = thjl_record_cellStyle;
                table_row2_KPImubiaozhi.SetCellValue("≤60分/年");

                ICell table_row2_KPIlianheyi = table_row2.CreateCell(4);
                table_row2_KPIlianheyi.CellStyle = cellStyle_normal;
                if (RecordforA2.Am.Count != 0)
                {
                    table_row2_KPIlianheyi.SetCellValue(RecordforA2.Am[0].scoreDeductFaultIntensity);
                }
                ICell table_row2_KPIlianheer = table_row2.CreateCell(5);
                table_row2_KPIlianheer.CellStyle = cellStyle_normal;
                if (RecordforA2.Bm.Count != 0)
                {
                    table_row2_KPIlianheer.SetCellValue(RecordforA2.Bm[0].scoreDeductFaultIntensity);
                }
                ICell table_row2_KPIlianhesan = table_row2.CreateCell(6);
                table_row2_KPIlianhesan.CellStyle = cellStyle_normal;
                if (RecordforA2.Cm.Count != 0)
                {
                    table_row2_KPIlianhesan.SetCellValue(RecordforA2.Cm[0].scoreDeductFaultIntensity);
                }
                ICell table_row2_KPIjiaohua = table_row2.CreateCell(7);
                table_row2_KPIjiaohua.CellStyle = cellStyle_normal;
                if (RecordforA2.Dm.Count != 0)
                {
                    table_row2_KPIjiaohua.SetCellValue(RecordforA2.Dm[0].scoreDeductFaultIntensity);
                }
                ICell table_row2_KPIhuagong = table_row2.CreateCell(8);
                table_row2_KPIhuagong.CellStyle = cellStyle_normal;
                if (RecordforA2.Em.Count != 0)
                {
                    table_row2_KPIhuagong.SetCellValue(RecordforA2.Em[0].scoreDeductFaultIntensity);
                }
                ICell table_row2_KPIzonghe = table_row2.CreateCell(9);
                table_row2_KPIzonghe.CellStyle = cellStyle_normal;
                if (RecordforA2.Fm.Count != 0)
                {
                    table_row2_KPIzonghe.SetCellValue(RecordforA2.Fm[0].scoreDeductFaultIntensity);
                }

                ICell table_row2_KPIxitong = table_row2.CreateCell(10);
                table_row2_KPIxitong.CellStyle = cellStyle_normal;
                if (RecordforA2.Gm.Count != 0)
                {
                    table_row2_KPIxitong.SetCellValue(RecordforA2.Gm[0].scoreDeductFaultIntensity);
                }

                IRow table_row3 = tb.CreateRow(4);
                ICell table_row3_zhibiaoleixing = table_row3.CreateCell(0);
                table_row3_zhibiaoleixing.CellStyle = cellStyle_normal;
                table_row3_zhibiaoleixing.SetCellValue("");

                ICell table_row3_KPIzhibiao = table_row3.CreateCell(1);
                table_row3_KPIzhibiao.CellStyle = thjl_record_cellStyle;
                table_row3_KPIzhibiao.SetCellValue("大机组故障率K");

                ICell table_row3_zhibiaoshuoming = table_row3.CreateCell(2);
                table_row3_zhibiaoshuoming.CellStyle = thjl_record_cellStyle;
                table_row3_zhibiaoshuoming.SetCellValue("K=∑(所有考核大机组故障时间)/∑(所有考核大机组计划投用时间)");

                ICell table_row3_KPImubiaozhi = table_row3.CreateCell(3);
                table_row3_KPImubiaozhi.CellStyle = thjl_record_cellStyle;
                table_row3_KPImubiaozhi.SetCellValue("<1‰");

                ICell table_row3_KPIlianheyi = table_row3.CreateCell(4);
                table_row3_KPIlianheyi.CellStyle = cellStyle_normal;
                if (RecordforA2.Am.Count != 0)
                {
                    table_row3_KPIlianheyi.SetCellValue(RecordforA2.Am[0].rateBigUnitFault);
                }
                ICell table_row3_KPIlianheer = table_row3.CreateCell(5);
                table_row3_KPIlianheer.CellStyle = cellStyle_normal;
                if (RecordforA2.Bm.Count != 0)
                {
                    table_row3_KPIlianheer.SetCellValue(RecordforA2.Bm[0].rateBigUnitFault);
                }
                ICell table_row3_KPIlianhesan = table_row3.CreateCell(6);
                table_row3_KPIlianhesan.CellStyle = cellStyle_normal;
                if (RecordforA2.Cm.Count != 0)
                {
                    table_row3_KPIlianhesan.SetCellValue(RecordforA2.Cm[0].rateBigUnitFault);
                }
                ICell table_row3_KPIjiaohua = table_row3.CreateCell(7);
                table_row3_KPIjiaohua.CellStyle = cellStyle_normal;
                if (RecordforA2.Dm.Count != 0)
                {
                    table_row3_KPIjiaohua.SetCellValue(RecordforA2.Dm[0].rateBigUnitFault);
                }
                ICell table_row3_KPIhuagong = table_row3.CreateCell(8);
                table_row3_KPIhuagong.CellStyle = cellStyle_normal;
                if (RecordforA2.Em.Count != 0)
                {
                    table_row3_KPIhuagong.SetCellValue(RecordforA2.Em[0].rateBigUnitFault);
                }
                ICell table_row3_KPIzonghe = table_row3.CreateCell(9);
                table_row3_KPIzonghe.CellStyle = cellStyle_normal;
                if (RecordforA2.Fm.Count != 0)
                {
                    table_row3_KPIzonghe.SetCellValue(RecordforA2.Fm[0].rateBigUnitFault);
                }

                ICell table_row3_KPIxitong = table_row3.CreateCell(10);
                table_row3_KPIxitong.CellStyle = cellStyle_normal;
                if (RecordforA2.Gm.Count != 0)
                {
                    table_row3_KPIxitong.SetCellValue(RecordforA2.Gm[0].rateBigUnitFault);
                }

                IRow table_row4 = tb.CreateRow(5);
                ICell table_row4_zhibiaoleixing = table_row4.CreateCell(0);
                table_row4_zhibiaoleixing.CellStyle = cellStyle_normal;
                table_row4_zhibiaoleixing.SetCellValue("");

                ICell table_row4_KPIzhibiao = table_row4.CreateCell(1);
                table_row4_KPIzhibiao.CellStyle = thjl_record_cellStyle;
                table_row4_KPIzhibiao.SetCellValue("故障维修率F");

                ICell table_row4_zhibiaoshuoming = table_row4.CreateCell(2);
                table_row4_zhibiaoshuoming.CellStyle = thjl_record_cellStyle;
                table_row4_zhibiaoshuoming.SetCellValue("F=故障维修次数/维修总次数");

                ICell table_row4_KPImubiaozhi = table_row4.CreateCell(3);
                table_row4_KPImubiaozhi.CellStyle = thjl_record_cellStyle;
                table_row4_KPImubiaozhi.SetCellValue("<7.5%");

                ICell table_row4_KPIlianheyi = table_row4.CreateCell(4);
                table_row4_KPIlianheyi.CellStyle = cellStyle_normal;
                if (RecordforA2.Am.Count != 0)
                {
                    table_row4_KPIlianheyi.SetCellValue(RecordforA2.Am[0].rateFaultMaintenance);
                }
                ICell table_row4_KPIlianheer = table_row4.CreateCell(5);
                table_row4_KPIlianheer.CellStyle = cellStyle_normal;
                if (RecordforA2.Bm.Count != 0)
                {
                    table_row4_KPIlianheer.SetCellValue(RecordforA2.Bm[0].rateFaultMaintenance);
                }
                ICell table_row4_KPIlianhesan = table_row4.CreateCell(6);
                table_row4_KPIlianhesan.CellStyle = cellStyle_normal;
                if (RecordforA2.Cm.Count != 0)
                {
                    table_row4_KPIlianhesan.SetCellValue(RecordforA2.Cm[0].rateFaultMaintenance);
                }
                ICell table_row4_KPIjiaohua = table_row4.CreateCell(7);
                table_row4_KPIjiaohua.CellStyle = cellStyle_normal;
                if (RecordforA2.Dm.Count != 0)
                {
                    table_row4_KPIjiaohua.SetCellValue(RecordforA2.Dm[0].rateFaultMaintenance);
                }
                ICell table_row4_KPIhuagong = table_row4.CreateCell(8);
                table_row4_KPIhuagong.CellStyle = cellStyle_normal;
                if (RecordforA2.Em.Count != 0)
                {
                    table_row4_KPIhuagong.SetCellValue(RecordforA2.Em[0].rateFaultMaintenance);
                }
                ICell table_row4_KPIzonghe = table_row4.CreateCell(9);
                table_row4_KPIzonghe.CellStyle = cellStyle_normal;
                if (RecordforA2.Fm.Count != 0)
                {
                    table_row4_KPIzonghe.SetCellValue(RecordforA2.Fm[0].rateFaultMaintenance);
                }

                ICell table_row4_KPIxitong = table_row4.CreateCell(10);
                table_row4_KPIxitong.CellStyle = cellStyle_normal;
                if (RecordforA2.Gm.Count != 0)
                {
                    table_row4_KPIxitong.SetCellValue(RecordforA2.Gm[0].rateFaultMaintenance);
                }

                IRow table_row5 = tb.CreateRow(6);
                ICell table_row5_zhibiaoleixing = table_row5.CreateCell(0);
                table_row5_zhibiaoleixing.CellStyle = cellStyle_normal;
                table_row5_zhibiaoleixing.SetCellValue("");

                ICell table_row5_KPIzhibiao = table_row5.CreateCell(1);
                table_row5_KPIzhibiao.CellStyle = thjl_record_cellStyle;
                table_row5_KPIzhibiao.SetCellValue("一般机泵设备平均无故障间隔期MTBF");

                ICell table_row5_zhibiaoshuoming = table_row5.CreateCell(2);
                table_row5_zhibiaoshuoming.CellStyle = thjl_record_cellStyle;
                table_row5_zhibiaoshuoming.SetCellValue("MTBF=考核期内机泵设备总数×12/设备故障维修总数");

                ICell table_row5_KPImubiaozhi = table_row5.CreateCell(3);
                table_row5_KPImubiaozhi.CellStyle = thjl_record_cellStyle;
                table_row5_KPImubiaozhi.SetCellValue(">68月");

                ICell table_row5_KPIlianheyi = table_row5.CreateCell(4);
                table_row5_KPIlianheyi.CellStyle = cellStyle_normal;
                if (RecordforA2.Am.Count != 0)
                {
                    table_row5_KPIlianheyi.SetCellValue(RecordforA2.Am[0].MTBF);
                }
                ICell table_row5_KPIlianheer = table_row5.CreateCell(5);
                table_row5_KPIlianheer.CellStyle = cellStyle_normal;
                if (RecordforA2.Bm.Count != 0)
                {
                    table_row5_KPIlianheer.SetCellValue(RecordforA2.Bm[0].MTBF);
                }
                ICell table_row5_KPIlianhesan = table_row5.CreateCell(6);
                table_row5_KPIlianhesan.CellStyle = cellStyle_normal;
                if (RecordforA2.Cm.Count != 0)
                {
                    table_row5_KPIlianhesan.SetCellValue(RecordforA2.Cm[0].MTBF);
                }
                ICell table_row5_KPIjiaohua = table_row5.CreateCell(7);
                table_row5_KPIjiaohua.CellStyle = cellStyle_normal;
                if (RecordforA2.Dm.Count != 0)
                {
                    table_row5_KPIjiaohua.SetCellValue(RecordforA2.Dm[0].MTBF);
                }
                ICell table_row5_KPIhuagong = table_row5.CreateCell(8);
                table_row5_KPIhuagong.CellStyle = cellStyle_normal;
                if (RecordforA2.Em.Count != 0)
                {
                    table_row5_KPIhuagong.SetCellValue(RecordforA2.Em[0].MTBF);
                }
                ICell table_row5_KPIzonghe = table_row5.CreateCell(9);
                table_row5_KPIzonghe.CellStyle = cellStyle_normal;
                if (RecordforA2.Fm.Count != 0)
                {
                    table_row5_KPIzonghe.SetCellValue(RecordforA2.Fm[0].MTBF);
                }

                ICell table_row5_KPIxitong = table_row5.CreateCell(10);
                table_row5_KPIxitong.CellStyle = cellStyle_normal;
                if (RecordforA2.Gm.Count != 0)
                {
                    table_row5_KPIxitong.SetCellValue(RecordforA2.Gm[0].MTBF);
                }

                IRow table_row6 = tb.CreateRow(7);
                ICell table_row6_zhibiaoleixing = table_row6.CreateCell(0);
                table_row6_zhibiaoleixing.CellStyle = cellStyle_normal;
                table_row6_zhibiaoleixing.SetCellValue("");

                ICell table_row6_KPIzhibiao = table_row6.CreateCell(1);
                table_row6_KPIzhibiao.CellStyle = thjl_record_cellStyle;
                table_row6_KPIzhibiao.SetCellValue("设备投用率R（反映MTTR)");

                ICell table_row6_zhibiaoshuoming = table_row6.CreateCell(2);
                table_row6_zhibiaoshuoming.CellStyle = thjl_record_cellStyle;
                table_row6_zhibiaoshuoming.SetCellValue("R=1-（∑(所有设备维修时间)/∑(所有设备计划投用时间)");

                ICell table_row6_KPImubiaozhi = table_row6.CreateCell(3);
                table_row6_KPImubiaozhi.CellStyle = thjl_record_cellStyle;
                table_row6_KPImubiaozhi.SetCellValue(">99.5%");

                ICell table_row6_KPIlianheyi = table_row6.CreateCell(4);
                table_row6_KPIlianheyi.CellStyle = cellStyle_normal;
                if (RecordforA2.Am.Count != 0)
                {
                    table_row6_KPIlianheyi.SetCellValue(RecordforA2.Am[0].rateEquipUse);
                }
                ICell table_row6_KPIlianheer = table_row6.CreateCell(5);
                table_row6_KPIlianheer.CellStyle = cellStyle_normal;
                if (RecordforA2.Bm.Count != 0)
                {
                    table_row6_KPIlianheer.SetCellValue(RecordforA2.Bm[0].rateEquipUse);
                }
                ICell table_row6_KPIlianhesan = table_row6.CreateCell(6);
                table_row6_KPIlianhesan.CellStyle = cellStyle_normal;
                if (RecordforA2.Cm.Count != 0)
                {
                    table_row6_KPIlianhesan.SetCellValue(RecordforA2.Cm[0].rateEquipUse);
                }
                ICell table_row6_KPIjiaohua = table_row6.CreateCell(7);
                table_row6_KPIjiaohua.CellStyle = cellStyle_normal;
                if (RecordforA2.Dm.Count != 0)
                {
                    table_row6_KPIjiaohua.SetCellValue(RecordforA2.Dm[0].rateEquipUse);
                }
                ICell table_row6_KPIhuagong = table_row6.CreateCell(8);
                table_row6_KPIhuagong.CellStyle = cellStyle_normal;
                if (RecordforA2.Em.Count != 0)
                {
                    table_row6_KPIhuagong.SetCellValue(RecordforA2.Em[0].rateEquipUse);
                }
                ICell table_row6_KPIzonghe = table_row6.CreateCell(9);
                table_row6_KPIzonghe.CellStyle = cellStyle_normal;
                if (RecordforA2.Fm.Count != 0)
                {
                    table_row6_KPIzonghe.SetCellValue(RecordforA2.Fm[0].rateEquipUse);
                }

                ICell table_row6_KPIxitong = table_row6.CreateCell(10);
                table_row6_KPIxitong.CellStyle = cellStyle_normal;
                if (RecordforA2.Gm.Count != 0)
                {
                    table_row6_KPIxitong.SetCellValue(RecordforA2.Gm[0].rateEquipUse);
                }
                IRow table_row7 = tb.CreateRow(8);
                ICell table_row7_zhibiaoleixing = table_row7.CreateCell(0);
                table_row7_zhibiaoleixing.CellStyle = thjl_record_cellStyle;
                table_row7_zhibiaoleixing.SetCellValue("专业指标");

                ICell table_row7_KPIzhibiao = table_row7.CreateCell(1);
                table_row7_KPIzhibiao.CellStyle = thjl_record_cellStyle;
                table_row7_KPIzhibiao.SetCellValue("机械密封平均寿命s");

                ICell table_row7_zhibiaoshuoming = table_row7.CreateCell(2);
                table_row7_zhibiaoshuoming.CellStyle = thjl_record_cellStyle;
                table_row7_zhibiaoshuoming.SetCellValue("S=∑各密封点寿命/动密封总数");

                ICell table_row7_KPImubiaozhi = table_row7.CreateCell(3);
                table_row7_KPImubiaozhi.CellStyle = thjl_record_cellStyle;
                table_row7_KPImubiaozhi.SetCellValue("≥20000小时");

                ICell table_row7_KPIlianheyi = table_row7.CreateCell(4);
                table_row7_KPIlianheyi.CellStyle = cellStyle_normal;
                if (RecordforA2.Am.Count != 0)
                {
                    table_row7_KPIlianheyi.SetCellValue(RecordforA2.Am[0].avgLifeSpanSeal);
                }
                ICell table_row7_KPIlianheer = table_row7.CreateCell(5);
                table_row7_KPIlianheer.CellStyle = cellStyle_normal;
                if (RecordforA2.Bm.Count != 0)
                {
                    table_row7_KPIlianheer.SetCellValue(RecordforA2.Bm[0].avgLifeSpanSeal);
                }
                ICell table_row7_KPIlianhesan = table_row7.CreateCell(6);
                table_row7_KPIlianhesan.CellStyle = cellStyle_normal;
                if (RecordforA2.Cm.Count != 0)
                {
                    table_row7_KPIlianhesan.SetCellValue(RecordforA2.Cm[0].avgLifeSpanSeal);
                }
                ICell table_row7_KPIjiaohua = table_row7.CreateCell(7);
                table_row7_KPIjiaohua.CellStyle = cellStyle_normal;
                if (RecordforA2.Dm.Count != 0)
                {
                    table_row7_KPIjiaohua.SetCellValue(RecordforA2.Dm[0].avgLifeSpanSeal);
                }
                ICell table_row7_KPIhuagong = table_row7.CreateCell(8);
                table_row7_KPIhuagong.CellStyle = cellStyle_normal;
                if (RecordforA2.Em.Count != 0)
                {
                    table_row7_KPIhuagong.SetCellValue(RecordforA2.Em[0].avgLifeSpanSeal);
                }
                ICell table_row7_KPIzonghe = table_row7.CreateCell(9);
                table_row7_KPIzonghe.CellStyle = cellStyle_normal;
                if (RecordforA2.Fm.Count != 0)
                {
                    table_row7_KPIzonghe.SetCellValue(RecordforA2.Fm[0].avgLifeSpanSeal);
                }

                ICell table_row7_KPIxitong = table_row7.CreateCell(10);
                table_row7_KPIxitong.CellStyle = cellStyle_normal;
                if (RecordforA2.Gm.Count != 0)
                {
                    table_row7_KPIxitong.SetCellValue(RecordforA2.Gm[0].avgLifeSpanSeal);
                }

                IRow table_row8 = tb.CreateRow(9);
                ICell table_row8_zhibiaoleixing = table_row8.CreateCell(0);
                table_row8_zhibiaoleixing.CellStyle = cellStyle_normal;
                table_row8_zhibiaoleixing.SetCellValue("");

                ICell table_row8_KPIzhibiao = table_row8.CreateCell(1);
                table_row8_KPIzhibiao.CellStyle = thjl_record_cellStyle;
                table_row8_KPIzhibiao.SetCellValue("轴承平均寿命B");

                ICell table_row8_zhibiaoshuoming = table_row8.CreateCell(2);
                table_row8_zhibiaoshuoming.CellStyle = thjl_record_cellStyle;
                table_row8_zhibiaoshuoming.SetCellValue("B=∑各轴承寿命/轴承总数");

                ICell table_row8_KPImubiaozhi = table_row8.CreateCell(3);
                table_row8_KPImubiaozhi.CellStyle = thjl_record_cellStyle;
                table_row8_KPImubiaozhi.SetCellValue("≥280000小时");

                ICell table_row8_KPIlianheyi = table_row8.CreateCell(4);
                table_row8_KPIlianheyi.CellStyle = cellStyle_normal;
                if (RecordforA2.Am.Count != 0)
                {
                    table_row8_KPIlianheyi.SetCellValue(RecordforA2.Am[0].avgLifeSpanAxle);
                }
                ICell table_row8_KPIlianheer = table_row8.CreateCell(5);
                table_row8_KPIlianheer.CellStyle = cellStyle_normal;
                if (RecordforA2.Bm.Count != 0)
                {
                    table_row8_KPIlianheer.SetCellValue(RecordforA2.Bm[0].avgLifeSpanAxle);
                }
                ICell table_row8_KPIlianhesan = table_row8.CreateCell(6);
                table_row8_KPIlianhesan.CellStyle = cellStyle_normal;
                if (RecordforA2.Cm.Count != 0)
                {
                    table_row8_KPIlianhesan.SetCellValue(RecordforA2.Cm[0].avgLifeSpanAxle);
                }
                ICell table_row8_KPIjiaohua = table_row8.CreateCell(7);
                table_row8_KPIjiaohua.CellStyle = cellStyle_normal;
                if (RecordforA2.Dm.Count != 0)
                {
                    table_row8_KPIjiaohua.SetCellValue(RecordforA2.Dm[0].avgLifeSpanAxle);
                }
                ICell table_row8_KPIhuagong = table_row8.CreateCell(8);
                table_row8_KPIhuagong.CellStyle = cellStyle_normal;
                if (RecordforA2.Em.Count != 0)
                {
                    table_row8_KPIhuagong.SetCellValue(RecordforA2.Em[0].avgLifeSpanAxle);
                }
                ICell table_row8_KPIzonghe = table_row8.CreateCell(9);
                table_row8_KPIzonghe.CellStyle = cellStyle_normal;
                if (RecordforA2.Fm.Count != 0)
                {
                    table_row8_KPIzonghe.SetCellValue(RecordforA2.Fm[0].avgLifeSpanAxle);
                }

                ICell table_row8_KPIxitong = table_row8.CreateCell(10);
                table_row8_KPIxitong.CellStyle = cellStyle_normal;
                if (RecordforA2.Gm.Count != 0)
                {
                    table_row8_KPIxitong.SetCellValue(RecordforA2.Gm[0].avgLifeSpanAxle);
                }

                IRow table_row9 = tb.CreateRow(10);
                ICell table_row9_zhibiaoleixing = table_row9.CreateCell(0);
                table_row9_zhibiaoleixing.CellStyle = cellStyle_normal;
                table_row9_zhibiaoleixing.SetCellValue("");

                ICell table_row9_KPIzhibiao = table_row9.CreateCell(1);
                table_row9_KPIzhibiao.CellStyle = thjl_record_cellStyle;
                table_row9_KPIzhibiao.SetCellValue("设备完好率W");

                ICell table_row9_zhibiaoshuoming = table_row9.CreateCell(2);
                table_row9_zhibiaoshuoming.CellStyle = thjl_record_cellStyle;
                table_row9_zhibiaoshuoming.SetCellValue("W=∑完好设备台数/总设备台数");

                ICell table_row9_KPImubiaozhi = table_row9.CreateCell(3);
                table_row9_KPImubiaozhi.CellStyle = thjl_record_cellStyle;
                table_row9_KPImubiaozhi.SetCellValue("≥98%");

                ICell table_row9_KPIlianheyi = table_row9.CreateCell(4);
                table_row9_KPIlianheyi.CellStyle = cellStyle_normal;
                if (RecordforA2.Am.Count != 0)
                {
                    table_row9_KPIlianheyi.SetCellValue(RecordforA2.Am[0].percentEquipAvailability);
                }
                ICell table_row9_KPIlianheer = table_row9.CreateCell(5);
                table_row9_KPIlianheer.CellStyle = cellStyle_normal;
                if (RecordforA2.Bm.Count != 0)
                {
                    table_row9_KPIlianheer.SetCellValue(RecordforA2.Bm[0].percentEquipAvailability);
                }
                ICell table_row9_KPIlianhesan = table_row9.CreateCell(6);
                table_row9_KPIlianhesan.CellStyle = cellStyle_normal;
                if (RecordforA2.Cm.Count != 0)
                {
                    table_row9_KPIlianhesan.SetCellValue(RecordforA2.Cm[0].percentEquipAvailability);
                }
                ICell table_row9_KPIjiaohua = table_row9.CreateCell(7);
                table_row9_KPIjiaohua.CellStyle = cellStyle_normal;
                if (RecordforA2.Dm.Count != 0)
                {
                    table_row9_KPIjiaohua.SetCellValue(RecordforA2.Dm[0].percentEquipAvailability);
                }
                ICell table_row9_KPIhuagong = table_row9.CreateCell(8);
                table_row9_KPIhuagong.CellStyle = cellStyle_normal;
                if (RecordforA2.Em.Count != 0)
                {
                    table_row9_KPIhuagong.SetCellValue(RecordforA2.Em[0].percentEquipAvailability);
                }
                ICell table_row9_KPIzonghe = table_row9.CreateCell(9);
                table_row9_KPIzonghe.CellStyle = cellStyle_normal;
                if (RecordforA2.Fm.Count != 0)
                {
                    table_row9_KPIzonghe.SetCellValue(RecordforA2.Fm[0].percentEquipAvailability);
                }

                ICell table_row9_KPIxitong = table_row9.CreateCell(10);
                table_row9_KPIxitong.CellStyle = cellStyle_normal;
                if (RecordforA2.Gm.Count != 0)
                {
                    table_row9_KPIxitong.SetCellValue(RecordforA2.Gm[0].percentEquipAvailability);
                }

                IRow table_row10 = tb.CreateRow(11);
                ICell table_row10_zhibiaoleixing = table_row10.CreateCell(0);
                table_row10_zhibiaoleixing.CellStyle = cellStyle_normal;
                table_row10_zhibiaoleixing.SetCellValue("");

                ICell table_row10_KPIzhibiao = table_row10.CreateCell(1);
                table_row10_KPIzhibiao.CellStyle = thjl_record_cellStyle;
                table_row10_KPIzhibiao.SetCellValue("检修一次合格率");

                ICell table_row10_zhibiaoshuoming = table_row10.CreateCell(2);
                table_row10_zhibiaoshuoming.CellStyle = thjl_record_cellStyle;
                table_row10_zhibiaoshuoming.SetCellValue("∑检修一次合格台数/检修设备总台数");

                ICell table_row10_KPImubiaozhi = table_row10.CreateCell(3);
                table_row10_KPImubiaozhi.CellStyle = thjl_record_cellStyle;
                table_row10_KPImubiaozhi.SetCellValue("≥98%");

                ICell table_row10_KPIlianheyi = table_row10.CreateCell(4);
                table_row10_KPIlianheyi.CellStyle = cellStyle_normal;
                if (RecordforA2.Am.Count != 0)
                {
                    table_row10_KPIlianheyi.SetCellValue(RecordforA2.Am[0].percentPassOnetimeRepair);
                }
                ICell table_row10_KPIlianheer = table_row10.CreateCell(5);
                table_row10_KPIlianheer.CellStyle = cellStyle_normal;
                if (RecordforA2.Bm.Count != 0)
                {
                    table_row10_KPIlianheer.SetCellValue(RecordforA2.Bm[0].percentPassOnetimeRepair);
                }
                ICell table_row10_KPIlianhesan = table_row10.CreateCell(6);
                table_row10_KPIlianhesan.CellStyle = cellStyle_normal;
                if (RecordforA2.Cm.Count != 0)
                {
                    table_row10_KPIlianhesan.SetCellValue(RecordforA2.Cm[0].percentPassOnetimeRepair);
                }
                ICell table_row10_KPIjiaohua = table_row10.CreateCell(7);
                table_row10_KPIjiaohua.CellStyle = cellStyle_normal;
                if (RecordforA2.Dm.Count != 0)
                {
                    table_row10_KPIjiaohua.SetCellValue(RecordforA2.Dm[0].percentPassOnetimeRepair);
                }
                ICell table_row10_KPIhuagong = table_row10.CreateCell(8);
                table_row10_KPIhuagong.CellStyle = cellStyle_normal;
                if (RecordforA2.Em.Count != 0)
                {
                    table_row10_KPIhuagong.SetCellValue(RecordforA2.Em[0].percentPassOnetimeRepair);
                }
                ICell table_row10_KPIzonghe = table_row10.CreateCell(9);
                table_row10_KPIzonghe.CellStyle = cellStyle_normal;
                if (RecordforA2.Fm.Count != 0)
                {
                    table_row10_KPIzonghe.SetCellValue(RecordforA2.Fm[0].percentPassOnetimeRepair);
                }

                ICell table_row10_KPIxitong = table_row10.CreateCell(10);
                table_row10_KPIxitong.CellStyle = cellStyle_normal;
                if (RecordforA2.Gm.Count != 0)
                {
                    table_row10_KPIxitong.SetCellValue(RecordforA2.Gm[0].percentPassOnetimeRepair);
                }
                string path = Server.MapPath("~/Upload//各片区KPI指标统计表.xls");
                using (FileStream fs = System.IO.File.OpenWrite(path)) //打开一个xls文件，如果没有则自行创建，如果存在myxls.xls文件则在创建是不要打开该文件！
                {
                    wk.Write(fs);   //向打开的这个xls文件中写入mySheet表并保存。
                    Console.WriteLine("提示：创建成功！");
                }
                return Path.Combine("/Upload", "各片区KPI指标统计表.xls");

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return "";
            }


        }
        //新月度KPI选取片区及月份

        public string newgetRecord(string time, string zzid, string zy,string cjname,string chaxunmethod)
        {
            
            List<object> r = new List<object>();
            if (zy == "企业级")
            {
             

                r = getObjectQiYe(time, zzid,zy,cjname,chaxunmethod);
            }
            else if (zy == "电气专业")
            {
                r = getObjectDian(time, zzid, zy);
            }
            else if (zy == "仪表专业")
            {
                r = getObjectYi(time, zzid, zy);
            }
            else if (zy == "动设备专业")
            {
                r = getObjectDong(time, zzid, zy);
            }
            else if (zy == "静设备专业")
            {
                r = getObjectJing(time, zzid, zy);
            }
            string str = JsonConvert.SerializeObject(r);
            return ("{" + "\"data\": " + str + "}");
                 
        }

        //public string newgetRecord(string time, string zzid, string zy)
        //{
        //    List<object> r = new List<object>();
        //    EquipManagment em = new EquipManagment();
        //    string zzname = "";
        //    if (zzid == "全厂")
        //    {
        //        zzname = "全厂";
        //    }
        //    else
        //    {
        //        zzname = em.getEa_namebyid(Convert.ToInt32(zzid));
        //    }

        //    if (zzname != "全厂")
        //    {
        //        List<A15dot1TabQiYe> res = Jx1.GetJxByA2(time, zzname, zy);
        //        if (res.Count != 0)
        //        {
        //            object a1 = new
        //            {
        //                zblx = "KPI",
        //                zbname = "装置可靠性指标",
        //                ndmbz = "",
        //                ljz = "",

        //                ActualValue = res[0].zzkkxzs,

        //            };
        //            r.Add(a1);

        //            object a2 = new
        //            {
        //                zblx = "KPI",
        //                zbname = "维修费用指数",
        //                ndmbz = "",
        //                ljz = "",
        //                ActualValue = res[0].wxfyzs,
        //            };
        //            r.Add(a2);
        //            object a3 = new
        //            {
        //                zblx = "KPI",
        //                zbname = "千台离心泵（机泵）密封消耗率",
        //                ndmbz = "",
        //                ljz = "",
        //                ActualValue = res[0].qtlxbmfxhl,
        //            };
        //            r.Add(a3);
        //            object a9 = new
        //            {
        //                zblx = "KPI",
        //                zbname = "千台冷换设备管束（含整台）更换率",
        //                ndmbz = "",
        //                ljz = "",
        //                ActualValue = res[0].qtlhsbgsghl,
        //            };
        //            r.Add(a9);
        //            object a4 = new
        //            {
        //                zblx = "KPI",
        //                zbname = "仪表实际控制率",
        //                ndmbz = "",
        //                ljz = "",
        //                ActualValue = res[0].ybsjkzl,
        //            };
        //            r.Add(a4);
        //            object a5 = new
        //            {
        //                zblx = "KPI",
        //                zbname = "事件数",
        //                ndmbz = "",
        //                ljz = "",
        //                ActualValue = res[0].sjs,
        //            };
        //            r.Add(a5);
        //            object a6 = new
        //            {
        //                zblx = "KPI",
        //                zbname = "故障强度扣分",
        //                ndmbz = "",
        //                ljz = "",
        //                ActualValue = res[0].gzqdkf,
        //            };
        //            r.Add(a6);
        //            object a7 = new
        //            {
        //                zblx = "KPI",
        //                zbname = "项目逾期率",
        //                ndmbz = "",
        //                ljz = "",
        //                ActualValue = res[0].xmyql,
        //            };
        //            r.Add(a7);
        //            object a8 = new
        //            {
        //                zblx = "KPI",
        //                zbname = "培训及能力",
        //                ndmbz = "",
        //                ljz = "",
        //                ActualValue = res[0].pxjnl,
        //            };
        //            r.Add(a8);



        //            string str = JsonConvert.SerializeObject(r);
        //            return ("{" + "\"data\": " + str + "}");
        //        }
        //        else
        //        {

        //            object a1 = new
        //            {
        //                zblx = "KPI",
        //                zbname = "装置可靠性指标",
        //                ndmbz = "",
        //                ljz = "",

        //                ActualValue = "",

        //            };
        //            r.Add(a1);

        //            object a2 = new
        //            {
        //                zblx = "KPI",
        //                zbname = "维修费用指数",
        //                ndmbz = "",
        //                ljz = "",
        //                ActualValue = "",
        //            };
        //            r.Add(a2);
        //            object a3 = new
        //            {
        //                zblx = "KPI",
        //                zbname = "千台离心泵（机泵）密封消耗率",
        //                ndmbz = "",
        //                ljz = "",
        //                ActualValue = "",
        //            };
        //            r.Add(a3);
        //            object a9 = new
        //            {
        //                zblx = "KPI",
        //                zbname = "千台冷换设备管束（含整台）更换率",
        //                ndmbz = "",
        //                ljz = "",
        //                ActualValue = "",
        //            };
        //            r.Add(a9);
        //            object a4 = new
        //            {
        //                zblx = "KPI",
        //                zbname = "仪表实际控制率",
        //                ndmbz = "",
        //                ljz = "",
        //                ActualValue = "",
        //            };
        //            r.Add(a4);
        //            object a5 = new
        //            {
        //                zblx = "KPI",
        //                zbname = "事件数",
        //                ndmbz = "",
        //                ljz = "",
        //                ActualValue = "",
        //            };
        //            r.Add(a5);
        //            object a6 = new
        //            {
        //                zblx = "KPI",
        //                zbname = "故障强度扣分",
        //                ndmbz = "",
        //                ljz = "",
        //                ActualValue = "",
        //            };
        //            r.Add(a6);
        //            object a7 = new
        //            {
        //                zblx = "KPI",
        //                zbname = "项目逾期率",
        //                ndmbz = "",
        //                ljz = "",
        //                ActualValue = "",
        //            };
        //            r.Add(a7);
        //            object a8 = new
        //            {
        //                zblx = "KPI",
        //                zbname = "培训及能力",
        //                ndmbz = "",
        //                ljz = "",
        //                ActualValue = "",
        //            };
        //            r.Add(a8);
        //            string str = JsonConvert.SerializeObject(r);
        //            return ("{" + "\"data\": " + str + "}");
        //        }
        //    }
        //    else
        //    {
        //        return ("{" + "\"data\": }");
        //    }

        //}
        public List<object> getObjectQiYe(string time, string zzid, string zy,string cjname,string chaxunmethod)
        {
            
            PersonManagment pm = new PersonManagment();
            EquipBLL.AdminManagment.PersonManagment.P_viewModal pv = pm.Get_PersonModal((Session["User"] as EquipModel.Entities.Person_Info).Person_Id);
            string rolename = pv.Role_Names;//获得角色名
            string username = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;//获得用户名
            List<object> r = new List<object>();
            EquipManagment em = new EquipManagment();
           
            if(chaxunmethod=="全厂"){
               zzid="全厂";//如果查询方式是全厂
            }else if(chaxunmethod=="装置")
            {
                   zzid = em.getEa_namebyid(Convert.ToInt32(zzid));//如果查询方式是装置将zzid转换为装置的名字
            }
            DateTime Cxtime = DateTime.Now;
            string stime;
            string etime;//设置查询月份的格式，eg：1月15日0：00：00到2月14日23：59：59查询的是2月的数据
            if (Convert.ToInt32(time) > 1)
            {
                string Reducetime = (Convert.ToInt32(time) -1).ToString();
                etime = Cxtime.Year.ToString() + '/' + time + '/' + "14" + " 23:59";
                stime = Cxtime.Year.ToString() + '/' + Reducetime + '/' + "15" + " 00:00";
            }
            else
            {
                string Addtime = (Convert.ToInt32(time) + 1).ToString();
                stime = (Cxtime.Year-1).ToString() + '/' + "12" + '/' + "15" + " 00:00";
                etime = Cxtime.Year.ToString() + '/' + "01" + '/' + "14" + " 23:59";
            }
            DateTime astime = Convert.ToDateTime(stime);
            DateTime aetime = Convert.ToDateTime(etime);

            if ((chaxunmethod == "全厂" && !rolename.Contains("管理员")) || (chaxunmethod == "装置") || (chaxunmethod == "车间") && !rolename.Contains("片区长"))
            {
                //直接在数据库中查询
             
             List<A15dot1TabQiYe> res = Jx1.GetJxByA2(astime,aetime, zzid, zy);
             if (res.Count != 0)
             {
                 

                 object a1 = new
                 {
                     zdm = "zzkkxzs",
                     zblx = "KPI",
                     zbname = "装置可靠性指标",
                     ndmbz = "",
                     ljz = res[0].zzkkxzs,
                     id = res[0].Id,
                     ActualValue = res[0].zzkkxzs,

                 };
                 r.Add(a1);

                 object a2 = new
                 {
                     zdm = "wxfyzs",
                     zblx = "KPI",
                     zbname = "维修费用指数",
                     id = res[0].Id,
                     ndmbz = "",
                     ljz = res[0].wxfyzs,
                     ActualValue = res[0].wxfyzs,
                 };
                 r.Add(a2);
                 object a3 = new
                 {
                     zdm = "qtlxbmfxhl",
                     id = res[0].Id,
                     zblx = "KPI",
                     zbname = "千台离心泵（机泵）密封消耗率",
                     ndmbz = "<320",
                     ljz = res[0].qtlxbmfxhl,
                     ActualValue = res[0].qtlxbmfxhl,
                 };
                 r.Add(a3);
                 object a9 = new
                 {
                     zdm = "qtlhsbgsghl",
                     id = res[0].Id,
                     zblx = "KPI",
                     zbname = "千台冷换设备管束（含整台）更换率",
                     ndmbz = "≤30‰",
                     ljz = res[0].qtlhsbgsghl,
                     ActualValue = res[0].qtlhsbgsghl,
                 };
                 r.Add(a9);
                 object a4 = new
                 {
                     zdm = "ybsjkzl",
                     id = res[0].Id,
                     zblx = "KPI",
                     zbname = "仪表实际控制率",
                     ndmbz = "0.93",
                     ljz = res[0].ybsjkzl,
                     ActualValue = res[0].ybsjkzl,
                 };
                 r.Add(a4);
                 object a5 = new
                 {
                     zdm = "sjs",
                     id = res[0].Id,
                     zblx = "KPI",
                     zbname = "事件数",
                     ndmbz = "",
                     ljz = res[0].sjs,
                     ActualValue = res[0].sjs,
                 };
                 r.Add(a5);
                 object a6 = new
                 {
                     zdm = "gzqdkf",
                     id = res[0].Id,
                     zblx = "KPI",
                     zbname = "故障强度扣分",
                     ndmbz = "",
                     ljz = res[0].gzqdkf,
                     ActualValue = res[0].gzqdkf,
                 };
                 r.Add(a6);
                 object a7 = new
                 {
                     zdm = "xmyql",
                     id = res[0].Id,
                     zblx = "KPI",
                     zbname = "项目逾期率",
                     ndmbz = "",
                     ljz = res[0].xmyql,
                     ActualValue = res[0].xmyql,
                 };
                 r.Add(a7);
                 object a8 = new
                 {
                     zdm = "pxjnl",
                     id = res[0].Id,
                     zblx = "KPI",
                     zbname = "培训及能力",
                     ndmbz = "",
                     ljz = res[0].pxjnl,
                     ActualValue = res[0].pxjnl,
                 };
                 r.Add(a8);
                 return r;
             }
             else
             {
                 return r;
             }


            }
            else if ((chaxunmethod == "全厂" && rolename.Contains("管理员")) || (chaxunmethod == "车间" && rolename.Contains("片区长")))
            {

                //再次判断管理员和片区长的查询位置
                //首先利用ifqc判断管理员是查询数据库还是计算查询
                bool ifqc = Jx1.GetifQiYeQc(zy, astime, aetime);//如果表中存在超级用户已经提报的全厂的数据，就在数据库中去查，否则计算显示.
                if (chaxunmethod == "全厂" && ifqc)
                {//表中存在超级用户已经提报的全厂的数据，就在数据库中去查
                    List<A15dot1TabQiYe> res = Jx1.GetJxByA2(astime,aetime, zzid, zy);
                    if (res.Count != 0)
                    {


                        object a1 = new
                        {
                            zdm = "zzkkxzs",
                            zblx = "KPI",
                            zbname = "装置可靠性指标",
                            ndmbz = "",
                            ljz = res[0].zzkkxzs,
                            id = res[0].Id,
                            ActualValue = res[0].zzkkxzs,

                        };
                        r.Add(a1);

                        object a2 = new
                        {
                            zdm = "wxfyzs",
                            zblx = "KPI",
                            zbname = "维修费用指数",
                            id = res[0].Id,
                            ndmbz = "",
                            ljz = res[0].wxfyzs,
                            ActualValue = res[0].wxfyzs,
                        };
                        r.Add(a2);
                        object a3 = new
                        {
                            zdm = "qtlxbmfxhl",
                            id = res[0].Id,
                            zblx = "KPI",
                            zbname = "千台离心泵（机泵）密封消耗率",
                            ndmbz = "<320",
                            ljz = res[0].qtlxbmfxhl,
                            ActualValue = res[0].qtlxbmfxhl,
                        };
                        r.Add(a3);
                        object a9 = new
                        {
                            zdm = "qtlhsbgsghl",
                            id = res[0].Id,
                            zblx = "KPI",
                            zbname = "千台冷换设备管束（含整台）更换率",
                            ndmbz = "≤30‰",
                            ljz = res[0].qtlhsbgsghl,
                            ActualValue = res[0].qtlhsbgsghl,
                        };
                        r.Add(a9);
                        object a4 = new
                        {
                            zdm = "ybsjkzl",
                            id = res[0].Id,
                            zblx = "KPI",
                            zbname = "仪表实际控制率",
                            ndmbz = "0.93",
                            ljz = res[0].ybsjkzl,
                            ActualValue = res[0].ybsjkzl,
                        };
                        r.Add(a4);
                        object a5 = new
                        {
                            zdm = "sjs",
                            id = res[0].Id,
                            zblx = "KPI",
                            zbname = "事件数",
                            ndmbz = "",
                            ljz = res[0].sjs,
                            ActualValue = res[0].sjs,
                        };
                        r.Add(a5);
                        object a6 = new
                        {
                            zdm = "gzqdkf",
                            id = res[0].Id,
                            zblx = "KPI",
                            zbname = "故障强度扣分",
                            ndmbz = "",
                            ljz = res[0].gzqdkf,
                            ActualValue = res[0].gzqdkf,
                        };
                        r.Add(a6);
                        object a7 = new
                        {
                            zdm = "xmyql",
                            id = res[0].Id,
                            zblx = "KPI",
                            zbname = "项目逾期率",
                            ndmbz = "",
                            ljz = res[0].xmyql,
                            ActualValue = res[0].xmyql,
                        };
                        r.Add(a7);
                        object a8 = new
                        {
                            zdm = "pxjnl",
                            id = res[0].Id,
                            zblx = "KPI",
                            zbname = "培训及能力",
                            ndmbz = "",
                            ljz = res[0].pxjnl,
                            ActualValue = res[0].pxjnl,
                        };
                        r.Add(a8);
                        return r;
                    }
                    else
                    {
                        return r;//数据库没有查到表
                    }
                }
                else if (chaxunmethod == "全厂")
                {
                    var qc = Jx1.GetQiYeQc(zy, astime, aetime);//计算的方式获得企业全厂的数据
                    List<string> qc_list = new List<string>();
                    foreach (var i in qc)
                    {
                        if (i == null)
                            qc_list.Add("当月无装置提报该kpi");//如果全厂在数据库中没有值，qc_list赋值为空字符串
                        else
                            qc_list.Add(i.ToString());

                    }
                    if (qc.Count == 9)
                    {
                        object a1 = new
                        {
                            zdm = "zzkkxzs",
                            zblx = "KPI",
                            zbname = "装置可靠性指数",
                            ndmbz = "45",
                            ljz = qc_list[0].ToString(),
                            id = 0,//id设置为0，则获得趋势图的时候可以判断
                            ActualValue = qc_list[0].ToString(),
                        };
                        r.Add(a1);

                        object a2 = new
                        {
                            zdm = "wxfyzs",
                            zblx = "KPI",
                            zbname = "维修费用指数",
                            id = 0,//id设置为0，则获得趋势图的时候可以判断
                            ndmbz = "<0.6‰",
                            ljz = qc_list[1].ToString(),
                            //id = res[0].Id,
                            ActualValue = qc_list[1].ToString(),
                        };
                        r.Add(a2);

                        object a3 = new
                        {
                            zdm = "qtlxbmfxhl",
                            id = 0,//id设置为0，则获得趋势图的时候可以判断
                            zblx = "KPI",
                            zbname = "千台离心泵（机泵）密封消耗率",
                            ndmbz = "<7%",
                            ljz = qc_list[2].ToString(),
                            //id = res[0].Id,
                            ActualValue = qc_list[2].ToString(),
                        };
                        r.Add(a3);

                        object a4 = new
                        {
                            zdm = "qtlxbmfxhl",
                            id = 0,//id设置为0，则获得趋势图的时候可以判断
                            zblx = "KPI",
                            zbname = "千台冷换设备管束（含整台）更换率",
                            ndmbz = "<7%",
                            ljz = qc_list[2].ToString(),
                            //id = res[0].Id,
                            ActualValue = qc_list[3].ToString(),
                        };
                        r.Add(a4);

                        object a5 = new
                        {
                            zdm = "ybsjkzl",
                            id = 0,//id设置为0，则获得趋势图的时候可以判断
                            zblx = "KPI",
                            zbname = "仪表实际控制率",
                            ndmbz = "",
                            ljz = qc_list[4].ToString(),
                            //id = res[0].Id,
                            ActualValue = qc_list[4].ToString(),
                        };
                        r.Add(a5);
                        object a6 = new
                        {
                            zdm = "sjs",
                            id = 0,//id设置为0，则获得趋势图的时候可以判断
                            zblx = "KPI",
                            zbname = "事件数",
                            ndmbz = "",
                            ljz = qc_list[5].ToString(),
                            //id = res[0].Id,
                            ActualValue = qc_list[5].ToString(),
                        };
                        r.Add(a6);
                        object a7 = new
                        {
                            zdm = "gzqdkf",
                            id = 0,//id设置为0，则获得趋势图的时候可以判断
                            zblx = "KPI",
                            zbname = "故障强度扣分",
                            ndmbz = "20000",
                            ljz = qc_list[6].ToString(),
                            //id = res[0].Id,
                            ActualValue = qc_list[6].ToString(),
                        };
                        r.Add(a7);
                        object a8 = new
                        {
                            zdm = "xmyql",
                            id = 0,//id设置为0，则获得趋势图的时候可以判断
                            zblx = "KPI",
                            zbname = "项目逾期率",
                            ndmbz = "28000",
                            ljz = qc_list[7].ToString(),
                            //id = res[0].Id,
                            ActualValue = qc_list[7].ToString(),
                        };
                        r.Add(a8);
                        object a9 = new
                        {
                            zdm = "pxjnl",
                            id = 0,//id设置为0，则获得趋势图的时候可以判断
                            zblx = "KPI",
                            zbname = "培训及能力",
                            ndmbz = "98%",
                            ljz = qc_list[8].ToString(),
                            //id = res[0].Id,
                            ActualValue = qc_list[8].ToString(),
                        };
                        r.Add(a9);
                        return r;
                    }
                    else
                    {
                        return r;//有人没有提报装置,计算不到数据

                    }

                }
                else if (chaxunmethod == "车间")
                {
                    var qc = Jx1.GetCjQiYe(zy, cjname,astime, aetime);//计算的方式获得企业全厂的数据
                    List<string> qc_list = new List<string>();
                    foreach (var i in qc)
                    {
                        if (i == null)
                            qc_list.Add("当月无装置提报该kpi");//如果全厂在数据库中没有值，qc_list赋值为空字符串
                        else
                            qc_list.Add(i.ToString());

                    }
                    object a1 = new
                    {
                        zdm = "zzkkxzs",
                        zblx = "KPI",
                        zbname = "装置可靠性指数",
                        ndmbz = "45",
                        ljz = qc_list[0].ToString(),
                        id = 0,//id设置为0，则获得趋势图的时候可以判断
                        ActualValue = qc_list[0].ToString(),
                    };
                    r.Add(a1);

                    object a2 = new
                    {
                        zdm = "wxfyzs",
                        zblx = "KPI",
                        zbname = "维修费用指数",
                        id = 0,//id设置为0，则获得趋势图的时候可以判断
                        ndmbz = "<0.6‰",
                        ljz = qc_list[1].ToString(),
                        //id = res[0].Id,
                        ActualValue = qc_list[1].ToString(),
                    };
                    r.Add(a2);

                    object a3 = new
                    {
                        zdm = "qtlxbmfxhl",
                        id = 0,//id设置为0，则获得趋势图的时候可以判断
                        zblx = "KPI",
                        zbname = "千台离心泵（机泵）密封消耗率",
                        ndmbz = "<7%",
                        ljz = qc_list[2].ToString(),
                        //id = res[0].Id,
                        ActualValue = qc_list[2].ToString(),
                    };
                    r.Add(a3);

                    object a4 = new
                    {
                        zdm = "qtlxbmfxhl",
                        id = 0,//id设置为0，则获得趋势图的时候可以判断
                        zblx = "KPI",
                        zbname = "千台冷换设备管束（含整台）更换率",
                        ndmbz = "<7%",
                        ljz = qc_list[2].ToString(),
                        //id = res[0].Id,
                        ActualValue = qc_list[3].ToString(),
                    };
                    r.Add(a4);

                    object a5 = new
                    {
                        zdm = "ybsjkzl",
                        id = 0,//id设置为0，则获得趋势图的时候可以判断
                        zblx = "KPI",
                        zbname = "仪表实际控制率",
                        ndmbz = "",
                        ljz = qc_list[4].ToString(),
                        //id = res[0].Id,
                        ActualValue = qc_list[4].ToString(),
                    };
                    r.Add(a5);
                    object a6 = new
                    {
                        zdm = "sjs",
                        id = 0,//id设置为0，则获得趋势图的时候可以判断
                        zblx = "KPI",
                        zbname = "事件数",
                        ndmbz = "",
                        ljz = qc_list[5].ToString(),
                        //id = res[0].Id,
                        ActualValue = qc_list[5].ToString(),
                    };
                    r.Add(a6);
                    object a7 = new
                    {
                        zdm = "gzqdkf",
                        id = 0,//id设置为0，则获得趋势图的时候可以判断
                        zblx = "KPI",
                        zbname = "故障强度扣分",
                        ndmbz = "20000",
                        ljz = qc_list[6].ToString(),
                        //id = res[0].Id,
                        ActualValue = qc_list[6].ToString(),
                    };
                    r.Add(a7);
                    object a8 = new
                    {
                        zdm = "xmyql",
                        id = 0,//id设置为0，则获得趋势图的时候可以判断
                        zblx = "KPI",
                        zbname = "项目逾期率",
                        ndmbz = "28000",
                        ljz = qc_list[7].ToString(),
                        //id = res[0].Id,
                        ActualValue = qc_list[7].ToString(),
                    };
                    r.Add(a8);
                    object a9 = new
                    {
                        zdm = "pxjnl",
                        id = 0,//id设置为0，则获得趋势图的时候可以判断
                        zblx = "KPI",
                        zbname = "培训及能力",
                        ndmbz = "98%",
                        ljz = qc_list[8].ToString(),
                        //id = res[0].Id,
                        ActualValue = qc_list[8].ToString(),
                    };
                    r.Add(a9);
                    return r;
              
                    //车间查询的写法
                }
                else
                {
                    return r;
                }

            }

            return r;
           
        }

        //public List<object> getObjectQiYeforuser(string time, string zzid, string zy)
        //{ 
        
        //}

        public List<object> getObjectJing(string time, string zzid, string zy)//回传了月份，装置id，专业
        {
            string username = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
            List<object> r = new List<object>();
            EquipManagment em = new EquipManagment();
            string Zzid = "";
            if (zzid == "全厂")
            {
                Zzid = "全厂";//全厂查询
            }
            else
            {
                if (zzid != "")//装置查询
                    Zzid = em.getEa_namebyid(Convert.ToInt32(zzid));//将zzid转换为装置的名字
            }

            if ((username != "sa" && username != "武文斌" && Zzid != "") || ((Zzid != "全厂" && Zzid != "") && (username == "sa" || username == "武文斌")))//普通用户全厂当做装置查询,超级用户查装置
            {
                List<A15dot1TabJing> res = Jx1.GetJingJxToA2(time, Zzid, zy);//
                if (res.Count != 0)
                {


                    object a1 = new
                    {
                        id = res[0].Id,
                        zdm = "gzqdkf",
                        zblx = "KPI",
                        zbname = "故障强度扣分",
                        ndmbz = "≤4",
                        ljz = res[0].gzqdkf,
                        ActualValue = res[0].gzqdkf,
                    };
                    r.Add(a1);

                    object a2 = new
                    {
                        id = res[0].Id,
                        zdm = "sbfsxlcs",
                        zblx = "KPI",
                        zbname = "设备腐蚀泄漏次数（有毒有害易燃易爆介质）",
                        ndmbz = "≤28次",
                        ljz = res[0].sbfsxlcs,
                        ActualValue = res[0].sbfsxlcs,
                    };
                    r.Add(a2);

                    object a3 = new
                    {
                        id = res[0].Id,
                        zdm = "qtlhsbgs",
                        zblx = "KPI",
                        zbname = "千台冷换设备管束（含整台）更换率",
                        ndmbz = "≤30‰",
                        ljz = res[0].qtlhsbgs,
                        ActualValue = res[0].qtlhsbgs,
                    };
                    r.Add(a3);

                    object a4 = new
                    {
                        id = res[0].Id,
                        zdm = "gylpjrxl",
                        zblx = "KPI",
                        zbname = "工业炉（≥10MW）平均热效率",
                        ndmbz = "≥92.2%",
                        ljz = res[0].gylpjrxl,
                        ActualValue = res[0].gylpjrxl,
                    };
                    r.Add(a4);

                    object a5 = new
                    {
                        id = res[0].Id,
                        zdm = "hrqjxl",
                        zblx = "KPI",
                        zbname = "换热器检修率",
                        ndmbz = "＜2%",
                        ljz = res[0].hrqjxl,
                        ActualValue = res[0].hrqjxl,
                    };
                    r.Add(a5);

                    object a6 = new
                    {
                        id = res[0].Id,
                        zdm = "ylrqdjl",
                        zblx = "KPI",
                        zbname = "压力容器定检率",
                        ndmbz = "1",
                        ljz = res[0].ylrqdjl,
                        ActualValue = res[0].ylrqdjl,
                    };
                    r.Add(a6);

                    object a7 = new
                    {
                        id = res[0].Id,
                        zdm = "ylgdndjxjhwcl",
                        zblx = "KPI",
                        zbname = "压力管道年度检验计划完成率",
                        ndmbz = "1",
                        ljz = res[0].ylgdndjxjhwcl,
                        ActualValue = res[0].ylgdndjxjhwcl,
                    };
                    r.Add(a7);

                    object a8 = new
                    {
                        id = res[0].Id,
                        zdm = "aqfndjyjhwcl",
                        zblx = "KPI",
                        zbname = "安全阀年度校验计划完成率",
                        ndmbz = "1",
                        ljz = res[0].aqfndjyjhwcl,
                        ActualValue = res[0].aqfndjyjhwcl,
                    };
                    r.Add(a8);

                    object a9 = new
                    {
                        id = res[0].Id,
                        zdm = "sbfsjcjhwcl",
                        zblx = "KPI",
                        zbname = "设备腐蚀监测计划完成率",
                        ndmbz = "1",
                        ljz = res[0].sbfsjcjhwcl,
                        ActualValue = res[0].sbfsjcjhwcl,
                    };
                    r.Add(a9);

                    object a10 = new
                    {
                        id = res[0].Id,
                        zdm = "jsbjwxychgl",
                        zblx = "KPI",
                        zbname = "静设备检维修一次合格率",
                        ndmbz = ">99%",
                        ljz = res[0].jsbjwxychgl,
                        ActualValue = res[0].jsbjwxychgl,
                    };
                    r.Add(a10);
                    return r;
                }
                else
                {
                    return r;//根据装置查询的时候数据库里面没有记录。
                }
            }
            else if (Zzid == "全厂" && (username == "sa" || username == "武文斌"))
            {
                List<object> qc = new List<object>();
                DateTime Cxtime = DateTime.Now;
                string stime;
                string etime;
                if (Convert.ToInt32(time) > 1)
                {
                    string Addtime = (Convert.ToInt32(time) + 1).ToString();
                    stime = Cxtime.Year.ToString() + '/' + time + '/' + "01" + " 00:00";
                    etime = Cxtime.Year.ToString() + '/' + Addtime + '/' + "01" + " 00:00";
                }
                else
                {
                    string Addtime = (Convert.ToInt32(time) + 1).ToString();
                    stime = (Cxtime.Year).ToString() + '/' + "01" + '/' + "01" + " 00:00";
                    etime = Cxtime.Year.ToString() + '/' + Addtime + '/' + "01" + " 00:00";
                }
                DateTime astime = Convert.ToDateTime(stime);
                DateTime aetime = Convert.ToDateTime(etime);
                qc = Jx1.GetQcJing(zy, astime, aetime);
                List<string> qc_list = new List<string>();
                foreach (var i in qc)
                {
                    if (i == null)
                        qc_list.Add("当月无装置提报该kpi");//如果全厂在数据库中没有值，qc_list赋值为空字符串
                    else
                        qc_list.Add(i.ToString());

                }
                if (qc.Count == 10)
                {
                    object a1 = new
                {
                    id = 0,
                    zdm = "gzqdkf",
                    zblx = "KPI",
                    zbname = "故障强度扣分",
                    ndmbz = "≤4",
                    ljz = qc_list[0].ToString(),
                    ActualValue = qc_list[0].ToString(),
                };

                    r.Add(a1);

                    object a2 = new
                   {
                       id = 0,
                       zdm = "sbfsxlcs",
                       zblx = "KPI",
                       zbname = "设备腐蚀泄漏次数（有毒有害易燃易爆介质）",
                       ndmbz = "≤28次",
                       ljz = qc_list[1].ToString(),
                       ActualValue = qc_list[1].ToString(),
                   };
                    r.Add(a2);

                    object a3 = new
                    {
                        id = 0,
                        zdm = "qtlhsbgs",
                        zblx = "KPI",
                        zbname = "千台冷换设备管束（含整台）更换率",
                        ndmbz = "≤30‰",
                        ljz = qc_list[2].ToString(),
                        ActualValue = qc_list[2].ToString(),
                    };
                    r.Add(a3);

                    object a4 = new
                    {
                        id = 0,
                        zdm = "gylpjrxl",
                        zblx = "KPI",
                        zbname = "工业炉（≥10MW）平均热效率",
                        ndmbz = "≥92.2%",
                        ljz = qc_list[3].ToString(),
                        ActualValue = qc_list[3].ToString(),
                    };
                    r.Add(a4);

                    object a5 = new
                    {
                        id = 0,
                        zdm = "hrqjxl",
                        zblx = "KPI",
                        zbname = "换热器检修率",
                        ndmbz = "＜2%",
                        ljz = qc_list[4].ToString(),
                        ActualValue = qc_list[4].ToString(),
                    };
                    r.Add(a5);

                    object a6 = new
                    {
                        id = 0,
                        zdm = "ylrqdjl",
                        zblx = "KPI",
                        zbname = "压力容器定检率",
                        ndmbz = "1",
                        ljz = qc_list[5].ToString(),
                        ActualValue = qc_list[5].ToString(),
                    };
                    r.Add(a6);

                    object a7 = new
                    {
                        id = 0,
                        zdm = "ylgdndjxjhwcl",
                        zblx = "KPI",
                        zbname = "压力管道年度检验计划完成率",
                        ndmbz = "1",
                        ljz = qc_list[6].ToString(),
                        ActualValue = qc_list[6].ToString(),
                    };
                    r.Add(a7);

                    object a8 = new
                    {
                        id = 0,
                        zdm = "aqfndjyjhwcl",
                        zblx = "KPI",
                        zbname = "安全阀年度校验计划完成率",
                        ndmbz = "1",
                        ljz = qc_list[7].ToString(),
                        ActualValue = qc_list[7].ToString(),
                    };
                    r.Add(a8);

                    object a9 = new
                    {
                        id = 0,
                        zdm = "sbfsjcjhwcl",
                        zblx = "KPI",
                        zbname = "设备腐蚀监测计划完成率",
                        ndmbz = "1",
                        ljz = qc_list[8].ToString(),
                        ActualValue = qc_list[8].ToString(),
                    };
                    r.Add(a9);

                    object a10 = new
                    {
                        id = 0,
                        zdm = "jsbjwxychgl",
                        zblx = "KPI",
                        zbname = "静设备检维修一次合格率",
                        ndmbz = ">99%",
                        ljz = qc_list[9].ToString(),
                        ActualValue = qc_list[9].ToString(),
                    };
                    r.Add(a10);
                    return r;
                }
                else
                {
                    return r;//查询片区
                }
            }
            else
            {
                return r;//查询全厂的时候数据库没有数据
            }

        }
        public List<object> getObjectDian(string time, string zzid, string zy)
        {
            string username = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
            List<object> r = new List<object>();
            EquipManagment em = new EquipManagment();
            string Zzid = "";
            if (zzid == "全厂")
            {
                Zzid = "全厂";//全厂查询
            }
            else
            {
                if (zzid != "")//装置查询
                    Zzid = em.getEa_namebyid(Convert.ToInt32(zzid));//将zzid转换为装置的名字
            }

            if ((username != "sa" && username != "武文斌" && Zzid != "") || ((Zzid != "全厂" && Zzid != "") && (username == "sa" || username == "武文斌")))//普通用户全厂当做装置查询,超级用户查装置
            {
                List<A15dot1TabDian> res = Jx1.GetDianJxToA2(time, Zzid, zy);
                if (res.Count != 0)
                {



                    object a1 = new
                    {
                        id = res[0].Id,
                        zdm = "gzqdkf",
                        zblx = "KPI",
                        zbname = "故障强度扣分",
                        ndmbz = "≤60分",
                        ljz = res[0].gzqdkf,
                        ActualValue = res[0].gzqdkf,
                    };
                    r.Add(a1);

                    object a2 = new
                    {
                        id = res[0].Id,
                        zdm = "dqwczcs",
                        zblx = "KPI",
                        zbname = "电气误操作次数",
                        ndmbz = "0",
                        ljz = res[0].dqwczcs,
                        ActualValue = res[0].dqwczcs,
                    };
                    r.Add(a2);

                    object a3 = new
                    {
                        id = res[0].Id,
                        zdm = "jdbhzqdzl",
                        zblx = "KPI",
                        zbname = "继电保护正确动作率",
                        ndmbz = "100%",
                        ljz = res[0].jdbhzqdzl,
                        ActualValue = res[0].jdbhzqdzl,
                    };
                    r.Add(a3);

                    object a4 = new
                    {
                        id = res[0].Id,
                        zdm = "sbgzl",
                        zblx = "KPI",
                        zbname = "设备故障率",
                        ndmbz = "",
                        ljz = res[0].sbgzl,
                        ActualValue = res[0].sbgzl,
                    };
                    r.Add(a4);

                    object a5 = new
                    {
                        id = res[0].Id,
                        zdm = "djMTBF",
                        zblx = "KPI",
                        zbname = "电机MTBF",
                        ndmbz = "240",
                        ljz = res[0].djMTBF,
                        ActualValue = res[0].djMTBF,
                    };
                    r.Add(a5);

                    object a6 = new
                    {
                        id = res[0].Id,
                        zdm = "dzdlsbMTBF",
                        zblx = "KPI",
                        zbname = "电力电子设备MTBF",
                        ndmbz = "180",
                        ljz = res[0].dzdlsbMTBF,
                        ActualValue = res[0].dzdlsbMTBF,
                    };
                    r.Add(a6);

                    object a7 = new
                    {
                        id = res[0].Id,
                        zdm = "zbglys",
                        zblx = "KPI",
                        zbname = "主变功率因素",
                        ndmbz = "",
                        ljz = res[0].zbglys,
                        ActualValue = res[0].zbglys,
                    };
                    r.Add(a7);

                    object a8 = new
                    {
                        id = res[0].Id,
                        zdm = "dnjlphl",
                        zblx = "KPI",
                        zbname = "电能计量平衡率",
                        ndmbz = "≥99.8%",
                        ljz = res[0].dnjlphl,
                        ActualValue = res[0].dnjlphl,
                    };
                    r.Add(a8);
                    return r;
                }
                else
                {
                    return r;
                }
            }
            else if (Zzid == "全厂" && (username == "sa" || username == "武文斌"))
            {
                List<object> qc = new List<object>();
                DateTime Cxtime = DateTime.Now;
                string stime;
                string etime;
                if (Convert.ToInt32(time) > 1)
                {
                    string Addtime = (Convert.ToInt32(time) + 1).ToString();
                    stime = Cxtime.Year.ToString() + '/' + time + '/' + "01" + " 00:00";
                    etime = Cxtime.Year.ToString() + '/' + Addtime + '/' + "01" + " 00:00";
                }
                else
                {
                    string Addtime = (Convert.ToInt32(time) + 1).ToString();
                    stime = (Cxtime.Year).ToString() + '/' + "01" + '/' + "01" + " 00:00";
                    etime = Cxtime.Year.ToString() + '/' + Addtime + '/' + "01" + " 00:00";
                }
                DateTime astime = Convert.ToDateTime(stime);
                DateTime aetime = Convert.ToDateTime(etime);
                qc = Jx1.GetQcDian(zy, astime, aetime);
                List<string> qc_list = new List<string>();
                foreach (var i in qc)
                {
                    if (i == null)
                        qc_list.Add("当月无装置提报该kpi");//如果全厂在数据库中没有值，qc_list赋值为空字符串
                    else
                        qc_list.Add(i.ToString());

                }
                if (qc.Count == 5)
                {
                    object a1 = new
                    {
                        //id = res[0].Id,
                        zdm = "gzqdkf",
                        zblx = "KPI",
                        zbname = "故障强度扣分",
                        ndmbz = "≤60分",
                        ljz = qc_list[0],
                        ActualValue = qc_list[0],
                    };
                    r.Add(a1);

                    object a2 = new
                    {
                        //   id = res[0].Id,
                        zdm = "dqwczcs",
                        zblx = "KPI",
                        zbname = "电气误操作次数",
                        ndmbz = "0",
                        ljz = qc_list[1],
                        ActualValue = qc_list[1],
                    };
                    r.Add(a2);

                    object a3 = new
                    {
                        // id = res[0].Id,
                        zdm = "jdbhzqdzl",
                        zblx = "KPI",
                        zbname = "继电保护正确动作率",
                        ndmbz = "100%",
                        ljz = qc_list[2],
                        ActualValue = qc_list[2],
                    };
                    r.Add(a3);

                    object a4 = new
                    {
                        // id = res[0].Id,
                        zdm = "sbgzl",
                        zblx = "KPI",
                        zbname = "设备故障率",
                        ndmbz = "",
                        ljz = qc_list[3],
                        ActualValue = qc_list[3],
                    };
                    r.Add(a4);

                    object a5 = new
                    {
                        //id = res[0].Id,
                        zdm = "djMTBF",
                        zblx = "KPI",
                        zbname = "电机MTBF",
                        ndmbz = "240",
                        ljz = qc_list[4],
                        ActualValue = qc_list[4],
                    };
                    r.Add(a5);

                    object a6 = new
                    {
                        // id = res[0].Id,
                        zdm = "dzdlsbMTBF",
                        zblx = "KPI",
                        zbname = "电力电子设备MTBF",
                        ndmbz = "180",
                        ljz = "",
                        ActualValue = "请人为填写",
                    };
                    r.Add(a6);

                    object a7 = new
                    {
                        // id = res[0].Id,
                        zdm = "zbglys",
                        zblx = "KPI",
                        zbname = "主变功率因素",
                        ndmbz = "",
                        ljz = "",
                        ActualValue = "请人为填写",
                    };
                    r.Add(a7);

                    object a8 = new
                    {
                        // id = res[0].Id,
                        zdm = "dnjlphl",
                        zblx = "KPI",
                        zbname = "电能计量平衡率",
                        ndmbz = "≥99.8%",
                        ljz = "",
                        ActualValue = "请人为填写",
                    };
                    r.Add(a8);
                    return r;
                }
                else
                {
                    return r;

                }
            }
            else 
            {
                return r;
            }
        }

        public List<object> getObjectYi(string time, string zzid, string zy)
        {
            string username = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
            List<object> r = new List<object>();
            EquipManagment em = new EquipManagment();
            string Zzid = "";
            if (zzid == "全厂")
            {
                Zzid = "全厂";
            }
            else
            {
                Zzid = em.getEa_namebyid(Convert.ToInt32(zzid));
            }

            if ((username != "sa" && username != "武文斌" && Zzid != "") || ((Zzid != "全厂" && Zzid != "") && (username == "sa" || username == "武文斌")))//普通用户全厂当做装置查询,超级用户查装置
            {
                List<A15dot1TabYi> res = Jx1.GetYiJxToA2(time, Zzid, zy);
                if (res.Count != 0)
                {
                    object a1 = new
                    {
                        id = res[0].Id,
                        zdm = "gzqdkf",
                        zblx = "KPI",
                        zbname = "故障强度扣分",
                        ndmbz = "≤4/50分",
                        ljz = res[0].gzqdkf,
                        ActualValue = res[0].gzqdkf,
                    };
                    r.Add(a1);

                    object a2 = new
                    {
                        id = res[0].Id,
                        zdm = "lsxtzqdzl",
                        zblx = "KPI",
                        zbname = "联锁系统正确动作率",
                        ndmbz = "≥99.8%",
                        ljz = res[0].lsxtzqdzl,
                        ActualValue = res[0].lsxtzqdzl,
                    };
                    r.Add(a2);

                    object a3 = new
                    {
                        id = res[0].Id,
                        zdm = "kzxtgzcs",
                        zblx = "KPI",
                        zbname = "控制系统故障次数",
                        ndmbz = "≤2",
                        ljz = res[0].kzxtgzcs,
                        ActualValue = res[0].kzxtgzcs,
                    };
                    r.Add(a3);

                    object a4 = new
                    {
                        id = res[0].Id,
                        zdm = "ybkzl",
                        zblx = "KPI",
                        zbname = "仪表控制率",
                        ndmbz = "0.99",
                        ljz = res[0].ybkzl,
                        ActualValue = res[0].ybkzl,
                    };
                    r.Add(a4);

                    object a5 = new
                    {
                        id = res[0].Id,
                        zdm = "ybsjkzl",
                        zblx = "KPI",
                        zbname = "仪表实际控制率",
                        ndmbz = "0.93",
                        ljz = res[0].ybsjkzl,
                        ActualValue = res[0].ybsjkzl,
                    };
                    r.Add(a5);

                    object a6 = new
                    {
                        id = res[0].Id,
                        zdm = "lsxttyl",
                        zblx = "KPI",
                        zbname = "联锁系统投用率",
                        ndmbz = "1",
                        ljz = res[0].lsxttyl,
                        ActualValue = res[0].lsxttyl,
                    };
                    r.Add(a6);

                    object a7 = new
                    {
                        id = res[0].Id,
                        zdm = "gjkzfmgzcs",
                        zblx = "KPI",
                        zbname = "关键控制阀门故障次数",
                        ndmbz = "3",
                        ljz = res[0].gjkzfmgzcs,
                        ActualValue = res[0].gjkzfmgzcs,
                    };
                    r.Add(a7);

                    object a8 = new
                    {
                        id = res[0].Id,
                        zdm = "kzxtgzbjcs",
                        zblx = "KPI",
                        zbname = "控制系统故障报警次数",
                        ndmbz = "25",
                        ljz = res[0].kzxtgzbjcs,
                        ActualValue = res[0].kzxtgzbjcs,
                    };
                    r.Add(a8);
                    object a9 = new
                    {
                        id = res[0].Id,
                        zdm = "cgybgzl",
                        zblx = "KPI",
                        zbname = "常规仪表故障率",
                        ndmbz = "≤1‰",
                        ljz = res[0].cgybgzl,
                        ActualValue = res[0].cgybgzl,
                    };
                    r.Add(a9);
                    object a10 = new
                    {
                        id = res[0].Id,
                        zdm = "tjfMDBF",
                        zblx = "KPI",
                        zbname = "调节阀MTBF",
                        ndmbz = "200",
                        ljz = res[0].tjfMDBF,
                        ActualValue = res[0].tjfMDBF,
                    };
                    r.Add(a10);
                    return r;
                }
                else
                {
                    return r;
                }
            }

            else if (Zzid == "全厂" && (username == "sa" || username == "武文斌"))
            {
                List<object> qc = new List<object>();
                DateTime Cxtime = DateTime.Now;
                string stime;
                string etime;
                if (Convert.ToInt32(time) > 1)
                {
                    string Addtime = (Convert.ToInt32(time) + 1).ToString();
                    stime = Cxtime.Year.ToString() + '/' + time + '/' + "01" + " 00:00";
                    etime = Cxtime.Year.ToString() + '/' + Addtime + '/' + "01" + " 00:00";
                }
                else
                {
                    string Addtime = (Convert.ToInt32(time) + 1).ToString();
                    stime = (Cxtime.Year).ToString() + '/' + "01" + '/' + "01" + " 00:00";
                    etime = Cxtime.Year.ToString() + '/' + Addtime + '/' + "01" + " 00:00";
                }
                DateTime astime = Convert.ToDateTime(stime);
                DateTime aetime = Convert.ToDateTime(etime);
                qc = Jx1.GetQcYi(zy, astime, aetime);
                List<string> qc_list = new List<string>();
                foreach (var i in qc)
                {
                    if (i == null)
                        qc_list.Add("当月无装置提报该kpi");//如果全厂在数据库中没有值，qc_list赋值为空字符串
                    else
                        qc_list.Add(i.ToString());

                }
                if (qc.Count == 10)
                {
                    object a1 = new
                    {
                        zdm = "gzqdkf",
                        zblx = "KPI",
                        zbname = "故障强度扣分",
                        ndmbz = "≤4/50分",
                        ljz = qc_list[0].ToString(),
                        id = "0",
                        ActualValue = qc_list[0].ToString(),

                    };
                    r.Add(a1);

                    object a2 = new
                    {
                        id = "0",
                        zdm = "lsxtzqdzl",
                        zblx = "KPI",
                        zbname = "联锁系统正确动作率",
                        // id = res[0].Id,
                        ndmbz = "≥99.8%",
                        ljz = qc_list[1].ToString(),
                        //id = res[0].Id,
                        ActualValue = qc_list[1].ToString(),
                    };
                    r.Add(a2);
                    object a3 = new
                    {
                        id = "0",
                        zdm = "kzxtgzcs",
                        //  id = res[0].Id,
                        zblx = "KPI",
                        zbname = "控制系统故障次数",
                        ndmbz = "≤2",
                        ljz = qc_list[2].ToString(),
                        //id = res[0].Id,
                        ActualValue = qc_list[2].ToString(),
                    };
                    r.Add(a3);
                    object a4 = new
                    {
                        id = "0",
                        zdm = "ybkzl",
                        // id = res[3].Id,
                        zblx = "KPI",
                        zbname = "仪表控制率",
                        ndmbz = "0.99",
                        ljz = qc_list[3].ToString(),
                        //id = res[0].Id,
                        ActualValue = qc_list[3].ToString(),
                    };
                    r.Add(a4);
                    object a5 = new
                    {
                        id = "0",
                        zdm = "ybsjkzl",
                        // id = res[0].Id,
                        zblx = "KPI",
                        zbname = "仪表实际控制率",
                        ndmbz = "0.93",
                        ljz = qc_list[4].ToString(),
                        //id = res[0].Id,
                        ActualValue = qc_list[4].ToString(),
                    };
                    r.Add(a5);
                    object a6 = new
                    {
                        id = "0",
                        zdm = "lsxttyl",
                        //id = res[0].Id,
                        zblx = "KPI",
                        zbname = "联锁系统投用率",
                        ndmbz = "1",
                        ljz = qc_list[5].ToString(),
                        //id = res[0].Id,
                        ActualValue = qc_list[5].ToString(),
                    };
                    r.Add(a6);
                    object a7 = new
                    {
                        id = "0",
                        zdm = "gjkzfmgzcs",
                        //id = res[0].Id,
                        zblx = "KPI",
                        zbname = "关键控制阀门故障次数",
                        ndmbz = "3",
                        ljz = qc_list[6].ToString(),
                        //id = res[0].Id,
                        ActualValue = qc_list[6].ToString(),
                    };
                    r.Add(a7);
                    object a8 = new
                    {
                        id = "0",
                        zdm = "kzxtgzbjcs",
                        // id = res[0].Id,
                        zblx = "KPI",
                        zbname = "控制系统故障报警次数",
                        ndmbz = "25",
                        ljz = qc_list[7].ToString(),
                        //id = res[0].Id,
                        ActualValue = qc_list[7].ToString(),
                    };
                    r.Add(a8);
                    object a9 = new
                    {
                        id = "0",
                        zdm = "cgybgzl",
                        // id = res[0].Id,
                        zblx = "KPI",
                        zbname = "常规仪表故障率",
                        ndmbz = "≤1‰",
                        ljz = qc_list[8].ToString(),
                        //id = res[0].Id,
                        ActualValue = qc_list[8].ToString(),
                    };
                    r.Add(a9);
                    object a10 = new
                    {
                        id = "0",
                        zdm = "tjfMDBF",
                        // id = res[0].Id,
                        zblx = "KPI",
                        zbname = "调节阀MTBF",
                        ndmbz = "200",
                        ljz = qc_list[9].ToString(),
                        //id = res[0].Id,
                        ActualValue = qc_list[9].ToString(),
                    };
                    r.Add(a10);
                    return r;
                }
                else
                {
                    return r;
                }
            }
            else
            {
                return r;
            }
        }
        
        public bool ModifyQiYe(string json1)
        {

            JObject item = (JObject)JsonConvert.DeserializeObject(json1);
            A15dot1TabQiYe new_15dot1qiye = new A15dot1TabQiYe();

            bool res=false;
          
            if(Convert.ToString(item["zzId"])=="全厂")
            {//管理员对于全厂的修改的保存
                
                DateTime Cxtime = DateTime.Now;
                string time=item["month"].ToString();
                string stime;
                string etime;//设置查询月份的格式，eg：1月15日0：00：00到2月14日23：59：59查询的是2月的数据
                if (Convert.ToInt32(time) > 1)
                {
                    string Reducetime = (Convert.ToInt32(time) - 1).ToString();
                    etime = Cxtime.Year.ToString() + '/' + time + '/' + "14" + " 23:59";
                    stime = Cxtime.Year.ToString() + '/' + Reducetime + '/' + "15" + " 00:00";
                }
                else
                {
                    string Addtime = (Convert.ToInt32(time) + 1).ToString();
                    stime = (Cxtime.Year - 1).ToString() + '/' + "12" + '/' + "15" + " 00:00";
                    etime = Cxtime.Year.ToString() + '/' + "01" + '/' + "14" + " 23:59";
                }
                DateTime astime = Convert.ToDateTime(stime);
                DateTime aetime = Convert.ToDateTime(etime);
                new_15dot1qiye.zzkkxzs = Convert.ToDouble(item["equipReliableExponent"]);
                new_15dot1qiye.wxfyzs = Convert.ToDouble(item["maintainChargeExponent"]);
                new_15dot1qiye.qtlxbmfxhl = Convert.ToDouble(item["thousandLXBrate"]);
                new_15dot1qiye.qtlhsbgsghl = Convert.ToDouble(item["thousandColdChangeRate"]);
                new_15dot1qiye.ybsjkzl = Convert.ToDouble(item["instrumentControlRate"]);
                double a = Convert.ToDouble(item["eventNumber"].ToString().Trim()); 
               
                new_15dot1qiye.sjs = Convert.ToInt16(a);

                double b = Convert.ToDouble(item["troubleKoufen"].ToString().Trim());   
                new_15dot1qiye.gzqdkf = Convert.ToInt32(b);
                new_15dot1qiye.xmyql = Convert.ToDouble(item["projectLateRate"]);
                new_15dot1qiye.pxjnl = Convert.ToDouble(item["trainAndAbility"]);
                new_15dot1qiye.submitDepartment = Convert.ToString(item["zzId"]);
                new_15dot1qiye.temp3 = Convert.ToString(item["cjname"]);//车间名字存于temp3
                new_15dot1qiye.temp2 = "企业级";//专业名字存于temp2
                new_15dot1qiye.submitUser = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
                new_15dot1qiye.submitTime = DateTime.Now;
                res = Jx1.ModifyQcQyJxItem(new_15dot1qiye, astime, aetime);

            }
            else
            {//管理员对于装置的更改的保存
                new_15dot1qiye.zzkkxzs = Convert.ToDouble(item["equipReliableExponent"]);
                new_15dot1qiye.wxfyzs = Convert.ToDouble(item["maintainChargeExponent"]);
                new_15dot1qiye.qtlxbmfxhl = Convert.ToDouble(item["thousandLXBrate"]);
                new_15dot1qiye.qtlhsbgsghl = Convert.ToDouble(item["thousandColdChangeRate"]);
                new_15dot1qiye.ybsjkzl = Convert.ToDouble(item["instrumentControlRate"]);
                new_15dot1qiye.sjs = Convert.ToInt32(item["eventNumber"]);
                new_15dot1qiye.Id = Convert.ToInt32(item["kpiid"]);
                new_15dot1qiye.gzqdkf = Convert.ToInt32(item["troubleKoufen"]);
                new_15dot1qiye.xmyql = Convert.ToDouble(item["projectLateRate"]);
                new_15dot1qiye.pxjnl = Convert.ToDouble(item["trainAndAbility"]);
                new_15dot1qiye.submitDepartment = Convert.ToString(item["zzId"]);
                new_15dot1qiye.temp3 = Convert.ToString(item["cjname"]);//车间名字存于temp3
                new_15dot1qiye.temp2 = "企业级";//专业名字存于temp2
                
                
                res = Jx1.ModifyJxItem(new_15dot1qiye);
            
            }
           
            return res;

        }


        public bool ModifyDong(string json1)
        {
            JObject item = (JObject)JsonConvert.DeserializeObject(json1);
            A15dot1TabDong new_15dot1dong = new A15dot1TabDong();
            string stime;
            string etime;
            bool res = false;

            if (Convert.ToString(item["zzId"]) == "全厂")
            {

                DateTime Cxtime = DateTime.Now;
                string time = item["month"].ToString();
                if (Convert.ToInt32(time) > 1)
                {
                    string Addtime = (Convert.ToInt32(time) + 1).ToString();
                    stime = Cxtime.Year.ToString() + '/' + time + '/' + "01" + " 00:00";
                    etime = Cxtime.Year.ToString() + '/' + Addtime + '/' + "01" + " 00:00";
                }
                else
                {
                    string Addtime = (Convert.ToInt32(time) + 1).ToString();
                    stime = (Cxtime.Year).ToString() + '/' + "01" + '/' + "01" + " 00:00";
                    etime = Cxtime.Year.ToString() + '/' + Addtime + '/' + "01" + " 00:00";
                }
                DateTime astime = Convert.ToDateTime(stime);
                DateTime aetime = Convert.ToDateTime(etime);

                double g = Convert.ToDouble(item["gzqdkf"].ToString().Trim());
                new_15dot1dong.gzqdkf = Convert.ToInt32(g);

                new_15dot1dong.djzgzl = Convert.ToDouble(item["djzgzl"]);
                new_15dot1dong.gzwxl = Convert.ToDouble(item["gzwxl"]);
                new_15dot1dong.qtlxbmfxhl = Convert.ToDouble(item["qtlxbmfxhl"]);
                new_15dot1dong.jjqxgsl = Convert.ToDouble(item["jjqxgsl"]);


                double b = Convert.ToDouble(item["gzqdkf"].ToString().Trim());
                new_15dot1dong.gzqdkf = Convert.ToInt32(b);
                new_15dot1dong.gdpjwcsj = Convert.ToDouble(item["gdpjwcsj"]);
                new_15dot1dong.jxmfpjsm = Convert.ToDouble(item["jxmfpjsm"]);
                new_15dot1dong.zcpjsm = Convert.ToDouble(item["zcpjsm"]);
                new_15dot1dong.sbwhl = Convert.ToDouble(item["sbwhl"]);
                new_15dot1dong.jxychgl = Convert.ToDouble(item["jxychgl"]);
                new_15dot1dong.zyjbpjxl = Convert.ToDouble(item["zyjbpjxl"]);
                new_15dot1dong.jzpjxl = Convert.ToDouble(item["jzpjxl"]);
                new_15dot1dong.wfjzgzl = Convert.ToDouble(item["wfjzgzl"]);
                new_15dot1dong.ndbtjbcfjxtc = Convert.ToDouble(item["ndbtjbcfjxtc"]);
                new_15dot1dong.jbpjwgzjgsjMTBF = Convert.ToDouble(item["jbpjwgzjgsjMTBF"]);
                new_15dot1dong.submitDepartment = Convert.ToString(item["zzId"]);
                new_15dot1dong.temp2 = "动设备专业";//专业名字存于temp2
                new_15dot1dong.submitUser = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
                new_15dot1dong.submitTime = DateTime.Now;
                res = Jx1.ModifyQcDongJxItem(new_15dot1dong, astime, aetime);

            }
            else
            {
                new_15dot1dong.gzqdkf = Convert.ToInt32(item["gzqdkf"]);
                new_15dot1dong.gdpjwcsj = Convert.ToDouble(item["gdpjwcsj"]);
                new_15dot1dong.jxmfpjsm = Convert.ToDouble(item["jxmfpjsm"]);
                new_15dot1dong.zcpjsm = Convert.ToDouble(item["zcpjsm"]);
                new_15dot1dong.sbwhl = Convert.ToDouble(item["sbwhl"]);
                new_15dot1dong.jxychgl = Convert.ToDouble(item["jxychgl"]);
                new_15dot1dong.Id = Convert.ToInt32(item["kpiid"]);
                new_15dot1dong.zyjbpjxl = Convert.ToDouble(item["zyjbpjxl"]);
                new_15dot1dong.jzpjxl = Convert.ToDouble(item["jzpjxl"]);
                new_15dot1dong.wfjzgzl = Convert.ToDouble(item["wfjzgzl"]);
                new_15dot1dong.ndbtjbcfjxtc = Convert.ToDouble(item["ndbtjbcfjxtc"]);
                new_15dot1dong.jbpjwgzjgsjMTBF = Convert.ToDouble(item["jbpjwgzjgsjMTBF"]);
                new_15dot1dong.submitDepartment = Convert.ToString(item["zzId"]);
                res = Jx1.ModifyJxItemDong(new_15dot1dong);

            }

            return res;
        }

        public bool ModifyJing(string json1)
        {
            JObject item = (JObject)JsonConvert.DeserializeObject(json1);
            A15dot1TabJing new_15dot1jing = new A15dot1TabJing();
            string stime;
            string etime;
            bool res = false;

            if (Convert.ToString(item["zzId"]) == "全厂")
            {

                DateTime Cxtime = DateTime.Now;
                string time = item["month"].ToString();
                if (Convert.ToInt32(time) > 1)
                {
                    string Addtime = (Convert.ToInt32(time) + 1).ToString();
                    stime = Cxtime.Year.ToString() + '/' + time + '/' + "01" + " 00:00";
                    etime = Cxtime.Year.ToString() + '/' + Addtime + '/' + "01" + " 00:00";
                }
                else
                {
                    string Addtime = (Convert.ToInt32(time) + 1).ToString();
                    stime = (Cxtime.Year).ToString() + '/' + "01" + '/' + "01" + " 00:00";
                    etime = Cxtime.Year.ToString() + '/' + Addtime + '/' + "01" + " 00:00";
                }
                DateTime astime = Convert.ToDateTime(stime);
                DateTime aetime = Convert.ToDateTime(etime);

                double g = Convert.ToDouble(item["gzqdkf"].ToString().Trim());
                new_15dot1jing.gzqdkf = Convert.ToInt16(g);

                double s = Convert.ToDouble(item["sbfsxlcs"]);
                new_15dot1jing.sbfsxlcs = Convert.ToInt16(s);

                new_15dot1jing.qtlhsbgs = Convert.ToDouble(item["qtlhsbgs"]);
                new_15dot1jing.gylpjrxl = Convert.ToDouble(item["gylpjrxl"]);
                new_15dot1jing.hrqjxl = Convert.ToDouble(item["hrqjxl"]);
                new_15dot1jing.ylrqdjl = Convert.ToDouble(item["ylrqdjl"]);
                new_15dot1jing.ylgdndjxjhwcl = Convert.ToDouble(item["ylgdndjxjhwcl"]);
                new_15dot1jing.aqfndjyjhwcl = Convert.ToDouble(item["aqfndjyjhwcl"]);
                new_15dot1jing.sbfsjcjhwcl = Convert.ToDouble(item["sbfsjcjhwcl"]);
                new_15dot1jing.jsbjwxychgl = Convert.ToDouble(item["jsbjwxychgl"]);
                new_15dot1jing.submitDepartment = Convert.ToString(item["zzId"]);
                new_15dot1jing.temp2 = "静设备专业";//专业名字存于temp2
                new_15dot1jing.submitUser = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
                new_15dot1jing.submitTime = DateTime.Now;
                res = Jx1.ModifyQcJingJxItem(new_15dot1jing, astime, aetime);

            }
            else
            {
                new_15dot1jing.gzqdkf = Convert.ToInt32(item["gzqdkf"]);
                new_15dot1jing.sbfsxlcs = Convert.ToInt32(item["sbfsxlcs"]);
                new_15dot1jing.qtlhsbgs = Convert.ToDouble(item["qtlhsbgs"]);
                new_15dot1jing.gylpjrxl = Convert.ToDouble(item["gylpjrxl"]);
                new_15dot1jing.hrqjxl = Convert.ToDouble(item["hrqjxl"]);
                new_15dot1jing.ylrqdjl = Convert.ToDouble(item["ylrqdjl"]);
                new_15dot1jing.Id = Convert.ToInt32(item["kpiid"]);
                new_15dot1jing.ylgdndjxjhwcl = Convert.ToDouble(item["ylgdndjxjhwcl"]);
                new_15dot1jing.aqfndjyjhwcl = Convert.ToDouble(item["aqfndjyjhwcl"]);
                new_15dot1jing.sbfsjcjhwcl = Convert.ToDouble(item["sbfsjcjhwcl"]);
                new_15dot1jing.jsbjwxychgl = Convert.ToDouble(item["jsbjwxychgl"]);
                new_15dot1jing.submitDepartment = Convert.ToString(item["zzId"]);
                new_15dot1jing.temp2 = "静设备专业";//专业名字存于temp2
                
               
                res = Jx1.ModifyJxItemJing(new_15dot1jing);

            }

            return res;
        }


        public bool ModifyDian(string json1)
        {
            JObject item = (JObject)JsonConvert.DeserializeObject(json1);
            A15dot1TabDian new_15dot1Dian = new A15dot1TabDian();
            string stime;
            string etime;
            bool res = false;

            if (Convert.ToString(item["zzId"]) == "全厂")
            {

                DateTime Cxtime = DateTime.Now;
                string time = item["month"].ToString();
                if (Convert.ToInt32(time) > 1)
                {
                    string Addtime = (Convert.ToInt32(time) + 1).ToString();
                    stime = Cxtime.Year.ToString() + '/' + time + '/' + "01" + " 00:00";
                    etime = Cxtime.Year.ToString() + '/' + Addtime + '/' + "01" + " 00:00";
                }
                else
                {
                    string Addtime = (Convert.ToInt32(time) + 1).ToString();
                    stime = (Cxtime.Year).ToString() + '/' + "01" + '/' + "01" + " 00:00";
                    etime = Cxtime.Year.ToString() + '/' + Addtime + '/' + "01" + " 00:00";
                }
                DateTime astime = Convert.ToDateTime(stime);
                DateTime aetime = Convert.ToDateTime(etime);

                double g = Convert.ToDouble(item["gzqdkf"].ToString().Trim());
                new_15dot1Dian.gzqdkf = Convert.ToInt16(g);

                double s = Convert.ToDouble(item["dqwczcs"]);
                new_15dot1Dian.dqwczcs = Convert.ToInt16(s);

                new_15dot1Dian.jdbhzqdzl = Convert.ToDouble(item["jdbhzqdzl"]);
                new_15dot1Dian.sbgzl = Convert.ToDouble(item["sbgzl"]);
                new_15dot1Dian.djMTBF = Convert.ToDouble(item["djMTBF"]);
                new_15dot1Dian.dzdlsbMTBF = Convert.ToDouble(item["dzdlsbMTBF"]);
                new_15dot1Dian.zbglys = Convert.ToDouble(item["zbglys"]);
                new_15dot1Dian.dnjlphl = Convert.ToDouble(item["dnjlphl"]);
                new_15dot1Dian.temp2 = "电气专业";//专业名字存于temp2
                new_15dot1Dian.submitUser = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
                new_15dot1Dian.submitTime = DateTime.Now;
                new_15dot1Dian.submitDepartment = Convert.ToString(item["zzId"]);

                res = Jx1.ModifyQcDianJxItem(new_15dot1Dian, astime, aetime);

            }
            else
            {
                new_15dot1Dian.gzqdkf = Convert.ToInt32(item["gzqdkf"]);
                new_15dot1Dian.dqwczcs = Convert.ToInt32(item["dqwczcs"]);
                new_15dot1Dian.jdbhzqdzl = Convert.ToDouble(item["jdbhzqdzl"]);
                new_15dot1Dian.sbgzl = Convert.ToDouble(item["sbgzl"]);
                new_15dot1Dian.djMTBF = Convert.ToDouble(item["djMTBF"]);
                new_15dot1Dian.dzdlsbMTBF = Convert.ToDouble(item["dzdlsbMTBF"]);
                new_15dot1Dian.Id = Convert.ToInt32(item["kpiid"]);
                new_15dot1Dian.zbglys = Convert.ToDouble(item["zbglys"]);
                new_15dot1Dian.dnjlphl = Convert.ToDouble(item["dnjlphl"]);
                new_15dot1Dian.temp2 = "电气专业";//专业名字存于temp2

                new_15dot1Dian.submitDepartment = Convert.ToString(item["zzId"]);

                res = Jx1.ModifyJxItemDian(new_15dot1Dian);

            }

            return res;
        }

        public bool ModifyYi(string json1)
        {
            JObject item = (JObject)JsonConvert.DeserializeObject(json1);
            A15dot1TabYi new_15dot1Yi = new A15dot1TabYi();
            string stime;
            string etime;
            bool res = false;

            if (Convert.ToString(item["zzId"]) == "全厂")
            {

                DateTime Cxtime = DateTime.Now;
                string time = item["month"].ToString();
                if (Convert.ToInt32(time) > 1)
                {
                    string Addtime = (Convert.ToInt32(time) + 1).ToString();
                    stime = Cxtime.Year.ToString() + '/' + time + '/' + "01" + " 00:00";
                    etime = Cxtime.Year.ToString() + '/' + Addtime + '/' + "01" + " 00:00";
                }
                else
                {
                    string Addtime = (Convert.ToInt32(time) + 1).ToString();
                    stime = (Cxtime.Year).ToString() + '/' + "01" + '/' + "01" + " 00:00";
                    etime = Cxtime.Year.ToString() + '/' + Addtime + '/' + "01" + " 00:00";
                }
                DateTime astime = Convert.ToDateTime(stime);
                DateTime aetime = Convert.ToDateTime(etime);

                double g = Convert.ToDouble(item["gzqdkf"].ToString().Trim());
                new_15dot1Yi.gzqdkf = Convert.ToInt16(g);

                new_15dot1Yi.lsxtzqdzl = Convert.ToDouble(item["lsxtzqdzl"]);

                double k = Convert.ToDouble(item["kzxtgzcs"]);
                new_15dot1Yi.kzxtgzcs = Convert.ToInt16(k);

                new_15dot1Yi.ybkzl = Convert.ToDouble(item["ybkzl"]);
                new_15dot1Yi.ybsjkzl = Convert.ToDouble(item["ybsjkzl"]);
                new_15dot1Yi.lsxttyl = Convert.ToDouble(item["lsxttyl"]);

                double s = Convert.ToDouble(item["gjkzfmgzcs"]);
                new_15dot1Yi.gjkzfmgzcs = Convert.ToInt16(s);

                double c = Convert.ToDouble(item["kzxtgzbjcs"]);
                new_15dot1Yi.kzxtgzbjcs = Convert.ToInt16(c);

                new_15dot1Yi.cgybgzl = Convert.ToDouble(item["cgybgzl"]);
                new_15dot1Yi.tjfMDBF = Convert.ToDouble(item["tjfMDBF"]);
                new_15dot1Yi.submitDepartment = Convert.ToString(item["zzId"]);

                new_15dot1Yi.temp2 = "仪表";//专业名字存于temp2
                new_15dot1Yi.submitUser = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
                new_15dot1Yi.submitTime = DateTime.Now;
                res = Jx1.ModifyQcYiJxItem(new_15dot1Yi, astime, aetime);

            }
            else
            {
                new_15dot1Yi.gzqdkf = Convert.ToInt32(item["gzqdkf"]);
                new_15dot1Yi.lsxtzqdzl = Convert.ToDouble(item["lsxtzqdzl"]);
                new_15dot1Yi.kzxtgzcs = Convert.ToInt32(item["kzxtgzcs"]);
                new_15dot1Yi.ybkzl = Convert.ToDouble(item["ybkzl"]);
                new_15dot1Yi.ybsjkzl = Convert.ToDouble(item["ybsjkzl"]);
                new_15dot1Yi.lsxttyl = Convert.ToDouble(item["lsxttyl"]);
                new_15dot1Yi.Id = Convert.ToInt32(item["kpiid"]);
                new_15dot1Yi.gjkzfmgzcs = Convert.ToInt32(item["gjkzfmgzcs"]);
                new_15dot1Yi.kzxtgzbjcs = Convert.ToInt32(item["kzxtgzbjcs"]);
                new_15dot1Yi.cgybgzl = Convert.ToDouble(item["cgybgzl"]);
                new_15dot1Yi.tjfMDBF = Convert.ToDouble(item["tjfMDBF"]);
                new_15dot1Yi.submitDepartment = Convert.ToString(item["zzId"]);

                new_15dot1Yi.temp2 = "仪表";//专业名字存于temp2


                res = Jx1.ModifyJxItemYi(new_15dot1Yi);

            }

            return res;
        }




        public ActionResult DongSubmit()
        {
            PersonManagment pm = new PersonManagment();
            submitmodel sm = new submitmodel();
            ViewBag.curtime = DateTime.Now;
            ViewBag.curuser = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
            sm.UserHasEquips = pm.Get_Person_Cj((Session["User"] as EquipModel.Entities.Person_Info).Person_Id);
            return View(sm);
        }

        public string qstqc(string grahpic_name, string graphic_zdm, string graphic_zy, string graphic_subdipartment)//获得全厂的趋势图
        {
            
            List<object> res = Jx1.qstqc(grahpic_name,  graphic_zdm, graphic_zy,  graphic_subdipartment);

            string str = JsonConvert.SerializeObject(res);

            return (str);

        }
        public bool DongSubmit_submitsignal(string json1)
        {

            JObject item = (JObject)JsonConvert.DeserializeObject(json1);
            A15dot1TabDong new_15dot1dong = new A15dot1TabDong();
            new_15dot1dong.gzqdkf = Convert.ToInt32(item["gzqdkf"]);
            new_15dot1dong.djzgzl = Convert.ToDouble(item["djzgzl"]);
            new_15dot1dong.gzwxl = Convert.ToDouble(item["gzwxl"]);
            new_15dot1dong.qtlxbmfxhl = Convert.ToDouble(item["qtlxbmfxhl"]);
            new_15dot1dong.jjqxgsl = Convert.ToDouble(item["jjqxgsl"]);
            new_15dot1dong.gdpjwcsj = Convert.ToDouble(item["gdpjwcsj"]);
            new_15dot1dong.jxmfpjsm = Convert.ToDouble(item["jxmfpjsm"]);
            new_15dot1dong.zcpjsm = Convert.ToDouble(item["zcpjsm"]);
            new_15dot1dong.sbwhl = Convert.ToDouble(item["sbwhl"]);
            new_15dot1dong.jxychgl = Convert.ToDouble(item["jxychgl"]);

            new_15dot1dong.zyjbpjxl = Convert.ToDouble(item["zyjbpjxl"]);
            new_15dot1dong.jzpjxl = Convert.ToDouble(item["jzpjxl"]);
            new_15dot1dong.wfjzgzl = Convert.ToDouble(item["wfjzgzl"]);
            new_15dot1dong.ndbtjbcfjxtc = Convert.ToDouble(item["ndbtjbcfjxtc"]);
            new_15dot1dong.jbpjwgzjgsjMTBF = Convert.ToDouble(item["jbpjwgzjgsjMTBF"]);

            new_15dot1dong.submitDepartment = item["zzId"].ToString();
            new_15dot1dong.temp3 = item["cjname"].ToString();//车间名字存于temp3
            new_15dot1dong.temp2 = "动设备专业";//专业名字存于temp2
            new_15dot1dong.submitUser = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
            new_15dot1dong.submitTime = DateTime.Now;
            bool res = Jx1.AddDongJxItem(new_15dot1dong);
            return res;

        }
        public string Dongqst(string grahpic_name, string zz)
        {


            List<object> res = Jx1.Dongqst(grahpic_name, zz);

            string str = JsonConvert.SerializeObject(res);

            return (str);

        }

        public List<object> getObjectDong(string time, string zzid, string zy)
        {
            List<object> r = new List<object>();
            EquipManagment em = new EquipManagment();
            string Zzid = "";
            if (zzid == "全厂")
            {
                Zzid = "全厂";
            }
            else
            {
                Zzid = em.getEa_namebyid(Convert.ToInt32(zzid));
            }

            if (Zzid != "全厂"&&Zzid!="")
            {
                List<A15dot1TabDong> res = Jx1.GetDongJxByA2(time, Zzid, zy);
                if (res.Count != 0)
                {
                    object a1 = new
                    {
                        zdm = "gzqdkf",
                        zblx = "KPI",
                        zbname = "故障强度扣分",
                        ndmbz = "45",
                        ljz = res[0].gzqdkf,
                        id = res[0].Id,
                        ActualValue = res[0].gzqdkf,

                    };
                    r.Add(a1);

                    object a2 = new
                    {
                        zdm = "djzgzl",
                        zblx = "KPI",
                        zbname = "大机组故障率",
                        id = res[0].Id,
                        ndmbz = "<0.6‰",
                        ljz = res[0].djzgzl,
                        ActualValue = res[0].djzgzl,
                    };
                    r.Add(a2);
                    object a3 = new
                    {
                        zdm = "gzwxl",
                        id = res[0].Id,
                        zblx = "KPI",
                        zbname = "故障维修率",
                        ndmbz = "<7%",
                        ljz = res[0].gzwxl,
                        ActualValue = res[0].gzwxl,
                    };
                    r.Add(a3);
                    object a9 = new
                    {
                        zdm = "qtlxbmfxhl",
                        id = res[0].Id,
                        zblx = "KPI",
                        zbname = "千台离心泵密封消耗率",
                        ndmbz = "<320",
                        ljz = res[0].qtlxbmfxhl,
                        ActualValue = res[0].qtlxbmfxhl,
                    };
                    r.Add(a9);
                    object a4 = new
                    {
                        zdm = "jjqxgsl",
                        id = res[0].Id,
                        zblx = "KPI",
                        zbname = "紧急抢修工时率",
                        ndmbz = "",
                        ljz = res[0].jjqxgsl,
                        ActualValue = res[0].jjqxgsl,
                    };
                    r.Add(a4);
                    object a5 = new
                    {
                        zdm = "gdpjwcsj",
                        id = res[0].Id,
                        zblx = "KPI",
                        zbname = "工单平均完成时间",
                        ndmbz = "",
                        ljz = res[0].gdpjwcsj,
                        ActualValue = res[0].gdpjwcsj,
                    };
                    r.Add(a5);
                    object a6 = new
                    {
                        zdm = "jxmfpjsm",
                        id = res[0].Id,
                        zblx = "KPI",
                        zbname = "机械密封平均寿命",
                        ndmbz = "20000",
                        ljz = res[0].jxmfpjsm,
                        ActualValue = res[0].jxmfpjsm,
                    };
                    r.Add(a6);
                    object a7 = new
                    {
                        zdm = "zcpjsm",
                        id = res[0].Id,
                        zblx = "KPI",
                        zbname = "轴承平均寿命",
                        ndmbz = "28000",
                        ljz = res[0].zcpjsm,
                        ActualValue = res[0].zcpjsm,
                    };
                    r.Add(a7);
                    object a8 = new
                    {
                        zdm = "sbwhl",
                        id = res[0].Id,
                        zblx = "KPI",
                        zbname = "设备完好率",
                        ndmbz = "98%",
                        ljz = res[0].sbwhl,
                        ActualValue = res[0].sbwhl,
                    };
                    r.Add(a8);
                    object a10 = new
                    {
                        zdm = "jxychgl",
                        id = res[0].Id,
                        zblx = "KPI",
                        zbname = "检修一次合格率",
                        ndmbz = "",
                        ljz = res[0].jxychgl,
                        ActualValue = res[0].jxychgl,
                    };
                    r.Add(a10);
                    object a11 = new
                    {
                        zdm = "zyjbpjxl",
                        id = res[0].Id,
                        zblx = "KPI",
                        zbname = "主要机泵平均效率",
                        ndmbz = "50%",
                        ljz = res[0].zyjbpjxl,
                        ActualValue = res[0].zyjbpjxl,
                    };
                    r.Add(a11);
                    object a12 = new
                    {
                        zdm = "jzpjxl",
                        id = res[0].Id,
                        zblx = "KPI",
                        zbname = "机组平均效率",
                        ndmbz = "",
                        ljz = res[0].jzpjxl,
                        ActualValue = res[0].jzpjxl,
                    };
                    r.Add(a12);
                    object a13 = new
                    {
                        zdm = "wfjzgzl",
                        id = res[0].Id,
                        zblx = "KPI",
                        zbname = "往复机组故障率",
                        ndmbz = "<0.15%",
                        ljz = res[0].wfjzgzl,
                        ActualValue = res[0].wfjzgzl,
                    };
                    r.Add(a13);
                    object a14 = new
                    {
                        zdm = "ndbtjbcfjxtc",
                        id = res[0].Id,
                        zblx = "KPI",
                        zbname = "年度百台机泵重复检修台次",
                        ndmbz = "<10台次/百台·年",
                        ljz = res[0].ndbtjbcfjxtc,
                        ActualValue = res[0].ndbtjbcfjxtc,
                    };
                    r.Add(a14);
                    object a15 = new
                    {
                        zdm = "jbpjwgzjgsjMTBF",
                        id = res[0].Id,
                        zblx = "KPI",
                        zbname = "机泵平均无故障间隔时间MTBF",
                        ndmbz = ">72月",
                        ljz = res[0].jbpjwgzjgsjMTBF,
                        ActualValue = res[0].jbpjwgzjgsjMTBF,
                    };
                    r.Add(a15);
                    return r;
                }
                else
                {
                    return r;
                }
            }
            else if(Zzid=="全厂")
            {
                List<object> qc = new List<object>();
                DateTime Cxtime = DateTime.Now;
                string stime;
                string etime;
                if (Convert.ToInt32(time) > 1)
                {
                    string Addtime = (Convert.ToInt32(time) + 1).ToString();
                    stime = Cxtime.Year.ToString() + '/' + time + '/' + "01" + " 00:00";
                    etime = Cxtime.Year.ToString() + '/' + Addtime + '/' + "01" + " 00:00";
                }
                else
                {
                    string Addtime = (Convert.ToInt32(time) + 1).ToString();
                    stime = (Cxtime.Year).ToString() + '/' + "01" + '/' + "01" + " 00:00";
                    etime = Cxtime.Year.ToString() + '/' + Addtime + '/' + "01" + " 00:00";
                }
                DateTime astime = Convert.ToDateTime(stime);
                DateTime aetime = Convert.ToDateTime(etime);
                qc = Jx1.GetQc(zy, astime, aetime);
                List<string> qc_list = new List<string>();
                foreach(var i in qc)
                {
                    if (i == null)
                        qc_list.Add("当月无装置提报该kpi");//如果全厂在数据库中没有值，qc_list赋值为空字符串
                    else
                        qc_list.Add(i.ToString());

                }
                if (qc.Count == 15)
                {
                    object a1 = new
                    {
                        zdm = "gzqdkf",
                        zblx = "KPI",
                        zbname = "故障强度扣分",
                        ndmbz = "45",
                        ljz = qc_list[0],
                        id = "0",
                        ActualValue = qc_list[0],

                    };
                    r.Add(a1);

                    object a2 = new
                    {
                        id = "0",
                        zdm = "djzgzl",
                        zblx = "KPI",
                        zbname = "大机组故障率",
                        // id = res[0].Id,
                        ndmbz = "<0.6‰",
                        ljz = qc_list[1],
                        //id = res[0].Id,
                        ActualValue = qc_list[1],
                    };
                    r.Add(a2);
                    object a3 = new
                    {
                        id = "0",
                        zdm = "gzwxl",
                        //  id = res[0].Id,
                        zblx = "KPI",
                        zbname = "故障维修率",
                        ndmbz = "<7%",
                        ljz = qc_list[2],
                        //id = res[0].Id,
                        ActualValue = qc_list[2],
                    };
                    r.Add(a3);
                    object a9 = new
                    {
                        id = "0",
                        zdm = "qtlxbmfxhl",
                        // id = res[3].Id,
                        zblx = "KPI",
                        zbname = "千台离心泵密封消耗率",
                        ndmbz = "<320",
                        ljz = qc_list[3],
                        //id = res[0].Id,
                        ActualValue = qc_list[3],
                    };
                    r.Add(a9);
                    object a4 = new
                    {
                        id = "0",
                        zdm = "jjqxgsl",
                        // id = res[0].Id,
                        zblx = "KPI",
                        zbname = "紧急抢修工时率",
                        ndmbz = "",
                        ljz = qc_list[4],
                        //id = res[0].Id,
                        ActualValue = qc_list[4],
                    };
                    r.Add(a4);
                    object a5 = new
                    {
                        id = "0",
                        zdm = "gdpjwcsj",
                        // id = res[0].Id,
                        zblx = "KPI",
                        zbname = "工单平均完成时间",
                        ndmbz = "",
                        ljz = qc_list[5],
                        //id = res[0].Id,
                        ActualValue = qc_list[5],
                    };
                    r.Add(a5);
                    object a6 = new
                    {
                        id = "0",
                        zdm = "jxmfpjsm",
                        //id = res[0].Id,
                        zblx = "KPI",
                        zbname = "机械密封平均寿命",
                        ndmbz = "20000",
                        ljz = qc_list[6],
                        //id = res[0].Id,
                        ActualValue = qc_list[6],
                    };
                    r.Add(a6);
                    object a7 = new
                    {
                        id = "0",
                        zdm = "zcpjsm",
                        //id = res[0].Id,
                        zblx = "KPI",
                        zbname = "轴承平均寿命",
                        ndmbz = "28000",
                        ljz = qc_list[7],
                        //id = res[0].Id,
                        ActualValue = qc_list[7],
                    };
                    r.Add(a7);
                    object a8 = new
                    {
                        id = "0",
                        zdm = "sbwhl",
                        // id = res[0].Id,
                        zblx = "KPI",
                        zbname = "设备完好率",
                        ndmbz = "98%",
                        ljz = qc_list[8],
                        //id = res[0].Id,
                        ActualValue = qc_list[8],
                    };
                    r.Add(a8);
                    object a10 = new
                    {
                        id = "0",
                        zdm = "jxychgl",
                        // id = res[0].Id,
                        zblx = "KPI",
                        zbname = "检修一次合格率",
                        ndmbz = "",
                        ljz = qc_list[9],
                        //id = res[0].Id,
                        ActualValue = qc_list[9],
                    };
                    r.Add(a10);
                    object a11 = new
                    {
                        id = "0",
                        zdm = "zyjbpjxl",
                        // id = res[0].Id,
                        zblx = "KPI",
                        zbname = "主要机泵平均效率",
                        ndmbz = "50%",
                        ljz = qc_list[10],
                        //id = res[0].Id,
                        ActualValue = qc_list[10],
                    };
                    r.Add(a11);
                    object a12 = new
                    {
                        id = "0",
                        zdm = "jzpjxl",
                        // id = res[0].Id,
                        zblx = "KPI",
                        zbname = "机组平均效率",
                        ndmbz = "",
                        ljz = qc_list[11],
                        //id = res[0].Id,
                        ActualValue = qc_list[11],
                    };
                    r.Add(a12);
                    object a13 = new
                    {
                        id = "0",
                        zdm = "wfjzgzl",
                        //  id = res[0].Id,
                        zblx = "KPI",
                        zbname = "往复机组故障率",
                        ndmbz = "<0.15%",
                        ljz = qc_list[12],
                        //id = res[0].Id,
                        ActualValue = qc_list[12],
                    };
                    r.Add(a13);
                    object a14 = new
                    {
                        id = "0",
                        zdm = "ndbtjbcfjxtc",
                        // id = res[0].Id,
                        zblx = "KPI",
                        zbname = "年度百台机泵重复检修台次",
                        ndmbz = "<10台次/百台·年",
                        ljz = qc_list[13],
                        //id = res[0].Id,
                        ActualValue = qc_list[13],
                    };
                    r.Add(a14);
                    object a15 = new
                    {
                        id = "0",
                        zdm = "jbpjwgzjgsjMTBF",
                        // id = res[0].Id,
                        zblx = "KPI",
                        zbname = "机泵平均无故障间隔时间MTBF",
                        ndmbz = ">72月",
                        ljz = qc_list[14],
                        //id = res[0].Id,
                        ActualValue = qc_list[14],
                    };
                    r.Add(a15);
                    return r;
                }
                else
                {
                    return r;

                }
            }
            else
            {
                return r;
            }
        }

    }
}
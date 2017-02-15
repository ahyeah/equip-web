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
using iTextSharp.text.pdf;
using iTextSharp.text;
using EquipBLL.AdminManagment.MenuConfig;

namespace WebApp.Controllers
{

    public class A15dot1DianController : CommonController
    {
        private KpiManagement Jx = new KpiManagement();
        public class submitmodel
        {
            public string timesNonPlanStop;//非计划停工次数
            public string scoreDeductFaultIntensity;//四类以上故障强度扣分
            public string rateBigUnitFault;//大机组故障率K
            public string rateFaultMaintenance;//故障维修率F
            public string MTBF;//一般机泵设备平均无故障间隔期MTBF
            public string rateEquipUse;  //  设备投用率R        
            public string avgLifeSpanSeal;//密封平均寿命S
            public string avgLifeSpanAxle;//轴承平均寿命B
            public string percentEquipAvailability;//设备完好率W
            public string percentPassOnetimeRepair;//检修一次合格率
            public List<Equip_Archi> UserHasEquips;

            public int kkxgcs;
            public int jwxry;
        }

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Submit()
        {
            submitmodel sm = new submitmodel();
            ViewBag.curtime = DateTime.Now;
            ViewBag.curuser = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
            int UserId = (Session["User"] as EquipModel.Entities.Person_Info).Person_Id;
            PersonManagment pm = new PersonManagment();
            sm.UserHasEquips = pm.Get_Person_Cj((Session["User"] as EquipModel.Entities.Person_Info).Person_Id);
            return View(sm);
        }

        public bool Submit_submitsignal(string json1)
        {

            JObject item = (JObject)JsonConvert.DeserializeObject(json1);
            A15dot1TabDian new_15dot1Dian = new A15dot1TabDian();
            new_15dot1Dian.gzqdkf = Convert.ToInt32(item["gzqdkf"].ToString());
            new_15dot1Dian.dqwczcs = Convert.ToInt32(item["dqwczcs"].ToString());
            new_15dot1Dian.sbgzl = Convert.ToDouble(item["sbgzl"].ToString());
            new_15dot1Dian.jdbhzqdzl = Convert.ToDouble(item["jdbhzqdzl"].ToString());
            new_15dot1Dian.djMTBF = Convert.ToDouble(item["djMTBF"].ToString());
            new_15dot1Dian.dzdlsbMTBF = Convert.ToDouble(item["dzdlsbMTBF"].ToString());
            new_15dot1Dian.zbglys = Convert.ToDouble(item["zbglys"].ToString());
            new_15dot1Dian.dnjlphl = Convert.ToDouble(item["dnjlphl"].ToString());
            new_15dot1Dian.temp3 = item["cjname"].ToString();
            new_15dot1Dian.submitDepartment = item["submitDepartment"].ToString();
            new_15dot1Dian.submitUser = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
            new_15dot1Dian.submitTime = DateTime.Now;
            new_15dot1Dian.state = 0;
            //new_15dot1.state = Convert.ToInt32(item["state"].ToString());
            new_15dot1Dian.reportType = item["reportType"].ToString();
            new_15dot1Dian.temp2 = "电气专业";
            bool res = Jx.AddDianItem(new_15dot1Dian);
            return res;
        }

        public ActionResult KkxSubmit(string id)
        {
            return View(getRecord_detail(id));
        }
        public class Index_ModelforA15
        {
            public List<A15dot1TabDian> Am;
            public List<A15dot1TabDian> Hm;
            public int isSubmit;
            public int kkxgcs;
            public int am;
            public int hm;
            public List<Equip_Archi> UserHasEquips;
        }

        public Index_ModelforA15 getRecord_detail(string id)
        {
            Index_ModelforA15 RecordforA15 = new Index_ModelforA15();
            RecordforA15.Am = new List<A15dot1TabDian>();
            //RecordforA15.Hm = new List<A15dot1Tab>();
            ViewBag.curtime = DateTime.Now.ToString();
            ViewBag.curuser = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
            int IntId = Convert.ToInt32(id);
            KpiManagement Jx = new KpiManagement();
            List<A15dot1TabDian> miss = Jx.GetJxItem_detailDian(IntId);
            foreach (var item in miss)
            {
                A15dot1TabDian a = new A15dot1TabDian();
                //a.timesNonPlanStop = item.timesNonPlanStop;
                a.gzqdkf = item.gzqdkf;
                a.dqwczcs = item.dqwczcs;
                a.sbgzl = item.sbgzl;
                a.jdbhzqdzl = item.jdbhzqdzl;
                a.djMTBF = item.djMTBF;
                a.dzdlsbMTBF = item.dzdlsbMTBF;
                a.zbglys = item.zbglys;
                a.reportType = item.reportType;
                a.dnjlphl = item.dnjlphl;
                a.submitDepartment = item.submitDepartment;
                a.submitUser = item.submitUser;
                a.temp2 = item.temp2;
                a.temp3 = item.temp3;
                a.Id = item.Id;

                RecordforA15.Am.Add(a);
            }
            string name = RecordforA15.Am[0].submitUser;
            PersonManagment pm = new PersonManagment();
            int UserId = pm.Get_Person(name).Person_Id;
            EquipBLL.AdminManagment.PersonManagment.P_viewModal pv = pm.Get_PersonModal(UserId);
            if (pv.Role_Names.Contains("可靠性工程师"))
                RecordforA15.am = 1;
            else
                RecordforA15.am = 0;
            //    List<A15dot1TabDian> His = Jx.GetHisJxItem_detail(IntId);
            //    foreach (var item in His)
            //    {
            //        A15dot1Tab a = new A15dot1Tab();
            //        a.timesNonPlanStop = item.timesNonPlanStop;
            //        a.scoreDeductFaultIntensity = item.scoreDeductFaultIntensity;
            //        a.rateBigUnitFault = item.rateBigUnitFault;
            //        a.rateFaultMaintenance = item.rateFaultMaintenance;
            //        a.MTBF = item.MTBF;
            //        a.rateEquipUse = item.rateEquipUse;
            //        a.rateUrgentRepairWorkHour = item.rateUrgentRepairWorkHour;
            //        a.hourWorkOrderFinish = item.hourWorkOrderFinish;
            //        a.avgLifeSpanSeal = item.avgLifeSpanSeal;
            //        a.avgLifeSpanAxle = item.avgLifeSpanAxle;
            //        a.percentEquipAvailability = item.percentEquipAvailability;
            //        a.percentPassOnetimeRepair = item.percentPassOnetimeRepair;
            //        a.avgEfficiencyPump = item.avgEfficiencyPump;
            //        a.avgEfficiencyUnit = item.avgEfficiencyUnit;
            //        a.hiddenDangerInvestigation = item.hiddenDangerInvestigation;
            //        a.rateLoad = item.rateLoad;
            //        a.gyChange = item.gyChange;
            //        a.equipChange = item.equipChange;
            //        a.otherDescription = item.otherDescription;
            //        a.evaluateEquipRunStaeDesc = item.evaluateEquipRunStaeDesc;
            //        a.evaluateEquipRunStaeImgPath = item.evaluateEquipRunStaeImgPath;
            //        a.reliabilityConclusion = item.reliabilityConclusion;
            //        a.jdcAdviseImproveMeasures = item.jdcAdviseImproveMeasures;
            //        a.submitDepartment = item.submitDepartment;
            //        a.submitUser = item.submitUser;
            //        a.submitTime = item.submitTime;
            //        a.jdcOperator = item.jdcOperator;
            //        a.jdcOperateTime = item.jdcOperateTime;
            //        a.reportType = item.reportType;
            //        a.temp1 = Convert.ToString(miss.IndexOf(item) + 1);
            //        a.submitUser = item.submitUser;
            //        a.submitTime = item.submitTime;
            //        a.state = item.state;
            //        a.Id = item.Id;
            //        //增加变量--参与统计台数
            //        a.Count_timesNonPlanStop = item.Count_timesNonPlanStop;
            //        a.Count_scoreDeductFaultIntensity = item.Count_scoreDeductFaultIntensity;
            //        a.Count_rateBigUnitFault = item.Count_rateBigUnitFault;
            //        a.Count_rateFaultMaintenance = item.Count_rateFaultMaintenance;
            //        a.Count_MTBF = item.Count_MTBF;
            //        a.Count_rateEquipUse = item.Count_rateEquipUse;
            //        a.Count_avgLifeSpanSeal = item.Count_avgLifeSpanSeal;
            //        a.Count_avgLifeSpanAxle = item.Count_avgLifeSpanAxle;
            //        a.Count_percentEquipAvailability = item.Count_percentEquipAvailability;
            //        a.Count_percentPassOnetimeRepair = item.Count_percentPassOnetimeRepair;
            //        RecordforA15.Hm.Add(a);
            //    }
            //    name = RecordforA15.Hm[0].submitUser;
            //    UserId = pm.Get_Person(name).Person_Id;
            //    pv = pm.Get_PersonModal(UserId);
            //    if (pv.Role_Names.Contains("可靠性工程师"))
            //        RecordforA15.hm = 1;
            //    else
            //        RecordforA15.hm = 0;
            return RecordforA15;
        }

        //public JsonResult Modify_submitsignal(string json1)
        //{

        //    JObject item = (JObject)JsonConvert.DeserializeObject(json1);
        //    A15dot1TabDian new_15dot1Dian = new A15dot1TabDian();
        //    new_15dot1Dian.Id = Convert.ToInt32(item["A15dot1Dian_id"].ToString());
        //    new_15dot1Dian.gzqdkf = Convert.ToInt32(item["gzqdkf"].ToString());
        //    new_15dot1Dian.dqwczcs = Convert.ToInt32(item["dqwczcs"].ToString());
        //    new_15dot1Dian.sbgzl = Convert.ToDouble(item["sbgzl"].ToString());
        //    new_15dot1Dian.jdbhzqdzl = Convert.ToDouble(item["jdbhzqdzl"].ToString());
        //    new_15dot1Dian.djMTBF = Convert.ToDouble(item["djMTBF"].ToString());
        //    new_15dot1Dian.dzdlsbMTBF = Convert.ToDouble(item["dzdlsbMTBF"].ToString());
        //    new_15dot1Dian.zbglys = Convert.ToDouble(item["zbglys"].ToString());
        //    new_15dot1Dian.dnjlphl = Convert.ToDouble(item["dnjlphl"].ToString());
        //    new_15dot1Dian.state = Convert.ToInt32(item["state"].ToString());
        //    new_15dot1Dian.reliabilityConclusion = item["reliabilityConclusion"].ToString();
        //    new_15dot1Dian.temp3 = item["cjname"].ToString();
        //    new_15dot1Dian.submitDepartment = item["submitDepartment"].ToString();
        //    new_15dot1Dian.submitUser = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
        //    new_15dot1Dian.submitTime = DateTime.Now;
        //    new_15dot1Dian.reportType = item["reportType"].ToString();

        //    bool res = Jx.ModifyJxItem(new_15dot1Dian);
        //    return Json(new { result = res });

        //}

        public string qst(string grahpic_name, string zzname)
        {


            List<object> res = Jx.qst(grahpic_name, zzname);

            string str = JsonConvert.SerializeObject(res);

            return (str);

        }
    }
}
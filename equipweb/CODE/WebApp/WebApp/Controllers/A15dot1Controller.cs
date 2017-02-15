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
    public class A15dot1Controller : CommonController
    {
        private JxpgManagment Jx = new JxpgManagment();
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

           public int kkxgcs;
           public int jwxry;
        
        }
        public ActionResult Index()
        {
            return View(getRecord());
        }
        public ActionResult Submit()
        {
            submitmodel sm = new submitmodel();
            ViewBag.curtime = DateTime.Now;
            ViewBag.curuser = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
            int UserId = (Session["User"] as EquipModel.Entities.Person_Info).Person_Id;
            PersonManagment pm = new PersonManagment();
            EquipBLL.AdminManagment.PersonManagment.P_viewModal pv = pm.Get_PersonModal(UserId);
            if (pv.Role_Names.Contains("可靠性工程师"))
                sm.kkxgcs = 1;
            if (pv.Role_Names.Contains("检维修人员"))
                sm.jwxry = 1;
            return View(sm);
        }
        public ActionResult PqSubmit(string id)
        {
            return View(getRecord_detail(id));
        }
        public ActionResult JdcSubmit(string id)
        {
            return View(getRecord_detail(id));
        }

        public ActionResult History(string id)
        {
            return View(getRecord_detail(id));
        }
      
      
        public class Index_ModelforA15
        {
            public List<A15dot1Tab> Am;
            public List<A15dot1Tab> Hm;
            public int isSubmit;
            public int kkxgcs;
            public int am;
            public int hm;
        }

        public Index_ModelforA15 getRecord()
        {
            Index_ModelforA15 RecordforA15 = new Index_ModelforA15();
            //ViewBag.curtime = DateTime.Now.ToString();
            int UserId = (Session["User"] as EquipModel.Entities.Person_Info).Person_Id;
            PersonManagment pm = new PersonManagment();
            EquipBLL.AdminManagment.PersonManagment.P_viewModal pv = pm.Get_PersonModal(UserId);
            RecordforA15.Am=new List<A15dot1Tab>();
            if (pv.Role_Names.Contains("可靠性工程师") || pv.Role_Names.Contains("检维修人员"))
                RecordforA15.isSubmit = 1;
            else
                RecordforA15.isSubmit = 0;
            if (pv.Role_Names.Contains("可靠性工程师"))
                RecordforA15.kkxgcs = 1;
            else
                RecordforA15.kkxgcs = 0;
            List<string> cjname = new List<string>();
            List<Equip_Archi> EA = pm.Get_Person_Cj(UserId);
            foreach (var ea in EA)
            {
                cjname.Add(ea.EA_Name);
            }

            List<A15dot1Tab> miss = Jx.GetJxItem(pv.Role_Names,pv.Department_Name,pv.Person_Name);
            foreach (var item in miss)
            {
                A15dot1Tab a = new A15dot1Tab();
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
                a.hiddenDangerInvestigation = item.hiddenDangerInvestigation;
                a.rateLoad = item.rateLoad;
                a.gyChange = item.gyChange;
                a.equipChange = item.equipChange;
                a.otherDescription = item.otherDescription;
                a.evaluateEquipRunStaeDesc = item.evaluateEquipRunStaeDesc;
                a.evaluateEquipRunStaeImgPath = item.evaluateEquipRunStaeImgPath;
                a.reliabilityConclusion = item.reliabilityConclusion;
                a.jdcAdviseImproveMeasures = item.jdcAdviseImproveMeasures;
                a.submitDepartment = item.submitDepartment;
                a.submitUser = item.submitUser;
                a.submitTime = item.submitTime;
                a.jdcOperator = item.jdcOperator;
                a.jdcOperateTime = item.jdcOperateTime;
                a.reportType = item.reportType;
                a.temp1 = Convert.ToString(miss.IndexOf(item) + 1);
                a.submitUser = item.submitUser;
                a.submitTime = item.submitTime;
                a.state = item.state;
                a.Id = item.Id;
                RecordforA15.Am.Add(a);
            }
            
            RecordforA15.Hm = new List<A15dot1Tab>();
            List<A15dot1Tab> His = Jx.GetHisJxItem(pv.Role_Names, pv.Department_Name, pv.Person_Name);
            foreach (var item in His)
            {
                A15dot1Tab a = new A15dot1Tab();
                a.Id = item.Id;
                a.state = item.state;
                a.jdcOperateTime = item.jdcOperateTime;
                a.jdcOperator = item.jdcOperator;
                a.temp1 = Convert.ToString(His.IndexOf(item) + 1);
                RecordforA15.Hm.Add(a);
            }

            return RecordforA15;
        }
        public Index_ModelforA15 getRecord_detail(string id)
        {
            Index_ModelforA15 RecordforA15 = new Index_ModelforA15();
            RecordforA15.Am = new List<A15dot1Tab>();
            RecordforA15.Hm = new List<A15dot1Tab>();
            ViewBag.curtime = DateTime.Now.ToString();
            ViewBag.curuser = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
            int IntId = Convert.ToInt32(id);
            List<A15dot1Tab> miss = Jx.GetJxItem_detail(IntId);
            foreach (var item in miss)
            {
                A15dot1Tab a = new A15dot1Tab();
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
                a.hiddenDangerInvestigation = item.hiddenDangerInvestigation;
                a.rateLoad = item.rateLoad;
                a.gyChange = item.gyChange;
                a.equipChange = item.equipChange;
                a.otherDescription = item.otherDescription;
                a.evaluateEquipRunStaeDesc = item.evaluateEquipRunStaeDesc;
                a.evaluateEquipRunStaeImgPath = item.evaluateEquipRunStaeImgPath;
                a.reliabilityConclusion = item.reliabilityConclusion;
                a.jdcAdviseImproveMeasures = item.jdcAdviseImproveMeasures;
                a.submitDepartment = item.submitDepartment;
                a.submitUser = item.submitUser;
                a.submitTime = item.submitTime;
                a.jdcOperator = item.jdcOperator;
                a.jdcOperateTime = item.jdcOperateTime;
                a.reportType = item.reportType;
                a.temp1 = Convert.ToString(miss.IndexOf(item) + 1);
                a.submitUser = item.submitUser;
                a.submitTime = item.submitTime;
                a.state = item.state;
                a.Id = item.Id;
                //增加变量--参与统计台数
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
            List<A15dot1Tab> His = Jx.GetHisJxItem_detail(IntId);
            foreach (var item in His)
            {
                A15dot1Tab a = new A15dot1Tab();
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
                a.hiddenDangerInvestigation = item.hiddenDangerInvestigation;
                a.rateLoad = item.rateLoad;
                a.gyChange = item.gyChange;
                a.equipChange = item.equipChange;
                a.otherDescription = item.otherDescription;
                a.evaluateEquipRunStaeDesc = item.evaluateEquipRunStaeDesc;
                a.evaluateEquipRunStaeImgPath = item.evaluateEquipRunStaeImgPath;
                a.reliabilityConclusion = item.reliabilityConclusion;
                a.jdcAdviseImproveMeasures = item.jdcAdviseImproveMeasures;
                a.submitDepartment = item.submitDepartment;
                a.submitUser = item.submitUser;
                a.submitTime = item.submitTime;
                a.jdcOperator = item.jdcOperator;
                a.jdcOperateTime = item.jdcOperateTime;
                a.reportType = item.reportType;
                a.temp1 = Convert.ToString(miss.IndexOf(item) + 1);
                a.submitUser = item.submitUser;
                a.submitTime = item.submitTime;
                a.state = item.state;
                a.Id = item.Id;
                //增加变量--参与统计台数
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
                RecordforA15.Hm.Add(a);
            }
            name = RecordforA15.Hm[0].submitUser;
            UserId = pm.Get_Person(name).Person_Id;
            pv = pm.Get_PersonModal(UserId);
            if (pv.Role_Names.Contains("可靠性工程师"))
                RecordforA15.hm = 1;
            else
                RecordforA15.hm = 0;
            return RecordforA15;
        }

        public JsonResult Submit_submitsignal(string json1)
        {
            
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);               
                A15dot1Tab new_15dot1 = new A15dot1Tab();                                             
                new_15dot1.timesNonPlanStop = Convert.ToInt32(item["timesNonPlanStop"].ToString());
                new_15dot1.scoreDeductFaultIntensity = Convert.ToInt32(item["scoreDeductFaultIntensity"].ToString());
                new_15dot1.rateBigUnitFault = Convert.ToDouble(item["scoreDeductFaultIntensity"].ToString());
                new_15dot1.rateFaultMaintenance = Convert.ToDouble(item["rateFaultMaintenance"].ToString());
                new_15dot1.MTBF = Convert.ToInt32(item["MTBF"].ToString());
                new_15dot1.rateEquipUse = Convert.ToDouble(item["rateEquipUse"].ToString());
                if (item["rateUrgentRepairWorkHour"].ToString() != "")
                { 
                new_15dot1.rateUrgentRepairWorkHour = Convert.ToDouble(item["rateUrgentRepairWorkHour"].ToString());
                }
                if (item["hourWorkOrderFinish"].ToString() != "")
                {
                    new_15dot1.hourWorkOrderFinish = Convert.ToDouble(item["hourWorkOrderFinish"].ToString());
                }
                new_15dot1.avgLifeSpanSeal = Convert.ToInt32(item["avgLifeSpanSeal"].ToString());
                new_15dot1.avgLifeSpanAxle = Convert.ToInt32(item["avgLifeSpanAxle"].ToString());
                new_15dot1.percentEquipAvailability = Convert.ToDouble(item["percentEquipAvailability"].ToString());
                new_15dot1.percentPassOnetimeRepair = Convert.ToDouble(item["percentPassOnetimeRepair"].ToString());

                new_15dot1.Count_timesNonPlanStop = Convert.ToInt32(item["Count_timesNonPlanStop"].ToString());
                new_15dot1.Count_scoreDeductFaultIntensity = Convert.ToInt32(item["Count_scoreDeductFaultIntensity"].ToString());
                new_15dot1.Count_rateBigUnitFault = Convert.ToInt32(item["Count_rateBigUnitFault"].ToString());
                new_15dot1.Count_rateFaultMaintenance = Convert.ToInt32(item["Count_rateFaultMaintenance"].ToString());
                new_15dot1.Count_MTBF = Convert.ToInt32(item["Count_MTBF"].ToString());
                new_15dot1.Count_rateEquipUse = Convert.ToInt32(item["Count_rateEquipUse"].ToString());
                new_15dot1.Count_avgLifeSpanSeal = Convert.ToInt32(item["Count_avgLifeSpanSeal"].ToString());
                new_15dot1.Count_avgLifeSpanAxle = Convert.ToInt32(item["Count_avgLifeSpanAxle"].ToString());
                new_15dot1.Count_percentEquipAvailability = Convert.ToInt32(item["Count_percentEquipAvailability"].ToString());
                new_15dot1.Count_percentPassOnetimeRepair = Convert.ToInt32(item["Count_percentPassOnetimeRepair"].ToString());

                if (item["avgEfficiencyPump"].ToString() != "")
                {
                    new_15dot1.avgEfficiencyPump = Convert.ToDouble(item["avgEfficiencyPump"].ToString());
                }
                if (item["avgEfficiencyUnit"].ToString() != "")
                {
                    new_15dot1.avgEfficiencyUnit = Convert.ToDouble(item["avgEfficiencyUnit"].ToString());
                }
                new_15dot1.hiddenDangerInvestigation = item["hiddenDangerInvestigation"].ToString();
                new_15dot1.rateLoad = item["rateLoad"].ToString();
                new_15dot1.gyChange = item["gyChange"].ToString();
                new_15dot1.equipChange = item["equipChange"].ToString();
                new_15dot1.otherDescription = item["otherDescription"].ToString();
                new_15dot1.evaluateEquipRunStaeDesc = item["evaluateEquipRunStaeDesc"].ToString();
                new_15dot1.evaluateEquipRunStaeImgPath = item["evaluateEquipRunStaeImgPath"].ToString();
                new_15dot1.reliabilityConclusion = item["reliabilityConclusion"].ToString();
                new_15dot1.jdcAdviseImproveMeasures = "";
                new_15dot1.submitDepartment = item["submitDepartment"].ToString();
                new_15dot1.submitUser = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
                new_15dot1.submitTime = DateTime.Now;
                new_15dot1.jdcOperator = "";
                
                //new_15dot1.state = Convert.ToInt32(item["state"].ToString());
                new_15dot1.reportType = item["reportType"].ToString();
                bool res = Jx.AddJxItem(new_15dot1);
                return Json(new { result = res });
                
            }
        public JsonResult DSSubmit_submitsignal(string json1)
        {

            JObject item = (JObject)JsonConvert.DeserializeObject(json1);
            A15dot1Tab new_15dot1 = new A15dot1Tab();
            new_15dot1.timesNonPlanStop = Convert.ToInt32(item["timesNonPlanStop"].ToString());
            new_15dot1.scoreDeductFaultIntensity = Convert.ToInt32(item["scoreDeductFaultIntensity"].ToString());
            new_15dot1.rateBigUnitFault = Convert.ToDouble(item["scoreDeductFaultIntensity"].ToString());
            new_15dot1.rateFaultMaintenance = Convert.ToDouble(item["rateFaultMaintenance"].ToString());
            new_15dot1.MTBF = Convert.ToInt32(item["MTBF"].ToString());
            new_15dot1.rateEquipUse = Convert.ToDouble(item["rateEquipUse"].ToString());
            if (item["rateUrgentRepairWorkHour"].ToString() != "")
            {
                new_15dot1.rateUrgentRepairWorkHour = Convert.ToDouble(item["rateUrgentRepairWorkHour"].ToString());
            }
            if (item["hourWorkOrderFinish"].ToString() != "")
            {
                new_15dot1.hourWorkOrderFinish = Convert.ToDouble(item["hourWorkOrderFinish"].ToString());
            }
            new_15dot1.avgLifeSpanSeal = Convert.ToInt32(item["avgLifeSpanSeal"].ToString());
            new_15dot1.avgLifeSpanAxle = Convert.ToInt32(item["avgLifeSpanAxle"].ToString());
            new_15dot1.percentEquipAvailability = Convert.ToDouble(item["percentEquipAvailability"].ToString());
            new_15dot1.percentPassOnetimeRepair = Convert.ToDouble(item["percentPassOnetimeRepair"].ToString());

            new_15dot1.Count_timesNonPlanStop = Convert.ToInt32(item["Count_timesNonPlanStop"].ToString());
            new_15dot1.Count_scoreDeductFaultIntensity = Convert.ToInt32(item["Count_scoreDeductFaultIntensity"].ToString());
            new_15dot1.Count_rateBigUnitFault = Convert.ToInt32(item["Count_rateBigUnitFault"].ToString());
            new_15dot1.Count_rateFaultMaintenance = Convert.ToInt32(item["Count_rateFaultMaintenance"].ToString());
            new_15dot1.Count_MTBF = Convert.ToInt32(item["Count_MTBF"].ToString());
            new_15dot1.Count_rateEquipUse = Convert.ToInt32(item["Count_rateEquipUse"].ToString());
            new_15dot1.Count_avgLifeSpanSeal = Convert.ToInt32(item["Count_avgLifeSpanSeal"].ToString());
            new_15dot1.Count_avgLifeSpanAxle = Convert.ToInt32(item["Count_avgLifeSpanAxle"].ToString());
            new_15dot1.Count_percentEquipAvailability = Convert.ToInt32(item["Count_percentEquipAvailability"].ToString());
            new_15dot1.Count_percentPassOnetimeRepair = Convert.ToInt32(item["Count_percentPassOnetimeRepair"].ToString());

            if (item["avgEfficiencyPump"].ToString() != "")
            {
                new_15dot1.avgEfficiencyPump = Convert.ToDouble(item["avgEfficiencyPump"].ToString());
            }
            if (item["avgEfficiencyUnit"].ToString() != "")
            {
                new_15dot1.avgEfficiencyUnit = Convert.ToDouble(item["avgEfficiencyUnit"].ToString());
            }
            new_15dot1.hiddenDangerInvestigation = item["hiddenDangerInvestigation"].ToString();
            new_15dot1.rateLoad = item["rateLoad"].ToString();
            new_15dot1.gyChange = item["gyChange"].ToString();
            new_15dot1.equipChange = item["equipChange"].ToString();
            new_15dot1.otherDescription = item["otherDescription"].ToString();
            new_15dot1.evaluateEquipRunStaeDesc = item["evaluateEquipRunStaeDesc"].ToString();
            new_15dot1.evaluateEquipRunStaeImgPath = item["evaluateEquipRunStaeImgPath"].ToString();
            new_15dot1.reliabilityConclusion = item["reliabilityConclusion"].ToString();
            new_15dot1.jdcAdviseImproveMeasures = "";
            new_15dot1.submitDepartment = item["submitDepartment"].ToString();
            new_15dot1.submitUser = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
            new_15dot1.submitTime = DateTime.Now;
            new_15dot1.jdcOperator = "";

            //new_15dot1.state = Convert.ToInt32(item["state"].ToString());
            new_15dot1.reportType = item["reportType"].ToString();
            bool res = Jx.AddJxItem(new_15dot1);
            string flowname = item["Flow_Name"].ToString();
            Dictionary<string, string> signal = new Dictionary<string, string>();
            signal["ZzSubmit_done"] = "true";
            //record
            Dictionary<string, string> record = new Dictionary<string, string>();
            record["username"] = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
            record["time"] = DateTime.Now.ToString();
            //submit
            CWFEngine.SubmitSignal(Convert.ToInt32(flowname), signal, record);
            return Json(new { result = res });

        }
        public JsonResult Modify_submitsignal(string json1)
        {

            JObject item = (JObject)JsonConvert.DeserializeObject(json1);
            A15dot1Tab new_15dot1 = new A15dot1Tab();
            new_15dot1.Id = Convert.ToInt32(item["A15dot1_id"].ToString());
            new_15dot1.timesNonPlanStop = Convert.ToInt32(item["timesNonPlanStop"].ToString());
            new_15dot1.scoreDeductFaultIntensity = Convert.ToInt32(item["scoreDeductFaultIntensity"].ToString());
            new_15dot1.rateBigUnitFault = Convert.ToDouble(item["scoreDeductFaultIntensity"].ToString());
            new_15dot1.rateFaultMaintenance = Convert.ToDouble(item["rateFaultMaintenance"].ToString());
            new_15dot1.MTBF = Convert.ToInt32(item["MTBF"].ToString());
            new_15dot1.rateEquipUse = Convert.ToDouble(item["rateEquipUse"].ToString());
            if (item["rateUrgentRepairWorkHour"].ToString() != "")
            {
                new_15dot1.rateUrgentRepairWorkHour = Convert.ToDouble(item["rateUrgentRepairWorkHour"].ToString());
            }
            else
                new_15dot1.rateUrgentRepairWorkHour = 0.0;
            if (item["hourWorkOrderFinish"].ToString() != "")
            {
                new_15dot1.hourWorkOrderFinish = Convert.ToDouble(item["hourWorkOrderFinish"].ToString());
            }
            new_15dot1.hourWorkOrderFinish = 0.0;

            new_15dot1.avgLifeSpanSeal = Convert.ToInt32(item["avgLifeSpanSeal"].ToString());
            new_15dot1.avgLifeSpanAxle = Convert.ToInt32(item["avgLifeSpanAxle"].ToString());
            new_15dot1.percentEquipAvailability = Convert.ToDouble(item["percentEquipAvailability"].ToString());
            new_15dot1.percentPassOnetimeRepair = Convert.ToDouble(item["percentPassOnetimeRepair"].ToString());
            if (item["avgEfficiencyPump"].ToString() != "")
            {
                new_15dot1.avgEfficiencyPump = Convert.ToDouble(item["avgEfficiencyPump"].ToString());
            }
            new_15dot1.avgEfficiencyPump = 0.0;
            if (item["avgEfficiencyUnit"].ToString() != "")
            {
                new_15dot1.avgEfficiencyUnit = Convert.ToDouble(item["avgEfficiencyUnit"].ToString());
            }
            new_15dot1.avgEfficiencyUnit = 0.0;

            new_15dot1.Count_timesNonPlanStop = Convert.ToInt32(item["Count_timesNonPlanStop"].ToString());
            new_15dot1.Count_scoreDeductFaultIntensity = Convert.ToInt32(item["Count_scoreDeductFaultIntensity"].ToString());
            new_15dot1.Count_rateBigUnitFault = Convert.ToInt32(item["Count_rateBigUnitFault"].ToString());
            new_15dot1.Count_rateFaultMaintenance = Convert.ToInt32(item["Count_rateFaultMaintenance"].ToString());
            new_15dot1.Count_MTBF = Convert.ToInt32(item["Count_MTBF"].ToString());
            new_15dot1.Count_rateEquipUse = Convert.ToInt32(item["Count_rateEquipUse"].ToString());
            new_15dot1.Count_avgLifeSpanSeal = Convert.ToInt32(item["Count_avgLifeSpanSeal"].ToString());
            new_15dot1.Count_avgLifeSpanAxle = Convert.ToInt32(item["Count_avgLifeSpanAxle"].ToString());
            new_15dot1.Count_percentEquipAvailability = Convert.ToInt32(item["Count_percentEquipAvailability"].ToString());
            new_15dot1.Count_percentPassOnetimeRepair = Convert.ToInt32(item["Count_percentPassOnetimeRepair"].ToString());

            new_15dot1.hiddenDangerInvestigation = item["hiddenDangerInvestigation"].ToString();
            new_15dot1.rateLoad = item["rateLoad"].ToString();
            new_15dot1.gyChange = item["gyChange"].ToString();
            new_15dot1.equipChange = item["equipChange"].ToString();
            new_15dot1.otherDescription = item["otherDescription"].ToString();
            new_15dot1.evaluateEquipRunStaeDesc = item["evaluateEquipRunStaeDesc"].ToString();
            new_15dot1.evaluateEquipRunStaeImgPath = item["evaluateEquipRunStaeImgPath"].ToString();
            new_15dot1.reliabilityConclusion = item["reliabilityConclusion"].ToString();
            new_15dot1.jdcAdviseImproveMeasures = "";
            new_15dot1.submitDepartment = item["submitDepartment"].ToString();
            new_15dot1.submitUser  = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
            new_15dot1.submitTime = DateTime.Now;
            new_15dot1.state = Convert.ToInt32(item["state"].ToString());
            new_15dot1.reportType = item["reportType"].ToString();
            bool res = Jx.ModifyJxItem(new_15dot1);
            return Json(new { result = res });

        }
        public JsonResult JdcModify_submitsignal(string json1)
        {

            JObject item = (JObject)JsonConvert.DeserializeObject(json1);
            A15dot1Tab new_15dot1 = new A15dot1Tab();
            new_15dot1.Id = Convert.ToInt32(item["A15dot1_id"].ToString());
            new_15dot1.jdcAdviseImproveMeasures = item["jdcAdviseImproveMeasures"].ToString();
            new_15dot1.jdcOperator = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
            new_15dot1.jdcOperateTime = DateTime.Now;
            new_15dot1.state = Convert.ToInt32(item["state"].ToString());
           
            bool res = Jx.JdcModifyJxItem(new_15dot1);
            return Json(new { result = res });

        
        }

        public string qst(string grahpic_name,string pianqu)
        {


            List<object> res = Jx.qst(grahpic_name, pianqu);

            string str = JsonConvert.SerializeObject(res);

            return (str);

        }
     
          public string out_pdf(string json1)
        {
    //  BaseFont bfchinese = BaseFont.CreateFont(@"c:\windows\fonts\simkai.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);//使用c盘系统中的字体，中文

      string fontpath = System.Web.HttpContext.Current.Request.PhysicalApplicationPath + "\\Plugins\\pdf_font\\SIMKAI.TTF";//把C盘中的字体复制到项目中使用，中文
      BaseFont bfchinese = BaseFont.CreateFont(fontpath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
         
            Font ChFont = new Font(bfchinese, 10, Font.NORMAL, new BaseColor(0, 0, 0));//正文，设置字体大小，颜色
            Font ChFont_title = new Font(bfchinese, 20, Font.NORMAL, new BaseColor(0, 0, 0));//主标题
            Font ChFont_title_1 = new Font(bfchinese, 15, Font.NORMAL, new BaseColor(0, 0, 0));//小标题


            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string col_1 = item["col1"].ToString();
                string[] s1 = col_1.Split(new char[] { ',' });

                string col_2 = item["col2"].ToString();
                string[] s2 = col_2.Split(new char[] { ',' });

                string col_3 = item["col3"].ToString();
                string[] s3 = col_3.Split(new char[] { ',' });

                string col_4 = item["col4"].ToString();
                string[] s4 = col_4.Split(new char[] { ',' });

                string col_5 = item["col5"].ToString();
                string[] s5 = col_5.Split(new char[] { ',' });



                Document document = new Document(PageSize.A4);          //初始化一个目标文档类           
                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(Server.MapPath("~/Upload//武汉石化绩效评估表.pdf"), FileMode.Create));
                document.Open();                //打开目标文档类

                Font pdfFont = new Font(Font.FontFamily.ZAPFDINGBATS, 7);//根据字体类型和字体大小属性创建PDF文档字体
                PdfPTable pdfTable = new PdfPTable(5); //根据数据表内容创建一个PDF格式的表,括号为表格的列数

                //表的标题
                PdfPCell cell = new PdfPCell(new Phrase("武汉石化绩效评估表", ChFont_title));
                cell.Colspan = 5;
                cell.Border = 0;
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                pdfTable.AddCell(cell);

             //报告信息
                string submitDepartment = item["submitDepartment"].ToString();
                string reportType = item["reportType"].ToString();
                int A15dot1_id =Convert.ToInt16(item["A15dot1_id"].ToString());
                

                PdfPCell cell_type = new PdfPCell(new Phrase("报告类型:" + reportType, ChFont));
                cell_type.Colspan = 3;
                cell_type.Border = 0;
                cell_type.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                pdfTable.AddCell(cell_type);

                PdfPCell cell_time = new PdfPCell(new Phrase("评估时间:" , ChFont));
                cell_time.Colspan = 1;
                cell_time.Border = 0;
                cell_time.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                pdfTable.AddCell(cell_time);


                List<A15dot1Tab> ss = new List<A15dot1Tab>();
                ss = Jx.GetJxItem_detail(A15dot1_id);
               DateTime time=Convert.ToDateTime(ss[0].submitTime);
               string subtime = "";
                if (reportType == "月报")
                {
                    subtime = time.ToString("yyyy/MM");

                }
                else
                {
                    subtime = time.ToString("yyyy") + "年";
                }

                PdfPCell cell_time_1 = new PdfPCell(new Phrase(subtime, ChFont));
                cell_time_1.Colspan = 1;
                cell_time_1.Border = 0;
                cell_time_1.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                pdfTable.AddCell(cell_time_1);

                PdfPCell cell_submit = new PdfPCell(new Phrase("提交单位:" + submitDepartment, ChFont));
                cell_submit.Colspan = 3;
                cell_submit.Border = 0;
                cell_submit.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                pdfTable.AddCell(cell_submit);

                PdfPCell cell_Person = new PdfPCell(new Phrase("提交人:" , ChFont));
                cell_Person.Colspan = 1;
                cell_Person.Border = 0;
                cell_Person.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                pdfTable.AddCell(cell_Person);

                PdfPCell cell_person_1 = new PdfPCell(new Phrase(((Session["User"] as EquipModel.Entities.Person_Info).Person_Name), ChFont));
                cell_person_1.Colspan = 1;
                cell_person_1.Border = 0;
                cell_person_1.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                pdfTable.AddCell(cell_person_1);
                //PdfPTable nested_7_2 = new PdfPTable(1);
                //nested_7_2.AddCell(new Phrase("报告类型:" + reportType, ChFont));
                //PdfPCell nesthousing_7_2 = new PdfPCell(nested_7_2);
                //nesthousing_7_2.HorizontalAlignment = 0;
                //nesthousing_7_2.Border = 0;
                //nesthousing_7_2.Padding = 0f;
                //pdfTable.AddCell(nesthousing_7_2);
                //PdfPCell detail_7_2 = new PdfPCell(new Phrase("评估时间:" + reportType, ChFont));
                //detail_7_2.Border = 0;
                //detail_7_2.HorizontalAlignment = 2;
                //detail_7_2.Colspan = 4;
                //pdfTable.AddCell(detail_7_2);


          
                //PdfPTable nested_7_1 = new PdfPTable(1);
                //nested_7_1.AddCell(new Phrase("提交单位：" + submitDepartment, ChFont));
                //PdfPCell nesthousing_7_1 = new PdfPCell(nested_7_1);
                //nesthousing_7_1.Padding = 0f;
                //pdfTable.AddCell(nesthousing_7_1);
                //PdfPCell detail_7_1 = new PdfPCell(new Phrase("提交人：" + submitDepartment, ChFont));
                //detail_7_1.Colspan = 4;
                //pdfTable.AddCell(detail_7_1);






                //一. KPI指标
                PdfPCell cell_1 = new PdfPCell(new Phrase("一.KPI指标", ChFont_title_1));
                cell_1.Colspan = 5;
                cell_1.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                pdfTable.AddCell(cell_1);


                pdfTable.AddCell(new Phrase("指标类型", ChFont));
                pdfTable.AddCell(new Phrase("KPI指标", ChFont));
                pdfTable.AddCell(new Phrase("指标说明", ChFont));
                pdfTable.AddCell(new Phrase("炼油目标值", ChFont));
                pdfTable.AddCell(new Phrase("实际值", ChFont));

                for (int i = 0; i < s1.Length; i++)
                {

                    pdfTable.AddCell(new Phrase(s1[i], ChFont));
                    pdfTable.AddCell(new Phrase(s2[i], ChFont));
                    pdfTable.AddCell(new Phrase(s3[i], ChFont));
                    pdfTable.AddCell(new Phrase(s4[i], ChFont));
                    pdfTable.AddCell(new Phrase(s5[i], ChFont));

                }

                //二. 隐患排查情况
                PdfPCell cell_2 = new PdfPCell(new Phrase("二.隐患排查情况", ChFont_title_1));
                cell_2.Colspan = 5;
                cell_2.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                pdfTable.AddCell(cell_2);
                string hiddenDangerInvestigation = item["hiddenDangerInvestigation"].ToString();
              
                PdfPTable nested_2 = new PdfPTable(1);
                nested_2.AddCell(new Phrase("隐患排查情况", ChFont));
                PdfPCell nesthousing_2 = new PdfPCell(nested_2);
                nesthousing_2.Padding = 0f;
                pdfTable.AddCell(nesthousing_2);
                PdfPCell detail_2 = new PdfPCell(new Phrase(hiddenDangerInvestigation, ChFont));
                detail_2.Colspan = 4;
                pdfTable.AddCell(detail_2);

                //三. 装置生产运行概况
                PdfPCell cell_3 = new PdfPCell(new Phrase("三.装置生产运行概况", ChFont_title_1));
                cell_3.Colspan = 5;
                cell_3.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                pdfTable.AddCell(cell_3);
                string rateLoad = item["rateLoad"].ToString();
                string gyChange = item["gyChange"].ToString();
                string equipChange = item["equipChange"].ToString();
                string otherDescription = item["otherDescription"].ToString();

                PdfPTable nested_3_1 = new PdfPTable(1);
                nested_3_1.AddCell(new Phrase("负荷率", ChFont));
                PdfPCell nesthousing_3_1 = new PdfPCell(nested_3_1);
                nesthousing_3_1.Padding = 0f;
                pdfTable.AddCell(nesthousing_3_1);
                PdfPCell detail_3_1 = new PdfPCell(new Phrase(rateLoad, ChFont));
                detail_3_1.Colspan = 4;
                pdfTable.AddCell(detail_3_1);

                PdfPTable nested_3_2 = new PdfPTable(1);
                nested_3_2.AddCell(new Phrase("工艺变更", ChFont));
                PdfPCell nesthousing_3_2 = new PdfPCell(nested_3_2);
                nesthousing_3_2.Padding = 0f;
                pdfTable.AddCell(nesthousing_3_2);
                PdfPCell detail_3_2 = new PdfPCell(new Phrase(gyChange, ChFont));
                detail_3_2.Colspan = 4;
                pdfTable.AddCell(detail_3_2);

                PdfPTable nested_3_3 = new PdfPTable(1);
                nested_3_3.AddCell(new Phrase("设备变更", ChFont));
                PdfPCell nesthousing_3_3 = new PdfPCell(nested_3_3);
                nesthousing_3_3.Padding = 0f;
                pdfTable.AddCell(nesthousing_3_3);
                PdfPCell detail_3_3 = new PdfPCell(new Phrase(equipChange, ChFont));
                detail_3_3.Colspan = 4;
                pdfTable.AddCell(detail_3_3);

                PdfPTable nested_3_4 = new PdfPTable(1);
                nested_3_4.AddCell(new Phrase("其他说明", ChFont));
                PdfPCell nesthousing_3_4 = new PdfPCell(nested_3_4);
                nesthousing_3_4.Padding = 0f;
                pdfTable.AddCell(nesthousing_3_4);
                PdfPCell detail_3_4 = new PdfPCell(new Phrase(otherDescription, ChFont));
                detail_3_4.Colspan = 4;
                pdfTable.AddCell(detail_3_4);

              //四 装置设备运行情况基本评估
                PdfPCell cell_4 = new PdfPCell(new Phrase("四.装置设备运行情况基本评估", ChFont_title_1));
                cell_4.Colspan = 5;
                cell_4.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                pdfTable.AddCell(cell_4);
                string evaluateEquipRunStaeDesc = item["evaluateEquipRunStaeDesc"].ToString();

                PdfPTable nested_4 = new PdfPTable(1);
                nested_4.AddCell(new Phrase("装置设备运行情况基本评估", ChFont));
                PdfPCell nesthousing_4 = new PdfPCell(nested_4);
                nesthousing_4.Padding = 0f;
                pdfTable.AddCell(nesthousing_4);
                PdfPCell detail_4 = new PdfPCell(new Phrase(evaluateEquipRunStaeDesc, ChFont));
                detail_4.Colspan = 4;
                pdfTable.AddCell(detail_4);

                //五 可靠性结论

                PdfPCell cell_5 = new PdfPCell(new Phrase("五.可靠性结论", ChFont_title_1));
                cell_5.Colspan = 5;
                cell_5.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                pdfTable.AddCell(cell_5);
                string reliabilityConclusion = item["reliabilityConclusion"].ToString();

                PdfPTable nested_5= new PdfPTable(1);
                nested_5.AddCell(new Phrase("可靠性结论", ChFont));
                PdfPCell nesthousing_5 = new PdfPCell(nested_5);
                nesthousing_5.Padding = 0f;
                pdfTable.AddCell(nesthousing_5);
                PdfPCell detail_5 = new PdfPCell(new Phrase(reliabilityConclusion, ChFont));
                detail_5.Colspan = 4;
                pdfTable.AddCell(detail_5);

                //六 机动处建议及改进措施

                PdfPCell cell_6 = new PdfPCell(new Phrase("六.机动处建议及改进措施", ChFont_title_1));
                cell_6.Colspan = 5;
                cell_6.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                pdfTable.AddCell(cell_6);
                string jdcAdviseImproveMeasures = item["jdcAdviseImproveMeasures"].ToString();

                PdfPTable nested_6 = new PdfPTable(1);
                nested_6.AddCell(new Phrase("机动处建议及改进措施", ChFont));
                PdfPCell nesthousing_6 = new PdfPCell(nested_6);
                nesthousing_6.Padding = 0f;
                pdfTable.AddCell(nesthousing_6);
                PdfPCell detail_6 = new PdfPCell(new Phrase(jdcAdviseImproveMeasures, ChFont));
                detail_6.Colspan = 4;
                pdfTable.AddCell(detail_6);
                //七， 图片

                string jpg_name = item["jpgname"].ToString();

                if (jpg_name.Contains(".jpg") || jpg_name.Contains(".JPG") || jpg_name.Contains(".png") || jpg_name.Contains(".PNG"))
                {

                    PdfPCell cell_7 = new PdfPCell(new Phrase("附:装置设备运行情况基本评估附图", ChFont_title_1));
                    cell_7.Colspan = 5;
                    cell_7.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    cell_7.Border = 0;
                    pdfTable.AddCell(cell_7);

                }
                document.Add(pdfTable);
                document.AddTitle("武汉石化绩效评估表");
            
                if(jpg_name.Contains(".jpg") ||jpg_name.Contains(".JPG") || jpg_name.Contains(".png") || jpg_name.Contains(".PNG")){
                
                    //显示图片的代码开始部分
                    string imagepath = Server.MapPath("~/Upload");
                    // Image jpg = Image.GetInstance(imagepath + "/课表.jpg");
                    Image jpg = Image.GetInstance(imagepath + "/" + jpg_name);
                    jpg.Alignment = Image.ALIGN_MIDDLE;//图片居中显示
                    // jpg.ScaleToFit(150,300);
                    jpg.ScaleToFit(400, 400);//图片容器大小
                    document.Add(jpg);

                    //显示图片的代码结束部分       

                }





                document.Close();
                writer.Close();
                return Path.Combine("/Upload", "武汉石化绩效评估表.pdf");

            }
            catch (Exception)
            {
                throw;
            }

            //return "打印失败";
            // return ("213");
        }

          public ActionResult DSZzSubmit(string wfe_id)
          {
              submitmodel sm = new submitmodel();
              ViewBag.curtime = DateTime.Now;
              ViewBag.curuser = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
              int UserId = (Session["User"] as EquipModel.Entities.Person_Info).Person_Id;
              PersonManagment pm = new PersonManagment();
              EquipBLL.AdminManagment.PersonManagment.P_viewModal pv = pm.Get_PersonModal(UserId);
              if (pv.Role_Names.Contains("可靠性工程师"))
                  sm.kkxgcs = 1;
              if (pv.Role_Names.Contains("检维修人员"))
                  sm.jwxry = 1;
              ERPInfoManagement erp = new ERPInfoManagement();
              EquipArchiManagment em = new EquipArchiManagment();
              UI_MISSION mi = CWFEngine.GetActiveMission<Person_Info>(int.Parse(wfe_id), ((IObjectContextAdapter)(new EquipWebContext())).ObjectContext);
              Dictionary<string, object> mi_params = mi.Miss_Params;
              ViewBag.Pqname = mi.Miss_Params["Pqname"].ToString();
              string ea_code = em.getEa_codebyname(mi.Miss_Params["Pqname"].ToString());
              ViewBag.timesNonPlanStop=erp.getNoticesYx_1(mi.Miss_Params["Pqname"].ToString());
              ViewBag.scoreDeductFaultIntensity = (erp.getNoticesYx_1(mi.Miss_Params["Pqname"].ToString()) * 50) + (erp.getNoticeYx_2(mi.Miss_Params["Pqname"].ToString()) * 30) + (erp.getNoticeYx_3(mi.Miss_Params["Pqname"].ToString()) * 20) + (erp.getNoticeYx_4(mi.Miss_Params["Pqname"].ToString()) * 5);
              ViewBag.rateFaultMaintenance = erp.getFaultRation(mi.Miss_Params["Pqname"].ToString());
              ViewBag.MTBF = erp.getNonFaultInterVal(mi.Miss_Params["Pqname"].ToString());
              ViewBag.rateEquipUse = erp.DeliverRatio(mi.Miss_Params["Pqname"].ToString());
              ViewBag.rateBigUnitFault = erp.bigEquipsRatio(mi.Miss_Params["Pqname"].ToString());
              ViewBag.wfe_id = wfe_id;
              TablesManagment tm = new TablesManagment();
              EquipManagment Em = new EquipManagment();
              List<EANummodel> E = Em.getequipnum_byarchi();
              List<Equip_Archi> AllCj_List = Em.GetAllCj();

              List<WebApp.Controllers.A5dot1Controller.NameandNum> cj = new List<WebApp.Controllers.A5dot1Controller.NameandNum>();
              List<WebApp.Controllers.A5dot1Controller.NameandNum> pq = new List<WebApp.Controllers.A5dot1Controller.NameandNum>();




              for (int i = 0; i < AllCj_List.Count; i++)
              {
                  int count = 0;
                  WebApp.Controllers.A5dot1Controller.NameandNum temp1 = new WebApp.Controllers.A5dot1Controller.NameandNum();
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
            
              WebApp.Controllers.A5dot1Controller.NameandNum temp = new WebApp.Controllers.A5dot1Controller.NameandNum();
              temp.name = mi.Miss_Params["Pqname"].ToString();
              List<Pq_Zz_map> Pq_Zz_map = Em.GetZzsofPq(mi.Miss_Params["Pqname"].ToString());
              int count1 = 0;
              for (int j = 0; j < Pq_Zz_map.Count; j++)
              {
                 
                  for (int z = 0; z < E.Count; z++)
                  {
                      if (Pq_Zz_map[j].Zz_Name == Em.getEa_namebyid(E[z].EA_Id))
                          count1 += E[z].Equip_Num;
                  }
              }
              temp.Equip_Num = count1;
              pq.Add(temp);

              double pq_bwh = 0.00;
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
                  pq_bwh=Math.Round(((double)pq_bxhcount / pq[i].Equip_Num), 6);
              }
              ViewBag.Pq_bwh = (1-pq_bwh)*100;
              return View(sm);
          }
      
    }
}
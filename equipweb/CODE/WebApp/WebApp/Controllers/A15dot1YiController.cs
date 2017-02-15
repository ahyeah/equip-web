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
    public class A15dot1YiController : CommonController
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
            A15dot1TabYi new_15dot1Yi = new A15dot1TabYi();
            new_15dot1Yi.gzqdkf = Convert.ToInt32(item["gzqdkf"].ToString());
            new_15dot1Yi.lsxtzqdzl = Convert.ToDouble(item["lsxtzqdzl"].ToString());
            new_15dot1Yi.kzxtgzcs = Convert.ToInt32(item["kzxtgzcs"].ToString());
            new_15dot1Yi.ybkzl = Convert.ToDouble(item["ybkzl"].ToString());
            new_15dot1Yi.ybsjkzl = Convert.ToDouble(item["ybsjkzl"].ToString());
            new_15dot1Yi.lsxttyl = Convert.ToDouble(item["lsxttyl"].ToString());
            new_15dot1Yi.gjkzfmgzcs = Convert.ToInt32(item["gjkzfmgzcs"].ToString());
            new_15dot1Yi.kzxtgzbjcs = Convert.ToInt32(item["kzxtgzbjcs"].ToString());
            new_15dot1Yi.cgybgzl = Convert.ToDouble(item["cgybgzl"].ToString());
            new_15dot1Yi.tjfMDBF = Convert.ToDouble(item["tjfMDBF"].ToString());
            new_15dot1Yi.temp3 = item["cjname"].ToString();
            new_15dot1Yi.submitDepartment = item["submitDepartment"].ToString();
            new_15dot1Yi.submitUser = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
            new_15dot1Yi.submitTime = DateTime.Now;
            new_15dot1Yi.state = 0;
            //new_15dot1.state = Convert.ToInt32(item["state"].ToString());
            new_15dot1Yi.reportType = item["reportType"].ToString();
            new_15dot1Yi.temp2 = "仪表专业";
            bool res = Jx.AddYiItem(new_15dot1Yi);
            return res;
        }

    }
}
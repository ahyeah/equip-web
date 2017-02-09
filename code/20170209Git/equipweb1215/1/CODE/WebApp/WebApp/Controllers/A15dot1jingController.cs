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
    public class A15dot1jingController: CommonController
    {
        private KpiManagement Jx = new KpiManagement();
        public class submitmodel
        {
            public string troubleKoufen;//故障强度扣分
            public string equipFushil;//设备腐蚀泄漏次数(有毒有害易燃易爆介质)
            public string thousandColdChangeRate;//千台冷换设备管束（含整台）更换率
            public string gongyelu;//工业炉（>=10MW）平均热效率
            public string huanreqiRate;//换热器检修率
            public string pressureRate;//压力容器定检率
            public string yaliguandao;//压力管道年度检验计划完成率
            public string safeRate;//安全阀年度校验计划完成率
            public string equipFushiRate;//设备腐蚀监测计划完成率
            public string jingEquipRate;//静设备检维修一次合格率
            public List<Equip_Archi> UserHasEquips;
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
            A15dot1TabJing new_15dot1Jing = new A15dot1TabJing();
            new_15dot1Jing.gzqdkf = Convert.ToInt32(item["gzqdkf"].ToString());
            new_15dot1Jing.sbfsxlcs = Convert.ToInt32(item["sbfsxlcs"].ToString());
            new_15dot1Jing.qtlhsbgs = Convert.ToInt32(item["qtlhsbgs"].ToString());
            new_15dot1Jing.gylpjrxl = Convert.ToDouble(item["gylpjrxl"].ToString());
            new_15dot1Jing.hrqjxl = Convert.ToDouble(item["hrqjxl"].ToString());
            new_15dot1Jing.ylrqdjl = Convert.ToDouble(item["ylrqdjl"].ToString());
            new_15dot1Jing.ylgdndjxjhwcl = Convert.ToInt32(item["ylgdndjxjhwcl"].ToString());
            new_15dot1Jing.aqfndjyjhwcl = Convert.ToInt32(item["aqfndjyjhwcl"].ToString());
            new_15dot1Jing.sbfsjcjhwcl = Convert.ToDouble(item["sbfsjcjhwcl"].ToString());
            new_15dot1Jing.jsbjwxychgl = Convert.ToDouble(item["jsbjwxychgl"].ToString());
            new_15dot1Jing.temp3 = item["cjname"].ToString();
            new_15dot1Jing.submitDepartment = item["submitDepartment"].ToString();
            new_15dot1Jing.submitUser = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
            new_15dot1Jing.submitTime = DateTime.Now;
            new_15dot1Jing.state = 0;
            new_15dot1Jing.reportType = item["reportType"].ToString();
            new_15dot1Jing.temp2 = "静设备专业";
            bool res = Jx.AddJingItem(new_15dot1Jing);
            return res;
        }



    }
}
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
    public class A15dot1qiyeController : CommonController
    {
        private KpiManagement Jx = new KpiManagement();
        public ActionResult Index()
        {
            return View(getRecord());
        }
        public class Index_ModelforA15
        {
            public List<A15dot1TabQiYe> Am;
            public List<A15dot1TabQiYe> Hm;
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
            RecordforA15.Am = new List<A15dot1TabQiYe>();
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

            List<A15dot1TabQiYe> miss = Jx.GetJxItem(pv.Role_Names, pv.Department_Name, pv.Person_Name,cjname);
            foreach (var item in miss)
            {
                A15dot1TabQiYe a = new A15dot1TabQiYe();
                a.zzkkxzs = item.zzkkxzs;
                a.wxfyzs = item.wxfyzs;
                a.qtlxbmfxhl = item.qtlxbmfxhl;
                a.qtlhsbgsghl = item.qtlhsbgsghl;
                a.ybsjkzl = item.ybsjkzl;
                a.sjs = item.sjs;
                a.gzqdkf = item.gzqdkf;
                a.xmyql = item.xmyql;
                a.pxjnl = item.pxjnl;
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

            RecordforA15.Hm = new List<A15dot1TabQiYe>();
            List<A15dot1TabQiYe> His = Jx.GetHisJxItem(pv.Role_Names, pv.Department_Name, pv.Person_Name);
            foreach (var item in His)
            {
                A15dot1TabQiYe a = new A15dot1TabQiYe();
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
            RecordforA15.Am = new List<A15dot1TabQiYe>();
            RecordforA15.Hm = new List<A15dot1TabQiYe>();
            ViewBag.curtime = DateTime.Now.ToString();
            ViewBag.curuser = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
            int IntId = Convert.ToInt32(id);
            List<A15dot1TabQiYe> miss = Jx.GetJxItem_detail(IntId);
            foreach (var item in miss)
            {
                A15dot1TabQiYe a = new A15dot1TabQiYe();
                a.zzkkxzs = item.zzkkxzs;
                a.wxfyzs = item.wxfyzs;
                a.qtlxbmfxhl = item.qtlxbmfxhl;
                a.qtlhsbgsghl = item.qtlhsbgsghl;
                a.ybsjkzl = item.ybsjkzl;
                a.sjs = item.sjs;
                a.gzqdkf = item.gzqdkf;
                a.xmyql = item.xmyql;
                a.pxjnl = item.pxjnl;
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
                a.temp3 = item.temp3;
               a.reliabilityConclusion = item.reliabilityConclusion;
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
            List<A15dot1TabQiYe> His = Jx.GetHisJxItem_detail(IntId);
            foreach (var item in His)
            {
                A15dot1TabQiYe a = new A15dot1TabQiYe();
                a.zzkkxzs = item.zzkkxzs;
                a.wxfyzs = item.wxfyzs;
                a.qtlxbmfxhl = item.qtlxbmfxhl;
                a.qtlhsbgsghl = item.qtlhsbgsghl;
                a.ybsjkzl = item.ybsjkzl;
                a.sjs = item.sjs;
                a.gzqdkf = item.gzqdkf;
                a.xmyql = item.xmyql;
                a.pxjnl = item.pxjnl;
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
        public ActionResult Submit()
        {
            PersonManagment pm = new PersonManagment();
            submitmodel sm = new submitmodel();
            ViewBag.curtime = DateTime.Now;
            ViewBag.curuser = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
            //int UserId = (Session["User"] as EquipModel.Entities.Person_Info).Person_Id;
            //PersonManagment pm = new PersonManagment();
            //EquipBLL.AdminManagment.PersonManagment.P_viewModal pv = pm.Get_PersonModal(UserId);
            //if (pv.Role_Names.Contains("可靠性工程师"))
            //    sm.kkxgcs = 1;
            //if (pv.Role_Names.Contains("检维修人员"))
            //    sm.jwxry = 1;
            sm.UserHasEquips = pm.Get_Person_Cj((Session["User"] as EquipModel.Entities.Person_Info).Person_Id);
            return View(sm);
        }
        public string qst(string grahpic_name, string zz)
        {


            List<object> res = Jx.qst(grahpic_name, zz);

            string str = JsonConvert.SerializeObject(res);

            return (str);

        }
        public ActionResult KkxSubmit(string id)
        {
            return View(getRecord_detail(id));
        }
        public ActionResult JdcSubmit(string id)
        {
            return View(getRecord_detail(id));
        }
        public bool Submit_submitsignal(string json1)
        {

            JObject item = (JObject)JsonConvert.DeserializeObject(json1);
            A15dot1TabQiYe new_15dot1qiye = new A15dot1TabQiYe();
            new_15dot1qiye.zzkkxzs = Convert.ToDouble(item["equipReliableExponent"]);
            new_15dot1qiye.wxfyzs = Convert.ToDouble(item["maintainChargeExponent"]);
            new_15dot1qiye.qtlxbmfxhl = Convert.ToDouble(item["thousandLXBrate"]);
            new_15dot1qiye.qtlhsbgsghl = Convert.ToDouble(item["thousandColdChangeRate"]);
            new_15dot1qiye.ybsjkzl = Convert.ToDouble(item["instrumentControlRate"]);
            new_15dot1qiye.sjs = Convert.ToInt32(item["eventNumber"]);
            new_15dot1qiye.gzqdkf = Convert.ToInt32(item["troubleKoufen"]);
            new_15dot1qiye.xmyql = Convert.ToDouble(item["projectLateRate"]);
            new_15dot1qiye.pxjnl = Convert.ToDouble(item["trainAndAbility"]);
            new_15dot1qiye.pxjnl = Convert.ToInt32(item["trainAndAbility"]);
            new_15dot1qiye.submitDepartment = Convert.ToString(item["zzId"]);//存储装置的名字
            new_15dot1qiye.temp3 = Convert.ToString(item["cjname"]);//车间名字存于temp3
            new_15dot1qiye.temp2 = "企业级";//专业名字存于temp2
            new_15dot1qiye.submitUser = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
            new_15dot1qiye.submitTime = DateTime.Now;
            bool res = Jx.AddJxItem(new_15dot1qiye);
            return res;

        }
        public JsonResult Modify_submitsignal(string json1)
        {

            JObject item = (JObject)JsonConvert.DeserializeObject(json1);
            A15dot1TabQiYe new_15dot1qiye = new A15dot1TabQiYe();
            new_15dot1qiye.zzkkxzs = Convert.ToDouble(item["equipReliableExponent"]);
            new_15dot1qiye.wxfyzs = Convert.ToDouble(item["maintainChargeExponent"]);
            new_15dot1qiye.qtlxbmfxhl = Convert.ToDouble(item["thousandLXBrate"]);
            new_15dot1qiye.qtlhsbgsghl = Convert.ToDouble(item["thousandColdChangeRate"]);
            new_15dot1qiye.ybsjkzl = Convert.ToDouble(item["instrumentControlRate"]);
            new_15dot1qiye.sjs = Convert.ToInt32(item["eventNumber"]);
            new_15dot1qiye.gzqdkf = Convert.ToInt32(item["troubleKoufen"]);
            new_15dot1qiye.xmyql = Convert.ToDouble(item["projectLateRate"]);
            new_15dot1qiye.pxjnl = Convert.ToDouble(item["trainAndAbility"]);
            new_15dot1qiye.reliabilityConclusion = Convert.ToString(item["reliabilityConclusion"]);
            new_15dot1qiye.submitDepartment = Convert.ToString(item["zzId"]);
            new_15dot1qiye.temp2 = "企业级";
            new_15dot1qiye.Id = Convert.ToInt32(item["A15dot1_id"]);
            new_15dot1qiye.submitUser = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
            new_15dot1qiye.submitTime = DateTime.Now;
            new_15dot1qiye.state = Convert.ToInt32(item["state"]);
            bool res = Jx.ModifyJxItem(new_15dot1qiye);
            return Json(new { result = res });

        }
      
    }
}
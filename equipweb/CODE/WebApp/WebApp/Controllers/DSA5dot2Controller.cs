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
using FlowEngine.TimerManage;
using FlowEngine.Modals;

namespace WebApp.Controllers
{
    public class DSA5dot2Controller : CommonController
    {
        //
        // GET: /LSA5dot2/
        PersonManagment pm = new PersonManagment();
        TablesManagment tm = new TablesManagment();
        EquipManagment Em = new EquipManagment();
        EquipArchiManagment Eam = new EquipArchiManagment();
        private SxglManagment Sx = new SxglManagment();
        public ActionResult ZzSubmit(string wfe_id)
        {
            UI_MISSION mi = CWFEngine.GetActiveMission<Person_Info>(int.Parse(wfe_id), ((IObjectContextAdapter)(new EquipWebContext())).ObjectContext);
            Dictionary<string, object> mi_params = mi.Miss_Params;
            int Zz_id = Eam.getEa_idbyname(mi.Miss_Params["Zz_Name"].ToString());
            ViewBag.Zz_id = Zz_id;
            ViewBag.Zz_name = mi.Miss_Params["Zz_Name"].ToString();
            ViewBag.wfe_id = wfe_id;
            return View();
        }
        public string ZzSubmit_Bcsignal(string json1)
        {
            try
            {

                JObject item = (JObject)JsonConvert.DeserializeObject(json1);

                CTimerTimeout timeout_job = new CTimerTimeout();
                timeout_job.CreateTime = DateTime.Now;
                timeout_job.for_using = TIMER_USING.FOR_SYSTEM;
                timeout_job.mission_name = "";
                timeout_job.status = TM_STATUS.TM_STATUS_ACTIVE;
                timeout_job.SetActionForWF("INVILID");
                timeout_job.AttachWFEntityID = Convert.ToInt32(item["wfe_id"]);
                timeout_job.CustomAction = "http://localhost/CallBack/testCallBack";
                timeout_job.EventName = "PqAessess";
                timeout_job.SetTriggerTiming("0 0 0 20 * ?");
                //保存该定时任务，并将其添加到激活任务列表 
                timeout_job.Save();
                int m_timerMissionID = timeout_job.ID;
                CTimerManage.AppendMissionToActiveList(timeout_job);


              //  A5dot1Tab1 a5dot1Tab1 = new A5dot1Tab1();
                DateTime my = DateTime.Now;
                int cjid = Em.getEA_parentid(Convert.ToInt32(item["Zz_Id"]));
                string cjname = Eam.getEa_namebyId(cjid);

                A5dot2Tab1 new_5dot2 = new A5dot2Tab1();
                new_5dot2.cjName = cjname;
                new_5dot2.zzName = item["Zz_Name"].ToString();
                new_5dot2.sbGyCode = item["Equip_GyCode"].ToString();
                new_5dot2.sbCode = item["Equip_Code"].ToString();
                new_5dot2.sbType = item["Equip_Type"].ToString();
                new_5dot2.zyType = item["Zy_Type"].ToString();
                new_5dot2.problemDescription = item["problemDescription"].ToString();
                new_5dot2.jxSubmitTime = DateTime.Now;
                new_5dot2.jxUserName = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
                new_5dot2.isRectified = 0;
                new_5dot2.state = 0;
                new_5dot2.temp2 = item["wfe_id"].ToString();
                new_5dot2.temp3 = m_timerMissionID.ToString();
                bool res = Sx.AddSxItem(new_5dot2);

                string wfe_id = item["wfe_id"].ToString();
                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();
                signal["ZzSubmit_done"] = "true";

                //record
                Dictionary<string, string> record = new Dictionary<string, string>();
                record["username"] = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
                record["time"] = DateTime.Now.ToString();
                //submit
                CWFEngine.SubmitSignal(Convert.ToInt32(wfe_id), signal, record);
            }
            catch (Exception e)
            {
                return "";
            }
            return ("/A5dot2/Index");
        }

   


    }
}
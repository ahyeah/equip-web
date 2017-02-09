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
using FlowEngine.DAL;
using FlowEngine.Modals;

namespace WebApp.Controllers
{
    public class LSA5dot2Controller : CommonController
    {
        //
        // GET: /LSA5dot2/
        PersonManagment pm = new PersonManagment();
        TablesManagment tm = new TablesManagment();
        EquipManagment Em = new EquipManagment();
        EquipArchiManagment Eam = new EquipArchiManagment();
        private SxglManagment Sx = new SxglManagment();
        public ActionResult Index(int job_id)
        {

            Jobs js = new Jobs();
            Timer_Jobs tj = js.GetTimerJob(job_id);
            string qeendtime = tj.STR_RES_3;
            if (DateTime.Now < DateTime.Parse(qeendtime))
                ViewBag.zgenable = 1;
            else
                ViewBag.zgenable = 0;
            ViewBag.jobName = tj.job_name;
            ViewBag.time = tj.STR_RES_2;
            ViewBag.depts = tj.STR_RES_1;
            ViewBag.wfe_ids = tj.run_result;

            int UserId = (Session["User"] as EquipModel.Entities.Person_Info).Person_Id;
            PersonManagment pm = new PersonManagment();
            EquipBLL.AdminManagment.PersonManagment.P_viewModal pv = pm.Get_PersonModal(UserId);
            if (pv.Role_Names.Contains("可靠性工程师"))
            {
                ViewBag.isKkxgcs = "1";
            }
            else
            {
                ViewBag.isKkxgcs = "0";
            }
            return View();
        }
        public class LSA5Model
        {
            public string userName;
            public string missTime;
            public int missIndex;
            public string miss_desc;
            public string miss_url;
            public string wfe_serial;
            public string zz_name;
            public string workFlowName;
        }
        public string getdcllist(string jobName, string time, string depts, string wfe_ids)
        {
            string run_results = wfe_ids.TrimStart('[').TrimEnd(']');
            List<string> run_enity_id = new List<string>();
            List<int> wfe_id = new List<int>();
            if (run_results.Contains(','))
            {
                run_enity_id = run_results.Split(new char[] { ',' }).ToList();

            }
            else
            {
                run_enity_id.Add(run_results);
            }
            for (int i = 0; i < run_enity_id.Count(); i++)
            {
                wfe_id.Add(Convert.ToInt16(run_enity_id[i]));
            }


            // string WorkFlow_Name = "A5dot1";
            List<LSA5Model> Am = new List<LSA5Model>();
            // List<UI_MISSION> miss;
            List<object> or = new List<object>();
            //miss = CWFEngine.GetActiveMissions<Person_Info>(((IObjectContextAdapter)(new EquipWebContext())).ObjectContext, WorkFlow_Name, true);
            foreach (var item in wfe_id)
            {
                LSA5Model o = new LSA5Model();
                Dictionary<string, object> paras = new Dictionary<string, object>();
                paras["Zz_Name"] = null;
                UI_WFEntity_Info wfei = CWFEngine.GetWorkFlowEntityWithParams(item, paras);
                UI_MISSION lastMi = CWFEngine.GetHistoryMissions(item).Last();

                int Miss_Id = lastMi.Miss_Id;
                IDictionary<string, string> r = CWFEngine.GetMissionRecordInfo(Miss_Id);
                if (r.Count > 0)
                {
                    //o.userName = r["username"];
                    o.missTime = r["time"];
                }
                else
                {
                    // o.userName = "";
                    o.missTime = "";
                }
                //if (item.Mission_Url.Contains("dot"))
                //    o.miss_url = item.Mission_Url;
                //else
                //    o.miss_url = "";
                o.wfe_serial = wfei.serial;
                o.zz_name = paras["Zz_Name"].ToString();
                o.miss_url = "/LSA5dot2/ZzSubmit/?wfe_id=" + item.ToString();
                Am.Add(o);
            }
            for (int i = 0; i < Am.Count; i++)
            {
                object o = new
                {
                    wfe_serial = Am[i].wfe_serial,
                    zz_Name = Am[i].zz_name,
                    job_name = jobName,
                    time = time,
                    missTime = Am[i].missTime,
                    depts = depts,
                    miss_url = Am[i].miss_url
                };
                or.Add(o);

            }
            string str = JsonConvert.SerializeObject(or);
            return ("{" + "\"data\": " + str + "}");
        }
        public string dcl_list(string wfe_ids)
        {
            string run_results = wfe_ids.TrimStart('[').TrimEnd(']');
            List<string> run_enity_id = new List<string>();
            List<int> wfe_id = new List<int>();
            if (run_results.Contains(','))
            {
                run_enity_id = run_results.Split(new char[] { ',' }).ToList();

            }
            else
            {
                run_enity_id.Add(run_results);
            }
            for (int i = 0; i < run_enity_id.Count(); i++)
            {
                wfe_id.Add(Convert.ToInt16(run_enity_id[i]));
            }
          
            List<A5dot2Tab1> E = Sx.GetLS_listbywfe_id(wfe_id);
            List<object> r = new List<object>();
            string isRectified = "";
            for (int i = 0; i < E.Count; i++)
            {

                if (E[i].isRectified == 0)
                    isRectified = "未整改";
                else
                    isRectified = "已整改";
                object o = new
                {
                    index = i + 1,
                    zzname = E[i].zzName.ToString(),
                    sbgybh = E[i].sbGyCode.ToString(),
                    sbcode = E[i].sbCode.ToString(),
                    notgood = E[i].problemDescription.ToString(),
                    iszg = isRectified,
                    a_id = E[i].Id
                };
                r.Add(o);
            }
            string str = JsonConvert.SerializeObject(r);
            return ("{" + "\"data\": " + str + "}");


        }
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
        public string Pqcheck_submitsignal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string pqname = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
                // int a_id = Convert.ToInt32(item["a_id"]);
                string a_ids = item["a_id"].ToString();
                List<string> a_id = a_ids.Split(new char[] { ',' }).ToList();
                DateTime time = DateTime.Now;
                TablesManagment tm = new TablesManagment();
                for (int i = 0; i < a_id.Count(); i++)
                {
                    tm.Pqcheck_byid(Convert.ToInt32(a_id[i]), pqname, time);
                    A5dot2Tab1 new_5dot21 = new A5dot2Tab1();
                    new_5dot21.pqCheckTime = DateTime.Now;
                    new_5dot21.pqUserName = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
                    new_5dot21.isRectified = 1;
                    new_5dot21.state = 1;                                                                                 
                    new_5dot21.Id = Convert.ToInt32(a_id[i]);
                    string res = Sx.ModifySxItem1(new_5dot21);
                }

            }
            catch (Exception e)
            {
                return "";
            }
            return ("/A5dot2/Index");
        }
        public string ZzSubmit_Bcsignal(string json1)
        {
            try
            {

                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                A5dot1Tab1 a5dot1Tab1 = new A5dot1Tab1();
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
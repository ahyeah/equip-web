using EquipBLL.AdminManagment;
using EquipBLL.AdminManagment.MenuConfig;
using EquipModel.Context;
using EquipModel.Entities;
using FlowEngine;
using FlowEngine.Modals;
using FlowEngine.TimerManage;
using FlowEngine.UserInterface;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Models.DateTables;

namespace WebApp.Controllers
{
    public class A6dot2dot2Controller : CommonController
    {
        //
        // GET: /A6dot2dot2/
        public class A6dot2dot2InfoModal
        {
            public string cj_name;
            public string wfe_id;
            public string tempjob_name;
            public List<Equip_Archi>  All_Zz;

        }

        public class LsTaskInfo
        {
            public string workflow_ser;
            public string workflow_name;
            public string cj_name;
            public string Zt_unit;
            public int status;
          

        }
        public ActionResult Index()
        {
            return View( getIndexListRecords("A6dot2dot2", (Session["User"] as EquipModel.Entities.Person_Info).Person_Name  )  );
        }
        public ActionResult Submit(string wfe_id)
        {
            EquipManagment EM = new EquipManagment();

            UI_MISSION mi = CWFEngine.GetActiveMission<Person_Info>(int.Parse(wfe_id), ((IObjectContextAdapter)(new EquipWebContext())).ObjectContext);
            Dictionary<string ,object> mi_params=mi.Miss_Params;
            string cj_name=mi.Miss_Params["Cj_Name"].ToString();
           string tempjob_name=mi.Miss_Params["Job_Name"].ToString();
            A6dot2Managment AM = new A6dot2Managment();
            int i = 1;
            List<A6dot2LsTaskTab> rList = AM.getLsTask(wfe_id);
            //string cj_name = "2";
            //string tempjob_name = "test";
            ViewBag.wfe_id = wfe_id;
            A6dot2dot2InfoModal infoModal = new A6dot2dot2InfoModal();
            infoModal.tempjob_name = tempjob_name;
            infoModal.cj_name = cj_name;
            infoModal.wfe_id = wfe_id;
            infoModal.All_Zz=EM.getZzs_ofCj(Convert.ToInt32(cj_name));

            return View(infoModal);

        }


        public ActionResult LsTaskHistoryDetail(string wfe_id)
            {
            EquipArchiManagment Em = new EquipArchiManagment();
            UI_MISSION mi=new UI_MISSION();

             List<UI_MISSION> t=CWFEngine.GetHistoryMissions(int.Parse(wfe_id));
             mi=(UI_MISSION)t.ElementAt(1);
            Dictionary<string ,object> mi_params=mi.Miss_Params;
            string cj_name=mi.Miss_Params["Cj_Name"].ToString();
            string tempjob_name=mi.Miss_Params["Job_Name"].ToString();
            

            ViewBag.wfe_id = wfe_id;
            A6dot2dot2InfoModal infoModal = new A6dot2dot2InfoModal();
            infoModal.tempjob_name = tempjob_name;
            infoModal.cj_name = cj_name;
            infoModal.wfe_id = wfe_id;
            EquipManagment EM = new EquipManagment();
            infoModal.All_Zz = EM.getZzs_ofCj(Convert.ToInt32(cj_name));

            return View(infoModal);
        }

        public string getA6dot2dot2Tab(string wfe_id)
       {
           List<object> or = new List<object>();
           A6dot2Managment  AM=new A6dot2Managment();
           int i=1;
           List<A6dot2LsTaskTab> rList= AM.getLsTask(wfe_id);
            foreach(var   r in rList)
            {
                object o = new
                {    ID=r.Id,
                    Zz_Name = r.Zz_Name,
                    Equip_Gybh = r.Equip_Gybh,
                    Equip_Code = r.Equip_Code,
                    Last_HY = r.lastOilTime,
                    HY_ZQ = r.oilInterval,
                    Next_HY = r.NextOilTime,
                    Problem_Cur = r.cur_problem,
                    ZG_status=r.cur_status,
                    Wfe_id=r.wfd_id

                };
            i=i+1;
                or.Add(o);

            }
            string str = JsonConvert.SerializeObject(or);
            return ("{" + "\"data\": " + str + "}");

       }


        public JsonResult List_Equips(string Zz_Id)
        {


            EquipManagment EM = new EquipManagment();
            List<Equip_Info> e_obj = EM.getEquips_OfZz(Convert.ToInt32(Zz_Id));
            List<object> r = new List<object>();
            foreach (Equip_Info item in e_obj)
            {
                object o = new
                {
                    Equip_Id = item.Equip_Id,
                    Equip_GyCode = item.Equip_GyCode,

                };
                r.Add(o);

            }

            return Json(r.ToArray());

           


           


        }



        /// <summary>
        /// 解析datatables 请求的Form数据
        /// </summary>
        /// <param name="Form"></param>
        /// <returns></returns>
        private List<KeyValuePair<string, string>> ParsePostForm(NameValueCollection Form)
        {
            var list = new List<KeyValuePair<string, string>>();
            if (Form != null)
            {
                foreach (var f in Form.AllKeys)
                {
                    list.Add(new KeyValuePair<string, string>(f, Form[f]));
                }
            }
            return list;
        }

        /// <summary>
        /// 处理datatables请求
        /// </summary>
        /// <param name="data"></param>
        private DtResponse ProcessRequest(List<KeyValuePair<string, string>> data,string wfe_id)
        {
            DtResponse dt = new DtResponse();

            var http = DtRequest.HttpData(data);
            if (http.ContainsKey("action"))
            {
                string action = http["action"] as string;
                if (action == "edit")
                {
                    var Data = http["data"] as Dictionary<string, object>;
                    foreach (var d in Data)
                    {
                        int id = Convert.ToInt32(d.Key);
                        List<string> pros = new List<string>();
                        List<object> vals = new List<object>();
                        Dictionary<string, object> m_kv = new Dictionary<string, object>();
                        foreach (var dd in d.Value as Dictionary<string, object>)
                        {
                            pros.Add(dd.Key);
                           vals.Add(dd.Value);

                        }
                        A6dot2Managment AM = new A6dot2Managment();
                        
                        A6dot2LsTaskTab m = AM.UpdateA6dot2LsTask(id, pros, vals);


                        m_kv["ID"] = m.Id;
                        m_kv["Zz_Name"] = m.Zz_Name;
                        m_kv["Equip_Gybh"] = m.Equip_Gybh;
                        m_kv["Equip_Code"] = m.Equip_Code;
                        m_kv["Last_HY"] = m.lastOilTime;
                        m_kv["HY_ZQ"] = m.oilInterval;
                        m_kv["Next_HY"] = m.NextOilTime;
                        m_kv["Problem_Cur"] = m.cur_problem;
                        m_kv["ZG_status"] = m.cur_status;
                        m_kv["Wfe_id"] = m.wfd_id;
                    
                        dt.data.Add(m_kv);
                    }
                }
                else if (action == "create") //新建工作流
                {
                    A6dot2Managment AM = new A6dot2Managment();
                    A6dot2LsTaskTab rt = new A6dot2LsTaskTab();
                    rt.wfd_id = wfe_id;

                    A6dot2LsTaskTab m = AM.AddA6dot2LsTask(rt);

                    Dictionary<string, object> m_kv = new Dictionary<string, object>();
                    m_kv["ID"] = m.Id;
                    m_kv["Zz_Name"] = m.Zz_Name;
                    m_kv["Equip_Gybh"] = m.Equip_Gybh;
                    m_kv["Equip_Code"] = m.Equip_Code;
                    m_kv["Last_HY"] = m.lastOilTime;
                    m_kv["HY_ZQ"] = m.oilInterval;
                    m_kv["Next_HY"] = m.NextOilTime;
                    m_kv["Problem_Cur"] = m.cur_problem;
                    m_kv["ZG_status"] = m.cur_status;
                    m_kv["Wfe_id"] = m.wfd_id;
                    dt.data.Add(m_kv);

                }
                else if (action == "remove")
                {
                    var Data = http["data"] as Dictionary<string, object>;
                    foreach (var d in Data)
                    {
                        int id = Convert.ToInt32(d.Key);

                        A6dot2Managment AM = new A6dot2Managment();
                       

                         AM.RemoveA6dot2LsTask(id);

                    }
                }
            }
            return dt;
        }

        //提交定时任务修改
        [HttpPost]
      

      

        public JsonResult PostChanges(string wfe_id)
        {

            var request = System.Web.HttpContext.Current.Request;
            var list = ParsePostForm(request.Form);
            var dtRes = ProcessRequest(list,wfe_id);


            return Json(dtRes);
        }

        public string LsTaskSubmit(string wfe_id)
        {
            Dictionary<string, string> signal = new Dictionary<string, string>();
            signal["Submit_Done"] = "true";
            Dictionary<string, string> record = new Dictionary<string, string>();
            record["username"] = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
            record["time"] = DateTime.Now.ToString();
            CWFEngine.SubmitSignal(Convert.ToInt32(wfe_id), signal, record);
            return ("/TempJob/Index");

        }

        public string LsTaskList(string wfe_id)
        {  //string wfe_id="[1,2]";
        EquipArchiManagment Em = new EquipArchiManagment();
            List<Object> r = new List<Object>();
            JArray item = (JArray)JsonConvert.DeserializeObject(wfe_id);
            int ii = 1;
            foreach (var i in item)
            {
               

                string workflow_entity;
                workflow_entity = i.ToString();

                Dictionary<string, object> paras = new Dictionary<string, object>();
                paras["Cj_Name"] = null;
                paras["Job_Name"] = null;


                paras["Submit_Done"] = null;
                UI_WFEntity_Info wfei = CWFEngine.GetWorkFlowEntityWithParams(Convert.ToInt32(workflow_entity), paras);
               
                object o = new
                {  ID=ii,
                   
                   workflow_ser = wfei.serial,
                   workflow_name = wfei.name,
                   cj_name = Em.getEa_namebyId(Convert.ToInt16(paras["Cj_Name"])),
                   Zt_unit = "",
                   status = (int)wfei.Status,
                   workflow_id = workflow_entity

                };
                r.Add(o);
                ii = ii + 1;
               

            }

            string str = JsonConvert.SerializeObject(r);
            return ("{" + "\"data\": " + str + "}");

        }
   public ActionResult LsTaskManager(string job_name,string run_result)
      {
          ViewBag.job_name= job_name;
          ViewBag.wfe_id = run_result;
          return View();
            

        }
       
    }
}
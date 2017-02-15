using FlowEngine;
using FlowEngine.DAL;
using FlowEngine.Modals;

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using WebApp.Models.DateTables;

namespace WebApp.Controllers
{
    public class A5TimeJobController : Controller
    {
        //
        // GET: /A5TimeJob/
        public ActionResult Index()
        {
           // ViewBag.flows = CWFEngine.ListAllWFDefine();

            return View();
        }
        public ActionResult A6dot2TimerJobs()
        {
            ViewBag.flows = CWFEngine.ListAllWFDefine();

            return View();
        }

        public JsonResult A5dot1GetTimerJobs()
        {
            List<object> res = new List<object>();

            for (int i = 0; i < 2; i++)
            {
                object o = new
                {
                    ID = i + 1,
                    zz_name = "1#常压装置",
                    jz_name = "1#常压常顶空冷风机机B",
                    sb_code = "210000001",
                    last_check_month = "12",
                    check_result = "",
                    zz_file = "",
                   
                };

                res.Add(o);
            }

            return Json(new { data = res.ToArray() });
        }
        public JsonResult A6dot2GetTimerJobs()
        {
            List<object> res = new List<object>();

            for (int i = 0; i < 2; i++)
            {
                object o = new
                {
                    ID = i + 1,
                    zz_name = "1#常压装置",
                    jz_name = "1#常压常顶空冷风机机B",
                    sb_code = "210000001",
                    last_check_month = "12",
                    check_result = "",
                    zz_file = "",
                    zz_done = "",
                    second_check_result = ""
                };

                res.Add(o);
            }

            return Json(new { data = res.ToArray() });
        }

        private DtResponse A5ProcessRequest(List<KeyValuePair<string, string>> data)
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
                        //   CTimerMission m = CTimerManage.UpdateTimerMission(id, pros, vals);

                        m_kv["ID"] = "12345";
                        m_kv["zz_name"] = "2#催化装置";
                        m_kv["jz_name"] = "2#催化主风机K101";
                        m_kv["sb_code"] = "210023808";
                        m_kv["last_check_month"] = "4";
                        m_kv["check_result"] = "5";
                        m_kv["zz_file"] = "";
                   



                        dt.data.Add(m_kv);
                    }
                }
                else if (action == "create") //新建工作流
                {

                    //CTimerMission m = CTimerManage.CreateAEmptyMission();

                    Dictionary<string, object> m_kv = new Dictionary<string, object>();
                    m_kv["ID"] = "123";
                    m_kv["zz_name"] = "选择装置";
                    m_kv["jz_name"] = "选择机组";
                    m_kv["sb_code"] = " ";
                    m_kv["last_check_month"] = " ";
                    m_kv["check_result"] = "";
                    m_kv["zz_file"] = "";
                   
                    dt.data.Add(m_kv);

                }
                else if (action == "remove")
                {
                    var Data = http["data"] as Dictionary<string, object>;
                    foreach (var d in Data)
                    {
                        int id = Convert.ToInt32(d.Key);

                        //CTimerMission m = CTimerManage.DeleteTimerJob(id);

                    }
                }
            }
            return dt;
        }
        private DtResponse ProcessRequest1(List<KeyValuePair<string, string>> data)
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
                        //   CTimerMission m = CTimerManage.UpdateTimerMission(id, pros, vals);

                        m_kv["ID"] = "12345";
                        m_kv["zz_name"] = "1";
                        m_kv["jz_name"] = "22";
                        m_kv["sb_code"] = "3";
                        m_kv["last_check_month"] = "4";
                        m_kv["check_result"] = "5";
                        m_kv["zz_file"] = "";
                        m_kv["zz_done"] = "";
                        m_kv["second_check_result"] = "";



                        dt.data.Add(m_kv);
                    }
                }
                else if (action == "create") //新建工作流
                {

                   // CTimerMission m = CTimerManage.CreateAEmptyMission();

                    Dictionary<string, object> m_kv = new Dictionary<string, object>();
                    m_kv["ID"] = "12";
                    m_kv["zz_name"] = "选择装置";
                    m_kv["jz_name"] = "选择机组";
                    m_kv["sb_code"] = " ";
                    m_kv["last_check_month"] = " ";
                    m_kv["check_result"] = "";
                    m_kv["zz_file"] = "";
                    m_kv["zz_done"] = "";
                    m_kv["second_check_result"] = "";
                    dt.data.Add(m_kv);

                }
                else if (action == "remove")
                {
                    var Data = http["data"] as Dictionary<string, object>;
                    foreach (var d in Data)
                    {
                        int id = Convert.ToInt32(d.Key);

                //    CTimerMission m = CTimerManage.DeleteTimerJob(id);

                    }
                }
            }
            return dt;
        }

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
        public JsonResult A5PostChanges()
        {

            var request = System.Web.HttpContext.Current.Request;
            var list = ParsePostForm(request.Form);
            var dtRes = A5ProcessRequest(list);


            return Json(dtRes);
        }
        public JsonResult PostChanges1()
        {

            var request = System.Web.HttpContext.Current.Request;
            var list = ParsePostForm(request.Form);
            var dtRes = ProcessRequest1(list);


            return Json(dtRes);
        }

	}
}
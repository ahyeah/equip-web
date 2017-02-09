using FlowEngine;
using FlowEngine.DAL;
using FlowEngine.Modals;
using FlowEngine.TimerManage;
using Newtonsoft.Json;
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
using EquipBLL.AdminManagment;
using EquipBLL.AdminManagment.MenuConfig;

namespace WebApp.Controllers
{
    public class TempJobController : CommonController
    {
        WorkFlows wfs = new WorkFlows();
        EquipArchiManagment Em = new EquipArchiManagment();
        // GET: JobsManagement
        public ActionResult Index()
        {
            int UserId = (Session["User"] as EquipModel.Entities.Person_Info).Person_Id;
            PersonManagment pm = new PersonManagment();
            EquipBLL.AdminManagment.PersonManagment.P_viewModal pv = pm.Get_PersonModal(UserId);
           
            if (pv.Role_Names.Contains("专业团队") || pv.Role_Names.Contains("专家团队") || pv.Role_Names.Contains("专业团队负责人"))
               ViewBag.zytd = 1;
            else
                ViewBag.zytd = 0;           
            return View();
        }

        public ActionResult A6dot2Mission(string wfe_id)
        {

            ViewBag.flows = CWFEngine.ListAllWFDefine();

            return View();
        }
        public ActionResult createmission(string wfe_id)
        {


            ViewBag.flows = CWFEngine.ListAllWFDefine();

            return View(getSubmitModel(wfe_id));
        }

        [HttpPost]
        public JsonResult GetTimerJobs()
        {
            List<CTimerMission> miss = CTimerManage.GetTimerJobsForCustom();
            List<object> res = new List<object>();

            for (int i = 0; i < miss.Count; i++)
            {
                var m = miss[i];
                CWorkFlow wf = m.GetAttachWorkFlow();
                object o = new
                {
                    ID = m.ID,
                    job_name = m.mission_name,
                    job_type = m.type,
                    order_item = i + 1,
                    workflow = wf == null ? null : "{ \"id\" : " + wf.DefineID + ", \"name\" : \"" + wf.name + "\", \"desc\" : \"" + wf.description + "\"}",
                    status = m.status,
                    pretime = m.PerTime.ToString(),
                    corn_express = m.GetTriggerTimmingString(),
                    create_time = m.CreateTime.ToString()
                };
                res.Add(o);
            }

            return Json(new { data = res.ToArray() });
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
        private DtResponse ProcessRequest(List<KeyValuePair<string, string>> data)
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
                            if (dd.Key == "job_type")
                            {
                                switch (dd.Value as string)
                                {
                                    case "CreateWorkFlow":
                                        vals.Add(TIMER_JOB_TYPE.CREATE_WORKFLOW);
                                        break;

                                    default:
                                    case "Empty":
                                        vals.Add(TIMER_JOB_TYPE.EMPTY);
                                        break;
                                }
                            }
                            else if (dd.Key == "workflow")
                            {
                                JObject obj = JObject.Parse((string)dd.Value);
                                vals.Add(Convert.ToInt32(obj["id"]));
                            }
                            else
                                vals.Add(dd.Value);

                        }
                        CTimerMission m = CTimerManage.UpdateTimerMission(id, pros, vals);


                        m_kv["ID"] = m.ID;
                        m_kv["job_name"] = m.mission_name;
                        m_kv["job_type"] = m.type;
                        CWorkFlow wf = m.GetAttachWorkFlow();

                        m_kv["workflow"] = wf == null ? null : "{ \"id\" : " + wf.DefineID + ", \"name\" : \"" + wf.name + "\", \"desc\" : \"" + wf.description + "\"}";
                        m_kv["status"] = m.status;
                        m_kv["pretime"] = m.PerTime.ToString();
                        m_kv["corn_express"] = m.GetTriggerTimmingString();
                        m_kv["create_time"] = m.CreateTime.ToString();
                        dt.data.Add(m_kv);
                    }
                }
                else if (action == "create") //新建工作流
                {
                    CTimerMission m = CTimerManage.CreateAEmptyMission();
                    m.mission_name = "新建任务";
                    m.SetTriggerTiming("0 0 0 * * ?");
                    m.Save();

                    Dictionary<string, object> m_kv = new Dictionary<string, object>();
                    m_kv["ID"] = m.ID;
                    m_kv["order_item"] = 1;
                    m_kv["job_name"] = m.mission_name;
                    m_kv["job_type"] = m.type;
                    CWorkFlow wf = m.GetAttachWorkFlow();

                    m_kv["workflow"] = wf == null ? null : "{ \"id\" : " + wf.DefineID + ", \"name\" : \"" + wf.name + "\", \"desc\" : \"" + wf.description + "\"}";
                    m_kv["status"] = m.status;
                    m_kv["pretime"] = m.PerTime.ToString();
                    m_kv["corn_express"] = m.GetTriggerTimmingString();
                    m_kv["create_time"] = m.CreateTime.ToString();
                    dt.data.Add(m_kv);

                }
                else if (action == "remove")
                {
                    var Data = http["data"] as Dictionary<string, object>;
                    foreach (var d in Data)
                    {
                        int id = Convert.ToInt32(d.Key);

                        CTimerMission m = CTimerManage.DeleteTimerJob(id);

                    }
                }
            }
            return dt;
        }

        //提交定时任务修改
        [HttpPost]
        public JsonResult PostChanges()
        {

            var request = System.Web.HttpContext.Current.Request;
            var list = ParsePostForm(request.Form);
            var dtRes = ProcessRequest(list);


            return Json(dtRes);
        }

        //提交变量修改
        //在这里仅做一个回应，不写到数据库
        //待“确定”时整体提交
        [HttpPost]
        public JsonResult PostParChanges()
        {
            var request = System.Web.HttpContext.Current.Request;
            DtResponse dt = new DtResponse();

            var list = ParsePostForm(request.Form);
            var http = DtRequest.HttpData(list);

            if (http.ContainsKey("action"))
            {
                string action = http["action"] as string;
                if (action == "edit")
                {
                    var Data = http["data"] as Dictionary<string, object>;
                    foreach (var d in Data)
                    {

                        Dictionary<string, object> m_kv = new Dictionary<string, object>();
                        m_kv["par_name"] = d.Key;
                        foreach (var dd in d.Value as Dictionary<string, object>)
                        {
                            m_kv[dd.Key] = dd.Value;
                        }

                        dt.data.Add(m_kv);
                    }
                }
            }
            return Json(dt);
        }

        [HttpPost]
        public JsonResult ListKeyParams(int defId)
        {
            List<object> pars = new List<object>();
            if (defId == -1)
            {
                return Json(new { data = pars.ToArray() });
            }

            CWorkFlow wf = new CWorkFlow();
            WorkFlows wfs = new WorkFlows();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(Encoding.Default.GetString(wfs.GetWorkFlowDefineByID(defId).W_Xml));
            wf.InstFromXmlNode((XmlNode)doc.DocumentElement);

            var ev = wf.events["Start"];
            foreach (var k in ev.paramlist)
            {
                pars.Add(new
                {
                    par_name = k.Value.name,
                    par_desc = k.Value.description,
                    par_value = ""
                });
            }

            return Json(new { data = pars.ToArray() });
        }
        //设置临时任务
        public string LSJob_Submit(string json1)
        {
            JObject item = (JObject)JsonConvert.DeserializeObject(json1);
            string WorkFlowName = item["Work_Name"].ToString();
            // WorkFlows ws = new WorkFlows();
            int workflow_id = wfs.GetWorkFlowDefine(WorkFlowName).W_ID;
            string cj_ids = item["Cj_Name"].ToString();
            string Zz_ids = item["Zz_Name"].ToString();
            string[] cjids = cj_ids.Split(new char[] { ',' });
            string[] zzids = Zz_ids.Split(new char[] { ',' });
            string Depts = item["Dept"].ToString();
            string qr_endtime = item["ZhengGaiTime"].ToString();
            CTimerCreateWF m = new CTimerCreateWF();

            //使用lambda表达式过滤掉空字符串
            zzids = zzids.Where(t => !string.IsNullOrEmpty(t)).ToArray();

            //这里需要创建一个回调函数
            string TempJobName = item["Job_Name"].ToString();

            string corn = "0 0 0 * * ?";
            string ReservationTime = item["ReservationTime"].ToString();


            m.Set_Res_Value("STR_RES_1", Depts);

            string[] s = ReservationTime.Split(new char[] { '-' });

            string[] ss = s[0].Split(new char[] { '/' });
            string endtime = s[1].Replace(" ", "");
            DateTime Endtime = DateTime.Parse(endtime);
            m.Set_Res_Value("STR_RES_2", Endtime);
            //11.12改
            if (qr_endtime != "")
                m.Set_Res_Value("STR_RES_3", DateTime.Parse(qr_endtime));

            corn = "0 0 0 " + ss[2] + "" + ss[1] + " " + "? " + ss[0];

            //未写
            if (WorkFlowName == "A6dot2")
            {
                for (int i = 0; i < cjids.Count(); i++)
                {
                    TimerCreateWFPa TCP = new TimerCreateWFPa();//
                    Dictionary<string, string> param = new Dictionary<string, string>();
                    Dictionary<string, string> record = new Dictionary<string, string>();
                    param.Add("Cj_Name", Em.getEa_namebyId(Convert.ToInt16(cjids[i])));
                    record.Add("username", "system_temporary");
                    record.Add("time", DateTime.Now.ToString());
                    TCP.wf_params = param;
                    TCP.wf_record = record;
                    m.AppendCreateParam(TCP);

                    TimerCreateWFPa.TimerSetting time_set = new TimerCreateWFPa.TimerSetting();
                    time_set.Time_start = "wf_create";
                    time_set.Exact_time = Endtime.ToString();
                    time_set.Offset_time = "";
                    time_set.Action = "SUSPEND";
                    time_set.Call_back = "";
                    TCP.AppendTimer("Xc_Sample", time_set);
                }


            }
            else if (WorkFlowName == "A5dot1")
            {
                for (int i = 0; i < zzids.Count(); i++)
                {
                    TimerCreateWFPa TCP = new TimerCreateWFPa();//
                    Dictionary<string, string> param = new Dictionary<string, string>();
                    Dictionary<string, string> record = new Dictionary<string, string>();
                    param.Add("Zz_Name", Em.getEa_namebyId(Convert.ToInt16(zzids[i])));
                    record.Add("username", "system_temporary");
                    record.Add("time", DateTime.Now.ToString());
                    TCP.wf_params = param;
                    TCP.wf_record = record;
                    m.AppendCreateParam(TCP);

                    TimerCreateWFPa.TimerSetting time_set = new TimerCreateWFPa.TimerSetting();
                    time_set.Time_start = "wf_create";
                    time_set.Exact_time = Endtime.ToString();
                    time_set.Offset_time = "";
                    time_set.Action = "SUSPEND";
                    time_set.Call_back = "";
                    TCP.AppendTimer("ZzSubmit", time_set);
                }
            }
            else if (WorkFlowName == "A11dot2dot1")
            {
                for (int i = 0; i < zzids.Count(); i++)
                {
                    TimerCreateWFPa TCP = new TimerCreateWFPa();//
                    Dictionary<string, string> param = new Dictionary<string, string>();
                    Dictionary<string, string> record = new Dictionary<string, string>();
                    param.Add("Zz_Name", Em.getEa_namebyId(Convert.ToInt16(zzids[i])));
                    record.Add("username", "system_temporary");
                    record.Add("time", DateTime.Now.ToString());
                    TCP.wf_params = param;
                    TCP.wf_record = record;
                    m.AppendCreateParam(TCP);

                    TimerCreateWFPa.TimerSetting time_set = new TimerCreateWFPa.TimerSetting();
                    time_set.Time_start = "wf_create";
                    time_set.Exact_time = Endtime.ToString();
                    time_set.Offset_time = "";
                    time_set.Action = "SUSPEND";
                    time_set.Call_back = "";
                    TCP.AppendTimer("ZzSubmit", time_set);
                }
            }
            else if (WorkFlowName == "A6dot2dot2")
            {
                for (int i = 0; i < cjids.Count(); i++)
                {
                    TimerCreateWFPa TCP = new TimerCreateWFPa();//
                    Dictionary<string, string> param = new Dictionary<string, string>();
                    Dictionary<string, string> record = new Dictionary<string, string>();
                    param.Add("Cj_Name", cjids[i]);
                    param.Add("Job_Name", TempJobName);
                    param.Add("Job_Ztdanwei", Depts);
                    record.Add("username", "system_temporary");
                    record.Add("time", DateTime.Now.ToString());
                    TCP.wf_params = param;
                    TCP.wf_record = record;
                    m.AppendCreateParam(TCP);

                    TimerCreateWFPa.TimerSetting time_set = new TimerCreateWFPa.TimerSetting();
                    time_set.Time_start = "wf_create";
                    time_set.Exact_time = Endtime.ToString();
                    time_set.Offset_time = "";
                    time_set.Action = "SUSPEND";
                    time_set.Call_back = "";
                    TCP.AppendTimer("ZzSubmit", time_set);
                }


            }
            else if (WorkFlowName == "A5dot2dot1")
            {
                for (int i = 0; i < zzids.Count(); i++)
                {
                    TimerCreateWFPa TCP = new TimerCreateWFPa();//
                    Dictionary<string, string> param = new Dictionary<string, string>();
                    Dictionary<string, string> record = new Dictionary<string, string>();
                    param.Add("Zz_Name", Em.getEa_namebyId(Convert.ToInt16(zzids[i])));
                    record.Add("username", "system_temporary");
                    record.Add("time", DateTime.Now.ToString());
                    TCP.wf_params = param;
                    TCP.wf_record = record;
                    m.AppendCreateParam(TCP);

                    TimerCreateWFPa.TimerSetting time_set = new TimerCreateWFPa.TimerSetting();
                    time_set.Time_start = "wf_create";
                    time_set.Exact_time = Endtime.ToString();
                    time_set.Offset_time = "";
                    time_set.Action = "SUSPEND";
                    time_set.Call_back = "";
                    TCP.AppendTimer("ZzSubmit", time_set);
                }


            }

            m.for_using = TIMER_USING.FOR_CUSTOM;
            m.CreateTime = DateTime.Now;
            m.SetTriggerTiming(corn);
            m.mission_name = item["Job_Name"].ToString();
            m.status = TM_STATUS.TM_STATUS_ACTIVE;
            //m.CreateCallback = "/zxhtest/QxFunction?depts="+Depts+"";//和权限有关的回调函数
            m.CustomFlag = 1;

            WorkFlow_Define wfd = wfs.GetWorkFlowDefineByID(workflow_id);

            m.attachTarget(wfd);
            m.GetAttachWorkFlow();//0910
            m.Save();

            CTimerManage.ActiveListActionForMission(m);
            return "/TempJob/index";

        }

        //获取临时待处理任务
        public string getJobList()
        {
            Jobs m = new Jobs();
            int UserId = (Session["User"] as EquipModel.Entities.Person_Info).Person_Id;
            PersonManagment pm = new PersonManagment();
            EquipBLL.AdminManagment.PersonManagment.P_viewModal pv = pm.Get_PersonModal(UserId);
            int zytd;
            if (pv.Role_Names.Contains("专业团队") || pv.Role_Names.Contains("专家团队") || pv.Role_Names.Contains("专业团队负责人"))
                zytd = 1;
            else
                zytd = 0; 
            List<Timer_Jobs> Joblist = m.GetAllTimerJob();

            List<object> r = new List<object>();
            for (int i = 0; i < Joblist.Count; i++)
            {
                if (Joblist[i].custom_flag == 1 && Joblist[i].Job_Type == TIMER_JOB_TYPE.CREATE_WORKFLOW)
                {

                    List<string> cjnames = new List<string>();
                    string Cj_Names = "";
                    JArray jsonVal = JArray.Parse(Joblist[i].run_params) as JArray;
                    dynamic table2 = jsonVal;
                    foreach (dynamic T in table2)
                    {
                        WorkFlow_Define wfd = wfs.GetWorkFlowDefineByID(Joblist[i].workflow_ID);
                        if (wfd.W_Name == "A6dot2dot2")
                        {
                            // JObject item = (JObject)JsonConvert.DeserializeObject(T.PARAMS);

                            string cjid = T.PARAMS.Cj_Name.ToString();
                            string cj_name = Em.getEa_namebyId(Convert.ToInt16(cjid));
                            cjnames.Add(cj_name);
                        }
                        else
                        {
                            foreach (dynamic t in T.PARAMS)
                            {
                                string cjtemp = "";
                                cjtemp = t.Value;




                                cjnames.Add(cjtemp);

                            }

                        }


                    }
                    for (int k = 0; k < cjnames.Count; k++)
                    {
                        EquipArchiManagment em = new EquipArchiManagment();
                        // Cj_Names = Cj_Names + em.getEa_namebyId(Convert.ToInt16(cjnames[k])) + "、";
                        Cj_Names = Cj_Names + cjnames[k] + "、";
                    }
                    //string job_time = "";
                    //string[] job_timelist = Joblist[i].corn_express.Split(new char[] { ' ' });
                    //job_time = job_timelist[6] + "年" + job_timelist[4] + "月" + job_timelist[3] + "日";
                    string wf_name = wfs.GetWorkFlowDefineByID(Joblist[i].workflow_ID).W_Name + ":" + wfs.GetWorkFlowDefineByID(Joblist[i].workflow_ID).W_Attribution;
                    string job_desc = wfs.GetWorkFlowDefineByID(Joblist[i].workflow_ID).W_Name;


                    object o = new
                    {
                        job_id = Joblist[i].JOB_ID,
                        index = i + 1,
                        jobName = Joblist[i].job_name,
                        jobType = wf_name,
                        job_desc = job_desc,
                        jobTIme = Joblist[i].STR_RES_2,
                        jobRunPara = Cj_Names,
                        jobStatus = Joblist[i].status,
                        job_dep = Joblist[i].STR_RES_1,
                        job_result = Joblist[i].run_result,
                        iszytd=zytd

                    };
                    r.Add(o);


                }

            }
            string str = JsonConvert.SerializeObject(r);
            return ("{" + "\"data\": " + str + "}");





        }


    }
}
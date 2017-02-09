using EquipBLL.AdminManagment;
using EquipBLL.AdminManagment.MenuConfig;
using EquipModel.Entities;
using FlowEngine.DAL;
using FlowEngine.Modals;
using FlowEngine.TimerManage;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class DingShiController : CommonController
    {
        //
        // GET: /DingShi/
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
        public ActionResult Submit(string wfe_id)
        {
            return View(getSubmitModel(wfe_id));
        }
        WorkFlows wfs = new WorkFlows();
        EquipArchiManagment Em = new EquipArchiManagment();
        public string DSJob_Submit(string json1)
        {
            JObject item = (JObject)JsonConvert.DeserializeObject(json1);
            string WorkFlowName = item["Work_Name"].ToString();
            // WorkFlows ws = new WorkFlows();
            int workflow_id = wfs.GetWorkFlowDefine(WorkFlowName).W_ID;
            Jobs job = new Jobs();
            List<Timer_Jobs> job_list = job.GetDSbyWorkflow(workflow_id);
            if (job_list.Count == 0)
            {
                string cj_ids = item["Cj_Name"].ToString();
                string Zz_ids = item["Zz_Name"].ToString();
                string[] cjids = cj_ids.Split(new char[] { ',' });
                string[] zzids = Zz_ids.Split(new char[] { ',' });
                CTimerCreateWF m = new CTimerCreateWF();
                //这里需要创建一个回调函数
                string DingShiJobName = item["Job_Name"].ToString();

                //使用lambda表达式过滤掉空字符串
                zzids = zzids.Where(s => !string.IsNullOrEmpty(s)).ToArray();
                //未写
                if (WorkFlowName == "A6dot2")
                {
                    for (int i = 0; i < cjids.Count(); i++)
                    {
                        TimerCreateWFPa TCP = new TimerCreateWFPa();//
                        Dictionary<string, string> param = new Dictionary<string, string>();
                        Dictionary<string, string> record = new Dictionary<string, string>();
                        param.Add("Cj_Name", Em.getEa_namebyId(Convert.ToInt16(cjids[i])));
                        record.Add("username", "system_scheduled");
                        record.Add("time", DateTime.Now.ToString());
                        TCP.wf_params = param;
                        TCP.wf_record = record;
                        m.AppendCreateParam(TCP);

                        TimerCreateWFPa.TimerSetting time_set = new TimerCreateWFPa.TimerSetting();
                        time_set.Time_start = "wf_create";
                        time_set.Exact_time = "";
                        time_set.Offset_time = (DateTime.Now.AddDays(5) - DateTime.Now).ToString();
                        //time_set.Offset_time = (DateTime.Now.AddMinutes(3) - DateTime.Now).ToString();
                        time_set.Action = "INVILID";
                        time_set.Call_back = "http://localhost/CallBack/testCallBack";
                        TCP.AppendTimer("Xc_Sample", time_set);
                    }


                }
                if (WorkFlowName == "A15dot1")
                {
                    List<string> pq_list = new List<string>();
                    pq_list.Add("联合一片区");
                    pq_list.Add("联合二片区");
                    pq_list.Add("联合三片区");
                    pq_list.Add("联合四片区");
                    pq_list.Add("化工片区");
                    pq_list.Add("综合片区");
                    pq_list.Add("其他");
                    //pq_list.Add("检修单位");
                    for (int i = 0; i < pq_list.Count(); i++)
                    {
                        TimerCreateWFPa TCP = new TimerCreateWFPa();//
                        Dictionary<string, string> param = new Dictionary<string, string>();
                        Dictionary<string, string> record = new Dictionary<string, string>();
                        param.Add("Pqname", pq_list[i]);
                        record.Add("username", "system_scheduled");
                        record.Add("time", DateTime.Now.ToString());
                        TCP.wf_params = param;
                        TCP.wf_record = record;
                        m.AppendCreateParam(TCP);


                        TimerCreateWFPa.TimerSetting time_set = new TimerCreateWFPa.TimerSetting();
                        time_set.Time_start = "wf_create";
                        time_set.Exact_time = "";
                        time_set.Offset_time = (DateTime.Now.AddDays(1) - DateTime.Now).ToString();
                        //time_set.Offset_time = (DateTime.Now.AddMinutes(3) - DateTime.Now).ToString();
                        time_set.Action = "INVILID";
                        time_set.Call_back = "http://localhost/CallBack/testCallBack";
                        TCP.AppendTimer("ZzSubmit", time_set);

                    }


                }
                else if (WorkFlowName == "A5dot1dot2")
                {
                    for (int i = 0; i < zzids.Count(); i++)
                    {
                        TimerCreateWFPa TCP = new TimerCreateWFPa();//
                        Dictionary<string, string> param = new Dictionary<string, string>();
                        Dictionary<string, string> record = new Dictionary<string, string>();
                        param.Add("Zz_Name", Em.getEa_namebyId(Convert.ToInt16(zzids[i])));
                        record.Add("username", "system_scheduled");
                        record.Add("time", DateTime.Now.ToString());
                        TCP.wf_params = param;
                        TCP.wf_record = record;
                        m.AppendCreateParam(TCP);

                        TimerCreateWFPa.TimerSetting time_set = new TimerCreateWFPa.TimerSetting();
                        time_set.Time_start = "wf_create";
                        time_set.Exact_time = "";
                        time_set.Offset_time = (DateTime.Now.AddDays(5) - DateTime.Now).ToString();
                        //time_set.Offset_time = (DateTime.Now.AddMinutes(3) - DateTime.Now).ToString();
                        time_set.Action = "INVILID";
                        time_set.Call_back = "http://localhost/CallBack/testCallBack";
                        TCP.AppendTimer("ZzSubmit", time_set);
                    }
                }
                else if (WorkFlowName == "A5dot2dot2")
                {
                    for (int i = 0; i < zzids.Count(); i++)
                    {
                        TimerCreateWFPa TCP = new TimerCreateWFPa();//
                        Dictionary<string, string> param = new Dictionary<string, string>();
                        Dictionary<string, string> record = new Dictionary<string, string>();
                        param.Add("Zz_Name", Em.getEa_namebyId(Convert.ToInt16(zzids[i])));
                        record.Add("username", "system_scheduled");
                        record.Add("time", DateTime.Now.ToString());
                        TCP.wf_params = param;
                        TCP.wf_record = record;
                        m.AppendCreateParam(TCP);

                        TimerCreateWFPa.TimerSetting time_set = new TimerCreateWFPa.TimerSetting();
                        time_set.Time_start = "wf_create";
                        time_set.Exact_time = "";
                        time_set.Offset_time = (DateTime.Now.AddDays(5) - DateTime.Now).ToString();
                        //time_set.Offset_time = (DateTime.Now.AddMinutes(3) - DateTime.Now).ToString();
                        time_set.Action = "INVILID";
                        time_set.Call_back = "http://localhost/CallBack/testCallBack";
                        TCP.AppendTimer("ZzSubmit", time_set);
                    }


                }
                else if (WorkFlowName == "A11dot2dot2")
                {
                    for (int i = 0; i < zzids.Count(); i++)
                    {
                        TimerCreateWFPa TCP = new TimerCreateWFPa();//
                        Dictionary<string, string> param = new Dictionary<string, string>();
                        Dictionary<string, string> record = new Dictionary<string, string>();
                        param.Add("Zz_Name", Em.getEa_namebyId(Convert.ToInt16(zzids[i])));
                        record.Add("username", "system_scheduled");
                        record.Add("time", DateTime.Now.ToString());
                        TCP.wf_params = param;
                        TCP.wf_record = record;
                        m.AppendCreateParam(TCP);

                        TimerCreateWFPa.TimerSetting time_set = new TimerCreateWFPa.TimerSetting();
                        time_set.Time_start = "wf_create";
                        time_set.Exact_time = "";
                        time_set.Offset_time = (DateTime.Now.AddDays(5) - DateTime.Now).ToString();
                        //time_set.Offset_time = (DateTime.Now.AddMinutes(3) - DateTime.Now).ToString();
                        time_set.Action = "INVILID";
                        time_set.Call_back = "http://localhost/CallBack/testCallBack";
                        TCP.AppendTimer("ZzSubmit", time_set);
                    }
                }
                if (WorkFlowName == "A12dot2dot1")
                {
                    for (int i = 0; i < zzids.Count(); i++)
                    {
                        TimerCreateWFPa TCP = new TimerCreateWFPa();//
                        Dictionary<string, string> param = new Dictionary<string, string>();
                        Dictionary<string, string> record = new Dictionary<string, string>();
                        param.Add("Zz_Name", Em.getEa_namebyId(Convert.ToInt16(zzids[i])));
                        record.Add("username", "system_scheduled");
                        record.Add("time", DateTime.Now.ToString());
                        TCP.wf_params = param;
                        TCP.wf_record = record;
                        m.AppendCreateParam(TCP);

                        TimerCreateWFPa.TimerSetting time_set = new TimerCreateWFPa.TimerSetting();
                        time_set.Time_start = "wf_create";
                        time_set.Exact_time = "";
                        time_set.Offset_time = (DateTime.Now.AddDays(5) - DateTime.Now).ToString();
                        time_set.Action = "INVILID";
                        time_set.Call_back = "http://localhost/CallBack/testCallBack";
                        TCP.AppendTimer("ZzSubmit", time_set);
                    }


                }
                else if (WorkFlowName == "A6dot3")
                {
                    for (int i = 0; i < cjids.Count(); i++)
                    {
                        TimerCreateWFPa TCP = new TimerCreateWFPa();//
                        Dictionary<string, string> param = new Dictionary<string, string>();
                        Dictionary<string, string> record = new Dictionary<string, string>();
                        param.Add("Zz_Name", Em.getEa_namebyId(Convert.ToInt16(zzids[i])));
                        record.Add("username", "system_scheduled");
                        record.Add("time", DateTime.Now.ToString());
                        TCP.wf_params = param;
                        TCP.wf_record = record;
                        m.AppendCreateParam(TCP);

                        TimerCreateWFPa.TimerSetting time_set = new TimerCreateWFPa.TimerSetting();
                        time_set.Time_start = "wf_create";
                        time_set.Exact_time = "";
                        time_set.Offset_time = (DateTime.Now.AddDays(1) - DateTime.Now).ToString();
                        time_set.Action = "INVILID";
                        time_set.Call_back = "http://localhost/CallBack/testCallBack";
                        TCP.AppendTimer("Ineligible_Submit", time_set);
                    }


                }
                else if (WorkFlowName == "A7dot1dot1")
                {
                    List<Equip_Info> Th_sb = new List<Equip_Info>();
                    EquipManagment Epm = new EquipManagment();
                    Th_sb = Epm.getAllThEquips();
                    for (int i = 0; i < Th_sb.Count(); i++)
                    {
                        TimerCreateWFPa TCP = new TimerCreateWFPa();//
                        Dictionary<string, string> param = new Dictionary<string, string>();
                        Dictionary<string, string> record = new Dictionary<string, string>();
                        param.Add("Equip_GyCode", Th_sb[i].Equip_GyCode);
                        record.Add("username", "system_scheduled");
                        record.Add("time", DateTime.Now.ToString());
                        TCP.wf_params = param;
                        TCP.wf_record = record;
                        m.AppendCreateParam(TCP);

                        TimerCreateWFPa.TimerSetting time_set = new TimerCreateWFPa.TimerSetting();
                        time_set.Time_start = "wf_create";
                        time_set.Exact_time = "";
                        time_set.Offset_time = (DateTime.Now.AddDays(1) - DateTime.Now).ToString();
                        time_set.Action = "INVILID";
                        time_set.Call_back = "http://localhost/CallBack/testCallBack";
                        TCP.AppendTimer("ZzSubmit", time_set);
                    }
                }
                else if (WorkFlowName == "A14dot2")
                {
                    for (int i = 0; i < zzids.Count(); i++)
                    {
                        TimerCreateWFPa TCP = new TimerCreateWFPa();//
                        Dictionary<string, string> param = new Dictionary<string, string>();
                        Dictionary<string, string> record = new Dictionary<string, string>();
                        param.Add("Zz_Name", Em.getEa_namebyId(Convert.ToInt16(zzids[i])));
                        record.Add("username", "system_scheduled");
                        record.Add("time", DateTime.Now.ToString());
                        TCP.wf_params = param;
                        TCP.wf_record = record;
                        m.AppendCreateParam(TCP);

                        TimerCreateWFPa.TimerSetting time_set = new TimerCreateWFPa.TimerSetting();
                        time_set.Time_start = "wf_create";
                        time_set.Exact_time = "";
                        time_set.Action = "INVILID";
                        time_set.Call_back = "http://localhost/CallBack/testCallBack";
                        time_set.Offset_time = (DateTime.Now.AddDays(5) - DateTime.Now).ToString();
                        TCP.AppendTimer("JxdwSubmit", time_set);
                        //TCP.AppendTimer("ZzConfirm", time_set);
                    }
                }
                else if (WorkFlowName == "A14dot3dot2")
                {
                    for (int i = 0; i < zzids.Count(); i++)
                    {
                        TimerCreateWFPa TCP = new TimerCreateWFPa();//
                        Dictionary<string, string> param = new Dictionary<string, string>();
                        Dictionary<string, string> record = new Dictionary<string, string>();
                        param.Add("Zz_Name", Em.getEa_namebyId(Convert.ToInt16(zzids[i])));
                        record.Add("username", "system_scheduled");
                        record.Add("time", DateTime.Now.ToString());
                        TCP.wf_params = param;
                        TCP.wf_record = record;
                        m.AppendCreateParam(TCP);

                        TimerCreateWFPa.TimerSetting time_set = new TimerCreateWFPa.TimerSetting();
                        time_set.Time_start = "wf_create";
                        time_set.Exact_time = "";
                        time_set.Offset_time = (DateTime.Now.AddDays(5) - DateTime.Now).ToString();
                        time_set.Action = "SUSPEND";
                        time_set.Call_back = "http://localhost/CallBack/testCallBack";
                        TCP.AppendTimer("ZzSubmit", time_set);
                    }
                }




                string corn = "0 0 0 * * ?";
                string corn_express = item["corn_express"].ToString();
                string ReservationTime = item["reservationtime"].ToString();
                m.Set_Res_Value("STR_RES_2", ReservationTime);
                corn = corn_express;
                m.for_using = TIMER_USING.FOR_CUSTOM;
                m.CreateTime = DateTime.Now;
                m.SetTriggerTiming(corn);
                m.mission_name = item["Job_Name"].ToString();
                m.status = TM_STATUS.TM_STATUS_ACTIVE;
                //m.CreateCallback = "/zxhtest/QxFunction?depts="+Depts+"";//和权限有关的回调函数
                m.CustomFlag = 0;

                WorkFlow_Define wfd = wfs.GetWorkFlowDefineByID(workflow_id);

                m.attachTarget(wfd);
                m.GetAttachWorkFlow();//0910
                m.Save();

                CTimerManage.ActiveListActionForMission(m);
                return "成功发起定时任务";
            }
            else
            {

                return "该工作流已经提报过，请删除后再发起";
            }

        }

        //获取定时待处理任务
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
            int w = 0;
            for (int i = 0; i < Joblist.Count; i++)
            {
                if (Joblist[i].custom_flag == 0 && Joblist[i].Job_Type == 0 && Joblist[i].status==TM_STATUS.TM_STATUS_ACTIVE)
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
                    string wf_name = wfs.GetWorkFlowDefineByID(Joblist[i].workflow_ID).W_Name + ":" + wfs.GetWorkFlowDefineByID(Joblist[i].workflow_ID).W_Attribution;
                    string job_desc = wfs.GetWorkFlowDefineByID(Joblist[i].workflow_ID).W_Name;

                    object o = new
                    {
                        job_id = Joblist[i].JOB_ID,
                        index = w + 1,
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
                    w++;

                }

            }
            string str = JsonConvert.SerializeObject(r);
            return ("{" + "\"data\": " + str + "}");





        }


        public string delete_job(string json)
        {
            JObject item = (JObject)JsonConvert.DeserializeObject(json);
            int job_id = Convert.ToInt32(item["job_id"].ToString());
            CTimerMission m = CTimerManage.RemoveFromActiveList(job_id);
            List<string> property = new List<string>();
            property.Add("status");
            List<object> val = new List<object>();
            val.Add(TM_STATUS.TM_DELETE);
            CTimerManage.UpdateTimerMission(job_id, property, val);
            return "删除成功";
        }
    }
}
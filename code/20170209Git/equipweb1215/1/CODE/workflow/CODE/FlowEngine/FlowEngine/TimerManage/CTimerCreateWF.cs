using FlowEngine.DAL;
using FlowEngine.Event;
using FlowEngine.Modals;
using FlowEngine.Param;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace FlowEngine.TimerManage
{
    //创建工作流所需的参数
    public class TimerCreateWFPa
    {
        public Dictionary<string, string> wf_params = new Dictionary<string, string>();
        public Dictionary<string, string> wf_record = new Dictionary<string, string>();
        //创建工作流时对各个event设置定时器(TimeOut)
        //其结构及含义如下:
        //wf_timer = 
        //              {
        //                  event11: {
        //                              ExactTime: ******,
        //                              TimeStart: ******,
        //                              OffsetTime: ****
        //                           },
        //                  event12: {
        //                              ExactTime: ******,
        //                              StartTime: ******,
        //                              OffsetTime: *****
        //                           },
        //                  ......
        //              }  
        //为保证使用安全，请勿直接访问该属性
        public Dictionary<string, Dictionary<string, string>> wf_timer = new Dictionary<string, Dictionary<string, string>>();

        public class TimerSetting
        {
            public string Exact_time;
            public string Time_start;  //"wf_create"或"ev_enter"
            public string Offset_time;
            public string Action;
            public string Call_back;
        }
        //添加一个定时器设置
        public void AppendTimer(string ev_name, TimerSetting ts)
        {
            if (wf_timer.ContainsKey(ev_name))
            {
                wf_timer[ev_name]["ExactTime"] = ts.Exact_time;
                wf_timer[ev_name]["TimeStart"] = ts.Time_start;
                wf_timer[ev_name]["OffsetTime"] = ts.Offset_time;
                wf_timer[ev_name]["Action"] = ts.Action;
                wf_timer[ev_name]["CallBack"] = ts.Call_back;
            }
            else
            {
                Dictionary<string, string> time_value = new Dictionary<string, string>();
                time_value["ExactTime"] = ts.Exact_time;
                time_value["TimeStart"] = ts.Time_start;
                time_value["OffsetTime"] = ts.Offset_time;
                time_value["Action"] = ts.Action;
                time_value["CallBack"] = ts.Call_back;
                wf_timer[ev_name] = time_value;
            }
        }
    }
    /// <summary>
    /// 创建工作流实体定时性任务
    /// </summary>
    public class CTimerCreateWF : CTimerMission
    {
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public CTimerCreateWF()
        {
            //允许一个定时任务创建多个工作流
            m_run_params = new List<TimerCreateWFPa>();
            m_run_result = new List<int>();
            type = "CreateWorkFlow";
        }
        #endregion

        #region 属性
        /// <summary>
        /// 任务执行对象 
        /// </summary>
        private CWorkFlow m_workFlow = new CWorkFlow();


        /// <summary>
        /// Affer Create 回调
        /// </summary>
        public string CreateCallback
        {
            get;
            set;
        }
        #endregion

        #region 公共方法

        /// <summary>
        /// 获得与之关联的工作流
        /// </summary>
        /// <returns></returns>
        public override CWorkFlow GetAttachWorkFlow()
        {
            return m_workFlow;
        }

        /// <summary>
        /// 清空创建工作流实体参数
        /// </summary>
        public void ClearCreateParams()
        {
            ((List<TimerCreateWFPa>)m_run_params).Clear();
        }

        /// <summary>
        /// 添加新建工作流实体变量
        /// </summary>
        /// <param name="newCreate"></param>
        public void AppendCreateParam(TimerCreateWFPa newCreate)
        {
            ((List<TimerCreateWFPa>)m_run_params).Add(newCreate);
        }

        /// <summary>
        /// 保存到数据库
        /// </summary>
        public override void Save(Timer_Jobs job = null)
        {
            Timer_Jobs self_Job = new Timer_Jobs();

            self_Job.workflow_ID = m_workFlow.DefineID;

            self_Job.Job_Type = TIMER_JOB_TYPE.CREATE_WORKFLOW;

            CustomAction = CreateCallback;


            base.Save(self_Job);
        }

        /// <summary>
        /// 绑定执行对象
        /// </summary>
        /// <param name="wf">工作流</param>
        /// <returns></returns>
        public bool attachTarget(WorkFlow_Define wf)
        {
            if (wf == null)
                return false;

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(Encoding.Default.GetString(wf.W_Xml));
            m_workFlow.InstFromXmlNode((XmlNode)doc.DocumentElement);

            m_workFlow.DefineID = wf.W_ID;
            return true;
        }

        /// <summary>
        /// 从数据库记录加载类对象
        /// </summary>
        /// <param name="job">数据库记录</param>
        public override void Load(Timer_Jobs job)
        {
            //加载工作流定义
            WorkFlows wfs = new WorkFlows();
            WorkFlow_Define wf_define = wfs.GetWorkFlowDefineByID(job.workflow_ID);
            m_workFlow = null;
            if (wf_define != null)
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(Encoding.Default.GetString(wf_define.W_Xml));
                m_workFlow = new CWorkFlow();
                m_workFlow.InstFromXmlNode((XmlNode)doc.DocumentElement);
                m_workFlow.DefineID = job.workflow_ID;
            }

            base.Load(job);

            CreateCallback = CustomAction;

        }

        /// <summary>
        /// 将RunParams 解析成JObject
        /// </summary>
        /// <returns></returns>
        public override object ParseRunParamsToJObject()
        {
            JArray jObj = new JArray();

            foreach (var pa in (List<TimerCreateWFPa>)m_run_params)
            {
                JObject jpar = new JObject();

                jpar["PARAMS"] = ParseDictionaryToJObject(pa.wf_params);

                jpar["RECORD"] = ParseDictionaryToJObject(pa.wf_record);

                jpar["TIMER"] = ParseDictionaryToJObject(pa.wf_timer);

                jObj.Add(jpar);
            }

            return jObj;
        }

        /// <summary>
        /// 将RunResult 解析成JObject
        /// </summary>
        /// <returns></returns>
        public override object ParseRunResultToJobject()
        {
            JArray jObj = new JArray();
            foreach (var rr in (List<int>)m_run_result)
            {
                JValue jrr = new JValue(rr);
                jObj.Add(jrr);
            }
            return jObj;
        }

        /// <summary>
        /// 解析CreateWF参数
        /// </summary>
        /// <param name="strPars"></param>
        public override void ParseRunParam(string strPars)
        {
            if (strPars == "null")
                return;

            ((List<TimerCreateWFPa>)m_run_params).Clear();

            //解析多个创建工作流实体的参数
            JArray j_pars = JArray.Parse(strPars);

            //对JSon数组元素进行遍历
            for (int i = 0; i < j_pars.Count(); i++)
            {
                var par = j_pars[i];

                if (par is JObject)
                {
                    Dictionary<string, string> dict_params = null;
                    Dictionary<string, string> dict_record = null;
                    Dictionary<string, Dictionary<string, string>> dict_timer = null;
                    var enumerator = ((JObject)par).GetEnumerator();
                    while (enumerator.MoveNext())
                    {
                        var Jval = enumerator.Current;

                        //解析传递给工作流实体参数的信息
                        if (Jval.Key == "PARAMS")
                            dict_params = ParseJObjectToDictionary((JObject)Jval.Value);
                        //解析传递给工作流实体的Record信息
                        if (Jval.Key == "RECORD")
                            dict_record = ParseJObjectToDictionary((JObject)Jval.Value);
                        //解析Timer
                        if (Jval.Key == "TIMER")
                            dict_timer = ParseJObjectToDictionary((JObject)Jval.Value, true);

                        //如果两者解析都合法
                        if (dict_params != null && dict_record != null && dict_timer != null)
                        {
                            TimerCreateWFPa creatPas = new TimerCreateWFPa();
                            creatPas.wf_params = dict_params;
                            creatPas.wf_record = dict_record;
                            creatPas.wf_timer = dict_timer;

                            ((List<TimerCreateWFPa>)m_run_params).Add(creatPas);
                        }
                    }
                }
            }

        }

        /// <summary>
        /// 将数据库中的run_result解析到对象中
        /// </summary>
        /// <param name="strResult"></param>
        public override void ParseRunResult(string strResult)
        {
            if (strResult == "" || strResult == "null" || strResult == null)
                return;

            JArray jobj = JArray.Parse(strResult);
            ((List<int>)m_run_result).Clear();

            foreach (var jj in jobj)
            {
                ((List<int>)m_run_result).Add(Convert.ToInt32(((JValue)jj).Value));
            }

        }

        public List<int> GetCreatedWFEntities()
        {
            return (List<int>)m_run_result;
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 创建工作流后的回调
        /// </summary>
        private void AfterCreate(int wfeId)
        {
            string custom_param = "{\"wfe_id\":" + wfeId.ToString() + "}";
            base.CallCustomAction(custom_param);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jobj"></param>
        private Dictionary<string, string> ParseJObjectToDictionary(JObject jobj)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            var enumerator = jobj.GetEnumerator();

            while (enumerator.MoveNext())
            {
                var jval = enumerator.Current;
                dict[jval.Key] = jval.Value.ToString();
            }
            return dict;
        }

        private Dictionary<string, Dictionary<string, string>> ParseJObjectToDictionary(JObject jobj, bool bToDict)
        {
            Dictionary<string, Dictionary<string, string>> dict = new Dictionary<string, Dictionary<string, string>>();
            var enumerator = jobj.GetEnumerator();

            while (enumerator.MoveNext())
            {
                var jval = enumerator.Current;
                dict[jval.Key] = ParseJObjectToDictionary(jval.Value as JObject);
            }
            return dict;
        }

        /// <summary>
        /// 将字典解析为JObject
        /// </summary>
        /// <param name="dict"></param>
        /// <returns></returns>
        private JObject ParseDictionaryToJObject(Dictionary<string, string> dict)
        {
            JObject jobj = new JObject();

            foreach (var di in dict)
            {
                jobj[di.Key] = di.Value.ToString();
            }

            return jobj;
        }

        private JObject ParseDictionaryToJObject(Dictionary<string, Dictionary<string, string>> dict)
        {
            JObject jobj = new JObject();
            if (dict!=null)
            {
                foreach (var di in dict)
                {
                    jobj[di.Key] = ParseDictionaryToJObject(di.Value);
                }
 
            }
           
        
            return jobj;
        }
        /// <summary>
        /// 执行工作流创建工作
        /// </summary>
        protected override void __processing()
        {
            //Trace.WriteLine(string.Format("Creating WorkFlow {0}", m_workFlow.name));
            foreach (var rp in (List<TimerCreateWFPa>)m_run_params)
            {
                if (rp.wf_timer != null)
                { //创建工作流
                    CWorkFlow wf = new CWorkFlow();

                    wf.InstFromXmlNode(m_workFlow.WriteToXmlNode());
                    //修改定时器
                    foreach (var ti in rp.wf_timer)
                    {
                        try
                        {
                            DateTime? dt = null;

                            if (ti.Value["ExactTime"] != "")
                                dt = DateTime.Parse(ti.Value["ExactTime"]);
                            wf.events[ti.Key].TimeOutProperty.SetAttribute("exact_time", dt);

                            TimeSpan? ts = null;
                            if (ti.Value["OffsetTime"] != "")
                                ts = TimeSpan.Parse(ti.Value["OffsetTime"]);
                            wf.events[ti.Key].TimeOutProperty.SetAttribute("offset_time", ts);

                            wf.events[ti.Key].TimeOutProperty.SetAttribute("time_start", ti.Value["TimeStart"]);

                            wf.events[ti.Key].TimeOutProperty.SetAttribute("action", ti.Value["Action"]);
                            wf.events[ti.Key].TimeOutProperty.SetAttribute("call_back", ti.Value["CallBack"]);

                        }
                        catch
                        {
                            continue;
                        }
                    }
                  



                    //更新工作流变量
                    foreach (var pp in ((Dictionary<string, string>)(rp.wf_params)))
                    {
                        try
                        {
                            CParam pa = wf.paramstable[pp.Key];
                            pa.value = pp.Value;
                            if (pp.Key == "Zz_Name" || pp.Key == "ZzName" || pp.Key == "Pqname" || pp.Key == "Equip_GyCode")
                                wf.description = wf.description + "(" + pp.Value + ")";
                        }
                        catch
                        {
                            continue;
                        }
                    }

                    //创建工作流
                    wf.CreateEntityBySelf();
                    //                                                                                  
                    //回调
                    AfterCreate(wf.EntityID);

                    ((List<int>)m_run_result).Add(wf.EntityID);
                    //工作流开始工作
                    wf.Start((IDictionary<string, string>)(rp.wf_record));
                }
            }
        }
        #endregion
    }
}

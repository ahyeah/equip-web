using FlowEngine.Authority;
using FlowEngine.DAL;
using FlowEngine.Event;
using FlowEngine.Modals;
using FlowEngine.Param;
using FlowEngine.UserInterface;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Web;
using System.Web.Security;

namespace FlowEngine
{
    public class CWFEngine
    {
        //public static Dictionary<string, Dictionary<string, object>> authority_params = new Dictionary<string, Dictionary<string, object>>();
        public static string authority_params = "";
        //获得系统所有的工作流定义
        public static List<UI_MissParam> GetMissionParams(int entity_id, int missId)
        {
            UI_MISSION miss = null; 
            List<UI_MISSION> m;
            List<UI_MissParam> Listmp = new List<UI_MissParam>();
            m = CWFEngine.GetHistoryMissions(entity_id);
            foreach(var item in m)
            {
                if (item.Miss_Id != missId) continue;
                else
                {
                    miss = item; break;
                }

            }
            
            foreach(var item in miss.Miss_ParamsAppRes)
            {
               
                if(miss.Miss_ParamsAppRes[item.Key]!="App_hidden")
                {  UI_MissParam mp = new UI_MissParam();
                    mp.Param_Desc = miss.Miss_ParamsDesc[item.Key].ToString();
                    mp.Param_Value = miss.Miss_Params[item.Key].ToString();
                    if (mp.Param_Desc.IndexOf("附件") > -1)
                    {
                        mp.Param_isFile = 1;
                        if (mp.Param_Desc.IndexOf("附件") > -1 && mp.Param_Value != "")
                        {
                            string[] savedFileName = mp.Param_Value.Split(new char[] { '$' });
                            string saveExistFilename = Path.Combine("/Upload", savedFileName[1]);
                            mp.Param_SavedFilePath = savedFileName[0];
                            mp.Param_UploadFilePath = saveExistFilename;
                        }
                    }
                    else
                    {
                        mp.Param_isFile = 0;
                        mp.Param_SavedFilePath = "";
                        mp.Param_UploadFilePath = "";
                    }

                     Listmp.Add(mp);
                    

                }

            }
            return Listmp;


         /*   m=wfs.GetMissParams(missId);
            foreach(var item in m)
            {
                UI_MissParam m1 = new UI_MissParam();
                m1.Param_Name=item.Param_Name;
                m1.Param_Value=item.Param_Value;
                m1.Param_Type=item.Param_Type;
                m1.Param_Desc=item.Param_Desc;
                m1.Param_AppRes = wf.paramstable[item.Param_Name].linkEventsApp_res[event_Name];

                missParams.Add(m1);
            }
            return missParams;*/
        }
        public static List<UI_WF_Define> ListAllWFDefine()
        {
            WorkFlows wfs = new WorkFlows();

            List<UI_WF_Define> ui_wfs = new List<UI_WF_Define>();
            List<WorkFlow_Define> wds = wfs.GetAllWorkFlows();
            wds.ForEach(s => ui_wfs.Add(new UI_WF_Define { wf_id = s.W_ID, wf_name = s.W_Name, wf_description = s.W_Attribution }));

            return ui_wfs;
        }

        //创建一个工作流
        public static UI_WorkFlow_Entity CreateAWFEntityByName(string wf_name, string Ser_Num = null)
        {
            CWorkFlow wf = new CWorkFlow();

            if (!wf.CreateEntity(wf_name, Ser_Num))
                return null;
            
            return wf;
        }

        public static CWorkFlow CreateWFEntityModifiedTimeOut(string wf_name, string event_name, CTimeOutProperty property, string Ser_Num = null /*2016/2/12--保证子工作流串号与父工作流相同*/)
        {
            CWorkFlow wf = new CWorkFlow();

            WorkFlows wfs = new WorkFlows();

            XmlDocument doc = new XmlDocument();
            WorkFlow_Define define = wfs.GetWorkFlowDefine(wf_name);
            doc.LoadXml(Encoding.Default.GetString(define.W_Xml));
            wf.InstFromXmlNode((XmlNode)doc.DocumentElement);

            //修改wf
            IEvent ev_target = wf.events[event_name];
            CTimeOutProperty timeout_pro = ev_target.GetTimeOutProperty();
            timeout_pro.ExactTime = property.ExactTime;
            timeout_pro.StartTime = property.StartTime;
            timeout_pro.TimeOffset = property.TimeOffset;

            WorkFlow_Entity wfe = new WorkFlow_Entity();
            wfe.WE_Status = WE_STATUS.CREATED;
            wfe.WE_Binary = Encoding.Default.GetBytes(wf.WriteToXmlNode().OuterXml);
                        
            if (Ser_Num == null)
                wfe.WE_Ser = "";
            else
                wfe.WE_Ser = Ser_Num;

            if (!wfs.AddWorkEntity(wf_name, wfe))
                return null;

            wf.EntityID = wfe.WE_Id;
            wf.EntitySerial = wfe.WE_Ser;
            wf.DefineID = define.W_ID;

           

            wf.RegEventsTimeOut(true);          

            return wf;
        }

        //返回一个工作流定义
        public static UI_WF_Define GetWorkFlowDefine(string wf_name)
        {
            WorkFlows wfs = new WorkFlows();
            WorkFlow_Define wfd = wfs.GetWorkFlowDefine(wf_name);
            UI_WF_Define ui_wfd = new UI_WF_Define { wf_name = wfd.W_Name, wf_description = wfd.W_Attribution };
            return ui_wfd;
        }

        /// <summary>
        /// 将Record添加到subprocess
        /// </summary>
        private static void Post_processSubprocess(IEvent ev, IDictionary<string, string> record)
        {
            if (record == null)
                return;

            CSubProcessEvent sp = (CSubProcessEvent)ev;

            List<UI_MISSION> miss = GetHistoryMissions(sp.WfEntityId);
            
            WorkFlows wfs = new WorkFlows();
            
            foreach (UI_MISSION mi in miss)
            {
                List<Process_Record> prs = new List<Process_Record>();
                foreach (var re in record)
                {
                    Process_Record pr = new Process_Record();
                    pr.Re_Name = re.Key;
                    pr.Re_Value = re.Value;
                    prs.Add(pr);
                }                
                wfs.LinkRecordInfoToMiss(mi.Miss_Id, prs);
            }
            //SubmitSignal(sp.WfEntityId, new Dictionary<string, string>(), record);
        }


        //fhp添加方法开始----

        public static UI_WFEntity_Info GetMainWorkFlowEntity(string wfe_ser)
        {
            UI_WFEntity_Info wfe = new UI_WFEntity_Info();
            WorkFlows wfs = new WorkFlows();
            WorkFlow_Entity wfe1 = wfs.GetMainWorkFlowEntity(wfe_ser);

            CWorkFlow wf = new CWorkFlow();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(Encoding.Default.GetString(wfe1.WE_Binary));
            wf.InstFromXmlNode(doc.DocumentElement);
            wfe.description = wf.description;
            wfe.name = wf.name;

            wfe.EntityID = wfe1.WE_Id;
            wfe.serial = wfe1.WE_Ser;
            wfe.Status = wfe1.WE_Status;
            return wfe;

        }

        //fhp添加方法结束-----

        /// <summary>
        /// 或的工作流的信息以及指定变量的值
        /// </summary>
        /// <param name="wfe_id">工作流实体ID</param>
        /// <param name="paras">变量列表
        /// 例如： 要获得工作流（ID 为 4）的Equip_GyCode的当前值，则：
        /// Dictionary paras = new Dictionary();
        /// paras["Equip_GyCode"] = null;
        /// UI_WFEntity_Info wfei = GetWorkFlowEntityWithParams(4, paras);
        /// 
        /// 调用完成后， wfei返回了 name, description, EntityID, serial, Status
        /// 而paras["Equip_GyCode"] 将会被设置为正确值
        /// </param>
        /// <returns></returns>
        public static UI_WFEntity_Info GetWorkFlowEntityWithParams(int wfe_id, IDictionary<string, object> paras)
        {
            UI_WFEntity_Info wfe = new UI_WFEntity_Info();
            WorkFlows wfs = new WorkFlows();
            WorkFlow_Entity wfe1 = wfs.GetWorkFlowEntity(wfe_id);

            CWorkFlow wf = new CWorkFlow();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(Encoding.Default.GetString(wfe1.WE_Binary));
            XmlNode xml = doc.DocumentElement;
            wf.ParaseBaseInfo(xml);
            wf.ParseParams(xml.SelectSingleNode("paramtable"));

            Dictionary<string, object> _tmp = new Dictionary<string, object>(paras);
            foreach (var par in _tmp)
            {
                paras[par.Key] = wf.paramstable[par.Key].value;
            }
            

            wfe.name = wf.name;
            wfe.description = wf.description;
            wfe.EntityID = wf.EntityID;
            wfe.serial = wf.EntitySerial;
            wfe.Status = wfe1.WE_Status;
            return wfe;
        }

        /// <summary>
        /// 通过工作流实体的id获得工作流实体
        /// </summary>
        /// <param name="wfe_id"></param>
        /// <returns></returns>
        public static UI_WFEntity_Info GetWorkFlowEntiy(int wfe_id, bool FullInfo = false)
        {
            UI_WFEntity_Info wfe = new UI_WFEntity_Info();
            WorkFlows wfs = new WorkFlows();
            WorkFlow_Entity wfe1 = wfs.GetWorkFlowEntity(wfe_id);

            if (FullInfo)
            {
                CWorkFlow wf = new CWorkFlow();
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(Encoding.Default.GetString(wfe1.WE_Binary));
                XmlNode xml = doc.DocumentElement;
                wf.ParaseBaseInfo(xml);
                WorkFlow_Define wfd = wfs.GetWorkFlowDefine(wfe_id);
                //wfe.name = wfd.W_Name;
                //wfe.description = wfd.W_Attribution;
                wfe.Binary = Encoding.Default.GetString(wfe1.WE_Binary);
                wfe.name = wf.name;
                wfe.description = wf.description;
            }

            wfe.EntityID = wfe1.WE_Id;
            wfe.serial = wfe1.WE_Ser;
            wfe.Status = wfe1.WE_Status;
            wfe.Last_TransTime = wfe1.Last_Trans_Time;
            return wfe;
        }

        //发送消息到工作流
        public static void SubmitSignal(int wfe_id, IDictionary<string, string> signal, IDictionary<string, string> record = null)
        {
            WorkFlows wfs = new WorkFlows();
            WorkFlow_Entity wfe = wfs.GetWorkFlowEntity(wfe_id);
            //如果该工作流处于非激活状态，则不能向其发送信息
            if (wfe.WE_Status != WE_STATUS.ACTIVE)
                return;

            CWorkFlow wf = new CWorkFlow();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(Encoding.Default.GetString(wfe.WE_Binary));
            wf.InstFromXmlNode(doc.DocumentElement);
            wf.EntityID = wfe.WE_Id;

            string preState = wf.GetCurrentState();
            
            JArray ja = new JArray();
            foreach(var s in signal)
            {
                JObject jo = new JObject();
                jo.Add("name", s.Key);
                jo.Add("value", s.Value);
                ja.Add(jo);
            }
            wf.SubmitSignal(JsonConvert.SerializeObject(ja));         
            //如果preEvent为subprocess
            IEvent preEvent = wf.GetCurrentEvent();
            if (preEvent.GetType().Name == "CSubProcessEvent")
            {
                Post_processSubprocess(preEvent, record);
                //wf.SubmitSignal("[]");
                
            }


            //状态发生了迁移
            if (wf.GetCurrentState() != preState)
            {
                if (record != null)
                {
                    Mission ms = wfs.GetWFEntityLastMission(wf.EntityID);

                    List<Process_Record> res = new List<Process_Record>();
                    foreach(var re in record)
                    {
                        //如果record中包含事件定义的需要记录的record item则记录到数据库中
                        if (wf.GetRecordItems().ContainsKey(re.Key))
                        {
                            Process_Record pre = new Process_Record();
                            pre.Re_Name = re.Key;
                            pre.Re_Value = re.Value;
                            res.Add(pre);
                        }
                    }
                    wfs.LinkRecordInfoToMiss(ms.Miss_Id, res);
                }
                //如果当前工作流是子流程，且已执行到End
                if (wf.GetCurrentEvent().GetType().Name == "CEndEvent" && wf.ParentEntityID != -1)
                {
                    WorkFlow_Entity P_wfe = wfs.GetWorkFlowEntity(wf.ParentEntityID);

                    CWorkFlow P_wf = new CWorkFlow();
                    XmlDocument P_doc = new XmlDocument();
                    P_doc.LoadXml(Encoding.Default.GetString(P_wfe.WE_Binary));
                    P_wf.InstFromXmlNode(P_doc.DocumentElement);
                    P_wf.EntityID = P_wfe.WE_Id;
                    P_wf.GetCurrentState();
                }
            }

            //如果当前时间为subprocess
            if (preEvent.GetType().Name == "CSubProcessEvent")
            {
                //如果该子事件的工作模式为并行的， 则需要发送一个信号，激励其自动运行一次
                if (((CSubProcessEvent)preEvent).WorkingMode == "parallel")
                    SubmitSignal(wfe_id, new Dictionary<string, string>(), record);
            }
        }

        private static void UpdateEntity(CWorkFlow wf, WE_STATUS status)
        {
            try
            {
                WorkFlow_Entity wfe = new WorkFlow_Entity
                {
                    WE_Id = wf.EntityID,
                    WE_Status = status
                };
                wfe.WE_Binary = Encoding.Default.GetBytes(wf.WriteToXmlNode().OuterXml);

                WorkFlows wfs = new WorkFlows();
                if (!wfs.SaveWorkFlowEntity(wfe))
                    throw new Exception("Save WorkFlow Entity failed!");

            }
            catch (Exception e)
            {
                return;
            }
        }

        //获取当前任务列表
        /*public static List<UI_MISSION> GetActiveMissions<T>(ObjectContext oc, string Entity_name="ALL", bool bAuthCheck = true)
        {
            List<UI_MISSION> missions = new List<UI_MISSION>();

            WorkFlows wfs = new WorkFlows();
            List<WorkFlow_Entity> wfes = wfs.GetActiveWorkFlowEntities(Entity_name);

            foreach(WorkFlow_Entity wfe in wfes)
            {
                

                UI_MISSION mi = new UI_MISSION();
                mi.WE_Entity_Id = wfe.WE_Id;
                mi.WE_Name = wfs.GetWorkFlowEntityName(wfe.WE_Id);

                //恢复工作流实体
                CWorkFlow wf = new CWorkFlow();
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(Encoding.Default.GetString(wfe.WE_Binary));
                wf.InstFromXmlNode(doc.DocumentElement);
                wf.EntityID = wfe.WE_Id;
                wf.EntitySerial = wfe.WE_Ser;

                //if (wf.CurrentEventLink() == "")
                //    continue;

                //权限验证
                if (bAuthCheck)
                {
                    IEvent ev = wf.GetCurrentEvent();
                    if (!ev.CheckAuthority<T>(authority_params, oc))
                        continue;
                }
                

                mi.WE_Event_Name = wf.GetCurrentEvent().name;
                mi.WE_Event_Desc = wf.GetCurrentEvent().description;
                mi.Mission_Url = wf.CurrentEventLink();
                //读取参数的值
                foreach(var pa in wf.GetCurrentEvent().paramlist)
                {
                    mi.Miss_Params[pa.Key] = pa.Value.value;
                }

                missions.Add(mi);
            }
            return missions;
        }*/
        public static List<UI_MISSION> GetActiveMissions<T>(ObjectContext oc, string Entity_name = "ALL", bool bAuthCheck = true)
        {
            List<UI_MISSION> missions = new List<UI_MISSION>();

            WorkFlows wfs = new WorkFlows();
            List<CURR_Mission> cMis = wfs.GetActiveMissionsOfEntity(Entity_name);

            foreach (CURR_Mission cm in cMis)
            {


                UI_MISSION mi = new UI_MISSION();
                mi.WE_Entity_Id = cm.WFE_ID;
                                

                //if (wf.CurrentEventLink() == "")
                //    continue;

                //权限验证
                if (bAuthCheck)
                {
                    CAuthority author = new CAuthority();
                    author.auth_string = cm.Str_Authority;

                    //if (!author.CheckAuth<T>(new Dictionary<string, object>(), authority_params[HttpContext.Current.Session.SessionID], oc))
                    //    continue;
                    if (!author.CheckAuth<T>(new Dictionary<string, object>(), (Dictionary<string, object>)HttpContext.Current.Session[authority_params], oc))
                        continue;
                }


                mi.WE_Event_Name = cm.Miss_Name;
                mi.WE_Event_Desc = cm.Miss_Desc;
                mi.Mission_Url = cm.Current_Action + @"/?wfe_id=" + mi.WE_Entity_Id;
                mi.WE_Event_Desc = cm.Miss_Desc;
                

                missions.Add(mi);
            }
            return missions;
        }

        //获得工作流实体的当前任务
        public static UI_MISSION GetActiveMission<T>(int entity_id, ObjectContext oc, bool bAuthCheck = true)
        {
            UI_MISSION miss = new UI_MISSION();

            WorkFlows wfs = new WorkFlows();

            WorkFlow_Entity wfe = wfs.GetWorkFlowEntity(entity_id);

            miss.WE_Entity_Id = wfe.WE_Id;
            miss.WE_Name = wfs.GetWorkFlowEntityName(wfe.WE_Id);
            miss.WE_Entity_Ser = wfe.WE_Ser;

            //恢复工作流实体
            CWorkFlow wf = new CWorkFlow();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(Encoding.Default.GetString(wfe.WE_Binary));
            wf.InstFromXmlNode(doc.DocumentElement);
            wf.EntityID = wfe.WE_Id;
            //权限验证
            if (bAuthCheck)
            {
                IEvent ev = wf.GetCurrentEvent();
                if (!ev.CheckAuthority<T>((Dictionary<string, object>)HttpContext.Current.Session[authority_params], oc))
                    return null;
            }

            miss.WE_Event_Name = wf.GetCurrentEvent().name;
            miss.WE_Event_Desc = wf.GetCurrentEvent().description;
            miss.Mission_Url = wf.CurrentEventLink();
            //读取参数的值
            foreach (var pa in wf.GetCurrentEvent().paramlist)
            {
                miss.Miss_Params[pa.Key] = pa.Value.value;
                miss.Miss_ParamsAppRes[pa.Key] = wf.GetCurrentEvent().paramsApp_res[pa.Key];
            }

            return miss;

        }

        /// <summary>
        /// 获取某一工作流实体的历史任务
        /// </summary>
        /// <param name="entity_id">工作流实体ID</param>
        /// <returns></returns>
        public static List<UI_MISSION> GetHistoryMissions(int entity_id)
        {
            List<UI_MISSION> his_miss = new List<UI_MISSION>();

            WorkFlows wfs = new WorkFlows();
            List<Mission> db_miss = wfs.GetWFEntityMissions(entity_id);

            WorkFlow_Entity wfe = wfs.GetWorkFlowEntity(entity_id);
            //恢复工作流实体
            CWorkFlow wf = new CWorkFlow();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(Encoding.Default.GetString(wfe.WE_Binary));
            wf.EntityID = wfe.WE_Id;
            wf.InstFromXmlNode(doc.DocumentElement);
            

            foreach(var mi in db_miss)
            {
                UI_MISSION ui_mi = new UI_MISSION();
                ui_mi.WE_Entity_Id = entity_id;
                ui_mi.WE_Entity_Ser = wfe.WE_Ser;
                ui_mi.WE_Event_Desc = mi.Miss_Desc;
                ui_mi.WE_Event_Name = mi.Event_Name;
                ui_mi.WE_Name = mi.Miss_Name;
                ui_mi.Mission_Url = ""; //历史任务的页面至空
                ui_mi.Miss_Id = mi.Miss_Id;

                List<Mission_Param> mis_pars = wfs.GetMissParams(mi.Miss_Id);
                foreach(var par in mis_pars)
                {
                    CParam cp = new CParam();
                    cp.type = par.Param_Type;
                    cp.name = par.Param_Name;
                    cp.value = par.Param_Value;
                    cp.description = par.Param_Desc;
                    ui_mi.Miss_Params[cp.name] = cp.value;
                    ui_mi.Miss_ParamsAppRes[cp.name] = wf.paramstable[cp.name].linkEventsApp_res[ui_mi.WE_Event_Name];
                    ui_mi.Miss_ParamsDesc[cp.name]=cp.description;//xwm modified
                }
                his_miss.Add(ui_mi);
            }
            return his_miss;
        }

        /// <summary>
        /// 根据历史任务状态选择符合条件的工作流
        /// </summary>
        /// <param name="condition">查询条件的lambda表达式如： s => (s.Params_Info.Count(t => t.name = "approve_user" && t.value = "cb1") > 0)</param>
        /// <returns></returns>
        public static List<UI_WFEntity_Info> GetWFEntityByHistoryStatus(System.Linq.Expressions.Expression<Func<Mission, bool>> condition)
        {
            List<UI_WFEntity_Info> wfe_info = new List<UI_WFEntity_Info>();

            WorkFlows wfs = new WorkFlows();

            List<WorkFlow_Entity> wfes = null;
            //List<WorkFlow_Entity> wfes = wfs.GetWFEntityByConditon(s => s.Process_Info.wh );
            try
            {
                using (var db = new WorkFlowContext())
                {
                    wfes = db.mission.Where(condition).Select(a => a.Miss_WFentity).Distinct().Where(s => s.WE_Status != WE_STATUS.DELETED).ToList();
                }
            }
            catch(Exception e)
            {
                return null;
            }

            foreach(WorkFlow_Entity fe in wfes)
            {
                UI_WFEntity_Info fe_info = new UI_WFEntity_Info();
                fe_info.name = wfs.GetWorkFlowEntityName(fe.WE_Id);
                fe_info.Status = fe.WE_Status;
                fe_info.EntityID = fe.WE_Id;
                fe_info.description = wfs.GetWorkFlowEntityDesc(fe.WE_Id);
                fe_info.serial = fe.WE_Ser;
                wfe_info.Add(fe_info);

            }
            return wfe_info;
        }
        public static List<UI_WFEntity_Info> GetWFEntityByHistoryDone(System.Linq.Expressions.Expression<Func<Mission, bool>> condition)
        {
            List<UI_WFEntity_Info> wfe_info = new List<UI_WFEntity_Info>();

            WorkFlows wfs = new WorkFlows();

            List<WorkFlow_Entity> wfes = null;
            //List<WorkFlow_Entity> wfes = wfs.GetWFEntityByConditon(s => s.Process_Info.wh );
            try
            {
                using (var db = new WorkFlowContext())
                {
                    wfes = db.mission.Where(condition).Select(a => a.Miss_WFentity).Distinct().Where(s => s.WE_Status == WE_STATUS.DONE).ToList();
                }
            }
            catch (Exception e)
            {
                return null;
            }

            foreach (WorkFlow_Entity fe in wfes)
            {
                UI_WFEntity_Info fe_info = new UI_WFEntity_Info();
                fe_info.name = wfs.GetWorkFlowEntityName(fe.WE_Id);
                fe_info.Status = fe.WE_Status;
                fe_info.EntityID = fe.WE_Id;
                fe_info.description = wfs.GetWorkFlowEntityDesc(fe.WE_Id);
                fe_info.serial = fe.WE_Ser;
                wfe_info.Add(fe_info);

            }
            return wfe_info;
        }

        /// <summary>
        /// 获得某一工作流历史平均需要的步骤数量
        /// </summary>
        /// <param name="wf_name">工作流的名称</param>
        /// <returns></returns>
        public static int GetWorkFlowAvgSteps(string wf_name)
        {
            WorkFlows wfs = new WorkFlows();
            return wfs.GetWFAvgSteps(wf_name);
        }
        
        /// <summary>
        /// 获得某一工作流实体目前完成的步骤数量
        /// </summary>
        /// <param name="wfe_id"></param>
        /// <returns></returns>
        public static int GetWFEntityFinishSteps(int wfe_id)
        {
            WorkFlows wfs = new WorkFlows();
            return wfs.GetWFEntityFinishSteps(wfe_id);
        }

        public static IDictionary<string, string> GetMissionRecordInfo(int wfe_id)
        {
            WorkFlows wfs = new WorkFlows();
            List<Process_Record> Pre = wfs.GetMissionRecordInfo(wfe_id);
            Dictionary<string, string> res = new Dictionary<string, string>();

            foreach(var p in Pre)
            {
                res[p.Re_Name] = p.Re_Value;
            }
            return res;
        }

        /// <summary>
        /// 从数据库中删除一个工作流实体
        /// 建议：在用户取消人工提报时调用
        /// </summary>
        /// <param name="wfe_id"></param>
        public static void RemoveWFEntity(int wfe_id)
        {
            WorkFlows wfs = new WorkFlows();
            wfs.DeleteWFEntity(wfe_id);
        }

        /// <summary>
        /// 将一个工作流实体置为已删除
        /// 建议： 若该工作流已执行数步， 需要将其删除，为了保证系统后期可追溯，建议调用该函数
        /// </summary>
        /// <param name="wfe_id"></param>
        public static void DeleteWFEntity(int wfe_id)
        {
            WorkFlows wfs = new WorkFlows();
            wfs.SetWFEntityDeleted(wfe_id);
        }

        /// <summary>
        /// 对工作流信息进行查询（low level）
        /// </summary>
        /// <param name="query_list">
        /// 需要查询的属性列——空字符串表示查询所有属性列， 慎用！！！！！
        /// 由于变量名的量比较大，强烈建议该参数不使用空串
        /// M 代表 Missions
        /// R 代表 Record
        /// P 代表 Params
        /// E 代表 WorkFlow_Entity
        /// 如 "M.Event_Name, M.Miss_Name, R.time, R.username, P.Proble_DataSrc, E.WE_Ser, E.W_Attribtuion, ..."
        /// </param>
        /// <param name="query_condition">查询条件，如： "R.username = 'fhp' and E.W_Name = 'A11dot1'"</param>
        /// <param name="record_filter">
        /// Record过滤器——空字符串表示不过滤
        /// 因为Process_Record表与Mission_Param是工作流数据库中最大的两张表，故而非常有必要再连接之前对两者进行预先筛选以提高效率
        /// 考虑到参数表（Mission_Param）的筛选条件可能比较复杂，因此在这个函数中只提供在连接前对Record进行预筛选
        /// 如: time >= '2015/12/25 0:00:00' and username = 'fhp'
        /// 特别需要提醒的是： 如果查询不需要record信息，请务必将该参数设置为 "1 <> 1"
        /// </param>        
        public static System.Data.DataTable QueryAllInformation(string query_list, string query_condition, string record_filter)
        {
            WorkFlows wfs = new WorkFlows();
            return wfs.QueryAllInformation(query_list, query_condition, record_filter);
        }
    }
}


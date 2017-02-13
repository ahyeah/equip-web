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
using FlowEngine.Modals;
using FlowEngine.DAL;
using System.Xml;
using FlowEngine.Event;
using EquipBLL.FileManagment;
//add
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;

namespace WebApp.Controllers
{
    public class CommonController : Controller
    {
      
        //Page Models

        public class MainMissionsModel
        {
            public string jobname;
            public string WF_Name;
            public string MISS_Url;
            public string wfe_serial;
            public string MISS_Name;
            public string time;
            public string Equip_GyCode;
        }
        public class MainModel
        {
            public string userName;
            public string missTime;
            public int missIndex;
            public string miss_desc;
            public string miss_url;
            public string wfe_serial;
            public string sbGycode;
            public string workFlowName;
        }
        public class ZzRunhuaSubmitModel
        {
            public string UserName;
            public string UserDepartment;
            public List<Runhua_Info> UserHasEquips;
        }
        public class HistroyModel
        {
            public int miss_wfe_Id;
            public string missStartName;
            public string missStartTime;
            public int missIndex;
            public string miss_LastStatusdesc;
            public string wfe_serial;
            public string sbGycode;
            public string workFlowName;
        }

        public class Index_Model
        {
            public List<MainModel> Am;
            public List<HistroyModel> Hm;
        }

        public class MissInfoModel
        {
            public UI_MISSION miModel;
            public List<string> MissTime;
            public List<string> MissUser;
        }

        public class WFDetail_Model
        {
            public List<UI_MISSION> ALLHistoryMiss;
            public List<string> MissTime;
            public List<string> MissUser;
        }

        public class ZzSubmitModel
        {
            public string UserName;
            public string UserDepartment;
            public List<Equip_Archi> UserHasEquips;
        }
        //A5_ModelInfo
        public class A5_ModelInfo
        {
            public string Pq_Name;
            public string Zz_Name;
            public string Equip_Code;
            public string Equip_GyCode;
            public int nTimesInMonth;
            public int nTimesInYear;
            public int nFlagTheWorstTen;//是否列入最差或最脏10台? 0-否，1-是
        }
        //A5_Model
        public class A5_Model
        {
            public List<A5_ModelInfo> A5_ModelInfoList;
        }
        //Risk Matrix
        public class RiskMatrixElement
        {
            public int danger_intensity;
            public int time_level;
            public string color;
            public string DangerType_isgreen;
        }

        public class MissHisLinkModal
        {
            public int Miss_Id;
            public string Miss_Name;
            public string Miss_Time;
            public int Miss_Type;  //0：历史的一般任务  1：子流程任务 2:活动的一般任务
            public string Miss_LinkFlowType;//"Normal" ，"paradel","Serial"
            public int Miss_LinkFlowId;//跳转的工作流Id
            public string Miss_LinkFlowName;//跳转的工作流名称

        }

        public class MissionTemp
        {
            public UI_MISSION miss_info;
            public int miss_status;//0:Histroy  1:active 
        }

        public class ListMissHisLInkModal
        {
            public int Flow_Id;
            public string Flow_Name;
            public string FLow_DescPath;
            public string FLow_EquipGyCode;
            public List<MissHisLinkModal> Miss;
        }

        public class A11dot2Model
            {
            public int index_Id;
            public string zz_name;
            public string sb_gycode;
            public string sb_code;
            public string problem_desc;
            public string riskMatrix_color;
            public string supervise_done;
            public string implementplan_done;
            public string missionname;
            }
        public class A14dot3Model
        {
            public int index_Id;
            public string zz_name;
            public string sb_gycode;
            public string sb_code;
            public string sb_type;
            public string sb_ABCMark;
            public string plan_name;
            public string jxreason;
            public string xcconfirm;
            public string kkconfirm;
            public string zytdconfirm;
            public string job_order;
            public string notice_order;
            public string missionname;
            public string role;
        }

        public class A14dot3dot3Model
        {
            public int index_Id;
            public string zz_name;
            public string sb_gycode;
            public string sb_code;
            public string sb_type;
            public string sb_ABCMark;
            public string plan_name;
            public string jxreason;
            public string xcconfirm;
            public string kkconfirm1;
            public string zytdconfirm1;
            public string job_order1;
            public string notice_order1;
            public string missionname;
            public string role;
        }
        public class A14dot3dot4Model
        {
            public int index_Id;
            public string zz_name;
            public string sb_gycode;
            public string sb_code;
            public string sb_type;
            public string sb_ABCMark;
            public string plan_name;
            public string jxreason;
            public string xcconfirm;
            public string kkconfirm2;
            public string zytdconfirm2;
            public string job_order2;
            public string notice_order2;
            public string missionname;
            public string role;
        }
        public class A8Model
        {
            public int index_Id;
            public string zz_name;
            public string sb_gycode;           
            public string plan_name;                      
            public string job_order;
            public string notice_order;
            public string Data_Src;
            public string gd_state;
            public string detail;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="wfName"></param>
        /// <param name="we"></param>
        /// <returns></returns>
      
        public MainMissionsModel GetMainMissionsInfo(int entity_id)
        {
            MainMissionsModel mm = new MainMissionsModel();
            UI_WFEntity_Info wfe = new UI_WFEntity_Info();
            WorkFlows wfs = new WorkFlows();
            WorkFlow_Entity wfe1 = wfs.GetWorkFlowEntity(entity_id);
            CWorkFlow wf = new CWorkFlow();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(Encoding.Default.GetString(wfe1.WE_Binary));
            XmlNode xml = doc.DocumentElement;
            wf.InstFromXmlNode(xml);
            IEvent e = wf.GetCurrentEvent();
            if (wfe1.Last_Trans_Time != null)
                mm.time = wfe1.Last_Trans_Time.ToString();
            else
                mm.time = "";
            mm.wfe_serial = wfe1.WE_Ser;
            mm.WF_Name = wf.description;
            mm.MISS_Name = e.description;
            mm.jobname = wf.name + ":" + e.name;
            mm.MISS_Url = e.currentaction;
           
            mm.Equip_GyCode = wf.paramstable["Equip_GyCode"].value.ToString();
            return mm;
        }
        public JsonResult get_zhidubymenu(int menu_id)
        {
            ZhiduManagment Zm = new ZhiduManagment();
            List<ZhiduFile> E = Zm.get_zhidu(menu_id);
            List<object> e_obj = new List<object>();
            foreach (var item in E)
            {
                object o = new
                {
                    pdf_name = item.pdf_name
                };
                e_obj.Add(o);
            }
            return Json(e_obj.ToArray());
        }

        public ListMissHisLInkModal List_subFLow(UI_WFEntity_Info wfe, int condition)
        {
            List<MissionTemp> miss_temp = new List<MissionTemp>();
            ListMissHisLInkModal missModals = new ListMissHisLInkModal();
            missModals.Miss = new List<MissHisLinkModal>();
            missModals.Flow_Id = wfe.EntityID;
            missModals.Flow_Name = wfe.description;
            missModals.FLow_DescPath = Path.Combine(("/files"), wfe.name + ".pdf");
            List<UI_MISSION> miss = CWFEngine.GetHistoryMissions(wfe.EntityID);
            if (condition == 1) missModals.FLow_EquipGyCode = miss[1].Miss_Params["Equip_GyCode"].ToString();
            foreach (var item in miss)
            {
                MissionTemp temp = new MissionTemp();
                temp.miss_info = item;
                temp.miss_status = 0;
                miss_temp.Add(temp);
            }
            if (wfe.Status == WE_STATUS.ACTIVE)
            {
                MissionTemp temp = new MissionTemp();
                temp.miss_info = CWFEngine.GetActiveMission<Person_Info>(wfe.EntityID, null, false);
                temp.miss_status = 1;
                miss_temp.Add(temp);
            }
            WorkFlows wfs = new WorkFlows();
            WorkFlow_Entity wfe2 = wfs.GetWorkFlowEntity(wfe.EntityID);
            CWorkFlow wf = new CWorkFlow();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(Encoding.Default.GetString(wfe2.WE_Binary));
            wf.InstFromXmlNode(doc.DocumentElement);
            wf.EntityID = wfe.EntityID;
            IDictionary<string, IEvent> allEvents = wf.events;
            foreach (var tt in miss_temp)
            {
                var item = tt.miss_info;
                if (item.WE_Event_Name == "Start") continue;
                MissHisLinkModal m = new MissHisLinkModal();
                m.Miss_Id = item.Miss_Id;
                m.Miss_Name = item.WE_Event_Desc;
                if (item.Miss_Id > 0)
                {
                    IDictionary<string, string> r = CWFEngine.GetMissionRecordInfo(item.Miss_Id);
                    if (r.Count > 0)
                    {
                        m.Miss_Time = r["time"];
                    }
                    else
                    {
                        m.Miss_Time = "";
                    }
                }
                else
                {
                    m.Miss_Time = "";
                }
                if (allEvents[item.WE_Event_Name].GetType().Name == "CSubProcessEvent")
                {

                    m.Miss_Type = 1;
                    m.Miss_LinkFlowType = ((CSubProcessEvent)allEvents[item.WE_Event_Name]).WorkingMode;
                    m.Miss_LinkFlowId = ((CSubProcessEvent)allEvents[item.WE_Event_Name]).WfEntityId;
                    m.Miss_LinkFlowName = Path.Combine(("/files"), ((CSubProcessEvent)allEvents[item.WE_Event_Name]).WfName + ".pdf");

                }
                else
                {
                    if (tt.miss_status == 1)
                    {
                        m.Miss_Type = 2;
                        m.Miss_LinkFlowType = "Normal";
                        m.Miss_LinkFlowId = -1;
                        m.Miss_LinkFlowName = "";

                    }
                    else
                    {
                        m.Miss_Type = 0;
                        m.Miss_LinkFlowType = "Normal";
                        m.Miss_LinkFlowId = -1;
                        m.Miss_LinkFlowName = "";
                    }
                }

                missModals.Miss.Add(m);
            }
            return missModals;
        }

        
        public JsonResult WorkFlow_MainProcess(string Entity_Ser)
        {
            UI_WFEntity_Info wfe = CWFEngine.GetMainWorkFlowEntity(Entity_Ser);


            return Json(List_subFLow(wfe, 1));
        }
        public JsonResult WorkFlow_ListParam(string json1)
        {
            JObject item = (JObject)JsonConvert.DeserializeObject(json1);

            int Flow_Id = Convert.ToInt32(item["Entity_Id"].ToString());
            int Mission_Id = Convert.ToInt32(item["Mission_Id"].ToString());
            List<UI_MissParam> MissParams = CWFEngine.GetMissionParams(Flow_Id, Mission_Id);
            return Json(MissParams.ToArray());


        }

        public JsonResult WorkFlow_SubProcess(int FlowId)
        {
            UI_WFEntity_Info wfe = CWFEngine.GetWorkFlowEntiy(FlowId, true);
            return Json(List_subFLow(wfe, 0));


        }

        public JsonResult WorkFlow_EquipInfo(String Equip_GyCode)
        {
            EquipManagment EM = new EquipManagment();
            Equip_Info Einfo = EM.getEquip_ByGyCode(Equip_GyCode);
            List<Equip_Archi> Equip_ZzBelong = EM.getEquip_ZzBelong(Einfo.Equip_Id);
            List<Object> listE = new List<object>();
            listE.Add(new { Param_Name = "设备所在车间", Param_value = Equip_ZzBelong[1].EA_Name });
            listE.Add(new { Param_Name = "设备所属装置", Param_value = Equip_ZzBelong[0].EA_Name });
            listE.Add(new { Param_Name = "设备工艺编号", Param_value = Einfo.Equip_GyCode });
            listE.Add(new { Param_Name = "设备B相分类", Param_value = Einfo.Equip_PhaseB });
            listE.Add(new { Param_Name = "设备专业分类", Param_value = Einfo.Equip_Specialty });
            listE.Add(new { Param_Name = "设备型号", Param_value = Einfo.Equip_Type });
            listE.Add(new { Param_Name = "设备生产厂家", Param_value = Einfo.Equip_Manufacturer });
            if (string.IsNullOrEmpty(Einfo.Equip_ManufactureDate))
                listE.Add(new { Param_Name = "设备生产日期", Param_value = "" });
            else
                listE.Add(new { Param_Name = "设备生产日期", Param_value = Einfo.Equip_ManufactureDate });
            return Json(listE.ToArray());


        }
        //Page BLL Codes
        /// <summary>
        /// 获取工作流Index页面的列表记录
        /// </summary>
        /// <param name="WorkFlow_Name">工作流名称，如A8dot2</param>
        /// <param name="username">用户名</param>
        /// <returns></returns>
        /// 
        //A6dot2用查询开始
        public string getdclA6dot2_list(string WorkFlow_Name)
        {
            List<MainModel> Am = new List<MainModel>();
            List<UI_MISSION> miss;
            List<object> or = new List<object>();
            miss = CWFEngine.GetActiveMissions<Person_Info>(((IObjectContextAdapter)(new EquipWebContext())).ObjectContext, WorkFlow_Name, true);
            foreach (var item in miss)
            {
                MainModel o = new MainModel();
                Dictionary<string, object> paras = new Dictionary<string, object>();
                paras["Pq_Name"] = null;
                UI_WFEntity_Info wfei = CWFEngine.GetWorkFlowEntityWithParams(item.WE_Entity_Id, paras);
                UI_MISSION lastMi = CWFEngine.GetHistoryMissions(item.WE_Entity_Id).Last();
                int Miss_Id = lastMi.Miss_Id;
                IDictionary<string, string> r = CWFEngine.GetMissionRecordInfo(Miss_Id);
                if (r.Count > 0)
                {
                    o.userName = r["username"];
                    o.missTime = r["time"];
                }
                else
                {
                    o.userName = "";
                    o.missTime = "";
                }
                o.miss_desc = item.WE_Event_Desc;
                if (item.Mission_Url.Contains("dot"))
                    o.miss_url = item.Mission_Url;
                else
                    o.miss_url = "";
                o.wfe_serial = wfei.serial;
                o.workFlowName = wfei.description;
                o.sbGycode = paras["Pq_Name"].ToString();
                Am.Add(o);
            }
            for (int i = 0; i < Am.Count; i++)
            {
                object o = new
                {
                    wfe_serial = Am[i].wfe_serial,
                    equip_gycode = Am[i].sbGycode,
                    workflow_name = Am[i].workFlowName,
                    miss_desc = Am[i].miss_desc,
                    missTime = Am[i].missTime,
                    userName = Am[i].userName,
                    miss_url = Am[i].miss_url
                };
                or.Add(o);

            }
            string str = JsonConvert.SerializeObject(or);
            return ("{" + "\"data\": " + str + "}");
        }
        public string gethistoryA6dot2_list(string WorkFlow_Name)
        {
            string username = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
            List<HistroyModel> Hm = new List<HistroyModel>();
            List<UI_WFEntity_Info> start_entities;
            List<object> or = new List<object>();
            start_entities = CWFEngine.GetWFEntityByHistoryDone(t => t.Record_Info.Count(q => q.Re_Name == "username" && q.Re_Value == username) > 0 && t.Miss_WFentity.WE_Wref.W_Name == WorkFlow_Name);
            foreach (var item in start_entities)
            {
                HistroyModel h = new HistroyModel();
                Dictionary<string, object> paras = new Dictionary<string, object>();
                paras["Pq_Name"] = null;
                UI_WFEntity_Info wfei = CWFEngine.GetWorkFlowEntityWithParams(item.EntityID, paras);
                List<UI_MISSION> AllHistoryMiss = CWFEngine.GetHistoryMissions(item.EntityID);
                int Miss_Id = AllHistoryMiss[0].Miss_Id;
                IDictionary<string, string> r = CWFEngine.GetMissionRecordInfo(Miss_Id);
                if (r.Count > 0)
                {
                    h.missStartName = r["username"];
                    h.missStartTime = r["time"];
                }
                else
                {
                    h.missStartName = "";
                    h.missStartTime = "";
                }

                h.wfe_serial = item.serial;
                h.miss_wfe_Id = item.EntityID;
                h.workFlowName = wfei.description;
                h.sbGycode = paras["Pq_Name"].ToString();
                Hm.Add(h);
            }
            for (int i = 0; i < Hm.Count; i++)
            {
                object o = new
                {
                    wfe_serial = Hm[i].wfe_serial,
                    equip_gycode = Hm[i].sbGycode,
                    workflow_name = Hm[i].workFlowName,
                    missStartName = Hm[i].missStartName,
                    missStartTime = Hm[i].missStartTime,
                    miss_LastStatusdesc = "已完成",
                    miss_wfe_Id = Hm[i].miss_wfe_Id

                };
                or.Add(o);

            }
            string str = JsonConvert.SerializeObject(or);
            return ("{" + "\"data\": " + str + "}");
        }


        public string getactiveA6dot2_list(string WorkFlow_Name)
        {
            string username = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
            string query_list = "distinct E.WE_Id,R.username";
            string query_condition = "E.W_Name='" + WorkFlow_Name + "' and E.WE_Status='0' and M.Miss_Name <> 'Start' and R.username is not null";
            string record_filter = "username='" + username + "'";
            List<int> wfe_id = new List<int>();
            DataTable dt = CWFEngine.QueryAllInformation(query_list, query_condition, record_filter);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                wfe_id.Add(Convert.ToInt16(dt.Rows[i]["WE_Id"]));

            }
            List<HistroyModel> Hm = new List<HistroyModel>();
            foreach (var item in wfe_id)
            {
                HistroyModel h = new HistroyModel();
                Dictionary<string, object> paras = new Dictionary<string, object>();
                paras["Pq_Name"] = null;
                UI_WFEntity_Info wfei = CWFEngine.GetWorkFlowEntityWithParams(item, paras);
                List<UI_MISSION> AllHistoryMiss = CWFEngine.GetHistoryMissions(item);
                int Miss_Id = AllHistoryMiss[0].Miss_Id;
                h.miss_LastStatusdesc = AllHistoryMiss[AllHistoryMiss.Count - 1].WE_Event_Desc;
                IDictionary<string, string> r = CWFEngine.GetMissionRecordInfo(Miss_Id);
                if (r.Count > 0)
                {
                    h.missStartName = r["username"];
                    h.missStartTime = r["time"];
                }
                else
                {
                    h.missStartName = "";
                    h.missStartTime = "";
                }

                h.wfe_serial = wfei.serial;
                // h.miss_wfe_Id = wfei.EntityID;
                h.miss_wfe_Id = item;
                h.workFlowName = wfei.description;
                h.sbGycode = paras["Pq_Name"].ToString();
                Hm.Add(h);
            }
            List<object> or = new List<object>();

            for (int i = 0; i < Hm.Count; i++)
            {
                object o = new
                {
                    wfe_serial = Hm[i].wfe_serial,
                    equip_gycode = Hm[i].sbGycode,
                    workflow_name = Hm[i].workFlowName,
                    missStartName = Hm[i].missStartName,
                    missStartTime = Hm[i].missStartTime,
                    miss_LastStatusdesc = Hm[i].miss_LastStatusdesc,
                    miss_wfe_Id = Hm[i].miss_wfe_Id

                };
                or.Add(o);

            }
            string str = JsonConvert.SerializeObject(or);
            return ("{" + "\"data\": " + str + "}");
        }


        //A6dot2用查询结束





        public Index_Model getIndexListRecords(string WorkFlow_Name, string username)
        {
            List<UI_MISSION> miss;
            //miss = CWFEngine.GetActiveMissions<Person_Info>(((IObjectContextAdapter)(new EquipWebContext())).ObjectContext);
            miss = CWFEngine.GetActiveMissions<Person_Info>(((IObjectContextAdapter)(new EquipWebContext())).ObjectContext, WorkFlow_Name, true);
            Index_Model listRecord = new Index_Model();
            listRecord.Am = new List<MainModel>();
            foreach (var item in miss)
            {
                MainModel o = new MainModel();
                UI_MISSION lastMi = CWFEngine.GetHistoryMissions(item.WE_Entity_Id).Last();
                int Miss_Id = lastMi.Miss_Id;
                IDictionary<string, string> r = CWFEngine.GetMissionRecordInfo(Miss_Id);
                if (r.Count > 0)
                {
                    o.userName = r["username"];
                    o.missTime = r["time"];
                }
                else
                {
                    o.userName = "";
                    o.missTime = "";
                }
                o.missIndex = miss.IndexOf(item) + 1;
                o.miss_desc = item.WE_Event_Desc;
                //if(!string.IsNullOrEmpty(item.Mission_Url))
                if (item.Mission_Url.Contains("dot"))
                    //o.miss_url = item.Mission_Url + @"wfe_id=" + item.WE_Entity_Id;
                    o.miss_url = item.Mission_Url;
                else
                    o.miss_url = "";
                //
                //List<UI_WFEntity_Info> current_entitie = CWFEngine.GetWFEntityByHistoryStatus(t => t.Miss_WFentity.WE_Id== item.WE_Entity_Id);
                //o.wfe_serial = current_entitie[0].serial;
                o.wfe_serial = lastMi.WE_Entity_Ser;
                listRecord.Am.Add(o);
            }
            listRecord.Hm = new List<HistroyModel>();
            HistroyModel h;
            List<UI_WFEntity_Info> start_entities = CWFEngine.GetWFEntityByHistoryDone(t => t.Record_Info.Count(q => q.Re_Name == "username" && q.Re_Value == username) > 0 && t.Miss_WFentity.WE_Wref.W_Name == WorkFlow_Name);
            foreach (var item in start_entities)
            {
                h = new HistroyModel();
                List<UI_MISSION> AllHistoryMiss = CWFEngine.GetHistoryMissions(item.EntityID);
                int Miss_Id = AllHistoryMiss[0].Miss_Id;
                IDictionary<string, string> r = CWFEngine.GetMissionRecordInfo(Miss_Id);
                if (r.Count > 0)
                {
                    h.missStartName = r["username"];
                    h.missStartTime = r["time"];
                }
                else
                {
                    h.missStartName = "";
                    h.missStartTime = "";
                }
                h.missIndex = start_entities.IndexOf(item) + 1;
                h.wfe_serial = item.serial;
                UI_MISSION miss_last = AllHistoryMiss[AllHistoryMiss.Count() - 1];

                h.miss_LastStatusdesc = miss_last.WE_Event_Desc;
                h.miss_wfe_Id = item.EntityID;
                listRecord.Hm.Add(h);
            }
            return listRecord;
        }

        /// <summary>
        /// 获取index页面待处理列表内容
        /// </summary>
        /// <param name="WorkFlow_Name"></param>
        /// <returns>json格式字符串给前台</returns>
        public string getdcl_list(string WorkFlow_Name)
        {
            List<MainModel> Am = new List<MainModel>();
            List<UI_MISSION> miss;
            List<object> or = new List<object>();
            miss = CWFEngine.GetActiveMissions<Person_Info>(((IObjectContextAdapter)(new EquipWebContext())).ObjectContext, WorkFlow_Name, true);
            foreach (var item in miss)
            {
                MainModel o = new MainModel();
                Dictionary<string, object> paras = new Dictionary<string, object>();
                paras["Equip_GyCode"] = null;
                UI_WFEntity_Info wfei = CWFEngine.GetWorkFlowEntityWithParams(item.WE_Entity_Id, paras);
                UI_MISSION lastMi = CWFEngine.GetHistoryMissions(item.WE_Entity_Id).Last();
                int Miss_Id = lastMi.Miss_Id;
                IDictionary<string, string> r = CWFEngine.GetMissionRecordInfo(Miss_Id);
                if (r.Count > 0)
                {
                    o.userName = r["username"];
                    o.missTime = r["time"];
                }
                else
                {
                    o.userName = "";
                    o.missTime = "";
                }
                o.miss_desc = item.WE_Event_Desc;
                if (item.Mission_Url.Contains("dot"))
                    o.miss_url = item.Mission_Url;
                else
                    o.miss_url = "";
                o.wfe_serial = wfei.serial;
                o.workFlowName = wfei.description;
                o.sbGycode = paras["Equip_GyCode"].ToString();
                Am.Add(o);
            }
            for (int i = 0; i < Am.Count; i++)
            {
                object o = new
                {
                    wfe_serial = Am[i].wfe_serial,
                    equip_gycode = Am[i].sbGycode,
                    workflow_name = Am[i].workFlowName,
                    miss_desc = Am[i].miss_desc,
                    missTime = Am[i].missTime,
                    userName = Am[i].userName,
                    miss_url = Am[i].miss_url
                };
                or.Add(o);

            }
            string str = JsonConvert.SerializeObject(or);
            return ("{" + "\"data\": " + str + "}");
        }
        /// <summary>
        /// 获取index页面历史操作列表
        /// </summary>
        /// <param name="WorkFlow_Name"></param>
        /// <returns>json格式字符串给前台</returns>
        public string gethistory_list(string WorkFlow_Name)
        {
            string username = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
            List<HistroyModel> Hm = new List<HistroyModel>();
            List<UI_WFEntity_Info> start_entities;
            List<object> or = new List<object>();
            if (WorkFlow_Name != "A7dot1dot1")
            {
                start_entities = CWFEngine.GetWFEntityByHistoryDone(t => t.Record_Info.Count(q => q.Re_Name == "username" && q.Re_Value == username) > 0 && t.Miss_WFentity.WE_Wref.W_Name == WorkFlow_Name);
                foreach (var item in start_entities)
                {
                    HistroyModel h = new HistroyModel();
                    Dictionary<string, object> paras = new Dictionary<string, object>();
                    paras["Equip_GyCode"] = null;
                    UI_WFEntity_Info wfei = CWFEngine.GetWorkFlowEntityWithParams(item.EntityID, paras);
                    List<UI_MISSION> AllHistoryMiss = CWFEngine.GetHistoryMissions(item.EntityID);
                    int Miss_Id = AllHistoryMiss[0].Miss_Id;
                    IDictionary<string, string> r = CWFEngine.GetMissionRecordInfo(Miss_Id);
                    if (r.Count > 0)
                    {
                        h.missStartName = r["username"];
                        h.missStartTime = r["time"];
                    }
                    else
                    {
                        h.missStartName = "";
                        h.missStartTime = "";
                    }

                    h.wfe_serial = item.serial;
                    h.miss_wfe_Id = item.EntityID;
                    h.workFlowName = wfei.description;
                    h.sbGycode = paras["Equip_GyCode"].ToString();
                    Hm.Add(h);
                }
            }
            else 
            {
                start_entities = CWFEngine.GetWFEntityByHistoryDone(t => t.Record_Info.Count(q => q.Re_Name == "username" && q.Re_Value == username) > 0 && t.Miss_WFentity.WE_Wref.W_Name == "A7dot1dot1");
                foreach (var item in start_entities)
                {
                    HistroyModel h = new HistroyModel();
                    Dictionary<string, object> paras = new Dictionary<string, object>();
                    paras["Equip_GyCode"] = null;
                    UI_WFEntity_Info wfei = CWFEngine.GetWorkFlowEntityWithParams(item.EntityID, paras);
                    List<UI_MISSION> AllHistoryMiss = CWFEngine.GetHistoryMissions(item.EntityID);
                    int Miss_Id = AllHistoryMiss[0].Miss_Id;
                    IDictionary<string, string> r = CWFEngine.GetMissionRecordInfo(Miss_Id);
                    if (r.Count > 0)
                    {
                        h.missStartName = r["username"];
                        h.missStartTime = r["time"];
                    }
                    else
                    {
                        h.missStartName = "";
                        h.missStartTime = "";
                    }

                    h.wfe_serial = item.serial;
                    h.miss_wfe_Id = item.EntityID;
                    h.workFlowName = wfei.description;
                    h.sbGycode = paras["Equip_GyCode"].ToString();
                    Hm.Add(h);
                }
                start_entities = CWFEngine.GetWFEntityByHistoryDone(t => t.Record_Info.Count(q => q.Re_Name == "username" && q.Re_Value == username) > 0 && t.Miss_WFentity.WE_Wref.W_Name == "A7dot1");
                foreach (var item in start_entities)
                {
                    HistroyModel h = new HistroyModel();
                    Dictionary<string, object> paras = new Dictionary<string, object>();
                    paras["Equip_GyCode"] = null;
                    UI_WFEntity_Info wfei = CWFEngine.GetWorkFlowEntityWithParams(item.EntityID, paras);
                    List<UI_MISSION> AllHistoryMiss = CWFEngine.GetHistoryMissions(item.EntityID);
                    int Miss_Id = AllHistoryMiss[0].Miss_Id;
                    IDictionary<string, string> r = CWFEngine.GetMissionRecordInfo(Miss_Id);
                    if (r.Count > 0)
                    {
                        h.missStartName = r["username"];
                        h.missStartTime = r["time"];
                    }
                    else
                    {
                        h.missStartName = "";
                        h.missStartTime = "";
                    }

                    h.wfe_serial = item.serial;
                    h.miss_wfe_Id = item.EntityID;
                    h.workFlowName = wfei.description;
                    h.sbGycode = paras["Equip_GyCode"].ToString();
                    Hm.Add(h);
                }
            }
            for (int i = 0; i < Hm.Count; i++)
            {
                object o = new
                {
                    wfe_serial = Hm[i].wfe_serial,
                    equip_gycode = Hm[i].sbGycode,
                    workflow_name = Hm[i].workFlowName,
                    missStartName = Hm[i].missStartName,
                    missStartTime = Hm[i].missStartTime,
                    miss_LastStatusdesc = "已完成",
                    miss_wfe_Id = Hm[i].miss_wfe_Id

                };
                or.Add(o);

            }
            string str = JsonConvert.SerializeObject(or);
            return ("{" + "\"data\": " + str + "}");
        }

        public string gethistory_listforA7dot1dot1(string WorkFlow_Name)
        {
            string username = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
            List<HistroyModel> Hm = new List<HistroyModel>();
            List<UI_WFEntity_Info> start_entities;
            List<object> or = new List<object>();
            if (WorkFlow_Name != "A7dot1dot1")
            {
                start_entities = CWFEngine.GetWFEntityByHistoryDone(t => t.Record_Info.Count(q => q.Re_Name == "username" && q.Re_Value == username) > 0 && t.Miss_WFentity.WE_Wref.W_Name == WorkFlow_Name);
                foreach (var item in start_entities)
                {
                    HistroyModel h = new HistroyModel();
                    Dictionary<string, object> paras = new Dictionary<string, object>();
                    paras["Equip_GyCode"] = null;
                    UI_WFEntity_Info wfei = CWFEngine.GetWorkFlowEntityWithParams(item.EntityID, paras);
                    List<UI_MISSION> AllHistoryMiss = CWFEngine.GetHistoryMissions(item.EntityID);
                    int Miss_Id = AllHistoryMiss[0].Miss_Id;
                    IDictionary<string, string> r = CWFEngine.GetMissionRecordInfo(Miss_Id);
                    if (r.Count > 0)
                    {
                        h.missStartName = r["username"];
                        h.missStartTime = r["time"];
                    }
                    else
                    {
                        h.missStartName = "";
                        h.missStartTime = "";
                    }

                    h.wfe_serial = item.serial;
                    h.miss_wfe_Id = item.EntityID;
                    h.workFlowName = wfei.description;
                    h.sbGycode = paras["Equip_GyCode"].ToString();
                    Hm.Add(h);
                }
            }
            else
            {
                start_entities = CWFEngine.GetWFEntityByHistoryDone(t => t.Record_Info.Count(q => q.Re_Name == "username" && q.Re_Value == username) > 0 && t.Miss_WFentity.WE_Wref.W_Name == "A7dot1dot1");
                foreach (var item in start_entities)
                {
                    HistroyModel h = new HistroyModel();
                    Dictionary<string, object> paras = new Dictionary<string, object>();
                    paras["Equip_GyCode"] = null;
                    UI_WFEntity_Info wfei = CWFEngine.GetWorkFlowEntityWithParams(item.EntityID, paras);
                    List<UI_MISSION> AllHistoryMiss = CWFEngine.GetHistoryMissions(item.EntityID);
                    int Miss_Id = AllHistoryMiss[0].Miss_Id;
                    IDictionary<string, string> r = CWFEngine.GetMissionRecordInfo(Miss_Id);
                    if (r.Count > 0)
                    {
                        h.missStartName = r["username"];
                        h.missStartTime = r["time"];
                    }
                    else
                    {
                        h.missStartName = "";
                        h.missStartTime = "";
                    }

                    h.wfe_serial = item.serial;
                    h.miss_wfe_Id = item.EntityID;
                    h.workFlowName = wfei.description;
                    h.sbGycode = paras["Equip_GyCode"].ToString();
                    Hm.Add(h);
                }
                start_entities = CWFEngine.GetWFEntityByHistoryDone(t => t.Record_Info.Count(q => q.Re_Name == "username" && q.Re_Value == username) > 0 && t.Miss_WFentity.WE_Wref.W_Name == "A7dot1");
                foreach (var item in start_entities)
                {
                    HistroyModel h = new HistroyModel();
                    Dictionary<string, object> paras = new Dictionary<string, object>();
                    paras["Equip_GyCode"] = null;
                    UI_WFEntity_Info wfei = CWFEngine.GetWorkFlowEntityWithParams(item.EntityID, paras);
                    List<UI_MISSION> AllHistoryMiss = CWFEngine.GetHistoryMissions(item.EntityID);
                    int Miss_Id = AllHistoryMiss[0].Miss_Id;
                    IDictionary<string, string> r = CWFEngine.GetMissionRecordInfo(Miss_Id);
                    if (r.Count > 0)
                    {
                        h.missStartName = r["username"];
                        h.missStartTime = r["time"];
                    }
                    else
                    {
                        h.missStartName = "";
                        h.missStartTime = "";
                    }

                    h.wfe_serial = item.serial;
                    h.miss_wfe_Id = item.EntityID;
                    h.workFlowName = wfei.description;
                    h.sbGycode = paras["Equip_GyCode"].ToString();
                    Hm.Add(h);
                }
            }
            for (int i = 0; i < Hm.Count; i++)
            {
                WorkFlows wfs = new WorkFlows();
                List<WorkFlow_Entity> getentity = wfs.GetWorkFlowEntitiesbySer(Hm[i].wfe_serial);
                int uncompletecount=0;//未完成的任务数
                string miss_LastStatusdesc="已完成";
                foreach(var vtim in getentity)
                {
                    if (vtim.WE_Status == WE_STATUS.ACTIVE)
                   {
                        uncompletecount++;
                        miss_LastStatusdesc="未封闭";
                   }
                }
                object o = new
                {
                    wfe_serial = Hm[i].wfe_serial,
                    equip_gycode = Hm[i].sbGycode,
                    workflow_name = Hm[i].workFlowName,
                    missStartName = Hm[i].missStartName,
                    missStartTime = Hm[i].missStartTime,
                    miss_LastStatusdesc = miss_LastStatusdesc,
                    miss_result = "有" + uncompletecount + "个未封闭任务",
                    miss_wfe_Id = Hm[i].miss_wfe_Id

                };
                or.Add(o);

            }
            string str = JsonConvert.SerializeObject(or);
            return ("{" + "\"data\": " + str + "}");
        }
        public string getactive_list(string WorkFlow_Name)
        {
            string username = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
            string query_list = "distinct E.WE_Id,R.username";
            string query_condition = "E.W_Name='" + WorkFlow_Name + "' and E.WE_Status='0' and M.Miss_Name <> 'Start' and R.username is not null";
            string record_filter = "username='" + username + "'";
            List<int> wfe_id = new List<int>();
            DataTable dt = CWFEngine.QueryAllInformation(query_list, query_condition, record_filter);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                wfe_id.Add(Convert.ToInt16(dt.Rows[i]["WE_Id"]));

            }
            List<HistroyModel> Hm = new List<HistroyModel>();
            foreach (var item in wfe_id)
            {
                HistroyModel h = new HistroyModel();
                Dictionary<string, object> paras = new Dictionary<string, object>();
                paras["Equip_GyCode"] = null;
                UI_WFEntity_Info wfei = CWFEngine.GetWorkFlowEntityWithParams(item, paras);
                List<UI_MISSION> AllHistoryMiss = CWFEngine.GetHistoryMissions(item);
                int Miss_Id = AllHistoryMiss[0].Miss_Id;
                h.miss_LastStatusdesc = AllHistoryMiss[AllHistoryMiss.Count - 1].WE_Event_Desc;
                IDictionary<string, string> r = CWFEngine.GetMissionRecordInfo(Miss_Id);
                if (r.Count > 0)
                {
                    h.missStartName = r["username"];
                    h.missStartTime = r["time"];
                }
                else
                {
                    h.missStartName = "";
                    h.missStartTime = "";
                }

                h.wfe_serial = wfei.serial;
                // h.miss_wfe_Id = wfei.EntityID;
                h.miss_wfe_Id = item;
                h.workFlowName = wfei.description;
                h.sbGycode = paras["Equip_GyCode"].ToString();
                Hm.Add(h);
            }
            List<object> or = new List<object>();

            for (int i = 0; i < Hm.Count; i++)
            {
                object o = new
                {
                    wfe_serial = Hm[i].wfe_serial,
                    equip_gycode = Hm[i].sbGycode,
                    workflow_name = Hm[i].workFlowName,
                    missStartName = Hm[i].missStartName,
                    missStartTime = Hm[i].missStartTime,
                    miss_LastStatusdesc = Hm[i].miss_LastStatusdesc,
                    miss_wfe_Id = Hm[i].miss_wfe_Id

                };
                or.Add(o);

            }
            string str = JsonConvert.SerializeObject(or);
            return ("{" + "\"data\": " + str + "}");
        }
        public JsonResult Cj_ZzsForA16(string cj_id)
        {
            EquipManagment pm = new EquipManagment();
            List<Equip_Archi> Zz = new List<Equip_Archi>();
            List<object> Zz_obj = new List<object>();
            string[] chejian_id = cj_id.Split(',');
            for (int i = 0; i < chejian_id.Length; i++)
            {
                Zz = pm.getZzs_ofCj(Convert.ToInt32(chejian_id[i]));
                foreach (var item in Zz)
                {
                    object o = new
                    {
                        Zz_Id = item.EA_Id,
                        Zz_Name = item.EA_Name
                    };
                    Zz_obj.Add(o);

                }
            }



            return Json(Zz_obj.ToArray());

        }

        public JsonResult Cj_Zzs(int cj_id)
        {
            PersonManagment pm = new PersonManagment();
            // EquipManagment em = new EquipManagment();
            int p_id = (Session["User"] as EquipModel.Entities.Person_Info).Person_Id;
            List<Equip_Archi> Zz = pm.Get_Person_Zz_ofCj(p_id, cj_id);
            List<object> Zz_obj = new List<object>();
            foreach (var item in Zz)
            {
                object o = new
                {
                    Zz_Id = item.EA_Id,
                    Zz_Name = item.EA_Name
                };
                Zz_obj.Add(o);

            }
            return Json(Zz_obj.ToArray());

        }

        public JsonResult Zz_Equips(int Zz_id)
        {
            string username = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
            EquipManagment pm = new EquipManagment();
            List<Equip_Info> Equips_of_Zz = pm.getEquips_OfZz(Zz_id, username);
            List<object> Equip_obj = new List<object>();
            foreach (var item in Equips_of_Zz)
            {
                object o = new
                {
                    Equip_Id = item.Equip_Id,
                    Equip_GyCode = item.Equip_GyCode,
                    // Equip_Code=item.Equip_Code,
                    //  Equip_Type=item.Equip_Type,
                    // Equip_Specialty=item.Equip_Specialty
                };
                Equip_obj.Add(o);
            }
            return Json(Equip_obj.ToArray());

        }

        public JsonResult ListEquip_Info(int Equip_Id)
        {
            EquipManagment pm = new EquipManagment();
            Equip_Info EquipInfo_selected = pm.getEquip_Info(Equip_Id);
            List<object> Equip_obj = new List<object>();

            object o = new
            {
                Equip_Code = EquipInfo_selected.Equip_Code,
                Equip_Type = EquipInfo_selected.Equip_Type,
                Equip_Specialty = EquipInfo_selected.Equip_Specialty,
                Equip_PhaseB = EquipInfo_selected.Equip_PhaseB
            };
            Equip_obj.Add(o);
            return Json(Equip_obj.ToArray());

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="wfe_id"></param>
        /// <returns></returns>
        public ZzSubmitModel getSubmitModel(string wfe_id)
        {
            PersonManagment pm = new PersonManagment();
            ZzSubmitModel mm = new ZzSubmitModel();
            ViewBag.WF_NAME = wfe_id;
            mm.UserName = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
            mm.UserDepartment = pm.Get_Person_DepartOfParent((Session["User"] as EquipModel.Entities.Person_Info).Person_Id).Depart_Name;
            mm.UserHasEquips = pm.Get_Person_Cj((Session["User"] as EquipModel.Entities.Person_Info).Person_Id);
            ViewBag.curtime = DateTime.Now.ToString();
            ViewBag.curuser = mm.UserName;
            ViewBag.equip_id = "";
            //邹晶修改161110
            try 
            {
                if (wfe_id != null && wfe_id != "")
                {
                 
                    EquipManagment Em = new EquipManagment();

                    ViewBag.cj_name = "";
                    ViewBag.zz_id = 0;
                    ViewBag.zz_name = "";
                    ViewBag.sb_gycode = "";
                    ViewBag.sb_code = "";
                    ViewBag.cj_id = 0;
                    Dictionary<string, object> paras = new Dictionary<string, object>();
                    paras["Zz_Name"] = null;
                    paras["Equip_GyCode"] = null;
                    paras["Equip_Code"] = null;
                    WorkFlows wfsd = new WorkFlows();
                    UI_WFEntity_Info wfei = CWFEngine.GetWorkFlowEntityWithParams(Convert.ToInt32(wfe_id), paras);
                    Dictionary<string, object> m_kv = new Dictionary<string, object>();
                    Mission db_miss = wfsd.GetWFEntityMissions(Convert.ToInt32(wfe_id)).Last();//获取该实体最后一个任务

                    if (paras["Zz_Name"].ToString() != null && paras["Zz_Name"].ToString() != "")
                    {

                        ViewBag.zz_name = paras["Zz_Name"].ToString();
                        ViewBag.zz_id = Em.getEa_idbyname(paras["Zz_Name"].ToString());
                        ViewBag.cj_name = Em.getEquip(paras["Zz_Name"].ToString()).ToString();
                        ViewBag.cj_id = Em.getEA_parentid(ViewBag.zz_id);
                    }
                       
                    if (paras["Equip_GyCode"].ToString() != null && paras["Equip_GyCode"].ToString() != "")
                    {
                        ViewBag.sb_gycode = paras["Equip_GyCode"].ToString();
                        ViewBag.sb_code = Em.getEquip_ByGyCode(paras["Equip_GyCode"].ToString()).Equip_Code;
                        ViewBag.equip_id = Em.getEquip_ByGyCode(paras["Equip_GyCode"].ToString()).Equip_Id.ToString();
                        ViewBag.zz_id = Em.getEA_id_byCode(ViewBag.sb_code);
                        ViewBag.zz_name = Em.getEquip_ZzBelong(Convert.ToInt32(ViewBag.equip_id))[0].EA_Name;
                       // ViewBag.zz_id = Em.getEa_idbyname(ViewBag.zz_name);
                        ViewBag.cj_name = Em.getEquip(ViewBag.zz_name).ToString();
                        ViewBag.cj_id = Em.getEA_parentid(ViewBag.zz_id);
                    }
                       
                }
            }
           catch(Exception e)
            {
                return mm;

            }

            return mm;
        }
        /// <summary>
        /// 为润滑间管理创建的model,目的是提报时获取润滑间列表
        /// </summary>
        /// <param name="wfe_id"></param>
        /// <returns></returns>
        public ZzRunhuaSubmitModel getRunhuaSubmitModel(string wfe_id)
        {
            PersonManagment pm = new PersonManagment();
            ZzRunhuaSubmitModel mm = new ZzRunhuaSubmitModel();
            ViewBag.WF_NAME = wfe_id;
            mm.UserName = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
            mm.UserDepartment = pm.Get_Person_DepartOfParent((Session["User"] as EquipModel.Entities.Person_Info).Person_Id).Depart_Name;
            mm.UserHasEquips = pm.Get_Runhua_Cj();
            ViewBag.curtime = DateTime.Now.ToString();
            ViewBag.curuser = mm.UserName;
            return mm;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="wfe_id"></param>
        /// <returns></returns>
        public MissInfoModel getMissInfoModel(string wfe_id)
        {
            MissInfoModel cm = new MissInfoModel();
            ViewBag.WF_NAME = wfe_id;
            cm.miModel = CWFEngine.GetActiveMission<Person_Info>(int.Parse(wfe_id), ((IObjectContextAdapter)(new EquipWebContext())).ObjectContext);
            cm.MissTime = new List<string>();
            cm.MissUser = new List<string>();
            string t;
            foreach (var item in CWFEngine.GetHistoryMissions(cm.miModel.WE_Entity_Id))
            {
                IDictionary<string, string> r = CWFEngine.GetMissionRecordInfo(item.Miss_Id);
                t = r["username"];
                cm.MissUser.Add(t);
                t = r["time"];
                cm.MissTime.Add(t);
            }
            ViewBag.curtime = DateTime.Now.ToString();
            ViewBag.curuser = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
            return cm;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="wfe_id"></param>
        /// <returns></returns>
        public WFDetail_Model getWFDetail_Model(string wfe_id)
        {
            WFDetail_Model cm = new WFDetail_Model();
            ViewBag.WF_NAME = wfe_id;
            cm.ALLHistoryMiss = CWFEngine.GetHistoryMissions(int.Parse(wfe_id));
            ViewBag.WF_Ser = cm.ALLHistoryMiss[0].WE_Entity_Ser;
            cm.MissTime = new List<string>();
            cm.MissUser = new List<string>();
            string t;
            foreach (var item in cm.ALLHistoryMiss)
            {
                IDictionary<string, string> r = CWFEngine.GetMissionRecordInfo(item.Miss_Id);
                if (r.Count > 0)
                {
                    t = r["username"];
                    cm.MissUser.Add(t);
                    t = r["time"];
                    cm.MissTime.Add(t);
                }
                else
                {
                    cm.MissUser.Add("");
                    cm.MissTime.Add("");
                }
            }
            ViewBag.curtime = DateTime.Now.ToString();
            ViewBag.curuser = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
            return cm;
        }
        public class Gxqmodel
        {
           public int WE_Id;
           public string WE_Ser;
        }
        public class A5dot1Gxqmodel
        {
            public string wfe_serial;
            public string sbGycode;
            public string workFlowName;
            public string missStartName;
            public string missStartTime;
            public string miss_LastStatusdesc;
            public string miss_wfe_Id;

        }
        /// <summary>
        /// 获取用户可能感兴趣的列表，邹晶0726
        /// </summary>
        /// <returns></returns>
        public string Getgxq_list(string WE_Status, string WorkFlow_Name, string Cjname, string time)
        {
            string username = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
            string record_filter = "";
            string query_list = "";
            if (time == "")
            {
                record_filter = "username is not null";
                query_list = "distinct E.WE_Ser, E.WE_Id, R.username,R.time";
            }
            else
            {
                string[] strtime = time.Split(new char[] { '-' });
                record_filter = "time >= '" + strtime[0] + "00:00:00'" + "and  time <= '" + strtime[1] + " 00:00:00' and username is not null";
                query_list = "distinct E.WE_Ser, E.WE_Id, R.username,R.time";
            }
            //string WorkFlow_Name = "A11dot1";
            //string Cjname = "联合一车间";
            //string WE_Status = "0";

            string query_condition = "";
            if (WorkFlow_Name != "" && Cjname != "")
                query_condition = "E.W_Name='" + WorkFlow_Name + "' and E.WE_Status='" + WE_Status + "'and P.Cj_Name='" + Cjname + "' and R.username is not null";
            else if (WorkFlow_Name != "" && Cjname == "")
                query_condition = "E.W_Name='" + WorkFlow_Name + "' and E.WE_Status='" + WE_Status + "' and R.username is not null";
            else if (WorkFlow_Name == "" && Cjname != "")
                query_condition = "E.WE_Status='" + WE_Status + "'and P.Cj_Name='" + Cjname + "' and R.username is not null";
            else
                query_condition = "WE_Status='0' and R.username is not null";

            DataTable dt = CWFEngine.QueryAllInformation(query_list, query_condition, record_filter);
           
            var tmp = from t in dt.AsEnumerable()
                      group t by t.Field<int>("WE_Id") into g
                      select new { WE_Id = g.First().Field<int>("WE_Id"), time = g.Max(item => item.Field<string>("time")) };
            //var a = tmp.ToList();
            var res = from d1 in dt.AsEnumerable()
                      from d2 in tmp
                      where d1.Field<int>("WE_Id") == d2.WE_Id && d1.Field<string>("time") == d2.time
                      select d1;
            //var b = res.ToList();      
            List<Gxqmodel> Gmlist = new List<Gxqmodel>();
            for (int i = 0; i < res.ToList().Count; i++)
            {
                Gxqmodel Gm = new Gxqmodel();
                Gm.WE_Id = Convert.ToInt16(res.ToList()[i]["WE_Id"]);
                Gm.WE_Ser = res.ToList()[i]["WE_Ser"].ToString();
                Gmlist.Add(Gm);

            }
            List<HistroyModel> Hm = new List<HistroyModel>();
            foreach (var item in Gmlist)
            {
                HistroyModel h = new HistroyModel();
                Dictionary<string, object> paras = new Dictionary<string, object>();
                paras["Equip_GyCode"] = null;
                UI_WFEntity_Info wfei = CWFEngine.GetWorkFlowEntityWithParams(item.WE_Id, paras);
                List<UI_MISSION> AllHistoryMiss = CWFEngine.GetHistoryMissions(item.WE_Id);
                int Miss_Id = AllHistoryMiss[0].Miss_Id;
                h.miss_LastStatusdesc = AllHistoryMiss[AllHistoryMiss.Count - 1].WE_Event_Desc;
                IDictionary<string, string> r = CWFEngine.GetMissionRecordInfo(Miss_Id);
                if (r.Count > 0)
                {
                    h.missStartName = r["username"];
                    h.missStartTime = r["time"];
                }
                else
                {
                    h.missStartName = "";
                    h.missStartTime = "";
                }

                h.wfe_serial = item.WE_Ser;
                // h.miss_wfe_Id = wfei.EntityID;
                h.miss_wfe_Id = item.WE_Id;
                h.workFlowName = wfei.description;
                h.sbGycode = paras["Equip_GyCode"].ToString();
                Hm.Add(h);
            }




            List<A5dot1Tab1> qc_list = new List<A5dot1Tab1>();
            TablesManagment tm = new TablesManagment();
            if (WorkFlow_Name == "" || WorkFlow_Name == "A5dot1")
            {
                if (Cjname == "")
                {
                    qc_list = tm.get_All();
                }
                else
                {
                    qc_list = tm.get_cj_bwh(Cjname, 0);
                }


            }


            List<A5dot1Gxqmodel> A5model = new List<A5dot1Gxqmodel>();
            //A5dot1Gxqmodel A5dot1Gxqmodeltemp = new A5dot1Gxqmodel();
            //double qc_bxhcount = 0;
            int wzgcount = 0;

            if (qc_list.Count > 0)
            {
                string s = "";
                string isZG = "";
                string problemDescription = "";
                //qc_bxhcount = 0;
                wzgcount = 0;
                string sbcode_temp = qc_list[0].sbCode;
                for (int j = 0; j < qc_list.Count; j++)
                {
                    if (WorkFlow_Name == "" || WorkFlow_Name == "A5dot1")
                    {
                        if (Cjname == "")
                        {
                            qc_list = tm.get_All();
                        }
                        else
                        {
                            qc_list = tm.get_cj_bwh(Cjname, 0);
                        }


                    }
                    if (qc_list[j].temp1 == null)
                    {
                        A5dot1Gxqmodel A5dot1Gxqmodeltemp = new A5dot1Gxqmodel();
                        List<A5dot1Tab1> cj_bycode = tm.GetAll1_bycode(qc_list[j].sbCode);
                        for (int k = 0; k < cj_bycode.Count; k++)
                        {
                            if (cj_bycode[k].isRectified == 0)
                            {
                                wzgcount++;
                                isZG = "未整改";
                            }
                            else
                                isZG = "已整改";
                            s += cj_bycode[k].notGoodContent + " (" + isZG + ") ";
                            problemDescription += cj_bycode[k].notGoodContent;

                            tm.modifytemp1_byid(cj_bycode[k].Id, "已合并");

                        }
                        if (wzgcount > 0)
                        {
                            // qc_bxhcount++;
                            A5dot1Gxqmodeltemp.miss_LastStatusdesc = "等待未整改项整改";

                        }
                        else
                        {
                            A5dot1Gxqmodeltemp.miss_LastStatusdesc = "可靠性工程师整改结束";
                        }
                        wzgcount = 0;

                        A5dot1Gxqmodeltemp.miss_wfe_Id = s;
                        A5dot1Gxqmodeltemp.sbGycode = qc_list[j].sbCode;
                        A5dot1Gxqmodeltemp.wfe_serial = "";
                        A5dot1Gxqmodeltemp.missStartName = qc_list[j].zzUserName;
                        A5dot1Gxqmodeltemp.missStartTime = qc_list[j].zzSubmitTime.ToString();
                        A5dot1Gxqmodeltemp.workFlowName = "设备完好";
                        A5model.Add(A5dot1Gxqmodeltemp);
                        s = "";
                    }
                }

            }
            for (int n = 0; n < qc_list.Count; n++)
            {
                tm.modifytemp1_byid(qc_list[n].Id, null);
            }




            List<object> or = new List<object>();
            for (int i = 0; i < A5model.Count; i++)
            {
                object o = new
                {
                    wfe_serial = A5model[i].wfe_serial,
                    equip_gycode = A5model[i].sbGycode,
                    workflow_name = A5model[i].workFlowName,
                    missStartName = A5model[i].missStartName,
                    missStartTime = A5model[i].missStartTime,
                    miss_LastStatusdesc = A5model[i].miss_LastStatusdesc,
                    miss_wfe_Id = A5model[i].miss_wfe_Id

                };
                or.Add(o);

            }


            for (int i = 0; i < Hm.Count; i++)
            {
                object o = new
                {
                    wfe_serial = Hm[i].wfe_serial,
                    equip_gycode = Hm[i].sbGycode,
                    workflow_name = Hm[i].workFlowName,
                    missStartName = Hm[i].missStartName,
                    missStartTime = Hm[i].missStartTime,
                    miss_LastStatusdesc = Hm[i].miss_LastStatusdesc,
                    miss_wfe_Id = Hm[i].miss_wfe_Id

                };
                or.Add(o);

            }
            string str = JsonConvert.SerializeObject(or);
            return ("{" + "\"data\": " + str + "}");
        }
        /// <summary>
        /// 获取用户默认全部可能感兴趣的列表，邹晶0726
        /// </summary>
        /// <returns></returns>
        public string MrGetgxq_list()
        {
            string username = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
            //string WorkFlow_Name = "A11dot1";
            //string Cjname = "联合一车间";
            //string WE_Status = "0";
            string query_list = "distinct E.WE_Ser, E.WE_Id, R.username, R.time";
            string query_condition = "WE_Status='0' and R.username is not null";
            string record_filter = "username is not null";
            DataTable dt = CWFEngine.QueryAllInformation(query_list, query_condition, record_filter);
            var tmp = from t in dt.AsEnumerable()
                      group t by t.Field<int>("WE_Id") into g
                      select new { WE_Id = g.First().Field<int>("WE_Id"), time = g.Max(item => item.Field<string>("time")) };
            //var a = tmp.ToList();
            var res = from d1 in dt.AsEnumerable()
                      from d2 in tmp
                      where d1.Field<int>("WE_Id") == d2.WE_Id && d1.Field<string>("time") == d2.time
                      select d1;
            //var b = res.ToList();      
            List<Gxqmodel> Gmlist = new List<Gxqmodel>();
            for (int i = 0; i < res.ToList().Count; i++)
            {
                Gxqmodel Gm = new Gxqmodel();
                Gm.WE_Id = Convert.ToInt16(res.ToList()[i]["WE_Id"]);
                Gm.WE_Ser = res.ToList()[i]["WE_Ser"].ToString();
                Gmlist.Add(Gm);

            }
            List<HistroyModel> Hm = new List<HistroyModel>();
            foreach (var item in Gmlist)
            {
                HistroyModel h = new HistroyModel();
                Dictionary<string, object> paras = new Dictionary<string, object>();
                paras["Equip_GyCode"] = null;
                UI_WFEntity_Info wfei = CWFEngine.GetWorkFlowEntityWithParams(item.WE_Id, paras);
                List<UI_MISSION> AllHistoryMiss = CWFEngine.GetHistoryMissions(item.WE_Id);
                int Miss_Id = AllHistoryMiss[0].Miss_Id;
                h.miss_LastStatusdesc = AllHistoryMiss.Last().WE_Event_Desc;
                IDictionary<string, string> r = CWFEngine.GetMissionRecordInfo(Miss_Id);
                if (r.Count > 0)
                {
                    h.missStartName = r["username"];
                    h.missStartTime = r["time"];
                }
                else
                {
                    h.missStartName = "";
                    h.missStartTime = "";
                }

                h.wfe_serial = item.WE_Ser;
                // h.miss_wfe_Id = wfei.EntityID;
                h.miss_wfe_Id = item.WE_Id;
                h.workFlowName = wfei.description;
                h.sbGycode = paras["Equip_GyCode"].ToString();
                Hm.Add(h);
            }

            TablesManagment tm = new TablesManagment();
            List<A5dot1Tab1> qc_list = tm.get_All();
            List<A5dot1Gxqmodel> A5model = new List<A5dot1Gxqmodel>();
            //A5dot1Gxqmodel A5dot1Gxqmodeltemp = new A5dot1Gxqmodel();
            //double qc_bxhcount = 0;
            int wzgcount = 0;

            if (qc_list.Count > 0)
            {
                string s = "";
                string isZG = "";
                string problemDescription = "";
                //qc_bxhcount = 0;
                wzgcount = 0;
                string sbcode_temp = qc_list[0].sbCode;
                for (int j = 0; j < qc_list.Count; j++)
                {
                    qc_list = tm.get_All();
                    if (qc_list[j].temp1 == null)
                    {
                        A5dot1Gxqmodel A5dot1Gxqmodeltemp = new A5dot1Gxqmodel();
                        List<A5dot1Tab1> cj_bycode = tm.GetAll1_bycode(qc_list[j].sbCode);
                        for (int k = 0; k < cj_bycode.Count; k++)
                        {
                            if (cj_bycode[k].isRectified == 0)
                            {
                                wzgcount++;
                                isZG = "未整改";
                            }
                            else
                                isZG = "已整改";
                            s += cj_bycode[k].notGoodContent + " (" + isZG + ") ";
                            problemDescription += cj_bycode[k].notGoodContent;

                            tm.modifytemp1_byid(cj_bycode[k].Id, "已合并");

                        }
                        if (wzgcount > 0)
                        {
                            // qc_bxhcount++;
                            A5dot1Gxqmodeltemp.miss_LastStatusdesc = "等待未整改项整改";

                        }
                        else
                        {
                            A5dot1Gxqmodeltemp.miss_LastStatusdesc = "可靠性工程师整改结束";
                        }
                        wzgcount = 0;

                        A5dot1Gxqmodeltemp.miss_wfe_Id = s;
                        A5dot1Gxqmodeltemp.sbGycode = qc_list[j].sbCode;
                        A5dot1Gxqmodeltemp.wfe_serial = "";
                        A5dot1Gxqmodeltemp.missStartName = qc_list[j].zzUserName;
                        A5dot1Gxqmodeltemp.missStartTime = qc_list[j].zzSubmitTime.ToString();
                        A5dot1Gxqmodeltemp.workFlowName = "设备完好";
                        A5model.Add(A5dot1Gxqmodeltemp);
                        s = "";
                    }
                }

            }
            for (int n = 0; n < qc_list.Count; n++)
            {
                tm.modifytemp1_byid(qc_list[n].Id, null);
            }




            List<object> or = new List<object>();
            for (int i = 0; i < A5model.Count; i++)
            {
                object o = new
                {
                    wfe_serial = A5model[i].wfe_serial,
                    equip_gycode = A5model[i].sbGycode,
                    workflow_name = A5model[i].workFlowName,
                    missStartName = A5model[i].missStartName,
                    missStartTime = A5model[i].missStartTime,
                    miss_LastStatusdesc = A5model[i].miss_LastStatusdesc,
                    miss_wfe_Id = A5model[i].miss_wfe_Id

                };
                or.Add(o);

            }


            for (int i = 0; i < Hm.Count; i++)
            {
                object o = new
                {
                    wfe_serial = Hm[i].wfe_serial,
                    equip_gycode = Hm[i].sbGycode,
                    workflow_name = Hm[i].workFlowName,
                    missStartName = Hm[i].missStartName,
                    missStartTime = Hm[i].missStartTime,
                    miss_LastStatusdesc = Hm[i].miss_LastStatusdesc,
                    miss_wfe_Id = Hm[i].miss_wfe_Id

                };
                or.Add(o);

            }

            string str = JsonConvert.SerializeObject(or);
            return ("{" + "\"data\": " + str + "}");
        }

        /// <summary>
        /// zoujing1012存定时事件详细信息
        /// </summary>
        /// <param name="work_name"></param>
        /// <param name="event_name"></param>
        /// <returns></returns>
        //public bool SubmitDSEventDetails(string work_name, string event_name)
        //{
        //    DsWorkManagment dm = new DsWorkManagment();
        //    DateTime now = DateTime.Now;
        //    string year = now.Year.ToString();
        //    string month = now.Month.ToString();
        //    if (dm.IsAlreadySave(work_name, event_name, year, month))
        //        return true;
        //    else
        //    {
        //        DSEventDetail ds = new DSEventDetail();
        //        ds.work_name = work_name;
        //        ds.event_name = event_name;
        //        ds.done_time = now;
        //        ds.year = year;
        //        ds.month = month;
        //        string time = dm.getDstime(work_name, event_name).Trim();

        //        if (time.Contains("-"))
        //        {
        //            string[] s = time.Split(new char[] { '-' });
        //            if (now.Month < Convert.ToInt16(s[0]))
        //                ds.state = "1";
        //            else if (now.Month == Convert.ToInt16(s[0]) & now.Day < Convert.ToInt16(s[1]))
        //                ds.state = "1";
        //            else
        //                ds.state = "0";
        //        }
        //        else if (time.Contains("*"))
        //        {
        //            string[] s = time.Split(new char[] { '*' });
        //            if ((int)now.DayOfWeek < Convert.ToInt16(s[0]))
        //                ds.state = "1";
        //            else
        //                ds.state = "0";
        //        }
        //        else if (time.Contains(","))
        //        {
        //            string[] s = time.Split(new char[] { ',' });
        //            if ((int)now.Day < Convert.ToInt16(s[s.Count() - 1]))
        //                ds.state = "1";
        //            else
        //                ds.state = "0";

        //        }
        //        else
        //        {
        //            if ((int)now.Day < Convert.ToInt16(time))
        //                ds.state = "1";
        //            else
        //                ds.state = "0";
        //        }
        //        dm.AddDsEvent(ds);
        //        return true;
        //    }
        //}
                
        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeA5">A5dot1或A5dot2</param>
        /// <returns></returns>
    
        public A5_Model getA5_Model(string WorkFlow_Name_Of_A5)
        {
            A5_Model A5model = new A5_Model();
            A5model.A5_ModelInfoList = new List<A5_ModelInfo>();
            A5_ModelInfo h;
            List<UI_WFEntity_Info> start_entities = CWFEngine.GetWFEntityByHistoryDone(t => t.Miss_WFentity.WE_Wref.W_Name == WorkFlow_Name_Of_A5);
            foreach (var item in start_entities)
            {
                h = new A5_ModelInfo();
                List<UI_MISSION> AllHistoryMiss = CWFEngine.GetHistoryMissions(item.EntityID);
                int Miss_Id = AllHistoryMiss[0].Miss_Id;
                if (AllHistoryMiss.Count > 2)
                {
                    h.Pq_Name = AllHistoryMiss[1].Miss_Params["Cj_Name"].ToString();
                    h.Zz_Name = AllHistoryMiss[1].Miss_Params["Zz_Name"].ToString();
                    h.Equip_Code = AllHistoryMiss[1].Miss_Params["Equip_Code"].ToString();
                    h.Equip_GyCode = AllHistoryMiss[1].Miss_Params["Equip_GyCode"].ToString();
                    h.nTimesInMonth = 2;
                    h.nTimesInYear = 3;
                    h.nFlagTheWorstTen = 0;
                    A5model.A5_ModelInfoList.Add(h);
                }
                else
                {
                }

            }
            return A5model;
        }

        //[HttpPost]
        //public JsonResult Upload(HttpPostedFileBase file)
        //{

        //    if (file.ContentLength == 0)
        //    {
        //        return Json(new { message = "没有文件" });
        //    }
        //    var fileName = Path.Combine(Request.MapPath("~/Upload"), Path.GetFileName(file.FileName));
        //    try
        //    {
        //        file.SaveAs(fileName);
        //        var FileSize = (file.ContentLength * 1.0 / 1024).ToString("0.00");
        //        Dictionary<string, string> r = new Dictionary<string, string>();
        //        r.Add("message", "上传成功");
        //        r.Add("fileName", file.FileName);
        //        r.Add("fileSize", FileSize);
        //        return Json(r);
        //    }
        //    catch
        //    {
        //        return Json(new { message = "上传失败" });
        //    }
        //}

        //public JsonResult DelAttachment(string file)
        //{
        //    try
        //    {
        //        string filePath = Path.Combine(Request.MapPath("~/Upload"), file);
        //        if (System.IO.File.Exists(filePath))
        //        {
        //            System.IO.File.Delete(filePath);
        //            return Json(new { status = "success" });
        //        }
        //        return Json(new { status = "false", message = "附件删除失败!" });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { status = "false", message = "附件删除失败!" });
        //    }
        //}

        private string getGUID()
        {
            System.Guid guid = new Guid();
            guid = Guid.NewGuid();
            string str = guid.ToString();
            return str;
        }
        [HttpPost]
        public JsonResult Upload(HttpPostedFileBase file)
        {
            if (file == null)
            {
                return Json(new { message = "没有选择上传文件！" });
            }

            if (file.ContentLength == 0)
            {
                return Json(new { message = "没有文件" });
            }
            string savedFileName = getGUID() + Path.GetExtension(file.FileName);
            string uploadFileName = Path.GetFileName(file.FileName);

            string allFileNames = uploadFileName + "$" + savedFileName;
            var fileName = Path.Combine(Request.MapPath("~/Upload"), Path.GetFileName(savedFileName));
            try
            {
                file.SaveAs(fileName);
                var FileSize = (file.ContentLength * 1.0 / 1024).ToString("0.00");
                Dictionary<string, string> r = new Dictionary<string, string>();
                r.Add("message", "上传成功");
                r.Add("fileName", Path.GetFileName(file.FileName));
                r.Add("fileSize", FileSize);
                r.Add("allFileNames", allFileNames);
                return Json(r);
            }
            catch
            {
                return Json(new { message = "上传失败" });
            }
        }
        [HttpPost]
        public JsonResult UploadforA6dot1(HttpPostedFileBase file)
        {
            if (file == null)
            {
                return Json(new { message = "没有选择上传文件！" });
            }

            if (file.ContentLength == 0)
            {
                return Json(new { message = "没有文件" });
            }
            string savedFileName = getGUID() + Path.GetExtension(file.FileName);
            string uploadFileName = Path.GetFileName(file.FileName);

            string allFileNames = uploadFileName + "$" + savedFileName;
            var fileName = Path.Combine(Request.MapPath("~/Upload/A3dot3"), Path.GetFileName(savedFileName));
            try
            {
                file.SaveAs(fileName);
                var FileSize = (file.ContentLength * 1.0 / 1024).ToString("0.00");
                Dictionary<string, string> r = new Dictionary<string, string>();
                r.Add("message", "上传成功");
                r.Add("fileName", Path.GetFileName(file.FileName));
                r.Add("fileSize", FileSize);
                r.Add("allFileNames", allFileNames);
                return Json(r);
            }
            catch
            {
                return Json(new { message = "上传失败" });
            }
        }
        [HttpPost]
        public JsonResult UploadXlxs(HttpPostedFileBase file)
        {
            if (file == null)
            {
                return Json(new { message = "没有选择上传文件！" });
            }

            if (file.ContentLength == 0)
            {
                return Json(new { message = "没有文件" });
            }
            if (Path.GetExtension(file.FileName) != ".xlsx")
            {
                return Json(new { message = "上传文件不是.xlsx文件！" });
            }
            string savedFileName = getGUID() + Path.GetExtension(file.FileName);
            string uploadFileName = Path.GetFileName(file.FileName);

            string allFileNames = uploadFileName + "$" + savedFileName;
            var fileName = Path.Combine(Request.MapPath("~/Upload"), Path.GetFileName(savedFileName));
            try
            {
                file.SaveAs(fileName);
                var FileSize = (file.ContentLength * 1.0 / 1024).ToString("0.00");
                Dictionary<string, string> r = new Dictionary<string, string>();
                r.Add("message", "上传成功");
                r.Add("fileName", Path.GetFileName(file.FileName));
                r.Add("fileSize", FileSize);
                r.Add("allFileNames", allFileNames);
                return Json(r);
            }
            catch
            {
                return Json(new { message = "上传失败" });
            }
        }
        [HttpPost]

        public JsonResult UploadForFileManage(FormCollection fromdata)
        {
            int pCatalogID = Convert.ToInt32(fromdata["pCataID"]);
            HttpPostedFileBase file = Request.Files[0];
            if (file.ContentLength == 0)
            {
                return Json(new { message = "没有文件" });
            }
            string savedFileName = getGUID() + Path.GetExtension(file.FileName);
            string uploadFileName = Path.GetFileName(file.FileName);

            string allFileNames = uploadFileName + "$" + savedFileName;
            var fileName = Path.Combine(Request.MapPath("~/Upload/A3dot3"), Path.GetFileName(savedFileName));
            try
            {
                file.SaveAs(fileName);
                var FileSize = (file.ContentLength * 1.0 / 1024).ToString("0.00");
                FileItem fi = new FileItem();
                fi.fileName = uploadFileName;
                fi.fileNamePresent = savedFileName;
                fi.updateTime = DateTime.Now.ToString();
                fi.path = @"/Upload/A3dot3";
                fi.ext = Path.GetExtension(file.FileName);
                if (!(new File_Cat_Manager()).AddNewFile(pCatalogID, fi, (Session["User"] as EquipModel.Entities.Person_Info).Person_Id))
                {
                    throw new Exception();
                }

                Dictionary<string, string> r = new Dictionary<string, string>();
                r.Add("message", "上传成功");
                r.Add("fileName", Path.GetFileName(file.FileName));
                r.Add("fileSize", FileSize);
                r.Add("allFileNames", allFileNames);
                return Json(r);
            }
            catch
            {
                return Json(new { message = "上传失败" });
            }
        }
        //2016-5-11 修改的方法如下
        [HttpPost]
        public void UploadForFileManage1(FormCollection fromdata)
        {


            try
            {
                int i;
                for (i = 0; i < Request.Files.Count; i++)
                {
                    int pCatalogID = Convert.ToInt32(fromdata["pCataID"]);

                    HttpPostedFileBase file = Request.Files[i];

                    if (file.ContentLength == 0)
                    {
                        //return Json(new { message = "没有文件" });
                    }
                    string savedFileName = getGUID() + Path.GetExtension(file.FileName);
                    string uploadFileName = Path.GetFileName(file.FileName);

                    string allFileNames = uploadFileName + "$" + savedFileName;
                    var fileName = Path.Combine(Request.MapPath("~/Upload/A3dot3"), Path.GetFileName(savedFileName));


                    file.SaveAs(fileName);
                    var FileSize = (file.ContentLength * 1.0 / 1024).ToString("0.00");
                    FileItem fi = new FileItem();
                    fi.fileName = uploadFileName;
                    fi.fileNamePresent = savedFileName;
                    fi.updateTime = DateTime.Now.ToString();
                    fi.path = @"/Upload/A3dot3";
                    fi.ext = Path.GetExtension(file.FileName);
                    if (!(new File_Cat_Manager()).AddNewFile(pCatalogID, fi, (Session["User"] as EquipModel.Entities.Person_Info).Person_Id))
                    {
                        throw new Exception();
                    }

                    Dictionary<string, string> r = new Dictionary<string, string>();
                    r.Add("message", "上传成功");
                    r.Add("fileName", Path.GetFileName(file.FileName));
                    r.Add("fileSize", FileSize);
                    r.Add("allFileNames", allFileNames);
                    //   return Json(r);

                }

            }
            catch
            {
                // return Json(new { message = "上传失败" });
            }



        }

        public JsonResult DelAttachment(string file)
        {
            try
            {
                string[] savedFileName = file.Split(new char[] { '$' });
                string filePath = Path.Combine(Request.MapPath("~/Upload"), savedFileName[1]);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                    return Json(new { status = "success" });
                }
                return Json(new { status = "false", message = "附件删除失败!" });
            }
            catch (Exception ex)
            {
                return Json(new { status = "false", message = "附件删除失败!" });
            }
        }

        //取消提交，从数据库中删除刚建立的一个工作流实体
        public string CancelSubmit(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string wfe_id = item["wef_id"].ToString();
                string return_url = item["return_url"].ToString();
                CWFEngine.RemoveWFEntity(int.Parse(wfe_id));
                return return_url;
            }
            catch (Exception e)
            {
                return "";
            }
        }


        //获取待处理任务:帽子工作流
        public string getLsA11dot2dcl_list(string WorkFlow_Name, string jobname)
            {
            List<MainModel> Am = new List<MainModel>();
            List<UI_MISSION> miss;
            List<object> or = new List<object>();
            miss = CWFEngine.GetActiveMissions<Person_Info>(((IObjectContextAdapter)(new EquipWebContext())).ObjectContext, WorkFlow_Name, true);
            foreach (var item in miss)
                {
                MainModel o = new MainModel();
                Dictionary<string, object> paras = new Dictionary<string, object>();
                paras["Equip_GyCode"] = null;
                UI_WFEntity_Info wfei = CWFEngine.GetWorkFlowEntityWithParams(item.WE_Entity_Id, paras);
                UI_MISSION lastMi = CWFEngine.GetHistoryMissions(item.WE_Entity_Id).Last();
                int Miss_Id = lastMi.Miss_Id;
                IDictionary<string, string> r = CWFEngine.GetMissionRecordInfo(Miss_Id);
                if (r.Count > 0)
                    {
                    o.userName = r["username"];
                    o.missTime = r["time"];
                    }
                else
                    {
                    o.userName = "";
                    o.missTime = "";
                    }
                o.miss_desc = item.WE_Event_Desc;
                if (item.Mission_Url.Contains("dot"))
                    o.miss_url = item.Mission_Url;
                else
                    o.miss_url = "";
                o.wfe_serial = wfei.serial;
                o.workFlowName = wfei.description;
                o.sbGycode = jobname;
                Am.Add(o);
                }
            for (int i = 0; i < Am.Count; i++)
                {
                object o = new
                {
                    wfe_serial = Am[i].wfe_serial,
                    equip_gycode = jobname,
                    workflow_name = Am[i].workFlowName,
                    miss_desc = Am[i].miss_desc,
                    missTime = Am[i].missTime,
                    userName = Am[i].userName,
                    miss_url = Am[i].miss_url
                };
                or.Add(o);

                }
            string str = JsonConvert.SerializeObject(or);
            return ("{" + "\"data\": " + str + "}");
            }


        public string getA11dot2dcl_list(string WorkFlow_Name)
            {
            List<A11dot2Model> Am = new List<A11dot2Model>();
            List<UI_MISSION> miss;
            List<object> or = new List<object>();
            miss = CWFEngine.GetActiveMissions<Person_Info>(((IObjectContextAdapter)(new EquipWebContext())).ObjectContext, WorkFlow_Name, true);
            int i = 1;
            foreach (var item in miss)
                {
                A11dot2Model o = new A11dot2Model();
                Dictionary<string, object> paras = new Dictionary<string, object>();
                paras["Zz_Name"] = null;
                paras["Equip_GyCode"] = null;
                paras["Equip_Code"] = null;
                paras["Problem_Desc"] = null;//隐患问题描述
                paras["RiskMatrix_Color"] = null;//隐患评估结果
                paras["Supervise_done"] = null;//片区监督实施是否完成
                paras["ImplementPlan_done"] = null;//黄色、措施实施是否完成


                WorkFlows wfsd = new WorkFlows();
                UI_WFEntity_Info wfei = CWFEngine.GetWorkFlowEntityWithParams(item.WE_Entity_Id, paras);
                Mission db_miss = wfsd.GetWFEntityMissions(item.WE_Entity_Id).Last();//获取该实体最后一个任务


                o.index_Id = item.WE_Entity_Id;
                o.zz_name = paras["Zz_Name"].ToString();
                o.sb_gycode = paras["Equip_GyCode"].ToString();
                o.sb_code = paras["Equip_Code"].ToString();

                o.problem_desc = paras["Problem_Desc"].ToString();
                o.riskMatrix_color = paras["RiskMatrix_Color"].ToString();
                o.supervise_done = paras["Supervise_done"].ToString();
                o.implementplan_done = paras["ImplementPlan_done"].ToString();
                o.missionname = db_miss.Miss_Desc;

                Am.Add(o);
                i++;
                }
            string str = JsonConvert.SerializeObject(Am);
            return ("{" + "\"data\": " + str + "}");
            }

        /// <summary>
        /// 获取A14dot3index页面待处理列表内容(在线表单)
        /// </summary>
        /// <param name="WorkFlow_Name"></param>
        /// <returns>json格式字符串给前台</returns>
        public string getA14dot3dcl_list(string WorkFlow_Name)
            {
            List<A14dot3Model> Am = new List<A14dot3Model>();
            List<UI_MISSION> miss;
            List<object> or = new List<object>();
            miss = CWFEngine.GetActiveMissions<Person_Info>(((IObjectContextAdapter)(new EquipWebContext())).ObjectContext, WorkFlow_Name, true);
            int i = 1;
            foreach (var item in miss)
                {
                A14dot3Model o = new A14dot3Model();
                Dictionary<string, object> paras = new Dictionary<string, object>();
                paras["Zz_Name"] = null;
                paras["Equip_GyCode"] = null;
                paras["Equip_Code"] = null;
                paras["Equip_Type"] = null;
                paras["Equip_ABCMark"] = null;
                paras["Plan_Name"] = null;
                paras["JxCauseDesc"] = null;
                paras["XcConfirm_Result"] = null;
                paras["KkConfirm_Result"] = null;
                paras["ZytdConfirm_Result"] = null;
                paras["JobOrder"] = null;
                paras["NoticeOrder"] = null;
                WorkFlows wfsd = new WorkFlows();
                UI_WFEntity_Info wfei = CWFEngine.GetWorkFlowEntityWithParams(item.WE_Entity_Id, paras);
                Mission db_miss = wfsd.GetWFEntityMissions(item.WE_Entity_Id).Last();//获取该实体最后一个任务


                o.index_Id = item.WE_Entity_Id;
                o.zz_name = paras["Zz_Name"].ToString();
                o.sb_gycode = paras["Equip_GyCode"].ToString();
                o.sb_code = paras["Equip_Code"].ToString();
                o.sb_type = paras["Equip_Type"].ToString();
                o.sb_ABCMark = paras["Equip_ABCMark"].ToString();
                o.plan_name = paras["Plan_Name"].ToString();
                o.jxreason = paras["JxCauseDesc"].ToString();
                o.xcconfirm = paras["XcConfirm_Result"].ToString();
                o.kkconfirm = paras["KkConfirm_Result"].ToString();
                o.zytdconfirm = paras["ZytdConfirm_Result"].ToString();
                o.job_order = paras["JobOrder"].ToString();
                o.notice_order = paras["NoticeOrder"].ToString();
                o.missionname = db_miss.Miss_Desc;
                int UserId = (Session["User"] as EquipModel.Entities.Person_Info).Person_Id;
                PersonManagment pm = new PersonManagment();
                EquipBLL.AdminManagment.PersonManagment.P_viewModal pv = pm.Get_PersonModal(UserId);
                o.role = pv.Role_Names;
                Am.Add(o);
                i++;
                }
            string str = JsonConvert.SerializeObject(Am);
            return ("{" + "\"data\": " + str + "}");
            }

        public string getA14dot3dot3dcl_list(string WorkFlow_Name)
        {
            List<A14dot3dot3Model> Am = new List<A14dot3dot3Model>();
            List<UI_MISSION> miss;
            List<object> or = new List<object>();
            miss = CWFEngine.GetActiveMissions<Person_Info>(((IObjectContextAdapter)(new EquipWebContext())).ObjectContext, WorkFlow_Name, true);
            int i = 1;
            foreach (var item in miss)
            {
                A14dot3dot3Model o = new A14dot3dot3Model();
                Dictionary<string, object> paras = new Dictionary<string, object>();
                paras["Zz_Name"] = null;
                paras["Equip_GyCode"] = null;
                paras["Equip_Code"] = null;
                paras["Equip_Type"] = null;
                paras["Equip_ABCMark"] = null;
                paras["Plan_Name"] = null;
                paras["JxCauseDesc"] = null;

                paras["KkConfirm_Result"] = null;
                paras["ZytdConfirm_Result"] = null;
                paras["JobOrder"] = null;
                paras["NoticeOrder"] = null;
                WorkFlows wfsd = new WorkFlows();
                UI_WFEntity_Info wfei = CWFEngine.GetWorkFlowEntityWithParams(item.WE_Entity_Id, paras);
                Mission db_miss = wfsd.GetWFEntityMissions(item.WE_Entity_Id).Last();//获取该实体最后一个任务


                o.index_Id = item.WE_Entity_Id;
                o.zz_name = paras["Zz_Name"].ToString();
                o.sb_gycode = paras["Equip_GyCode"].ToString();
                o.sb_code = paras["Equip_Code"].ToString();
                o.sb_type = paras["Equip_Type"].ToString();
                o.sb_ABCMark = paras["Equip_ABCMark"].ToString();
                o.plan_name = paras["Plan_Name"].ToString();
                o.jxreason = paras["JxCauseDesc"].ToString();
                o.xcconfirm = "同意";
                o.kkconfirm1 = paras["KkConfirm_Result"].ToString();
                o.zytdconfirm1 = paras["ZytdConfirm_Result"].ToString();
                o.job_order1 = paras["JobOrder"].ToString();
                o.notice_order1 = paras["NoticeOrder"].ToString();
                o.missionname = db_miss.Miss_Desc;
                int UserId = (Session["User"] as EquipModel.Entities.Person_Info).Person_Id;
                PersonManagment pm = new PersonManagment();
                EquipBLL.AdminManagment.PersonManagment.P_viewModal pv = pm.Get_PersonModal(UserId);
                o.role = pv.Role_Names;

                Am.Add(o);
                i++;
            }
            string str = JsonConvert.SerializeObject(Am);
            return ("{" + "\"data\": " + str + "}");
        }




        #region DRBPM系统接口 -公共
        //数据库连接
        public static SqlConnection getDRBPMSqlConnection()
        {
            return new SqlConnection("Password=Wust123456;Persist Security Info=True;User ID=sa;Initial Catalog=DRBPMNew;Data Source=10.128.189.203");
            //投用时替换：Password=Wust123456;Persist Security Info=True;User ID=sa;Initial Catalog=DRBPMNew;Data Source=10.128.189.203
            //本地测试：Data Source=127.0.0.1;Initial Catalog=DRBPMNew;User ID=sa;Password=sa77858654
        }

        //计划检修原因解析
        public static string GetReason(string code)
        {
            string str = string.Empty;
            switch (code)
            {
                case "PM1":
                    str = "[PM1] 超期在役未修；正常运行";
                    break;
                case "PM2":
                    str = "[PM2] 故障强度等级较高设备，一级报警，运行趋势差，达到检修时间";
                    break;
                case "PM3":
                    str = "[PM3] 故障强度等级较高设备，二级报警";
                    break;
                case "PM4":
                    str = "[PM4] 二级报警，达到检修时间";
                    break;
                case "PM5":
                    str = "[PM5] 故障强度等级较高设备，运行趋势差，达到检修时间";
                    break;
                case "PM21":
                    str = "[PM2-1] 故障强度等级较高设备，一级报警，运行趋势差，达到检修时间；超期在役未修";
                    break;
                case "PM31":
                    str = "[PM3-1] 故障强度等级较高设备，二级报警；超期在役未修";
                    break;
                case "PM41":
                    str = "[PM4-1] 二级报警，达到检修时间；超期在役未修";
                    break;
                case "PM51":
                    str = "[PM5-1] 故障强度等级较高设备，运行趋势差，达到检修时间；超期在役未修";
                    break;
                case "PMx":
                    str = "[PMx] 人工筛选，预防性维修";
                    break;
                case "OP":
                    str = "[OP] 一般性关注";
                    break;
                case "OPx":
                    str = "[OPx] 人工筛选，一般性关注";
                    break;
                case "PS":
                    str = "[PS] 正常运行";
                    break;
                //2015.5.22
                case "PM6":
                    str = "[PM6] 密封泄漏级别达到检修要求";
                    break;
                case "PM6-U":
                    str = "[PM6-U] 不允许泄漏机泵渗漏、轻微泄漏或泄漏；或易燃易爆、烃类、轻质油、高温机泵泄漏";
                    break;
                case "PM6-1":
                    str = "[PM6-1] 易燃易爆、烃类、轻质油、高温机泵轻微泄漏";
                    break;
                case "PM6-2":
                    str = "[PM6-2] 一般性机泵泄漏";
                    break;
                case "PDM":
                    str = "[PDM] 预知性检修";
                    break;
                case "PMG":
                    str = "[PMG] 所监测的工艺量状态达到检修要求";
                    break;
                //default:
                //    str = code;
                //    break;
            }
            return str;
        }

        public static string GetReasonAll(string SbStateBak, string Temp6, string Temp3, int IsUrgentPM)
        {
            //2015.6.2 add +GetReasonPM6(sb.Temp6)
            string desc = string.Empty;

            if (!string.IsNullOrEmpty(SbStateBak))
            {
                if (SbStateBak == "PM6")
                {
                    desc = GetReasonPM6(Temp6, false);
                    if (IsUrgentPM >= 1)  //
                        desc = desc + " --[紧急!]  " + Temp3;
                }
                else
                {
                    //2015.6.5 修改显示规则
                    if (IsUrgentPM >= 1 && !Temp6.Contains("PM6-U"))  //加上干预理由
                        desc = GetReason(SbStateBak) + " --[紧急!]  " + Temp3;
                    else if (SbStateBak == "PMx" || SbStateBak == "OPx" || SbStateBak == "PDM")
                        desc = GetReason(SbStateBak) + " --[干预理由]  " + Temp3;
                    else
                        desc = GetReason(SbStateBak);

                    desc = desc + GetReasonPM6(Temp6, false);
                    if (IsUrgentPM >= 1 && Temp6.Contains("PM6-U"))  //加上干预理由
                        desc = desc + " --[紧急!]  " + Temp3;
                }
            }

            return desc;
        }

        //密封-PM6-说明
        public static string GetReasonPM6(string PM6Type, bool bAddBr = false)
        {
            string desc = string.Empty;
            if (!string.IsNullOrEmpty(PM6Type) && PM6Type.Contains("PM6"))
            {
                if (bAddBr)
                    desc = "\r\n" + GetReason(PM6Type);
                else
                    desc = GetReason(PM6Type);
            }
            return desc;
        }
        //工艺异常指标及说明（综合描述，分行显示）
        public static string GetGyStateDescription(string GyStatePMGList, bool bAddBr = false)
        {
            string desc = string.Empty;
            //顺序：FQmin,  FQavg,  TGavg,  LGmin,  DA,  EnergyEfficiency
            //if(sb.IsAddCorrectTabstrList.Substring(0,1)=="1")

            //依次判断PMG1-4，同时存在要分行显示  （相关量为FQavg,   EnergyEfficiency， LGmin） 
            if (GyStatePMGList.Contains("PMG1"))
                desc = "[PMG1] 流量低于设计允许范围，为额定流量的50%～70%，且工艺无法修正；";

            if (GyStatePMGList.Contains("PMG2"))
            {
                if (!string.IsNullOrEmpty(desc))
                    desc += "\r\n";  //Web显示时的换行符
                //出于兼容性和面向对象化的考虑，推荐使用System.Environment.NewLine作为换行符(？？？试过，不行）

                desc += "[PMG2] 流量低于设计允许范围，低于额定流量的50%，且工艺无法修正；";
            }

            if (GyStatePMGList.Contains("PMG3"))
            {
                if (!string.IsNullOrEmpty(desc))
                    desc += "\r\n";

                desc += "[PMG3] 设备能效低于设计效率75%；";
            }

            if (GyStatePMGList.Contains("PMG4"))
            {
                if (!string.IsNullOrEmpty(desc))
                    desc += "\r\n";

                desc += "[PMG4] 液位低于设计允许范围，工艺无法修正，且气蚀余量不满足要求；";
            }

            if (bAddBr && !string.IsNullOrEmpty(desc))
                desc = "\r\n" + desc;   //加一个换行

            return desc;

            //其它3个警示信息FQmin, TGavg,  LGmin
            //最低流量在允许范围外，由sb.IsFQminAlarm判定;
            //介质温度高于176℃（超工艺温度，核对设计条件），由sb.IsTGavgAlarm判定;
            //液位低，请核对汽蚀余量，是否抽空？由sb.IsNPSHokLG判定;

        }

        //短时趋势分析-shortTermTrend-说明
        public static string GetShortTermTrendDescription(string shortTermTrend, bool bAddBr = false)
        {
            string desc = string.Empty;
            if (string.IsNullOrEmpty(shortTermTrend))
                return desc;
            if (shortTermTrend.Length >= 2)
            {
                if (bAddBr)
                    desc = "\r\n" + "[PMT] 设备短时趋势明显恶化" + "[" + shortTermTrend + "值]";
                else
                    desc = "[PMT] 设备短时趋势明显恶化" + "[" + shortTermTrend + "值]";

            }
            else if (shortTermTrend.Length == 1)
            {
                if (bAddBr)
                    desc = "\r\n" + "[OPT] 设备短时趋势恶化" + "[" + shortTermTrend + "值],需及时关注并排查原因";
                else
                    desc = "[OPT] 设备短时趋势恶化" + "[" + shortTermTrend + "值],需及时关注并排查原因";
            }
            return desc;
        }

        /// <summary>
        /// 写DRBMP数据库，生成检修计划（相当于装置自动提报），后续在DRBPM系统由检修单位、机动处确认
        /// </summary>
        /// <param name="sbcode">设备编号</param>
        /// <param name="reason">理由，如："一级缺陷" 或 "二级缺陷" 等</param>
        /// <param name="isUrgent">是否生成紧急计划，true-是，false-否</param>
        public static void writeDRBPM(string sbcode, string reason, bool isUrgent)
        {
            SqlConnection con = getDRBPMSqlConnection();//连接DRBPM数据库
            if (con.State == ConnectionState.Closed)
                con.Open();

            //机动处同意在本月执行的检修计划(非紧急计划,A14dot1中用）
            //string strWhere = String.Format(" GybhId='{0}' and SUBSTRING(JxPlanDate,0,8)='{1}' ", sbcode, System.DateTime.Now.ToString("yyyy-MM"));
            string strWhere = String.Format(" GybhId='{0}' ", sbcode);
            string sqlcmd = String.Format("select * from SbStateFullAnalysis where  {0}   order by id asc ", strWhere);
            //"select id,ZzId,GybhId,SbName,JxPlanDate,sbStateAuto,sbStateBak,IsUrgentPM,jdcOpinion from SbStateFullAnalysisx"
            SqlDataAdapter da = new SqlDataAdapter(sqlcmd, con);
            DataTable dt = new DataTable();
            da.Fill(dt); //准备好查询到的数据到dt，供访问
            if (dt.Rows.Count == 1)
            {
                string vals = "";
                for (int i = 0; i <= 3; i++)
                {
                    vals += string.Format("'{0}',", dt.Rows[0][i].ToString().Trim());
                }
                if (isUrgent)
                    vals += string.Format("'{0}',", System.DateTime.Now.ToString("yyyy-MM-dd")); //JxPlanDate
                else
                    vals += string.Format("'{0}',", System.DateTime.Now.AddMonths(1).ToString("yyyy-MM-01")); //JxPlanDate

                vals += string.Format("'{0}',", dt.Rows[0]["sbStateAuto"].ToString());//sbStateAuto
                vals += string.Format("'{0}',", "PMx");//sbStateBak
                for (int i = 7; i <= 15; i++)
                {
                    vals += string.Format("'{0}',", dt.Rows[0][i].ToString());
                }

                if (isUrgent)
                    vals += string.Format("'{0}',", 2); //IsUrgentPM  2-人工干预的紧急计划
                else
                    vals += string.Format("'{0}',", dt.Rows[0]["IsUrgentPM"].ToString()); //IsUrgentPM


                vals += string.Format("'{0}',", dt.Rows[0]["LatestDate"].ToString());//LatestDate
                vals += string.Format("'{0}',", "同意");//zzOpinion
                vals += string.Format("'{0}',", "完整性管理系统生成");//zzReason
                for (int i = 20; i <= 23; i++)
                {
                    vals += string.Format("'{0}',", dt.Rows[0][i].ToString());
                }
                vals += string.Format("'{0}',", 1);//State
                vals += string.Format("'{0}',", System.DateTime.Now.ToString("yyyy-MM-dd"));//temp2 存放装置操作的日期
                vals += string.Format("'完整性管理系统{0}',", reason);//temp3 存放人工干预的理由
                for (int i = 27; i <= dt.Columns.Count - 2; i++)
                {
                    vals += string.Format("'{0}',", dt.Rows[0][i].ToString());
                }
                vals += string.Format("'{0}'", dt.Rows[0][dt.Columns.Count - 1].ToString());


                string insertSql = String.Format(" insert into SbStateFullAnalysisx (OrgId,ZzId,GybhId,SbName,JxPlanDate,sbStateAuto,sbStateBak,SbType,service_interval,FaultIntensity,AlarmState,AvgInterval,RunTrend_L,RunTrend_T,RunTrend_V,RunTrend,IsUrgentPM,LatestDate,zzOpinion,zzReason,jxOpinion,jxReason,jxKeyContent,jdcOpinion,State,temp2,temp3,temp4,temp5,temp6,temp7,temp8,temp9,temp10) values({0})", vals);
                SqlCommand cmd = new SqlCommand(insertSql, con); //定义一个sql操作命令对象           
                cmd.ExecuteNonQuery(); //执行语句  
            }

            con.Close();
            con.Dispose();
        }
        #endregion

        #region DRBPM系统接口 for A14dot1
        //A14dot1_ModelInfo
        public class A14dot1_ModelInfo
        {
            public string Cj_Name;
            public string Zz_Name;
            public string Equip_Code;
            public string Equip_GyCode;
            public string Equip_Type;
            public string Zy_Type;
            public string Zy_SubType;
            public string Equip_ABCMark;
            //from DRBPM
            public string drbpmRecordId;//唯一标志ID-在DRBPM表中
            public string Jx_Reason;//检修原因
            public string jxPlanDate;//计划检修时间
            public string faultIntensity; //故障强度
            public string alarmState;//报警状态
            public string runTrend;//运行趋势
            public string latestDate; //上次检修时间
            //from A8.2
            public string Job_Order = ""; //工单号
            public string Job_OrderState = ""; //工单号(当前计划)状态
            public string Plan_DescFilePath = ""; //检修方案	
            public string ZzConfirmPlan_Result = ""; //计划是否可实施,现场工程师确认是否可实施计划
            public string JxdwConfirmEnd_done = ""; //计划实施情况-是否完成	
            public string PlanFinishDate = ""; //完工时间
            public string JxSubmit_done = ""; //检修单位-是否提报（A8.2是否生成？）
        }
            //A14dot1_Model
            public class A14dot1_Model
            {
                public List<A14dot1_ModelInfo> A14dot1_ModelInfoList;
                public string isJx;//0712,zoujing
            }

            public A14dot1_Model getA14dot1_Model()
            {
                string equip_codes = "";
                SqlConnection con = getDRBPMSqlConnection();//连接DRBPM数据库
                if (con.State == ConnectionState.Closed)
                    con.Open();
                //string zzcode = "A03"; //
                //机动处同意在本月执行的检修计划(非紧急计划,A14dot1中用）
                //string strWhere = " sbStateBak like 'PM%'  and IsUrgentPM=0  and JdcOpinion = '同意' and SUBSTRING(JxPlanDate,0,8)='" + System.DateTime.Now.ToString("yyyy-MM") + "' " + " and ZzId='" + zzcode + "' ";
                string strWhere = " sbStateBak like 'PM%'  and JdcOpinion = '同意' and SUBSTRING(JxPlanDate,0,8)='" + System.DateTime.Now.ToString("yyyy-MM") + "' "; //and IsUrgentPM=0  
                string sqlcmd = String.Format("select * from SbStateFullAnalysisx where  {0}   order by id asc ", strWhere);
                //"select id,ZzId,GybhId,SbName,JxPlanDate,sbStateAuto,sbStateBak,IsUrgentPM,jdcOpinion from SbStateFullAnalysisx"
                SqlDataAdapter da = new SqlDataAdapter(sqlcmd, con);
                DataTable dt = new DataTable();
                da.Fill(dt); //准备好查询到的数据到dt，供访问

                A14dot1_Model A14dot1model = new A14dot1_Model();
                int UserId = (Session["User"] as EquipModel.Entities.Person_Info).Person_Id;
                PersonManagment pm = new PersonManagment();
                EquipBLL.AdminManagment.PersonManagment.P_viewModal pv = pm.Get_PersonModal(UserId);

                if (pv.Role_Names.Contains("检维修人员"))
                    A14dot1model.isJx = "true";
                A14dot1model.A14dot1_ModelInfoList = new List<A14dot1_ModelInfo>();
                int j = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    A14dot1_ModelInfo h = new A14dot1_ModelInfo();
                    h.Equip_Code = dt.Rows[i]["GybhId"].ToString(); //sbid
                    EquipManagment em = new EquipManagment();
                    Equip_Info eqinfo = em.getEquip_Info(h.Equip_Code);
                    if (eqinfo == null)
                        continue;

                    if (j > 0)
                        equip_codes += ",";

                    equip_codes += h.Equip_Code;
                    j += 1;

                    List<Equip_Archi> Equip_ZzBelong = em.getEquip_ZzBelong(eqinfo.Equip_Id);
                    h.Cj_Name = Equip_ZzBelong[1].EA_Name; //Cj_Name
                    h.Zz_Name = Equip_ZzBelong[0].EA_Name; //Zz_Name
                    h.Equip_GyCode = eqinfo.Equip_GyCode;
                    h.Equip_Type = eqinfo.Equip_Type;
                    h.Zy_Type = eqinfo.Equip_Specialty;
                    h.Zy_SubType = eqinfo.Equip_PhaseB;
                    h.Equip_ABCMark = eqinfo.Equip_ABCmark;

                    h.drbpmRecordId = dt.Rows[i]["Id"].ToString();//唯一标志ID-在DRBPM表中
                    string SbStateBak = dt.Rows[i]["sbStateBak"].ToString();
                    string Temp6 = dt.Rows[i]["Temp6"].ToString();
                    string Temp3 = dt.Rows[i]["Temp3"].ToString();
                    int IsUrgentPM = int.Parse(dt.Rows[i]["IsUrgentPM"].ToString());
                    string Temp5 = dt.Rows[i]["Temp5"].ToString();
                    string Temp9 = dt.Rows[i]["Temp9"].ToString();
                    h.Jx_Reason = GetReasonAll(SbStateBak, Temp6, Temp3, IsUrgentPM) + GetShortTermTrendDescription(Temp9, false);
                    //h.Jx_Reason = GetReasonAll(SbStateBak, Temp6, Temp3, IsUrgentPM) + GetGyStateDescription(Temp5, true) + GetShortTermTrendDescription(Temp9, true);
                    //h.Jx_Reason = getDRBPMJxReason(dt.Rows[i]["sbStateBak"].ToString()); // "一级缺陷"?  //计划检修原因 PM？ 
                    h.jxPlanDate = DateTime.Now.ToString("yyyy-MM");

                    h.faultIntensity = dt.Rows[i]["faultIntensity"].ToString(); //故障强度
                    h.alarmState = dt.Rows[i]["alarmState"].ToString();//报警状态
                    h.runTrend = dt.Rows[i]["runTrend"].ToString(); //运行趋势
                    h.latestDate = dt.Rows[i]["latestDate"].ToString(); //上次检修时间                              

                    A14dot1model.A14dot1_ModelInfoList.Add(h);

                }

                string query_list = " E.W_Name,M.Event_Name, P.Equip_Code, P.Job_Order, P.Job_OrderState, P.Plan_DescFilePath, P.ZzConfirmPlan_Result,P.JxdwConfirmEnd_done, R.time,P.JxSubmit_done";
                string query_condition = string.Format("P.Equip_Code in ({0}) and E.W_Name = 'A8dot2'", equip_codes);

                DateTime dtime = DateTime.Now;
                dtime = dtime.AddDays(-dtime.Day + 1);
                dtime = dtime.AddHours(-dtime.Hour);
                dtime = dtime.AddMinutes(-dtime.Minute);
                dtime = dtime.AddSeconds(-dtime.Second);
                string record_filter = string.Format("time >= '{0}' ", dtime.ToString()); //"1 <> 1";

                System.Data.DataTable dtA8dot2 = CWFEngine.QueryAllInformation(query_list, query_condition, record_filter);
                if (dtA8dot2 != null && dtA8dot2.Rows.Count > 0)
                {
                    for (int i = 0; i < A14dot1model.A14dot1_ModelInfoList.Count; i++)
                    {
                        string current_code = A14dot1model.A14dot1_ModelInfoList[i].Equip_Code;
                        //Job_Order
                        dtA8dot2.DefaultView.RowFilter = "Equip_Code = '" + current_code + "' and Job_Order is not null";
                        dtA8dot2.DefaultView.Sort = "time";
                        DataTable dtTmp = dtA8dot2.DefaultView.ToTable();
                        if (dtTmp.Rows.Count > 0)
                            A14dot1model.A14dot1_ModelInfoList[i].Job_Order = dtTmp.Rows[dtTmp.Rows.Count - 1]["Job_Order"].ToString();
                        dtTmp.Clear();

                        //Job_OrderState
                        dtA8dot2.DefaultView.RowFilter = "Equip_Code = '" + current_code + "' and Job_OrderState is not null";
                        dtA8dot2.DefaultView.Sort = "time";
                        dtTmp = dtA8dot2.DefaultView.ToTable();
                        if (dtTmp.Rows.Count > 0)
                            A14dot1model.A14dot1_ModelInfoList[i].Job_OrderState = dtTmp.Rows[dtTmp.Rows.Count - 1]["Job_OrderState"].ToString();
                        dtTmp.Clear();

                        //Plan_DescFilePath
                        dtA8dot2.DefaultView.RowFilter = "Equip_Code = '" + current_code + "' and Plan_DescFilePath is not null";
                        dtA8dot2.DefaultView.Sort = "time";
                        dtTmp = dtA8dot2.DefaultView.ToTable();
                        if (dtTmp.Rows.Count > 0)
                            A14dot1model.A14dot1_ModelInfoList[i].Plan_DescFilePath = dtTmp.Rows[dtTmp.Rows.Count - 1]["Plan_DescFilePath"].ToString();
                        dtTmp.Clear();

                        //ZzConfirmPlan_Result
                        dtA8dot2.DefaultView.RowFilter = "Equip_Code = '" + current_code + "' and ZzConfirmPlan_Result is not null";
                        dtA8dot2.DefaultView.Sort = "time";
                        dtTmp = dtA8dot2.DefaultView.ToTable();
                        if (dtTmp.Rows.Count > 0)
                            A14dot1model.A14dot1_ModelInfoList[i].ZzConfirmPlan_Result = dtTmp.Rows[dtTmp.Rows.Count - 1]["ZzConfirmPlan_Result"].ToString();
                        dtTmp.Clear();

                        //JxdwConfirmEnd_done
                        dtA8dot2.DefaultView.RowFilter = "JxdwConfirmEnd_done is not null";
                        dtA8dot2.DefaultView.Sort = "time";
                        dtTmp = dtA8dot2.DefaultView.ToTable();
                        if (dtTmp.Rows.Count > 0)
                            A14dot1model.A14dot1_ModelInfoList[i].JxdwConfirmEnd_done = dtTmp.Rows[dtTmp.Rows.Count - 1]["JxdwConfirmEnd_done"].ToString();
                        dtTmp.Clear();

                        //PlanFinishDate
                        dtA8dot2.DefaultView.RowFilter = "Equip_Code = '" + current_code + "' and JxdwConfirmEnd_done =true and time is not null";//?
                        dtA8dot2.DefaultView.Sort = "time";
                        dtTmp = dtA8dot2.DefaultView.ToTable();
                        if (dtTmp.Rows.Count > 0)
                            A14dot1model.A14dot1_ModelInfoList[i].PlanFinishDate = dtTmp.Rows[dtTmp.Rows.Count - 1]["time"].ToString();
                        dtTmp.Clear();

                        //JxSubmit_done
                        dtA8dot2.DefaultView.RowFilter = "Equip_Code = '" + current_code + "' and JxSubmit_done =true and time is not null";//?
                        dtA8dot2.DefaultView.Sort = "time";
                        dtTmp = dtA8dot2.DefaultView.ToTable();
                        if (dtTmp.Rows.Count > 0)
                            A14dot1model.A14dot1_ModelInfoList[i].JxSubmit_done = dtTmp.Rows[dtTmp.Rows.Count - 1]["JxSubmit_done"].ToString().ToLower();
                        dtTmp.Clear();

                    }

                }



                con.Close();
                con.Dispose();              
                return A14dot1model;
            }

        #endregion

        #region DRBPM系统接口 for A13dot2
            //A13dot2_ModelInfo
            public class A13dot2_ModelInfo
            {
                public string Cj_Name;
                public string Zz_Name;
                public string Equip_Code;
                public string Equip_GyCode;
                public string Equip_Type;
                public string Zy_Type;
                public string Zy_SubType;
                public string Equip_ABCMark;
                //from DRBPM
                public string drbpmRecordId;//唯一标志ID-在DRBPM表中
                public string Jx_Reason;//检修原因
                public string jxPlanDate;//计划检修时间
                public string faultIntensity; //故障强度
                public string alarmState;//报警状态
                public string runTrend;//运行趋势
                public string latestDate; //上次检修时间
                //from A8.2
                public string Job_Order = ""; //工单号
                public string Job_OrderState = ""; //工单号(当前计划)状态
                public string Plan_DescFilePath = ""; //检修方案	
                public string ZzConfirmPlan_Result = ""; //计划是否可实施,现场工程师确认是否可实施计划
                public string JxdwConfirmEnd_done = ""; //计划实施情况-是否完成	
                public string PlanFinishDate = ""; //完工时间
                public string JxSubmit_done = ""; //检修单位-是否提报（A8.2是否生成？）
            }

            //A13dot2_Model
            public class A13dot2_Model
            {
                public List<A13dot2_ModelInfo> A13dot2_ModelInfoList;
                public string isJx;//0712,邹晶，添加index页面按钮权限
            }

            public A13dot2_Model getA13dot2_Model()
            {  

                string equip_codes = "";
                SqlConnection con = getDRBPMSqlConnection();//连接DRBPM数据库
                if (con.State == ConnectionState.Closed)
                    con.Open();
                //string zzcode = "A03"; //
                //机动处同意在本月执行的紧急检修计划（A13dot2中用）
                //string strWhere = " IsUrgentPM>0  AND  JdcOpinion = '同意' AND  SUBSTRING(JxPlanDate,0,8)='" + System.DateTime.Now.ToString("yyyy-MM") + "' " + " and ZzId='" + zzcode + "' ";
                string strWhere = " IsUrgentPM>0  AND  JdcOpinion = '同意' AND  SUBSTRING(JxPlanDate,0,8)='" + System.DateTime.Now.ToString("yyyy-MM") + "' ";
                string sqlcmd = String.Format("select * from SbStateFullAnalysisx where  {0}   order by id asc ", strWhere);
                //"select id,ZzId,GybhId,SbName,JxPlanDate,sbStateAuto,sbStateBak,IsUrgentPM,jdcOpinion from SbStateFullAnalysisx"
                SqlDataAdapter da = new SqlDataAdapter(sqlcmd, con);
                DataTable dt = new DataTable();
                da.Fill(dt); //准备好查询到的数据到dt，供访问

                A13dot2_Model A13dot2model = new A13dot2_Model();
                int UserId = (Session["User"] as EquipModel.Entities.Person_Info).Person_Id;
                PersonManagment pm = new PersonManagment();
                EquipBLL.AdminManagment.PersonManagment.P_viewModal pv = pm.Get_PersonModal(UserId);

                if (pv.Role_Names.Contains("检维修人员"))
                    A13dot2model.isJx = "true";
                A13dot2model.A13dot2_ModelInfoList = new List<A13dot2_ModelInfo>();
                int j = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //if (i > 0)
                    //    equip_codes += ",";

                    A13dot2_ModelInfo h = new A13dot2_ModelInfo();
                    h.Equip_Code = dt.Rows[i]["GybhId"].ToString(); //sbid
                    EquipManagment em = new EquipManagment();
                    Equip_Info eqinfo = em.getEquip_Info(h.Equip_Code);
                    if (eqinfo == null)
                        continue;

                    if (j > 0)
                        equip_codes += ",";

                    equip_codes += h.Equip_Code;
                    j += 1;

                    List<Equip_Archi> Equip_ZzBelong = em.getEquip_ZzBelong(eqinfo.Equip_Id);
                    h.Cj_Name = Equip_ZzBelong[1].EA_Name; //Cj_Name
                    h.Zz_Name = Equip_ZzBelong[0].EA_Name; //Zz_Name
                    h.Equip_GyCode = eqinfo.Equip_GyCode;
                    h.Equip_Type = eqinfo.Equip_Type;
                    h.Zy_Type = eqinfo.Equip_Specialty;
                    h.Zy_SubType = eqinfo.Equip_PhaseB;
                    h.Equip_ABCMark = eqinfo.Equip_ABCmark;

                    h.drbpmRecordId = dt.Rows[i]["Id"].ToString();//唯一标志ID-在DRBPM表中
                    string SbStateBak = dt.Rows[i]["sbStateBak"].ToString();
                    string Temp6 = dt.Rows[i]["Temp6"].ToString();
                    string Temp3 = dt.Rows[i]["Temp3"].ToString();
                    int IsUrgentPM = int.Parse(dt.Rows[i]["IsUrgentPM"].ToString());
                    string Temp5 = dt.Rows[i]["Temp5"].ToString();
                    string Temp9 = dt.Rows[i]["Temp9"].ToString();
                    h.Jx_Reason = GetReasonAll(SbStateBak, Temp6, Temp3, IsUrgentPM) + GetShortTermTrendDescription(Temp9, false);
                    //h.Jx_Reason = GetReasonAll(SbStateBak, Temp6, Temp3, IsUrgentPM) + GetGyStateDescription(Temp5, true) + GetShortTermTrendDescription(Temp9, true);
                    //h.Jx_Reason = getDRBPMJxReason(dt.Rows[i]["sbStateBak"].ToString()); // "一级缺陷"?  //计划检修原因 PM？  
                    h.jxPlanDate = DateTime.Now.ToString("yyyy-MM");

                    h.faultIntensity = dt.Rows[i]["faultIntensity"].ToString(); //故障强度
                    h.alarmState = dt.Rows[i]["alarmState"].ToString();//报警状态
                    h.runTrend = dt.Rows[i]["runTrend"].ToString(); //运行趋势
                    h.latestDate = dt.Rows[i]["latestDate"].ToString(); //上次检修时间

                    A13dot2model.A13dot2_ModelInfoList.Add(h);

                }

                string query_list = " E.W_Name,M.Event_Name, P.Equip_Code, P.Job_Order, P.Job_OrderState, P.Plan_DescFilePath, P.ZzConfirmPlan_Result,P.JxdwConfirmEnd_done, R.time,P.JxSubmit_done";
                string query_condition = string.Format("P.Equip_Code in ({0}) and E.W_Name = 'A8dot2'", equip_codes);

                DateTime dtime = DateTime.Now;
                dtime = dtime.AddDays(-dtime.Day + 1);
                dtime = dtime.AddHours(-dtime.Hour);
                dtime = dtime.AddMinutes(-dtime.Minute);
                dtime = dtime.AddSeconds(-dtime.Second);
                string record_filter = string.Format("time >= '{0}' ", dtime.ToString()); //"1 <> 1";

                System.Data.DataTable dtA8dot2 = CWFEngine.QueryAllInformation(query_list, query_condition, record_filter);
                if (dtA8dot2 != null && dtA8dot2.Rows.Count > 0)
                {
                    for (int i = 0; i < A13dot2model.A13dot2_ModelInfoList.Count; i++)
                    {
                        string current_code = A13dot2model.A13dot2_ModelInfoList[i].Equip_Code;
                        //Job_Order
                        dtA8dot2.DefaultView.RowFilter = "Equip_Code = '" + current_code + "' and Job_Order is not null";
                        dtA8dot2.DefaultView.Sort = "time";
                        DataTable dtTmp = dtA8dot2.DefaultView.ToTable();
                        if (dtTmp.Rows.Count > 0)
                            A13dot2model.A13dot2_ModelInfoList[i].Job_Order = dtTmp.Rows[dtTmp.Rows.Count - 1]["Job_Order"].ToString();
                        dtTmp.Clear();

                        //Job_OrderState
                        dtA8dot2.DefaultView.RowFilter = "Equip_Code = '" + current_code + "' and Job_OrderState is not null";
                        dtA8dot2.DefaultView.Sort = "time";
                        dtTmp = dtA8dot2.DefaultView.ToTable();
                        if (dtTmp.Rows.Count > 0)
                            A13dot2model.A13dot2_ModelInfoList[i].Job_OrderState = dtTmp.Rows[dtTmp.Rows.Count - 1]["Job_OrderState"].ToString();
                        dtTmp.Clear();

                        //Plan_DescFilePath
                        dtA8dot2.DefaultView.RowFilter = "Equip_Code = '" + current_code + "' and Plan_DescFilePath is not null";
                        dtA8dot2.DefaultView.Sort = "time";
                        dtTmp = dtA8dot2.DefaultView.ToTable();
                        if (dtTmp.Rows.Count > 0)
                            A13dot2model.A13dot2_ModelInfoList[i].Plan_DescFilePath = dtTmp.Rows[dtTmp.Rows.Count - 1]["Plan_DescFilePath"].ToString();
                        dtTmp.Clear();

                        //ZzConfirmPlan_Result
                        dtA8dot2.DefaultView.RowFilter = "Equip_Code = '" + current_code + "' and ZzConfirmPlan_Result is not null";
                        dtA8dot2.DefaultView.Sort = "time";
                        dtTmp = dtA8dot2.DefaultView.ToTable();
                        if (dtTmp.Rows.Count > 0)
                            A13dot2model.A13dot2_ModelInfoList[i].ZzConfirmPlan_Result = dtTmp.Rows[dtTmp.Rows.Count - 1]["ZzConfirmPlan_Result"].ToString();
                        dtTmp.Clear();

                        //JxdwConfirmEnd_done
                        dtA8dot2.DefaultView.RowFilter = "Equip_Code = '" + current_code + "' and JxdwConfirmEnd_done is not null";
                        dtA8dot2.DefaultView.Sort = "time";
                        dtTmp = dtA8dot2.DefaultView.ToTable();
                        if (dtTmp.Rows.Count > 0)
                            A13dot2model.A13dot2_ModelInfoList[i].JxdwConfirmEnd_done = dtTmp.Rows[dtTmp.Rows.Count - 1]["JxdwConfirmEnd_done"].ToString();
                        dtTmp.Clear();

                        //PlanFinishDate
                        dtA8dot2.DefaultView.RowFilter = "Equip_Code = '" + current_code + "' and JxdwConfirmEnd_done =true and time is not null";//?
                        dtA8dot2.DefaultView.Sort = "time";
                        dtTmp = dtA8dot2.DefaultView.ToTable();
                        if (dtTmp.Rows.Count > 0)
                            A13dot2model.A13dot2_ModelInfoList[i].PlanFinishDate = dtTmp.Rows[dtTmp.Rows.Count - 1]["time"].ToString();
                        dtTmp.Clear();

                        //JxSubmit_done
                        dtA8dot2.DefaultView.RowFilter = "Equip_Code = '" + current_code + "' and JxSubmit_done =true and time is not null";//?
                        dtA8dot2.DefaultView.Sort = "time";
                        dtTmp = dtA8dot2.DefaultView.ToTable();
                        if (dtTmp.Rows.Count > 0)
                            A13dot2model.A13dot2_ModelInfoList[i].JxSubmit_done = dtTmp.Rows[dtTmp.Rows.Count - 1]["JxSubmit_done"].ToString().ToLower();
                        dtTmp.Clear();

                    }

                }

                con.Close();
                con.Dispose();

                return A13dot2model;
            }

            #endregion

        #region DRBPM系统接口 for A7dot1MxAlarm (美迅一二级报警）
            //A7dot1MxAlarm_ModelInfo
            public class A7dot1MxAlarm_ModelInfo
            {
                public string Cj_Name;
                public string Zz_Name;
                public string Equip_Code;
                public string Equip_GyCode;
                public string Equip_Type;
                public string Zy_Type;
                public string Zy_SubType;
                public string Equip_ABCMark;
                public string AlarmState;
                public string AlarmDate;

            }
            //A7dot1MxAlarm_Model
            public class A7dot1MxAlarm_Model
            {
                public List<A7dot1MxAlarm_ModelInfo> A7dot1MxAlarm_ModelInfoList;
            }

            public A7dot1MxAlarm_Model getA7dot1MxAlarm_Model()
            {
                SqlConnection con = getDRBPMSqlConnection();//连接DRBPM数据库
                if (con.State == ConnectionState.Closed)
                    con.Open();
                //string zzcode = "A03"; //投用时，改为操作者所能管辖的装置的代码，如果是多个，需要处理一下
                //一二级报警数据，DRBPM平台已处理
                //string strWhere = " (AlarmState = '1' or AlarmState = '2') and sbStateBak not like 'PM%' " + " and ZzId='" + zzcode + "' ";
                string strWhere = " (AlarmState = '1' or AlarmState = '2') and sbStateBak not like 'PM%' ";
                string sqlcmd = String.Format("select * from SbStateFullAnalysis where  {0}  order by id asc ", strWhere);
                //"select id,ZzId,GybhId,SbName,JxPlanDate,sbStateAuto,sbStateBak,IsUrgentPM,jdcOpinion from SbStateFullAnalysisx"
                SqlDataAdapter da = new SqlDataAdapter(sqlcmd, con);
                DataTable dt = new DataTable();
                da.Fill(dt); //准备好查询到的数据到dt，供访问

                A7dot1MxAlarm_Model A7dot1MxAlarmmodel = new A7dot1MxAlarm_Model();
                A7dot1MxAlarmmodel.A7dot1MxAlarm_ModelInfoList = new List<A7dot1MxAlarm_ModelInfo>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    A7dot1MxAlarm_ModelInfo h = new A7dot1MxAlarm_ModelInfo();
                    h.Equip_Code = dt.Rows[i]["GybhId"].ToString(); //sbid
                    EquipManagment em = new EquipManagment();
                    Equip_Info eqinfo = em.getEquip_Info(h.Equip_Code);
                    if (eqinfo == null)
                        continue;

                    List<Equip_Archi> Equip_ZzBelong = em.getEquip_ZzBelong(eqinfo.Equip_Id);
                    h.Cj_Name = Equip_ZzBelong[1].EA_Name; //Cj_Name
                    h.Zz_Name = Equip_ZzBelong[0].EA_Name; //Zz_Name
                    h.Equip_GyCode = eqinfo.Equip_GyCode;
                    h.Equip_Type = eqinfo.Equip_Type;
                    h.Zy_Type = eqinfo.Equip_Specialty;
                    h.Zy_SubType = eqinfo.Equip_PhaseB;
                    h.Equip_ABCMark = eqinfo.Equip_ABCmark;
                    h.AlarmState = dt.Rows[i]["AlarmState"].ToString(); //AlarmState 
                    h.AlarmDate = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"); //AlarmState       
                    A7dot1MxAlarmmodel.A7dot1MxAlarm_ModelInfoList.Add(h);

                }
                con.Close();
                con.Dispose();

                //for test:
                //h = new A7dot1MxAlarm_ModelInfo();            
                //h.Equip_Code = "210002277";
                //eqinfo = em.getEquip_Info(h.Equip_Code);
                //Equip_ZzBelong = em.getEquip_ZzBelong(eqinfo.Equip_Id);
                //h.Cj_Name = Equip_ZzBelong[1].EA_Name; //Cj_Name
                //h.Zz_Name = Equip_ZzBelong[0].EA_Name; //Zz_Name
                //h.Equip_GyCode = eqinfo.Equip_GyCode;
                //h.Equip_Type = eqinfo.Equip_Type;
                //h.Zy_Type = eqinfo.Equip_Specialty;
                //h.Zy_SubType = eqinfo.Equip_PhaseB;
                //h.Equip_ABCMark = eqinfo.Equip_ABCmark;
                //h.AlarmState = "2"; //AlarmState         
                //A7dot1MxAlarmmodel.A7dot1MxAlarm_ModelInfoList.Add(h);

                return A7dot1MxAlarmmodel;
            }
            #endregion

        #region DRBPM系统接口 for A7dot2 (工艺效能监察，能效低设备）
            //A7dot2_ModelInfo
            public class A7dot2_ModelInfo
            {
                public string Cj_Name;
                public string Zz_Name;
                public string Equip_Code;
                public string Equip_GyCode;
                public string Equip_Type;
                public string Zy_Type;
                public string Zy_SubType;
                public string Equip_ABCMark;
                public string gyState_PMGList;
                public string gyAnalysis_Date;
                public string gyAnalysis_EnValue;

            }
            //A7dot2_Model
            public class A7dot2_Model
            {
                public List<A7dot2_ModelInfo> A7dot2_ModelInfoList;
            }

            public A7dot2_Model getA7dot2_Model()
            {
                SqlConnection con = getDRBPMSqlConnection();//连接DRBPM数据库
                if (con.State == ConnectionState.Closed)
                    con.Open();
                //string zzcode = "A03"; //投用时，改为操作者所能管辖的装置的代码，如果是多个，需要处理一下
                //能效低PMG3，DRBPM平台已处理
                //string strWhere = " temp5 like '%PMG3%'  and sbStateBak not like 'PM%' " + " and ZzId='" + zzcode + "' ";
                string strWhere = " temp5 like '%PMG3%' and sbStateBak not like 'PM%' ";
                string sqlcmd = String.Format("select * from SbStateFullAnalysisx where  {0}  order by id asc ", strWhere);
                //"select id,ZzId,GybhId,SbName,JxPlanDate,sbStateAuto,sbStateBak,IsUrgentPM,jdcOpinion from SbStateFullAnalysisx"
                SqlDataAdapter da = new SqlDataAdapter(sqlcmd, con);
                DataTable dt = new DataTable();
                da.Fill(dt); //准备好查询到的数据到dt，供访问

                A7dot2_Model A7dot2model = new A7dot2_Model();
                A7dot2model.A7dot2_ModelInfoList = new List<A7dot2_ModelInfo>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    A7dot2_ModelInfo h = new A7dot2_ModelInfo();
                    h.Equip_Code = dt.Rows[i]["GybhId"].ToString(); //sbid
                    EquipManagment em = new EquipManagment();
                    Equip_Info eqinfo = em.getEquip_Info(h.Equip_Code);
                    if (eqinfo == null)
                        continue;

                    List<Equip_Archi> Equip_ZzBelong = em.getEquip_ZzBelong(eqinfo.Equip_Id);
                    h.Cj_Name = Equip_ZzBelong[1].EA_Name; //Cj_Name
                    h.Zz_Name = Equip_ZzBelong[0].EA_Name; //Zz_Name
                    h.Equip_GyCode = eqinfo.Equip_GyCode;
                    h.Equip_Type = eqinfo.Equip_Type;
                    h.Zy_Type = eqinfo.Equip_Specialty;
                    h.Zy_SubType = eqinfo.Equip_PhaseB;
                    h.Equip_ABCMark = eqinfo.Equip_ABCmark;
                    h.gyState_PMGList = GetGyStateDescription(dt.Rows[i]["gyState_PMGList"].ToString(),true); //gyState_PMGList
                    h.gyAnalysis_Date = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"); 
                    //h.gyAnalysis_EnValue=;
                    A7dot2model.A7dot2_ModelInfoList.Add(h);

                }
                con.Close();
                con.Dispose();

                //for test:
                //h = new A7dot2_ModelInfo();            
                //h.Equip_Code = "210394641";
                //eqinfo = em.getEquip_Info(h.Equip_Code);
                //Equip_ZzBelong = em.getEquip_ZzBelong(eqinfo.Equip_Id);
                //h.Cj_Name = Equip_ZzBelong[1].EA_Name; //Cj_Name
                //h.Zz_Name = Equip_ZzBelong[0].EA_Name; //Zz_Name
                //h.Equip_GyCode = eqinfo.Equip_GyCode;
                //h.Equip_Type = eqinfo.Equip_Type;
                //h.Zy_Type = eqinfo.Equip_Specialty;
                //h.Zy_SubType = eqinfo.Equip_PhaseB;
                //h.Equip_ABCMark = eqinfo.Equip_ABCmark;
                //h.gyState_PMGList = "PMG3"; //gyState_PMGList         
                //A7dot2model.A7dot2_ModelInfoList.Add(h);

                return A7dot2model;
            }
            #endregion

        #region DRBPM系统接口 for Query Table SbStateFullAnalysis(x) - 设备PM状态分析
            //DRBPM_SbStateFullAnalysisInfo
            public class DRBPM_SbStateFullAnalysisInfo
            {
                //equip basic info
                public string Pq_Name;
                public string Cj_Name;
                public string Zz_Name;
                public string Equip_Code;
                public string Equip_GyCode;
                public string Equip_Type;
                public string Zy_Type;
                public string Zy_SubType;
                public string Equip_ABCMark;
                //drbmp result
                public string jxPlanDate;
                public string sbStateAuto;
                public string sbStateBak;
                public string serviceinterval;
                public string faultIntensity; //故障强度
                public string alarmState;//报警状态
                public string avgInterval;
                public string runTrendL;
                public string runTrendT;
                public string runTrendV;
                public string runTrend;//运行趋势
                public int? isUrgentPM;
                public DateTime? latestDate;
                public string zzOpinion;
                public string zzReason;
                public string jxOpinion;
                public string jxReason;
                public string jxKeyContent;
                public string jdcOpinion;
                public int? state;
                public string temp2;
                public string temp3;
                public string temp4;
                public string temp5;
                public string temp6;
                public string temp7;
                public string temp8;
                public string temp9;
                public string temp10;
            }

            //DRBPM_SbStateFullAnalysisList
            public class DRBPM_SbStateFullAnalysisList
            {
                public List<DRBPM_SbStateFullAnalysisInfo> DRBPM_SbStateFullAnalysisInfoList;
            }

            /// <summary>
            /// 获取DRBPM设备状态分析结果列表
            /// </summary>
            /// <param name="strTableName">要查询的表名，如：SbStateFullAnalysis（每天自动分析结果） 或 SbStateFullAnalysisx（审批结束后的结果）</param>
            /// <param name="strWhere">查询条件，使用是要根据需要构造合适的条件语句</param>
            /// <returns></returns>
            public DRBPM_SbStateFullAnalysisList getDRBPM_SbStateFullAnalysisList(string strTableName, string strWhere)
            {
                SqlConnection con = getDRBPMSqlConnection();//连接DRBPM数据库
                if (con.State == ConnectionState.Closed)
                    con.Open();

                //string zzcode = "A03"; //投用时，改为操作者所能管辖的装置的代码，如果是多个，需要处理一下
                //机动处同意在本月执行的检修计划(非紧急计划,A14dot1中用）
                //string strWhere = " sbStateBak like 'PM%'  and IsUrgentPM=0  and JdcOpinion = '同意' and SUBSTRING(JxPlanDate,0,8)='" + System.DateTime.Now.ToString("yyyy-MM") + "' " + " and ZzId='" + zzcode + "' ";
                //string strWhere = " sbStateBak like 'PM%'  and IsUrgentPM=0  and JdcOpinion = '同意' and SUBSTRING(JxPlanDate,0,8)='" + System.DateTime.Now.ToString("yyyy-MM") + "' ";
                string sqlcmd = String.Format("select * from {0} where  {1}   order by id asc ", strTableName, strWhere);
                //"select id,ZzId,GybhId,SbName,JxPlanDate,sbStateAuto,sbStateBak,IsUrgentPM,jdcOpinion from SbStateFullAnalysisx"
                SqlDataAdapter da = new SqlDataAdapter(sqlcmd, con);
                DataTable dt = new DataTable();
                da.Fill(dt); //准备好查询到的数据到dt，供访问

                DRBPM_SbStateFullAnalysisList drbmpSFAlist = new DRBPM_SbStateFullAnalysisList();
                //drbmpSFAlist.DRBPM_SbStateFullAnalysisInfoList = new List<DRBPM_SbStateFullAnalysisInfo>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DRBPM_SbStateFullAnalysisInfo h = new DRBPM_SbStateFullAnalysisInfo();
                    h.Equip_Code = dt.Rows[i]["GybhId"].ToString(); //sbid
                    EquipManagment em = new EquipManagment();
                    Equip_Info eqinfo = em.getEquip_Info(h.Equip_Code);
                    if (eqinfo == null)
                        continue;
                    //Equip info
                    List<Equip_Archi> Equip_ZzBelong = em.getEquip_ZzBelong(eqinfo.Equip_Id);
                    h.Cj_Name = Equip_ZzBelong[1].EA_Name; //Cj_Name
                    h.Zz_Name = Equip_ZzBelong[0].EA_Name; //Zz_Name
                    h.Equip_GyCode = eqinfo.Equip_GyCode;
                    h.Equip_Type = eqinfo.Equip_Type;
                    h.Zy_Type = eqinfo.Equip_Specialty;
                    h.Zy_SubType = eqinfo.Equip_PhaseB;
                    h.Equip_ABCMark = eqinfo.Equip_ABCmark;
                    //DRBPM result
                    h.jxPlanDate = dt.Rows[i]["jxPlanDate"].ToString();
                    h.sbStateAuto = dt.Rows[i]["sbStateAuto"].ToString();
                    h.sbStateBak = dt.Rows[i]["sbStateBak"].ToString();
                    h.serviceinterval = dt.Rows[i]["serviceinterval"].ToString();
                    h.faultIntensity = dt.Rows[i]["faultIntensity"].ToString();  //故障强度
                    h.alarmState = dt.Rows[i]["alarmState"].ToString(); //报警状态
                    h.avgInterval = dt.Rows[i]["avgInterval"].ToString();
                    h.runTrendL = dt.Rows[i]["runTrendL"].ToString();
                    h.runTrendT = dt.Rows[i]["runTrendT"].ToString();
                    h.runTrendV = dt.Rows[i]["runTrendV"].ToString();
                    h.runTrend = dt.Rows[i]["runTrend"].ToString(); //运行趋势
                    h.isUrgentPM = int.Parse(dt.Rows[i]["isUrgentPM"].ToString());
                    h.latestDate = DateTime.Parse(dt.Rows[i]["latestDate"].ToString());
                    h.zzOpinion = dt.Rows[i]["zzOpinion"].ToString();
                    h.zzReason = dt.Rows[i]["zzReason"].ToString();
                    h.jxOpinion = dt.Rows[i]["jxOpinion"].ToString();
                    h.jxReason = dt.Rows[i]["jxReason"].ToString();
                    h.jxKeyContent = dt.Rows[i]["jxKeyContent"].ToString();
                    h.jdcOpinion = dt.Rows[i]["jdcOpinion"].ToString();
                    h.state = int.Parse(dt.Rows[i]["state"].ToString());
                    h.temp2 = dt.Rows[i]["temp2"].ToString();
                    h.temp3 = dt.Rows[i]["temp3"].ToString();
                    h.temp4 = dt.Rows[i]["temp4"].ToString();
                    h.temp5 = dt.Rows[i]["temp5"].ToString();
                    h.temp6 = dt.Rows[i]["temp6"].ToString();
                    h.temp7 = dt.Rows[i]["temp7"].ToString();
                    h.temp8 = dt.Rows[i]["temp8"].ToString();
                    h.temp9 = dt.Rows[i]["temp9"].ToString();
                    h.temp10 = dt.Rows[i]["temp10"].ToString();
                    //add to List
                    drbmpSFAlist.DRBPM_SbStateFullAnalysisInfoList.Add(h);
                }
                con.Close();
                con.Dispose();

                return drbmpSFAlist;
            }

            #endregion
           
        #region DRBPM系统接口 for Query Table SbGyAnalysis -工艺能效分析
            //DRBPM_SbGyAnalysisInfo
            public class DRBPM_SbGyAnalysisInfo
            {
                //equip basic info
                public string Pq_Name;
                public string Cj_Name;
                public string Zz_Name;
                public string Equip_Code;
                public string Equip_GyCode;
                public string Equip_Type;
                public string Zy_Type;
                public string Zy_SubType;
                public string Equip_ABCMark;
                public string sbWh;
                //drbmp result         
                public Single? energyEfficiency;
                public string gyState_PMGList;
                public string gyState_PMGList_Desc;
                public string pState;
                public string currentDate;

            }

            //DRBPM_SbGyAnalysisList
            public class DRBPM_SbGyAnalysisList
            {
                public List<DRBPM_SbGyAnalysisInfo> DRBPM_SbGyAnalysisInfoList;
            }

            /// <summary>
            /// 获取DRBPM设备工艺能效分析结果列表
            /// </summary>
            /// <param name="strTableName">要查询的表名，如：SbGyAnalysis（每天自动分析结果）</param>
            /// <param name="strWhere">查询条件，使用是要根据需要构造合适的条件语句</param>
            /// <returns></returns>
            public DRBPM_SbGyAnalysisList getDRBPM_SbGyAnalysisList(string strTableName = "SbGyAnalysis", string strWhere = "1=1")
            {
                SqlConnection con = getDRBPMSqlConnection();//连接DRBPM数据库
                if (con.State == ConnectionState.Closed)
                    con.Open();

                DateTime dtime = DateTime.Now;
                //dtime = dtime.AddDays(-1);
                dtime = dtime.AddHours(-dtime.Hour);
                dtime = dtime.AddMinutes(-dtime.Minute);
                dtime = dtime.AddSeconds(-dtime.Second);
                string record_filter = string.Format("currentDate >= '{0}' ", dtime.ToString()); //"1 <> 1";
                strWhere = strWhere + " and " + record_filter;
                string sqlcmd = String.Format("select * from {0} where  {1}   order by id asc ", strTableName, strWhere);
                SqlDataAdapter da = new SqlDataAdapter(sqlcmd, con);
                DataTable dt = new DataTable();
                da.Fill(dt); //准备好查询到的数据到dt，供访问

                DRBPM_SbGyAnalysisList drbmpGyAlist = new DRBPM_SbGyAnalysisList();
                drbmpGyAlist.DRBPM_SbGyAnalysisInfoList = new List<DRBPM_SbGyAnalysisInfo>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DRBPM_SbGyAnalysisInfo h = new DRBPM_SbGyAnalysisInfo();
                    h.Equip_Code = dt.Rows[i]["sbId"].ToString(); //sbId
                    EquipManagment em = new EquipManagment();
                    Equip_Info eqinfo = em.getEquip_Info(h.Equip_Code);
                    if (eqinfo == null)
                        continue;
                    //Equip info
                    List<Equip_Archi> Equip_ZzBelong = em.getEquip_ZzBelong(eqinfo.Equip_Id);
                    h.Cj_Name = Equip_ZzBelong[1].EA_Name; //Cj_Name
                    h.Zz_Name = Equip_ZzBelong[0].EA_Name; //Zz_Name
                    h.Equip_GyCode = eqinfo.Equip_GyCode;
                    h.Equip_Type = eqinfo.Equip_Type;
                    h.Zy_Type = eqinfo.Equip_Specialty;
                    h.Zy_SubType = eqinfo.Equip_PhaseB;
                    h.Equip_ABCMark = eqinfo.Equip_ABCmark;
                    h.sbWh = dt.Rows[i]["sbWh"].ToString();
                    //drbmp result         
                    h.energyEfficiency = Single.Parse(dt.Rows[i]["energyEfficiency"].ToString());
                    h.gyState_PMGList=dt.Rows[i]["gyState_PMGList"].ToString();
                    h.gyState_PMGList_Desc = GetGyStateDescription(dt.Rows[i]["gyState_PMGList"].ToString(), true); 
                    h.pState = dt.Rows[i]["pState"].ToString();
                    h.currentDate = DateTime.Parse(dt.Rows[i]["currentDate"].ToString()).ToString("yyyy-MM-dd");
                    //add to List
                    drbmpGyAlist.DRBPM_SbGyAnalysisInfoList.Add(h);
                }
                con.Close();
                con.Dispose();

                return drbmpGyAlist;
            }

            #endregion

            #region DRBPM系统接口 for A14dot3dot4
            //A14dot3dot4_ModelInfo
            public class A14dot3dot4_ModelInfo
            {
                public string Cj_Name;
                public string Zz_Name;
                public string Equip_Code;
                public string Equip_GyCode;
                public string Equip_Type;
                public string Zy_Type;
                public string Zy_SubType;
                public string Equip_ABCMark;
                //from DRBPM
                public string drbpmRecordId;//唯一标志ID-在DRBPM表中
                public string Jx_Reason;//检修原因
                public string jxPlanDate;//计划检修时间
                public string faultIntensity; //故障强度
                public string alarmState;//报警状态
                public string runTrend;//运行趋势
                public string latestDate; //上次检修时间
                //from A8.2
                public string Job_Order = ""; //工单号
                public string Job_OrderState = ""; //工单号(当前计划)状态
                public string Plan_DescFilePath = ""; //检修方案	
                public string ZzConfirmPlan_Result = ""; //计划是否可实施,现场工程师确认是否可实施计划
                public string JxdwConfirmEnd_done = ""; //计划实施情况-是否完成	
                public string PlanFinishDate = ""; //完工时间
                public string JxSubmit_done = ""; //检修单位-是否提报（A8.2是否生成？）
            }
            //A14dot3dot4_Model
            public class A14dot3dot4_Model
            {
                public List<A14dot3dot4_ModelInfo> A14dot3dot4_ModelInfoList;
                public string isJx;//0712,zoujing
            }

            public A14dot3dot4_Model getA14dot3dot4_Model()
            {
                string equip_codes = "";
                SqlConnection con = getDRBPMSqlConnection();//连接DRBPM数据库
                if (con.State == ConnectionState.Closed)
                    con.Open();
                //string zzcode = "A03"; //
                //机动处同意在本月执行的检修计划(非紧急计划,A14dot3dot4中用）
                //string strWhere = " sbStateBak like 'PM%'  and IsUrgentPM=0  and JdcOpinion = '同意' and SUBSTRING(JxPlanDate,0,8)='" + System.DateTime.Now.ToString("yyyy-MM") + "' " + " and ZzId='" + zzcode + "' ";
                string strWhere = " sbStateBak like 'PM%'  and JdcOpinion = '同意' and SUBSTRING(JxPlanDate,0,8)='" + System.DateTime.Now.ToString("yyyy-MM") + "' "; //and IsUrgentPM=0  
                string sqlcmd = String.Format("select * from SbStateFullAnalysisx where  {0}   order by id asc ", strWhere);
                //"select id,ZzId,GybhId,SbName,JxPlanDate,sbStateAuto,sbStateBak,IsUrgentPM,jdcOpinion from SbStateFullAnalysisx"
                SqlDataAdapter da = new SqlDataAdapter(sqlcmd, con);
                DataTable dt = new DataTable();
                da.Fill(dt); //准备好查询到的数据到dt，供访问

                A14dot3dot4_Model A14dot3dot4model = new A14dot3dot4_Model();
                int UserId = (Session["User"] as EquipModel.Entities.Person_Info).Person_Id;
                PersonManagment pm = new PersonManagment();
                EquipBLL.AdminManagment.PersonManagment.P_viewModal pv = pm.Get_PersonModal(UserId);

                if (pv.Role_Names.Contains("检维修人员"))
                    A14dot3dot4model.isJx = "true";
                A14dot3dot4model.A14dot3dot4_ModelInfoList = new List<A14dot3dot4_ModelInfo>();
                int j = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    A14dot3dot4_ModelInfo h = new A14dot3dot4_ModelInfo();
                    h.Equip_Code = dt.Rows[i]["GybhId"].ToString(); //sbid
                    EquipManagment em = new EquipManagment();
                    Equip_Info eqinfo = em.getEquip_Info(h.Equip_Code);
                    if (eqinfo == null)
                        continue;

                    if (j > 0)
                        equip_codes += ",";

                    equip_codes += h.Equip_Code;
                    j += 1;

                    List<Equip_Archi> Equip_ZzBelong = em.getEquip_ZzBelong(eqinfo.Equip_Id);
                    h.Cj_Name = Equip_ZzBelong[1].EA_Name; //Cj_Name
                    h.Zz_Name = Equip_ZzBelong[0].EA_Name; //Zz_Name
                    h.Equip_GyCode = eqinfo.Equip_GyCode;
                    h.Equip_Type = eqinfo.Equip_Type;
                    h.Zy_Type = eqinfo.Equip_Specialty;
                    h.Zy_SubType = eqinfo.Equip_PhaseB;
                    h.Equip_ABCMark = eqinfo.Equip_ABCmark;

                    h.drbpmRecordId = dt.Rows[i]["Id"].ToString();//唯一标志ID-在DRBPM表中
                    string SbStateBak = dt.Rows[i]["sbStateBak"].ToString();
                    string Temp6 = dt.Rows[i]["Temp6"].ToString();
                    string Temp3 = dt.Rows[i]["Temp3"].ToString();
                    int IsUrgentPM = int.Parse(dt.Rows[i]["IsUrgentPM"].ToString());
                    string Temp5 = dt.Rows[i]["Temp5"].ToString();
                    string Temp9 = dt.Rows[i]["Temp9"].ToString();
                    h.Jx_Reason = GetReasonAll(SbStateBak, Temp6, Temp3, IsUrgentPM) + GetShortTermTrendDescription(Temp9, false);
                    //h.Jx_Reason = GetReasonAll(SbStateBak, Temp6, Temp3, IsUrgentPM) + GetGyStateDescription(Temp5, true) + GetShortTermTrendDescription(Temp9, true);
                    //h.Jx_Reason = getDRBPMJxReason(dt.Rows[i]["sbStateBak"].ToString()); // "一级缺陷"?  //计划检修原因 PM？ 
                    h.jxPlanDate = DateTime.Now.ToString("yyyy-MM");

                    h.faultIntensity = dt.Rows[i]["faultIntensity"].ToString(); //故障强度
                    h.alarmState = dt.Rows[i]["alarmState"].ToString();//报警状态
                    h.runTrend = dt.Rows[i]["runTrend"].ToString(); //运行趋势
                    h.latestDate = dt.Rows[i]["latestDate"].ToString(); //上次检修时间                              

                    A14dot3dot4model.A14dot3dot4_ModelInfoList.Add(h);

                }

                string query_list = " E.W_Name,M.Event_Name, P.Equip_Code, P.Job_Order, P.Job_OrderState, P.Plan_DescFilePath, P.ZzConfirmPlan_Result,P.JxdwConfirmEnd_done, R.time,P.JxSubmit_done";
                string query_condition = string.Format("P.Equip_Code in ({0}) and E.W_Name = 'A8dot2'", equip_codes);

                DateTime dtime = DateTime.Now;
                dtime = dtime.AddDays(-dtime.Day + 1);
                dtime = dtime.AddHours(-dtime.Hour);
                dtime = dtime.AddMinutes(-dtime.Minute);
                dtime = dtime.AddSeconds(-dtime.Second);
                string record_filter = string.Format("time >= '{0}' ", dtime.ToString()); //"1 <> 1";

                System.Data.DataTable dtA8dot2 = CWFEngine.QueryAllInformation(query_list, query_condition, record_filter);
                if (dtA8dot2 != null && dtA8dot2.Rows.Count > 0)
                {
                    for (int i = 0; i < A14dot3dot4model.A14dot3dot4_ModelInfoList.Count; i++)
                    {
                        string current_code = A14dot3dot4model.A14dot3dot4_ModelInfoList[i].Equip_Code;
                        //Job_Order
                        dtA8dot2.DefaultView.RowFilter = "Equip_Code = '" + current_code + "' and Job_Order is not null";
                        dtA8dot2.DefaultView.Sort = "time";
                        DataTable dtTmp = dtA8dot2.DefaultView.ToTable();
                        if (dtTmp.Rows.Count > 0)
                            A14dot3dot4model.A14dot3dot4_ModelInfoList[i].Job_Order = dtTmp.Rows[dtTmp.Rows.Count - 1]["Job_Order"].ToString();
                        dtTmp.Clear();

                        //Job_OrderState
                        dtA8dot2.DefaultView.RowFilter = "Equip_Code = '" + current_code + "' and Job_OrderState is not null";
                        dtA8dot2.DefaultView.Sort = "time";
                        dtTmp = dtA8dot2.DefaultView.ToTable();
                        if (dtTmp.Rows.Count > 0)
                            A14dot3dot4model.A14dot3dot4_ModelInfoList[i].Job_OrderState = dtTmp.Rows[dtTmp.Rows.Count - 1]["Job_OrderState"].ToString();
                        dtTmp.Clear();

                        //Plan_DescFilePath
                        dtA8dot2.DefaultView.RowFilter = "Equip_Code = '" + current_code + "' and Plan_DescFilePath is not null";
                        dtA8dot2.DefaultView.Sort = "time";
                        dtTmp = dtA8dot2.DefaultView.ToTable();
                        if (dtTmp.Rows.Count > 0)
                            A14dot3dot4model.A14dot3dot4_ModelInfoList[i].Plan_DescFilePath = dtTmp.Rows[dtTmp.Rows.Count - 1]["Plan_DescFilePath"].ToString();
                        dtTmp.Clear();

                        //ZzConfirmPlan_Result
                        dtA8dot2.DefaultView.RowFilter = "Equip_Code = '" + current_code + "' and ZzConfirmPlan_Result is not null";
                        dtA8dot2.DefaultView.Sort = "time";
                        dtTmp = dtA8dot2.DefaultView.ToTable();
                        if (dtTmp.Rows.Count > 0)
                            A14dot3dot4model.A14dot3dot4_ModelInfoList[i].ZzConfirmPlan_Result = dtTmp.Rows[dtTmp.Rows.Count - 1]["ZzConfirmPlan_Result"].ToString();
                        dtTmp.Clear();

                        //JxdwConfirmEnd_done
                        dtA8dot2.DefaultView.RowFilter = "JxdwConfirmEnd_done is not null";
                        dtA8dot2.DefaultView.Sort = "time";
                        dtTmp = dtA8dot2.DefaultView.ToTable();
                        if (dtTmp.Rows.Count > 0)
                            A14dot3dot4model.A14dot3dot4_ModelInfoList[i].JxdwConfirmEnd_done = dtTmp.Rows[dtTmp.Rows.Count - 1]["JxdwConfirmEnd_done"].ToString();
                        dtTmp.Clear();

                        //PlanFinishDate
                        dtA8dot2.DefaultView.RowFilter = "Equip_Code = '" + current_code + "' and JxdwConfirmEnd_done =true and time is not null";//?
                        dtA8dot2.DefaultView.Sort = "time";
                        dtTmp = dtA8dot2.DefaultView.ToTable();
                        if (dtTmp.Rows.Count > 0)
                            A14dot3dot4model.A14dot3dot4_ModelInfoList[i].PlanFinishDate = dtTmp.Rows[dtTmp.Rows.Count - 1]["time"].ToString();
                        dtTmp.Clear();

                        //JxSubmit_done
                        dtA8dot2.DefaultView.RowFilter = "Equip_Code = '" + current_code + "' and JxSubmit_done =true and time is not null";//?
                        dtA8dot2.DefaultView.Sort = "time";
                        dtTmp = dtA8dot2.DefaultView.ToTable();
                        if (dtTmp.Rows.Count > 0)
                            A14dot3dot4model.A14dot3dot4_ModelInfoList[i].JxSubmit_done = dtTmp.Rows[dtTmp.Rows.Count - 1]["JxSubmit_done"].ToString().ToLower();
                        dtTmp.Clear();

                    }

                }



                con.Close();
                con.Dispose();
                return A14dot3dot4model;
            }

            #endregion
        }
  }

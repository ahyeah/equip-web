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
using FlowEngine.Param;
using System.Data;

namespace WebApp.Controllers
{
    public class A11dot3Controller : CommonController
    {
        //
        // GET: /A11dot3/
        public ActionResult Index()
        {
            return View(getIndexListRecords("A11dot3", (Session["User"] as EquipModel.Entities.Person_Info).Person_Name));    
        }

        public ActionResult CjFollow(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }
        public ActionResult ZytdAdvise(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }
        public ActionResult UpToDirector(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }
        public ActionResult ImplementPlan(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }
        public ActionResult WaitStop(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }

        public ActionResult Submit(string wfe_id)
        {
            return View(getSubmitModel(wfe_id));
        }

        public ActionResult JdcConfirm(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }
        public ActionResult RiskAccept(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }

        public ActionResult CreateGroup(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }

        public ActionResult RiskRegister(string wfe_id)
        {
            return View(getRecordforRiskRegister(wfe_id));
        }
        //工作流详情
        public ActionResult WorkFolw_Detail(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }
        //******************风险登记表取数据******************************//
        public class RiskRegisterModel
        {
            public string zz;//装置  
            public string sbGycode;//设备位号
            public string sbCode;//设备编号
            public string problemDesc;//风险问题描述
            public string RiskRecognitionResult;//危害识别结果            
            public string PlanName;//管控措施文件名
            public string PlanPath;//管控措施文件路径
            public string PlanDesc;//管控措施描述
            public string RiskIsAccept;//风险是否可接受
            public string CjFollowResult="通过";//车间定期跟踪评估结果
            public string YDorTG_Result;
            public string WF_Ser;//工作流串号
            public string WF_Id;//工作流实体号
        }
        public RiskRegisterModel getRecordforRiskRegister(string wfe_id)
        {
            RiskRegisterModel record = new RiskRegisterModel();
            
            WorkFlows wfsd = new WorkFlows();
            WorkFlow_Entity wfe = wfsd.GetWorkFlowEntity(Convert.ToInt32(wfe_id));
            List<WorkFlow_Entity> getentity = wfsd.GetWorkFlowEntitiesbySer(wfe.WE_Ser);//通过实体号得到串号，通过串号在得到11.1或11.2的实体号及参数
            record.WF_Ser = wfe.WE_Ser;
            record.WF_Id = wfe_id;
            foreach (var item in getentity)
            {
                
                WorkFlow_Define getentity_name = wfsd.GetWorkFlowDefine(item.WE_Id);//通过每个实体号找到该实体号的名字，就可以确定每个实体号的内容
                if (getentity_name.W_Name == "A11dot2")
                {
                    List<Mission> db_miss = wfsd.GetWFEntityMissions(item.WE_Id);
                    foreach(var atc in db_miss)
                    {
                        if (atc.Miss_Desc == "现场工程师提报隐患")
                        {
                            UI_MISSION ui_mi = new UI_MISSION();
                            List<Mission_Param> mis_pars1 = wfsd.GetMissParams(atc.Miss_Id);//获取当前任务参数
                            foreach (var par in mis_pars1)
                            {
                                CParam cp = new CParam();
                                cp.type = par.Param_Type;
                                cp.name = par.Param_Name;
                                cp.value = par.Param_Value;
                                cp.description = par.Param_Desc;
                                ui_mi.Miss_Params[cp.name] = cp.value;
                                // ui_mi.Miss_ParamsAppRes[cp.name] = wf.paramstable[cp.name].linkEventsApp_res[ui_mi.WE_Event_Name];
                                ui_mi.Miss_ParamsDesc[cp.name] = cp.description;//xwm modified
                            }
                            record.zz = ui_mi.Miss_Params["Zz_Name"].ToString();
                            record.sbGycode = ui_mi.Miss_Params["Equip_GyCode"].ToString();
                            record.sbCode = ui_mi.Miss_Params["Equip_Code"].ToString();
                            record.problemDesc = ui_mi.Miss_Params["Problem_Desc"].ToString();
                        }
                        if (atc.Miss_Desc == "可靠性工程师风险矩阵评估")
                        {
                            UI_MISSION ui_mi = new UI_MISSION();
                            List<Mission_Param> mis_pars1 = wfsd.GetMissParams(atc.Miss_Id);//获取当前任务参数
                            foreach (var par in mis_pars1)
                            {
                                CParam cp = new CParam();
                                cp.type = par.Param_Type;
                                cp.name = par.Param_Name;
                                cp.value = par.Param_Value;
                                cp.description = par.Param_Desc;
                                ui_mi.Miss_Params[cp.name] = cp.value;
                                // ui_mi.Miss_ParamsAppRes[cp.name] = wf.paramstable[cp.name].linkEventsApp_res[ui_mi.WE_Event_Name];
                                ui_mi.Miss_ParamsDesc[cp.name] = cp.description;//xwm modified
                            }
                            if (ui_mi.Miss_Params["RiskMatrix_Color"].ToString() == "yellow")
                            {
                                record.RiskRecognitionResult = "一般风险";
                            }
                            if (ui_mi.Miss_Params["RiskMatrix_Color"].ToString() == "red")
                            {
                                record.RiskRecognitionResult = "重大风险";
                            }
                        }
                        if (atc.Miss_Desc == "专业团队确立管控措施")
                        {
                            UI_MISSION ui_mi = new UI_MISSION();
                            List<Mission_Param> mis_pars1 = wfsd.GetMissParams(atc.Miss_Id);//获取当前任务参数
                            foreach (var par in mis_pars1)
                            {
                                CParam cp = new CParam();
                                cp.type = par.Param_Type;
                                cp.name = par.Param_Name;
                                cp.value = par.Param_Value;
                                cp.description = par.Param_Desc;
                                ui_mi.Miss_Params[cp.name] = cp.value;
                                // ui_mi.Miss_ParamsAppRes[cp.name] = wf.paramstable[cp.name].linkEventsApp_res[ui_mi.WE_Event_Name];
                                ui_mi.Miss_ParamsDesc[cp.name] = cp.description;//xwm modified
                            }
                            if (ui_mi.Miss_Params["Plan_DescFilePath"].ToString() != "")
                            {
                                string[] PlanFile = ui_mi.Miss_Params["Plan_DescFilePath"].ToString().Split(new char[] { '$' });
                                record.PlanName = PlanFile[0];
                                record.PlanPath = PlanFile[1];
                            }
                            record.PlanDesc = ui_mi.Miss_Params["Plan_Desc"].ToString();

                        }
                        if (atc.Miss_Desc == "车间确立管控措施")
                        {
                            UI_MISSION ui_mi = new UI_MISSION();
                            List<Mission_Param> mis_pars1 = wfsd.GetMissParams(atc.Miss_Id);//获取当前任务参数
                            foreach (var par in mis_pars1)
                            {
                                CParam cp = new CParam();
                                cp.type = par.Param_Type;
                                cp.name = par.Param_Name;
                                cp.value = par.Param_Value;
                                cp.description = par.Param_Desc;
                                ui_mi.Miss_Params[cp.name] = cp.value;
                                // ui_mi.Miss_ParamsAppRes[cp.name] = wf.paramstable[cp.name].linkEventsApp_res[ui_mi.WE_Event_Name];
                                ui_mi.Miss_ParamsDesc[cp.name] = cp.description;//xwm modified
                            }
                            if (ui_mi.Miss_Params["Plan_DescFilePath"].ToString() != "")
                            {
                                string[] PlanFile = ui_mi.Miss_Params["Plan_DescFilePath"].ToString().Split(new char[] { '$' });
                                record.PlanName = PlanFile[0];
                                record.PlanPath = PlanFile[1];
                            }
                            record.PlanDesc = ui_mi.Miss_Params["Plan_Desc"].ToString();
                        }
                        if (atc.Miss_Desc == "管控措施实施后确认风险是否可接受，并进行风险登记")
                        {
                            UI_MISSION ui_mi = new UI_MISSION();
                            List<Mission_Param> mis_pars1 = wfsd.GetMissParams(atc.Miss_Id);//获取当前任务参数
                            foreach (var par in mis_pars1)
                            {
                                CParam cp = new CParam();
                                cp.type = par.Param_Type;
                                cp.name = par.Param_Name;
                                cp.value = par.Param_Value;
                                cp.description = par.Param_Desc;
                                ui_mi.Miss_Params[cp.name] = cp.value;
                                // ui_mi.Miss_ParamsAppRes[cp.name] = wf.paramstable[cp.name].linkEventsApp_res[ui_mi.WE_Event_Name];
                                ui_mi.Miss_ParamsDesc[cp.name] = cp.description;//xwm modified
                            }
                            record.RiskIsAccept = ui_mi.Miss_Params["RiskIsAcceptable"].ToString();

                        }
                        if (atc.Miss_Desc == "管控措施实施后确认风险是否消除，并进行风险登记")
                        {
                            UI_MISSION ui_mi = new UI_MISSION();
                            List<Mission_Param> mis_pars1 = wfsd.GetMissParams(atc.Miss_Id);//获取当前任务参数
                            foreach (var par in mis_pars1)
                            {
                                CParam cp = new CParam();
                                cp.type = par.Param_Type;
                                cp.name = par.Param_Name;
                                cp.value = par.Param_Value;
                                cp.description = par.Param_Desc;
                                ui_mi.Miss_Params[cp.name] = cp.value;
                                // ui_mi.Miss_ParamsAppRes[cp.name] = wf.paramstable[cp.name].linkEventsApp_res[ui_mi.WE_Event_Name];
                                ui_mi.Miss_ParamsDesc[cp.name] = cp.description;//xwm modified
                            }
                            record.RiskIsAccept = ui_mi.Miss_Params["RiskIsAcceptable"].ToString();

                        }
                    }
                }
                if (getentity_name.W_Name == "A11dot1")
                {
                    List<Mission> db_miss = wfsd.GetWFEntityMissions(item.WE_Id);
                    foreach (var atc in db_miss)
                    {
                        if (atc.Miss_Desc == "风险提报")
                        {
                            UI_MISSION ui_mi = new UI_MISSION();
                            List<Mission_Param> mis_pars1 = wfsd.GetMissParams(atc.Miss_Id);//获取当前任务参数
                            foreach (var par in mis_pars1)
                            {
                                CParam cp = new CParam();
                                cp.type = par.Param_Type;
                                cp.name = par.Param_Name;
                                cp.value = par.Param_Value;
                                cp.description = par.Param_Desc;
                                ui_mi.Miss_Params[cp.name] = cp.value;
                                // ui_mi.Miss_ParamsAppRes[cp.name] = wf.paramstable[cp.name].linkEventsApp_res[ui_mi.WE_Event_Name];
                                ui_mi.Miss_ParamsDesc[cp.name] = cp.description;//xwm modified
                            }
                            record.zz = ui_mi.Miss_Params["Zz_Name"].ToString();
                            record.sbGycode = ui_mi.Miss_Params["Equip_GyCode"].ToString();
                            record.sbCode = ui_mi.Miss_Params["Equip_Code"].ToString();
                            record.problemDesc = ui_mi.Miss_Params["SubmitProblemDesc"].ToString();
                        }
                        if (atc.Miss_Desc == "危害识别")
                        {
                            UI_MISSION ui_mi = new UI_MISSION();
                            List<Mission_Param> mis_pars1 = wfsd.GetMissParams(atc.Miss_Id);//获取当前任务参数
                            foreach (var par in mis_pars1)
                            {
                                CParam cp = new CParam();
                                cp.type = par.Param_Type;
                                cp.name = par.Param_Name;
                                cp.value = par.Param_Value;
                                cp.description = par.Param_Desc;
                                ui_mi.Miss_Params[cp.name] = cp.value;
                                // ui_mi.Miss_ParamsAppRes[cp.name] = wf.paramstable[cp.name].linkEventsApp_res[ui_mi.WE_Event_Name];
                                ui_mi.Miss_ParamsDesc[cp.name] = cp.description;//xwm modified
                            }
                            if (ui_mi.Miss_Params["Risk_Type"].ToString() == "一般风险")
                            {
                                record.RiskRecognitionResult = "一般风险";
                            }
                            if (ui_mi.Miss_Params["Risk_Type"].ToString() == "重大风险")
                            {
                                record.RiskRecognitionResult = "重大风险";
                            }
                        }
                        if (atc.Miss_Desc == "评估小组确立管控措施")
                        {
                            UI_MISSION ui_mi = new UI_MISSION();
                            List<Mission_Param> mis_pars1 = wfsd.GetMissParams(atc.Miss_Id);//获取当前任务参数
                            foreach (var par in mis_pars1)
                            {
                                CParam cp = new CParam();
                                cp.type = par.Param_Type;
                                cp.name = par.Param_Name;
                                cp.value = par.Param_Value;
                                cp.description = par.Param_Desc;
                                ui_mi.Miss_Params[cp.name] = cp.value;
                                // ui_mi.Miss_ParamsAppRes[cp.name] = wf.paramstable[cp.name].linkEventsApp_res[ui_mi.WE_Event_Name];
                                ui_mi.Miss_ParamsDesc[cp.name] = cp.description;//xwm modified
                            }
                            if (ui_mi.Miss_Params["Plan_DescFilePath"].ToString() != "")
                            {
                                string[] PlanFile = ui_mi.Miss_Params["Plan_DescFilePath"].ToString().Split(new char[] { '$' });
                                record.PlanName = PlanFile[0];
                                record.PlanPath = PlanFile[1];
                            }
                            record.PlanDesc = ui_mi.Miss_Params["Plan_Desc"].ToString();
                        }
                        if (atc.Miss_Desc == "车间确立管控措施")
                        {
                            UI_MISSION ui_mi = new UI_MISSION();
                            List<Mission_Param> mis_pars1 = wfsd.GetMissParams(atc.Miss_Id);//获取当前任务参数
                            foreach (var par in mis_pars1)
                            {
                                CParam cp = new CParam();
                                cp.type = par.Param_Type;
                                cp.name = par.Param_Name;
                                cp.value = par.Param_Value;
                                cp.description = par.Param_Desc;
                                ui_mi.Miss_Params[cp.name] = cp.value;
                                // ui_mi.Miss_ParamsAppRes[cp.name] = wf.paramstable[cp.name].linkEventsApp_res[ui_mi.WE_Event_Name];
                                ui_mi.Miss_ParamsDesc[cp.name] = cp.description;//xwm modified
                            }
                            if( ui_mi.Miss_Params["Plan_DescFilePath"].ToString()!="")
                            { 
                                string[] PlanFile = ui_mi.Miss_Params["Plan_DescFilePath"].ToString().Split(new char[] { '$' });
                                record.PlanName = PlanFile[0];
                                record.PlanPath = PlanFile[1];
                            }
                            record.PlanDesc = ui_mi.Miss_Params["Plan_Desc"].ToString();
                        }
                    }
                }

            }
            return record;
        }
        //************************************************//
        public string ZzSubmit_submitsignal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string flowname = item["Flow_Name"].ToString();
                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();
               
                signal["Cj_Name"] = item["Cj_Name"].ToString();
                signal["Zz_Name"] = item["Zz_Name"].ToString();
                signal["Equip_GyCode"] = item["Equip_GyCode"].ToString();
                signal["Equip_Code"] = item["Equip_Code"].ToString();
                signal["Equip_Type"] = item["Equip_Type"].ToString();
                signal["Risk_IsAcceptable"] = item["Risk_IsAcceptable"].ToString();
                signal["flag_NandD"] = item["flag_NandD"].ToString();
                signal["Zy_Type"] = item["Zy_Type"].ToString();
                signal["Zy_SubType"] = item["Zy_SubType"].ToString();
                //record
                Dictionary<string, string> record = new Dictionary<string, string>();
                record["username"] = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
                record["time"] = DateTime.Now.ToString();
                //submit
                CWFEngine.SubmitSignal(Convert.ToInt32(flowname), signal, record);
            }
            catch (Exception e)
            {
                return "";
            }
            return ("/A11dot3/Index");
        }
        public string CjFollow_submitsignal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string flowname = item["Flow_Name"].ToString();
                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();

                signal["CjFollow_Result"] = item["CjFollow_Result"].ToString();                
                //record
                Dictionary<string, string> record = new Dictionary<string, string>();
                record["username"] = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
                record["time"] = DateTime.Now.ToString();
                //submit
                CWFEngine.SubmitSignal(Convert.ToInt32(flowname), signal, record);
            }
            catch (Exception e)
            {
                return "";
            }
            return ("/A11dot3/Index");
        }

        public string WaitStop_submitsignal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string flowname = item["Flow_Name"].ToString();
                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();

                signal["WaitStop_Result"] = item["WaitStop_Result"].ToString();
                //record
                Dictionary<string, string> record = new Dictionary<string, string>();
                record["username"] = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
                record["time"] = DateTime.Now.ToString();
                //submit
                CWFEngine.SubmitSignal(Convert.ToInt32(flowname), signal, record);
            }
            catch (Exception e)
            {
                return "";
            }
            return ("/A11dot3/Index");
        }

        public string ZytdAdvise_submitsignal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string flowname = item["Flow_Name"].ToString();
                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();
                signal["Plan_Desc"] = item["Plan_Desc"].ToString();
                signal["Plan_DescFilePath"] = item["Plan_DescFilePath"].ToString();
                signal["ZytdAdvise_Result"] = item["ZytdAdvise_Result"].ToString();
                //record
                Dictionary<string, string> record = new Dictionary<string, string>();
                record["username"] = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
                record["time"] = DateTime.Now.ToString();
                //submit
                CWFEngine.SubmitSignal(Convert.ToInt32(flowname), signal, record);
            }
            catch (Exception e)
            {
                return "";
            }
            return ("/A11dot3/Index");
        }
        public string UpToDirector_submitsignal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string flowname = item["Flow_Name"].ToString();
                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();

                signal["UpToDirector_Done"] = "true";
                //record
                Dictionary<string, string> record = new Dictionary<string, string>();
                record["username"] = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
                record["time"] = DateTime.Now.ToString();
                //submit
                CWFEngine.SubmitSignal(Convert.ToInt32(flowname), signal, record);
            }
            catch (Exception e)
            {
                return "";
            }
            return ("/A11dot3/Index");
        }
        public string ImplementPlan_submitsignal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string flowname = item["Flow_Name"].ToString();
                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();

                signal["ImplementPlan_Done"] = "true";
                //record
                Dictionary<string, string> record = new Dictionary<string, string>();
                record["username"] = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
                record["time"] = DateTime.Now.ToString();
                //submit
                CWFEngine.SubmitSignal(Convert.ToInt32(flowname), signal, record);
            }
            catch (Exception e)
            {
                return "";
            }
            return ("/A11dot3/Index");
        }
        public string JdcConfirm_submitsignal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string flowname = item["Flow_Name"].ToString();
                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();

                signal["JdcConfirm_Done"] = "true";
                //record
                Dictionary<string, string> record = new Dictionary<string, string>();
                record["username"] = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
                record["time"] = DateTime.Now.ToString();
                //submit
                CWFEngine.SubmitSignal(Convert.ToInt32(flowname), signal, record);
            }
            catch (Exception e)
            {
                return "";
            }
            return ("/A11dot3/Index");
        }
        public string RiskAccept_submitsignal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string flowname = item["Flow_Name"].ToString();
                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();

                signal["RiskAccept_Result"] = item["RiskAccept_Result"].ToString(); 
                //record
                Dictionary<string, string> record = new Dictionary<string, string>();
                record["username"] = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
                record["time"] = DateTime.Now.ToString();
                //submit
                CWFEngine.SubmitSignal(Convert.ToInt32(flowname), signal, record);
            }
            catch (Exception e)
            {
                return "";
            }
            return ("/A11dot3/Index");
        }
        public string CreateGroup_submitsignal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string flowname = item["Flow_Name"].ToString();
                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();
                signal["CreateGroup_Done"] = item["CreateGroup_Done"].ToString();
                signal["Group_Header"] = item["Group_Header"].ToString();
                signal["Group_Member"] = item["Group_Member"].ToString();
                //record
                Dictionary<string, string> record = new Dictionary<string, string>();
                record["username"] = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
                record["time"] = DateTime.Now.ToString();
                //submit
                CWFEngine.SubmitSignal(Convert.ToInt32(flowname), signal, record);
            }
            catch (Exception e)
            {
                return "";
            }
            return ("/A11dot3/Index");
        }
        public string RiskRegister_submitsignal(string json1)
            {
            try
                {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string flowname = item["Flow_Name"].ToString();
                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();
                //补充跳转A14dot3的变量，Equip_ABCMark
                Dictionary<string, object> paras1 = new Dictionary<string, object>();

                paras1["Equip_GyCode"] = null;

                UI_WFEntity_Info wfei = CWFEngine.GetWorkFlowEntityWithParams(Convert.ToInt32(flowname), paras1);
                //获取设备ABC标识
                EquipManagment tm = new EquipManagment();
                Equip_Info getZy = tm.getEquip_ByGyCode(paras1["Equip_GyCode"].ToString());
                signal["Equip_ABCMark"] = getZy.Equip_ABCmark;


                signal["RiskRegister_Done"] = "true";
                signal["YDorTG_Result"] = item["YDorTG_Result"].ToString();
                //record
                Dictionary<string, string> record = new Dictionary<string, string>();
                record["username"] = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
                record["time"] = DateTime.Now.ToString();
                //submit
                CWFEngine.SubmitSignal(Convert.ToInt32(flowname), signal, record);
                }
            catch (Exception e)
                {
                return "";
                }
            return ("/A11dot3/Index");
            }
        public class Gxqmodel
        {
            public string WE_Ser;
            public int WE_Id;
        }
        public string A11HistoryList(string WorkFlow_Name)
        {
            string username = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;

            List<object> or = new List<object>();
            string WE_Status = "3";
            string query_list = "distinct E.WE_Ser, E.WE_Id, R.username,M.Event_Name";
            string query_condition = "E.W_Name='" + WorkFlow_Name + "' and E.WE_Status='" + WE_Status + "' and M.Event_Name='RiskRegister" + "' and R.username is not null";
            string record_filter = "username is not null";
            DataTable dt = CWFEngine.QueryAllInformation(query_list, query_condition, record_filter);
            List<Gxqmodel> Gmlist = new List<Gxqmodel>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Gxqmodel Gm = new Gxqmodel();
                Gm.WE_Id = Convert.ToInt16(dt.Rows[i]["WE_Id"]);
                Gm.WE_Ser = dt.Rows[i]["WE_Ser"].ToString();
                Gmlist.Add(Gm);
            }
            List<HistroyModel> Hm = new List<HistroyModel>();
            int ind = 1;
            foreach (var arc in Gmlist)
            {
                RiskRegisterModel record = new RiskRegisterModel();

                WorkFlows wfsd = new WorkFlows();

                List<WorkFlow_Entity> getentity = wfsd.GetWorkFlowEntitiesbySer(arc.WE_Ser);//通过实体号得到串号，通过串号在得到11.1或11.2的实体号及参数
                record.WF_Ser = arc.WE_Ser;
                record.WF_Id = Convert.ToString(arc.WE_Id);
                foreach (var item in getentity)
                {

                    WorkFlow_Define getentity_name = wfsd.GetWorkFlowDefine(item.WE_Id);//通过每个实体号找到该实体号的名字，就可以确定每个实体号的内容
                        //List<UI_MISSION> AllHistoryMiss = CWFEngine.GetHistoryMissions(item.WE_Id);
                        //if (AllHistoryMiss[AllHistoryMiss.Count - 1].WE_Event_Desc == "风险登记")
                        //{ }
                    if (getentity_name.W_Name == "A11dot3")
                    { 
                         List<Mission> db_miss = wfsd.GetWFEntityMissions(item.WE_Id);
                         foreach (var atc in db_miss)
                         {
                             if (atc.Miss_Desc == "风险登记")
                             {
                                 UI_MISSION ui_mi = new UI_MISSION();
                                 List<Mission_Param> mis_pars1 = wfsd.GetMissParams(atc.Miss_Id);//获取当前任务参数
                                 foreach (var par in mis_pars1)
                                 {
                                     CParam cp = new CParam();
                                     cp.type = par.Param_Type;
                                     cp.name = par.Param_Name;
                                     cp.value = par.Param_Value;
                                     cp.description = par.Param_Desc;
                                     ui_mi.Miss_Params[cp.name] = cp.value;
                                     // ui_mi.Miss_ParamsAppRes[cp.name] = wf.paramstable[cp.name].linkEventsApp_res[ui_mi.WE_Event_Name];
                                     ui_mi.Miss_ParamsDesc[cp.name] = cp.description;//xwm modified
                                 }
                                 record.YDorTG_Result = ui_mi.Miss_Params["YDorTG_Result"].ToString();
                             }
                         }
                    }
                    if (getentity_name.W_Name == "A11dot2")
                    {
                        List<Mission> db_miss = wfsd.GetWFEntityMissions(item.WE_Id);
                        foreach (var atc in db_miss)
                        {
                            if (atc.Miss_Desc == "现场工程师提报隐患")
                            {
                                UI_MISSION ui_mi = new UI_MISSION();
                                List<Mission_Param> mis_pars1 = wfsd.GetMissParams(atc.Miss_Id);//获取当前任务参数
                                foreach (var par in mis_pars1)
                                {
                                    CParam cp = new CParam();
                                    cp.type = par.Param_Type;
                                    cp.name = par.Param_Name;
                                    cp.value = par.Param_Value;
                                    cp.description = par.Param_Desc;
                                    ui_mi.Miss_Params[cp.name] = cp.value;
                                    // ui_mi.Miss_ParamsAppRes[cp.name] = wf.paramstable[cp.name].linkEventsApp_res[ui_mi.WE_Event_Name];
                                    ui_mi.Miss_ParamsDesc[cp.name] = cp.description;//xwm modified
                                }
                                record.zz = ui_mi.Miss_Params["Zz_Name"].ToString();
                                record.sbGycode = ui_mi.Miss_Params["Equip_GyCode"].ToString();
                                record.sbCode = ui_mi.Miss_Params["Equip_Code"].ToString();
                                record.problemDesc = ui_mi.Miss_Params["Problem_Desc"].ToString();
                            }
                            if (atc.Miss_Desc == "可靠性工程师风险矩阵评估")
                            {
                                UI_MISSION ui_mi = new UI_MISSION();
                                List<Mission_Param> mis_pars1 = wfsd.GetMissParams(atc.Miss_Id);//获取当前任务参数
                                foreach (var par in mis_pars1)
                                {
                                    CParam cp = new CParam();
                                    cp.type = par.Param_Type;
                                    cp.name = par.Param_Name;
                                    cp.value = par.Param_Value;
                                    cp.description = par.Param_Desc;
                                    ui_mi.Miss_Params[cp.name] = cp.value;
                                    // ui_mi.Miss_ParamsAppRes[cp.name] = wf.paramstable[cp.name].linkEventsApp_res[ui_mi.WE_Event_Name];
                                    ui_mi.Miss_ParamsDesc[cp.name] = cp.description;//xwm modified
                                }
                                if (ui_mi.Miss_Params["RiskMatrix_Color"].ToString() == "yellow")
                                {
                                    record.RiskRecognitionResult = "一般风险";
                                }
                                if (ui_mi.Miss_Params["RiskMatrix_Color"].ToString() == "red")
                                {
                                    record.RiskRecognitionResult = "重大风险";
                                }
                            }
                            if (atc.Miss_Desc == "专业团队确立管控措施")
                            {
                                UI_MISSION ui_mi = new UI_MISSION();
                                List<Mission_Param> mis_pars1 = wfsd.GetMissParams(atc.Miss_Id);//获取当前任务参数
                                foreach (var par in mis_pars1)
                                {
                                    CParam cp = new CParam();
                                    cp.type = par.Param_Type;
                                    cp.name = par.Param_Name;
                                    cp.value = par.Param_Value;
                                    cp.description = par.Param_Desc;
                                    ui_mi.Miss_Params[cp.name] = cp.value;
                                    // ui_mi.Miss_ParamsAppRes[cp.name] = wf.paramstable[cp.name].linkEventsApp_res[ui_mi.WE_Event_Name];
                                    ui_mi.Miss_ParamsDesc[cp.name] = cp.description;//xwm modified
                                }
                                if (ui_mi.Miss_Params["Plan_DescFilePath"].ToString() != "")
                                {
                                    string[] PlanFile = ui_mi.Miss_Params["Plan_DescFilePath"].ToString().Split(new char[] { '$' });
                                    record.PlanName = PlanFile[0];
                                    record.PlanPath = PlanFile[1];
                                }
                                record.PlanDesc = ui_mi.Miss_Params["Plan_Desc"].ToString();

                            }
                            if (atc.Miss_Desc == "车间确立管控措施")
                            {
                                UI_MISSION ui_mi = new UI_MISSION();
                                List<Mission_Param> mis_pars1 = wfsd.GetMissParams(atc.Miss_Id);//获取当前任务参数
                                foreach (var par in mis_pars1)
                                {
                                    CParam cp = new CParam();
                                    cp.type = par.Param_Type;
                                    cp.name = par.Param_Name;
                                    cp.value = par.Param_Value;
                                    cp.description = par.Param_Desc;
                                    ui_mi.Miss_Params[cp.name] = cp.value;
                                    // ui_mi.Miss_ParamsAppRes[cp.name] = wf.paramstable[cp.name].linkEventsApp_res[ui_mi.WE_Event_Name];
                                    ui_mi.Miss_ParamsDesc[cp.name] = cp.description;//xwm modified
                                }
                                if (ui_mi.Miss_Params["Plan_DescFilePath"].ToString() != "")
                                {
                                    string[] PlanFile = ui_mi.Miss_Params["Plan_DescFilePath"].ToString().Split(new char[] { '$' });
                                    record.PlanName = PlanFile[0];
                                    record.PlanPath = PlanFile[1];
                                }
                                record.PlanDesc = ui_mi.Miss_Params["Plan_Desc"].ToString();
                            }
                            if (atc.Miss_Desc == "管控措施实施后确认风险是否可接受，并进行风险登记")
                            {
                                UI_MISSION ui_mi = new UI_MISSION();
                                List<Mission_Param> mis_pars1 = wfsd.GetMissParams(atc.Miss_Id);//获取当前任务参数
                                foreach (var par in mis_pars1)
                                {
                                    CParam cp = new CParam();
                                    cp.type = par.Param_Type;
                                    cp.name = par.Param_Name;
                                    cp.value = par.Param_Value;
                                    cp.description = par.Param_Desc;
                                    ui_mi.Miss_Params[cp.name] = cp.value;
                                    // ui_mi.Miss_ParamsAppRes[cp.name] = wf.paramstable[cp.name].linkEventsApp_res[ui_mi.WE_Event_Name];
                                    ui_mi.Miss_ParamsDesc[cp.name] = cp.description;//xwm modified
                                }
                                record.RiskIsAccept = ui_mi.Miss_Params["RiskIsAcceptable"].ToString();

                            }
                            if (atc.Miss_Desc == "管控措施实施后确认风险是否消除，并进行风险登记")
                            {
                                UI_MISSION ui_mi = new UI_MISSION();
                                List<Mission_Param> mis_pars1 = wfsd.GetMissParams(atc.Miss_Id);//获取当前任务参数
                                foreach (var par in mis_pars1)
                                {
                                    CParam cp = new CParam();
                                    cp.type = par.Param_Type;
                                    cp.name = par.Param_Name;
                                    cp.value = par.Param_Value;
                                    cp.description = par.Param_Desc;
                                    ui_mi.Miss_Params[cp.name] = cp.value;
                                    // ui_mi.Miss_ParamsAppRes[cp.name] = wf.paramstable[cp.name].linkEventsApp_res[ui_mi.WE_Event_Name];
                                    ui_mi.Miss_ParamsDesc[cp.name] = cp.description;//xwm modified
                                }
                                record.RiskIsAccept = ui_mi.Miss_Params["RiskIsAcceptable"].ToString();

                            }
                        }
                    }
                    if (getentity_name.W_Name == "A11dot1")
                    {
                        List<Mission> db_miss = wfsd.GetWFEntityMissions(item.WE_Id);
                        foreach (var atc in db_miss)
                        {
                            if (atc.Miss_Desc == "风险提报")
                            {
                                UI_MISSION ui_mi = new UI_MISSION();
                                List<Mission_Param> mis_pars1 = wfsd.GetMissParams(atc.Miss_Id);//获取当前任务参数
                                foreach (var par in mis_pars1)
                                {
                                    CParam cp = new CParam();
                                    cp.type = par.Param_Type;
                                    cp.name = par.Param_Name;
                                    cp.value = par.Param_Value;
                                    cp.description = par.Param_Desc;
                                    ui_mi.Miss_Params[cp.name] = cp.value;
                                    // ui_mi.Miss_ParamsAppRes[cp.name] = wf.paramstable[cp.name].linkEventsApp_res[ui_mi.WE_Event_Name];
                                    ui_mi.Miss_ParamsDesc[cp.name] = cp.description;//xwm modified
                                }
                                record.zz = ui_mi.Miss_Params["Zz_Name"].ToString();
                                record.sbGycode = ui_mi.Miss_Params["Equip_GyCode"].ToString();
                                record.sbCode = ui_mi.Miss_Params["Equip_Code"].ToString();
                                record.problemDesc = ui_mi.Miss_Params["SubmitProblemDesc"].ToString();
                            }
                            if (atc.Miss_Desc == "危害识别")
                            {
                                UI_MISSION ui_mi = new UI_MISSION();
                                List<Mission_Param> mis_pars1 = wfsd.GetMissParams(atc.Miss_Id);//获取当前任务参数
                                foreach (var par in mis_pars1)
                                {
                                    CParam cp = new CParam();
                                    cp.type = par.Param_Type;
                                    cp.name = par.Param_Name;
                                    cp.value = par.Param_Value;
                                    cp.description = par.Param_Desc;
                                    ui_mi.Miss_Params[cp.name] = cp.value;
                                    // ui_mi.Miss_ParamsAppRes[cp.name] = wf.paramstable[cp.name].linkEventsApp_res[ui_mi.WE_Event_Name];
                                    ui_mi.Miss_ParamsDesc[cp.name] = cp.description;//xwm modified
                                }
                                if (ui_mi.Miss_Params["Risk_Type"].ToString() == "一般风险")
                                {
                                    record.RiskRecognitionResult = "一般风险";
                                }
                                if (ui_mi.Miss_Params["Risk_Type"].ToString() == "重大风险")
                                {
                                    record.RiskRecognitionResult = "重大风险";
                                }
                            }
                            if (atc.Miss_Desc == "评估小组确立管控措施")
                            {
                                UI_MISSION ui_mi = new UI_MISSION();
                                List<Mission_Param> mis_pars1 = wfsd.GetMissParams(atc.Miss_Id);//获取当前任务参数
                                foreach (var par in mis_pars1)
                                {
                                    CParam cp = new CParam();
                                    cp.type = par.Param_Type;
                                    cp.name = par.Param_Name;
                                    cp.value = par.Param_Value;
                                    cp.description = par.Param_Desc;
                                    ui_mi.Miss_Params[cp.name] = cp.value;
                                    // ui_mi.Miss_ParamsAppRes[cp.name] = wf.paramstable[cp.name].linkEventsApp_res[ui_mi.WE_Event_Name];
                                    ui_mi.Miss_ParamsDesc[cp.name] = cp.description;//xwm modified
                                }
                                if (ui_mi.Miss_Params["Plan_DescFilePath"].ToString() != "")
                                {
                                    string[] PlanFile = ui_mi.Miss_Params["Plan_DescFilePath"].ToString().Split(new char[] { '$' });
                                    record.PlanName = PlanFile[0];
                                    record.PlanPath = PlanFile[1];
                                }
                                record.PlanDesc = ui_mi.Miss_Params["Plan_Desc"].ToString();
                            }
                            if (atc.Miss_Desc == "车间确立管控措施")
                            {
                                UI_MISSION ui_mi = new UI_MISSION();
                                List<Mission_Param> mis_pars1 = wfsd.GetMissParams(atc.Miss_Id);//获取当前任务参数
                                foreach (var par in mis_pars1)
                                {
                                    CParam cp = new CParam();
                                    cp.type = par.Param_Type;
                                    cp.name = par.Param_Name;
                                    cp.value = par.Param_Value;
                                    cp.description = par.Param_Desc;
                                    ui_mi.Miss_Params[cp.name] = cp.value;
                                    // ui_mi.Miss_ParamsAppRes[cp.name] = wf.paramstable[cp.name].linkEventsApp_res[ui_mi.WE_Event_Name];
                                    ui_mi.Miss_ParamsDesc[cp.name] = cp.description;//xwm modified
                                }
                                if (ui_mi.Miss_Params["Plan_DescFilePath"].ToString() != "")
                                {
                                    string[] PlanFile = ui_mi.Miss_Params["Plan_DescFilePath"].ToString().Split(new char[] { '$' });
                                    record.PlanName = PlanFile[0];
                                    record.PlanPath = PlanFile[1];
                                }
                                record.PlanDesc = ui_mi.Miss_Params["Plan_Desc"].ToString();
                            }
                            if (atc.Miss_Desc == "管控措施实施后确认风险是否可接受，并进行风险登记")
                            {
                                UI_MISSION ui_mi = new UI_MISSION();
                                List<Mission_Param> mis_pars1 = wfsd.GetMissParams(atc.Miss_Id);//获取当前任务参数
                                foreach (var par in mis_pars1)
                                {
                                    CParam cp = new CParam();
                                    cp.type = par.Param_Type;
                                    cp.name = par.Param_Name;
                                    cp.value = par.Param_Value;
                                    cp.description = par.Param_Desc;
                                    ui_mi.Miss_Params[cp.name] = cp.value;
                                    // ui_mi.Miss_ParamsAppRes[cp.name] = wf.paramstable[cp.name].linkEventsApp_res[ui_mi.WE_Event_Name];
                                    ui_mi.Miss_ParamsDesc[cp.name] = cp.description;//xwm modified
                                }
                                record.RiskIsAccept = ui_mi.Miss_Params["Risk_IsAcceptable"].ToString();

                            }
                            if (atc.Miss_Desc == "管控措施实施后确认风险是否消除，并进行风险登记")
                            {
                                UI_MISSION ui_mi = new UI_MISSION();
                                List<Mission_Param> mis_pars1 = wfsd.GetMissParams(atc.Miss_Id);//获取当前任务参数
                                foreach (var par in mis_pars1)
                                {
                                    CParam cp = new CParam();
                                    cp.type = par.Param_Type;
                                    cp.name = par.Param_Name;
                                    cp.value = par.Param_Value;
                                    cp.description = par.Param_Desc;
                                    ui_mi.Miss_Params[cp.name] = cp.value;
                                    // ui_mi.Miss_ParamsAppRes[cp.name] = wf.paramstable[cp.name].linkEventsApp_res[ui_mi.WE_Event_Name];
                                    ui_mi.Miss_ParamsDesc[cp.name] = cp.description;//xwm modified
                                }
                                record.RiskIsAccept = ui_mi.Miss_Params["Risk_IsAcceptable"].ToString();

                            }
                        }
                    }

                }
                
                object o = new
                {
                    index=ind,
                    zz = record.zz,
                    equip_gycode = record.sbGycode,
                    sbcode = record.sbCode,
                    problemdesc = record.problemDesc,
                    RiskRecognitionResult = record.RiskRecognitionResult,
                    PlanDesc=record.PlanDesc,
                    PlanName ="/Upload/"+record.PlanPath + "$$" + record.PlanName,                   
                    SSQK = "已实施",
                    RiskIsAccept = record.RiskIsAccept,
                    FollowResult = record.CjFollowResult,
                    PlanCategory = record.YDorTG_Result,

                };
                or.Add(o);
                ind++;
            }
           
          
            string str = JsonConvert.SerializeObject(or);
            return ("{" + "\"data\": " + str + "}");
        }
	}
}
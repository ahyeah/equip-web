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

using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;

using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using EquipBLL.FileManagment;
using FlowEngine.DAL;
using FlowEngine.Modals;
using FlowEngine.Param;

namespace WebApp.Controllers
{
    public class A6dot1Controller : CommonController
    {
        public ActionResult A6()
        {
            string WE_Status = "0";
            string query_list = "distinct E.WE_Ser, E.WE_Id, R.username";
            string query_condition = "E.W_Name='A6dot2dot1" + "' and E.WE_Status='" + WE_Status + "' and R.username is not null";
            string record_filter = "username is not null";
            DataTable dt = CWFEngine.QueryAllInformation(query_list, query_condition, record_filter);
            ViewBag.CompleteCount = 18 - dt.Rows.Count;
            return View();
        }
        public ActionResult Index()
        {
            return View(getIndexListRecords("A6dot1", (Session["User"] as EquipModel.Entities.Person_Info).Person_Name));
        }
        public ActionResult WorkFolw_Detail(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }

        public ActionResult SBSubmit(string wfe_id)
        {           
            return View(getSubmitModel(wfe_id));
        }

        public ActionResult NeedWrite(string wfe_id)
        {

            return View(getWFDetail_Model(wfe_id));
        }
        public ActionResult UploadOperInstruction(string wfe_id)
        {

            return View(getWFDetail_Model(wfe_id));
        }
        public ActionResult ModifyAndUpload(string wfe_id)
        {

            return View(getWFDetail_Model(wfe_id));
        }
        public ActionResult SBConfirm(string wfe_id)
        {

            return View(getWFDetail_Model(wfe_id));
        }
        public ActionResult SBJudge(string wfe_id)
        {

            return View(getWFDetail_Model(wfe_id));
        }
        public ActionResult JdcConfirm(string wfe_id)
        {

            return View(getWFDetail_Model(wfe_id));
        }
        public string A6guicheng()
        {
            List<EQ> list = new List<EQ>();//接收结果
            List<object> r = new List<object>();//输出结果
            A6dot2Managment tm = new A6dot2Managment();
            list = tm.caozuoguicheng();
            for (int i = 0; i < list.Count; i++)
            {
            EquipManagment zm = new EquipManagment();
                //string zzname 
                object o = new
                {
                    index = i + 1,
                    sbGyCode = list[i].sbGyCode,
                    sbCode = list[i].sbCode,
                    sbType = list[i].sbType,
                    GCnewname = Path.Combine("/Upload/A3dot3", list[i].GCnewname) + "$$" + list[i].GColdname

                };
                r.Add(o);
            }





            string str = JsonConvert.SerializeObject(r);
            return ("{" + "\"data\": " + str + "}");
        }
        public string SBSubmit_submitsignal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string flowname = item["Flow_Name"].ToString();
                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();
                signal["SBSubmit_Done"] = "true";
                signal["Cj_Name"] = item["Cj_Name"].ToString();
                signal["Zz_Name"] = item["Zz_Name"].ToString();
                signal["Equip_GyCode"] = item["Equip_GyCode"].ToString();
                signal["Equip_Code"] = item["Equip_Code"].ToString();
                signal["Equip_Type"] = item["Equip_Type"].ToString();
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
            return ("/A6dot1/Index");
        }
        public string NeedWrite_submitsignal(string json1)
        {
            JObject item = (JObject)JsonConvert.DeserializeObject(json1);
            string flowname = item["Flow_Name"].ToString();
            Dictionary<string, string> signal = new Dictionary<string, string>();
            //new input paras
            signal["NeedWrite_Result"] = item["NeedWrite_Result"].ToString();
          
            Dictionary<string, string> record = new Dictionary<string, string>();
            record["username"] = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
            record["time"] = DateTime.Now.ToString();
            CWFEngine.SubmitSignal(Convert.ToInt32(flowname), signal, record);
            return ("/A6dot1/Index");
        }
        public string UploadOperInstruction_submitsignal(string json1)
        {
            JObject item = (JObject)JsonConvert.DeserializeObject(json1);
            string flowname = item["Flow_Name"].ToString();
            Dictionary<string, string> signal = new Dictionary<string, string>();
            //new input paras
            signal["OperInstruction"] = item["OperInstruction"].ToString();
            signal["UploadOperInstruction_Done"] = "true";
            
            Dictionary<string, string> record = new Dictionary<string, string>();
            record["username"] = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
            record["time"] = DateTime.Now.ToString();
            CWFEngine.SubmitSignal(Convert.ToInt32(flowname), signal, record);
            return ("/A6dot1/Index");
        }
        public string ModifyAndUpload_submitsignal(string json1)
        {
            JObject item = (JObject)JsonConvert.DeserializeObject(json1);
            string flowname = item["Flow_Name"].ToString();
            Dictionary<string, string> signal = new Dictionary<string, string>();
            //new input paras
            signal["ModifyOperInstruc"] = item["ModifyOperaInstruc"].ToString();
            signal["ModifyAndUpload_Done"] = "true";

            Dictionary<string, string> record = new Dictionary<string, string>();
            record["username"] = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
            record["time"] = DateTime.Now.ToString();
            CWFEngine.SubmitSignal(Convert.ToInt32(flowname), signal, record);
            return ("/A6dot1/Index");
        }
        public string SBConfirm_submitsignal(string json1)
        {
            JObject item = (JObject)JsonConvert.DeserializeObject(json1);
            string flowname = item["Flow_Name"].ToString();
            Dictionary<string, string> signal = new Dictionary<string, string>();
            A6dot2Managment WM = new A6dot2Managment();
            FileItem file = new FileItem();
            int Equip_Id = 0;
            //new input paras
            signal["SBConfirm_Result"] = item["SBConfirm_Result"].ToString();
            signal["SBSupplement"] = item["SBSupplement"].ToString();
            if (signal["SBConfirm_Result"] == "通过")
            {
                //向File_Info里写文件信息
                WorkFlows wfsd = new WorkFlows();
                List<Mission> db_miss = wfsd.GetWFEntityMissions(Convert.ToInt32(flowname));//获取该实体的所有任务
                for (int k = 0; k < db_miss.Count; k++)
                {
                    if (db_miss[k].Miss_Desc == "设备主任判定是否为重要设备")
                    {
                        UI_MISSION ui_mi1 = new UI_MISSION();
                        List<Mission_Param> mis_pars1 = wfsd.GetMissParams(db_miss[k].Miss_Id);//获取当前任务参数
                        foreach (var par in mis_pars1)
                        {
                            CParam cp = new CParam();
                            cp.type = par.Param_Type;
                            cp.name = par.Param_Name;
                            cp.value = par.Param_Value;
                            cp.description = par.Param_Desc;
                            ui_mi1.Miss_Params[cp.name] = cp.value;
                            // ui_mi.Miss_ParamsAppRes[cp.name] = wf.paramstable[cp.name].linkEventsApp_res[ui_mi.WE_Event_Name];
                            ui_mi1.Miss_ParamsDesc[cp.name] = cp.description;//xwm modified
                        }
                        if (ui_mi1.Miss_Params["SBJudge_Result"].ToString() == "否")
                        {
                            for (int i = 0; i < db_miss.Count; i++)
                            {
                                if (db_miss[i].Miss_Desc == "可靠性工程师修改再上传")
                                {
                                    UI_MISSION ui_mi = new UI_MISSION();
                                    ui_mi.WE_Entity_Id = Convert.ToInt32(flowname);
                                    ui_mi.WE_Event_Desc = db_miss[i].Miss_Desc;
                                    ui_mi.WE_Event_Name = db_miss[i].Event_Name;
                                    ui_mi.WE_Name = db_miss[i].Miss_Name;
                                    ui_mi.Mission_Url = ""; //历史任务的页面至空
                                    ui_mi.Miss_Id = db_miss[i].Miss_Id;


                                    List<Mission_Param> mis_pars = wfsd.GetMissParams(db_miss[i].Miss_Id);//获取当前任务参数
                                    foreach (var par in mis_pars)
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
                                    Equip_Id = WM.GetEquipIdBySbCode(ui_mi.Miss_Params["Equip_Code"].ToString());
                                    string[] File_AllName = ui_mi.Miss_Params["ModifyOperInstruc"].ToString().Split(new char[] { '$' });
                                    file.fileName = File_AllName[0];
                                    file.fileNamePresent = File_AllName[1];
                                    file.ext = Path.GetExtension(ui_mi.Miss_Params["ModifyOperInstruc"].ToString());
                                    file.updateTime = DateTime.Now.ToString();
                                    file.Mission_Id = db_miss[i].Miss_Id;
                                    file.WfEntity_Id = Convert.ToInt32(flowname);
                                    //file.Self_Catalog_Catalog_Id = 28;
                                    file.path = @"/Upload/A3dot3";
                                    bool res = WM.AddNewFile(28, file);
                                    break;
                                }

                            }
                            //通过上传时间找到刚才上传到File_Info的文件的File_Id
                            int File_Id = WM.GetFileIdByUpdateTime(Convert.ToDateTime(file.updateTime));
                            //调用File.cs里的AttachtoEuip使连接表产生数据，让设备与操作规程勾连,需要用到Equip_Id和File_Id
                            bool les = WM.AttachtoEuipAndFile(File_Id, Equip_Id);
                        }
                    }
                }


            }
            Dictionary<string, string> record = new Dictionary<string, string>();
            record["username"] = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
            record["time"] = DateTime.Now.ToString();
            CWFEngine.SubmitSignal(Convert.ToInt32(flowname), signal, record);
            return ("/A6dot1/Index");
        }
        public string SBJudge_submitsignal(string json1)
        {
            JObject item = (JObject)JsonConvert.DeserializeObject(json1);
            string flowname = item["Flow_Name"].ToString();

            Dictionary<string, string> signal = new Dictionary<string, string>();
            //new input paras
            signal["SBJudge_Result"] = item["SBJudge_Result"].ToString();

            Dictionary<string, string> record = new Dictionary<string, string>();
            record["username"] = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
            record["time"] = DateTime.Now.ToString();
            CWFEngine.SubmitSignal(Convert.ToInt32(flowname), signal, record);
            return ("/A6dot1/Index");
        }
        public string JdcConfirm_submitsignal(string json1)
        {
            JObject item = (JObject)JsonConvert.DeserializeObject(json1);
            string flowname = item["Flow_Name"].ToString();
            A6dot2Managment WM = new A6dot2Managment();
            FileItem file = new FileItem();
            int Equip_Id = 0;
            Dictionary<string, string> signal = new Dictionary<string, string>();
            //new input paras
            signal["JdcConfirm_Result"] = item["JdcConfirm_Result"].ToString();
            signal["JdcSupplement"] = item["JdcSupplement"].ToString();
            //上传到File_Info

            if (signal["JdcConfirm_Result"] == "通过")
            {
                //向File_Info里写文件信息
                WorkFlows wfsd = new WorkFlows();
                List<Mission> db_miss = wfsd.GetWFEntityMissions(Convert.ToInt32(flowname));//获取该实体的所有任务
                for (int i = 0; i < db_miss.Count; i++)
                {
                    if (db_miss[i].Miss_Desc == "可靠性工程师修改再上传")
                    {
                        UI_MISSION ui_mi = new UI_MISSION();
                        ui_mi.WE_Entity_Id = Convert.ToInt32(flowname);
                        ui_mi.WE_Event_Desc = db_miss[i].Miss_Desc;
                        ui_mi.WE_Event_Name = db_miss[i].Event_Name;
                        ui_mi.WE_Name = db_miss[i].Miss_Name;
                        ui_mi.Mission_Url = ""; //历史任务的页面至空
                        ui_mi.Miss_Id = db_miss[i].Miss_Id;


                        List<Mission_Param> mis_pars = wfsd.GetMissParams(db_miss[i].Miss_Id);//获取当前任务参数
                        foreach (var par in mis_pars)
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
                        Equip_Id = WM.GetEquipIdBySbCode(ui_mi.Miss_Params["Equip_Code"].ToString());
                        string[] File_AllName = ui_mi.Miss_Params["ModifyOperInstruc"].ToString().Split(new char[] { '$' });
                        file.fileName = File_AllName[0];
                        file.fileNamePresent = File_AllName[1];
                        file.ext = Path.GetExtension(ui_mi.Miss_Params["ModifyOperInstruc"].ToString());
                        file.updateTime = DateTime.Now.ToString();
                        file.Mission_Id = db_miss[i].Miss_Id;
                        file.WfEntity_Id = Convert.ToInt32(flowname);
                        //file.Self_Catalog_Catalog_Id = 28;
                        file.path = @"/Upload/A3dot3";
                        bool res = WM.AddNewFile(28, file);
                        break;
                    }

                }
                //通过上传时间找到刚才上传到File_Info的文件的File_Id
                int File_Id = WM.GetFileIdByUpdateTime(Convert.ToDateTime(file.updateTime));
                //调用File.cs里的AttachtoEuip使连接表产生数据，让设备与操作规程勾连,需要用到Equip_Id和File_Id
                bool les = WM.AttachtoEuipAndFile(File_Id, Equip_Id);
            }
            Dictionary<string, string> record = new Dictionary<string, string>();
            record["username"] = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
            record["time"] = DateTime.Now.ToString();
            CWFEngine.SubmitSignal(Convert.ToInt32(flowname), signal, record);
            return ("/A6dot1/Index");
        }
    }
}
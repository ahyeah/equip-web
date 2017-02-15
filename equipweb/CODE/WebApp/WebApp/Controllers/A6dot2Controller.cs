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
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;

namespace WebApp.Controllers
{
    public class A6dot2Controller : CommonController
    {
        A6dot2Managment WDTManagment = new A6dot2Managment();
        public class WDT_detail_Role
        {
            public List<EquipBLL.AdminManagment.A6dot2Managment.WDTHistoy_ListModal> wdt_List = new List<EquipBLL.AdminManagment.A6dot2Managment.WDTHistoy_ListModal>();
            public int isRole;
        }
        public ActionResult Index0()
        {
            return View();

        }
        public ActionResult WDT_TjQuery()
        {
            return View();
        }
        public ActionResult Index_Tj()
        {
            WDT_detail_Role r = new WDT_detail_Role();
            PersonManagment PM = new PersonManagment();
            if (PM.Get_Person_Roles((Session["User"] as EquipModel.Entities.Person_Info).Person_Id).Where(x => x.Role_Name == "现场工程师").Count() > 0)
                r.isRole = 1;
            else
                r.isRole = 0;

            r.wdt_List = WDTManagment.getHistoryList();

            return View(r);
        }
        public ActionResult WDT_detail(int WDT_Id)
        {

            return View(WDTManagment.getWDT_detail(WDT_Id));
        }
        public ActionResult UploadWDT()
        {
            PersonManagment pm = new PersonManagment();
            ViewBag.curtime = DateTime.Now.ToString();
            ViewBag.curuser = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
            return View();
        }
        public JsonResult Tj_PrePrint(int Id)
        {
            A6dot2Managment WM = new A6dot2Managment();

            List<EquipBLL.AdminManagment.A6dot2Managment.WDTTJ_RowModal> r = WM.WDTTJ(Id);
            return Json(r.ToArray());

        }
        public JsonResult Tj_Search(string daterange)
        {

            string starttime, endtime;
            if (!daterange.Equals(""))
            {
                string[] strtime = daterange.Split(new char[] { '-' });
                starttime = strtime[0].Trim() + " 00:00:00";
                endtime = strtime[1].Trim() + " 23:59:59";
            }
            else
            {
                starttime = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
                endtime = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
            }
            A6dot2Managment WM = new A6dot2Managment();

            List<EquipBLL.AdminManagment.A6dot2Managment.WDTTJ_RowModal> r = WM.WDTTJ(starttime, endtime);
            return Json(r.ToArray());
        }
        public JsonResult Tj_PrePrintIndex(string starttime, string endtime)
        {
            A6dot2Managment WM = new A6dot2Managment();

            List<EquipBLL.AdminManagment.A6dot2Managment.WDTTJ_RowModal> r = WM.WDTTJ(starttime, endtime);
            return Json(r.ToArray());

        }
        public string WDTSubmit(string json1)
        {
            try
            {
                PersonManagment PM = new PersonManagment();
                A6dot2Managment WM = new A6dot2Managment();
                A6dot2Tab1 WDT_list = new A6dot2Tab1();

                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                WDT_list.uploadDesc = item["WDT_Desc"].ToString();
                WDT_list.uuploadFileName = item["WDT_filename"].ToString();
                string[] savedFileName = WDT_list.uuploadFileName.Split(new char[] { '$' });
                string wdt_filename = Path.Combine(Request.MapPath("~/Upload"), savedFileName[1]);
                WDT_list.userName = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
                WDT_list.uploadtime = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");

                WDT_list.pqName = PM.Get_Person_Depart((Session["User"] as EquipModel.Entities.Person_Info).Person_Id).Depart_Name;
                int WDT_ID = WM.add_WDT_list(WDT_list);

                JArray wdt_content = (JArray)JsonConvert.DeserializeObject(item["WDT_content"].ToString());
                List<A6dot2Tab2> wdt_list = new List<A6dot2Tab2>();
                foreach (var i in wdt_content)
                {
                    if (Convert.ToInt32(i["isValid"].ToString()) == 0) continue;
                    A6dot2Tab2 tmp = new A6dot2Tab2();
                    tmp.isValid = Convert.ToInt32(i["isValid"].ToString());
                    tmp.equipCode = i["equipCode"].ToString(); ;
                    tmp.equipDesc = i["equipDesc"].ToString();
                    tmp.funLoc = i["funLoc"].ToString();
                    tmp.funLoc_desc = i["funLoc_desc"].ToString();
                    tmp.oilLoc = i["oilLoc"].ToString();
                    tmp.oilLoc_desc = i["oilLoc_desc"].ToString();
                    tmp.oilInterval = Convert.ToInt32(i["oilInterval"].ToString());
                    tmp.unit = i["unit"].ToString();
                    tmp.lastOilTime = i["lastOilTime"].ToString();
                    tmp.lastOilNumber = Convert.ToDouble(i["lastOilNumber"].ToString());
                    tmp.lastOilUnit = i["lastOilUnit"].ToString();
                    tmp.NextOilTime = i["NextOilTime"].ToString();
                    tmp.NextOilNumber = Convert.ToDouble(i["NextOilNumber"].ToString());
                    tmp.NextOilUnit = i["NextOilUnit"].ToString();
                    tmp.oilCode = i["oilCode"].ToString();
                    tmp.oilCode_desc = i["oilCode_desc"].ToString();
                    tmp.substiOilCode = i["substiOilCode"].ToString();
                    tmp.substiOilCode_desc = i["substiOilCode_desc"].ToString();
                    tmp.equip_ZzName = i["equip_ZzName"].ToString();
                    tmp.equip_CjName = i["equip_CjName"].ToString();
                    tmp.equip_PqName = i["equip_PqName"].ToString();
                    tmp.isExceed = Convert.ToInt32(i["isExceed"].ToString());
                    tmp.isOilType = Convert.ToInt32(i["isOilType"].ToString());
                    tmp.Tab1_Id = WDT_ID;
                    wdt_list.Add(tmp);
                }
                WM.add_WDT_content(WDT_ID, wdt_list);
                /*  foreach(var i in wdt_content)
                  {
                      return i.equip_CjName;
                  }
                 * */

                return "/A6dot2/Index_Tj";
            }
            catch { return ""; }

        }
        public string CQ_detail(string pqName, string starttime, string endtime)
        {
            starttime = starttime + " 00:00:00";
            endtime = endtime + " 23:59:59";
            A6dot2Managment WM = new A6dot2Managment();
            string str = JsonConvert.SerializeObject(WM.getCQdetail(pqName, starttime, endtime));
            return ("{" + "\"data\": " + str + "}");

        }
        public string CQ_detailId(int Id,string pqName)
        {
            A6dot2Managment WM = new A6dot2Managment();
            string str = JsonConvert.SerializeObject(WM.getCQdetailId(Id,pqName));
            return ("{" + "\"data\": " + str + "}");
        }
        public string Tj(string file)
        {

            List<A6dot2Tab2> tb_list = new List<A6dot2Tab2>();
            if (file.Equals(""))
            {
                List<A6dot2Tab2> t = new List<A6dot2Tab2>();
                string str = JsonConvert.SerializeObject(t);
                return ("{" + "\"data\": " + str + "}");
            }
            EquipManagment EM = new EquipManagment();
            string curdate = DateTime.Now.ToString("yyyy.MM.dd");
            string EquipPhaseB;
            DataSet ds = new DataSet();
            string[] savedFileName = file.Split(new char[] { '$' });
            string filePath = Path.Combine(Request.MapPath("~/Upload"), savedFileName[1]);
            string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties=Excel 8.0;HDR=Yes;IMEX=1";
            //strConn = "Provider=Microsoft.Ace.OleDb.12.0;Data Source=" + filePath + ";" + "Extended Properties=Excel 12.0;HDR=Yes;IMEX=1";
            strConn = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + filePath + ";" + ";Extended Properties=\"Excel 12.0;HDR=YES;IMEX=1\"";

            //string strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=E:\\equipweb\\1\\CODE\\WebApp\\WebApp\\Upload\\1.xls;Extended Properties=\"Excel 12.0 XML;HDR=YES;IMEX=1\"";
            //       string strConn = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source="
            //     + strFilepPath + ";Extended Properties=\"Excel 12.0 Xml;HDR=YES;IMEX=1\"";
            //   参数HDR的值：
            //   HDR=Yes，这代表第一行是标题，不做为数据使用 ，如果用HDR=NO，则表示第一行不是标题，做为数据来使用。系统默认的是YES
            //IMEX 有三种模式：
            //当 IMEX=0 时为“汇出模式”，这个模式开启的 Excel 档案只能用来做“写入”用途。
            //当 IMEX=1 时为“汇入模式”，这个模式开启的 Excel 档案只能用来做“读取”用途。
            //当 IMEX=2 时为“连結模式”，这个模式开启的 Excel 档案可同时支援“读取”与“写入”用途。
            OleDbConnection conn = new OleDbConnection(strConn);
            //SqlConnection conn = new SqlConnection(strConn);
            try
            {
                conn.Open();
                DataTable sheetNames = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
                OleDbCommand cmd = new OleDbCommand("SELECT * FROM [" + sheetNames.Rows[0][2] + "]", conn);     //为Sheet命名后，顺序可以能编号，要注意   
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                da.Fill(ds);
                // 
                for (int i = 1; i < ds.Tables[0].Rows.Count - 1; i++) // HDR=Yes,故从i=2开始，而非i=3, xwm modify
                {
                    string name = ds.Tables[0].Rows[i][0].ToString();
                    string standard_value = ds.Tables[0].Rows[i][1].ToString();
                    A6dot2Tab2 tmp = new A6dot2Tab2();
                    tmp.equipCode = ds.Tables[0].Rows[i][0].ToString(); ;
                    tmp.equipDesc = ds.Tables[0].Rows[i][1].ToString();
                    tmp.funLoc = ds.Tables[0].Rows[i][2].ToString();
                    tmp.funLoc_desc = ds.Tables[0].Rows[i][3].ToString();
                    tmp.oilLoc = ds.Tables[0].Rows[i][4].ToString();
                    tmp.oilLoc_desc = ds.Tables[0].Rows[i][5].ToString();
                    tmp.oilInterval = Convert.ToInt32(ds.Tables[0].Rows[i][6].ToString());
                    tmp.unit = ds.Tables[0].Rows[i][7].ToString();
                    tmp.lastOilTime = ds.Tables[0].Rows[i][8].ToString();
                    tmp.lastOilNumber = Convert.ToDouble(ds.Tables[0].Rows[i][9].ToString());
                    tmp.lastOilUnit = ds.Tables[0].Rows[i][10].ToString();
                    tmp.NextOilTime = ds.Tables[0].Rows[i][11].ToString();
                    tmp.NextOilNumber = Convert.ToDouble(ds.Tables[0].Rows[i][12].ToString());
                    tmp.NextOilUnit = ds.Tables[0].Rows[i][13].ToString();
                    tmp.oilCode = ds.Tables[0].Rows[i][14].ToString();
                    tmp.oilCode_desc = ds.Tables[0].Rows[i][15].ToString();
                    tmp.substiOilCode = ds.Tables[0].Rows[i][16].ToString();
                    tmp.substiOilCode_desc = ds.Tables[0].Rows[i][17].ToString();
                    if (EM.getEquip_Info(tmp.equipCode) != null)
                    {
                        EquipPhaseB = EM.getEquip_Info(tmp.equipCode).Equip_PhaseB;
                        if (EquipPhaseB.Equals("机泵") || EquipPhaseB.Equals("风机"))
                            tmp.isOilType = 1;
                        else
                            tmp.isOilType = 0;

                        List<Equip_Archi> ZzCj = EM.getEquip_ZzBelong(EM.getEquip_Info(tmp.equipCode).Equip_Id);
                        tmp.equip_ZzName = ZzCj.First().EA_Name;
                        tmp.equip_CjName = ZzCj.Last().EA_Name;
                        tmp.equip_PqName = EM.getPq(tmp.equip_CjName);
                        tmp.isValid = 1;
                    }
                    else
                        tmp.isValid = 0;

                    if (curdate.CompareTo(tmp.NextOilTime) > 0)
                        tmp.isExceed = 1;
                    else
                        tmp.isExceed = 0;
                    tb_list.Add(tmp);
                }
                conn.Close();
                string str = JsonConvert.SerializeObject(tb_list);
                return ("{" + "\"data\": " + str + "}");

            }
            catch (Exception e)
            {
                List<A6dot2Tab2> t = new List<A6dot2Tab2>();
                string str = JsonConvert.SerializeObject(t);
                return ("{" + "\"data\": " + str + "}");


            }




        }
        public class Equipinfo
        {
       
            public string username;
            public int userid;
            public string belong_depart;
            public List<Equip_Info> pp;
        }
        public class k_model
        {

            public List<UI_MISSION> ALLHistoryMiss;
            public List<string> MissTime;
            public List<string> MissUser;
            public Dictionary<string, string> e_name;

            //可靠性工程师上传整改方案
           public List<Kk_change> Kk;
           public List<Kk_change> Zj;
        }
        public class Kk_change
        {


            public string eq_name;
            public string eq_code;
            public string eq_check_result;
            public string eq_check_reason;
            public string eq_detail;
            public string eq_file;
            public string eq_file_0;
            public string eq_file_1;
            public string eq_zj_reason;
           
           
        }
        public ActionResult Index()
        {
            return View(getIndexListRecords("A6dot2", (Session["User"] as EquipModel.Entities.Person_Info).Person_Name));            
        }
        // GET: /A6dot2/不具备备用条件设备装置提报
        public ActionResult Xc_Sample(string wfe_id)
        {



            return View(getSubmitModel(wfe_id));
        }
        public ActionResult Sj_Result(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }
        public ActionResult Kk_Confirm(string wfe_id)
        {
            return View(get_kmodel(wfe_id));
        }
        public k_model get_kmodel(string wfe_id)
        {
            k_model cm = new k_model();
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
            UI_MISSION miss = CWFEngine.GetActiveMission<Person_Info>(int.Parse(wfe_id), ((IObjectContextAdapter)(new EquipWebContext())).ObjectContext);
            string a = miss.Miss_Params["Equip_Info"].ToString();
            string file = miss.Miss_Params["Assay_File"].ToString();
            JArray jsonVal = JArray.Parse(a) as JArray;
            dynamic table2 = jsonVal;
            Dictionary<string, string> e_name = new Dictionary<string, string>();
            List<string> e_code = new List<string>();
            foreach (dynamic T in table2)
            {
                if (T.equip_check == "true")
                {
                    string temp = T.equip_name;
                    string tcode = T.equip_code;

                    e_name.Add(temp,tcode);
                   
                }
            }
            ViewBag.file = file;
            cm.e_name = e_name; 
            return cm;
        }
        public ActionResult Kk_Change_File(string wfe_id)
        {
            return View(get_Kk_Changemodel(wfe_id));
        }
        public k_model get_Kk_Changemodel(string wfe_id)
        {
            k_model cm = new k_model();
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
            UI_MISSION miss = CWFEngine.GetActiveMission<Person_Info>(int.Parse(wfe_id), ((IObjectContextAdapter)(new EquipWebContext())).ObjectContext);
            string a = miss.Miss_Params["Equip_Info"].ToString();
            cm.Kk=new List<Kk_change>();
            JArray jsonVal = JArray.Parse(a) as JArray;
            dynamic table2 = jsonVal;
          
           // k_model result = new k_model();
            foreach (dynamic T in table2)
            {

                if (T.equip_check_result == "false" && T.equip_zj_result == "")
                {
                    Kk_change aa = new Kk_change();
                    aa.eq_name = T.equip_name;
                    aa.eq_check_reason = T.equip_reason;
                    aa.eq_code = T.equip_code;

                    cm.Kk.Add(aa);



                    //     result.Add();

                }
                else if (T.equip_zj_result == "false" && T.equip_check_result == "false")
                {
                    Kk_change aa = new Kk_change();
                    aa.eq_name = T.equip_name;
                    aa.eq_check_reason = T.equip_reason;
                    aa.eq_code = T.equip_code;
                    aa.eq_zj_reason = T.equip_zj_reason;
                    aa.eq_file = T.equip_file;
                    if (aa.eq_file != "")
                    {
                        string[] ss = (aa.eq_file).Split(new char[] { '$' });
                        aa.eq_file_0 = ss[0];

                        aa.eq_file_1 = ss[1];
                        string ExistFilename = Path.Combine("/Upload", aa.eq_file_1);
                        aa.eq_file_1 = ExistFilename;
                    }



                    cm.Kk.Add(aa);

                    ViewBag.zj_1 = "否";
                
                }








            }

             
            return cm;
        }
        public ActionResult Zytd_Confirm(string wfe_id)

        {
            UI_MISSION miss = CWFEngine.GetActiveMission<Person_Info>(int.Parse(wfe_id), ((IObjectContextAdapter)(new EquipWebContext())).ObjectContext);
            ViewBag.currentMiss = miss;


            return View(get_Zj_confirm(wfe_id));
        }
        public k_model get_Zj_confirm(string wfe_id)
        {
            k_model cm = new k_model();
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
            UI_MISSION miss = CWFEngine.GetActiveMission<Person_Info>(int.Parse(wfe_id), ((IObjectContextAdapter)(new EquipWebContext())).ObjectContext);
            string a = miss.Miss_Params["Equip_Info"].ToString();
            
            cm.Zj = new List<Kk_change>();
            JArray jsonVal = JArray.Parse(a) as JArray;
            dynamic table2 = jsonVal;

            // k_model result = new k_model();
            foreach (dynamic T in table2)
            {

             
                    Kk_change aa = new Kk_change();
                    aa.eq_name = T.equip_name;
                    aa.eq_code = T.equip_code;
                    aa.eq_check_reason = T.equip_reason;
                   
                    aa.eq_file = T.equip_file;
                    if (aa.eq_file != "") {
                        string[] ss = (aa.eq_file).Split(new char[] { '$' });
                        aa.eq_file_0 = ss[0];
                        
                        aa.eq_file_1 = ss[1];
                        string ExistFilename = Path.Combine("/Upload", aa.eq_file_1);
                        aa.eq_file_1 = ExistFilename;
                    }



                    cm.Zj.Add(aa);
                   


                    //     result.Add();

               
            }


            return cm;
        }
        public ActionResult Xc_Confirm(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }
        public ActionResult Sj_Assay_File(string wfe_id)
        {
            UI_MISSION miss = CWFEngine.GetActiveMission<Person_Info>(int.Parse(wfe_id), ((IObjectContextAdapter)(new EquipWebContext())).ObjectContext);
            ViewBag.currentMiss = miss;

            return View(getWFDetail_Model(wfe_id));
        }
        public ActionResult Kk_Confirm_Again(string wfe_id)
        {
            return View(get_Kk_Again(wfe_id));
        }
        public k_model get_Kk_Again(string wfe_id)
        {
            k_model cm = new k_model();
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
            UI_MISSION miss = CWFEngine.GetActiveMission<Person_Info>(int.Parse(wfe_id), ((IObjectContextAdapter)(new EquipWebContext())).ObjectContext);
            string a = miss.Miss_Params["Equip_Info"].ToString();
            string file = miss.Miss_Params["Sj_Assay_File"].ToString();
            JArray jsonVal = JArray.Parse(a) as JArray;
            dynamic table2 = jsonVal;
            Dictionary<string, string> e_name = new Dictionary<string, string>();
            List<string> e_code = new List<string>();
            foreach (dynamic T in table2)
            {
                if (T.equip_zj_result == "true")
                {
                    string temp = T.equip_name;
                    string tcode = T.equip_code;

                    e_name.Add(temp, tcode);

                }
            }
            ViewBag.file = file;
            cm.e_name = e_name;
            return cm;
        }
        public ActionResult WorkFolw_Detail(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }
        //public string submit(string name)
        //{

        //    string s = name;
        //    string c = "";

        //    string[] namesubmit = s.Split(new char[] { ',' });
        //    int user_id = (Session["User"] as EquipModel.Entities.Person_Info).Person_Id;
        //    PersonManagment PM = new PersonManagment();
        //    EquipBLL.AdminManagment.PersonManagment.P_viewModal user_part = PM.Get_PersonModal(user_id);
        //    for (int i = 0; i < namesubmit.Length; i++)
        //    {
        //        int submitid = (Session[namesubmit[i]] as EquipModel.Entities.Person_Info).Person_Id;
        //        PersonManagment Pp = new PersonManagment();
        //        EquipBLL.AdminManagment.PersonManagment.P_viewModal sub_part = Pp.Get_PersonModal(submitid);
        //        if (sub_part == user_part)
        //        {
        //            c = "false";
        //        }
        //        else
        //        {
        //            c = "true";
        //        };
        //    }
        //    if (c == "false")
        //    {

        //        return ("false");
        //    }
        //    else
        //    {
        //        return ("true");
        //    }
        //}
        public string get_equip_info()
        {
            Equipinfo info = new Equipinfo();
            PersonManagment PM = new PersonManagment();
            EquipManagment Em=new EquipManagment();
            info.userid = (Session["User"] as EquipModel.Entities.Person_Info).Person_Id;
            info.pp = PM.Get_Person_Equips(info.userid);
            List<Object> miss_obj = new List<object>();
            foreach (var item in info.pp)
            {

                string ie = item.Equip_PhaseB;
                if (ie.Contains("机组"))
                {
                    object m = new
                    {
                        equip_id=item.Equip_Id,
                        equip_code=item.Equip_Code,
                        equip_name = item.Equip_GyCode,
                        equip_type = item.Equip_Type,
                        //equip_zzid=Em.getEA_id_byCode(item.Equip_Code),
                        equip_cjname = Em.getCjnamebyEa_id(Em.getEA_id_byCode(item.Equip_Code)),
                        equip_pqname=Em.getPq(Em.getCjnamebyEa_id(Em.getEA_id_byCode(item.Equip_Code))),
                        //equip_cjname=
                        equip_check="",
                        equip_reason="",
                        equip_file=""
                        
                    };

                    miss_obj.Add(m);
                }
            }
           
            string str = JsonConvert.SerializeObject(miss_obj);
            return ("{" + "\"data\": " + str + "}");

        }
        //每月20号提醒现场工程师采样
        public string submit_sample(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string flowname = item["Flow_Name"].ToString();
                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();
                signal["Equip_Info"] = item["sample"].ToString();
                signal["Cj_Name"] = item["cjname"].ToString();
                signal["Pq_Name"] = item["pqname"].ToString();

                signal["Equip_GyCode"] = "";
                
                signal["sample_done"] ="true";
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
            return ("/A6dot2/Index");
        }
        //每月30号提醒设监中心上传化验结果
        //先判断用户是不是设监中心
            public string Is_Sjzx() {
              string userName = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name.ToString();
              int userId = (Session["User"] as EquipModel.Entities.Person_Info).Person_Id;
              PersonManagment pm = new PersonManagment();
              EquipBLL.AdminManagment.PersonManagment.P_viewModal pv = pm.Get_PersonModal(userId);
              if (pv.Department_Name.Contains("设监中心")) {

                  return "yes";
              }else



                return "no";
            }
       
        public string index_SjFile(string json1) {

            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);

                string wef_id_str = item["wef_id_str"].ToString();
                string Sj_File = item["Sj_File"].ToString();
                string[] wef_id = wef_id_str.Split(new char[] { ',' });
                for (int i = 0; i < wef_id.Length - 1; i++) {

                    //paras
                    Dictionary<string, string> signal = new Dictionary<string, string>();
                    signal["Assay_File"] = Sj_File;
                    signal["Time_Assay"] = DateTime.Now.ToString();

                    //record
                    Dictionary<string, string> record = new Dictionary<string, string>();
                    record["username"] = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
                    record["time"] = DateTime.Now.ToString();
                    //submit
                    CWFEngine.SubmitSignal(Convert.ToInt32(wef_id[i]), signal, record);
                
                }

        




            }
            catch (Exception e)
            {
                return "";
            }
            return ("/A6dot2/Index");
        
        
        
        
        
        }
        


        public string SjFile_signal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string flowname = item["Flow_Name"].ToString();
                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();
                signal["Assay_File"] = item["Sj_File"].ToString();
                signal["Time_Assay"] = DateTime.Now.ToString();

               
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
            return ("/A6dot2/Index");
        }
        //可靠性工程师判断化验结果
        public string submit_KkConfirm(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string flowname = item["Flow_Name"].ToString();
                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();
                signal["Assay_Result"] = item["Assay_result"].ToString();
                signal["Equip_Info"] = item["equip_info"].ToString();
                
               
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
            return ("/A6dot2/Index");
        }
        //可靠性工程师上传整改方案
        public string Kkxgcs_Change_File(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string flowname = item["Flow_Name"].ToString();
                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();
                signal["Equip_Info"] = item["equip_info"].ToString();
                signal["Zg_desc"] = item["Zg_detail"].ToString();
               
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
            return ("/A6dot2/Index");
        }
        //专业团队审核方案是否通过
        public string ZjConfirm_submitsignal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string flowname = item["Flow_Name"].ToString();
                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();
                signal["Zytd_Confirm"] = item["ZjConfirm_Done"].ToString();
                signal["Equip_Info"] = item["equip_info"].ToString();
               
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
            return ("/A6dot2/Index");
        }
        //现场工程师实施确认
        public string XcConfirm_submitsignal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string flowname = item["Flow_Name"].ToString();
                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();
                signal["Xc_Confirm"] = item["XcConfirm_Done"].ToString();
             
               
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
            return ("/A6dot2/Index");
        }
        //设监中心再次上传结果
        public string SjFile_signal_2(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string flowname = item["Flow_Name"].ToString();
                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();
                signal["Sj_Assay_File"] = item["Sj_File_2"].ToString();

               
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
            return ("/A6dot2/Index");
        }
        //可靠性工程师再次判断
        public string submit_KkConfirm_Again(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string flowname = item["Flow_Name"].ToString();
                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();
                signal["Assay_Result_Again"] = item["Assay_result"].ToString();
                signal["Equip_Info"] = item["equip_info"].ToString();
                
               
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
            return ("/A6dot2/Index");
        }

        //导出excel2016-7-18 zxh
        public string out_excel(string json1)
        {
            //json1的值包括表格1,3,5列的值，包括表头（在下面取值到s11，s33，s55数组时已去掉表头），由于前台页面的表格存在合并单元格的情况，致使取到的表格的第一列，第三列，第五列的值并不是直观上的列值，故，若今后表格的格式发生了变化，此处的代码赋值处也需要相应的改变
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string cols_1 = item["cols1"].ToString();//第一列的值
                string[] s11 = new string[22];//生成的表格中有22处属于第一列值（严格按照现有表格的总览设计的）
                for (int i = 0; i < s11.Length; i++)//给数组赋值，全为空
                {
                    s11[i] = "";
                }
                string[] s1 = cols_1.Split(new char[] { ',' });
                for (int i = 1; i < s1.Length; i++)//将前台传来的第一列值放置在固定长度的数组中
                {
                    s11[i - 1] = s1[i];
                }

                string cols_3 = item["cols3"].ToString();
                string[] s33 = new string[15];
                for (int i = 0; i < s33.Length; i++)
                {
                    s33[i] = "";
                }
                string[] s3 = cols_3.Split(new char[] { ',' });
                for (int i = 1; i < s3.Length; i++)
                {
                    s33[i - 1] = s3[i];
                }
                string cols_5 = item["cols5"].ToString();
                string[] s55 = new string[8];
                for (int i = 0; i < s55.Length; i++)
                {
                    s55[i] = "";
                }

                string[] s5 = cols_5.Split(new char[] { ',' });
                for (int i = 1; i < s5.Length; i++)
                {
                    s55[i - 1] = s5[i];
                }

                // 创建Excel 文档
                HSSFWorkbook wk = new HSSFWorkbook();
                ISheet tb = wk.CreateSheet("sheet1");
                //设置单元的宽度  
                tb.SetColumnWidth(0, 25 * 256);
                tb.SetColumnWidth(1, 20 * 256);
                tb.SetColumnWidth(2, 20 * 256);
                tb.SetColumnWidth(3, 20 * 256);
                tb.SetColumnWidth(4, 20 * 256);
                tb.SetColumnWidth(5, 20 * 256);
                //合并标题头，该方法的参数次序是：开始行号，结束行号，开始列号，结束列号。
                tb.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 0, 0, 5));
                IRow head = tb.CreateRow(0);
                head.Height = 20 * 30;
                ICell head_first_cell = head.CreateCell(0);
                ICellStyle cellStyle_head = wk.CreateCellStyle();
                //对齐
                cellStyle_head.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                // 字体
                IFont font = wk.CreateFont();
                font.FontName = "微软雅黑";
                font.Boldweight = 8;
                font.FontHeightInPoints = 16;
                cellStyle_head.SetFont(font);
                head_first_cell.CellStyle = head.CreateCell(1).CellStyle
                                          = head.CreateCell(2).CellStyle
                                          = head.CreateCell(3).CellStyle
                                          = cellStyle_head;
                head_first_cell.SetCellValue("本周超期未换油统计结果");
                tb.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(1, 1, 0, 5));
                IRow row1 = tb.CreateRow(1);
                row1.Height = 20 * 20;

                IRow table_head = tb.CreateRow(2);
                ICellStyle cellStyle_normal = wk.CreateCellStyle();//表格主体部分样式
                //cellStyle_normal.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                //cellStyle_normal.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                //cellStyle_normal.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                //cellStyle_normal.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                cellStyle_normal.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;//水平居中
                cellStyle_normal.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center; //垂直居中 
                //表头
                ICell pq_name = table_head.CreateCell(0);
                pq_name.CellStyle = cellStyle_normal;
                pq_name.SetCellValue("片区名称");

                ICell pq_count = table_head.CreateCell(1);
                pq_count.CellStyle = cellStyle_normal;
                pq_count.SetCellValue("片区超时未换油设备数");

                ICell cj_name = table_head.CreateCell(2);
                cj_name.CellStyle = cellStyle_normal;
                cj_name.SetCellValue("车间名称");

                ICell cj_count = table_head.CreateCell(3);
                cj_count.CellStyle = cellStyle_normal;
                cj_count.SetCellValue("车间超时未换油设备数");

                ICell zz_name = table_head.CreateCell(4);
                zz_name.CellStyle = cellStyle_normal;
                zz_name.SetCellValue("装置名称");

                ICell zz_count = table_head.CreateCell(5);
                zz_count.CellStyle = cellStyle_normal;
                zz_count.SetCellValue("装置超时未换油设备数");


                //联合一片区
                IRow tb3 = tb.CreateRow(3);//创建excel时，由于本次的表格样式固定，每行每列的位置固定，且存在合并单元格的情况，故此表格是一行一行的设计。在设计表格时，首先定义一行，即，  IRow tb3 = tb.CreateRow(3);表示定义了excel的第三行，接下来的第三行设计将基于tb3

                ICell pq_1 = tb3.CreateCell(0);//定义第三行的第0列

                pq_1.SetCellValue("联合一片区");
                tb.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(3, 5, 0, 0));//第三行第0列，存在合并单元格，3,5,0,0表示合并的起始行，终止行，起始列，终止列
                pq_1.CellStyle = cellStyle_normal;//样式


                ICell pq_1_count = tb3.CreateCell(1);
                pq_1_count.CellStyle = cellStyle_normal;
                pq_1_count.SetCellValue(s11[0]);
                tb.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(3, 5, 1, 1));

                ICell cj_1 = tb3.CreateCell(2);
                cj_1.CellStyle = cellStyle_normal;
                cj_1.SetCellValue("联合一车间");
                tb.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(3, 5, 2, 2));

                ICell cj_1_count = tb3.CreateCell(3);
                cj_1_count.CellStyle = cellStyle_normal;
                cj_1_count.SetCellValue(s33[0]);
                tb.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(3, 5, 3, 3));


                ICell lhy_cy = tb3.CreateCell(4);
                lhy_cy.CellStyle = cellStyle_normal;
                lhy_cy.SetCellValue("联合一常压");
                tb.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(3, 3, 4, 4));
                ICell lhy_cy_count = tb3.CreateCell(5);
                lhy_cy_count.CellStyle = cellStyle_normal;
                lhy_cy_count.SetCellValue(s55[0]);
                tb.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(3, 3, 5, 5));

                IRow tb4 = tb.CreateRow(4);//注：由于tb3中存在合并单元格的情况，故tb4的设计是与合并单元格的平行设计

                ICell lhy_cz = tb4.CreateCell(4);
                lhy_cz.CellStyle = cellStyle_normal;
                lhy_cz.SetCellValue("联合一重整");
                tb.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(4, 4, 4, 4));
                ICell lhy_cz_count = tb4.CreateCell(5);
                lhy_cz_count.CellStyle = cellStyle_normal;
                lhy_cz_count.SetCellValue(s11[1]);
                tb.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(4, 4, 5, 5));

                IRow tb5 = tb.CreateRow(5);
                ICell lhy_jq = tb5.CreateCell(4);
                lhy_jq.CellStyle = cellStyle_normal;
                lhy_jq.SetCellValue("联合一加氢");
                tb.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(5, 5, 4, 4));
                ICell lhy_jq_count = tb5.CreateCell(5);
                lhy_jq_count.CellStyle = cellStyle_normal;
                lhy_jq_count.SetCellValue(s11[2]);
                tb.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(5, 5, 5, 5));
                //联合二片区

                IRow tb6 = tb.CreateRow(6);

                ICell pq_2 = tb6.CreateCell(0);
                pq_2.CellStyle = cellStyle_normal;
                pq_2.SetCellValue("联合二片区");
                tb.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(6, 8, 0, 0));

                ICell pq_2_count = tb6.CreateCell(1);
                pq_2_count.CellStyle = cellStyle_normal;
                pq_2_count.SetCellValue(s11[3]);
                tb.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(6, 8, 1, 1));

                ICell cj_2 = tb6.CreateCell(2);
                cj_2.CellStyle = cellStyle_normal;
                cj_2.SetCellValue("联合二车间");
                tb.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(6, 8, 2, 2));

                ICell cj_2_count = tb6.CreateCell(3);
                cj_2_count.CellStyle = cellStyle_normal;
                cj_2_count.SetCellValue(s33[1]);
                tb.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(6, 8, 3, 3));


                ICell lhe_ch_1 = tb6.CreateCell(4);
                lhe_ch_1.CellStyle = cellStyle_normal;
                lhe_ch_1.SetCellValue("联合二 1#催化");
                tb.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(6, 6, 4, 4));
                ICell lhe_ch_1_count = tb6.CreateCell(5);
                lhe_ch_1_count.CellStyle = cellStyle_normal;
                lhe_ch_1_count.SetCellValue(s55[1]);
                tb.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(6, 6, 5, 5));

                IRow tb7 = tb.CreateRow(7);

                ICell lhe_ch_2 = tb7.CreateCell(4);
                lhe_ch_2.CellStyle = cellStyle_normal;
                lhe_ch_2.SetCellValue("联合二 2#催化");
                tb.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(7, 7, 4, 4));
                ICell lhe_ch_2_count = tb7.CreateCell(5);
                lhe_ch_2_count.CellStyle = cellStyle_normal;
                lhe_ch_2_count.SetCellValue(s11[4]);
                tb.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(7, 7, 5, 5));

                IRow tb8 = tb.CreateRow(8);
                ICell lhe_jq = tb8.CreateCell(4);
                lhe_jq.CellStyle = cellStyle_normal;
                lhe_jq.SetCellValue("联合二加氢处理");
                tb.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(8, 8, 4, 4));
                ICell lhe_jq_count = tb8.CreateCell(5);
                lhe_jq_count.CellStyle = cellStyle_normal;
                lhe_jq_count.SetCellValue(s11[5]);
                tb.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(8, 8, 5, 5));


                //联合三片区

                IRow tb9 = tb.CreateRow(9);

                ICell pq_3 = tb9.CreateCell(0);
                pq_3.CellStyle = cellStyle_normal;
                pq_3.SetCellValue("联合三片区");
                tb.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(9, 11, 0, 0));

                ICell pq_3_count = tb9.CreateCell(1);
                pq_3_count.CellStyle = cellStyle_normal;
                pq_3_count.SetCellValue(s11[6]);
                tb.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(9, 11, 1, 1));

                ICell cj_3 = tb9.CreateCell(2);
                cj_3.CellStyle = cellStyle_normal;
                cj_3.SetCellValue("联合三车间");
                tb.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(9, 11, 2, 2));

                ICell cj_3_count = tb9.CreateCell(3);
                cj_3_count.CellStyle = cellStyle_normal;
                cj_3_count.SetCellValue(s33[2]);
                tb.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(9, 11, 3, 3));


                ICell jq_lh = tb9.CreateCell(4);
                jq_lh.CellStyle = cellStyle_normal;
                jq_lh.SetCellValue("加氢裂化");
                tb.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(9, 9, 4, 4));
                ICell jq_lh_count = tb9.CreateCell(5);
                jq_lh_count.CellStyle = cellStyle_normal;
                jq_lh_count.SetCellValue(s55[2]);
                tb.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(9, 9, 5, 5));

                IRow tb10 = tb.CreateRow(10);

                ICell zq_tn = tb10.CreateCell(4);
                zq_tn.CellStyle = cellStyle_normal;
                zq_tn.SetCellValue("制氢及干气提浓");
                tb.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(10, 10, 4, 4));
                ICell zq_tn_count = tb10.CreateCell(5);
                zq_tn_count.CellStyle = cellStyle_normal;
                zq_tn_count.SetCellValue(s11[7]);
                tb.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(10, 10, 5, 5));

                IRow tb11 = tb.CreateRow(11);
                ICell lhs_gq = tb11.CreateCell(4);
                lhs_gq.CellStyle = cellStyle_normal;
                lhs_gq.SetCellValue("联合三罐区");
                tb.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(11, 11, 4, 4));
                ICell lhs_gq_count = tb11.CreateCell(5);
                lhs_gq_count.CellStyle = cellStyle_normal;
                lhs_gq_count.SetCellValue(s11[8]);
                tb.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(11, 11, 5, 5));

                //焦化片区
                IRow tb12 = tb.CreateRow(12);

                ICell jh_pq = tb12.CreateCell(0);
                jh_pq.CellStyle = cellStyle_normal;
                jh_pq.SetCellValue("焦化片区");
                tb.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(12, 13, 0, 0));
                ICell jh_pq_count = tb12.CreateCell(1);
                jh_pq_count.CellStyle = cellStyle_normal;
                jh_pq_count.SetCellValue(s11[9]);
                tb.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(12, 13, 1, 1));

                ICell jh_cj = tb12.CreateCell(2);
                jh_cj.CellStyle = cellStyle_normal;
                jh_cj.SetCellValue("焦化车间");
                tb.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(12, 12, 2, 2));
                ICell jh_cj_count = tb12.CreateCell(3);
                jh_cj_count.CellStyle = cellStyle_normal;
                jh_cj_count.SetCellValue(s33[3]);
                tb.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(12, 12, 3, 3));

                ICell jh = tb12.CreateCell(4);
                jh.CellStyle = cellStyle_normal;
                jh.SetCellValue("焦化");
                tb.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(12, 12, 4, 4));
                ICell jh_count = tb12.CreateCell(5);
                jh_count.CellStyle = cellStyle_normal;
                jh_count.SetCellValue(s55[3]);
                tb.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(12, 12, 5, 5));

                IRow tb13 = tb.CreateRow(13);

                ICell tl_cj = tb13.CreateCell(2);
                tl_cj.CellStyle = cellStyle_normal;
                tl_cj.SetCellValue("铁路车间");
                tb.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(13, 13, 2, 2));
                ICell tl_cj_count = tb13.CreateCell(3);
                tl_cj_count.CellStyle = cellStyle_normal;
                tl_cj_count.SetCellValue(s11[10]);
                tb.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(13, 13, 3, 3));

                ICell tl = tb13.CreateCell(4);
                tl.CellStyle = cellStyle_normal;
                tl.SetCellValue("铁路（资产）");
                tb.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(13, 13, 4, 4));
                ICell tl_count = tb13.CreateCell(5);
                tl_count.CellStyle = cellStyle_normal;
                tl_count.SetCellValue(s33[4]);
                tb.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(13, 13, 5, 5));

                //化工片区

                IRow tb14 = tb.CreateRow(14);

                ICell hg_pq = tb14.CreateCell(0);
                hg_pq.CellStyle = cellStyle_normal;
                hg_pq.SetCellValue("化工片区");
                tb.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(14, 16, 0, 0));
                ICell hg_pq_count = tb14.CreateCell(1);
                hg_pq_count.CellStyle = cellStyle_normal;
                hg_pq_count.SetCellValue(s11[11]);
                tb.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(14, 16, 1, 1));

                ICell qj_cj = tb14.CreateCell(2);
                qj_cj.CellStyle = cellStyle_normal;
                qj_cj.SetCellValue("气加车间");
                tb.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(14, 14, 2, 2));
                ICell qj_cj_count = tb14.CreateCell(3);
                qj_cj_count.CellStyle = cellStyle_normal;
                qj_cj_count.SetCellValue(s33[5]);
                tb.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(14, 14, 3, 3));

                ICell qi_jg = tb14.CreateCell(4);
                qi_jg.CellStyle = cellStyle_normal;
                qi_jg.SetCellValue("气加加工");
                tb.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(14, 14, 4, 4));
                ICell qi_jg_count = tb14.CreateCell(5);
                qi_jg_count.CellStyle = cellStyle_normal;
                qi_jg_count.SetCellValue(s55[4]);
                tb.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(14, 14, 5, 5));

                IRow tb15 = tb.CreateRow(15);
                ICell jbx_cj = tb15.CreateCell(2);
                jbx_cj.CellStyle = cellStyle_normal;
                jbx_cj.SetCellValue("聚丙烯车间");
                tb.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(15, 16, 2, 2));
                ICell jbx_cj_count = tb15.CreateCell(3);
                jbx_cj_count.CellStyle = cellStyle_normal;
                jbx_cj_count.SetCellValue(s11[12]);
                tb.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(15, 16, 3, 3));

                ICell jbx_1 = tb15.CreateCell(4);
                jbx_1.CellStyle = cellStyle_normal;
                jbx_1.SetCellValue("聚丙烯一");
                tb.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(15, 15, 4, 4));
                ICell jbx_1_count = tb15.CreateCell(5);
                jbx_1_count.CellStyle = cellStyle_normal;
                jbx_1_count.SetCellValue(s33[6]);
                tb.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(15, 15, 5, 5));

                IRow tb16 = tb.CreateRow(16);
                ICell jbx_2 = tb16.CreateCell(4);
                jbx_2.CellStyle = cellStyle_normal;
                jbx_2.SetCellValue("聚丙烯二");
                tb.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(16, 16, 4, 4));
                ICell jbx_2_count = tb16.CreateCell(5);
                jbx_2_count.CellStyle = cellStyle_normal;
                jbx_2_count.SetCellValue(s11[13]);
                tb.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(16, 16, 5, 5));

                //综合片区
                IRow tb17 = tb.CreateRow(17);

                ICell zh_pq = tb17.CreateCell(0);
                zh_pq.CellStyle = cellStyle_normal;
                zh_pq.SetCellValue("综合片区");

                ICell zh_pq_count = tb17.CreateCell(1);
                zh_pq_count.CellStyle = cellStyle_normal;
                zh_pq_count.SetCellValue(s11[14]);

                ICell zh_cj = tb17.CreateCell(2);
                zh_cj.CellStyle = cellStyle_normal;
                zh_cj.SetCellValue("综合车间");

                ICell zh_cj_count = tb17.CreateCell(3);
                zh_cj_count.CellStyle = cellStyle_normal;
                zh_cj_count.SetCellValue(s33[7]);

                ICell zh = tb17.CreateCell(4);
                zh.CellStyle = cellStyle_normal;
                zh.SetCellValue("综合");

                ICell zh_count = tb17.CreateCell(5);
                zh_count.CellStyle = cellStyle_normal;
                zh_count.SetCellValue(s55[5]);

                //系统片区

                IRow tb18 = tb.CreateRow(18);
                ICell xt_pq = tb18.CreateCell(0);
                xt_pq.CellStyle = cellStyle_normal;
                xt_pq.SetCellValue("系统片区");
                tb.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(18, 22, 0, 0));
                ICell xt_pq_count = tb18.CreateCell(1);
                xt_pq_count.CellStyle = cellStyle_normal;
                xt_pq_count.SetCellValue(s11[15]);
                tb.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(18, 22, 1, 1));

                ICell yp_cj = tb18.CreateCell(2);
                yp_cj.CellStyle = cellStyle_normal;
                yp_cj.SetCellValue("油品车间");
                tb.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(18, 18, 2, 2));
                ICell yp_cj_count = tb18.CreateCell(3);
                yp_cj_count.CellStyle = cellStyle_normal;
                yp_cj_count.SetCellValue(s33[8]);
                tb.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(18, 18, 3, 3));

                ICell yp = tb18.CreateCell(4);
                yp.CellStyle = cellStyle_normal;
                yp.SetCellValue("油品");
                tb.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(18, 18, 4, 4));
                ICell yp_count = tb18.CreateCell(5);
                yp_count.CellStyle = cellStyle_normal;
                yp_count.SetCellValue(s55[6]);
                tb.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(18, 18, 5, 5));

                IRow tb19 = tb.CreateRow(19);
                ICell ps_cj = tb19.CreateCell(2);
                ps_cj.CellStyle = cellStyle_normal;
                ps_cj.SetCellValue("排水车间");
                tb.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(19, 19, 2, 2));
                ICell ps_cj_count = tb19.CreateCell(3);
                ps_cj_count.CellStyle = cellStyle_normal;
                ps_cj_count.SetCellValue(s11[16]);
                tb.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(19, 19, 3, 3));

                ICell ps = tb19.CreateCell(4);
                ps.CellStyle = cellStyle_normal;
                ps.SetCellValue("排水");
                tb.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(19, 19, 4, 4));
                ICell ps_count = tb19.CreateCell(5);
                ps_count.CellStyle = cellStyle_normal;
                ps_count.SetCellValue(s33[9]);
                tb.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(19, 19, 5, 5));

                IRow tb20 = tb.CreateRow(20);

                ICell gs_cj = tb20.CreateCell(2);
                gs_cj.CellStyle = cellStyle_normal;
                gs_cj.SetCellValue("供水车间");
                tb.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(20, 20, 2, 2));
                ICell gs_cj_count = tb20.CreateCell(3);
                gs_cj_count.CellStyle = cellStyle_normal;
                gs_cj_count.SetCellValue(s11[17]);
                tb.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(20, 20, 3, 3));

                ICell gs_zc = tb20.CreateCell(4);
                gs_zc.CellStyle = cellStyle_normal;
                gs_zc.SetCellValue("供水（资产）");
                tb.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(20, 20, 4, 4));
                ICell gs_zc_count = tb20.CreateCell(5);
                gs_zc_count.CellStyle = cellStyle_normal;
                gs_zc_count.SetCellValue(s33[10]);
                tb.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(20, 20, 5, 5));

                IRow tb21 = tb.CreateRow(21);

                ICell rd_cj = tb21.CreateCell(2);
                rd_cj.CellStyle = cellStyle_normal;
                rd_cj.SetCellValue("热电车间");
                tb.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(21, 21, 2, 2));
                ICell rd_cj_count = tb21.CreateCell(3);
                rd_cj_count.CellStyle = cellStyle_normal;
                rd_cj_count.SetCellValue(s11[18]);
                tb.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(21, 21, 3, 3));

                ICell rd_zc = tb21.CreateCell(4);
                rd_zc.CellStyle = cellStyle_normal;
                rd_zc.SetCellValue("热电（资产）");
                tb.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(21, 21, 4, 4));
                ICell rd_zc_count = tb21.CreateCell(5);
                rd_zc_count.CellStyle = cellStyle_normal;
                rd_zc_count.SetCellValue(s33[11]);
                tb.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(21, 21, 5, 5));

                IRow tb22 = tb.CreateRow(22);

                ICell mt_cj = tb22.CreateCell(2);
                mt_cj.CellStyle = cellStyle_normal;
                mt_cj.SetCellValue("码头车间");
                tb.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(22, 22, 2, 2));
                ICell mt_cj_count = tb22.CreateCell(3);
                mt_cj_count.CellStyle = cellStyle_normal;
                mt_cj_count.SetCellValue(s11[19]);
                tb.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(22, 22, 3, 3));

                ICell mt_zc = tb22.CreateCell(4);
                mt_zc.CellStyle = cellStyle_normal;
                mt_zc.SetCellValue("码头（资产）");
                tb.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(22, 22, 4, 4));
                ICell mt_zc_count = tb22.CreateCell(5);
                mt_zc_count.CellStyle = cellStyle_normal;
                mt_zc_count.SetCellValue(s33[12]);
                tb.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(22, 22, 5, 5));

                //其他

                IRow tb23 = tb.CreateRow(23);
                ICell qt = tb23.CreateCell(0);
                qt.CellStyle = cellStyle_normal;
                qt.SetCellValue("其他");
                tb.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(23, 24, 0, 0));
                ICell qt_count = tb23.CreateCell(1);
                qt_count.CellStyle = cellStyle_normal;
                qt_count.SetCellValue(s11[20]);
                tb.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(23, 24, 1, 1));

                ICell xfd_cj = tb23.CreateCell(2);
                xfd_cj.CellStyle = cellStyle_normal;
                xfd_cj.SetCellValue("消防队");
                tb.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(23, 23, 2, 2));
                ICell xfd_cj_count = tb23.CreateCell(3);
                xfd_cj_count.CellStyle = cellStyle_normal;
                xfd_cj_count.SetCellValue(s33[13]);
                tb.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(23, 23, 3, 3));

                ICell xfd_zz = tb23.CreateCell(4);
                xfd_zz.CellStyle = cellStyle_normal;
                xfd_zz.SetCellValue("消防队");
                tb.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(23, 23, 4, 4));
                ICell xfd_zz_count = tb23.CreateCell(5);
                xfd_zz_count.CellStyle = cellStyle_normal;
                xfd_zz_count.SetCellValue(s55[7]);
                tb.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(23, 23, 5, 5));

                IRow tb24 = tb.CreateRow(24);
                ICell jlz_cj = tb24.CreateCell(2);
                jlz_cj.CellStyle = cellStyle_normal;
                jlz_cj.SetCellValue("计量站");
                tb.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(24, 24, 2, 2));
                ICell jlz_cj_count = tb24.CreateCell(3);
                jlz_cj_count.CellStyle = cellStyle_normal;
                jlz_cj_count.SetCellValue(s11[21]);
                tb.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(24, 24, 3, 3));

                ICell jlz_zz = tb24.CreateCell(4);
                jlz_zz.CellStyle = cellStyle_normal;
                jlz_zz.SetCellValue("计量站");
                tb.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(24, 24, 4, 4));
                ICell jlz_zz_count = tb24.CreateCell(5);
                jlz_zz_count.CellStyle = cellStyle_normal;
                jlz_zz_count.SetCellValue(s33[14]);
                tb.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(24, 24, 5, 5));



                string path = Server.MapPath("~/Upload//本周超期未换油统计结果.xls");
                using (FileStream fs = System.IO.File.OpenWrite(path)) //打开一个xls文件，如果没有则自行创建，如果存在myxls.xls文件则在创建是不要打开该文件！
                {
                    wk.Write(fs);   //向打开的这个xls文件中写入mySheet表并保存。
                    Console.WriteLine("提示：创建成功！");
                }
                return Path.Combine("/Upload", "本周超期未换油统计结果.xls");

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return "";
            }


        }




    }
}
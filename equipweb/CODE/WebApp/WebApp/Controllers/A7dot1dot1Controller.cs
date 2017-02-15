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
using FlowEngine.DAL;
using FlowEngine.Modals;

namespace WebApp.Controllers
{
    public class  A7dot1dot1Controller : CommonController
    {
        //
        public class TableModel
        {
            public string name;
            public string standard_value;
            public string exact_value;
        }

        // GET: /A7dot1/
        public ActionResult Index()
        {
            return View(getIndexListRecords("A7dot1dot1", (Session["User"] as EquipModel.Entities.Person_Info).Person_Name));
        }

        // GET: /A7dot1dot1/装置提报
        public ActionResult ZzSubmit(string wfe_id)
        {
            return View(getSubmitModel(wfe_id));//
        }

        // GET: /A7dot1dot1/检修单位确认
        public ActionResult JxdwConfirm(string wfe_id)
        {
            int cur_PersionID = (Session["User"] as EquipModel.Entities.Person_Info).Person_Id;
            PersonManagment pm = new PersonManagment();
            EquipBLL.AdminManagment.PersonManagment.P_viewModal pv = pm.Get_PersonModal(cur_PersionID);
            ViewBag.user_specis = pv.Speciaty_Names;
            UI_MISSION miss = CWFEngine.GetActiveMission<Person_Info>(int.Parse(wfe_id), ((IObjectContextAdapter)(new EquipWebContext())).ObjectContext);
            ViewBag.currentMiss = miss;
            //由于DongZyConfirm_done 等变量未与该Event关联，所以无法获取， 故而我在下面模拟了这几个变量
            //同样的问题也会出现在JxdwConfirm_submitsignal函数中
            //WFDetail_Model dfm = getWFDetail_Model(wfe_id);
            //dfm.ALLHistoryMiss.Last().Miss_Params["DongZyConfirm_done"] = true.ToString();
            //dfm.ALLHistoryMiss.Last().Miss_Params["DongZyMan"] = "fhp";
            //dfm.ALLHistoryMiss.Last().Miss_Params["DianZyConfirm_done"] = false.ToString();
            //dfm.ALLHistoryMiss.Last().Miss_Params["DianZyMan"] = "";
            //dfm.ALLHistoryMiss.Last().Miss_Params["YiZyConfirm_done"] = false.ToString();
            //dfm.ALLHistoryMiss.Last().Miss_Params["YiZyMan"] = "";
            return View(getWFDetail_Model(wfe_id));
            //return View(dfm);
        }

        // GET: /A7dot1dot1/机动处确认
        public ActionResult JdcConfirm(string wfe_id)
        {
            UI_MISSION miss = CWFEngine.GetActiveMission<Person_Info>(int.Parse(wfe_id), ((IObjectContextAdapter)(new EquipWebContext())).ObjectContext);
            ViewBag.currentMiss = miss;
            return View(getWFDetail_Model(wfe_id));
        }
        public ActionResult GraphicData(string wfe_id)
        {
            int cur_PersionID = (Session["User"] as EquipModel.Entities.Person_Info).Person_Id;
            PersonManagment pm = new PersonManagment();
            EquipBLL.AdminManagment.PersonManagment.P_viewModal pv = pm.Get_PersonModal(cur_PersionID);
            ViewBag.user_specis = pv.Speciaty_Names;
            UI_MISSION miss = CWFEngine.GetActiveMission<Person_Info>(int.Parse(wfe_id), ((IObjectContextAdapter)(new EquipWebContext())).ObjectContext,false);
            ViewBag.currentMiss = miss;
            return View(getWFDetail_Model(wfe_id));
        }


        // GET: /A7dot1dot1/机动处确认
        public void createA13dot1(string from_wfe_id)
        {
            //create new flow A13.1
            UI_MISSION miss = CWFEngine.GetActiveMission<Person_Info>(Convert.ToInt32(from_wfe_id), ((IObjectContextAdapter)(new EquipWebContext())).ObjectContext);
            string th_problem = Convert.ToString(miss.Miss_Params["Th_ProblemRecords"]);
            JArray j_Problem_data = JArray.Parse(th_problem);

            for (int i = 0; i < j_Problem_data.Count; i++)
            {
                JObject j_obj = JObject.Parse(j_Problem_data[i].ToString());
                string problem_catalogy = j_obj["problem_catalogy"].ToString();
                string problem_detail = j_obj["problem_detail"].ToString();
                //将7.1.1的串号赋给新产生的13.1的工作流
                WorkFlows wfsd = new WorkFlows();
                WorkFlow_Entity wfecurrent = wfsd.GetWorkFlowEntity(Convert.ToInt32(from_wfe_id));
                UI_WorkFlow_Entity wfe = CWFEngine.CreateAWFEntityByName("A13dot1", wfecurrent.WE_Ser);

                if (wfe != null)
                {
                    Dictionary<string, string> record = wfe.GetRecordItems();
                    record["username"] = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
                    record["time"] = DateTime.Now.ToString();                    
                    wfe.Start(record);
                    int flow_id = wfe.EntityID;
                    Dictionary<string, string> signal1 = new Dictionary<string, string>();
                    signal1["start_done"] = "true";
                    CWFEngine.SubmitSignal(flow_id, signal1, record);
                    //paras
                    Dictionary<string, string> signal = new Dictionary<string, string>();
                    signal["ZzSubmit_done"] = "true";
                    signal["Cj_Name"] = miss.Miss_Params["Cj_Name"].ToString();
                    signal["Zz_Name"] = miss.Miss_Params["Zz_Name"].ToString();
                    signal["Equip_GyCode"] = miss.Miss_Params["Equip_GyCode"].ToString();
                    signal["Equip_Code"] = miss.Miss_Params["Equip_Code"].ToString();
                    signal["Equip_Type"] = miss.Miss_Params["Equip_Type"].ToString();
                    signal["Problem_Desc"] = problem_detail;
                    signal["Problem_DescFilePath"] = "";
                    signal["Zy_Type"] = problem_catalogy;
                    signal["Zy_SubType"] = miss.Miss_Params["Zy_SubType"].ToString();
                    signal["Equip_ABCMark"] = miss.Miss_Params["Equip_ABCMark"].ToString();
                    signal["Data_Src"] = "特护记录";
                   
                    //submit
                    //record                    
                    //record["username"] = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
                    record["time"] = DateTime.Now.ToString();
                    //submit
                    CWFEngine.SubmitSignal(flow_id, signal, record);
                }
            }
        }

        public ActionResult WorkFolw_Detail(string wfe_id)
        {
            ViewBag.wfe_id = wfe_id;
            return View(getWFDetail_Model(wfe_id));
        }

        public string ZzSubmit_submitsignal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string flowname = item["Flow_Name"].ToString();
                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();
                signal["ZzSubmit_done"] = "true"; 
                signal["Cj_Name"] = item["Cj_Name"].ToString();
                signal["Zz_Name"] = item["Zz_Name"].ToString();              
                signal["Equip_GyCode"] = item["Equip_GyCode"].ToString();
                signal["Equip_Code"] = item["Equip_Code"].ToString();
                signal["Equip_Type"] = item["Equip_Type"].ToString();
                EquipManagment em = new EquipManagment();
                Equip_Info eqinfo=em.getEquip_Info(item["Equip_Code"].ToString());
                signal["Equip_ABCMark"] = eqinfo.Equip_ABCmark;
                signal["Zy_Type"] = eqinfo.Equip_Type;
                signal["Zy_SubType"] = eqinfo.Equip_PhaseB;            
                signal["Data_Src"] = "特护记录";
                signal["Th_CheckTime"] = DateTime.Now.ToString();
                signal["Th_CheckMen"] = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
                signal["Group_State"] = item["Group_State"].ToString();
                //string Thjl_data = item["Thjl_data"].ToString();
                //string Problem_data = item["Problem_data"].ToString();
                //string workdetail = item["workdetail"].ToString();

                signal["Th_ItemRecord"] = item["Thjl_data"].ToString();
                signal["Th_WorkDetail"] = item["workdetail"].ToString();
                signal["Th_ProblemRecords"] = item["Problem_data"].ToString();
                //record
                Dictionary<string, string> record =  new Dictionary<string, string>();
                record["username"] = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
                record["time"] = DateTime.Now.ToString("yyyyMMddhhmmss");
                //submit
                //由于DongZyConfirm_done 等变量未与该Event关联， 所以submitSignal不会将确认信息提交到工作流
                CWFEngine.SubmitSignal(Convert.ToInt32(flowname), signal,record);
            }
            catch (Exception e)
            {
                return "";
            }
            return ("/A7dot1dot1/Index");
        }

        public string JxdwConfirm_submitsignal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string flowname = item["Flow_Name"].ToString();
                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();
                signal["DongZyConfirm_done"] = item["DongZyConfirm_done"].ToString();
                signal["DianZyConfirm_done"] = item["DianZyConfirm_done"].ToString();
                signal["YiZyConfirm_done"] = item["YiZyConfirm_done"].ToString();
                signal["DongZyMan"] = item["DongZyMan"].ToString();
                signal["DianZyMan"] = item["DianZyMan"].ToString();
                signal["YiZyMan"] = item["YiZyMan"].ToString();

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
            return ("/A7dot1dot1/Index");
        }

        public string JdcConfirm_submitsignal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                string flowname = item["Flow_Name"].ToString();
                //create new flow A13.1
                createA13dot1(flowname);

                //paras
                Dictionary<string, string> signal = new Dictionary<string, string>();
                signal["JdcConfirm_done"] = item["JdcConfirm_done"].ToString();
                UI_MISSION miss = CWFEngine.GetActiveMission<Person_Info>(int.Parse(flowname), ((IObjectContextAdapter)(new EquipWebContext())).ObjectContext);
                signal["Th_CheckMen"] = miss.Miss_Params["Th_CheckMen"].ToString() + "," + miss.Miss_Params["DongZyMan"].ToString() + "," + miss.Miss_Params["DianZyMan"].ToString() + "," + miss.Miss_Params["YiZyMan"].ToString() + "," + (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
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
            return ("/A7dot1dot1/Index");
        }

        public JsonResult Read_THRecord(int WFE_ID)
        {
            List<TableModel> tb_list = new List<TableModel>();
            UI_MISSION miss = CWFEngine.GetActiveMission<Person_Info>(WFE_ID, ((IObjectContextAdapter)(new EquipWebContext())).ObjectContext);
            //由于Th_ItemRecord等变量未于当前event关联， 导致我无法从当前任务中读取这些变量
            //所以我读取了上一条历史Mission， 这种方法效率较低， 应该将这些变量与之关联，并使用上面被注释的语句，更为妥当
            //UI_MISSION miss = CWFEngine.GetHistoryMissions(WFE_ID).Last();
            string th_record = Convert.ToString(miss.Miss_Params["Th_ItemRecord"]);
            string th_problem = Convert.ToString(miss.Miss_Params["Th_ProblemRecords"]);
            string th_detail = Convert.ToString(miss.Miss_Params["Th_WorkDetail"]);
            
            return Json(new { th_record = th_record, th_problem = th_problem, th_detail = th_detail });
        }


        public String judge(int thjl_id)//判断upload下面有没有对应的excel表格，连接成功返回true，失败返回false
        {
            int Thji_Code = thjl_id;
            List<TableModel> tb_list = new List<TableModel>();

            DataSet ds = new DataSet();

            string DirectoryPath = Server.MapPath("~/Upload/特护记录表//");

            String strFilepPath = DirectoryPath + "" + Thji_Code + "特护记录表.xls";

            string strConn = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + strFilepPath + ";" + ";Extended Properties=\"Excel 12.0;HDR=YES;IMEX=1\"";

            OleDbConnection conn = new OleDbConnection(strConn);
            //SqlConnection conn = new SqlConnection(strConn);
            try
            {
                conn.Open();
                DataTable sheetNames = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
                OleDbCommand cmd = new OleDbCommand("SELECT * FROM [" + sheetNames.Rows[0][2] + "]", conn);     //为Sheet命名后，顺序可以能编号，要注意   
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                da.Fill(ds);

                conn.Close();
                return ("true");
            }
            catch (Exception e)
            {
               
                return  ("false");
    
            }



        }

        public JsonResult Thjl_excel(int thjl_id)
        {
            int Thji_Code = thjl_id;
            List<TableModel> tb_list = new List<TableModel>();

            DataSet ds = new DataSet();

            string DirectoryPath = Server.MapPath("~/Upload/特护记录表//");

            String strFilepPath = DirectoryPath + "" + Thji_Code + "特护记录表.xls";

            string strConn = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + strFilepPath + ";" + ";Extended Properties=\"Excel 12.0;HDR=YES;IMEX=1\"";
            
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

                for (int i = 2; i < ds.Tables[0].Rows.Count - 1; i++) // HDR=Yes,故从i=2开始，而非i=3, xwm modify
                {
                    string name = ds.Tables[0].Rows[i][0].ToString();
                    string standard_value = ds.Tables[0].Rows[i][1].ToString();
                    TableModel tmp = new TableModel();
                    tmp.name = name;
                    tmp.standard_value = standard_value;
                    tb_list.Add(tmp);
                }
                conn.Close();
                return Json(tb_list.ToArray(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
            //    List<Object> urlobj = new List<object>();
            //    object tmp = new
            //    {
            //        name = "0",
            //        standard_value = "0"
            //    };
            //    urlobj.Add(tmp);
            //return Json(urlobj.ToArray());
                return Json("");
               // Console.WriteLine(e.Message);
            }



        }

        public string OutputExcel(string wfe_id)
        {

            UI_MISSION miss = CWFEngine.GetActiveMission<Person_Info>(int.Parse(wfe_id), ((IObjectContextAdapter)(new EquipWebContext())).ObjectContext);
            try
            {

                
                string th_record = Convert.ToString(miss.Miss_Params["Th_ItemRecord"]);
                string th_problem = Convert.ToString(miss.Miss_Params["Th_ProblemRecords"]);
                string th_detail = Convert.ToString(miss.Miss_Params["Th_WorkDetail"]);

                string work_detail = th_detail;

                JArray j_Problem_data = JArray.Parse(th_problem);

                JArray j_Thjl_data = JArray.Parse(th_record);

                // Console.WriteLine(work_detail);


                // 创建Excel 文档

                HSSFWorkbook wk = new HSSFWorkbook();

                ISheet tb = wk.CreateSheet("sheet1");

                //设置单元的宽度  
                tb.SetColumnWidth(0, 25 * 256);
                tb.SetColumnWidth(1, 20 * 256);
                tb.SetColumnWidth(2, 20 * 256);
                tb.SetColumnWidth(3, 45 * 256);


                //合并标题头，该方法的参数次序是：开始行号，结束行号，开始列号，结束列号。
                tb.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 0, 0, 3));

                IRow head = tb.CreateRow(0);

                head.Height = 20 * 30;
                ICell head_first_cell = head.CreateCell(0);
                ICellStyle cellStyle_head = wk.CreateCellStyle();

                //对齐
                cellStyle_head.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                // 边框
                /*
                cellStyle_head.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                cellStyle_head.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                cellStyle_head.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                cellStyle_head.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                 * */

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


                head_first_cell.SetCellValue("Ⅰ催化气压机特护记录");
                tb.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(1, 1, 0, 3));

                IRow row1 = tb.CreateRow(1);
                row1.Height = 20 * 20;

                ICell row1_last = row1.CreateCell(0);



                ICellStyle cellStyle_row1 = wk.CreateCellStyle();
                cellStyle_row1.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Right;
                /*
                cellStyle_row1.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                cellStyle_row1.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                cellStyle_row1.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                cellStyle_row1.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                 * */
                row1_last.CellStyle = row1.CreateCell(1).CellStyle = row1.CreateCell(2).CellStyle = row1.CreateCell(3).CellStyle = cellStyle_row1;
                row1_last.SetCellValue("JD04-JD01"); ;

                IRow table_head = tb.CreateRow(2);

                ICellStyle cellStyle_normal = wk.CreateCellStyle();

                cellStyle_normal.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                cellStyle_normal.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                cellStyle_normal.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                cellStyle_normal.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;

                ICell table_head_name = table_head.CreateCell(0);
                table_head_name.CellStyle = cellStyle_normal;
                table_head_name.SetCellValue("项目");

                ICell table_head_standrad = table_head.CreateCell(1);
                table_head_standrad.CellStyle = cellStyle_normal;
                table_head_standrad.SetCellValue("设计值");

                ICell table_head_exact = table_head.CreateCell(2);
                table_head_exact.CellStyle = cellStyle_normal;
                table_head_exact.SetCellValue("实测值");

                ICell table_head_thjl = table_head.CreateCell(3);
                table_head_thjl.CellStyle = cellStyle_normal;
                table_head_thjl.SetCellValue("特护记录");




                ICellStyle thjl_record_cellStyle = wk.CreateCellStyle();

                thjl_record_cellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                thjl_record_cellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                thjl_record_cellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                thjl_record_cellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                //水平对齐  
                thjl_record_cellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Left;
                //垂直对齐  
                thjl_record_cellStyle.VerticalAlignment = VerticalAlignment.Top;
                //自动换行  
                thjl_record_cellStyle.WrapText = true;


                int row_index = 3;

                for (int i = 0; i < j_Thjl_data.Count; i++)
                {
                    JObject j_obj = JObject.Parse(j_Thjl_data[i].ToString());


                    IRow thjl_row = tb.CreateRow(row_index);
                    ICell thjl_name = thjl_row.CreateCell(0);
                    thjl_name.CellStyle = cellStyle_normal;
                    thjl_name.SetCellValue(j_obj["name"].ToString());

                    ICell thjl_standard_value = thjl_row.CreateCell(1);
                    thjl_standard_value.CellStyle = cellStyle_normal;
                    thjl_standard_value.SetCellValue(j_obj["standard_value"].ToString());

                    ICell thjl_exact_value = thjl_row.CreateCell(2);
                    thjl_exact_value.CellStyle = cellStyle_normal;
                    thjl_exact_value.SetCellValue(j_obj["exact_value"].ToString());

                    thjl_row.CreateCell(3).CellStyle = thjl_record_cellStyle;

                    row_index++;

                }

                tb.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(3, row_index - 1, 3, 3));

                ICell thjl_record = tb.GetRow(3).CreateCell(3);
                thjl_record.CellStyle = thjl_record_cellStyle;


                String thjl_detail = "工作要点: " + "\r\n" + work_detail + "\r\n" + "\r\n" + "\r\n";
                thjl_detail = thjl_detail + "设备问题： " + "\r\n";
                for (int i = 0; i < j_Problem_data.Count; i++)
                {
                    JObject j_obj = JObject.Parse(j_Problem_data[i].ToString());
                    Console.WriteLine(j_obj["problem_catalogy"].ToString());
                    Console.WriteLine(j_obj["problem_detail"].ToString());
                    thjl_detail = thjl_detail + (i + 1) + ". " + "问题分类: " +
                        j_obj["problem_catalogy"].ToString() + " ; 问题描述： " + j_obj["problem_detail"].ToString() + "\r\n";
                }

                thjl_record.SetCellValue(thjl_detail);


                IRow last_row = tb.CreateRow(row_index);
                last_row.CreateCell(0).SetCellValue("检查时间：");
                last_row.CreateCell(1).SetCellValue(miss.Miss_Params["Th_CheckTime"].ToString());
                last_row.CreateCell(2).SetCellValue("检查人：");
                last_row.CreateCell(3).SetCellValue(miss.Miss_Params["Th_CheckMen"].ToString());


                string path = Server.MapPath("~/Upload//特护记录表.xls");
                using (FileStream fs = System.IO.File.OpenWrite(path)) //打开一个xls文件，如果没有则自行创建，如果存在myxls.xls文件则在创建是不要打开该文件！
                {
                    wk.Write(fs);   //向打开的这个xls文件中写入mySheet表并保存。
                    Console.WriteLine("提示：创建成功！");
                }
                return Path.Combine("/Upload", "特护记录表.xls");

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return "";
            }


        }

        [HttpPost]
        public JsonResult GetGraphicData(string graphicName,string ser)
        {
            List<double> result = new List<double>();
            List<string> r_time = new List<string>();

            DateTime time_now = DateTime.Now.AddYears(-1);

            DataTable dt = CWFEngine.QueryAllInformation("E.WE_Ser, E.W_Name, M.Event_Name, R.time, P.Th_ItemRecord",
                           "E.W_Name = 'A7dot1dot1' and M.Event_Name = 'ZzSubmit'", "time >= '" + time_now.ToString() + "'");
            DataRow[] NNullRows = dt.Select("Th_ItemRecord is not Null");

            foreach (DataRow r in NNullRows)
            {
                if (r["WE_Ser"].ToString() == ser)
                {
                    string val = r["Th_ItemRecord"].ToString();
                    JArray item = (JArray)JsonConvert.DeserializeObject(val);
                    if (item != null)
                    {
                        foreach (var i in item)
                        {
                            if (i["name"].ToString() == graphicName)
                            {
                                string val1 = i["exact_value"].ToString();
                                r_time.Add(r["time"].ToString());
                                try
                                {
                                    double e_val = Convert.ToDouble(val1);
                                    result.Add(e_val);
                                }
                                catch
                                {
                                    result.Add(0.0);
                                }
                                break;
                            }
                        }
                    }
                }
                
            }
            return Json(new { val = result.ToArray(), valtime = r_time.ToArray() });
        }



        public JsonResult A7Zz_Equips(int Zz_id)
        {
            EquipManagment pm = new EquipManagment();
            List<Equip_Info> Equips_of_Zz = pm.getThEquips_OfZz(Zz_id);
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
    }
}
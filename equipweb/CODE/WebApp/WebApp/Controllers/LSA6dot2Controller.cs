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
using WebApp.Models.DateTables;
using System.Collections.Specialized;
using System.Data;

namespace WebApp.Controllers
{
    public class LSA6dot2Controller : CommonController
    {
        EquipManagment Em = new EquipManagment();

        // GET: /LSA6dot2/
        public ActionResult Index( )
        {
            return View( );
        }
        public ActionResult ZzSubmit(string wfe_id)
        {
            return View(getSubmitModel(wfe_id));
        }

        public ActionResult LSA6dot2(string run_result ,string job_title)
        {
            string s = run_result;
            // ViewBag.flows = CWFEngine.ListAllWFDefine();
            ViewBag.run_result = run_result;
            ViewBag.jobtitle = job_title;
            return View();
        }


        public JsonResult A6dot2GetTimerJobs(string run_result)
        {


            string run_results=   run_result.TrimStart('[').TrimEnd(']');
            List<string> run_enity_id=new List<string>();
            List<object> res = new List<object>();
       

            if (run_results.Contains(','))
             {
                run_enity_id = run_results.Split(new char[] { ',' }).ToList();

              }else
              {
                  run_enity_id.Add(run_results);
              }

            for (int i = 0; i < run_enity_id.Count; i++)
            {
                List<string> Zz_list = new List<string>();//整改方案

                List<string> equip_list = new List<string>();//设备name
                List<string> equip_code_list = new List<string>();//设备code
                List<string> equip_file_list = new List<string>();//整改方案
                List<string> equip_check_result_list = new List<string>();//化验结果

                



                Dictionary<string, object> paras = new Dictionary<string, object>();
                paras["Cj_Name"] = null;
                 paras["Equip_Info"] = null;
                 paras["Assay_Result"] = null;
                paras["Time_Assay"]=null;
                paras["Assay_File"] = null;
                paras["Xc_Confirm"] = null;
                paras["Assay_Result_Again"] = null;

                UI_WFEntity_Info wfei = CWFEngine.GetWorkFlowEntityWithParams(Convert.ToInt16(run_enity_id[i]), paras);
                if (paras["Equip_Info"].ToString() != "")
                {
                    JArray jsonVal = JArray.Parse(paras["Equip_Info"].ToString());
                    dynamic table = jsonVal;
                    foreach (dynamic T in table)
                    {
                        string eq_name = T.equip_name;
                        equip_list.Add(eq_name);
                        string eq_code = T.equip_code;
                        equip_code_list.Add(eq_code);
                        string equip_file = T.equip_file;
                        equip_file_list.Add(equip_file);
                        string equip_check_result=T.equip_check_result;
                        equip_check_result_list.Add(equip_check_result);
                    }
              
                    for (int k = 0; k < equip_list.Count; k++)
                    {
 
                        object o = new
                        {
                            ID = k + 1,
                            zz_name = Em.getZzName(Convert.ToInt16(Em.getEA_id_byCode(equip_code_list[k]))),
                            jz_name = equip_list[k],
                            sb_code = equip_code_list[k],
                            last_check_month = paras["Time_Assay"].ToString(),
                            check_result = equip_check_result_list[k],
                            zz_file = equip_file_list[k],
                            zz_done = paras["Xc_Confirm"].ToString(),
                            second_check_result = paras["Assay_Result_Again"].ToString()

                        };

                        res.Add(o);
                    }
                 
                }
          














            }

       

         

            return Json(new { data = res.ToArray() });
        }



	}
}
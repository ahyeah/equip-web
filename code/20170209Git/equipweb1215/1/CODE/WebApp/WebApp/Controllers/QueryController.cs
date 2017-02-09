using EquipBLL.AdminManagment;
using EquipModel.Entities;
using FlowEngine;
using FlowEngine.UserInterface;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class QueryController : CommonController
    {
        //
        // GET: /Query/
        public class QueryModal
        {
          public  List<UI_WF_Define> wf;
          public List<Equip_Archi> UserHasEquips;

        }
      
        public ActionResult Index()
        {
            QueryModal qm=new QueryModal();
            qm.wf = CWFEngine.ListAllWFDefine();
            PersonManagment pm = new PersonManagment();
            qm.UserHasEquips = pm.Get_Person_Cj((Session["User"] as EquipModel.Entities.Person_Info).Person_Id);
            return View(qm);
        }

        public ActionResult Gxq_Query()
        {
            QueryModal qm = new QueryModal();
            qm.wf = CWFEngine.ListAllWFDefine();
            PersonManagment pm = new PersonManagment();
            qm.UserHasEquips = pm.Get_Person_Cj((Session["User"] as EquipModel.Entities.Person_Info).Person_Id);
            return View(qm);
        }
        public ActionResult WorkFolw_Detail(string wfe_id)
        {
            return View(getWFDetail_Model(wfe_id));
        }



        public string Query_HistoryFlow(string time, string flow_id, string equip_gycode)//string json1)//,)
        {
            // JObject item = (JObject)JsonConvert.DeserializeObject(json1);
            List<object> r = new List<object>();
            if (time != "" || flow_id != "" || equip_gycode != "")
            {
                string Equip_GyCode = equip_gycode;
                string record_filter;

                if (time == "")
                    record_filter = "1<>1";
                else
                {
                    string[] strtime = time.Split(new char[] { '-' });
                    record_filter = "time >= '" + strtime[0] + "00:00:00'" + "and  time <= '" + strtime[1] + " 00:00:00'";
                }
                string Query_list = "distinct E.WE_Ser ,E.WE_Id,R.time  ";
                string Query_condition;
                if ((flow_id == "" && Equip_GyCode == ""))
                    Query_condition = "";
                else
                {
                    if (flow_id != "")
                    {
                        Query_condition = "E.W_Name like '%" + flow_id + "%'";
                        if (equip_gycode != "")
                            Query_condition = Query_condition + "  and P.Equip_Code like '%" + Equip_GyCode + "%'";
                    }
                    else
                        Query_condition = "P.Equip_Code like'%" + Equip_GyCode + "%'";
                }

                System.Data.DataTable dt = CWFEngine.QueryAllInformation(Query_list, Query_condition, record_filter);



                for (var i = 0; i < dt.Rows.Count; i++)
                {
                    UI_MISSION lastMi = CWFEngine.GetHistoryMissions(Convert.ToInt32(dt.Rows[i].ItemArray[1])).Last();
                    int Miss_Id = lastMi.Miss_Id;
                    IDictionary<string, string> rr = CWFEngine.GetMissionRecordInfo(Miss_Id);
                    string userName = "";
                    string missTime = "";
                    if (r.Count > 0)
                    {
                        userName = rr["username"];
                        missTime = rr["time"];
                    }

                    object o = new
                    {
                        index = i,
                        Flow_Name = "<a  onclick=\"DispalsySide('" + dt.Rows[i].ItemArray[0].ToString() + "')\"> " + dt.Rows[i].ItemArray[0].ToString() + "</a>",
                        Mission_Name = CWFEngine.GetWorkFlowEntiy(Convert.ToInt32(dt.Rows[i].ItemArray[1]), true).description,
                        Mission_Time = userName,
                        Mission_User = missTime
                    };
                    r.Add(o);
                }
            }
            string str = JsonConvert.SerializeObject(r);
            return ("{" + "\"data\": " + str + "}");


        }
	}
}
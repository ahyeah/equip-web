using EquipBLL.AdminManagment;
using EquipBLL.AdminManagment.ZyConfig;
using EquipBLL.AdminManagment.MenuConfig;
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
    public class QuickEntranceController : Controller
    {
        //
        // GET: /QuickEntrance/
        public ActionResult Index()
        {
            ViewBag.userName = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
            ViewBag.userId = (Session["User"] as EquipModel.Entities.Person_Info).Person_Id;
            return View(ViewBag);
        }
        public string Add(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                int person_id = Convert.ToInt16(item["user_Id"].ToString());
                int menu_id = Convert.ToInt16(item["menu_Id"].ToString());
                int q_id = Convert.ToInt16(item["q_Id"].ToString());
                string datastring;
                QEntranceManagment qem = new QEntranceManagment();
                bool flag = qem.AddQ_Entrance(person_id, menu_id, q_id);
                if(flag)
                    datastring ="添加成功";
                else
                    datastring = "添加失败";
                object r = new
                {
                    backdata = datastring
                };
                string str = JsonConvert.SerializeObject(r);
                return ("{" + "\"data\": " + str + "}");
            }
            catch
            {
                object r = new
                {
                    backdata = "添加失败"
                };
                string str = JsonConvert.SerializeObject(r);
                return ("{" + "\"data\": " + str + "}");
                
            }
        }

        public JsonResult GetQE_InfobyPerson()
        {
            int p_id = (Session["User"] as EquipModel.Entities.Person_Info).Person_Id;
            List<object> r = new List<object>();
            QEntranceManagment qem = new QEntranceManagment();
            Quick_Entrance[] qe = new Quick_Entrance[8]{null,null,null,null,null,null,null,null};
            List<Quick_Entrance> QE = new List<Quick_Entrance>();
            QE = qem.GetQ_EbyP_Id(p_id);
            foreach (var q in QE)
            {
                qe[q.xuhao - 1] = q;
            }
            for (int i = 0; i < 8;i++)
            {
                if (qe[i] == null)
                {
                    object o = new
                    {
                        menuname = "",
                        menulink = "",
                        qe_id = i+1
                    };
                    r.Add(o);
                }
                else
                {
                    object o = new
                    {
                        menuname = qe[i].Menu.Menu_Name,
                        menulink = qe[i].Menu.Link_Url,
                        qe_id = qe[i].xuhao
                    };
                    r.Add(o);
                }

            }
            return Json(r.ToArray());
        }
	}
}
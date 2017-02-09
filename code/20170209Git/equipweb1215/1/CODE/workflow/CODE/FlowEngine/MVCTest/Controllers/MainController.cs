using FlowEngine;
using FlowEngine.Modals;
using FlowEngine.UserInterface;
using MVCTest.Models;
using MVCTest.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Objects;
using System.Data.Entity.Infrastructure;
using FlowEngine.DAL;

namespace MVCTest.Controllers
{
    public class MainController : Controller
    {
        public class MainModel
        {
            public List<UI_WF_Define> wfs;
            public List<UI_MISSION> miss;
        }
        // GET: Main
        public ActionResult Index()
        {
            MainModel mm = new MainModel();
            mm.wfs = CWFEngine.ListAllWFDefine();
            
            mm.miss = CWFEngine.GetActiveMissions<UserInfo>(((IObjectContextAdapter)(new UserContext())).ObjectContext);
            return View(mm);
        }

        [HttpPost]
        public string CreateFlow(string flowname)
        {
            UI_WorkFlow_Entity wfe = CWFEngine.CreateAWFEntityByName(flowname);
            if (wfe != null)
            {
                Dictionary<string, string> record = wfe.GetRecordItems();

                if (record.ContainsKey("username"))
                    record["username"] = "cb";

                if (record.ContainsKey("time"))
                    record["time"] = DateTime.Now.ToString();

                return wfe.Start(record);
                //Json(new { url = wfe.Start(record), wfe_id = wfe.EntityID }); 
                    //"{url:'" + wfe.Start(record) + "', wfe_id:'" + wfe.EntityID + "'}";

            }
            else
                return null;
        }

        
    }
}
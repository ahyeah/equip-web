using FlowEngine;
using FlowEngine.UserInterface;
using MVCTest.Models;
using MVCTest.Models.User;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCTest.Controllers
{
    public class HolidayController : Controller
    {
        // GET: Holiday
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Application(string wfe_id)
        {
            ViewBag.WF_Name = wfe_id;
            return View();
        }

        public ActionResult submitsignal(string WF_Name, string reason)
        {
            Dictionary<string, string> signal = new Dictionary<string,string>();
            signal["application_reason"] = reason;
            signal["application_person"] = (Session["User"] as UserInfo).name;
            signal["application_done"] = "true";
            CWFEngine.SubmitSignal(Convert.ToInt32(WF_Name), signal);

            return Redirect("/Main/Index");
        }

        public ActionResult Approve(int wfe_id)
        {
            UI_MISSION mi = CWFEngine.GetActiveMission<UserInfo>(wfe_id, ((IObjectContextAdapter)(new UserContext())).ObjectContext);
            string str = mi.Miss_Params["application_person"].ToString();
            ViewBag.poster = str;
            str = mi.Miss_Params["application_reason"].ToString();
            ViewBag.reason = str;
            ViewBag.wfid = mi.WE_Entity_Id;
            return View();
        }

        public ActionResult submitapprove(string WFE_Id, string appr_result)
        {
            Dictionary<string, string> signal = new Dictionary<string, string>();
            signal["approve_result"] = appr_result;

            CWFEngine.SubmitSignal(Convert.ToInt32(WFE_Id), signal);

            return Redirect("/Main/Index");
        }
    }
}
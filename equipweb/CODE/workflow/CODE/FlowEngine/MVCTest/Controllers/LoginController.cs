using FlowEngine;
using MVCTest.Models;
using MVCTest.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCTest.Controllers
{
    public class LoginController : Controller
    {
        
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LogOn(string userName, string passWord)
        {
            using(var db = new UserContext())
            {
                UserInfo ui = db.users.Where(s => s.name == userName & s.password == passWord).First();
                
                if (ui == null)
                    return Redirect("Index");
                else
                {
                    CWFEngine.authority_params["currentuser"] = userName;
                    Session["User"] = ui;
                    return Redirect("/Main/Index");
                }
            }
        }
    }
}
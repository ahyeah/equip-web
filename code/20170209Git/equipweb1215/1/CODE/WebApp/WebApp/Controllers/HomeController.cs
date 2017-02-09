using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EquipBLL.AdminManagment.MenuConfig;
using EquipBLL.AdminManagment;
using EquipModel.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using FlowEngine;
using FlowEngine.UserInterface;
using EquipModel.Context;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;




namespace WebApp.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }
    }
}
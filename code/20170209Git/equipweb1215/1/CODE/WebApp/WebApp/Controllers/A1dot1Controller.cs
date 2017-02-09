using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Security.AccessControl;
using System.Text;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Collections;

using EquipBLL.FileManagment;
using Newtonsoft.Json;

namespace WebApp.Controllers
{
    public class A1dot1Controller : Controller
    {
        //
        // GET: /A1dot1/
        public ActionResult Index()
        {
            return View();
        }
	}
}
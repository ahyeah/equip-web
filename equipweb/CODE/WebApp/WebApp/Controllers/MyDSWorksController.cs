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

namespace WebApp.Controllers
{
    public class MyDSWorksController : Controller
    {
        public class DSIndex_Model
        {

            public int Month_Num;
            public List<string> title;

        }
        //
        // GET: /MyDSWorks/
        public ActionResult Index()
        {
            return View(getIndex());
        }

        public DSIndex_Model getIndex()
        {
            DSIndex_Model Ds = new DSIndex_Model();
            int month_num = DateTime.Now.Month;
            List<string> titlename = new List<string>();
            Ds.Month_Num = month_num;
            for (int i = 1; i < month_num + 1; i++)
            {
               titlename.Add(i.ToString() + "月完成情况");
            }
            Ds.title = titlename;
            return Ds;
        }
	}
}
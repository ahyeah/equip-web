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

namespace WebApp.Controllers
{
    public class PersonAndWorkflowsController : CommonController
    {



        public ActionResult Index()
        {
            return View();
        }

        public string submitsignal(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                PersonManagment pm = new PersonManagment();
                string workflow = item["WorkFlow"].ToString();
                string[] member = item["Member"].ToString().Split(new char[] { ',' });
                for (int i = 0; i < member.Length; i++)
                {
                    int pid = pm.GetPersonId(member[i]);
                    int mid = pm.GetMenuId(workflow);
                    bool res = pm.AddPW(pid, mid);
                }

                //bool ress = pm.AddPW(Convert.ToInt32(item["personid"].ToString()), Convert.ToInt32(item["menuid"].ToString()));

            }
            catch (Exception e)
            {
                return "";
            }
            return ("/PersonAndWorkflows/Index");
        }

    }
}

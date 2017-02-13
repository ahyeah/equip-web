using FlowEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Models;
using WebApp.Models.User;
using EquipBLL.AdminManagment;
using EquipModel.Entities;
using System.Security.Cryptography;
using System.Text;

namespace WebApp.Controllers
{
    public class LoginController : Controller
    {
        //
        // GET: /Login/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult chaoshi()
        {
            return View();
        }
        public ActionResult VerCheck()
        {
            return View();
        }
        [HttpPost]
        public string LogOn(string userName, string userPwd)
        {

            /* PersonManagment pm = new PersonManagment();
             Person_Info p = pm.Get_Person(userName);
             if (p == null)
                 return "Index";

             else
             {
                 CWFEngine.authority_params["currentuser"] = userName;
                 Session["User"] = p;
                 return "/Main/Index0";
             }*/


            //GetNWorkSerManagment g = new GetNWorkSerManagment();
            //g.AddNWorkEntity("A11dot3", "20170200001");
            int a;
            PersonManagment pm = new PersonManagment();
            MD5 md5 = new MD5CryptoServiceProvider();

            byte[] result = md5.ComputeHash(System.Text.Encoding.Default.GetBytes(userPwd.Trim()));
                           // pm.ModifyPerson_Pwd(userName, result);
            Person_Info p = pm.Get_Person(userName);
            if (p == null)
            {
                return "Index";
            }
            else
            {
                byte[] r = p.Person_Pwd;
                string userPwdMD5 = System.Text.UTF8Encoding.Unicode.GetString(result);
                string userPwDb = System.Text.UTF8Encoding.Unicode.GetString(r);
               if (!userPwDb.Equals(userPwdMD5))
                    return "Index";
                //// string =System.Text.Encoding.Default.GetString(result);



               else
               {

            Dictionary<string, object> dict_authority = new Dictionary<string, object>();
            dict_authority["currentuser"] = userName;

            Session["authority"] = dict_authority;

            CWFEngine.authority_params = "authority";

            Session["User"] = p;
            return "/Main/Index0";
               }
            }



        }

        public ActionResult ModifyPWD_Index()
        {
            ViewBag.userName = (Session["User"] as EquipModel.Entities.Person_Info).Person_Name;
            return View();
        }
        public string ModifyPWD(string userName, string userPwd, string newUserPWd)
        {
            int a;
            PersonManagment pm = new PersonManagment();
            MD5 md5 = new MD5CryptoServiceProvider();

            byte[] result = md5.ComputeHash(System.Text.Encoding.Default.GetBytes(userPwd.Trim()));
            //pm.ModifyPerson_Pwd(userName, result);
            Person_Info p = pm.Get_Person(userName);
            if (p == null)
            {
                return "ModifyPWD_Index";
            }
            else
            {
                byte[] r = p.Person_Pwd;
                string userPwdMD5 = System.Text.UTF8Encoding.Unicode.GetString(result);
                string userPwDb = System.Text.UTF8Encoding.Unicode.GetString(r);
                if (!userPwDb.Equals(userPwdMD5))
                    return "ModifyPWD_Index";
                // string =System.Text.Encoding.Default.GetString(result);



                else
                {
                    result = md5.ComputeHash(System.Text.Encoding.Default.GetBytes(newUserPWd.Trim()));
                    if (pm.ModifyPerson_Pwd(userName, result))

                        return "/Main/Home";
                    else
                        return "ModifyPWD_Index";
                }
            }

        }


       
    }
}
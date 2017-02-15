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
using EquipBLL.AdminManagment.ZyConfig;
using System.Security.Cryptography;
using System.Text;



namespace WebApp.Controllers
{
    public class UserManagerController : Controller
    {
        private SysMenuManagment menuconfig = new SysMenuManagment();
       
        //
        // GET: /UserManager/
        public ActionResult Index()
        {  PersonManagment PM=new PersonManagment();
             List<Person_Info> r = PM.Get_All_basePersonInfo();
            return View(r);
        }
        public ActionResult AddUser()
        {
            BaseDataManagment DM = new BaseDataManagment();
            List<Role_Info> R_obj = DM.GetAllRoles();
            return View(R_obj);
        }

        public ActionResult UpdateUser(int  userId)
        {
            PersonManagment PM = new PersonManagment();
            EquipBLL.AdminManagment.PersonManagment.P_viewModal pm_info = PM.Get_PersonModal(userId);
            return View(pm_info);
        }
        public JsonResult List_Roles(string Role_selected)
        {
             PersonManagment PM = new PersonManagment();

             List<EquipBLL.AdminManagment.PersonManagment.Role_viewModal> role_obj = PM.Get_All_Roles(Role_selected);
            return Json(role_obj.ToArray());

        }
        public JsonResult List_Depart()
        {

            BaseDataManagment DM = new BaseDataManagment();

            List<TreeListNode> Depart_obj = DM.BuildDepartList();
            return Json(Depart_obj.ToArray());



        }
        public JsonResult List_EquipArchi()
        {

            BaseDataManagment DM = new BaseDataManagment();

            List<TreeListNode> EquipArchi_obj = DM.BuildEquipArchiList();
            return Json(EquipArchi_obj.ToArray());



        }
        

        public JsonResult List_Speciaties()
        {
            BaseDataManagment DM = new BaseDataManagment();
            List<TreeListNode> Speciaty_obj = DM.BuildSpeciatyList();
            return Json(Speciaty_obj.ToArray());
        }

        public JsonResult List_Menus(string Role_Ids)
        {
            BaseDataManagment DM = new BaseDataManagment();
            List<Menu> Menu_obj = DM.GetRole_Menus(Role_Ids);
            List<object> r = new List<object>();
            foreach (Menu item in Menu_obj)
            {
                object o = new
                {
                    Menu_Id = item.Menu_Id,
                    Menu_Name = item.Menu_Name,

                };
                r.Add(o);

            }

            return Json(r.ToArray());

        }

        public JsonResult List_MenuTree(string Role_Ids)
        {
            BaseDataManagment DM = new BaseDataManagment();

            List<TreeListNode> Menu_obj = DM.BuildMenuList(Role_Ids);
            return Json(Menu_obj.ToArray());


        }

        public bool submitDeleteUser(int userId)
        {
            try
            {
                PersonManagment PM = new PersonManagment();
                PM.Delete_Person(userId);
                return true;
            }
            catch
            { return false; }
        }
        public string submitUpdateUser(string json1)
        {
            try
            {
                SpeciatyManagment SM = new SpeciatyManagment();
                PersonManagment PM=new PersonManagment();
                Person_Info newP=new Person_Info();
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                newP.Person_Id = Convert.ToInt32(item["UserId"].ToString());
                newP.Person_Name= item["UserName"].ToString();
                newP.Person_tel= item["UserTel"].ToString();
                newP.Person_mail=item["UserMail"].ToString();
                var roleStr = item["UserRole"].ToString();
                var speciatyStr = item["UserSpeciatySel"].ToString();
                var MenuStr = item["UserMenus"].ToString();
                var equipArchiStr = item["UserEquipArchiSel"].ToString();

                int Depart_id = Convert.ToInt32(item["UserDepartId"].ToString());
                List<int> EquipArchiList = new List<int>();
                if (equipArchiStr != "")
                {
                    string[] s = equipArchiStr.Split(new char[] { ',' });
                    for (int i = 0; i < s.Length; i++)
                    { EquipArchiList.Add(Convert.ToInt32(s[i])); }
                }


               /* List<int> SpeciatyList = new List<int>();
                if (speciatyStr != "")
                {
                    string[] s = speciatyStr.Split(new char[] { ',' });
                    for (int i = 0; i < s.Length; i++)
                    { SpeciatyList.Add(Convert.ToInt32(s[i])); }
                }*/
                List<int> SpeciatyList = new List<int>();
                if (speciatyStr != "")
                {
                    string[] s = speciatyStr.Split(new char[] { ',' });
                    for (int i = 0; i < s.Length; i++)
                    {
                        int id = Convert.ToInt16(s[i]);
                        List<Speciaty_Info> r = SM.GetChildsspecialty(id);
                        if (r == null)
                        { if (SpeciatyList.Where(x => x == id).Count() == 0) SpeciatyList.Add(Convert.ToInt32(s[i])); }
                        else
                        {
                            if (SpeciatyList.Where(x => x == id).Count() == 0) SpeciatyList.Add(Convert.ToInt32(s[i]));
                            foreach (var t in r)
                            {
                                if (SpeciatyList.Where(x => x == t.Specialty_Id).Count() == 0) SpeciatyList.Add(Convert.ToInt32(t.Specialty_Id));
                            }


                        }



                    }
                }

                List<int> MenuList = new List<int>();
                if (MenuStr != "")
                {
                    string[] s1 = MenuStr.Split(new char[] { ',' });
                    for (int i = 0; i < s1.Length; i++)
                    { MenuList.Add(Convert.ToInt32(s1[i])); }
                }
               
                    List<int> RoleList = new List<int>();
                    if (roleStr != "")
                    {
                    string[] s2 = roleStr.Split(new char[] { ',' });
                    for (int i = 0; i < s2.Length; i++)
                    { RoleList.Add(Convert.ToInt32(s2[i])); }
                }

                    PM.Update_Person(newP, Depart_id, RoleList, EquipArchiList,SpeciatyList, MenuList);  //保存基础信息

                    return "用户信息修改成功！";
            }
            catch { return ""; };
	 }
       
        public string submitNewUser(string json1)
        {
        
            try
            {
                SpeciatyManagment SM = new SpeciatyManagment();
                PersonManagment PM=new PersonManagment();
                Person_Info newP=new Person_Info();
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                newP.Person_Name= item["UserName"].ToString();

                /*0710添加*/
                var pwd = item["UserPwd"].ToString();
                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] result = md5.ComputeHash(System.Text.Encoding.Default.GetBytes(pwd.Trim()));
                newP.Person_Pwd = result;
                /*0710end*/

                newP.Person_tel= item["UserTel"].ToString();
                newP.Person_mail=item["UserMail"].ToString();
                var equipArchiStr=item["UserEquipArchiSel"].ToString();
                var roleStr = item["UserRole"].ToString();
                var speciatyStr = item["UserSpeciatySel"].ToString();
                var MenuStr = item["UserMenus"].ToString();

                int Depart_id = Convert.ToInt32(item["UserDepartId"].ToString());

                List<int> EquipArchiList = new List<int>();
                if (equipArchiStr != "")
                {
                    string[] s = equipArchiStr.Split(new char[] { ',' });
                    for (int i = 0; i < s.Length; i++)
                    { EquipArchiList.Add(Convert.ToInt32(s[i])); }
                }

               /* List<int> SpeciatyList = new List<int>();
                if (speciatyStr != "")
                {
                    string[] s = speciatyStr.Split(new char[] { ',' });
                    for (int i = 0; i < s.Length; i++)
                    { SpeciatyList.Add(Convert.ToInt32(s[i])); }
                }*/
                List<int> SpeciatyList = new List<int>();
                if (speciatyStr != "")
                {
                    string[] s = speciatyStr.Split(new char[] { ',' });
                    for (int i = 0; i < s.Length; i++)
                    {
                        int id = Convert.ToInt16(s[i]);
                        List<Speciaty_Info> r = SM.GetChildsspecialty(id);
                        if(r==null)
                        { if (SpeciatyList.Where(x=>x==id).Count()==0) SpeciatyList.Add(Convert.ToInt32(s[i]));}
                        else
                        {
                            if (SpeciatyList.Where(x => x == id).Count() == 0) SpeciatyList.Add(Convert.ToInt32(s[i]));
                            foreach(var t in r)
                            {
                                if (SpeciatyList.Where(x =>x== t.Specialty_Id).Count() == 0) SpeciatyList.Add(Convert.ToInt32(t.Specialty_Id));
                            }


                        }
                       
                    
                    
                    }
                }


                List<int> MenuList = new List<int>();
                if (MenuStr != "")
                {
                    string[] s1 = MenuStr.Split(new char[] { ',' });
                    for (int i = 0; i < s1.Length; i++)
                    { MenuList.Add(Convert.ToInt32(s1[i])); }
                }
                List<int> RoleList = new List<int>();
                if (roleStr != "")
                {
                    string[] s2 = roleStr.Split(new char[] { ',' });
                    for (int i = 0; i < s2.Length; i++)
                    { RoleList.Add(Convert.ToInt32(s2[i])); }
                }
                PM.Add_Person(newP, Depart_id, RoleList, EquipArchiList,SpeciatyList, MenuList);  //保存基础信息

                    return "保存成功！";
            }
            catch { return ""; };
	}
    }
}
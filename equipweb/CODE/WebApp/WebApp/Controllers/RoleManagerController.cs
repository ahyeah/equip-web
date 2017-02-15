using EquipBLL.AdminManagment;
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
    public class RoleManagerController : Controller
    {
        //
        // GET: /Query/
        private SysMenuManagment menuconfig = new SysMenuManagment();
      
        public ActionResult Index()
        {
         //   RoleManagment RM = new RoleManagment();
         //   List<Role_Info> r = RM.get_ALl_Roles();
            return View();
        }

       //  新增角色信息时获取权限树
        public JsonResult List_MenuTree()
        {
            RoleManagment RM = new RoleManagment();

            List<TreeListNode> Menu_obj = RM.BuildMenuList();
            return Json(Menu_obj.ToArray());


        }

        //   修改角色信息的时候获取树
        public JsonResult List_R_MenuTree(string Role_Ids)   
        {
            BaseDataManagment DM = new BaseDataManagment();

            List<TreeListNode> Menu_obj = DM.BuildMenuList(Role_Ids);
            return Json(Menu_obj.ToArray());
        }


        // 新加角色时向数据库提交信息信息
        public string submitNewRole(string json1)
        {

            try
            {
                RoleManagment PM = new RoleManagment();
                Role_Info newR = new Role_Info();
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);


                newR.Role_Name = item["Role_Name"].ToString();
                newR.Role_Desc = item["Role_Desc"].ToString();

                var MenuStr = item["RoleMenus"].ToString();


                List<int> MenuList = new List<int>();
                if (MenuStr != "")
                {
                    string[] s1 = MenuStr.Split(new char[] { ',' });
                    for (int i = 0; i < s1.Length; i++)
                    { MenuList.Add(Convert.ToInt32(s1[i])); }
                }

                PM.Add_Role(newR, MenuList);  //保存基础信息

                return "保存成功！";
            }
            catch { return ""; };
        }
        //    修改后的角色向数据库提交数据
        public string submitUpdateRole(string json1)
        {
            try
            {
                RoleManagment PM = new RoleManagment();
                Role_Info newR = new Role_Info();
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                newR.Role_Id = Convert.ToInt32(item["Role_Id"].ToString()); //将string型转换成int型
                newR.Role_Name = item["Role_Name"].ToString();
                newR.Role_Desc = item["Role_Desc"].ToString();


                var MenuStr = item["RoleMenus"].ToString();
                List<int> MenuList = new List<int>();
                if (MenuStr != "")
                {
                    string[] s1 = MenuStr.Split(new char[] { ',' });
                    for (int i = 0; i < s1.Length; i++)
                    { MenuList.Add(Convert.ToInt32(s1[i])); }
                }


                PM.Update_Role(newR, MenuList);  //保存基础信息

                return "用户信息修改成功！";
            }
            catch { return ""; };
        }



        //   显示历史角色信息
        public string Role_History()
        {
            
           
          //  JObject item = (JObject)JsonConvert.DeserializeObject(Role_Id);
            RoleManagment RM = new RoleManagment();
            List<Role_Info> R = RM.get_ALl_Roles();

           List<object> r = new List<object>();
  
            
            for(var i=0;i<R.Count;i++)
            { 
                string m = null;
               
                EquipBLL.AdminManagment.RoleManagment.Role_viewModal M_info = RM.Get_RoleModal(R[i].Role_Id);
                if (M_info.Menus.Count > 0)
                {
                    m = M_info.Menu_Names;
                }
                object o = new
                {
                  index = i+1,
                  Role_Id=R[i].Role_Id.ToString(),
                  Role_Name = R[i].Role_Name.ToString(),
                  Role_Desc=R[i].Role_Desc.ToString(),
                 // Role_Menus=R[i].Role_Menus.ToString(),               
                //    Role_Menu = RM.Get_RoleModal(R[i].Role_Id).Menus,
              
            
                   Role_Menus=m,
                  mod_Role = R[i].Role_Id.ToString(),
                  Role_Id_del = R[i].Role_Id.ToString(),


                };
                r.Add(o);
            }
            
            string str = JsonConvert.SerializeObject(r);
            return ("{" + "\"data\": " + str + "}");

        
        }


        //  删除角色
        public string delete_Role(string json1)
        {
            try
            {
                
                RoleManagment RM = new RoleManagment();
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                int Role_Id = Convert.ToInt32(item["Role_Id"].ToString()); //将string型转换成int型
                bool del = RM.Delete_Role(Role_Id);
                if (del)
                    return "删除成功！";
                else
                    return "删除失败！";
            }
            catch
            { return ""; };
        }



	}
}
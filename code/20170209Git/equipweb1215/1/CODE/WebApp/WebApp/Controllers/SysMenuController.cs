using EquipBLL.AdminManagment.MenuConfig;
using EquipBLL.AdminManagment;
using EquipModel.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;

namespace WebApp.Controllers
{
    public class SysMenuController : Controller
    {        
        private SysMenuManagment menuconfig = new SysMenuManagment();
        // GET: SysMenu
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public string test()
        {
            List<MenuListNode> mt = menuconfig.BuildMenuList();
            string str = JsonConvert.SerializeObject(mt);
            return "{" +
                 "\"data\": " + str +
                 "}";

        }

        [HttpPost]
        public JsonResult Modifyitem(string json1)
        {
            JObject item = (JObject)JsonConvert.DeserializeObject(json1);
            Menu new_menu = new Menu();
            new_menu.Menu_Id = Convert.ToInt32(item["Menu_Id"].ToString());
            new_menu.Menu_Name = item["Menu_Name"].ToString();
            new_menu.Menu_Icon = item["Menu_Icon"].ToString();
            new_menu.Link_Url = item["Link_Url"].ToString();

            bool res = menuconfig.ModifyMenuItem(new_menu);

            return Json(new { result = res });            
        }

        [HttpPost]
        public JsonResult Additem(string json1)
        {
            JObject item = (JObject)JsonConvert.DeserializeObject(json1);
            Menu new_menu = new Menu();
            new_menu.Menu_Id = Convert.ToInt32(item["Menu_Id"].ToString());
            new_menu.Menu_Name = item["Menu_Name"].ToString();
            new_menu.Menu_Icon = item["Menu_Icon"].ToString();
            new_menu.Link_Url = item["Link_Url"].ToString();

            bool res = menuconfig.AddMenuItem(Convert.ToInt32(item["Parent_id"].ToString()), new_menu);

            return Json(new { result = res }); 
        }

        [HttpPost]
        public JsonResult Deleteitem(string del)
        {       

            bool res = menuconfig.DeleteMenuItem(Convert.ToInt32(del));

            return Json(new { result = res });
        }
    }
}
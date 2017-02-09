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
    public class EquipArchiController : Controller
    {
        private EquipArchiManagment menuconfig = new EquipArchiManagment();
        // GET: SysMenu
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public string test()
        {
            List<MenuListNode1> mt = menuconfig.BuildMenuList();
            string str = JsonConvert.SerializeObject(mt);
            return "{" +
                 "\"data\": " + str +
                 "}";

        }

        [HttpPost]
        public JsonResult Modifyitem(string json1)
        {
            JObject item = (JObject)JsonConvert.DeserializeObject(json1);
            Equip_Archi new_menu = new Equip_Archi();
            new_menu.EA_Id = Convert.ToInt32(item["EA_Id"].ToString());
            new_menu.EA_Name = item["EA_Name"].ToString();
            new_menu.EA_Code = item["EA_Code"].ToString();
            new_menu.EA_Title = item["EA_Title"].ToString();

            bool res = menuconfig.ModifyEquipArchiItem(new_menu);

            return Json(new { result = res });
        }

        [HttpPost]
        public JsonResult Additem(string json1)
        {
            JObject item = (JObject)JsonConvert.DeserializeObject(json1);
            Equip_Archi new_menu = new Equip_Archi();
            new_menu.EA_Id = Convert.ToInt32(item["EA_Id"].ToString());
            new_menu.EA_Name = item["EA_Name"].ToString();
            new_menu.EA_Code = item["EA_Code"].ToString();
            new_menu.EA_Title = item["EA_Title"].ToString();

            bool res = menuconfig.AddEquipArchiItem(Convert.ToInt32(item["Parent_id"].ToString()), new_menu);

            return Json(new { result = res });
        }

        [HttpPost]
        public JsonResult Deleteitem(string del)
        {

            bool res = menuconfig.DeleteEquipArchiItem(Convert.ToInt32(del));

            return Json(new { result = res });
        }
    }
}
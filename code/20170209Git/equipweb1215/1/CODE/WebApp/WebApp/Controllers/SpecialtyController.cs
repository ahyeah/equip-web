using EquipBLL.AdminManagment.ZyConfig;
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
    public class SpecialtyController : Controller
    {
        private SpeciatyManagment Zyconfig = new SpeciatyManagment();
        // GET: Speciaty
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public string test()
        {
            List<SpecialtyListNode> mt = Zyconfig.BuildSpeciatyList();
            string str = JsonConvert.SerializeObject(mt);
            return "{" +
                 "\"data\": " + str +
                 "}";

        }

        [HttpPost]
        public JsonResult Modifyitem(string json1)
        {
            JObject item = (JObject)JsonConvert.DeserializeObject(json1);
            Speciaty_Info new_specialty = new Speciaty_Info();
            new_specialty.Specialty_Id = Convert.ToInt32(item["Specialty_Id"].ToString());
            new_specialty.Specialty_Name = item["Specialty_Name"].ToString();


            bool res = Zyconfig.ModifySpecialtyItem(new_specialty);

            return Json(new { result = res });
        }

        [HttpPost]
        public JsonResult Additem(string json1)
        {
            JObject item = (JObject)JsonConvert.DeserializeObject(json1);
            Speciaty_Info new_specialty = new Speciaty_Info();
            new_specialty.Specialty_Id = Convert.ToInt32(item["Specialty_Id"].ToString());
            new_specialty.Specialty_Name = item["Specialty_Name"].ToString();


            bool res = Zyconfig.AddSpecialtyItem(Convert.ToInt32(item["Parent_id"].ToString()), new_specialty);

            return Json(new { result = res });
        }

        [HttpPost]
        public JsonResult Deleteitem(string del)
        {

            bool res = Zyconfig.DeleteSpecialtyItem(Convert.ToInt32(del));

            return Json(new { result = res });
        }
    }
}
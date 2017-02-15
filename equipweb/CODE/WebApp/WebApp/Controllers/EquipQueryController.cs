using EquipBLL.AdminManagment;
using EquipBLL.AdminManagment.ZyConfig;
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
    public class EquipQueryController : Controller
    {
        //
        // GET: /EquipQuery/
        public class QueryModal
        {
            public List<UI_WF_Define> wf;
            public List<Equip_Archi> UserHasEquips;
            public List<Speciaty_Info> sps;
        }
        public ActionResult AddEquip()
        {
            QueryModal qm = new QueryModal();
            SpeciatyManagment spm = new SpeciatyManagment();
            qm.sps = spm.getsps();
            return View(qm);
        }
        //返回设备专业子类
        public JsonResult EquipSp_Info(int sp_id)
        {
            SpeciatyManagment sm = new SpeciatyManagment();
            List<Speciaty_Info> sp = sm.getsps_child(sp_id);
            List<object> sp_obj = new List<object>();
            foreach (var item in sp)
            {
                object o = new
                {
                   sp_Id = item.Specialty_Id,
                   sp_Name = item.Specialty_Name
                };
                sp_obj.Add(o);

            }
            return Json(sp_obj.ToArray());

        }
        public ActionResult EquipQuery()
        {
            QueryModal qm = new QueryModal();
            qm.wf = CWFEngine.ListAllWFDefine();
            PersonManagment pm = new PersonManagment();
            qm.UserHasEquips = pm.Get_Person_Cj((Session["User"] as EquipModel.Entities.Person_Info).Person_Id);
            return View(qm);
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
        //功能：根据查询信息查询设备
        //参数：设备工艺编号，设备位置，设备专业分类
        //返回：数据拼接string
        public string Query_Equip(string equip_gycode, int equiparchi_id, string equip_specialty)//string json1)//,)
        {
            List<object> r = new List<object>();
            List<Equip_Info> e = new List<Equip_Info>();

            if (equip_gycode != "" || equiparchi_id != 0 || equip_specialty != "")
            {
                EquipManagment EM = new EquipManagment();
                e = EM.getAllEquips_byinfo(equip_gycode, equiparchi_id, equip_specialty);

                for (var i = 0; i < e.Count; i++)
                {   
                    //数据库中可能为空这些数据做些处理在赋值给o
                string ecode;
                string manuf;
                if (e[i].Equip_Code == null)
                    ecode = "无";
                else
                {
                    ecode = e[i].Equip_Code;
                }
                if (e[i].Equip_Manufacturer == null)
                    manuf = "无";
                else
                {
                    manuf = e[i].Equip_Manufacturer;
                }
                    //返回前台显示的数据
                    object o = new
                    {
                        index = i+1,
                        equip_gycode = e[i].Equip_GyCode.ToString(),
                        equip_code = ecode,
                        equip_specialty = e[i].Equip_Specialty.ToString(),
                        equip_phaseB = e[i].Equip_PhaseB.ToString(),
                        equip_manufacturer = manuf
                    };
                    r.Add(o);
                }
            }
            string str = JsonConvert.SerializeObject(r);
            return ("{" + "\"data\": " + str + "}");


        }
     

        //获得点击修改的设备的具体信息
        public JsonResult List_Equipinfo(string json1)
        {
            
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                //取得设备编号    
                string e_code = item["equip_code"].ToString();
                EquipManagment EM = new EquipManagment();
                int EA_id = EM.getEA_id_byCode(e_code);
                EquipArchiManagment EAM=new EquipArchiManagment();
                
                Equip_Info mod_equip = new Equip_Info();
                mod_equip = EM.getEquip_Info(e_code);
                
                object mod = new 
                {
                    e_abc=mod_equip.Equip_ABCmark,
                    e_code=mod_equip.Equip_Code,
                    e_gycode=mod_equip.Equip_GyCode,
                    e_man=mod_equip.Equip_Manufacturer,
                    e_phaseB=mod_equip.Equip_PhaseB,
                    e_sp=mod_equip.Equip_Specialty,
                    e_type=mod_equip.Equip_Type,
                    e_Achi=EAM.getEa_namebyId(EA_id)
                };
                return Json(mod);
            
        }
        //添加新设备
        public string submitNewEquip(string json1)
        {

            try
            {
                EquipManagment EM = new EquipManagment();
                Equip_Info new_equip = new Equip_Info();
                int Zz_Id;
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                Zz_Id = Convert.ToInt16(item["Equip_Archi_Id"]);
                new_equip.Equip_ABCmark = item["EquipABCMark"].ToString();
                new_equip.Equip_GyCode = item["EquipName"].ToString();
                new_equip.Equip_Code = item["EquipCode"].ToString();
                new_equip.Equip_Type = item["EquipType"].ToString();
                new_equip.Equip_Specialty = item["EquipSpecialty"].ToString();
                new_equip.Equip_PhaseB = item["EquipPhaseB"].ToString();
                new_equip.Equip_Manufacturer = item["EquipManufacturer"].ToString();
                EM.addEquip(new_equip,Zz_Id);
                return "保存成功！";
            }
            catch { return ""; };
        }


        //删除设备
        public string delete_equip(string json1)
        {
            try
            {
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                EquipManagment EM = new EquipManagment();
                string equip_id =item["equip_id"].ToString();
                int E_id = EM.getE_id_byGybh(equip_id);
                bool del= EM.deleteEquip(E_id);
                if(del)
                    return "删除成功！";
                else
                    return "删除失败！";
            }
            catch { return "error"; }
        }

        //修改设备
        public string modifyEquip(string json1)
        {

            try
            {
                EquipManagment EM = new EquipManagment();
                Equip_Info new_equip = new Equip_Info();
                string Ea_name;
                JObject item = (JObject)JsonConvert.DeserializeObject(json1);
                Ea_name = item["Equip_Archi"].ToString();
                EquipArchiManagment EAM = new EquipArchiManagment();
                int Zz_Id = EAM.getEa_idbyname(Ea_name);
                new_equip.Equip_ABCmark = item["EquipABCMark"].ToString();
                new_equip.Equip_GyCode = item["EquipName"].ToString();
                new_equip.Equip_Code = item["EquipCode"].ToString();
                new_equip.Equip_Type = item["EquipType"].ToString();
                new_equip.Equip_Specialty = item["EquipSpecialty"].ToString();
                new_equip.Equip_PhaseB = item["EquipPhaseB"].ToString();
                new_equip.Equip_Manufacturer = item["EquipManufacturer"].ToString();
                if(EM.modifyEquip(new_equip,Zz_Id))
                return "保存成功！";
                else
                    return "保存失败！";
            }
            catch { return ""; };
        }

	}
}
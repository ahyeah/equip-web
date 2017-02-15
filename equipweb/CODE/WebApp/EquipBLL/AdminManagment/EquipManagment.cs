using EquipDAL.Implement;
using EquipModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipBLL.AdminManagment
{
   public  class EquipManagment
   {
       
       private Equip_Archis EAs = new Equip_Archis();
       private Equips Es=new Equips();
       private Depart_Archis Ds = new Depart_Archis();
       //功能：获取所有的车间信息
       //参数：空
       //返回值：List<Equip_Archi>
       public List<Equip_Archi> getAllCj()
       {
           List<Equip_Archi> r=EAs.getEAs_isCj();
           return r;
       }
       //功能：查询设备列表
       //参数：设备工艺编号，位置id，设备子类
       //返回值：List<Equip_Archi>
       public List<Equip_Info> getAllEquips_byinfo(string Equip_gycode, int Equip_archi, string Equip_specialty)
       {
           List<Equip_Info> r = Es.getEquips_byinfo(Equip_gycode, Equip_archi, Equip_specialty);
           return r;
       }
       //开始0801
       public string getCjnamebyEa_id(int id)
       {
           return EAs.getEA_Parent(id).EA_Name;
       }
       //结束
       public string getPq(string Cjname)
       {
           return Ds.getDepart_Pq(Cjname);
       }
       public int getE_id_byGybh(string equip_Gy)
       {
           int r = Es.getE_id_byGybh(equip_Gy);
           return r;
       }
       public int getEA_id_byCode(string equip_code)
       {
           int r = Es.getEA_id_byCode(equip_code);
           return r;
       }

       //功能：获取某车间的所有装置信息
       //参数：Cj_id 整型 车间Id
       //返回值：List<Equip_Archi>
       public List<Equip_Archi> getZzs_ofCj(int Cj_id)
       {
           List<Equip_Archi> r = EAs.getEA_Childs(Cj_id);
           return r;
       }
       //功能：获取某装置的所有设备信息
       //参数：Zz_Id 整型 装置Id
       //返回值：List<Equip_Info>
       public List<Equip_Info> getEquips_OfZz(int Zz_Id, string username = null)
       {
           List<Equip_Info> r = EAs.getEA_Equips(Zz_Id, username);
           return r;
       }
      public List<Equip_Info> getThEquips_OfZz(int Zz_Id)
      {
          List<Equip_Info> r = EAs.getThEA_Equips(Zz_Id);
          return r;
      }
      //功能：获取某设备的详细信息
      //参数： Equip_Id 整型 设备Id
      //返回值：Equip_Info
       public Equip_Info getEquip_Info(int Equip_Id)
       {
           Equip_Info e=Es.getEquip_Info(Equip_Id);
           return e;
       }


       //功能：获取某设备的详细信息   --xwm add
       //参数： Equip_Code, string 设备编码
       //返回值：Equip_Info
       public Equip_Info getEquip_Info(string Equip_Code)
       {
           Equip_Info e = Es.getEquip_Info(Equip_Code);
           return e;
       }

       public Equip_Info getEquip_ByGyCode(string Equip_GyCode)
       {
           Equip_Info e = Es.getEquip_byGyCode(Equip_GyCode);
           return e;
       }
       public List<Equip_Archi> getEquip_ZzBelong(int Equip_Id)
       {
           List<Equip_Archi> E_ZzCjInfo = new List<Equip_Archi>();
           Equip_Archi m = Es.getEquip_EA(Equip_Id);
           E_ZzCjInfo.Add(m);
           E_ZzCjInfo.Add(EAs.getEA_Parent(m.EA_Id));

           return E_ZzCjInfo;
       }

        //功能：添加设备信息
       //参数： new_Equip 新添加设备相关信息
       //       Zz_Id  新添加设备所属装置Id
      //         Person_Ids 新添加设备所属管理者集合,该参数默认为null,当不将设备添加给用户时，取默认值
       //返回值：bool 
      public bool addEquip(Equip_Info new_Equip,int Zz_Id,List<int> Person_Ids=null)
       {
           try
           {
               Es.AddEquip(new_Equip, Zz_Id);
               if (Person_Ids!=null) Es.Equip_LinkPersons(new_Equip.Equip_Id,Person_Ids);
               return true;
           }
          catch{return false;}
       }

      //功能：修改设备信息
      //参数： new_Equip 设备的修改信息
      //       Zz_Id  设备新的所属装置Id
      //         Person_Ids 设备所属的新管理者集合,该参数默认为null,当不改变设备所属用户时，该参数取默认值
      //返回值：bool
       public bool modifyEquip(Equip_Info new_Equip,int Zz_Id,List<int> Person_Ids=null)
      {
          try
          {
              Es.ModifyEquip(new_Equip,Zz_Id);
              if (Person_Ids != null) Es.Equip_LinkPersons(new_Equip.Equip_Id, Person_Ids);
              return true;
          }
          catch { return false; }

      }
       //功能：删除设备信息，会自动删除该设备链接的其他信息
       //参数：E_Id 设备Id
       //返回值：bool
       public bool deleteEquip(int E_Id)
       {
           try
           {
               Es.DeleteEquip(E_Id );
              
               return true;
           }
           catch { return false; }

       }

       public List<EANummodel> getequipnum_byarchi()
       {
           try
           {
               List<EANummodel> E = Es.getequipnum_byarchi();

               return E;
           }
           catch { return null; }
       
       }
       public int getEA_parentid(int Ea_id)
       {

           try
           {
               int E = Es.getEA_parentid(Ea_id);

               return E;
           }
           catch { return 0; }
       }

       public int getEa_idbyname(string Ea_name)
       {

           try
           {
               int E = EAs.getEa_idbyname(Ea_name);

               return E;
           }
           catch { return 0; }
       }
       public string getEa_namebyid(int Ea_id)
       {

           try
           {
               string E = EAs.getEa_namebyId(Ea_id);

               return E;
           }
           catch { return null; }
       }
       public string getZzName(int Equip_location_EA_Id)
           {
           List<Equip_Archi> E_Zz = new List<Equip_Archi>();
           string ZzName = EAs.getEA_Name(Equip_location_EA_Id);
           return ZzName;
           }
       /// <summary>
       /// 0917xs
       /// </summary>
       /// <param name="EA_Name"></param>
       /// <returns></returns>
       public string getEquip(string EA_Name)
       {



           return EAs.getEA_NameByZzNAme(EA_Name);
       }
       public List<Equip_Info> getAllThEquips()
       {
           return Es.getAllThEquips();
       }
       public List<Equip_Archi> GetAllCj()
       {

           return Es.GetAllCj();
       }
       public List<Pq_Zz_map> GetZzsofPq(string Pqname)
       {
           return Es.GetZzsofPq(Pqname);
       }
       public Pq_Zz_map GetPqofZz(string Zzname)
       {
           return Es.GetPqofZz(Zzname);
       }
    }
}

using EquipModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EquipDAL.Implement
{
   
   public  class Equips:BaseDAO
    {
    
        public Equip_Info getEquip_Info(int Equip_id)
        {
            using (var db = base.NewDB())
            {
                try
                {
                    var E = db.Equips.Where(a => a.Equip_Id == Equip_id).First();
                    return E;
                }
                catch
                { return null; }
            }
        }



       //根据设备编号获取设备信息-xwm add
        public Equip_Info getEquip_byGyCode(string Equip_GyCode)
        {
            using (var db = base.NewDB())
            {
                try
                {
                    var E = db.Equips.Where(a => a.Equip_GyCode == Equip_GyCode).First();
                    return E;
                }
                catch
                { return null; }
            }
        }


        public int getE_id_byGybh(string equip_Gy)
        {
            using (var db = base.NewDB())
            {
                try
                {
                    var E = Convert.ToInt16(db.Equips.Where(a => a.Equip_Code == equip_Gy).First().Equip_Id);
                    return E;
                }
                catch
                { return 0; }


            }
        }
        public int getEA_id_byCode(string equip_code)
        {
            using (var db = base.NewDB())
            {
                try
                {
                    var E = Convert.ToInt16(db.Equips.Where(a => a.Equip_Code == equip_code).First().Equip_Location.EA_Id);
                    return E;
                }
                catch
                { return 0; }


            }
        }

        //查询设备
        public List<Equip_Info> getEquips_byinfo(string Equip_gycode, int Equip_archi, string Equip_specialty)
        {
            using (var db = base.NewDB())
            {
                try
                {
                    if (Equip_gycode != "" && Equip_archi != 0 && Equip_specialty != "")
                    {
                        var E = db.Equips.Where(a => a.Equip_PhaseB.Contains(Equip_specialty) && a.Equip_GyCode.Contains(Equip_gycode) && a.Equip_Location.EA_Id == Equip_archi).ToList();
                        return E;
                    }
                    else if (Equip_gycode == "" && Equip_archi != 0 && Equip_specialty != "")
                    {
                        var E = db.Equips.Where(a => a.Equip_PhaseB.Contains(Equip_specialty) && a.Equip_Location.EA_Id == Equip_archi).ToList();
                        return E;
                    }
                    else if (Equip_archi == 0 && Equip_specialty != "" && Equip_gycode != "")
                    {
                        var E = db.Equips.Where(a => a.Equip_PhaseB.Contains(Equip_specialty) && a.Equip_GyCode.Contains(Equip_gycode)).ToList();
                        return E;
                    }
                    else if (Equip_specialty == "" && Equip_archi != 0 && Equip_gycode != "")
                    {
                        var E = db.Equips.Where(a => a.Equip_Location.EA_Id == Equip_archi && a.Equip_GyCode.Contains(Equip_gycode)).ToList();
                        return E;
                    }
                    else if (Equip_specialty == "" && Equip_archi == 0 && Equip_gycode != "")
                    {
                        var E = db.Equips.Where(a => a.Equip_GyCode.Contains(Equip_gycode)).ToList();
                        return E;
                    }
                    else if (Equip_specialty == "" && Equip_archi != 0 && Equip_gycode == "")
                    {
                        var E = db.Equips.Where(a => a.Equip_Location.EA_Id == Equip_archi).ToList();
                        return E;
                    }
                    else
                    {
                        var E = db.Equips.Where(a => a.Equip_PhaseB.Contains(Equip_specialty)).ToList();
                        return E;
                    }
                }
                catch
                { return null; }
            }
        }
        public Equip_Info getEquip_Info(string Equip_Code)
        {
            using (var db = base.NewDB())
            {
                try
                {
                    var E = db.Equips.Where(a => a.Equip_Code == Equip_Code).First();
                    return E;
                }
                catch
                { return null; }
            }
        }
       //获取某设备所在的装置信息
       public Equip_Archi getEquip_EA(int Equip_id)
        {
            using (var db = base.NewDB())
            {
                try
                {
                    var E = db.Equips.Where(a => a.Equip_Id == Equip_id).First();

                    return E.Equip_Location;
                }
                catch
                { return null; }
            }
        }
       //获取某设备的所有负责人信息
        public List<Person_Info> getEquipManagers(int Equip_id)
        {
            using (var db = base.NewDB())
            {
                try
                {
                    var E = db.Equips.Where(a => a.Equip_Id == Equip_id).First();
                    return E.Equip_Manager.ToList();
                }
                catch
                { return null; }
            }
        }
       //添加设备的管理员,链接Equip表和Person_info表
        public bool Equip_LinkPersons(int Equip_id,List<int> Person_Ids)
        {
            try
            {
                using (var db = base.NewDB())
                {
                    var E = db.Equips.Where(a => a.Equip_Id == Equip_id).First();
                    if ( Person_Ids!=null &&  Person_Ids.Count>0 )
                    {  var Ep_old = E.Equip_Manager.ToList();
                       if (Ep_old.Count > 0)
                      {
                        E.Equip_Manager.Clear();
                       }
                   
                      foreach (var item in Person_Ids)
                     {
                        var P = db.Persons.Where(a => a.Person_Id == item).First();
                        E.Equip_Manager.Add(P);
                      }
                    
                     db.SaveChanges();
                    }
                    return true;
                }
            }
            catch
            {
                return false;
            }
             
        }
       //添加设备，并将其和装置建立链接
        public bool AddEquip(Equip_Info New_Equip_Info,int ZzId)
        {
            try
            {
                using (var db = base.NewDB())
                {
                    Equip_Info e=db.Equips.Add(New_Equip_Info);
                    Equip_Archi eItem = db.EArchis.Where(a => a.EA_Id == ZzId).First();
                    e.Equip_Location = eItem;
                    db.SaveChanges();
                    return true;
                }
            }
            catch
            {
                return false;
            }
             

        }
       //删除设备信息
        public bool DeleteEquip(int Equip_id)
        {
            using (var db = base.NewDB())
            {
                try
                {
                    var E = db.Equips.Where(a => a.Equip_Id == Equip_id).First();
                    if (E == null)
                        return false;
                    else
                    {   db.Equips.Remove(E);
                        db.SaveChanges();
                        return true;

                    }
                }
                catch { return false; }
            }

        }
       //修改设备信息相关信息
       //参数：New_Equip_Info 新的设备信息
       //装置      EA_Id 新的设备所属装置Id
        public bool ModifyEquip(Equip_Info New_Equip_Info,int EA_Id)
        {
            using (var db = base.NewDB())
            {
                try
                {
                    var E = db.Equips.Where(a => a.Equip_Code== New_Equip_Info.Equip_Code).First();
                    Equip_Archi eItem = db.EArchis.Where(a => a.EA_Id == EA_Id && a.EA_Title=="装置").First();
                    if (E == null || eItem==null)
                        return false;
                    else
                    {
                        E.Equip_ABCmark = New_Equip_Info.Equip_ABCmark;
                        E.Equip_Code = New_Equip_Info.Equip_Code;
                        E.Equip_ManufactureDate = New_Equip_Info.Equip_ManufactureDate;
                        E.Equip_Manufacturer = New_Equip_Info.Equip_Manufacturer;
                        E.Equip_PhaseB = New_Equip_Info.Equip_PhaseB;
                        E.Equip_Specialty = New_Equip_Info.Equip_Specialty;
                        E.Equip_Type = New_Equip_Info.Equip_Type;
                        E.Equip_GyCode = New_Equip_Info.Equip_GyCode;
                        E.Equip_Location = New_Equip_Info.Equip_Location;
                        E.Equip_PerformanceParasJson=New_Equip_Info.Equip_PerformanceParasJson;
                        E.Equip_Location = eItem;
                        db.SaveChanges();
                        return true;

                    }
                }
                catch { return false; }
            }
        }


        public List<EANummodel> getequipnum_byarchi()
        {

            using (var db = base.NewDB())
            {
                try
                {
                    List<EANummodel> Em = new List<EANummodel>();
                    
                    var E = db.Equips.Select(a => new { EA_Id = a.Equip_Location.EA_Id}).GroupBy(a => a.EA_Id).ToList();
                    foreach (var i in E)
                    {
                        EANummodel temp = new EANummodel();
                        temp.Equip_Num = i.Count();
                        temp.EA_Id = i.Key;
                        Em.Add(temp);
                    }
                    return Em;
                }
                catch(Exception e)
                { return null; }
            }
        
        }

        public int getEA_parentid(int Ea_id)
        {
            using (var db = base.NewDB())
            {
                try
                {
                    var E = db.EArchis.Where(a => a.EA_Id == Ea_id).First().EA_Parent.EA_Id;
                    return E;
                }
                catch
                { return 0; }
            }
        }
        public List<Equip_Info> getAllThEquips()
        {
            using (var db = base.NewDB())
            {
                try
                {
                    var E = db.Equips.Where(a => a.thRecordTable== "1").ToList();
                    return E;
                }
                catch
                { return null; }
            }
        }
        public List<Equip_Archi> GetAllCj()
        {

            using (var db = base.NewDB())
            {
                try
                {
                    var E = db.EArchis.Where(a => a.EA_Parent.EA_Id == 1).ToList();
                    return E;
                }
                catch
                { return null; }
            }
        }
        public List<Pq_Zz_map> GetZzsofPq(string Pqname)
        {

            using (var db = base.NewDB())
            {
                try
                {
                    var E = db.Pq_Zz_maps.Where(a => a.Pq_Name == Pqname).ToList();
                    return E;
                }
                catch
                { return null; }
            }
        }
        public Pq_Zz_map GetPqofZz(string Zzname)
        {

            using (var db = base.NewDB())
            {
                try
                {
                    var E = db.Pq_Zz_maps.Where(a => a.Zz_Name == Zzname).First();
                    return E;
                }
                catch
                { return null; }
            }
        }
    }
}

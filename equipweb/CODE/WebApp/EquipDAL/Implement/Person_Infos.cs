﻿using EquipDAL.Interface;
using EquipModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace EquipDAL.Implement
{
    public class Person_Infos : BaseDAO, IPerson_Infos
    {
        public class PersonModal
        {
            public Person_Info P_Info;//用户基本信息
            public Depart_Archi D_Info;//部门信息
            public List<Equip_Archi> EA_Info;
            public List<Speciaty_Info> S_Info;//专业信息
            public List<Role_Info> R_Info;//角色信息
            public List<Menu> M_Info;//系统菜单信息


        }
        public Person_Info GetPerson_info(int id)
        {
            try
            {
                using (var db = base.NewDB())
                {
                    return db.Persons.Where(a => a.Person_Id == id).First();
                }
            }
            catch (Exception e)
            { return null; }
        }
        public List<Equip_Archi> getZz_of_Person(int Person_Id, int cj_id)
        {
            using (var db = base.NewDB())
            {
                try
                {
                    //Equip_Archi EA = new Equip_Archi();
                    Equip_Archi EA;
                    List<Equip_Archi> EA_list = new List<Equip_Archi>();

                    // var EA_Set = db.EArchis.Where(a => a.EA_Title == "车间" && a.EA_Childs.Where(s => s.Equips_Belong.Where(t => t.Equip_Manager.Where(b => b.Person_Id == Person_Id).Count() > 0).Count() > 0).Count() > 0);
                    var EA_Set = db.EArchis.Where(a => a.EA_Title == "装置" && a.EA_Parent.EA_Id == cj_id && a.Equips_Belong.Where(t => t.Equip_Manager.Where(b => b.Person_Id == Person_Id).Count() > 0).Count() > 0);

                    foreach (var item in EA_Set)
                    {
                        EA = new Equip_Archi();
                        EA.EA_Id = item.EA_Id;
                        EA.EA_Name = item.EA_Name;
                        EA.EA_Code = item.EA_Code;
                        EA.EA_Title = item.EA_Title;
                        EA_list.Add(EA);
                    }
                    return EA_list;
                }
                catch { return null; }
            }


        }
        public Person_Info GetPerson_info(string Person_Name)
        {
            try
            {
                using (var db = base.NewDB())
                {
                    return db.Persons.Where(a => a.Person_Name == Person_Name).First();
                }
            }
            catch (Exception e)
            { return null; }
        }
        public PersonModal Get_PersonModal(int userId)
        {
            try
            {
                PersonModal PM = new PersonModal();
                using (var db = base.NewDB())
                {
                    PM.P_Info = db.Persons.Where(a => a.Person_Id == userId).First();
                    PM.D_Info = PM.P_Info.Dept_Belong;
                    PM.R_Info = PM.P_Info.Person_roles.ToList();
                    PM.S_Info = PM.P_Info.Person_specialties.ToList();
                    PM.EA_Info = PM.P_Info.Person_EquipEAs.ToList();
                    PM.M_Info = PM.P_Info.Person_Menus.ToList();
                    return PM;
                }
            }
            catch (Exception e)
            { return null; }

        }
        public List<Person_Info> GetAllPerson_info()
        {
            try
            {
                using (var db = base.NewDB())
                {
                    return db.Persons.ToList();
                }
            }
            catch (Exception e)
            { return null; }
        }
        public List<Role_Info> GetPerson_Roles(int id)
        {
            using (var db = base.NewDB())
            {
                var P = db.Persons.Where(a => a.Person_Id == id).First();
                return P.Person_roles.ToList();
            }


        }
        public List<Equip_Archi> getEA_of_Person(int Person_Id)
        {
            using (var db = base.NewDB())
            {
                try
                {
                    //Equip_Archi EA = new Equip_Archi();
                    Equip_Archi EA;
                    List<Equip_Archi> EA_list = new List<Equip_Archi>();
                    var EA_Set = db.EArchis.Where(a => a.EA_Title == "车间" && a.EA_Childs.Where(s => s.Equips_Belong.Where(t => t.Equip_Manager.Where(b => b.Person_Id == Person_Id).Count() > 0).Count() > 0).Count() > 0);
                    foreach (var item in EA_Set)
                    {
                        EA = new Equip_Archi();
                        EA.EA_Id = item.EA_Id;
                        EA.EA_Name = item.EA_Name;
                        EA.EA_Code = item.EA_Code;
                        EA.EA_Title = item.EA_Title;
                        EA_list.Add(EA);
                    }
                    return EA_list;
                }
                catch { return null; }
            }


        }
        public List<Equip_Info> GetPerson_Equips(int id)
        {
            using (var db = base.NewDB())
            {
                var P = db.Persons.Where(a => a.Person_Id == id).First();
                return P.Person_Equips.ToList();
            }

        }

        public Depart_Archi GetPerson_Depart(int id)
        {
            using (var db = base.NewDB())
            {
                var P = db.Persons.Where(a => a.Person_Id == id).First();
                return P.Dept_Belong;
            }
        }

        public List<Menu> GetPerson_Menus(int id)
        {
            using (var db = base.NewDB())
            {
                var p = db.Persons.Where(a => a.Person_Id == id).First();
                return p.Person_Menus.ToList();
            }
        }
        public bool AddRole(int Person_id, int Role_id)
        {
            using (var db = base.NewDB())
            {
                try
                {
                    var P = db.Persons.Where(a => a.Person_Id == Person_id).First();
                    var PR = db.Roles.Where(a => a.Role_Id == Role_id).First();
                    P.Person_roles.Add(PR);
                    db.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }

        }
        public bool AddEquip(int Person_id, int Equip_id)
        {
            using (var db = base.NewDB())
            {
                try
                {
                    var P = db.Persons.Where(a => a.Person_Id == Person_id).First();
                    var E = db.Equips.Where(a => a.Equip_Id == Equip_id).First();
                    P.Person_Equips.Add(E);

                    db.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }

        }
        public int AddPerson_info(Person_Info New_Person)
        {
            try
            {
                using (var db = base.NewDB())
                {
                    Person_Info newP = db.Persons.Add(New_Person);
                    db.SaveChanges();
                    return newP.Person_Id;
                }
            }
            catch
            {
                return 0;
            }



        }

        public bool Person_LinkRole(int Person_id, List<int> Role_ids)
        {
            try
            {
                using (var db = base.NewDB())
                {
                    var p = db.Persons.Where(s => s.Person_Id == Person_id).First();
                    var pr_old = p.Person_roles.ToList();
                    if (pr_old.Count > 0)
                    {
                        p.Person_roles.Clear();
                    }
                    foreach (var item in Role_ids)
                    {
                        var r = db.Roles.Where(s => s.Role_Id == item).First();
                        p.Person_roles.Add(r);
                    }
                    db.SaveChanges();
                    return true;
                }
            }
            catch
            {
                return false;
            }

        }

        public bool Person_LinkDepart(int Person_id, int Depart_id)
        {
            try
            {
                using (var db = base.NewDB())
                {
                    var p = db.Persons.Where(s => s.Person_Id == Person_id).First();

                    var r = db.Department.Where(s => s.Depart_Id == Depart_id).First();
                    p.Dept_Belong = r;
                    db.SaveChanges();
                    return true;
                }
            }
            catch
            {
                return false;
            }

        }

        public bool Person_LinkSpeciaties(int Person_id, List<int> Speciaty_IDs)
        {
            try
            {
                using (var db = base.NewDB())
                {
                    var p = db.Persons.Where(s => s.Person_Id == Person_id).First();
                    var ps_old = p.Person_specialties.ToList();
                    if (ps_old.Count > 0)
                    {
                        p.Person_specialties.Clear();
                    }
                    foreach (var item in Speciaty_IDs)
                    {
                        Speciaty_Info r = db.Specialties.Where(s => s.Specialty_Id == item).First();
                        p.Person_specialties.Add(r);
                    }
                    db.SaveChanges();
                    return true;
                }
            }
            catch
            {
                return false;
            }

        }
        public bool Person_LinkEAs(int Person_id, List<int> EquipArchi_IDs)
        {
            try
            {
                using (var db = base.NewDB())
                {
                    var p = db.Persons.Where(s => s.Person_Id == Person_id).First();
                    var ps_old = p.Person_EquipEAs.ToList();
                    if (ps_old.Count > 0)
                    {
                        p.Person_EquipEAs.Clear();
                    }
                    foreach (var item in EquipArchi_IDs)
                    {
                        Equip_Archi r = db.EArchis.Where(s => s.EA_Id == item).First();
                        p.Person_EquipEAs.Add(r);
                    }
                    db.SaveChanges();
                    return true;
                }
            }
            catch
            {
                return false;
            }

        }

        public bool Person_LinkEquip(int Person_id, List<int> EquipArchi_IDs, List<int> Speciaty_IDs)
        {
            try
            {
                using (var db = base.NewDB())
                {
                    var p = db.Persons.Where(s => s.Person_Id == Person_id).First();
                    var pr_old = p.Person_Equips.ToList();
                    if (pr_old.Count > 0)
                    {
                        p.Person_Equips.Clear();
                    }
                    var r = db.Specialties.Where(s => Speciaty_IDs.Where(m => m == s.Specialty_Id).Count() > 0);
                    if (Person_id == 36 && Person_id == 1)
                    {
                        List<Equip_Info> e = db.Equips.Where(s => ((r.Where(rr => rr.Speciaty_Parent.Specialty_Name == s.Equip_Specialty && rr.Specialty_Name == s.Equip_PhaseB)).Count() > 0) && (EquipArchi_IDs.Where(t => (s.Equip_Location).EA_Id == t).Count() > 0)).ToList();
                        foreach (var eitem in e)
                        { p.Person_Equips.Add(eitem); }

                        e = db.Equips.Where(s => ((r.Where(rr => rr.Speciaty_Parent.Specialty_Name == "root" && rr.Specialty_Name != "动" && rr.Specialty_Name == s.Equip_Specialty)).Count() > 0) && (EquipArchi_IDs.Where(t => (s.Equip_Location).EA_Id == t).Count() > 0)).ToList();
                        foreach (var eitem1 in e)
                        { p.Person_Equips.Add(eitem1); }

                    }

                    else
                    {
                        List<Equip_Info> e = db.Equips.Where(s => (s.Equip_Specialty == "动" && (r.Where(rr => rr.Speciaty_Parent.Specialty_Name == s.Equip_Specialty && rr.Specialty_Name == s.Equip_PhaseB)).Count() > 0) && (EquipArchi_IDs.Where(t => (s.Equip_Location).EA_Id == t).Count() > 0)).ToList();
                        foreach (var eitem in e)
                        { p.Person_Equips.Add(eitem); }
                        e = db.Equips.Where(s => (s.Equip_Specialty != "动" && (r.Where(rr => rr.Specialty_Name == s.Equip_Specialty)).Count() > 0) && (EquipArchi_IDs.Where(t => (s.Equip_Location).EA_Id == t).Count() > 0)).ToList();
                        foreach (var eitem in e)
                        { p.Person_Equips.Add(eitem); }


                    }


                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception e)
            {
                return false;
            }

        }

        public bool Person_LinkMenus(int Person_id, List<int> Menu_IDs)
        {
            try
            {
                using (var db = base.NewDB())
                {
                    var p = db.Persons.Where(s => s.Person_Id == Person_id).First();
                    var pr_old = p.Person_Menus.ToList();
                    if (pr_old.Count > 0)
                    {
                        p.Person_Menus.Clear();
                    }

                    foreach (var item in Menu_IDs)
                    {
                        Menu r = db.Sys_Menus.Where(s => s.Menu_Id == item).First();
                        p.Person_Menus.Add(r);
                    }
                    db.SaveChanges();
                    return true;
                }
            }
            catch
            {
                return false;
            }

        }

        public bool DeletePerson_info(int id)
        {
            using (var db = base.NewDB())
            {
                try
                {
                    var P = db.Persons.Where(a => a.Person_Id == id).First();
                    if (P == null)
                        return false;
                    else
                    {


                        //  P.Person_Equips.Clear();
                        // P.Person_Menus.Clear();
                        // P.Person_specialties.Clear();
                        // P.Person_roles.Clear();
                        db.Persons.Remove(P);
                        db.SaveChanges();
                        return true;

                    }
                }
                catch { return false; }
            }

        }

        public bool ModifyPerson_info(Person_Info New_Person)
        {
            using (var db = base.NewDB())
            {
                try
                {
                    var P = db.Persons.Where(a => a.Person_Id == New_Person.Person_Id).First();
                    if (P == null)
                        return false;
                    else
                    {
                        P.Person_Name = New_Person.Person_Name;
                     //   P.Person_Pwd = New_Person.Person_Pwd;
                        P.Person_tel = New_Person.Person_tel;
                        P.Person_mail = New_Person.Person_mail;
                        db.SaveChanges();
                        return true;

                    }
                }
                catch { return false; }
            }

        }

        public int isAdmin(string username)
        {
            using (var db = base.NewDB())
            {
                var P = db.Persons.Where(a => a.Person_Name == username && a.Person_roles.Where(r => r.Role_Name == "系统管理员").Count() > 0).Count();
                if (P > 0)
                    return 1;
                else
                    return 0;
            }


        }
        public bool ModifyPerson_Pwd(string userName, byte[] userPwd)
        {
            using (var db = base.NewDB())
            {
                try
                {
                    var P = db.Persons.Where(a => a.Person_Name == userName).First();
                    P.Person_Pwd = userPwd;
                    db.SaveChanges();
                    return true;
                }
                catch { return false; }
            }
        }

        public Person_Info GetPerson_info(string Person_Name, byte[] Person_pwd)
        {
            try
            {
                using (var db = base.NewDB())
                {
                    return db.Persons.Where(a => a.Person_Name == Person_Name && a.Person_Pwd == Person_pwd).First();
                }
            }
            catch (Exception e)
            { return null; }
        }

        public string Get_PqnamebyCjname(string CjName)
        {
            try
            {
                using (var db = base.NewDB())
                {
                    var p = db.Department.Where(a => a.Depart_Name == CjName).First().Depart_Parent.Depart_Name;
                    return p;
                }
            }
            catch (Exception e)

            { return ""; }
        }

        /// <summary>
        /// 16/9/8新增，获取润滑油间列表，尚未区分权限，此表为独立的表，未与人员表关联
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public List<Runhua_Info> getRunhuaCj()
        {
            using (var db = base.NewDB())
            {
                try
                {
                    //Equip_Archi EA = new Equip_Archi();
                    Runhua_Info EA;
                    List<Runhua_Info> EA_list = new List<Runhua_Info>();
                    var EA_Set = db.Runhua;
                    foreach (var item in EA_Set)
                    {
                        EA = new Runhua_Info();
                        EA.Runhua_Id = item.Runhua_Id;
                        EA.EA_Name = item.EA_Name;
                        EA.RH_Name = item.RH_Name;
                        EA.RH_ZZ_Name = item.RH_ZZ_Name;
                        EA.RH_Detail = item.RH_Detail;
                        EA_list.Add(EA);
                    }
                    return EA_list;
                }
                catch { return null; }
            }


        }

        public List<Equip_Archi> getZzByPerson(int Person_Id)
        {
            using (var db = base.NewDB())
            {
                try
                {
                    //Equip_Archi EA = new Equip_Archi();
                    Equip_Archi EA;
                    List<Equip_Archi> EA_list = new List<Equip_Archi>();

                    // var EA_Set = db.EArchis.Where(a => a.EA_Title == "车间" && a.EA_Childs.Where(s => s.Equips_Belong.Where(t => t.Equip_Manager.Where(b => b.Person_Id == Person_Id).Count() > 0).Count() > 0).Count() > 0);
                    var EA_Set = db.EArchis.Where(a => a.EA_Title == "装置" && a.Equips_Belong.Where(t => t.Equip_Manager.Where(b => b.Person_Id == Person_Id).Count() > 0).Count() > 0);

                    foreach (var item in EA_Set)
                    {
                        EA = new Equip_Archi();
                        EA.EA_Id = item.EA_Id;
                        EA.EA_Name = item.EA_Name;
                        EA.EA_Code = item.EA_Code;
                        EA.EA_Title = item.EA_Title;
                        EA_list.Add(EA);
                    }
                    return EA_list;
                }
                catch { return null; }
            }


        }

    }
}

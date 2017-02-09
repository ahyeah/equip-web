using EquipDAL.Interface;
using EquipModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipDAL.Implement
{
    public class Equip_Archis : BaseDAO, IEquipArchis
    {

        public bool AddEquip_Archi(int parent_Id, Equip_Archi New_Equip_Archi)
        {
            using (var db = base.NewDB())
            {
                try
                {
                    Equip_Archi parent = db.EArchis.Where(a => a.EA_Id == parent_Id).First();
                    if (parent == null)
                        return false;

                    parent.EA_Childs.Add(New_Equip_Archi);
                    db.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }
        public string getEa_namebyId(int Ea_id)
        {
            using (var db = base.NewDB())
            {
                try
                {
                    var E = db.EArchis.Where(a => a.EA_Id == Ea_id).First().EA_Name;
                    return E;
                }
                catch
                { return ""; }
            }
        }

        public string getcodebyname(string Ea_name)
        {
            using (var db = base.NewDB())
            {
                try
                {
                    var E = db.EArchis.Where(a => a.EA_Name == Ea_name).First().EA_Code;
                    return E;
                }
                catch
                { return null; }
            }
        }
        public int getEa_idbyname(string Ea_name)
        {
            using (var db = base.NewDB())
            {
                try
                {
                    var E = db.EArchis.Where(a => a.EA_Name == Ea_name).First().EA_Id;
                    return E;
                }
                catch
                { return 0; }
            }
        }

        public bool AddEA_Euip(int EA_Id, Equip_Info New_Equip_Info)
        {
            using (var db = base.NewDB())
            {
                try
                {
                    Equip_Archi EA = db.EArchis.Where(a => a.EA_Id == EA_Id).First();
                    if (EA == null)
                        return false;
                    EA.Equips_Belong.Add(New_Equip_Info);
                    db.SaveChanges();
                    return true;
                }
                catch { return false; }
            }

        }


        public Equip_Archi getEA_Archi(int EA_Id)
        {
            using (var db = base.NewDB())
            {
                try
                {
                    Equip_Archi EA = db.EArchis.Where(a => a.EA_Id == EA_Id).First();
                    return EA;
                }
                catch { return null; }
            }
        }

        //开始


        public EquipModel.Entities.Equip_Archi GetEquipArchi(int id)
        {
            using (var db = base.NewDB())
            {
                return db.EArchis.Where(a => a.EA_Id == id).First();
            }
        }

        public List<EquipModel.Entities.Equip_Archi> GetEquipArchi(string name)
        {
            using (var db = base.NewDB())
            {
                return db.EArchis.Where(a => a.EA_Name == name).OrderBy(a => a.EA_Name).ToList();
            }
        }

        public List<EquipModel.Entities.Equip_Archi> GetChildMenu(int id)
        {
            using (var db = base.NewDB())
            {
                var menu = db.EArchis.Where(a => a.EA_Id == id).First();
                return menu.EA_Childs.ToList();
            }
        }

        public List<EquipModel.Entities.Equip_Archi> GetChildMenu(string name)
        {
            using (var db = base.NewDB())
            {
                var menu = db.EArchis.Where(a => a.EA_Name == name).First();
                return menu.EA_Childs.ToList();
            }
        }

        public bool AddEquipArchi(int parentID, Equip_Archi newMenu)
        {
            using (var db = base.NewDB())
            {
                try
                {
                    Equip_Archi parent = db.EArchis.Where(a => a.EA_Id == parentID).First();
                    if (parent == null)
                        return false;

                    parent.EA_Childs.Add(newMenu);
                    db.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public bool DeleteEquipArchi(int id)
        {
            using (var db = base.NewDB())
            {
                try
                {
                    var menu = db.EArchis.Where(a => a.EA_Id == id).First();
                    if (menu == null)
                        return true;

                    foreach (var child in menu.EA_Childs)
                    {
                        DeleteEquipArchi(child.EA_Id);
                    }
                    db.EArchis.Remove(menu);
                    db.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public bool ModifyEquipArchi(int id, EquipModel.Entities.Equip_Archi nVal)
        {
            using (var db = base.NewDB())
            {
                try
                {
                    var menu = db.EArchis.Where(a => a.EA_Id == id).First();

                    if (menu == null)
                        return false;

                    menu.EA_Name = nVal.EA_Name;
                    menu.EA_Code = nVal.EA_Code;
                    menu.EA_Title = nVal.EA_Title;

                    db.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }








        //结束



        public List<Equip_Archi> getEA_Childs(int EA_Id)
        {
            using (var db = base.NewDB())
            {
                try
                {
                    Equip_Archi EA = db.EArchis.Where(a => a.EA_Id == EA_Id).First();

                    return EA.EA_Childs.ToList(); ;
                }
                catch { return null; }
            }
        }

        public List<Equip_Archi> getEA_Childs(string EA_Name)
        {
            using (var db = base.NewDB())
            {
                try
                {
                    Equip_Archi EA = db.EArchis.Where(a => a.EA_Name == EA_Name).First();

                    return EA.EA_Childs.ToList();
                }
                catch { return null; }
            }
        }

        public List<Equip_Archi> getEAs_isCj()
        {
            using (var db = base.NewDB())
            {
                try
                {
                    Equip_Archi EA = db.EArchis.Where(a => a.EA_Title == "车间").First();

                    return EA.EA_Childs.ToList();
                }
                catch { return null; }
            }
        }

        public Equip_Archi getEA_Parent(int EA_Id)
        {
            using (var db = base.NewDB())
            {
                try
                {
                    Equip_Archi EA = db.EArchis.Where(a => a.EA_Id == EA_Id).First();

                    return EA.EA_Parent;
                }
                catch { return null; }
            }
        }

        public List<Equip_Info> getEA_Equips(int EA_Id, string username = null)
        {
            List<Equip_Info> E = new List<Equip_Info>();
            using (var db = base.NewDB())
            {
                try
                {
                    if (username != null && username!="sa")
                    {
                        //List<Equip_Info> e = db.Persons.Where(a => a.Person_Name == username).First().Person_Equips.ToList();
                        //foreach (var temp in e)
                        //{
                        //    if (temp.Equip_Location.EA_Id == EA_Id)
                        //        E.Add(temp);

                        //}
                        //return E;
                        E = db.Equips.Where(a => a.Equip_Specialty == "动" && a.Equip_Location.EA_Id == EA_Id).ToList();

                        return E;
                    }
                    else if( username=="sa")
                    {
                        Equip_Archi EA = db.EArchis.Where(a => a.EA_Id == EA_Id).First();

                        return EA.Equips_Belong.ToList();

                    }
                    else
                    {
                        E = db.Equips.Where(a => a.Equip_Specialty == "动" && a.Equip_Location.EA_Id == EA_Id).ToList();

                        return E;

                    }

                }
                catch { return null; }
            }
        }
        public List<Equip_Info> getThEA_Equips(int EA_Id)
        {
            using (var db = base.NewDB())
            {
                try
                {
                    List<Equip_Info> E = db.Equips.Where(a => a.Equip_Location.EA_Id == EA_Id&&a.thRecordTable=="1").ToList();

                    return E;
                }
                catch { return null; }
            }
        }
        public bool ModifyEquip_Archi(int EA_Id, Equip_Archi New_Equip_Archi)
        {
            using (var db = base.NewDB())
            {
                try
                {
                    Equip_Archi EA = db.EArchis.Where(a => a.EA_Id == EA_Id).First();
                    if (EA == null)
                        return false;
                    EA.EA_Name = New_Equip_Archi.EA_Name;
                    EA.EA_Title = New_Equip_Archi.EA_Title;
                    db.SaveChanges();
                    return true;
                }
                catch { return false; }
            }

        }

        public bool DeleteEquip_Archi(int EA_Id)
        {

            using (var db = base.NewDB())
            {
                try
                {
                    Equip_Archi EA = db.EArchis.Where(a => a.EA_Id == EA_Id).First();
                    if (EA == null)
                        return false;
                    EA.Equips_Belong.Clear();
                    db.EArchis.Remove(EA);
                    db.SaveChanges();
                    return true;
                }
                catch { return false; }
            }
        }


        public Equip_Archi GetEA_Archi(int EA_Id)
        {
            throw new NotImplementedException();
        }

        public List<Equip_Archi> GetEA_Archi(string name)
        {
            throw new NotImplementedException();
        }


        public string getEA_Name(int EA_Id)
            {
            using (var db = base.NewDB())
                {
                try
                    {
                    string EA_Name = db.EArchis.Where(a => a.EA_Id == EA_Id).First().EA_Name;

                    return EA_Name;
                    }
                catch { return null; }
                }
            }
        /// <summary>
        /// 通过装置名字获取装置所属车间0917
        /// </summary>
        /// <param name="EA_Name"></param>
        /// <returns></returns>
        public string getEA_NameByZzNAme(string EA_Name)
        {
            using (var db = base.NewDB())
            {
                try
                {
                    string Name = db.EArchis.Where(a => a.EA_Name == EA_Name).First().EA_Parent.EA_Name;

                    return Name;
                }
                catch { return null; }
            }
        }
        //2016-4-22注释
        //Equip_Archi IEquipArchis.GetEquipArchi(int EA_Id)
        //{
        //    throw new NotImplementedException();
        //}

        //List<Equip_Archi> IEquipArchis.getEA_Archi(string name)
        //{
        //    throw new NotImplementedException();
        //}

        //List<Equip_Archi> IEquipArchis.getEA_Childs(int EA_Id)
        //{
        //    throw new NotImplementedException();
        //}

        //bool IEquipArchis.AddEquip_Archi(int parent_Id, Equip_Archi New_Equip_Archi)
        //{
        //    throw new NotImplementedException();
        //}

        //bool IEquipArchis.DeleteEquip_Archi(int EA_Id)
        //{
        //    throw new NotImplementedException();
        //}

        //bool IEquipArchis.ModifyEquip_Archi(int EA_Id, Equip_Archi New_Equip_Archi)
        //{
        //    throw new NotImplementedException();
        //}
    }
}

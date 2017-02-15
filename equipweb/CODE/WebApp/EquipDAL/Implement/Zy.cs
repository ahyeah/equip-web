using EquipDAL.Interface;
using EquipModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipDAL.Implement
{
    public class Zy : BaseDAO, ISpecialty
    {

        public Speciaty_Info GetSpecialty(int id)
        {
            using (var db = base.NewDB())
            {
                return db.Specialties.Where(a => a.Specialty_Id == id).First();
            }
        }

        public List<EquipModel.Entities.Speciaty_Info> GetSpecialty(string name)
        {
            using (var db = base.NewDB())
            {
                return db.Specialties.Where(a => a.Specialty_Name == name).OrderBy(a => a.Specialty_Name).ToList();
            }
        }

        public List<EquipModel.Entities.Speciaty_Info> GetChildSpecialty(int id)
        {
            using (var db = base.NewDB())
            {
                var zy = db.Specialties.Where(a => a.Specialty_Id == id).First();
                return zy.Speciaty_Childs.ToList();
            }
        }

        public List<EquipModel.Entities.Speciaty_Info> GetChildSpecialty(string name)
        {
            using (var db = base.NewDB())
            {
                var zy = db.Specialties.Where(a => a.Specialty_Name == name).First();
                return zy.Speciaty_Childs.ToList();
            }
        }

        public bool AddSpecialty(int parentID, Speciaty_Info newZy)
        {
            using (var db = base.NewDB())
            {
                try
                {
                    Speciaty_Info parent = db.Specialties.Where(a => a.Specialty_Id == parentID).First();
                    if (parent == null)
                        return false;

                    parent.Speciaty_Childs.Add(newZy);
                    db.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public bool DeleteSpecialty(int id)
        {
            using (var db = base.NewDB())
            {
                try
                {
                    var Zy = db.Specialties.Where(a => a.Specialty_Id == id).First();
                    if (Zy == null)
                        return true;

                    foreach (var child in Zy.Speciaty_Childs)
                    {
                        DeleteSpecialty(child.Specialty_Id);
                    }
                    db.Specialties.Remove(Zy);
                    db.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public bool ModifySpecialty(int id, Speciaty_Info nVal)
        {
            using (var db = base.NewDB())
            {
                try
                {
                    var zy = db.Specialties.Where(a => a.Specialty_Id == id).First();

                    if (zy == null)
                        return false;

                    zy.Specialty_Name= nVal.Specialty_Name;
                    

                    db.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

    }
}

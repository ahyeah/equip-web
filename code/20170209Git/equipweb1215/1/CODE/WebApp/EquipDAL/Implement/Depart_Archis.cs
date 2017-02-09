using EquipModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipDAL.Implement
{
   public  class Depart_Archis:BaseDAO
    {
       
       public   Depart_Archi getDepart_info(int Depart_id)
        {
            using(var db=base.NewDB())
            {
                return db.Department.Where(a => a.Depart_Id == Depart_id).First();

            }
        }

       public Depart_Archi getDepart_root(string  Depart_name)
       {
           using (var db = base.NewDB())
           {
               return db.Department.Where(a => a.Depart_Name == Depart_name).First();

           }
       }
       public string getDepart_Pq(string Depart_name)
       {
           using (var db = base.NewDB())
           {
               return db.Department.Where(a => a.Depart_Name == Depart_name).First().Depart_Parent.Depart_Name;

           }
       }

       public int getDepart_Id(string Depart_name)
       {
           using (var db = base.NewDB())
           {
               return db.Department.Where(a => a.Depart_Name == Depart_name).First().Depart_Id;

           }
       }
        public List<Depart_Archi> getDepart_Childs(int Depart_id)
        {
            using (var db = base.NewDB())
            {
                var DepartArchi = db.Department.Where(a => a.Depart_Id == Depart_id).First();
                return DepartArchi.Depart_child.ToList();
            }

        }

        public List<Depart_Archi> getDepart_Childs(string Depart_name)
        {
            using (var db = base.NewDB())
            {
                var DepartArchi = db.Department.Where(a => a.Depart_Name == Depart_name).First();
                return DepartArchi.Depart_child.ToList();
            }

        }

        public List<Person_Info> getPersons_Belong(int Depart_id)
        {
            using (var db = base.NewDB())
            {
                var DepartArchi = db.Department.Where(a => a.Depart_Id == Depart_id).First();
                return DepartArchi.Depart_Persons.ToList();
            }

        }

        public Depart_Archi getDepart_Parent(int Depart_id)
        {
            using (var db = base.NewDB())
            {
                var DepartArchi = db.Department.Where(a => a.Depart_Id == Depart_id).First();
                return DepartArchi.Depart_Parent;
            }
        }
        public bool AddDepart_Archi(int parentId,Depart_Archi New_Depart_info)
        {
            using (var db = base.NewDB())
            {
                try
                {
                    Depart_Archi parent = db.Department.Where(a => a.Depart_Id == parentId).First();
                    if (parent == null)
                        return false;

                    parent.Depart_child.Add(New_Depart_info);
                    db.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }
        public bool DeleteDepart_Archi(int Depart_id)
        {
            using (var db = base.NewDB())
            {
                try
                {
                    var P = db.Department.Where(a => a.Depart_Id==Depart_id).First();
                    if (P == null)
                        return false;
                    else
                    {  
                        // foreach(var  pc in P.Depart_child)
                        //{ DeleteDepart_Archi(pc.Depart_Id); }
                        P.Depart_Persons.Clear();                  
                        db.Department.Remove(P);

                        db.SaveChanges();
                        return true;

                    }
                }
                catch { return false; }
            }
        }
        public bool ModifyDepart_Archi(int Depart_id,Depart_Archi New_Depart_info)
        {
            using (var db = base.NewDB())
            {
                try
                {
                    var P = db.Department.Where(a => a.Depart_Id == Depart_id).First();
                    if (P == null)
                        return false;
                    else
                    {
                        P.Depart_Name = New_Depart_info.Depart_Name;
                        db.SaveChanges();
                        return true;

                    }
                }
                catch { return false; }
            }
        
        }
    }
}

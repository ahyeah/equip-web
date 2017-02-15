using EquipModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipDAL.Implement
{
    public class Speciaties:BaseDAO
    {
     
        //得到某个专业的孩子节点
        public List<Speciaty_Info> getSepciaty_Childs(int Speciaty_Id)
        {
            using(var db=base.NewDB())
            {
                return db.Specialties.Where(a => a.Speciaty_Parent.Specialty_Id == Speciaty_Id).ToList();
                
            }

        }
        public List<Speciaty_Info> getSepciaty_Parent()
        {
            using (var db = base.NewDB())
            {
                return db.Specialties.Where(a => a.Speciaty_Parent.Specialty_Name == "root").ToList();

            }

        }
        public List<Speciaty_Info> getSepciaty_Childs(string Speciaty_name)
        {
            using (var db = base.NewDB())
            {
                return db.Specialties.Where(a => a.Speciaty_Parent.Specialty_Name == Speciaty_name).ToList();

            }

        }




    }
}

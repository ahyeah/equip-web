using EquipDAL.Interface;
using EquipModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipDAL.Implement
{
    public class Q_Entrance : BaseDAO
    {
        public bool AddQ_Entrance(int p_id, int m_id, int q_id)
        {
            //Menus ms = new Menus();
            //Menu m = ms.GetMenu(m_id);
            //Person_Infos pis = new Person_Infos();
            //Person_Info p = pis.GetPerson_info(p_id);

            using (var db = base.NewDB())
            {
                try
                {
                    //Quick_Entrance q_e = db.Quick_Entrance.Where(a => a.Quick_Entrance_id== q_id&&a.Person_Info.Person_Id==p_id).First();
                    var q_e = db.Quick_Entrance.Where(a => a.xuhao == q_id && a.Person_Info.Person_Id == p_id).ToList();
                    if (q_e.Count() != 0)
                    {
                        q_e.First().Menu = db.Sys_Menus.Where(a => a.Menu_Id == m_id).First();
                        db.SaveChanges();
                        return true;
                    }
                    else
                    {
                        Quick_Entrance QE = new Quick_Entrance();
                        db.Quick_Entrance.Add(QE);
                        QE.Menu = db.Sys_Menus.Where(a => a.Menu_Id == m_id).First();
                        QE.Person_Info = db.Persons.Where(a => a.Person_Id == p_id).First();
                        QE.xuhao = q_id;
                        db.SaveChanges();
                        return true;
                    }
                }
                catch (Exception e)
                {
                    return false;
                }
            }
        }

        public List<Quick_Entrance> GetQ_EbyP_Id(int p_id)
        {
            //List<Quick_Entrance> QE = new List<Quick_Entrance>();
            //using (var db = base.NewDB())
            //{
            //    try
            //    {
            //        QE = db.Quick_Entrance.Where(a => a.Person_Info.Person_Id == p_id).ToList();

            //    }
            //    catch
            //    { return null; }
            //}
            //return QE;
            List<Quick_Entrance> QE = new List<Quick_Entrance>();

            try
            {
                var db = base.NewDB();
                QE = db.Quick_Entrance.Where(a => a.Person_Info.Person_Id == p_id).ToList();

            }
            catch
            { return null; }

            return QE;
        }
    }
}

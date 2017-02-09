using EquipModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipDAL.Implement
{
    public class A6dot2LsTaskTabs : BaseDAO
    {

        public List<A6dot2LsTaskTab> getAllLsTaskTabs(string  wfe_id)
        
        {
            using (var db = base.NewDB())
            {
                return db.A6dot2LsTaskTab.Where(a => a.wfd_id==wfe_id).ToList();

            }
        }
        public A6dot2LsTaskTab AddA6dot2LsTask(A6dot2LsTaskTab new_A6dot2LsTask)
        {
            try
            {
                using (var db = base.NewDB())
                {
                    A6dot2LsTaskTab newP = db.A6dot2LsTaskTab.Add(new_A6dot2LsTask);
                    db.SaveChanges();
                    return newP;
                }
            }
            catch
            {
                return null;
            }



        }


        public bool remove(int  id)
        {
            try
            {
                using (var db = base.NewDB())
                {
                    var P = db.A6dot2LsTaskTab.Where(a => a.Id == id).First();
                    if (P == null)
                        return false;
                    else
                    {



                        db.A6dot2LsTaskTab.Remove(P);
                        db.SaveChanges();
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }


        }

        public A6dot2LsTaskTab UpdateA6dot2LsTask(int id, List<string> propertis, List<object> vals)
        {
            try
            {
                using (var db = base.NewDB())
                {
                    var LsTask = db.A6dot2LsTaskTab.Where(a => a.Id == id).First();
                    if (propertis.Count == 0)
                        return LsTask;

                    for (int i = 0; i < propertis.Count; i++)
                    {
                        string property = propertis[i];
                        object val = vals[i];
                        switch (property)
                        {
                            case "Zz_Name":
                                LsTask.Zz_Name = val as string;
                                break;

                            case "Equip_Gybh":
                                 LsTask.Equip_Gybh = val as string ;
                                break;

                            case "Equip_Code":
                                LsTask.Equip_Code = val as string;
                                break;
                            case "Last_HY":
                                LsTask.lastOilTime = val as string;
                                break;
                            case "HY_ZQ":
                                LsTask.oilInterval = (int)val;
                                break;
                            case "Problem_Cur":
                                LsTask.cur_problem = val as string;
                                break;

                            case "ZG_status":
                                LsTask.cur_status = val as string;
                                LsTask.cur_problem = "1";
                                break;
                           

                            default: //其他字段
                                break;
                        }
                    }
                    if (db.SaveChanges() != 1)
                        return null;

                    return LsTask;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

    }
}

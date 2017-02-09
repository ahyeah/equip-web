using EquipDAL.Interface;
using EquipModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipDAL.Implement
{
    public class WorkFlowTables : BaseDAO
    {
        public List<A5dot1Tab1> get_Allbytime(DateTime time)
        {

            using (var db = base.NewDB())
            {
                try
                {
                    var E = db.A5dot1Tab1.Where(a => a.zzSubmitTime < time).ToList();

                    return E;
                }
                catch (Exception e)
                {
                    return null;
                }

            }
        }
        public List<A5dot1Tab1> GetLS_listbywfe_id(List<int> wfe_id)
            {
            using (var db = base.NewDB())
                {
                List<A5dot1Tab1> E = new List<A5dot1Tab1>();
                try
                    {
                    foreach (var w in wfe_id)
                        {
                        var e = db.A5dot1Tab1.Where(a => a.dataSource == w.ToString()).ToList();
                        E.AddRange(e);
                        }
                    return E;
                    }
                catch (Exception e)
                    {
                    return null;
                    }

                }
            }

        public bool Zzsubmit(A5dot1Tab1 a5dot1Tab1)
        {
            using (var db = base.NewDB())
            {
                try
                {
                    //Quick_Entrance QE = new Quick_Entrance();
                    //db.Quick_Entrance.Add(QE);
                    //QE.Menu = db.Sys_Menus.Where(a => a.Menu_Id == m_id).First();
                    //QE.Person_Info = db.Persons.Where(a => a.Person_Id == p_id).First();
                    //QE.Quick_Entrance_id = p_id;
                    //db.SaveChanges();
                    db.A5dot1Tab1.Add(a5dot1Tab1);
                    db.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    return false;
                }
            }
        }
        public bool savefivesb(A5dot1Tab2 a5dot1Tab2)
        {
            //A5dot1Tab2 temp = new A5dot1Tab2();
            using (var db = base.NewDB())
            {
                try
                {
                    //var E = db.A5dot1Tab2.Where(a => a.sbCode == a5dot1Tab2.sbCode).First();
                    //if (E!=null)
                    //{
                    //    a5dot1Tab2.notGoodContents += E.notGoodContents;
                    //    a5dot1Tab2.timesNotGood += 1;
                    //    db.SaveChanges();
                    //}
                    //else
                    //{
                    db.A5dot1Tab2.Add(a5dot1Tab2);
                    db.SaveChanges();
                    //}
                    return true;
                }
                catch (Exception e)
                {
                    return false;
                }
            }
        }
        public bool setstate_byid(int id, int state, string name)
        {

            using (var db = base.NewDB())
            {
                try
                {
                    var E = db.A5dot1Tab2.Where(a => a.Id == id).First();

                    E.state = state;
                    if (state == 2)
                        E.jxdwUserName = name;
                    else
                        E.jdcUserName = name;
                    db.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    return false;
                }
            }
        }
        public bool setisworst10_byid(int id, int isworst10)
        {

            using (var db = base.NewDB())
            {
                try
                {
                    var E = db.A5dot1Tab2.Where(a => a.Id == id).First();

                    E.isSetAsTop10Worst = isworst10;

                    db.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    return false;
                }
            }
        }

        public List<A5dot1Tab1> Getdcl_listbyisZG(int isRectified, List<string> cjname)
        {
            using (var db = base.NewDB())
            {
                List<A5dot1Tab1> E = new List<A5dot1Tab1>();
                try
                {
                    foreach (var cn in cjname)
                    {
                        var e = db.A5dot1Tab1.Where(a => a.isRectified == isRectified && a.cjName == cn).ToList();
                        E.AddRange(e);
                    }
                    return E;
                }
                catch (Exception e)
                {
                    return null;
                }

            }
        }
        public List<A5dot1Tab1> GetAll1_byym(string ym, List<string> cjname)
        {
            using (var db = base.NewDB())
            {
                List<A5dot1Tab1> E = new List<A5dot1Tab1>();
                try
                {
                    foreach (var cn in cjname)
                    {
                        var e = db.A5dot1Tab1.Where(a => a.yearMonthForStatistic == ym && a.cjName == cn).ToList();
                        E.AddRange(e);
                    }
                    return E;
                }
                catch (Exception e)
                {
                    return null;
                }

            }
        }
        public List<A5dot1Tab1> Getbwh_byname(string name, int code)
        {
            using (var db = base.NewDB())
            {
                try
                {
                    var E = new List<A5dot1Tab1>();
                    if (code == 1)
                    {
                        E = db.A5dot1Tab1.Where(a => a.cjName == name).ToList();
                    }
                    else if (code == 2)
                    {
                        E = db.A5dot1Tab1.Where(a => a.pqName == name).ToList();
                    }
                    else if (code == 3)
                    {
                        E = db.A5dot1Tab1.ToList();
                    }
                    else
                        E = db.A5dot1Tab1.Where(a => a.cjName == name).ToList();
                    return E;
                }
                catch (Exception e)
                {
                    return null;
                }

            }
        }

        public List<A5dot1Tab2> GetAll2_byymandstate(string ym, int state)
        {
            using (var db = base.NewDB())
            {
                try
                {
                    var E = db.A5dot1Tab2.Where(a => a.yearMonthForStatistic == ym && a.state == state).ToList();
                    return E;
                }
                catch (Exception e)
                {
                    return null;
                }

            }
        }
        public List<A5dot1Tab2> GetAll_byymandcode(string ym, string sbCode)
        {
            using (var db = base.NewDB())
            {
                try
                {
                    var E = db.A5dot1Tab2.Where(a => a.yearMonthForStatistic == ym && a.sbCode == sbCode).ToList();
                    return E;
                }
                catch (Exception e)
                {
                    return null;
                }

            }
        }
        public List<A5dot1Tab1> GetAll1_byymandcode(string ym, string sbCode)
        {
            using (var db = base.NewDB())
            {
                try
                {
                    var E = db.A5dot1Tab1.Where(a => a.yearMonthForStatistic == ym && a.sbCode == sbCode).ToList();
                    return E;
                }
                catch (Exception e)
                {
                    return null;
                }

            }
        }
        public List<A5dot1Tab1> GetAll1_bycode(string sbCode)
        {
            using (var db = base.NewDB())
            {
                try
                {
                    var E = db.A5dot1Tab1.Where(a => a.sbCode == sbCode).ToList();
                    return E;
                }
                catch (Exception e)
                {
                    return null;
                }

            }
        }
        public A5dot1Tab1 GetA_byid(int a_id)
        {
            using (var db = base.NewDB())
            {
                try
                {
                    var E = db.A5dot1Tab1.Where(a => a.Id == a_id).First();
                    return E;
                }
                catch (Exception e)
                {
                    return null;
                }

            }

        }

        public bool Pqcheck_byid(int a_id, string pqname, DateTime time)
        {

            using (var db = base.NewDB())
            {
                try
                {
                    var E = db.A5dot1Tab1.Where(a => a.Id == a_id).First();
                    E.isRectified = 1;
                    E.pqUserName = pqname;
                    E.pqCheckTime = time;
                    db.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    return false;
                }

            }
        }
        public bool modifytNG_byid(int a_id, int tNG)
        {

            using (var db = base.NewDB())
            {
                try
                {
                    var E = db.A5dot1Tab2.Where(a => a.Id == a_id).First();
                    E.timesNotGood = tNG;
                    db.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    return false;
                }

            }
        }
        public bool modifytemp1_byid(int a_id, string temp1)
        {

            using (var db = base.NewDB())
            {
                try
                {
                    var E = db.A5dot1Tab1.Where(a => a.Id == a_id).First();
                    E.temp1 = temp1;
                    db.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    return false;
                }

            }
        }
        public bool delete_byid(int id)
        {
            using (var db = base.NewDB())
            {
                try
                {
                    var E = db.A5dot1Tab2.Where(a => a.Id == id).First();
                    db.A5dot1Tab2.Remove(E);
                    db.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    return false;
                }

            }
        }



        public List<A5dot1Tab1> get_cj_bwh(string cj_name, int equip_num)
        {
            using (var db = base.NewDB())
            {
                try
                {
                    var E = db.A5dot1Tab1.Where(a => a.cjName == cj_name).ToList();
                    //var bwh = Math.Round(((double)E.Count/equip_num),6);
                    return E;
                }
                catch (Exception e)
                {
                    return null;
                }

            }

        }
        public List<A5dot1Tab1> get_pq_bwh(string pq_name, int equip_num)
        {
            using (var db = base.NewDB())
            {
                try
                {
                    if (pq_name != "其他")
                    {
                        var E = db.A5dot1Tab1.Where(a => a.pqName == pq_name).ToList();
                        //var bwh = Math.Round(((double)E.Count / equip_num), 6);
                        return E;
                    }
                    else
                    {
                        var E = db.A5dot1Tab1.Where(a => a.pqName == "消防队" || a.pqName == "计量站").ToList();
                        // var bwh = Math.Round(((double)E.Count / equip_num), 6);
                        return E;

                    }
                }
                catch (Exception e)
                {
                    return null;
                }

            }

        }

        public List<A5dot1Tab2> get_worst10byym(string ym)
        {

            using (var db = base.NewDB())
            {
                try
                {
                    var E = db.A5dot1Tab2.Where(a => a.yearMonthForStatistic == ym && a.state == 3).ToList();

                    return E;
                }
                catch (Exception e)
                {
                    return null;
                }

            }
        }


        public List<A5dot1Tab1> get_All()
        {

            using (var db = base.NewDB())
            {
                try
                {
                    var E = db.A5dot1Tab1.ToList();

                    return E;
                }
                catch (Exception e)
                {
                    return null;
                }

            }
        }
        public List<A5dot1Tab1> getrecordbyentity(string entity_id)
        {
            using (var db = base.NewDB())
            {
                try
                {
                    var E = db.A5dot1Tab1.Where(a => a.temp2 == entity_id).ToList();

                    return E;
                }
                catch (Exception e)
                {
                    return null;
                }

            }
        }
    }
}

using EquipDAL.Interface;
using EquipModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipDAL.Implement
{
    public class Sxgl : BaseDAO
    {
        public bool AddSxRecord(A5dot2Tab1 nVal)
        {
            using (var db = base.NewDB())
            {
                try
                {
                    db.A5dot2Tab1.Add(nVal);
                    db.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }
        public List<A5dot2Tab1> GetSxRecord(List<string> cjname)
        {
            using (var db = base.NewDB())
            {
                List<A5dot2Tab1> E = new List<A5dot2Tab1>();
                foreach (var cn in cjname)
                {
                    var e = db.A5dot2Tab1.Where(a => a.state == 0 && a.cjName == cn).ToList();
                    E.AddRange(e);
                }
                return E;
            }
        }
        public List<A5dot2Tab1> GetSxRecord_detail(int id)
        {
            using (var db = base.NewDB())
            {
                return db.A5dot2Tab1.Where(a => a.Id == id).ToList();
            }
        }
        public A5dot2Tab1 getAllbyid(int a_id)
        {
         
          using (var db = base.NewDB())
            {
                return db.A5dot2Tab1.Where(a => a.Id ==a_id).First();
            }
         
         }
        public string ModifySxRecord(A5dot2Tab1 nVal)
        {
            using (var db = base.NewDB())
            {
                try
                {
                    var modifyA5dot2Tab1 = db.A5dot2Tab1.Where(a => a.Id == nVal.Id).First();
                    modifyA5dot2Tab1.pqCheckTime = nVal.pqCheckTime;
                    modifyA5dot2Tab1.pqUserName = nVal.pqUserName;
                    modifyA5dot2Tab1.isRectified = nVal.isRectified;
                    modifyA5dot2Tab1.state = nVal.state;
                    modifyA5dot2Tab1.problemDescription = nVal.problemDescription;

                    db.SaveChanges();
                    return modifyA5dot2Tab1.sbCode;
                }
                catch
                {
                    return "出错！";
                }
            }
        }

        public string ModifySxRecord1(A5dot2Tab1 nVal)
            {
            using (var db = base.NewDB())
                {
                try
                    {
                    var modifyA5dot2Tab1 = db.A5dot2Tab1.Where(a => a.Id == nVal.Id).First();
                    modifyA5dot2Tab1.pqCheckTime = nVal.pqCheckTime;
                    modifyA5dot2Tab1.pqUserName = nVal.pqUserName;
                    modifyA5dot2Tab1.isRectified = nVal.isRectified;
                    modifyA5dot2Tab1.state = nVal.state;
                    

                    db.SaveChanges();
                    return modifyA5dot2Tab1.sbCode;
                    }
                catch
                    {
                    return "出错！";
                    }
                }
            }
        public List<A5dot2Tab2> GetA5dot2Tab2(string res)
        {
            try
            {
                using (var db = base.NewDB())
                {
                    return db.A5dot2Tab2.Where(a => a.sbCode == res).ToList();
                }
            }
            catch(Exception e)
            {
                return null;
            }
        }
        public bool AddA5dot2Tab2(A5dot2Tab2 nVal)
        {
            using (var db = base.NewDB())
            {
                try
                {
                    db.A5dot2Tab2.Add(nVal);
                    db.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public bool ModifyA5dot2Tab2(A5dot2Tab2 nVal)
        {
            using (var db = base.NewDB())
            {
                try
                {
                    List<A5dot2Tab2> modifyA5dot2Tab2 = db.A5dot2Tab2.Where(a => a.sbCode == nVal.sbCode).ToList();
                    
                    string[] yearandmonth = (nVal.yearMonthForStatistic).Split(new char[] { '年' });
                    int key = 0;
                   
                        for (int k = 0; k < modifyA5dot2Tab2.Count;k++ )
                        {
                            string[] YandMfromDB = (modifyA5dot2Tab2[k].yearMonthForStatistic).Split(new char[] { '年' });
                            if (YandMfromDB[1] == yearandmonth[1] && YandMfromDB[0] == yearandmonth[0])//年月一样
                            {
                                modifyA5dot2Tab2[k].problemDescription = modifyA5dot2Tab2[k].problemDescription + "$" + nVal.problemDescription;
                                modifyA5dot2Tab2[k].zyType = modifyA5dot2Tab2[k].zyType + "$" + nVal.zyType;
                                modifyA5dot2Tab2[k].nProblemsInCurMonth++;
                                modifyA5dot2Tab2[k].nProblemsInCurYear++;
                                key = 1;
                            }
                           
                        }
                        
                    
                    if(key==0)
                    { 
                        db.A5dot2Tab2.Add(nVal);//年一样月不一样或年不一样月一样或年月不一样
                    }


                    //string[] YandMfromDB = (modifyA5dot2Tab2.yearMonthForStatistic).Split(new char[] { '年' });
                    //if (YandMfromDB[1] == yearandmonth[1] && YandMfromDB[0] == yearandmonth[0])
                    //{
                    //    modifyA5dot2Tab2.problemDescription = modifyA5dot2Tab2.problemDescription + "$" + nVal.problemDescription;
                    //    modifyA5dot2Tab2.zyType = modifyA5dot2Tab2.zyType + "$" + nVal.zyType;
                    //    modifyA5dot2Tab2.nProblemsInCurMonth++;
                    //    modifyA5dot2Tab2.nProblemsInCurYear++;
                    //}
                    //else if (YandMfromDB[1] != yearandmonth[1] && YandMfromDB[0] == yearandmonth[0])
                    //{
                        
                    //    db.A5dot2Tab2.Add(nVal);                       
                    //}
                    db.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        //片区汇总筛选10台最脏设备
        public List<A5dot2Tab2> GetA5dot2Tab2ForPq(string yearandmonth)
        {
            try
            {
                using (var db = base.NewDB())
                {
                    return db.A5dot2Tab2.Where(a => a.yearMonthForStatistic == yearandmonth).ToList();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        //单台设备全年问题次数
        public int GetnProblemsInYear(string sbCode)
        {
            try
            {
                using (var db=base.NewDB())
                {
                    int nprobleminyear = 0;
                    List<A5dot2Tab2> c = db.A5dot2Tab2.Where(a => a.sbCode == sbCode).ToList();
                    foreach(var item in c)
                    {
                        nprobleminyear = nprobleminyear + item.nProblemsInCurMonth;
                    }
                    return nprobleminyear;
                }
            }
            catch(Exception e )
            {
                return 0;
            }
        }
        //将选中的最脏设备更新到库
        public bool SetWorse(int id,string user)
        {
            try
            {
                using (var db = base.NewDB())
                {
                    var setworse = db.A5dot2Tab2.Where(a => a.Id == id).First();
                    setworse.isSetAsTop10Worst = 1;
                    setworse.jdcOperateTime = DateTime.Now;
                    setworse.jdcUserName = user;
                    db.SaveChanges();
                }
                
                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }
        //查出本月最脏十台
        public List<A5dot2Tab2> jdcsummy(string yearandmonth)
        {
            try
            {
                using (var db = base.NewDB())
                {
                    return db.A5dot2Tab2.Where(a => a.yearMonthForStatistic == yearandmonth && a.isSetAsTop10Worst==1).ToList();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }
        //查出最脏设备未整改问题
        public List<A5dot2Tab1> uncomp(string sbCode ,DateTime time)
        {
            try
            {
                using (var db = base.NewDB())
                {
                    return db.A5dot2Tab1.Where(a => a.sbCode == sbCode && a.isRectified == 0 && a.pqCheckTime<time).ToList();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public string forlastmonthproblem(string lastmonth, string sbCode)
        {
            try
            {
                using (var db = base.NewDB())
                {
                    var i= db.A5dot2Tab2.Where(a => a.yearMonthForStatistic == lastmonth && a.sbCode == sbCode).First();
                    string lastmonthproblem = i.problemDescription;
                    return lastmonthproblem;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }


        public List<A5dot2Tab1> GetLS_listbywfe_id(List<int> wfe_id)
            {
            using (var db = base.NewDB())
                {
                List<A5dot2Tab1> E = new List<A5dot2Tab1>();
                try
                    {
                    foreach (var w in wfe_id)
                        {
                        var e = db.A5dot2Tab1.Where(a => a.temp2 == w.ToString()).ToList();
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

    }
}

using System.Collections.Generic;
using System.Linq;
using EquipModel.Entities;
using System;

namespace EquipDAL.Implement
{
    public class A6dot2Tabs : BaseDAO
    {
        public class WDT_PqTj
        {
            public string pqName;
            public int tjNum;
        }

        public class WDT_CjTj
        {
            public string pqName;
            public string cjName;
            public int tjNum;
        }

        public class WDT_ZzTj
        {
            public string pqName;
            public string cjName;
            public string zzName;
            public int tjNum;
        }

        public A6dot2Tab1 get_WDTInfo(int Id)
        {
            using (var db = base.NewDB())
            {
                return db.A6dot2Tab1.Where(a => a.Id == Id).First();

            }
        }

        public List<EquipModel.Entities.A6dot2Tab1> getWDTList(string startTime, string endTime)
        {
            using (var db = base.NewDB())
            {
                return db.A6dot2Tab1.Where(a => a.uploadtime.CompareTo(startTime) > 0 && a.uploadtime.CompareTo(endTime) < 0).ToList();

            }
        }

        public List<EquipModel.Entities.A6dot2Tab2> getCQList(string pqName, string startTime, string endTime)
        {
            using (var db = base.NewDB())
            {
                return db.A6dot2Tab2.Where(x => x.equip_PqName == pqName && x.isValid == 1 && x.isOilType == 1 && x.isExceed == 1 && x.Tab1_Belong.uploadtime.CompareTo(startTime) >= 0 && x.Tab1_Belong.uploadtime.CompareTo(endTime) <= 0).ToList();

            }
        }
        
        public List<EquipModel.Entities.A6dot2Tab2> getCQListId(int Id,string pqName )
        {
            using (var db = base.NewDB())
            {
                return db.A6dot2Tab2.Where(x => x.equip_PqName == pqName && x.Tab1_Id == Id && x.isValid == 1 && x.isOilType == 1 && x.isExceed == 1).ToList();

            }
        }

        public List<EquipModel.Entities.A6dot2Tab1> getWDTList()
        {
            using (var db = base.NewDB())
            {
                return db.A6dot2Tab1.ToList();

            }
        }

        public List<WDT_PqTj> PqGroup(string starttime, string endtime)
        {

            using (var db = base.NewDB())
            {


                var a = db.A6dot2Tab2.Where(x => x.isValid == 1 && x.isOilType == 1 && x.Tab1_Belong.uploadtime.CompareTo(starttime) >= 0 && x.Tab1_Belong.uploadtime.CompareTo(endtime) <= 0).GroupBy(x => new { x.equip_PqName }).Select(g => new { PqName = g.Key.equip_PqName, ExceedNum = g.Sum(x => x.isExceed) });
                //var b = db.A6dot2Tab2.Where(x => x.isValid == 1 && x.isOilType == 1).First().Tab1_Belong.uploadtime;

                List<WDT_PqTj> r = new List<WDT_PqTj>();
                foreach (var item in a)
                {
                    WDT_PqTj tmp = new WDT_PqTj();
                    tmp.pqName = item.PqName;
                    tmp.tjNum = item.ExceedNum;
                    r.Add(tmp);
                }
                return r;
            }
        }


        public List<WDT_PqTj> PqGroup(int Id)
        {

            using (var db = base.NewDB())
            {


                var a = db.A6dot2Tab2.Where(x => x.isValid == 1 && x.isOilType == 1 && x.Tab1_Id == Id).GroupBy(x => new { x.equip_PqName }).Select(g => new { PqName = g.Key.equip_PqName, ExceedNum = g.Sum(x => x.isExceed) });
                List<WDT_PqTj> r = new List<WDT_PqTj>();
                foreach (var item in a)
                {
                    WDT_PqTj tmp = new WDT_PqTj();
                    tmp.pqName = item.PqName;
                    tmp.tjNum = item.ExceedNum;
                    r.Add(tmp);
                }
                return r;
            }
        }


        public List<WDT_CjTj> CjGroup(string starttime, string endtime)
        {

            using (var db = base.NewDB())
            {

                var a = db.A6dot2Tab2.Where(x => x.isValid == 1 && x.isOilType == 1 && x.Tab1_Belong.uploadtime.CompareTo(starttime) >= 0 && x.Tab1_Belong.uploadtime.CompareTo(endtime) <= 0).GroupBy(x => new { x.equip_PqName, x.equip_CjName }).Select(g => new { PqName = g.Key.equip_PqName, cjName = g.Key.equip_CjName, ExceedNum = g.Sum(x => x.isExceed) });
                List<WDT_CjTj> r = new List<WDT_CjTj>();
                foreach (var item in a)
                {
                    WDT_CjTj tmp = new WDT_CjTj();
                    tmp.pqName = item.PqName;
                    tmp.cjName = item.cjName;
                    tmp.tjNum = item.ExceedNum;
                    r.Add(tmp);
                }
                return r;
            }
        }
        public List<WDT_CjTj> CjGroup(int Id)
        {

            using (var db = base.NewDB())
            {

                var a = db.A6dot2Tab2.Where(x => x.isValid == 1 && x.isOilType == 1 && x.Tab1_Id == Id).GroupBy(x => new { x.equip_PqName, x.equip_CjName }).Select(g => new { PqName = g.Key.equip_PqName, cjName = g.Key.equip_CjName, ExceedNum = g.Sum(x => x.isExceed) });
                List<WDT_CjTj> r = new List<WDT_CjTj>();
                foreach (var item in a)
                {
                    WDT_CjTj tmp = new WDT_CjTj();
                    tmp.pqName = item.PqName;
                    tmp.cjName = item.cjName;
                    tmp.tjNum = item.ExceedNum;
                    r.Add(tmp);
                }
                return r;
            }
        }


        public List<WDT_ZzTj> ZzGroup(string starttime, string endtime)
        {

            using (var db = base.NewDB())
            {
                var a = db.A6dot2Tab2.Where(x => x.isValid == 1 && x.isOilType == 1 && x.Tab1_Belong.uploadtime.CompareTo(starttime) >= 0 && x.Tab1_Belong.uploadtime.CompareTo(endtime) <= 0).GroupBy(x => new { x.equip_PqName, x.equip_CjName, x.equip_ZzName }).Select(g => new { PqName = g.Key.equip_PqName, cjName = g.Key.equip_CjName, zzName = g.Key.equip_ZzName, ExceedNum = g.Sum(x => x.isExceed) });
                List<WDT_ZzTj> r = new List<WDT_ZzTj>();
                foreach (var item in a)
                {
                    WDT_ZzTj tmp = new WDT_ZzTj();
                    tmp.pqName = item.PqName;
                    tmp.cjName = item.cjName;
                    tmp.zzName = item.zzName;
                    tmp.tjNum = item.ExceedNum;
                    r.Add(tmp);
                }
                return r;
            }
        }
        public List<WDT_ZzTj> ZzGroup(int Id)
        {

            using (var db = base.NewDB())
            {
                var a = db.A6dot2Tab2.Where(x => x.isValid == 1 && x.isOilType == 1 && x.Tab1_Id == Id).GroupBy(x => new { x.equip_PqName, x.equip_CjName, x.equip_ZzName }).Select(g => new { PqName = g.Key.equip_PqName, cjName = g.Key.equip_CjName, zzName = g.Key.equip_ZzName, ExceedNum = g.Sum(x => x.isExceed) });
                List<WDT_ZzTj> r = new List<WDT_ZzTj>();
                foreach (var item in a)
                {
                    WDT_ZzTj tmp = new WDT_ZzTj();
                    tmp.pqName = item.PqName;
                    tmp.cjName = item.cjName;
                    tmp.zzName = item.zzName;
                    tmp.tjNum = item.ExceedNum;
                    r.Add(tmp);
                }
                return r;
            }
        }
        public A6dot2Tab1 Add_A6dot2Tab1(A6dot2Tab1 New_Tab1)
        {
            try
            {
                using (var db = base.NewDB())
                {
                    A6dot2Tab1 newP = db.A6dot2Tab1.Add(New_Tab1);
                    db.SaveChanges();
                    return newP;
                }
            }
            catch
            {
                return null;
            }

        }



        public int Add_A6dot2Tab2(int id, List<A6dot2Tab2> New_Tab2s)
        {
            try
            {
                using (var db = base.NewDB())
                {

                    var p = db.A6dot2Tab1.Where(a => a.Id == id).First();
                    foreach (var item in New_Tab2s)
                    { p.WDTable_details.Add(item); }
                    db.SaveChanges();
                    return 1;
                }
            }
            catch
            {
                return 0;
            }

        }
        //向File_Info中写数据
        public bool AddFiletoCatalog(int pID, File_Info nf)
        {
            try
            {
                using (var db = base.NewDB())
                {
                    db.FCatalogs.Where(s => s.Catalog_Id == pID).First().Files_Included.Add(nf);
                    db.SaveChanges();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
        //通过上传时间找到File_Id(其实可以与AddFiletoCatalog合并，返回File_Id)
        public int getfileid(DateTime time)
        {
            try
            {
                using (var db = base.NewDB())
                {

                    var p = db.Files.Where(a => a.File_UploadTime == time).First().File_Id;
                    return p;
                }
            }
            catch
            {
                return 0;
            }

        }
        /// <summary>
        /// 通过设备编号来获得Equip_Id
        /// </summary>
        /// <param name="sbcode"></param>
        /// <returns></returns>
        public int getequipid(string sbcode)
        {
            try
            {
                using (var db = base.NewDB())
                {

                    var p = db.Equips.Where(a => a.Equip_Code == sbcode).First().Equip_Id;
                    return p;
                }
            }
            catch
            {
                return 0;
            }

        }

        public List<EQ> getguicheng()
        {
            List<EQ> c = new List<EQ>();

            try
            {
                using (var db = base.NewDB())
                {

                    List<File_Info> p = db.Files.Where(a => a.Self_Catalog.Catalog_Id == 28).ToList();//跨表查询Self_Catalog.Catalog_Id
                    for (int i = 0; i < p.Count; i++)
                    {
                        for (int k = 0; k < p[i].File_Equips.Count; k++)//存在文件对应的设备才进入，File_Equips.Count只可能等于一
                        {
                            Equip_Info[] zz = new Equip_Info[1];
                            p[i].File_Equips.CopyTo(zz, 0);
                            EQ temp = new EQ();

                            temp.sbGyCode = zz[0].Equip_GyCode;
                            temp.sbCode = zz[0].Equip_Code;
                            temp.sbType = zz[0].Equip_Type;
                            temp.GCnewname = p[i].File_NewName;
                            temp.GColdname = p[i].File_OldName;

                            c.Add(temp);
                        }

                    }

                    return c;
                }
            }
            catch
            {
                return c;
            }

        }
        
    }
}
using EquipDAL.Interface;
using EquipModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipDAL.Implement
{
    public class Kpi : BaseDAO
    {

        public List<A15dot1TabQiYe> GetJxRecord(string roles, string dep, string name,List<string> cjname)
        {
            List<A15dot1TabQiYe> e = new List<A15dot1TabQiYe>();
            List<A15dot1TabQiYe> temp = new List<A15dot1TabQiYe>();
            using (var db = base.NewDB())
            {
                if (dep.Contains("机动处"))
                {
                    if (roles.Contains("可靠性工程师") )
                    {
                        return db.A15dot1TabQiYe.Where(a => (a.state == 1 || a.state == 2 || a.state == 0) && a.submitUser == name).ToList();
                    }
                    else
                        return db.A15dot1TabQiYe.Where(a => a.state == 1 || a.state == 2).ToList();
                }
                else if (roles.Contains("可靠性工程师"))
                {
                    foreach (var cj in cjname)
                    {
                        List<A15dot1TabQiYe> t = db.A15dot1TabQiYe.Where(a => a.state == 0 && a.temp3 == cj).ToList();
                       foreach(var tt in t)
                       {
                           temp.Add(tt);
                       }                       
                    }
                    return temp;
                }
                else
                    return e;
            }
        }
        public List<A15dot1TabQiYe> GetJxRecord_detail(int id)
        {
            using (var db = base.NewDB())
            {
                return db.A15dot1TabQiYe.Where(a => a.Id == id).ToList();
            }
        }
        public List<A15dot1TabQiYe> GetHisJxRecord(string roles, string dep, string name)
        {
            List<A15dot1TabQiYe> e = new List<A15dot1TabQiYe>();
            List<A15dot1TabQiYe>temp=new List<A15dot1TabQiYe>();
            using (var db = base.NewDB())
            {
                if (dep.Contains("机动处"))
                {
                    return db.A15dot1TabQiYe.Where(a => a.state == 3).ToList();
                }
                else if (roles.Contains("可靠性工程师") )
                {
                    return db.A15dot1TabQiYe.Where(a => a.state == 3 && a.submitUser == name).ToList();
                }
                else
                    return e;

            }
        }
        public List<A15dot1TabQiYe> GetHisJxRecord_detail(int id)
        {
            using (var db = base.NewDB())
            {
                return db.A15dot1TabQiYe.Where(a => a.Id == id).ToList();
            }
        }
        public List<A15dot1TabDian> GetJxRecord_detailDian(int id)
        {
            using (var db = base.NewDB())
            {
                return db.A15dot1TabDian.Where(a => a.Id == id).ToList();
            }
        }
        public List<object> qstdata(string grahpic_name, string zz)
        {
            using (var db = base.NewDB())
            {
                var i = db.Database.SqlQuery<A15dot1TabQiYe>("select * from A15dot1TabQiYe where submitDepartment='" + zz + "'").ToList();

                List<object> a = new List<object>();
                foreach (var item in i)
                {
                    var t = item.GetType().GetProperty(grahpic_name);
                    var result = t.GetValue(item, null);
                    a.Add(result);
                }
                object cut = new object();
                cut = "$$";
                a.Add(cut);
                foreach (var ex in i)
                {
                    var t = ex.GetType().GetProperty("jdcOperateTime");
                    var result = t.GetValue(ex, null);
                    a.Add(result);
                }


                return a;
            }
        }
        /// <summary>
        /// 趋势图的方法
        /// </summary>
        /// <param name="grahpic_name">字段名</param>
        /// <param name="zz">装置</param>
        /// <param name="zy">专业</param>
        /// <returns></returns>
        public List<object> qstdataQiYe(string grahpic_name, string zzId,string zy)
        {
            using (var db = base.NewDB())
            {
                var i = db.Database.SqlQuery<A15dot1TabQiYe>("select * from A15dot1TabQiYe where temp2='"+zy+"'and submitDepartment='" + zzId + "'").ToList();

                List<object> a = new List<object>();
                foreach (var item in i)
                {
                    var t = item.GetType().GetProperty(grahpic_name);
                    var result = t.GetValue(item, null);
                    a.Add(result);
                }
                object cut = new object();
                cut = "$$";
                a.Add(cut);
                foreach (var ex in i)
                {
                    var t = ex.GetType().GetProperty("submitDepartment");
                    var result = t.GetValue(ex, null);
                    a.Add(result);
                }


                return a;
            }
        }



        public List<object> qstdataqc(string grahpic_name, string graphic_zdm, string graphic_zy, string graphic_subdipartment)//全厂的趋势图
        {
            using (var db = base.NewDB())
            {
                List<object> a = new List<object>();
                object cut = new object();
                cut = "$$";
                if (graphic_zy == "企业级")//查询企业级的qst
                {
                    var i = db.Database.SqlQuery<A15dot1TabQiYe>("select * from A15dot1TabQiYe where  submitDepartment='" + graphic_subdipartment + "'").ToList();
                    
                    foreach (var item in i)
                    {
                        var t = item.GetType().GetProperty(graphic_zdm);
                        var result = t.GetValue(item, null);
                        a.Add(result);
                    }
                    
                    a.Add(cut);
                    foreach (var ex in i)
                    {
                        var t = ex.GetType().GetProperty("submitTime");
                        var result = t.GetValue(ex, null);
                        a.Add(result);
                    }


                    return a;
                }
                else if (graphic_zy == "动设备专业")//查询企业级的qst
                {
                    var i = db.Database.SqlQuery<A15dot1TabDong>("select * from A15dot1TabDong where submitDepartment='" + graphic_subdipartment + "'").ToList();

                    foreach (var item in i)
                    {
                        var t = item.GetType().GetProperty(graphic_zdm);
                        var result = t.GetValue(item, null);
                        a.Add(result);
                    }

                    a.Add(cut);
                    foreach (var ex in i)
                    {
                        var t = ex.GetType().GetProperty("submitTime");
                        var result = t.GetValue(ex, null);
                        a.Add(result);
                    }


                    return a;
                }
                else if (graphic_zy == "静设备专业")//查询企业级的qst
                {
                    var i = db.Database.SqlQuery<A15dot1TabJing>("select * from A15dot1TabJing where  submitDepartment='" + graphic_subdipartment + "'").ToList();

                    foreach (var item in i)
                    {
                        var t = item.GetType().GetProperty(graphic_zdm);
                        var result = t.GetValue(item, null);
                        a.Add(result);
                    }

                    a.Add(cut);
                    foreach (var ex in i)
                    {
                        var t = ex.GetType().GetProperty("submitTime");
                        var result = t.GetValue(ex, null);
                        a.Add(result);
                    }


                    return a;
                }
                else if (graphic_zy == "电气专业")//查询企业级的qst
                {
                    var i = db.Database.SqlQuery<A15dot1TabDian>("select * from A15dot1TabDian where  submitDepartment='" + graphic_subdipartment + "'").ToList();

                    foreach (var item in i)
                    {
                        var t = item.GetType().GetProperty(graphic_zdm);
                        var result = t.GetValue(item, null);
                        a.Add(result);
                    }

                    a.Add(cut);
                    foreach (var ex in i)
                    {
                        var t = ex.GetType().GetProperty("submitTime");
                        var result = t.GetValue(ex, null);
                        a.Add(result);
                    }


                    return a;
                }
                else if (graphic_zy == "仪表专业")//查询企业级的qst
                {
                    var i = db.Database.SqlQuery<A15dot1TabYi>("select * from A15dot1TabYi where  submitDepartment='" + graphic_subdipartment + "'").ToList();

                    foreach (var item in i)
                    {
                        var t = item.GetType().GetProperty(graphic_zdm);
                        var result = t.GetValue(item, null);
                        a.Add(result);
                    }

                    a.Add(cut);
                    foreach (var ex in i)
                    {
                        var t = ex.GetType().GetProperty("submitTime");
                        var result = t.GetValue(ex, null);
                        a.Add(result);
                    }


                    return a;
                }
                else
                {
                    return a;
                }

                
            }

        }

        public bool ModifyJxRecord(A15dot1TabQiYe nVal)
        {
            using (var db = base.NewDB())
            {
                try
                {
                    var modifyA15QiYe = db.A15dot1TabQiYe.Where(a => a.Id == nVal.Id).First();

                    modifyA15QiYe.zzkkxzs = nVal.zzkkxzs;
                    modifyA15QiYe.wxfyzs = nVal.wxfyzs;
                    modifyA15QiYe.qtlxbmfxhl = nVal.qtlxbmfxhl;
                    modifyA15QiYe.qtlhsbgsghl = nVal.qtlhsbgsghl;
                    modifyA15QiYe.ybsjkzl = nVal.ybsjkzl;
                    modifyA15QiYe.sjs = nVal.sjs;
                    modifyA15QiYe.gzqdkf = nVal.gzqdkf;
                    modifyA15QiYe.xmyql = nVal.xmyql;
                    modifyA15QiYe.pxjnl = nVal.pxjnl;
                    modifyA15QiYe.submitDepartment = nVal.submitDepartment;
                    modifyA15QiYe.temp2 = nVal.temp2;
                    modifyA15QiYe.Id = nVal.Id;
                    modifyA15QiYe.reliabilityConclusion = nVal.reliabilityConclusion;
                    modifyA15QiYe.state = nVal.state;
                    //modifyA15QiYe.submitTime = nVal.submitTime;
                    //modifyA15QiYe.submitUser = nVal.submitUser;

                    db.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }//更新企业级表装置的内容
        public bool ModifyQcQyJxItem(A15dot1TabQiYe nVal,DateTime stime,DateTime etime)
        {
            using (var db = base.NewDB())
            {
                try
                {
                  
                    var modifyA15QiYeList = db.A15dot1TabQiYe.Where(a => a.submitDepartment == nVal.submitDepartment&&a.submitTime>stime&&a.submitTime<etime).ToList();
                    if (modifyA15QiYeList.Count>0)
                    {
                        int id = modifyA15QiYeList.First().Id;
                        var modifyA15QiYe = db.A15dot1TabQiYe.Where(a => a.Id == id).First();
                        modifyA15QiYe.zzkkxzs = nVal.zzkkxzs;
                        modifyA15QiYe.wxfyzs = nVal.wxfyzs;
                        modifyA15QiYe.qtlxbmfxhl = nVal.qtlxbmfxhl;
                        modifyA15QiYe.qtlhsbgsghl = nVal.qtlhsbgsghl;
                        modifyA15QiYe.ybsjkzl = nVal.ybsjkzl;
                        modifyA15QiYe.sjs = nVal.sjs;
                        modifyA15QiYe.gzqdkf = nVal.gzqdkf;
                        modifyA15QiYe.xmyql = nVal.xmyql;
                        modifyA15QiYe.pxjnl = nVal.pxjnl;
                      
               
                       
                        //modifyA15QiYe.submitUser = nVal.submitUser;
                        db.SaveChanges();
                        return true;
                    }
                    else
                    {
                        db.A15dot1TabQiYe.Add(nVal);
                        db.SaveChanges();
                        return true;
                    }


                }
                catch(Exception e)
                {
                    return false;
                }
            }
        }
        public bool ModifyQcDongJxItem(A15dot1TabDong nVal, DateTime stime, DateTime etime)
        {
            using (var db = base.NewDB())
            {
                try
                {

                    var modifyA15DongList = db.A15dot1TabDong.Where(a => a.submitDepartment == nVal.submitDepartment && a.submitTime > stime && a.submitTime < etime).ToList();
                    if (modifyA15DongList.Count > 0)
                    {
                        int id = modifyA15DongList.First().Id;
                        var modifyA15Dong = db.A15dot1TabDong.Where(a => a.Id == id).First();
                        modifyA15Dong.gzqdkf = nVal.gzqdkf;
                        modifyA15Dong.djzgzl = nVal.djzgzl;
                        modifyA15Dong.gzwxl = nVal.gzwxl;
                        modifyA15Dong.qtlxbmfxhl = nVal.qtlxbmfxhl;
                        modifyA15Dong.jjqxgsl = nVal.jjqxgsl;
                        modifyA15Dong.gdpjwcsj = nVal.gdpjwcsj;
                        modifyA15Dong.jxmfpjsm = nVal.jxmfpjsm;
                        modifyA15Dong.zcpjsm = nVal.zcpjsm;
                        modifyA15Dong.sbwhl = nVal.sbwhl;
                        modifyA15Dong.jxychgl = nVal.jxychgl;
                        modifyA15Dong.zyjbpjxl = nVal.zyjbpjxl;
                        modifyA15Dong.jzpjxl = nVal.jzpjxl;
                        modifyA15Dong.wfjzgzl = nVal.wfjzgzl;
                        modifyA15Dong.ndbtjbcfjxtc = nVal.ndbtjbcfjxtc;
                        modifyA15Dong.jbpjwgzjgsjMTBF = nVal.jbpjwgzjgsjMTBF;


                        db.SaveChanges();
                        return true;
                    }
                    else
                    {
                        db.A15dot1TabDong.Add(nVal);
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
        public bool ModifyQcJingJxItem(A15dot1TabJing nVal, DateTime stime, DateTime etime)
        {
            using (var db = base.NewDB())
            {
                try
                {

                    var modifyA15JingList = db.A15dot1TabJing.Where(a => a.submitDepartment == nVal.submitDepartment && a.submitTime > stime && a.submitTime < etime).ToList();
                    if (modifyA15JingList.Count > 0)
                    {
                        int id = modifyA15JingList.First().Id;
                        var modifyA15Jing = db.A15dot1TabJing.Where(a => a.Id == id).First();
                        modifyA15Jing.gzqdkf = nVal.gzqdkf;
                        modifyA15Jing.sbfsxlcs = nVal.sbfsxlcs;
                        modifyA15Jing.qtlhsbgs = nVal.qtlhsbgs;
                        modifyA15Jing.gylpjrxl = nVal.gylpjrxl;
                        modifyA15Jing.hrqjxl = nVal.hrqjxl;
                        modifyA15Jing.ylrqdjl = nVal.ylrqdjl;
                        modifyA15Jing.ylgdndjxjhwcl = nVal.ylgdndjxjhwcl;
                        modifyA15Jing.aqfndjyjhwcl = nVal.aqfndjyjhwcl;
                        modifyA15Jing.sbfsjcjhwcl = nVal.sbfsjcjhwcl;
                        modifyA15Jing.jsbjwxychgl = nVal.jsbjwxychgl;


                        db.SaveChanges();
                        return true;
                    }
                    else
                    {
                        db.A15dot1TabJing.Add(nVal);
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
        public bool ModifyQcDianJxItem(A15dot1TabDian nVal, DateTime stime, DateTime etime)
        {
            using (var db = base.NewDB())
            {
                try
                {

                    var modifyA15DianList = db.A15dot1TabDian.Where(a => a.submitDepartment == nVal.submitDepartment && a.submitTime > stime && a.submitTime < etime).ToList();
                    if (modifyA15DianList.Count > 0)
                    {
                        int id = modifyA15DianList.First().Id;
                        var modifyA15Dian = db.A15dot1TabDian.Where(a => a.Id == id).First();
                        modifyA15Dian.gzqdkf = nVal.gzqdkf;
                        modifyA15Dian.dqwczcs = nVal.dqwczcs;
                        modifyA15Dian.jdbhzqdzl = nVal.jdbhzqdzl;
                        modifyA15Dian.sbgzl = nVal.sbgzl;
                        modifyA15Dian.djMTBF = nVal.djMTBF;
                        modifyA15Dian.dzdlsbMTBF = nVal.dzdlsbMTBF;
                        modifyA15Dian.zbglys = nVal.zbglys;
                        modifyA15Dian.dnjlphl = nVal.dnjlphl;


                        db.SaveChanges();
                        return true;
                    }
                    else
                    {
                        db.A15dot1TabDian.Add(nVal);
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
        public bool ModifyQcYiJxItem(A15dot1TabYi nVal, DateTime stime, DateTime etime)
        {
            using (var db = base.NewDB())
            {
                try
                {

                    var modifyA15YiList = db.A15dot1TabYi.Where(a => a.submitDepartment == nVal.submitDepartment && a.submitTime > stime && a.submitTime < etime).ToList();
                    if (modifyA15YiList.Count > 0)
                    {
                        int id = modifyA15YiList.First().Id;
                        var modifyA15Yi = db.A15dot1TabYi.Where(a => a.Id == id).First();
                        modifyA15Yi.gzqdkf = nVal.gzqdkf;
                        modifyA15Yi.lsxtzqdzl = nVal.lsxtzqdzl;
                        modifyA15Yi.kzxtgzcs = nVal.kzxtgzcs;
                        modifyA15Yi.ybkzl = nVal.ybkzl;
                        modifyA15Yi.ybsjkzl = nVal.ybsjkzl;
                        modifyA15Yi.lsxttyl = nVal.lsxttyl;
                        modifyA15Yi.gjkzfmgzcs = nVal.gjkzfmgzcs;
                        modifyA15Yi.kzxtgzbjcs = nVal.kzxtgzbjcs;
                        modifyA15Yi.cgybgzl = nVal.cgybgzl;
                        modifyA15Yi.tjfMDBF = nVal.tjfMDBF;


                        db.SaveChanges();
                        return true;
                    }
                    else
                    {
                        db.A15dot1TabYi.Add(nVal);
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

        public bool ModifyJxRecordDong(A15dot1TabDong nVal)
        {
            using (var db = base.NewDB())
            {
                try
                {
                    var modifyA15Dong = db.A15dot1TabDong.Where(a => a.Id == nVal.Id).First();


                    modifyA15Dong.gzqdkf = nVal.gzqdkf;
                    modifyA15Dong.djzgzl = nVal.djzgzl;
                    modifyA15Dong.gzwxl = nVal.gzwxl;
                    modifyA15Dong.qtlxbmfxhl = nVal.qtlxbmfxhl;
                    modifyA15Dong.jjqxgsl = nVal.jjqxgsl;
                    modifyA15Dong.gdpjwcsj = nVal.gdpjwcsj;
                    modifyA15Dong.jxmfpjsm = nVal.jxmfpjsm;
                    modifyA15Dong.zcpjsm = nVal.zcpjsm;
                    modifyA15Dong.sbwhl = nVal.sbwhl;
                    modifyA15Dong.jxychgl = nVal.jxychgl;
                    modifyA15Dong.zyjbpjxl = nVal.zyjbpjxl;
                    modifyA15Dong.jzpjxl = nVal.jzpjxl;
                    modifyA15Dong.wfjzgzl = nVal.wfjzgzl;
                    modifyA15Dong.ndbtjbcfjxtc = nVal.ndbtjbcfjxtc;
                    modifyA15Dong.jbpjwgzjgsjMTBF = nVal.jbpjwgzjgsjMTBF;                  

                    db.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }//更新企业级表装置的内容

        public bool ModifyJxRecordDian(A15dot1TabDian nVal)
        {
            using (var db = base.NewDB())
            {
                try
                {
                    var modifyA15Dian = db.A15dot1TabDian.Where(a => a.Id == nVal.Id).First();

                    modifyA15Dian.gzqdkf = nVal.gzqdkf;
                    modifyA15Dian.dqwczcs = nVal.dqwczcs;
                    modifyA15Dian.jdbhzqdzl = nVal.jdbhzqdzl;
                    modifyA15Dian.sbgzl = nVal.sbgzl;
                    modifyA15Dian.djMTBF = nVal.djMTBF;
                    modifyA15Dian.zbglys = nVal.zbglys;
                    modifyA15Dian.dnjlphl = nVal.dnjlphl;
             
                  

                    db.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }
        public bool ModifyJxRecordJing(A15dot1TabJing nVal)
        {
            using (var db = base.NewDB())
            {
                try
                {
                    var modifyA15Jing = db.A15dot1TabJing.Where(a => a.Id == nVal.Id).First();

                    modifyA15Jing.gzqdkf = nVal.gzqdkf;
                    modifyA15Jing.sbfsxlcs = nVal.sbfsxlcs;
                    modifyA15Jing.qtlhsbgs = nVal.qtlhsbgs;
                    modifyA15Jing.gylpjrxl = nVal.gylpjrxl;
                    modifyA15Jing.hrqjxl = nVal.hrqjxl;
                    modifyA15Jing.ylrqdjl = nVal.ylrqdjl;
                    modifyA15Jing.ylgdndjxjhwcl = nVal.ylgdndjxjhwcl;
                    modifyA15Jing.aqfndjyjhwcl = nVal.aqfndjyjhwcl;
                    modifyA15Jing.sbfsjcjhwcl = nVal.sbfsjcjhwcl;
                    modifyA15Jing.jsbjwxychgl = nVal.jsbjwxychgl;
              

                    db.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }
        public bool ModifyJxRecordYi(A15dot1TabYi nVal)
        {
            using (var db = base.NewDB())
            {
                try
                {
                    var modifyA15Yi = db.A15dot1TabYi.Where(a => a.Id == nVal.Id).First();

                    modifyA15Yi.gzqdkf = nVal.gzqdkf;
                    modifyA15Yi.lsxtzqdzl = nVal.lsxtzqdzl;
                    modifyA15Yi.kzxtgzcs = nVal.kzxtgzcs;
                    modifyA15Yi.ybkzl = nVal.ybkzl;
                    modifyA15Yi.ybsjkzl = nVal.ybsjkzl;
                    modifyA15Yi.lsxttyl = nVal.lsxttyl;
                    modifyA15Yi.gjkzfmgzcs = nVal.gjkzfmgzcs;
                    modifyA15Yi.kzxtgzbjcs = nVal.kzxtgzbjcs;
                    modifyA15Yi.cgybgzl = nVal.cgybgzl;
                    modifyA15Yi.tjfMDBF = nVal.tjfMDBF;
               

                    db.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }
        public bool AddJxRecord(A15dot1TabQiYe nVal)
        {
            using (var db = base.NewDB())
            {
                try
                {

                    db.A15dot1TabQiYe.Add(nVal);
                    db.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }
        public bool AddDianRecord(A15dot1TabDian nVal)
        {
            using (var db = base.NewDB())
            {
                try
                {

                    db.A15dot1TabDian.Add(nVal);
                    db.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }
        public bool AddJingRecord(A15dot1TabJing nVal)
        {
            using (var db = base.NewDB())
            {
                try
                {

                    db.A15dot1TabJing.Add(nVal);
                    db.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }
        public bool AddYiRecord(A15dot1TabYi nVal)
        {
            using (var db = base.NewDB())
            {
                try
                {

                    db.A15dot1TabYi.Add(nVal);
                    db.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }
        public List<A15dot1TabQiYe> GetQiYeJxByA2(DateTime astime,DateTime aetime, string zzname, string zy)
        {
            DateTime Cxtime = DateTime.Now;
            //string stime;
            //string etime;
            using (var db = base.NewDB())
            {
                
                
                //查出全厂的数据，超级用户修改计算出的全厂的数据后就在这里查询
               
                    return db.A15dot1TabQiYe.Where(a => a.submitDepartment == zzname  && a.submitTime >= astime && a.submitTime <= aetime).ToList();
                    //&& a.submitTime <= Convert.ToDateTime(etime)
               
            }
        }//取企业级表中的数据
        public List<A15dot1TabDong> GetDongJxByA2(DateTime astime, DateTime aetime, string zzname, string zy)
        {
            DateTime Cxtime = DateTime.Now;
            //string stime;
            //string etime;
            using (var db = base.NewDB())
            {
                //查出全厂的数据，超级用户修改计算出的全厂的数据后就在这里查询
                return db.A15dot1TabDong.Where(a => a.submitDepartment == zzname&& a.submitTime >= astime && a.submitTime <= aetime).ToList();
                //&& a.submitTime <= Convert.ToDateTime(etime)

            }
        }
        public List<A15dot1TabJing> GetJingJxByA2(DateTime astime, DateTime aetime, string zzname, string zy)
        {
            DateTime Cxtime = DateTime.Now;
            //string stime;
            //string etime;
            using (var db = base.NewDB())
            {
                //查出全厂的数据，超级用户修改计算出的全厂的数据后就在这里查询
                return db.A15dot1TabJing.Where(a => a.submitDepartment == zzname  && a.submitTime >= astime && a.submitTime <= aetime).ToList();
                //&& a.submitTime <= Convert.ToDateTime(etime)
            }
        }//取静设备专业表中的数据
        public List<A15dot1TabDian> GetDianJxByA2(DateTime astime, DateTime aetime, string zzname, string zy)
        {
            DateTime Cxtime = DateTime.Now;
            //string stime;
            //string etime;
            using (var db = base.NewDB())
            {
                //查出全厂的数据，超级用户修改计算出的全厂的数据后就在这里查询
                return db.A15dot1TabDian.Where(a => a.submitDepartment == zzname  && a.submitTime >= astime && a.submitTime <= aetime).ToList();
                //&& a.submitTime <= Convert.ToDateTime(etime)
            }
        }//取电气专业表中的数据

        public List<A15dot1TabYi> GetYiJxByA2(DateTime astime, DateTime aetime, string zzname, string zy)
        {
            DateTime Cxtime = DateTime.Now;
            //string stime;
            //string etime;
            using (var db = base.NewDB())
            {
                //查出全厂的数据，超级用户修改计算出的全厂的数据后就在这里查询
                return db.A15dot1TabYi.Where(a => a.submitDepartment == zzname &&  a.submitTime >= astime && a.submitTime <= aetime).ToList();
                //&& a.submitTime <= Convert.ToDateTime(etime)
            }
        }//取仪表专业表中的数据
       
        public List<object> qstdata2(string grahpic_name, string zzname, string zy)
        {
            using (var db = base.NewDB())
            {
                var i = db.Database.SqlQuery<A15dot1TabDian>("select * from A15dot1TabDian where submitDepartment='" + zzname + "'and temp2='" + zy + "'").ToList();

                List<object> a = new List<object>();
                foreach (var item in i)
                {
                    var t = item.GetType().GetProperty(grahpic_name);
                    var result = t.GetValue(item, null);
                    a.Add(result);
                }
                object cut = new object();
                cut = "$$";
                a.Add(cut);
                foreach (var ex in i)
                {
                    var t = ex.GetType().GetProperty("jdcOperateTime");
                    var result = t.GetValue(ex, null);
                    a.Add(result);
                }


                return a;
            }
        }//取电气专业表的数据生成趋势图
        public List<object> qstdata3(string grahpic_name, string zzname, string zy)
        {
            using (var db = base.NewDB())
            {
                var i = db.Database.SqlQuery<A15dot1TabYi>("select * from A15dot1TabYi where submitDepartment='" + zzname + "'and temp2='" + zy + "'").ToList();

                List<object> a = new List<object>();
                foreach (var item in i)
                {
                    var t = item.GetType().GetProperty(grahpic_name);
                    var result = t.GetValue(item, null);
                    a.Add(result);
                }
                object cut = new object();
                cut = "$$";
                a.Add(cut);
                foreach (var ex in i)
                {
                    var t = ex.GetType().GetProperty("jdcOperateTime");
                    var result = t.GetValue(ex, null);
                    a.Add(result);
                }


                return a;
            }
        }//取仪表专业表的数据生成趋势图


        public bool GetifQiYeQc(string zy, DateTime stime, DateTime etime)
        {

            using (var db = base.NewDB())
            {


                var A15Tabxiugai = db.A15dot1TabQiYe.Where(a => a.submitDepartment == "全厂" && a.submitTime >= stime && a.submitTime <= etime).ToList();
                if (A15Tabxiugai.Count() == 0)//超级用户没有修改数据，
                    return false;
                else
                {
                    return true;
                }
            }
        }
        public bool GetifDongQc(string zy, DateTime stime, DateTime etime)
        {
            using (var db = base.NewDB())
            {
                var A15Tabxiugai = db.A15dot1TabDong.Where(a => a.submitDepartment == "全厂" && a.submitTime >= stime && a.submitTime <= etime).ToList();
                if (A15Tabxiugai.Count() == 0)//超级用户没有修改数据，
                    return false;
                else
                {
                    return true;
                }
            }
        }
        public bool GetifJingQc(string zy, DateTime stime, DateTime etime)
        {
            using (var db = base.NewDB())
            {
                var A15Tabxiugai = db.A15dot1TabJing.Where(a => a.submitDepartment == "全厂" && a.submitTime >= stime && a.submitTime <= etime).ToList();
                if (A15Tabxiugai.Count() == 0)//超级用户没有修改数据，
                    return false;
                else
                {
                    return true;
                }
            }
        }
        public bool GetifDianQc(string zy, DateTime stime, DateTime etime)
        {
            using (var db = base.NewDB())
            {
                var A15Tabxiugai = db.A15dot1TabDian.Where(a => a.submitDepartment == "全厂" && a.submitTime >= stime && a.submitTime <= etime).ToList();
                if (A15Tabxiugai.Count() == 0)//超级用户没有修改数据，
                    return false;
                else
                {
                    return true;
                }
            }
        }
        public bool GetifYiQc(string zy, DateTime stime, DateTime etime)
        {
            using (var db = base.NewDB())
            {
                var A15Tabxiugai = db.A15dot1TabYi.Where(a => a.submitDepartment == "全厂" && a.submitTime >= stime && a.submitTime <= etime).ToList();
                if (A15Tabxiugai.Count() == 0)//超级用户没有修改数据，
                    return false;
                else
                {
                    return true;
                }
            }
        }

        public bool GetifQiYeCj(string cjname,string zy, DateTime stime, DateTime etime)
        {

            using (var db = base.NewDB())
            {
                var A15Tabxiugai = db.A15dot1TabQiYe.Where(a => a.submitDepartment == cjname && a.submitTime >= stime && a.submitTime <= etime).ToList();
                if (A15Tabxiugai.Count() == 0)//超级用户没有修改数据，
                    return false;
                else
                {
                    return true;
                }
            }
        }

        public bool GetifDongCj(string cjname, string zy, DateTime stime, DateTime etime)
        {
            using (var db = base.NewDB())
            {
                var A15Tabxiugai = db.A15dot1TabDong.Where(a => a.submitDepartment == cjname && a.submitTime >= stime && a.submitTime <= etime).ToList();
                if (A15Tabxiugai.Count() == 0)//超级用户没有修改数据，
                    return false;
                else
                {
                    return true;
                }
            }
        }
        public bool GetifJingCj(string cjname, string zy, DateTime stime, DateTime etime)
        {
            using (var db = base.NewDB())
            {
                var A15Tabxiugai = db.A15dot1TabJing.Where(a => a.submitDepartment == cjname && a.submitTime >= stime && a.submitTime <= etime).ToList();
                if (A15Tabxiugai.Count() == 0)//超级用户没有修改数据，
                    return false;
                else
                {
                    return true;
                }
            }
        }
        public bool GetifDianCj(string cjname, string zy, DateTime stime, DateTime etime)
        {
            using (var db = base.NewDB())
            {
                var A15Tabxiugai = db.A15dot1TabDian.Where(a => a.submitDepartment == cjname && a.submitTime >= stime && a.submitTime <= etime).ToList();
                if (A15Tabxiugai.Count() == 0)//超级用户没有修改数据，
                    return false;
                else
                {
                    return true;
                }
            }
        }
        public bool GetifYiCj(string cjname, string zy, DateTime stime, DateTime etime)
        {
            using (var db = base.NewDB())
            {
                var A15Tabxiugai = db.A15dot1TabYi.Where(a => a.submitDepartment == cjname && a.submitTime >= stime && a.submitTime <= etime).ToList();
                if (A15Tabxiugai.Count() == 0)//超级用户没有修改数据，
                    return false;
                else
                {
                    return true;
                }
            }
        }


        //获取全厂的动静电仪专业
        public List<object> GetQiYeQc(string zy, DateTime stime, DateTime etime)
        {
            string r = "";
            List<object> a = new List<object>();
            using (var db = base.NewDB())
            {
                r = "select sum(a.zzkkxzs*b.zzkkxzs_weight) zzkkxzs,sum(a.wxfyzs*b.wxfyzs_weight) wxfyzs,sum(a.qtlxbmfxhl*b.qtlxbmfxhl_weight) qtlxbmfxhl,sum(a.qtlhsbgsghl*b.qtlhsbgsghl_weight) qtlhsbgsghl,sum(a.ybsjkzl*b.ybsjkzl_weight) ybsjkzl,sum(a.sjs*b.sjs_weight) sjs,sum(a.gzqdkf*b.gzqdkf_weight) gzqdkf,sum(a.xmyql*b.xmyql_weight) xmyql,sum(a.pxjnl*b.pxjnl_weight) pxjnl from [EquipWeb].[dbo].[A15dot1TabQiYe] a,[EquipWeb].[dbo].[A15dot1TabQiYe_Weight] b  where  b.Zz_Name=a.submitDepartment and a.submitTime>'" + stime + "' and a.submitTime<'" + etime + "'";
                var i = db.Database.SqlQuery<qiyemodel>(r).First();

                a.Add(i.GetType().GetProperty("zzkkxzs").GetValue(i, null));
                a.Add(i.GetType().GetProperty("wxfyzs").GetValue(i, null));
                a.Add(i.GetType().GetProperty("qtlxbmfxhl").GetValue(i, null));
                a.Add(i.GetType().GetProperty("qtlhsbgsghl").GetValue(i, null));
                a.Add(i.GetType().GetProperty("ybsjkzl").GetValue(i, null));
                a.Add(i.GetType().GetProperty("sjs").GetValue(i, null));
                a.Add(i.GetType().GetProperty("gzqdkf").GetValue(i, null));
                a.Add(i.GetType().GetProperty("xmyql").GetValue(i, null));
                a.Add(i.GetType().GetProperty("pxjnl").GetValue(i, null));

                return a;
            }
        }
        public List<object> GetDongQc(string zy, DateTime stime, DateTime etime)
        {
            string r = "";
            List<object> a = new List<object>();
            using (var db = base.NewDB())
            {
                if (zy == "动设备专业")
                {
                    r = "select sum(a.gzqdkf*b.gzqdkf_weight) gzqdkf,sum(a.djzgzl*b.djzgzl_weight) djzgzl,sum(a.gzwxl*b.gzwxl_weight) gzwxl,sum(a.qtlxbmfxhl*b.qtlxbmfxhl_weight) qtlxbmfxhl,sum(a.jjqxgsl*b.jjqxgsl_weight) jjqxgsl,sum(a.gdpjwcsj*b.gdpjwcsj_weight) gdpjwcsj,sum(a.jxmfpjsm*b.jxmfpjsm_weight) jxmfpjsm,sum(a.zcpjsm*b.zcpjsm_weight) zcpjsm,sum(a.sbwhl*b.sbwhl_weight) sbwhl,sum(a.jxychgl*b.jxychgl_weight) jxychgl,sum(a.zyjbpjxl*b.zyjbpjxl_weight) zyjbpjxl,sum(a.jzpjxl*b.jzpjxl_weight) jzpjxl,sum(a.wfjzgzl*b.wfjzgzl_weight) wfjzgzl,sum(a.ndbtjbcfjxtc*b.ndbtjbcfjxtc_weight) ndbtjbcfjxtc,sum(a.jbpjwgzjgsjMTBF*b.jbpjwgzjgsjMTBF_weight) jbpjwgzjgsjMTBF from [EquipWeb].[dbo].[A15dot1TabDong] a,[EquipWeb].[dbo].[A15dot1TabDong_Weight] b  where  b.Zz_Name=a.submitDepartment and a.submitTime>'" + stime + "' and a.submitTime<'" + etime + "'";
                    var i = db.Database.SqlQuery<dongmodel>(r).First();

                    a.Add(i.GetType().GetProperty("gzqdkf").GetValue(i, null));
                    a.Add(i.GetType().GetProperty("djzgzl").GetValue(i, null));
                    a.Add(i.GetType().GetProperty("gzwxl").GetValue(i, null));
                    a.Add(i.GetType().GetProperty("qtlxbmfxhl").GetValue(i, null));
                    a.Add(i.GetType().GetProperty("jjqxgsl").GetValue(i, null));
                    a.Add(i.GetType().GetProperty("gdpjwcsj").GetValue(i, null));
                    a.Add(i.GetType().GetProperty("jxmfpjsm").GetValue(i, null));
                    a.Add(i.GetType().GetProperty("zcpjsm").GetValue(i, null));
                    a.Add(i.GetType().GetProperty("sbwhl").GetValue(i, null));
                    a.Add(i.GetType().GetProperty("jxychgl").GetValue(i, null));
                    a.Add(i.GetType().GetProperty("zyjbpjxl").GetValue(i, null));
                    a.Add(i.GetType().GetProperty("jzpjxl").GetValue(i, null));
                    a.Add(i.GetType().GetProperty("wfjzgzl").GetValue(i, null));
                    a.Add(i.GetType().GetProperty("ndbtjbcfjxtc").GetValue(i, null));
                    a.Add(i.GetType().GetProperty("jbpjwgzjgsjMTBF").GetValue(i, null));
                }
               
                return a;
            }
        }
        public List<object> GetJingQc(string zy, DateTime stime, DateTime etime)
        {
            string r = "";
            List<object> a = new List<object>();
            using (var db = base.NewDB())
            {
                if (zy == "静设备专业")
                {
                    r = "select sum(a.gzqdkf*b.gzqdkf_weight) gzqdkf,sum(a.sbfsxlcs*b.sbfsxlcs_weight) sbfsxlcs,sum(a.qtlhsbgs*b.qtlhsbgs_weight) qtlhsbgs,sum(a.gylpjrxl*b.gylpjrxl_weight)gylpjrxl,sum(a.hrqjxl*b.hrqjxl_weight) hrqjxl,sum(a.ylrqdjl*b.ylrqdjl_weight) ylrqdjl,sum(a.ylgdndjxjhwcl*b.ylgdndjxjhwcl_weight) ylgdndjxjhwcl,sum(a.aqfndjyjhwcl*b.aqfndjyjhwcl_weight) aqfndjyjhwcl,sum(a.sbfsjcjhwcl*b.sbfsjcjhwcl_weight) sbfsjcjhwcl,sum(a.jsbjwxychgl*b.jsbjwxychgl_weight) jsbjwxychgl from [EquipWeb].[dbo].[A15dot1TabJing] a,[EquipWeb].[dbo].[A15dot1TabJing_Weight] b  where  b.Zz_Name=a.submitDepartment and a.submitTime>'" + stime + "' and a.submitTime<'" + etime + "'";
                    var i = db.Database.SqlQuery<jingmodel>(r).First();

                    a.Add(i.GetType().GetProperty("gzqdkf").GetValue(i, null));
                    a.Add(i.GetType().GetProperty("sbfsxlcs").GetValue(i, null));
                    a.Add(i.GetType().GetProperty("qtlhsbgs").GetValue(i, null));
                    a.Add(i.GetType().GetProperty("gylpjrxl").GetValue(i, null));
                    a.Add(i.GetType().GetProperty("hrqjxl").GetValue(i, null));
                    a.Add(i.GetType().GetProperty("ylrqdjl").GetValue(i, null));
                    a.Add(i.GetType().GetProperty("ylgdndjxjhwcl").GetValue(i, null));
                    a.Add(i.GetType().GetProperty("aqfndjyjhwcl").GetValue(i, null));
                    a.Add(i.GetType().GetProperty("sbfsjcjhwcl").GetValue(i, null));
                    a.Add(i.GetType().GetProperty("jsbjwxychgl").GetValue(i, null));
                }

                return a;
            }
        }
        public List<object> GetDianQc(string zy, DateTime stime, DateTime etime)
        {
            string r = "";
            List<object> a = new List<object>();
            using (var db = base.NewDB())
            {

                r = "select sum(a.gzqdkf*b.gzqdkf_weight) gzqdkf,sum(a.dqwczcs*b.dqwczcs_weight) dqwczcs,sum(a.jdbhzqdzl*b.jdbhzqdzl_weight) jdbhzqdzl,sum(a.sbgzl*b.sbgzl_weight) sbgzl,sum(a.djMTBF*b.djMTBF_weight) djMTBF from [EquipWeb].[dbo].[A15dot1TabDian] a,[EquipWeb].[dbo].[A15dot1TabDian_Weight] b  where  b.Zz_Name=a.submitDepartment and a.submitTime>'" + stime + "' and a.submitTime<'" + etime + "'";
                var i = db.Database.SqlQuery<Dianmodel>(r).First();

                a.Add(i.GetType().GetProperty("gzqdkf").GetValue(i, null));
                a.Add(i.GetType().GetProperty("dqwczcs").GetValue(i, null));
                a.Add(i.GetType().GetProperty("jdbhzqdzl").GetValue(i, null));
                a.Add(i.GetType().GetProperty("sbgzl").GetValue(i, null));
                a.Add(i.GetType().GetProperty("djMTBF").GetValue(i, null));



                return a;
            }
        }     
        public List<object> GetYiQc(string zy, DateTime stime, DateTime etime)
        {
            string r = "";
            List<object> a = new List<object>();
            using (var db = base.NewDB())
            {

                r = "select sum(a.gzqdkf*b.gzqdkf_weight) gzqdkf,sum(a.lsxtzqdzl*b.lsxtzqdzl_weight) lsxtzqdzl,sum(a.kzxtgzcs*b.kzxtgzcs_weight) kzxtgzcs,sum(a.ybkzl*b.ybkzl_weight) ybkzl,sum(a.ybsjkzl*b.ybsjkzl_weight) ybsjkzl,sum(a.lsxttyl*b.lsxttyl_weight) lsxttyl,sum(a.gjkzfmgzcs*b.gjkzfmgzcs_weight) gjkzfmgzcs,sum(a.kzxtgzbjcs*b.kzxtgzbjcs_weight) kzxtgzbjcs,sum(a.cgybgzl*b.cgybgzl_weight) cgybgzl,sum(a.tjfMDBF*b.tjfMDBF_weight) tjfMDBF from [EquipWeb].[dbo].[A15dot1TabYi] a,[EquipWeb].[dbo].[A15dot1TabYi_Weight] b  where  b.Zz_Name=a.submitDepartment and a.submitTime>'" + stime + "' and a.submitTime<'" + etime + "'";
                var i = db.Database.SqlQuery<yimodel>(r).First();

                a.Add(i.GetType().GetProperty("gzqdkf").GetValue(i, null));
                a.Add(i.GetType().GetProperty("lsxtzqdzl").GetValue(i, null));
                a.Add(i.GetType().GetProperty("kzxtgzcs").GetValue(i, null));
                a.Add(i.GetType().GetProperty("ybkzl").GetValue(i, null));
                a.Add(i.GetType().GetProperty("ybsjkzl").GetValue(i, null));
                a.Add(i.GetType().GetProperty("lsxttyl").GetValue(i, null));
                a.Add(i.GetType().GetProperty("gjkzfmgzcs").GetValue(i, null));
                a.Add(i.GetType().GetProperty("kzxtgzbjcs").GetValue(i, null));
                a.Add(i.GetType().GetProperty("cgybgzl").GetValue(i, null));
                a.Add(i.GetType().GetProperty("tjfMDBF").GetValue(i, null));

                return a;
            }
        }

        public List<object> GetCjQiYe(string zy, string cjname, DateTime stime, DateTime etime)
        {
            string r = "";
            List<object> a = new List<object>();
            using (var db = base.NewDB())
            {
                r = "select sum(a.zzkkxzs*b.zzkkxzs_weight) zzkkxzs,sum(a.wxfyzs*b.wxfyzs_weight) wxfyzs,sum(a.qtlxbmfxhl*b.qtlxbmfxhl_weight) qtlxbmfxhl,sum(a.qtlhsbgsghl*b.qtlhsbgsghl_weight) qtlhsbgsghl,sum(a.ybsjkzl*b.ybsjkzl_weight) ybsjkzl,sum(a.sjs*b.sjs_weight) sjs,sum(a.gzqdkf*b.gzqdkf_weight) gzqdkf,sum(a.xmyql*b.xmyql_weight) xmyql,sum(a.pxjnl*b.pxjnl_weight) pxjnl from [EquipWeb].[dbo].[A15dot1TabQiYe] a,[EquipWeb].[dbo].[Pq_A15dot1TabQiYe_Weight] b  where  b.Zz_Name=a.submitDepartment and a.submitTime>'" + stime + "' and a.submitTime<'" + etime + "'and b.Pq_Name='" + cjname + "'";
                var i = db.Database.SqlQuery<qiyemodel>(r).First();

                a.Add(i.GetType().GetProperty("zzkkxzs").GetValue(i, null));
                a.Add(i.GetType().GetProperty("wxfyzs").GetValue(i, null));
                a.Add(i.GetType().GetProperty("qtlxbmfxhl").GetValue(i, null));
                a.Add(i.GetType().GetProperty("qtlhsbgsghl").GetValue(i, null));
                a.Add(i.GetType().GetProperty("ybsjkzl").GetValue(i, null));
                a.Add(i.GetType().GetProperty("sjs").GetValue(i, null));
                a.Add(i.GetType().GetProperty("gzqdkf").GetValue(i, null));
                a.Add(i.GetType().GetProperty("xmyql").GetValue(i, null));
                a.Add(i.GetType().GetProperty("pxjnl").GetValue(i, null));

                return a;
            }
        }

        public List<object> GetCjDong(string zy, string cjname, DateTime stime, DateTime etime)
        {
            string r = "";
            List<object> a = new List<object>();
            using (var db = base.NewDB())
            {

                r = "select sum(a.gzqdkf*b.gzqdkf_weight) gzqdkf,sum(a.djzgzl*b.djzgzl_weight) djzgzl,sum(a.gzwxl*b.gzwxl_weight) gzwxl,sum(a.qtlxbmfxhl*b.qtlxbmfxhl_weight) qtlxbmfxhl,sum(a.jjqxgsl*b.jjqxgsl_weight) jjqxgsl,sum(a.gdpjwcsj*b.gdpjwcsj_weight) gdpjwcsj,sum(a.jxmfpjsm*b.jxmfpjsm_weight) gzqdkf,sum(a.zcpjsm*b.zcpjsm_weight) zcpjsm,sum(a.zyjbpjxl*b.zyjbpjxl_weight) zyjbpjxl，sum(a.jzpjxl*b.jzpjxl_weight) jzpjxl，sum(a.wfjzgzl*b.wfjzgzl_weight) wfjzgzl，sum(a.zcpjsm*b.zcpjsm_weight) zcpjsm，sum(a.ndbtjbcfjxtc*b.ndbtjbcfjxtc_weight) ndbtjbcfjxtc，sum(a.ndbtjbcfjxtc*b.ndbtjbcfjxtc_weight) ndbtjbcfjxtc from [EquipWeb].[dbo].[A15dot1TabDong] a,[EquipWeb].[dbo].[Pq_A15dot1TabDong_Weight] b  where  b.Zz_Name=a.submitDepartment and a.submitTime>'" + stime + "' and a.submitTime<'" + etime + "'and b.Pq_Name='" + cjname + "'";
                var i = db.Database.SqlQuery<dongmodel>(r).First();

                a.Add(i.GetType().GetProperty("gzqdkf").GetValue(i, null));
                a.Add(i.GetType().GetProperty("djzgzl").GetValue(i, null));
                a.Add(i.GetType().GetProperty("gzwxl").GetValue(i, null));
                a.Add(i.GetType().GetProperty("qtlxbmfxhl").GetValue(i, null));
                a.Add(i.GetType().GetProperty("jjqxgsl").GetValue(i, null));
                a.Add(i.GetType().GetProperty("gdpjwcsj").GetValue(i, null));
                a.Add(i.GetType().GetProperty("jxmfpjsm").GetValue(i, null));
                a.Add(i.GetType().GetProperty("zcpjsm").GetValue(i, null));
                a.Add(i.GetType().GetProperty("jxychgl").GetValue(i, null));
                a.Add(i.GetType().GetProperty("zyjbpjxl").GetValue(i, null));
                a.Add(i.GetType().GetProperty("sbwhl").GetValue(i, null));
                a.Add(i.GetType().GetProperty("jzpjxl").GetValue(i, null));
                a.Add(i.GetType().GetProperty("wfjzgzl").GetValue(i, null));
                a.Add(i.GetType().GetProperty("ndbtjbcfjxtc").GetValue(i, null));
                a.Add(i.GetType().GetProperty("jbpjwgzjgsjMTBF").GetValue(i, null));




                return a;
            }
        }
        public List<object> GetCjJing(string zy, string cjname, DateTime stime, DateTime etime)
        {
            string r = "";
            List<object> a = new List<object>();
            using (var db = base.NewDB())
            {

                r = "select sum(a.gzqdkf*b.gzqdkf_weight) gzqdkf,sum(a.sbfsxlcs*b.sbfsxlcs_weight) sbfsxlcs,sum(a.qtlhsbgs*b.qtlhsbgs_weight) qtlhsbgs,sum(a.gylpjrxl*b.gylpjrxl_weight) gylpjrxl,sum(a.hrqjxl*b.hrqjxl_weight) hrqjxl,sum(a.ylrqdjl*b.ylrqdjl_weight) ylrqdjl,sum(a.ylgdndjxjhwcl*b.ylgdndjxjhwcl_weight) ylgdndjxjhwcl,sum(a.aqfndjyjhwcl*b.aqfndjyjhwcl_weight) aqfndjyjhwcl,sum(a.sbfsjcjhwcl*b.sbfsjcjhwcl_weight) sbfsjcjhwcl,sum(a.jsbjwxychgl*b.jsbjwxychgl_weight) jsbjwxychgl from [EquipWeb].[dbo].[A15dot1TabJing] a,[EquipWeb].[dbo].[Pq_A15dot1TabJing_Weight] b  where  b.Zz_Name=a.submitDepartment and a.submitTime>'" + stime + "' and a.submitTime<'" + etime + "'and b.Pq_Name='" + cjname + "'";
                var i = db.Database.SqlQuery<jingmodel>(r).First();

                a.Add(i.GetType().GetProperty("gzqdkf").GetValue(i, null));
                a.Add(i.GetType().GetProperty("sbfsxlcs").GetValue(i, null));
                a.Add(i.GetType().GetProperty("qtlhsbgs").GetValue(i, null));
                a.Add(i.GetType().GetProperty("gylpjrxl").GetValue(i, null));
                a.Add(i.GetType().GetProperty("hrqjxl").GetValue(i, null));
                a.Add(i.GetType().GetProperty("ylrqdjl").GetValue(i, null));
                a.Add(i.GetType().GetProperty("ylgdndjxjhwcl").GetValue(i, null));
                a.Add(i.GetType().GetProperty("aqfndjyjhwcl").GetValue(i, null));
                a.Add(i.GetType().GetProperty("sbfsjcjhwcl").GetValue(i, null));
                a.Add(i.GetType().GetProperty("jsbjwxychgl").GetValue(i, null));




                return a;
            }
        }
        public List<object> GetCjDian(string zy, string cjname, DateTime stime, DateTime etime)
        {
            string r = "";
            List<object> a = new List<object>();
            using (var db = base.NewDB())
            {

                r = "select sum(a.gzqdkf*b.gzqdkf_weight) gzqdkf,sum(a.dqwczcs*b.dqwczcs_weight) dqwczcs,sum(a.jdbhzqdzl*b.jdbhzqdzl_weight) jdbhzqdzl,sum(a.sbgzl*b.sbgzl_weight) sbgzl,sum(a.djMTBF*b.djMTBF_weight) djMTBF from [EquipWeb].[dbo].[A15dot1TabDian] a,[EquipWeb].[dbo].[Pq_A15dot1TabDian_Weight] b  where  b.Zz_Name=a.submitDepartment and a.submitTime>'" + stime + "' and a.submitTime<'" + etime + "'and b.Pq_Name='" + cjname + "'";
                var i = db.Database.SqlQuery<Dianmodel>(r).First();

                a.Add(i.GetType().GetProperty("gzqdkf").GetValue(i, null));
                a.Add(i.GetType().GetProperty("dqwczcs").GetValue(i, null));
                a.Add(i.GetType().GetProperty("jdbhzqdzl").GetValue(i, null));
                a.Add(i.GetType().GetProperty("sbgzl").GetValue(i, null));
                a.Add(i.GetType().GetProperty("djMTBF").GetValue(i, null));



                return a;
            }
        }
        public List<object> GetCjYi(string zy, string cjname, DateTime stime, DateTime etime)
        {
            string r = "";
            List<object> a = new List<object>();
            using (var db = base.NewDB())
            {

                r = "select sum(a.gzqdkf*b.gzqdkf_weight) gzqdkf,sum(a.lsxtzqdzl*b.lsxtzqdzl_weight) lsxtzqdzl,sum(a.kzxtgzcs*b.kzxtgzcs_weight) kzxtgzcs,sum(a.ybkzl*b.ybkzl_weight) ybkzl,sum(a.ybsjkzl*b.ybsjkzl_weight) ybsjkzl,sum(a.lsxttyl*b.lsxttyl_weight) lsxttyl,sum(a.gjkzfmgzcs*b.gjkzfmgzcs_weight) gjkzfmgzcs,sum(a.kzxtgzbjcs*b.kzxtgzbjcs_weight) kzxtgzbjcs,sum(a.cgybgzl*b.cgybgzl_weight) cgybgzl,sum(a.tjfMDBF*b.tjfMDBF_weight) tjfMDBF from [EquipWeb].[dbo].[A15dot1TabYi] a,[EquipWeb].[dbo].[Pq_A15dot1TabYi_Weight] b  where  b.Zz_Name=a.submitDepartment and a.submitTime>'" + stime + "' and a.submitTime<'" + etime + "'and b.Pq_Name='" + cjname + "'";
                var i = db.Database.SqlQuery<yimodel>(r).First();

                a.Add(i.GetType().GetProperty("gzqdkf").GetValue(i, null));
                a.Add(i.GetType().GetProperty("sbfsxlcs").GetValue(i, null));
                a.Add(i.GetType().GetProperty("qtlhsbgs").GetValue(i, null));
                a.Add(i.GetType().GetProperty("gylpjrxl").GetValue(i, null));
                a.Add(i.GetType().GetProperty("hrqjxl").GetValue(i, null));
                a.Add(i.GetType().GetProperty("ylrqdjl").GetValue(i, null));
                a.Add(i.GetType().GetProperty("ylgdndjxjhwcl").GetValue(i, null));
                a.Add(i.GetType().GetProperty("aqfndjyjhwcl").GetValue(i, null));
                a.Add(i.GetType().GetProperty("sbfsjcjhwcl").GetValue(i, null));
                a.Add(i.GetType().GetProperty("jsbjwxychgl").GetValue(i, null));




                return a;
            }
        }


        public bool AddDongJxRecord(A15dot1TabDong nVal)
        {
            using (var db = base.NewDB())
            {
                try
                {

                    db.A15dot1TabDong.Add(nVal);
                    db.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }
        //public List<object> Dongqstdata(string grahpic_name, string pianqu)
        //{
        //    using (var db = base.NewDB())
        //    {
        //        var i = db.Database.SqlQuery<A15dot1TabDong>("select * from A15dot1TabDong where submitDepartment='" + pianqu + "'").ToList();

        //        List<object> a = new List<object>();
        //        foreach (var item in i)
        //        {
        //            var t = item.GetType().GetProperty(grahpic_name);
        //            var result = t.GetValue(item, null);
        //            a.Add(result);
        //        }
        //        object cut = new object();
        //        cut = "$$";
        //        a.Add(cut);
        //        foreach (var ex in i)
        //        {
        //            var t = ex.GetType().GetProperty("jdcOperateTime");
        //            var result = t.GetValue(ex, null);
        //            a.Add(result);
        //        }


        //        return a;
        //    }
        //}


        public List<object> qstdataQiYe(string grahpic_name, string zzname)
        {
            using (var db = base.NewDB())
            {
                var i = db.Database.SqlQuery<A15dot1TabQiYe>("select * from A15dot1TabQiYe where submitDepartment='" + zzname + "'").ToList();

                List<object> a = new List<object>();
                foreach (var item in i)
                {
                    var t = item.GetType().GetProperty(grahpic_name);
                    var result = t.GetValue(item, null);
                    a.Add(result);
                }
                object cut = new object();
                cut = "$$";
                a.Add(cut);
                foreach (var ex in i)
                {
                    var t = ex.GetType().GetProperty("submitTime");
                    var result = t.GetValue(ex, null);
                    a.Add(result);
                }


                return a;
            }
        }

        public List<object> qstdataDong(string grahpic_name, string zzname)

        {
            using (var db = base.NewDB())
            {
                var i = db.Database.SqlQuery<A15dot1TabDong>("select * from A15dot1TabDong where submitDepartment='" + zzname + "'").ToList();

                List<object> a = new List<object>();
                foreach (var item in i)
                {
                    var t = item.GetType().GetProperty(grahpic_name);
                    var result = t.GetValue(item, null);
                    a.Add(result);
                }
                object cut = new object();
                cut = "$$";
                a.Add(cut);
                foreach (var ex in i)
                {
                    var t = ex.GetType().GetProperty("submitTime");
                    var result = t.GetValue(ex, null);
                    a.Add(result);
                }


                return a;
            }
        }

        public List<object> qstdataYi(string grahpic_name, string zzname)
        {
            using (var db = base.NewDB())
            {
                var i = db.Database.SqlQuery<A15dot1TabYi>("select * from A15dot1TabYi where submitDepartment='" + zzname + "'").ToList();

                List<object> a = new List<object>();
                foreach (var item in i)
                {
                    var t = item.GetType().GetProperty(grahpic_name);
                    var result = t.GetValue(item, null);
                    a.Add(result);
                }
                object cut = new object();
                cut = "$$";
                a.Add(cut);
                foreach (var ex in i)
                {
                    var t = ex.GetType().GetProperty("submitTime");
                    var result = t.GetValue(ex, null);
                    a.Add(result);
                }


                return a;
            }
        }//取仪表专业表的数据生成趋势图

        public List<object> qstdataJing(string grahpic_name, string zzname)
        {
            using (var db = base.NewDB())
            {
                var i = db.Database.SqlQuery<A15dot1TabJing>("select * from A15dot1TabJing where submitDepartment='" + zzname + "'").ToList();

                List<object> a = new List<object>();
                foreach (var item in i)
                {
                    var t = item.GetType().GetProperty(grahpic_name);
                    var result = t.GetValue(item, null);
                    a.Add(result);
                }
                object cut = new object();
                cut = "$$";
                a.Add(cut);
                foreach (var ex in i)
                {
                    var t = ex.GetType().GetProperty("submitTime");
                    var result = t.GetValue(ex, null);
                    a.Add(result);
                }


                return a;
            }
        }

        public List<object> qstdataDian(string grahpic_name, string zzname)
        {
            using (var db = base.NewDB())
            {
                var i = db.Database.SqlQuery<A15dot1TabDian>("select * from A15dot1TabDian where submitDepartment='" + zzname + "'").ToList();

                List<object> a = new List<object>();
                foreach (var item in i)
                {
                    var t = item.GetType().GetProperty(grahpic_name);
                    var result = t.GetValue(item, null);
                    a.Add(result);
                }
                object cut = new object();
                cut = "$$";
                a.Add(cut);
                foreach (var ex in i)
                {
                    var t = ex.GetType().GetProperty("submitTime");
                    var result = t.GetValue(ex, null);
                    a.Add(result);
                }


                return a;
            }
        }




        public bool GetifzztibaoQiYe( DateTime stime, DateTime etime, string Zz_Name)
        {
            using (var db = base.NewDB())
            {


                var A15Tabtibao = db.A15dot1TabQiYe.Where(a => a.submitDepartment == Zz_Name && a.submitTime >= stime && a.submitTime <= etime).ToList();
                if (A15Tabtibao.Count() == 0)//没有提报数据，
                    return false;
                else
                {
                    return true;
                }
            }
        }

        public bool GetifzztibaoDong(DateTime stime, DateTime etime, string Zz_Name)
        {
            using (var db = base.NewDB())
            {


                var A15Tabtibao = db.A15dot1TabDong.Where(a => a.submitDepartment == Zz_Name && a.submitTime >= stime && a.submitTime <= etime).ToList();
                if (A15Tabtibao.Count() == 0)//没有提报数据，
                    return false;
                else
                {
                    return true;
                }
            }
        }

        public bool GetifzztibaoJing(DateTime stime, DateTime etime, string Zz_Name)
        {
            using (var db = base.NewDB())
            {


                var A15Tabtibao = db.A15dot1TabJing.Where(a => a.submitDepartment == Zz_Name && a.submitTime >= stime && a.submitTime <= etime).ToList();
                if (A15Tabtibao.Count() == 0)//没有提报数据，
                    return false;
                else
                {
                    return true;
                }
            }
        }

        public bool GetifzztibaoDian(DateTime stime, DateTime etime, string Zz_Name)
        {
            using (var db = base.NewDB())
            {


                var A15Tabtibao = db.A15dot1TabDian.Where(a => a.submitDepartment == Zz_Name && a.submitTime >= stime && a.submitTime <= etime).ToList();
                if (A15Tabtibao.Count() == 0)//没有提报数据，
                    return false;
                else
                {
                    return true;
                }
            }
        }

        public bool GetifzztibaoYi(DateTime stime, DateTime etime, string Zz_Name)
        {
            using (var db = base.NewDB())
            {


                var A15Tabtibao = db.A15dot1TabYi.Where(a => a.submitDepartment == Zz_Name && a.submitTime >= stime && a.submitTime <= etime).ToList();
                if (A15Tabtibao.Count() == 0)//没有提报数据，
                    return false;
                else
                {
                    return true;
                }
            }
        }


    }
}

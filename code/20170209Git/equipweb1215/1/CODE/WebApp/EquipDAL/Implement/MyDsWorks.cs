using EquipDAL.Interface;
using EquipModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EquipDAL.Implement
{
    public class MyDsWorks:BaseDAO
    {

        /// <summary>
        /// 检查是否本月已经存过定时信息
        /// </summary>
        /// <param name="work_name"></param>
        /// <param name="event_name"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        //public bool IsAlreadySave(string work_name,string event_name,string year,string month)
        //{
        //    using (var db = base.NewDB())
        //    {
        //        var R = db.DSEventDetail.Where(a => a.work_name == work_name & a.event_name == event_name&a.year==year&a.month==month);
        //        if (R.Count() != 0)
        //            return true;
        //        else
        //            return false;
        //    }
        //}

        public string getDstime(string work_name, string event_name)
        {
            using (var db = base.NewDB())
            {
                var R = db.DsTimeOfWork.Where(a => a.work_name == work_name & a.event_name == event_name);
                if (R.Count() != 0)
                    return R.First().time;
                else
                    return "";
            }
        }
        public int AddDsEvent(DSEventDetail ds)
        {
            try
            {
                using (var db = base.NewDB())
                {
                    DSEventDetail newds = db.DSEventDetail.Add(ds);
                    db.SaveChanges();
                    return 1;
                }
            }
            catch(Exception e)
            {
                return 0;
            }
        }

        public DSEventDetail getdetailbyE_id(int entity_id)
        {
            using (var db = base.NewDB())
            {
                try
                {
                    var E = db.DSEventDetail.Where(a => a.entity_id == entity_id).First();
                    return E;
                }
                catch
                { return null; }
            }
        }
        public bool modifystate(int entity_id,string event_name)
        {
            using (var db = base.NewDB())
            {
                try
                {
                    var E = db.DSEventDetail.Where(a => a.entity_id == entity_id&&a.event_name==event_name).First();
                    if (E != null)
                    {
                        E.state = 1;
                        E.done_time = DateTime.Now;
                        db.SaveChanges();
                        return true;
                    }
                    else
                        return true;
                }
                catch
                { return false; }
            }
        }
    }
}

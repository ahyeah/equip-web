using EquipModel.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipDAL.Implement
{
    public class GetNWorkSer:BaseDAO
    {
        private static object insert_lock = new object();
        public NWorkFlowSer AddNWorkEntity(string wfName, string WE_Ser = "")
        {
            NWorkFlowSer NW = new NWorkFlowSer();
            NWorkFlowSer n = new NWorkFlowSer();
            lock (insert_lock)
            { //locked
                using (var db = base.NewDB())
                {

                    if (WE_Ser == "") //保证子工作流串号与父工作流相同
                    {
                        //对WorkFlow_Entity编号的处理
                        string perFix = DateTime.Now.ToString("yyyyMM");
                        IQueryable<NWorkFlowSer> we_Ser = db.NWorkFlowSer.Where(s => s.WE_Ser.StartsWith("N"+perFix)).OrderBy(s => s.WE_Ser);
                        if (we_Ser.ToList().Count == 0)
                            WE_Ser = "N" + perFix + "00001";
                        else
                        {
                            string last_ser = we_Ser.ToList().Last().WE_Ser;
                            string[] b = last_ser.Split(new Char[] { 'N' });
                            string num_ser = b[1];
                            WE_Ser = "N"+(Convert.ToInt64(b[1]) + 1).ToString();
                        }
                        NW.time = DateTime.Now;
                        NW.WE_Ser = WE_Ser;
                        NW.WF_Name = wfName;
                        n = db.NWorkFlowSer.Add(NW);
                        if (db.SaveChanges() != 1)
                            return null;
                    }
                    else
                    {
                        NW.time = DateTime.Now;
                        NW.WE_Ser = WE_Ser;
                        NW.WF_Name = wfName;
                        n = db.NWorkFlowSer.Add(NW);
                        if (db.SaveChanges() != 1)
                            return null;
                    }


                }
            }//unlocked
            return n;
        }
    }
}

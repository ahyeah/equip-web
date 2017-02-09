using EquipDAL.Implement;
using EquipDAL.Interface;
using EquipModel.Context;
using EquipModel.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipBLL.AdminManagment
{
    public class SxglManagment
    {
        private Sxgl db_Sxgl = new Sxgl();


        public bool AddSxItem(A5dot2Tab1 add)
        {
          return db_Sxgl.AddSxRecord(add);
        }
        public List<A5dot2Tab1> GetSxItem(List<string> cjname)
        {
            return db_Sxgl.GetSxRecord(cjname);
        }
        public List<A5dot2Tab1> GetSxItem_detail(int id)
        {
            return db_Sxgl.GetSxRecord_detail(id);
        }
        public string ModifySxItem(A5dot2Tab1 add)
        {
            return db_Sxgl.ModifySxRecord(add);
        }
        public A5dot2Tab1 getAllbyid(int a_id)
        {
            return db_Sxgl.getAllbyid(a_id);
        }
        public string ModifySxItem1(A5dot2Tab1 add)
            {
            return db_Sxgl.ModifySxRecord1(add);
            }
        public bool AddA5dot2Tab2Item(A5dot2Tab2 add)
        {
            return db_Sxgl.AddA5dot2Tab2(add);
        }
        public List<A5dot2Tab2> GetA5dot2Tab2Item(string add)
        {
            return db_Sxgl.GetA5dot2Tab2(add);
        }
        public bool ModifyA5dot2Tab2Item(A5dot2Tab2 add)
        {
            return db_Sxgl.ModifyA5dot2Tab2(add);
        }
        public List<A5dot2Tab2> GetA5dot2Tab2ItemForPq(string yearandmonth)
        {
            return db_Sxgl.GetA5dot2Tab2ForPq(yearandmonth);
        }
        public int GetnProblemsInYearItem(string sbCode)
        {
            return db_Sxgl.GetnProblemsInYear(sbCode);
        }
        public bool IsSetWorse(string id,string user)
        {
            int IntId = Convert.ToInt32(id);
            return db_Sxgl.SetWorse(IntId,user);
        }
        public List<A5dot2Tab2> jdcsum(string yearandmonth)
        {
            return db_Sxgl.jdcsummy(yearandmonth);
        }
        public List<A5dot2Tab1> uncom(string sbCode,DateTime time)
        {
            return db_Sxgl.uncomp(sbCode,time);
        }
        public string lastmonthproblem(string lastmonth, string sbCode)
        {
            return db_Sxgl.forlastmonthproblem(lastmonth, sbCode);
        }

        public List<A5dot2Tab1> GetLS_listbywfe_id(List<int> wfe_id)
            {
            return db_Sxgl.GetLS_listbywfe_id(wfe_id);
            }

    }
}

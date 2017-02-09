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
    public class JxpgManagment
    {
        private Jxpg db_Jxpg = new Jxpg();

        
        public bool AddJxItem( A15dot1Tab add)
        {



            return db_Jxpg.AddJxRecord(add);
        }


        public bool ModifyJxItem(A15dot1Tab add)
        {
            return db_Jxpg.ModifyJxRecord(add);
        }

        //时机动处修改
        public bool JdcModifyJxItem(A15dot1Tab add)
        {
            return db_Jxpg.JdcModifyJxRecord(add);
        }

        public List<A15dot1Tab> GetJxItem(string roles, string dep, string name)
        {
            return db_Jxpg.GetJxRecord(roles,dep,name);
        }
        public List<A15dot1Tab> GetJxItem_detail(int id)
        {
            return db_Jxpg.GetJxRecord_detail(id);
        }

        public List<A15dot1Tab> GetHisJxItem(string roles, string dep, string name)
        {
            return db_Jxpg.GetHisJxRecord(roles, dep, name);
        }
        public List<A15dot1Tab> GetHisJxItem_detail(int id)
        {
            return db_Jxpg.GetHisJxRecord_detail(id);
        }
        public List<object> qst(string grahpic_name, string pianqu)
        {
            return db_Jxpg.qstdata(grahpic_name, pianqu);
        }

        public List<A15dot1Tab> GetJxItemforA2(DateTime time, DateTime time2)
        {
            return db_Jxpg.GetJxItemforA2Tab(time,time2);
        }
        public List<A15dot1Tab> newGetJxItemforA2(DateTime time, DateTime time2, string pianqu)
        {
            return db_Jxpg.newGetJxItemforA2Tab(time, time2, pianqu);
        }
    }
}

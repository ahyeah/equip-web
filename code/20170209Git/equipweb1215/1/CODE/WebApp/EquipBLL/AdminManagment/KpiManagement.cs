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
   public  class KpiManagement
    {
       private Kpi db_Kpi = new Kpi();
       public bool AddJxItem(A15dot1TabQiYe add)
       {
           return db_Kpi.AddJxRecord(add);
       }
       public bool AddDianItem(A15dot1TabDian add)
       {
           return db_Kpi.AddDianRecord(add);
       }
       public bool AddJingItem(A15dot1TabJing add)
       {
           return db_Kpi.AddJingRecord(add);
       }
       public bool AddYiItem(A15dot1TabYi add)
       {
           return db_Kpi.AddYiRecord(add);
       }
       public List<object> qst(string grahpic_name, string zz)
       {
           return db_Kpi.qstdata(grahpic_name, zz);
       }
       //public List<object> qstQiYe(string grahpic_name, string zzId,string zy)
       //{
       //    return db_Kpi.qstdataQiYe(grahpic_name, zzId,zy);
       //}
       public List<object> qstqc(string grahpic_name, string graphic_zdm, string graphic_zy, string graphic_subdipartment)//全厂的趋势图
       {
           return db_Kpi.qstdataqc(grahpic_name, graphic_zdm, graphic_zy, graphic_subdipartment);
       }
       public bool ModifyQcQyJxItem(A15dot1TabQiYe add,DateTime stime,DateTime etime)
       {
           return db_Kpi.ModifyQcQyJxItem(add, stime, etime);
       }
       public bool ModifyJxItem(A15dot1TabQiYe add)
       {
           return db_Kpi.ModifyJxRecord(add);
       }
       public bool ModifyQcDongJxItem(A15dot1TabDong add, DateTime stime, DateTime etime)
       {
           return db_Kpi.ModifyQcDongJxItem(add, stime, etime);
       }


       public bool ModifyQcJingJxItem(A15dot1TabJing add, DateTime stime, DateTime etime)
       {
           return db_Kpi.ModifyQcJingJxItem(add, stime, etime);
       }

       public bool ModifyQcDianJxItem(A15dot1TabDian add, DateTime stime, DateTime etime)
       {
           return db_Kpi.ModifyQcDianJxItem(add, stime, etime);
       }

       public bool ModifyQcYiJxItem(A15dot1TabYi add, DateTime stime, DateTime etime)
       {
           return db_Kpi.ModifyQcYiJxItem(add, stime, etime);
       }
       public bool ModifyJxItemDong(A15dot1TabDong add)
       {
           return db_Kpi.ModifyJxRecordDong(add);
       }
       public bool ModifyJxItemDian(A15dot1TabDian add)
       {
           return db_Kpi.ModifyJxRecordDian(add);
       }

       public bool ModifyJxItemJing(A15dot1TabJing add)
       {
           return db_Kpi.ModifyJxRecordJing(add);
       }
       public bool ModifyJxItemYi(A15dot1TabYi add)
       {
           return db_Kpi.ModifyJxRecordYi(add);
       }
       public List<A15dot1TabQiYe> GetJxItem(string roles, string dep, string name,List<string > cjname)//cjname存于temp3，专业存于temp2
       {
           return db_Kpi.GetJxRecord(roles, dep, name, cjname);
       }
     
       public List<A15dot1TabQiYe> GetJxItem_detail(int id)
       {
           return db_Kpi.GetJxRecord_detail(id);
       }
       public List<A15dot1TabDian> GetJxItem_detailDian(int id)
       {
           return db_Kpi.GetJxRecord_detailDian(id);
       }
       public List<object> GetQcDian(string zy, DateTime stime, DateTime etime)
       {
           return db_Kpi.GetQcDian(zy, stime, etime);
       }
       public List<object> GetCjDian(string zy, string cjname, DateTime stime, DateTime etime) 
       {
           return db_Kpi.GetCjDian(zy,cjname, stime, etime);
       }
       public List<object> GetCjQiYe(string zy, string cjname, DateTime stime, DateTime etime)
       {
           return db_Kpi.GetCjQiYe(zy, cjname, stime, etime);
       }
       public List<A15dot1TabQiYe> GetHisJxItem(string roles, string dep, string name)
       {
           return db_Kpi.GetHisJxRecord(roles, dep, name);
       }
       public List<A15dot1TabQiYe> GetHisJxItem_detail(int id)
       {
           return db_Kpi.GetHisJxRecord_detail(id);
       }

       public bool GetifQiYeQc(string zy, DateTime stime, DateTime etime)
       {
           return db_Kpi.GetifQiYeQc(zy, stime, etime);
       }
       public List<A15dot1TabQiYe> GetJxByA2(DateTime astime,DateTime aetime, string zzname, string zy)
       {
           return db_Kpi.GetJxByA2(astime,aetime, zzname, zy);
       }
       public List<A15dot1TabJing> GetJingJxToA2(string time, string zzname, string zy)
       {
           return db_Kpi.GetJxRecordJing(time, zzname, zy);
       }
       public List<A15dot1TabDian> GetDianJxToA2(string time, string zzname, string zy)
       {
           return db_Kpi.GetJx1Record(time, zzname, zy);
       }
       public List<A15dot1TabYi> GetYiJxToA2(string time, string zzname, string zy)
       {
           return db_Kpi.GetJx2Record(time, zzname, zy);
       }


       //动的 全厂提取
       public List<object> GetQc(string zy, DateTime stime, DateTime etime)
       {
           return db_Kpi.GetQc(zy, stime, etime);
       }

       //企业的 全厂提取
       public List<object> GetQiYeQc(string zy, DateTime stime, DateTime etime)
       {
           return db_Kpi.GetQiYeQc(zy, stime, etime);
       }

      public List<object> GetQcJing(string zy, DateTime stime, DateTime etime)
       {
           return db_Kpi.GetQcJing(zy, stime, etime);
       }

       public List<object> GetQcYi(string zy, DateTime stime, DateTime etime)
            {
                return db_Kpi.GetQcYi(zy, stime, etime);
            }

    public List<A15dot1TabDong> GetDongJxByA2(string time, string zzname, string zy)
    {
        return db_Kpi.GetDongJxByA2(time, zzname, zy);
    }

    public bool AddDongJxItem(A15dot1TabDong add)
    {



        return db_Kpi.AddDongJxRecord(add);
    }
    public List<object> Dongqst(string grahpic_name, string pianqu)
    {
        return db_Kpi.Dongqstdata(grahpic_name, pianqu);
    }


    }
}

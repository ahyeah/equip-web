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
       public List<A15dot1TabQiYe> GetHisJxItem(string roles, string dep, string name)
       {
           return db_Kpi.GetHisJxRecord(roles, dep, name);
       }
       public List<A15dot1TabQiYe> GetHisJxItem_detail(int id)
       {
           return db_Kpi.GetHisJxRecord_detail(id);
       }

       //判断管理员是否已经保存了修改的数据
       public bool GetifQiYeQc(string zy, DateTime stime, DateTime etime)
       {
           return db_Kpi.GetifQiYeQc(zy, stime, etime);
       }
       public bool GetifDongQc(string zy, DateTime stime, DateTime etime)
       {
           return db_Kpi.GetifDongQc(zy, stime, etime);
       }
       public bool GetifJingQc(string zy, DateTime stime, DateTime etime)
       {
           return db_Kpi.GetifJingQc(zy, stime, etime);
       }
       public bool GetifDianQc(string zy, DateTime stime, DateTime etime)
       {
           return db_Kpi.GetifDianQc(zy, stime, etime);
       }
       public bool GetifYiQc(string zy, DateTime stime, DateTime etime)
       {
           return db_Kpi.GetifYiQc(zy, stime, etime);
       }
       //按车间查询判断库里是否存在数据（即片区长是否提交保存数据）
       public bool GetifQiYeCj(string cjname,string zy, DateTime stime, DateTime etime)
       {
           return db_Kpi.GetifQiYeCj(cjname, zy, stime, etime);
       }

       public bool GetifDongCj(string cjname,string zy, DateTime stime, DateTime etime)
       {
           return db_Kpi.GetifDongCj(cjname,zy, stime, etime);
       }
       public bool GetifJingCj(string cjname,string zy, DateTime stime, DateTime etime)
       {
           return db_Kpi.GetifJingCj(cjname,zy, stime, etime);
       }
       public bool GetifDianCj(string cjname,string zy, DateTime stime, DateTime etime)
       {
           return db_Kpi.GetifDianCj(cjname,zy, stime, etime);
       }
       public bool GetifYiCj(string cjname,string zy, DateTime stime, DateTime etime)
       {
           return db_Kpi.GetifYiCj(cjname,zy, stime, etime);
       }


       public List<A15dot1TabQiYe> GetQiYeJxByA2(DateTime astime,DateTime aetime, string zzname, string zy)
       {
           return db_Kpi.GetQiYeJxByA2(astime,aetime, zzname, zy);
       }

       public List<A15dot1TabDong> GetDongJxByA2(DateTime astime, DateTime aetime, string zzname, string zy)
       {
           return db_Kpi.GetDongJxByA2(astime, aetime, zzname, zy);
       }

       public List<A15dot1TabJing> GetJingJxByA2(DateTime astime, DateTime aetime, string zzname, string zy)
       {
           return db_Kpi.GetJingJxByA2(astime, aetime, zzname, zy);
       }
       public List<A15dot1TabDian> GetDianJxByA2(DateTime astime, DateTime aetime, string zzname, string zy)
       {
           return db_Kpi.GetDianJxByA2(astime, aetime, zzname, zy);
       }
       public List<A15dot1TabYi> GetYiJxByA2(DateTime astime, DateTime aetime, string zzname, string zy)
       {
           return db_Kpi.GetYiJxByA2(astime, aetime, zzname, zy);
       }


       //动的 全厂提取
       public List<object> GetQiYeQc(string zy, DateTime stime, DateTime etime)
       {
           return db_Kpi.GetQiYeQc(zy, stime, etime);
       }

       public List<object> GetDongQc(string zy, DateTime stime, DateTime etime)
       {
           return db_Kpi.GetDongQc(zy, stime, etime);
       }
       public List<object> GetJingQc(string zy, DateTime stime, DateTime etime)
       {
           return db_Kpi.GetJingQc(zy, stime, etime);
       }

       public List<object> GetDianQc(string zy, DateTime stime, DateTime etime)
       {
           return db_Kpi.GetDianQc(zy, stime, etime);
       }
       public List<object> GetYiQc(string zy, DateTime stime, DateTime etime)
       {
           return db_Kpi.GetYiQc(zy, stime, etime);
       }


       public List<object> GetCjQiYe(string zy, string cjname, DateTime astime, DateTime aetime)
       {
           return db_Kpi.GetCjQiYe(zy, cjname, astime, aetime);
       }

       public List<object> GetCjDong(string zy, string cjname, DateTime astime, DateTime aetime)
       {
           return db_Kpi.GetCjDong(zy, cjname, astime, aetime);
       }
       public List<object> GetCjJing(string zy, string cjname, DateTime astime, DateTime aetime)
       {
           return db_Kpi.GetCjJing(zy, cjname, astime, aetime);
       }
       public List<object> GetCjDian(string zy, string cjname, DateTime astime, DateTime aetime)
       {
           return db_Kpi.GetCjDian(zy, cjname, astime, aetime);
       }
       public List<object> GetCjYi(string zy, string cjname, DateTime astime, DateTime aetime)
       {
           return db_Kpi.GetCjYi(zy, cjname, astime, aetime);
       }

    public bool AddDongJxItem(A15dot1TabDong add)
    {



        return db_Kpi.AddDongJxRecord(add);
    }
    //public List<object> Dongqst(string grahpic_name, string pianqu)
    //{
    //    return db_Kpi.Dongqstdata(grahpic_name, pianqu);
    //}
    public List<object> qstQiYe(string grahpic_name, string zzname)
    {
        return db_Kpi.qstdataQiYe(grahpic_name, zzname);
    }
    public List<object> qstDong(string grahpic_name, string zzname)
    {
        return db_Kpi.qstdataDong(grahpic_name, zzname);
    }
    public List<object> qstJing(string grahpic_name, string zzname)
    {
        return db_Kpi.qstdataJing(grahpic_name, zzname);
    }
    public List<object> qstDian(string grahpic_name, string zzname)
    {
        return db_Kpi.qstdataDian(grahpic_name, zzname);
    }
    public List<object> qstYi(string grahpic_name, string zzname)
    {
        return db_Kpi.qstdataYi(grahpic_name, zzname);
    }



    public bool GetifzztibaoQiYe(DateTime stime, DateTime etime, string Zz_Name)
    {
        return db_Kpi.GetifzztibaoQiYe(stime, etime, Zz_Name);
    }

    public bool GetifzztibaoDong(DateTime stime, DateTime etime, string Zz_Name)
    {
        return db_Kpi.GetifzztibaoDong(stime, etime, Zz_Name);
    }

    public bool GetifzztibaoJing(DateTime stime, DateTime etime, string Zz_Name)
    {
        return db_Kpi.GetifzztibaoJing(stime, etime, Zz_Name);
    }

    public bool GetifzztibaoDian(DateTime stime, DateTime etime, string Zz_Name)
    {
        return db_Kpi.GetifzztibaoDian(stime, etime, Zz_Name);
    }

    public bool GetifzztibaoYi(DateTime stime, DateTime etime, string Zz_Name)
    {
        return db_Kpi.GetifzztibaoYi(stime, etime, Zz_Name);
    }

    }
}

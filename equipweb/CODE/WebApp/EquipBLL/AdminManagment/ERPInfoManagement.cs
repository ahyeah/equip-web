using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EquipModel.Entities;
using EquipDAL.Implement;
namespace EquipBLL.AdminManagment
{
    public class GD_InfoModal
    {
        public string GD_Id;
        public string GD_Notice_Id;
        public string GD_UserState;
        public string GD_Desc;
        public string GD_State;
        public string GD_EquipCode;
    }
    public  class ERPInfoManagement
    { //适用于流程A8.2 
        //输入：工单号
        //输出：GD_InfoModal{记录工单的通知单号、用户状态和简要说明信息}
        public GD_InfoModal getGD_Modal_GDId(string gd_Id)
        {
            ERP_Infos gd = new ERP_Infos();
            GongDan r_gd= gd.getGD_info(gd_Id);
            if (r_gd != null)
            {
                GD_InfoModal r = new GD_InfoModal();
                r.GD_Id = r_gd.GD_Id;
                r.GD_Notice_Id = r_gd.GD_Notice_Id;
                r.GD_UserState = r_gd.GD_State;
                r.GD_Desc = r_gd.GD_Desc;
                r.GD_State = r_gd.GD_State;
                r.GD_EquipCode = r_gd.GD_EquipCode;
                return r;
            }
            else
                return null;
        }

        //适用于流程A14.3 
        //输入：通知单单号
        //输出：GD_InfoModal{记录工单的工单号、用户状态和简要说明信息}
        public GD_InfoModal getGD_Modal_Notice(string gd_Notice_Id)
        {
            ERP_Infos gd = new ERP_Infos();
            GongDan r_gd = gd.getGD_Notice_info(gd_Notice_Id);
            if (r_gd != null)
            {
                GD_InfoModal r = new GD_InfoModal();
                r.GD_Id = r_gd.GD_Id;
                r.GD_Notice_Id = r_gd.GD_Notice_Id;
                r.GD_UserState = r_gd.GD_State;
                r.GD_Desc = r_gd.GD_Desc;
                return r;
            }
            else
                return null;
        }

        public List<OilInfo> getOilInfo_overdue()
        {
            string curdate = DateTime.Now.ToString("yyyyMMdd");
            ERP_Infos gd = new ERP_Infos();
            return gd.getOilInfo_Overdue(curdate);
        }
        //public List<OilInfo> getOilInfo_overdue()
        //{
        //    string curdate=DateTime.Now.ToString("YYYYMMdd");
        //    ERP_Infos gd = new ERP_Infos();
        //    return gd.getOilInfo_Overdue(curdate);
        //}

        //影响级别为1级，非计划停工通知单数
        public int getNoticesYx_1(string Pq_Code)
        {
            ERP_Infos erp = new ERP_Infos();
            List<Notice> notices = erp.getNoticeInfos_ByNoticeType(Pq_Code,"M2", "删除");
            return notices.Where(a => a.Notice_FaultYx == "1").Count();
        }
        //影响级别为2级 单个装置非计划停工、多套装置生产波动的通知单数目 
        public int getNoticeYx_2(string Pq_Code)
        {
            ERP_Infos erp = new ERP_Infos();
            List<Notice> notices = erp.getNoticeInfos_ByNoticeType(Pq_Code,"M2", "删除");
            return notices.Where(a => a.Notice_FaultYx == "2").Count();
        }
        //影响级别3级 单个生产装置降量或局部切除，大机组急停   
        public int getNoticeYx_3(string Pq_Code)
         {
             ERP_Infos erp = new ERP_Infos();
             List<Notice> notices = erp.getNoticeInfos_ByNoticeType(Pq_Code,"M2", "删除");
             return notices.Where(a => a.Notice_FaultYx == "3").Count();
         }
        //影响级别4级 单个生产装置局部波动 
        public int getNoticeYx_4(string Pq_Code)
         {
             ERP_Infos erp = new ERP_Infos();
             List<Notice> notices = erp.getNoticeInfos_ByNoticeType(Pq_Code,"M2", "删除");
             return notices.Where(a => a.Notice_FaultYx == "4").Count();
         }
        //故障维修率F
        public double getFaultRation(string Pq_Code)
        {
            ERP_Infos erp = new ERP_Infos();
            List<Notice> noticesUp = erp.getNoticeInfos_ByNoticeType(Pq_Code,"M2", "删除","3");
            List<Notice> noticesDown = erp.getNoticeInfos_ByNoticeTypes(Pq_Code,"删除", "3");
            if (noticesDown.Count() != 0)
                return (noticesUp.Count() / noticesDown.Count()) * 100;
            else
                return 0;
        }
        //一般机泵设备平均无故障间隔周期
        public double getNonFaultInterVal(string Pq_Code)
        {
            ERP_Infos erp = new ERP_Infos();
            List<Notice> noticesUp = erp.getNoticeInfos(Pq_Code,"投用");
            List<Notice> noticesDown1 = erp.getNoticeInfos_ByNoticeType(Pq_Code,"M2", "删除", "2");
            List<Notice> noticesDown2 = erp.getNoticeInfos_ByNoticeType(Pq_Code,"M2", "删除", "3");
            if (noticesDown1.Count() + noticesDown2.Count() != 0)
                return (noticesUp.Count() / (noticesDown1.Count() + noticesDown2.Count())) * 100;
            else
                return -1;
        }
        //设备投用率
        public double DeliverRatio(string Pq_Code)
        {
            double faultTimeSum = 0;
            DateTime D_end, D_start;
            ERP_Infos erp = new ERP_Infos();
            List<Notice> noticesUp = erp.getNoticeInfos_ByNoticeType(Pq_Code, "M2", "删除");
            List<Notice> noticesDown = erp.getNoticeInfos(Pq_Code, "投用");
            foreach (var n in noticesUp)
            {
                if (n.Notice_FaultEnd != "00000000")
                {
                    D_end = DateTime.ParseExact(n.Notice_FaultEnd, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);
                    D_start = DateTime.ParseExact(n.Notice_FaultStart, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);
                    faultTimeSum = faultTimeSum + (D_end - D_start).Days * 24;

                }

            }
            if (noticesDown.Count() != 0)
                return 1 - (faultTimeSum / (noticesDown.Count() * 30 * 24));
            else
                return 100;
        }
        //维修平均工时

        public double faultServiceTime_Avg(string Pq_Code)
        {
            double faultTimeSum = 0;

            DateTime D_end, D_start;
            ERP_Infos erp = new ERP_Infos();
            List<Notice> noticesUp = erp.getNoticeInfos_ByNoticeType(Pq_Code, "M2", "删除");
            List<Notice> noticesDown = erp.getNoticeInfos(Pq_Code, "投用");
            foreach (var n in noticesUp)
            {
                if (n.Notice_FaultEnd != "00000000")
                {
                    D_end = DateTime.ParseExact(n.Notice_FaultEnd, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);
                    D_start = DateTime.ParseExact(n.Notice_FaultStart, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);
                    faultTimeSum = faultTimeSum + (D_end - D_start).Days * 24;

                }

            }
            if (noticesDown.Count() != 0)
                return (faultTimeSum / (noticesDown.Count()));
            else
                return 0;
        }

       
        //public double DeliverRatio(string Pq_Code)
        //{
        //    double faultTimeSum = 0;
        //    ERP_Infos erp = new ERP_Infos();
        //    List<Notice> noticesUp = erp.getNoticeInfos_ByNoticeType(Pq_Code,"M2", "删除");
        //    List<Notice> noticesDown = erp.getNoticeInfos(Pq_Code,"投用");
        //    foreach(var n in noticesUp)
        //    {
        //        faultTimeSum = faultTimeSum + (000.000);

        //    }
        //    if (noticesDown.Count() != 0)
        //        return 1 - (faultTimeSum / (noticesDown.Count() * 30 * 24));
        //    else
        //        return 100;
        //}
        ////维修平均工时

        //public double faultServiceTime_Avg(string Pq_Code)
        //{
        //    double faultTimeSum = 0;
        //    ERP_Infos erp = new ERP_Infos();
        //    List<Notice> noticesUp = erp.getNoticeInfos_ByNoticeType(Pq_Code,"M2", "删除");
        //    List<Notice> noticesDown = erp.getNoticeInfos(Pq_Code,"投用");
        //    foreach (var n in noticesUp)
        //    {
        //        faultTimeSum = faultTimeSum + (000.000);

        //    }
        //    if (noticesDown.Count() != 0)
        //        return (faultTimeSum / (noticesDown.Count()));
        //    else
        //        return 0;
        //}



        //大机组统计，全厂统计Pq_Code=ALL
        public double bigEquipsRatio(string Pq_Code)
        {
            double faultTimeSum = 0;
            DateTime D_end, D_start;
            ERP_Infos erp = new ERP_Infos();
            List<Notice> noticesUp = erp.getNoticeInfos_BigEquip(Pq_Code, "M2", "删除", "3");
            int bigEquipCount = erp.geBigEquip_Num(Pq_Code);
            foreach (var n in noticesUp)
            {
                if (n.Notice_FaultEnd != "00000000")
                {
                    D_end = DateTime.ParseExact(n.Notice_FaultEnd + n.Notice_FaultEndTime, "yyyyMMddhhmmss", System.Globalization.CultureInfo.CurrentCulture);
                    D_start = DateTime.ParseExact(n.Notice_FaultStart + n.Notice_FaultEndTime, "yyyyMMddhhmmss", System.Globalization.CultureInfo.CurrentCulture);
                    faultTimeSum = faultTimeSum + (D_end - D_start).Days * 24 + (D_end - D_start).Hours;

                }

            }

            if (bigEquipCount != -1 && bigEquipCount != 0)
                return (faultTimeSum / (bigEquipCount * 30 * 24));
           else
                return -1;
        }

    }
}

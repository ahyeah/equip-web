using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EquipModel.Entities;
using EquipModel.Context;
namespace EquipDAL.Implement
{
    public class ERP_Infos : BaseDAO
    {
        //已知工单号，获取工单的用户状态和简要说明
        //适用流程A8.2
        public GongDan getGD_info(string gd_Id)
        {
            try
            {
                using (var db = new EquipWebContext())
                {
                   var GongdanList= db.Database.SqlQuery<GongDan>("select * from GongDan where GD_Id ='"+gd_Id +"'");
                   
                  List<GongDan> r= GongdanList.ToList<GongDan>();
                  return r.ElementAt(0);
                }
            }
            catch (Exception )
            { return null; }
        }
        public List<OilInfo> getOilInfo_Overdue(string curDate)
        {
            try
            {
                using (var db = new EquipWebContext())
                {

                    var oilInfoList = db.Database.SqlQuery<OilInfo>("select * from oilInfo where oil_NextDate <'" + curDate + "'");

                    List<OilInfo> r = oilInfoList.ToList<OilInfo>();
                    return r;
                }
            }
            catch (Exception)
            { return null; }
        }

        public GongDan getGD_Notice_info(string gd_Notice_Id)
        {
            try
            {
                using (var db = new EquipWebContext())
                {
                   var GongdanList= db.Database.SqlQuery<GongDan>("select * from GongDan where GD_NoticeId ='"+gd_Notice_Id +"'");
                   
                  List<GongDan> r= GongdanList.ToList<GongDan>();
                  return r.ElementAt(0);
                }
            }
            catch (Exception )
            { return null; }
        }
        
        //适用流程A6.2 
        //获取超期未换油信息
        //public List<OilInfo> getOilInfo_Overdue(string curDate)
        //{
        //    try
        //    {
        //        using (var db = new EquipWebContext())
        //        {
                    
        //            var oilInfoList = db.Database.SqlQuery<OilInfo>("select * from oilInfo where oil_NextDate <'" + curDate + "'");

        //            List<OilInfo> r = oilInfoList.ToList<OilInfo>();
        //            return r;
        //        }
        //    }
        //    catch (Exception)
        //    { return null; }
        //}


        //适用于KPI计算
        public List<Notice> getNoticeInfos_ByNoticeType(string Pq_code,string Notice_type,string Notice_state,string Notice_OverCondition="")
        {
            try
            {
                using (var db = new EquipWebContext())
                { 
                    string lastDate = DateTime.Now.AddMonths(-1).ToString("yyyyMM") + "16";
                    string nextDate = DateTime.Now.ToString("yyyyMM") + "15";
                   List<Notice> r ;
                   //string sql = "select * from Notice where  Notice_Type='" + Notice_type + "'  and Notice_state!='" + Notice_state + "' and Notice_CR_Date>='" + lastDate + "' and Notice_CR_Date<='" + nextDate + "' and Notice_PlanGroup in(select rtrim(ltrim(cName)) collate Chinese_PRC_CI_AS from Pq_Zz_map where Pq_Name = '" + Pq_code + "')";

                    if (Notice_OverCondition=="")
                        r = db.Database.SqlQuery<Notice>("select * from Notice where  Notice_Type='" + Notice_type + "'  and Notice_state!='" + Notice_state + "' and Notice_CR_Date>='" + lastDate + "' and Notice_CR_Date<='" + nextDate + "' and Notice_PlanGroup in (select rtrim(ltrim(Zz_Code)) collate Chinese_PRC_CI_AS from Pq_Zz_map where Pq_Name = '" + Pq_code + "')").ToList<Notice>();
                    else
                        r = db.Database.SqlQuery<Notice>("select * from Notice where Notice_Type='" + Notice_type + "'  and Notice_state!='" + Notice_state + "' and  Notice_OverConditon='" + Notice_OverCondition + "' and Notice_CR_Date>='" + lastDate + "' and Notice_CR_Date<='" + nextDate + "' and Notice_PlanGroup in (select rtrim(ltrim(Zz_Code)) collate Chinese_PRC_CI_AS from Pq_Zz_map where Pq_Name='" + Pq_code + "' )").ToList<Notice>(); 
                   
                    return r;
                }
            }
            catch (Exception)
            { return null; }
        }

        //不需要Notice_Type
        public List<Notice> getNoticeInfos_ByNoticeTypes(string Pq_code, string Notice_state, string Notice_OverCondition = "")
        {
            try
            {
                using (var db = new EquipWebContext())
                {
                    string lastDate = DateTime.Now.AddMonths(-1).ToString("yyyyMM") + "16";
                    string nextDate = DateTime.Now.ToString("yyyyMM") + "15";
                    List<Notice> r;
                    //string sql = "select * from Notice where  Notice_Type='" + Notice_type + "'  and Notice_state!='" + Notice_state + "' and Notice_CR_Date>='" + lastDate + "' and Notice_CR_Date<='" + nextDate + "' and Notice_PlanGroup in(select rtrim(ltrim(cName)) collate Chinese_PRC_CI_AS from Pq_Zz_map where Pq_Name = '" + Pq_code + "')";

                    if (Notice_OverCondition == "")
                        r = db.Database.SqlQuery<Notice>("select * from Notice where  Notice_state!='" + Notice_state + "' and Notice_CR_Date>='" + lastDate + "' and Notice_CR_Date<='" + nextDate + "' and Notice_PlanGroup in (select rtrim(ltrim(Zz_Code)) collate Chinese_PRC_CI_AS from Pq_Zz_map where Pq_Name = '" + Pq_code + "')").ToList<Notice>();
                    else
                        r = db.Database.SqlQuery<Notice>("select * from Notice where  Notice_state!='" + Notice_state + "' and  Notice_OverConditon='" + Notice_OverCondition + "' and Notice_CR_Date>='" + lastDate + "' and Notice_CR_Date<='" + nextDate + "' and Notice_PlanGroup in (select rtrim(ltrim(Zz_Code)) collate Chinese_PRC_CI_AS from Pq_Zz_map where Pq_Name='" + Pq_code + "' )").ToList<Notice>();

                    return r;
                }
            }
            catch (Exception)
            { return null; }
        }
        public List<Notice> getNoticeInfos(string Pq_code,string Notice_state)
        {
            try
            {
                using (var db = new EquipWebContext())
                {
                    string lastDate = DateTime.Now.AddMonths(-1).ToString("yyyyMM") + "16";
                    string nextDate = DateTime.Now.ToString("yyyyMM") + "15";
                    List<Notice> r;

                    r = db.Database.SqlQuery<Notice>("select * from Notice where  Notice_state !='" + Notice_state + "' and Notice_CR_Date>='" + lastDate + "' and Notice_CR_Date<='" + nextDate + "' and Notice_PlanGroup  in (select rtrim(ltrim(Zz_Code)) collate Chinese_PRC_CI_AS from Pq_Zz_map where Pq_Name='" + Pq_code + "')").ToList<Notice>();

                    return r;
                }
            }
            catch (Exception)
            { return null; }
        }

        //全厂统计 Pq_Code=ALL
        public List<Notice> getNoticeInfos_BigEquip(string Pq_code, string Notice_type, string Notice_state, string Notice_OverCondition)
        {
            try
            {
                using (var db = new EquipWebContext())
                {
                    string lastDate = DateTime.Now.AddMonths(-1).ToString("yyyyMM") + "16000000";
                    string nextDate = DateTime.Now.ToString("yyyyMM") + "15235959";
                    List<Notice> r;
                    //string asd = "select * from Notice where Notice_Type +'" + Notice_type + "' and Notice_State!='" + Notice_state + "' and Notice_FaultStart+Notice_FaultStartTime>='" + lastDate + "' and  Notice_FaultEnd+Notice_FaultEndTime<='" + nextDate + "' and Notice_OverConditon='" + Notice_OverCondition + "' and Notice_EquipCode in (select rtrim(ltrim(Equip_Code)) collate Chinese_PRC_CI_AS from Pq_EC_map where Pq_Name='" + Pq_code + "')";
                    if (Pq_code == "ALL")


                        r = db.Database.SqlQuery<Notice>("select * from Notice where Notice_Type ='" + Notice_type + "' and Notice_State!='" + Notice_state + "' and Notice_FaultStart+Notice_FaultStartTime>='" + lastDate + "' and  Notice_FaultEnd+Notice_FaultEndTime<='" + nextDate + "' and Notice_OverConditon='" + Notice_OverCondition + "' and Notice_EquipCode in (select rtrim(ltrim(Equip_Code)) collate Chinese_PRC_CI_AS from Pq_EC_map)").ToList<Notice>();
                    else
                        r = db.Database.SqlQuery<Notice>("select * from Notice where Notice_Type ='" + Notice_type + "' and Notice_State!='" + Notice_state + "' and Notice_FaultStart+Notice_FaultStartTime>='" + lastDate + "' and  Notice_FaultEnd+Notice_FaultEndTime<='" + nextDate + "' and Notice_OverConditon='" + Notice_OverCondition + "' and Notice_EquipCode in (select rtrim(ltrim(Equip_Code)) collate Chinese_PRC_CI_AS from Pq_EC_map where Pq_Name='" + Pq_code + "')").ToList<Notice>();

                    return r;
                }
            }
            catch (Exception)
            { return null; }
        }


        public int geBigEquip_Num(string Pq_code)
        {
            try
            {
                using (var db = new EquipWebContext())
                {
                    int r = 0;
                    if (Pq_code == "ALL")


                        r = db.Database.SqlQuery<Notice>("select * from Pq_EC_map").Count();
                    else
                        r = db.Database.SqlQuery<Notice>("select * from Pq_EC_map where Pq_Name='" + Pq_code + "'").Count();

                    return r;
                }
            }
            catch (Exception)
            { return -1; }

        }
       
    }
}

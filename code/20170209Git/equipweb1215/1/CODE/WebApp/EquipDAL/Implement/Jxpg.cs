using EquipDAL.Interface;
using EquipModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipDAL.Implement
{
    public class Jxpg : BaseDAO
    {

        public List<A15dot1Tab> GetJxRecord(string roles,string dep, string name)
        {   List<A15dot1Tab> e=new List<A15dot1Tab>();
            using (var db = base.NewDB())
            {
                if (dep.Contains("机动处"))
                {
                    if (roles.Contains("可靠性工程师") || roles.Contains("检维修人员"))
                    {
                        return db.A15dot1Tab.Where(a => (a.state == 1 || a.state == 2 ||a.state == 0) && a.submitUser == name).ToList();
                    }
                    else
                    return db.A15dot1Tab.Where(a => a.state == 1 || a.state == 2).ToList();
                }
                else if (roles.Contains("可靠性工程师") || roles.Contains("检维修人员"))
                {
                    return db.A15dot1Tab.Where(a => a.state == 0).ToList();
                }
                else
                    return e;
            }
        }
        public List<A15dot1Tab> GetJxRecord_detail(int id)
        {
            using (var db = base.NewDB())
            {
                return db.A15dot1Tab.Where(a => a.Id == id).ToList();
            }
        }
        public List<A15dot1Tab> GetHisJxRecord(string roles, string dep, string name)
        {
            List<A15dot1Tab> e = new List<A15dot1Tab>();
            using (var db = base.NewDB())
            {
                if (dep.Contains("机动处"))
                {
                    return db.A15dot1Tab.Where(a => a.state == 3).ToList();
                }
                else if (roles.Contains("可靠性工程师") || roles.Contains("检维修人员"))
                {
                    return db.A15dot1Tab.Where(a => a.state == 3 && a.submitUser == name).ToList();
                }
                else
                    return e;
                
            }
        }
        public List<A15dot1Tab> GetHisJxRecord_detail(int id)
        {
            using (var db = base.NewDB())
            {
                 return db.A15dot1Tab.Where(a => a.Id == id).ToList();
            }
        }


        public bool AddJxRecord(A15dot1Tab nVal)
        {
            using (var db = base.NewDB())
            {
                try
                {
                    
                    db.A15dot1Tab.Add(nVal);                                      
                    db.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }



        public bool ModifyJxRecord(A15dot1Tab nVal)
        {
            using (var db = base.NewDB())
            {
                try
                {
                    var modifyA15 = db.A15dot1Tab.Where(a => a.Id == nVal.Id).First();

                    modifyA15.timesNonPlanStop = nVal.timesNonPlanStop;
                    modifyA15.scoreDeductFaultIntensity = nVal.scoreDeductFaultIntensity;
                    modifyA15.rateBigUnitFault = nVal.rateBigUnitFault;
                    modifyA15.rateFaultMaintenance = nVal.rateFaultMaintenance;
                    modifyA15.MTBF = nVal.MTBF;
                    modifyA15.rateEquipUse = nVal.rateEquipUse;
                    modifyA15.rateUrgentRepairWorkHour = nVal.rateUrgentRepairWorkHour;
                    modifyA15.hourWorkOrderFinish = nVal.hourWorkOrderFinish;
                    modifyA15.avgLifeSpanSeal = nVal.avgLifeSpanSeal;
                    modifyA15.avgLifeSpanAxle = nVal.avgLifeSpanAxle;
                    modifyA15.percentEquipAvailability = nVal.percentEquipAvailability;
                    modifyA15.percentPassOnetimeRepair = nVal.percentPassOnetimeRepair;
                    modifyA15.avgEfficiencyPump = nVal.avgEfficiencyPump;
                    modifyA15.avgEfficiencyUnit = nVal.avgEfficiencyUnit;
                    modifyA15.hiddenDangerInvestigation = nVal.hiddenDangerInvestigation;
                    modifyA15.rateLoad = nVal.rateLoad;
                    modifyA15.gyChange = nVal.gyChange;
                    modifyA15.equipChange = nVal.equipChange;
                    modifyA15.otherDescription = nVal.otherDescription;
                    modifyA15.evaluateEquipRunStaeDesc = nVal.evaluateEquipRunStaeDesc;
                    modifyA15.evaluateEquipRunStaeImgPath = nVal.evaluateEquipRunStaeImgPath;
                    modifyA15.reliabilityConclusion = nVal.reliabilityConclusion;
                    modifyA15.jdcAdviseImproveMeasures = nVal.jdcAdviseImproveMeasures;
                    modifyA15.submitDepartment = nVal.submitDepartment;
                    modifyA15.submitUser = nVal.submitUser;
                    modifyA15.submitTime = nVal.submitTime;                    
                    modifyA15.reportType = nVal.reportType;                   
                    modifyA15.submitUser = nVal.submitUser;
                    modifyA15.submitTime = nVal.submitTime;
                    modifyA15.state = nVal.state;
                    


                    db.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }
        public bool JdcModifyJxRecord(A15dot1Tab nVal)
        {
            using (var db = base.NewDB())
            {
                try
                {
                    var modifyA15 = db.A15dot1Tab.Where(a => a.Id == nVal.Id).First();

                    modifyA15.jdcAdviseImproveMeasures = nVal.jdcAdviseImproveMeasures;
                    modifyA15.jdcOperateTime = nVal.jdcOperateTime;
                    modifyA15.jdcOperator = nVal.jdcOperator;
                    modifyA15.state = nVal.state;



                    db.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }
        //查询表中一列的数据，列名不定
        public List<object> qstdata(string grahpic_name, string pianqu)
        {
            using (var db = base.NewDB())
            {
                var i = db.Database.SqlQuery<A15dot1Tab>("select * from A15dot1Tab where submitDepartment='" + pianqu + "'").ToList();

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

        public List<A15dot1Tab> GetJxItemforA2Tab(DateTime time,DateTime time2)
        {
            using (var db = base.NewDB())
            {
                return db.A15dot1Tab.Where(a => a.state == 3 && a.jdcOperateTime < time &&a.jdcOperateTime>time2).ToList();
            }
        }

        public List<A15dot1Tab> newGetJxItemforA2Tab(DateTime time, DateTime time2, string pianqu)
        {
            using (var db = base.NewDB())
            {
                return db.A15dot1Tab.Where(a => a.state == 3 && a.jdcOperateTime < time && a.jdcOperateTime > time2 && a.submitDepartment == pianqu).ToList();
            }
        }
    }
}

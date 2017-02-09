using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipModel.Entities
{
    //与工作流相关的表（在EquipWeb数据库中）

    public class A15dot1Tab  //设备绩效管理，片区或检修单位提报，机动处给出建议及改进措施
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //主键
        public int Id { get; set; }

        //[Required()]

        //非计划停工次数	
        public int timesNonPlanStop { get; set; }

        //非计划停工次数***参与统计台数
        public int Count_timesNonPlanStop { get; set; }

        //四类以上故障强度扣分	
        public int scoreDeductFaultIntensity { get; set; }

        //四类以上故障强度扣分***参与统计台数	
        public int Count_scoreDeductFaultIntensity { get; set; }

        //大机组故障率K	
        public double rateBigUnitFault { get; set; }


        //大机组故障率K	***参与统计台数
        public int Count_rateBigUnitFault { get; set; }

        //故障维修率F	
        public double rateFaultMaintenance { get; set; }

        //故障维修率F***参与统计台数	
        public int Count_rateFaultMaintenance { get; set; }

        //一般机泵设备平均无故障间隔期MTBF	
        public double MTBF { get; set; }

        //一般机泵设备平均无故障间隔期MTBF***参与统计台数	
        public int Count_MTBF { get; set; }

        //设备投用率R（反映MTTR）	
        public double rateEquipUse { get; set; }

        //设备投用率R（反映MTTR）***参与统计台数		
        public int Count_rateEquipUse { get; set; }

        //紧急抢修工时率C--成本能效指标	
        public double rateUrgentRepairWorkHour { get; set; }

        //维修工单有效完成时间T	-小时--成本能效指标
        public double hourWorkOrderFinish { get; set; }

        //机械密封平均寿命s	-小时
        public int avgLifeSpanSeal { get; set; }

        //机械密封平均寿命s	-小时***参与统计台数
        public int Count_avgLifeSpanSeal { get; set; }

        //轴承平均寿命B	-小时
        public int avgLifeSpanAxle { get; set; }

        //轴承平均寿命B	-小时***参与统计台数
        public int Count_avgLifeSpanAxle { get; set; }

        //设备完好率W	
        public double percentEquipAvailability { get; set; }

        //设备完好率W***参与统计台数	
        public int Count_percentEquipAvailability { get; set; }

        //检修一次合格率	
        public double percentPassOnetimeRepair { get; set; }

        //检修一次合格率***参与统计台数	
        public int Count_percentPassOnetimeRepair { get; set; }

        //机泵平均效率n1--统计指标	
        public double avgEfficiencyPump { get; set; }

        //机组平均效率n2--统计指标	
        public double avgEfficiencyUnit { get; set; }

        //隐患排查情况	
        public string hiddenDangerInvestigation { get; set; }

        //负荷率	
        public string rateLoad { get; set; }

        //工艺变更	
        public string gyChange { get; set; }

        //设备变更	
        public string equipChange { get; set; }

        //其他说明	
        public string otherDescription { get; set; }

        //装置设备运行情况基本评估	
        public string evaluateEquipRunStaeDesc { get; set; }

        //装置设备运行情况基本评估附图	
        public string evaluateEquipRunStaeImgPath { get; set; }

        //可靠性结论	
        public string reliabilityConclusion { get; set; }

        //机动处建议及改进措施	
        public string jdcAdviseImproveMeasures { get; set; }

        //提交单位：	XX片区或检修单位
        public string submitDepartment { get; set; }

        //单位提交人	
        public string submitUser { get; set; }

        //单位提交时间	
        public DateTime? submitTime { get; set; }

        //机动处操作人	
        public string jdcOperator { get; set; }

        //机动处操作时间	
        public DateTime? jdcOperateTime { get; set; }

        //进度状态：    0-单位保存，1-单位提交完成，2-机动处保存，3-机动处提交	
        public int state { get; set; }

        //报告类型：	 月报或年报
        public string reportType { get; set; }

        //预留字段1
        public string temp1 { get; set; }
        //预留字段2
        public string temp2 { get; set; }
        //预留字段3
        public string temp3 { get; set; }
    }

    public class A5dot1Tab1   //用于提报不完好内容
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //主键
        public int Id { get; set; }

        //[Required()]
        public string pqName { get; set; }
        //车间	
        public string cjName { get; set; }

        //装置	
        public string zzName { get; set; }

        //设备位号	
        public string sbGyCode { get; set; }

        //设备编号
        [Required()]
        public string sbCode { get; set; }

        //设备型号	
        public string sbType { get; set; }

        //不完好内容	
        public string notGoodContent { get; set; }

        //是否整改	0-未整改 1-已整改
        public int isRectified { get; set; }

        //专业分类	
        public string zyType { get; set; }

        //现场工程师	
        public string zzUserName { get; set; }

        //现场工程师提报时间	
        public DateTime? zzSubmitTime { get; set; }

        //可靠性工程师	
        public string pqUserName { get; set; }

        //可靠性工程师检查时间	
        public DateTime? pqCheckTime { get; set; }

        //统计年月	
        public string yearMonthForStatistic { get; set; }

        //数据来源:	A5dot1现场工程师提报；A5dot1检修单位新增；A13dot1
        public string dataSource { get; set; }

        //预留字段1
        public string temp1 { get; set; }
        //预留字段2
        public string temp2 { get; set; }
        //预留字段3
        public string temp3 { get; set; }
    }

    public class A5dot1Tab2  //用于统计不完好内容，以及筛选最差5台和10台
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //主键
        public int Id { get; set; }

        //[Required()]
        public string pqName { get; set; }
        //车间	
        public string cjName { get; set; }

        //装置	
        public string zzName { get; set; }

        //设备位号	
        public string sbGyCode { get; set; }

        //设备编号
        [Required()]
        public string sbCode { get; set; }

        //设备型号	
        public string sbType { get; set; }

        //不完好内容及整改情况	
        public string notGoodContents { get; set; }

        //当月不完好次数	
        public int  timesNotGood { get; set; }

        //当前累计未整改不完好项数	
        public int countAllNoRectifed { get; set; }

        //专业分类	
        public string zyType { get; set; }

        //问题描述	
        public string problemDescription { get; set; }

        //问题分析与处理建议	
        public string problemAnalysisAdvise { get; set; }

        //处理手段与方法	
        public string processMeansMethods { get; set; }

        //处理结果	勾选-待处理、处理中、已处理
        public string processReuslt { get; set; }

        //是否列入最差5台机泵	0-否（默认），1-是
        public int isSetAsTop5Worst { get; set; }

        //是否列入最差10台机泵		0-否（默认），1-是
        public int isSetAsTop10Worst { get; set; }

        //可靠性工程师	
        public string pqUserName { get; set; }

        //检修单位人员	
        public string jxdwUserName { get; set; }

        //机动处	
        public string jdcUserName { get; set; }

        //统计年月	
        public string yearMonthForStatistic { get; set; }

        //进度状态	0-默认；1-片区挑选完成；2-检修单位修改完成；3-机动处汇总完成
        public int state { get; set; }

        //预留字段1
        public string temp1 { get; set; }
        //预留字段2
        public string temp2 { get; set; }
        //预留字段3
        public string temp3 { get; set; }
    }

    public class A5dot2Tab1   //用于提报竖向问题
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //主键
        public int Id { get; set; }

        //[Required()]
        public string pqName { get; set; }
        //车间	
        public string cjName { get; set; }

        //装置	
        public string zzName { get; set; }

        //设备位号	
        public string sbGyCode { get; set; }

        //设备编号
        [Required()]
        public string sbCode { get; set; }

        //设备型号	
        public string sbType { get; set; }

        //竖向问题描述	
        public string problemDescription { get; set; }

        //是否整改	0-未整改 1-已整改
        public int isRectified { get; set; }

        //专业分类	
        public string zyType { get; set; }

        //检修人员	
        public string jxUserName { get; set; }

        //检修人员提报时间	
        public DateTime? jxSubmitTime { get; set; }

        //可靠性工程师	
        public string pqUserName { get; set; }

        //可靠性工程师检查时间	
        public DateTime? pqCheckTime { get; set; }

        //统计年月	
        public string yearMonthForStatistic { get; set; }

        //进度状态	0-默认；1-片区操作完成
        public int state { get; set; }

        //预留字段1
        public string temp1 { get; set; }
        //预留字段2
        public string temp2 { get; set; }
        //预留字段3
        public string temp3 { get; set; }
    }

    public class A5dot2Tab2   //用于统计竖向问题，以及筛选最脏10台
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //主键
        public int Id { get; set; }

        //[Required()]
        public string pqName { get; set; }
        //车间	
        public string cjName { get; set; }

        //装置	
        public string zzName { get; set; }

        //设备位号	
        public string sbGyCode { get; set; }

        //设备编号
        [Required()]
        public string sbCode { get; set; }

        //设备型号	
        public string sbType { get; set; }

        //本月竖向问题次数	
        public int nProblemsInCurMonth { get; set; }

        //全年竖向问题次数	
        public int nProblemsInCurYear { get; set; }

        //专业分类	
        public string zyType { get; set; }

        //问题描述	
        public string problemDescription { get; set; }

        //是否列入最脏10台机泵	0-否（默认），1-是
        public int isSetAsTop10Worst { get; set; }

        //机动处	
        public string jdcUserName { get; set; }

        //机动处操作时间	
        public DateTime? jdcOperateTime { get; set; }

        //统计年月	
        public string yearMonthForStatistic { get; set; }

        //进度状态	0-默认；1-机动处操作完成
        public int state { get; set; }

        //预留字段1
        public string temp1 { get; set; }
        //预留字段2
        public string temp2 { get; set; }
        //预留字段3
        public string temp3 { get; set; }
    }
    public class A6dot2Tab1  //用于记录上传五定表的日志信息
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string userName { get; set; }
        public string pqName { get; set; }
        public string uploadtime { get; set; }
        public string uuploadFileName { get; set; }
        public string uploadDesc { get; set; }
        public virtual ICollection<A6dot2Tab2> WDTable_details { get; set; }

    }

    public class A6dot2Tab2 //用于记录上传五定表的详细数据
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string equipCode { get; set; }
        public string equipDesc { get; set; }
        public string funLoc { get; set; }
        public string funLoc_desc { get; set; }
        public string oilLoc { get; set; }
        public string oilLoc_desc { get; set; }
        public int oilInterval { get; set; }
        public string unit { get; set; }
        public string lastOilTime { get; set; }
        public double lastOilNumber { get; set; }

        public string lastOilUnit { get; set; }
        public string NextOilTime { get; set; }
        public double NextOilNumber { get; set; }
        public string NextOilUnit { get; set; }
        public string oilCode { get; set; }
        public string oilCode_desc { get; set; }
        public string substiOilCode { get; set; }
        public string substiOilCode_desc { get; set; }

        public string equip_PqName { get; set; }
        public string equip_CjName { get; set; }
        public string equip_ZzName { get; set; }
        public int isExceed { get; set; }
        public int isOilType { get; set; }
        public int isValid { get; set; }
        public int Tab1_Id { get; set; }
        public virtual A6dot2Tab1 Tab1_Belong { get; set; }

    }

    public class A6dot2LsTaskTab
        {
        /// <summary>
        /// 主键
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Zz_Name { get; set; }
        public string Equip_Gybh { get; set; }
        public string Equip_Code { get; set; }
        public string lastOilTime { get; set; }
        public int oilInterval { get; set; }
        public string NextOilTime { get; set; }
        public string cur_problem { get; set; }
        public string cur_status { get; set; }
        public string wfd_id { get; set; }
        }
}

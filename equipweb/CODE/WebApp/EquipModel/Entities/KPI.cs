using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipModel.Entities
{
    public class A15dot1TabJing  //静
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //主键
        public int Id { get; set; }

        //[Required()]

        //故障强度扣分
        public int gzqdkf { get; set; }


        //设备腐蚀泄漏次数
        public int sbfsxlcs { get; set; }

        //千台冷换设备管束
        public double qtlhsbgs { get; set; }

        //工业炉平均热效效率
        public double gylpjrxl { get; set; }

        //换热器检修率
        public double hrqjxl { get; set; }

        //压力容器定检率
        public double ylrqdjl { get; set; }

        //压力管道年度检验计划完成率
        public double ylgdndjxjhwcl { get; set; }
        //安全阀年度校验计划完成率
        public double aqfndjyjhwcl { get; set; }
        //设备腐蚀检测计划完成率
        public double sbfsjcjhwcl { get; set; }
        //静设备检维修一次合格率
        public double jsbjwxychgl { get; set; }



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
    public class A15dot1TabDian  //电
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //主键
        public int Id { get; set; }

        //[Required()]

        //故障强度扣分
        public int gzqdkf { get; set; }
        //电气误操作次数
        public int dqwczcs { get; set; }

        //继电保护正确动作率
        public double jdbhzqdzl { get; set; }
        //设备故障率 
        public double sbgzl { get; set; }
        // 电机MTBF

        public double djMTBF { get; set; }
        //电力电子设备MTBF
        public double dzdlsbMTBF { get; set; }
        //主变功率因素
        public double zbglys { get; set; }
        //电能计量平衡率
        public double dnjlphl { get; set; }
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
    public class A15dot1TabYi  //仪
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //主键
        public int Id { get; set; }

        //[Required()]


        //故障强度扣分
        public int gzqdkf { get; set; }
        //联锁系统正确动作率
        public double lsxtzqdzl { get; set; }
        // 控制系统故障次数
        public int kzxtgzcs { get; set; }
        //仪表控制率
        public double ybkzl { get; set; }
        //仪表实际控制率

        public double ybsjkzl { get; set; }
        //联锁系统投用率
        public double lsxttyl { get; set; }
        //关键控制阀门故障次数
        public int gjkzfmgzcs { get; set; }
        //控制系统故障报警次数
        public int kzxtgzbjcs { get; set; }
        // 常规仪表故障率
        public double cgybgzl { get; set; }
        //调节阀门MTBF
        public double tjfMDBF { get; set; }

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
    public class A15dot1TabQiYe  //企业级KPI
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //主键
        public int Id { get; set; }

        //[Required()]
        //装置可靠性指数
        public double zzkkxzs { get; set; }
        //维修费用指数
        public double wxfyzs { get; set; }
        //千台离心泵（机泵）密封消耗率
        public double qtlxbmfxhl { get; set; }
        //千台冷换设备管束（含整台）更换率
        public double qtlhsbgsghl { get; set; }

        //仪表实际控制率

        public double ybsjkzl { get; set; }
        //事件数
        public int sjs { get; set; }
        //故障强度扣分
        public int gzqdkf { get; set; }

        //项目逾期率
        public double xmyql { get; set; }
        //培训及能力
        public double pxjnl { get; set; }



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
    public class A15dot1TabDong  //动
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //主键
        public int Id { get; set; }

        //[Required()]
        //故障强度扣分
        public int gzqdkf { get; set; }
        //大机组故障率
        public double djzgzl { get; set; }
        //故障维修率
        public double gzwxl { get; set; }
        //千台离心泵密封消耗率
        public double qtlxbmfxhl { get; set; }

        //紧急抢修工时率

        public double jjqxgsl { get; set; }
        //工单平均完成时间
        public double gdpjwcsj { get; set; }
        //机械密封检修率
        public double jxmfpjsm { get; set; }
        //轴承平均寿命
        public double zcpjsm { get; set; }

        //设备完好率
        public double sbwhl { get; set; }
        //检修一次合格率
        public double jxychgl { get; set; }
        //主要机泵平均效率
        public double zyjbpjxl { get; set; }

        //机组平均效率
        public double jzpjxl { get; set; }
        //往复机组故障率
        public double wfjzgzl { get; set; }
        //年度百台机泵重复维修率
        public double ndbtjbcfjxtc { get; set; }
        //机泵平均无故障间隔时间MTBF
        public double jbpjwgzjgsjMTBF { get; set; }

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
}

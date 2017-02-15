using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EquipModel.Entities
{

    public class A15dot1TabJing_Weight  //静
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //主键
        public int Id { get; set; }

        //[Required()]

        //故障强度扣分
        public double gzqdkf_weight { get; set; }


        //设备腐蚀泄漏次数
        public double sbfsxlcs_weight { get; set; }

        //千台冷换设备管束
        public double qtlhsbgs_weight { get; set; }

        //工业炉平均热效效率
        public double gylpjrxl_weight { get; set; }

        //换热器检修率
        public double hrqjxl_weight { get; set; }

        //压力容器定检率
        public double ylrqdjl_weight { get; set; }

        //压力管道年度检验计划完成率
        public double ylgdndjxjhwcl_weight { get; set; }
        //安全阀年度校验计划完成率
        public double aqfndjyjhwcl_weight { get; set; }
        //设备腐蚀检测计划完成率
        public double sbfsjcjhwcl_weight { get; set; }
        //静设备检维修一次合格率
        public double jsbjwxychgl_weight { get; set; }
        //片区名称
        public string Pq_Name { get; set; }
        //装置计划元组
        public string Zz_Code { get; set; }
        //装置名称
        public string Zz_Name { get; set; }


    }
    public class A15dot1TabDian_Weight  //电
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //主键
        public int Id { get; set; }

        //[Required()]

        //故障强度扣分
        public double gzqdkf_weight { get; set; }
        //电气误操作次数
        public double dqwczcs_weight { get; set; }

        //继电保护正确动作率
        public double jdbhzqdzl_weight { get; set; }
        //设备故障率 
        public double sbgzl_weight { get; set; }
        // 电机MTBF

        public double djMTBF_weight { get; set; }
        //电力电子设备MTBF
        public double dzdlsbMTBF_weight { get; set; }
        //主变功率因素
        public double zbglys_weight { get; set; }
        //电能计量平衡率
        public double dnjlphl_weight { get; set; }
        //片区名称
        public string Pq_Name { get; set; }
        //装置计划元组
        public string Zz_Code { get; set; }
        //装置名称
        public string Zz_Name { get; set; }
    }

    public class A15dot1TabYi_Weight  //仪
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //主键
        public int Id { get; set; }

        //[Required()]


        //故障强度扣分
        public double gzqdkf_weight { get; set; }
        //联锁系统正确动作率
        public double lsxtzqdzl_weight { get; set; }
        // 控制系统故障次数
        public double kzxtgzcs_weight { get; set; }
        //仪表控制率
        public double ybkzl_weight { get; set; }
        //仪表实际控制率

        public double ybsjkzl_weight { get; set; }
        //联锁系统投用率
        public double lsxttyl_weight { get; set; }
        //关键控制阀门故障次数
        public double gjkzfmgzcs_weight { get; set; }
        //控制系统故障报警次数
        public double kzxtgzbjcs_weight { get; set; }
        // 常规仪表故障率
        public double cgybgzl_weight { get; set; }
        //调节阀门MTBF
        public double tjfMDBF_weight { get; set; }

        //片区名称
        public string Pq_Name { get; set; }
        //装置计划元组
        public string Zz_Code { get; set; }
        //装置名称
        public string Zz_Name { get; set; }
    }

    public class A15dot1TabQiYe_Weight  //企业级KPI
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //主键
        public int Id { get; set; }

        //[Required()]
        //装置可靠性指数
        public double zzkkxzs_weight { get; set; }
        //维修费用指数
        public double wxfyzs_weight { get; set; }
        //千台离心泵（机泵）密封消耗率
        public double qtlxbmfxhl_weight { get; set; }
        //千台冷换设备管束（含整台）更换率
        public double qtlhsbgsghl_weight { get; set; }

        //仪表实际控制率

        public double ybsjkzl_weight { get; set; }
        //事件数
        public double sjs_weight { get; set; }
        //故障强度扣分
        public double gzqdkf_weight { get; set; }

        //项目逾期率
        public double xmyql_weight { get; set; }
        //培训及能力
        public double pxjnl_weight { get; set; }
        //片区名称
        public string Pq_Name { get; set; }
        //装置计划元组
        public string Zz_Code { get; set; }
        //装置名称
        public string Zz_Name { get; set; }
    }

    public class A15dot1TabDong_Weight  //动
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //主键
        public int Id { get; set; }

        //[Required()]
        //故障强度扣分
        public double gzqdkf_weight { get; set; }
        //大机组故障率
        public double djzgzl_weight { get; set; }
        //故障维修率
        public double gzwxl_weight { get; set; }
        //千台离心泵密封消耗率
        public double qtlxbmfxhl_weight { get; set; }

        //紧急抢修工时率

        public double jjqxgsl_weight { get; set; }
        //工单平均完成时间
        public double gdpjwcsj_weight { get; set; }
        //机械密封检修率
        public double jxmfpjsm_weight { get; set; }
        //轴承平均寿命
        public double zcpjsm_weight { get; set; }

        //设备完好率
        public double sbwhl_weight { get; set; }
        //检修一次合格率
        public double jxychgl_weight { get; set; }
        //主要机泵平均效率
        public double zyjbpjxl_weight { get; set; }

        //机组平均效率
        public double jzpjxl_weight { get; set; }
        //往复机组故障率
        public double wfjzgzl_weight { get; set; }
        //年度百台机泵重复维修率
        public double ndbtjbcfjxtc_weight { get; set; }
        //机泵平均无故障间隔时间MTBF
        public double jbpjwgzjgsjMTBF_weight { get; set; }

        //片区名称
        public string Pq_Name { get; set; }
        //装置计划元组
        public string Zz_Code { get; set; }
        //装置名称
        public string Zz_Name { get; set; }
    }

    public class Pq_A15dot1TabJing_Weight  //静
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //主键
        public int Id { get; set; }

        //[Required()]

        //故障强度扣分
        public double gzqdkf_weight { get; set; }


        //设备腐蚀泄漏次数
        public double sbfsxlcs_weight { get; set; }

        //千台冷换设备管束
        public double qtlhsbgs_weight { get; set; }

        //工业炉平均热效效率
        public double gylpjrxl_weight { get; set; }

        //换热器检修率
        public double hrqjxl_weight { get; set; }

        //压力容器定检率
        public double ylrqdjl_weight { get; set; }

        //压力管道年度检验计划完成率
        public double ylgdndjxjhwcl_weight { get; set; }
        //安全阀年度校验计划完成率
        public double aqfndjyjhwcl_weight { get; set; }
        //设备腐蚀检测计划完成率
        public double sbfsjcjhwcl_weight { get; set; }
        //静设备检维修一次合格率
        public double jsbjwxychgl_weight { get; set; }
        //片区名称
        public string Pq_Name { get; set; }
        //装置计划元组
        public string Zz_Code { get; set; }
        //装置名称
        public string Zz_Name { get; set; }


    }
    public class Pq_A15dot1TabDian_Weight  //电
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //主键
        public int Id { get; set; }

        //[Required()]

        //故障强度扣分
        public double gzqdkf_weight { get; set; }
        //电气误操作次数
        public double dqwczcs_weight { get; set; }

        //继电保护正确动作率
        public double jdbhzqdzl_weight { get; set; }
        //设备故障率 
        public double sbgzl_weight { get; set; }
        // 电机MTBF

        public double djMTBF_weight { get; set; }
        //电力电子设备MTBF
        public double dzdlsbMTBF_weight { get; set; }
        //主变功率因素
        public double zbglys_weight { get; set; }
        //电能计量平衡率
        public double dnjlphl_weight { get; set; }
        //片区名称
        public string Pq_Name { get; set; }
        //装置计划元组
        public string Zz_Code { get; set; }
        //装置名称
        public string Zz_Name { get; set; }
    }

    public class Pq_A15dot1TabYi_Weight  //仪
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //主键
        public int Id { get; set; }

        //[Required()]


        //故障强度扣分
        public double gzqdkf_weight { get; set; }
        //联锁系统正确动作率
        public double lsxtzqdzl_weight { get; set; }
        // 控制系统故障次数
        public double kzxtgzcs_weight { get; set; }
        //仪表控制率
        public double ybkzl_weight { get; set; }
        //仪表实际控制率

        public double ybsjkzl_weight { get; set; }
        //联锁系统投用率
        public double lsxttyl_weight { get; set; }
        //关键控制阀门故障次数
        public double gjkzfmgzcs_weight { get; set; }
        //控制系统故障报警次数
        public double kzxtgzbjcs_weight { get; set; }
        // 常规仪表故障率
        public double cgybgzl_weight { get; set; }
        //调节阀门MTBF
        public double tjfMDBF_weight { get; set; }

        //片区名称
        public string Pq_Name { get; set; }
        //装置计划元组
        public string Zz_Code { get; set; }
        //装置名称
        public string Zz_Name { get; set; }
    }

    public class Pq_A15dot1TabQiYe_Weight  //企业级KPI
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //主键
        public int Id { get; set; }

        //[Required()]
        //装置可靠性指数
        public double zzkkxzs_weight { get; set; }
        //维修费用指数
        public double wxfyzs_weight { get; set; }
        //千台离心泵（机泵）密封消耗率
        public double qtlxbmfxhl_weight { get; set; }
        //千台冷换设备管束（含整台）更换率
        public double qtlhsbgsghl_weight { get; set; }

        //仪表实际控制率

        public double ybsjkzl_weight { get; set; }
        //事件数
        public double sjs_weight { get; set; }
        //故障强度扣分
        public double gzqdkf_weight { get; set; }

        //项目逾期率
        public double xmyql_weight { get; set; }
        //培训及能力
        public double pxjnl_weight { get; set; }
        //片区名称
        public string Pq_Name { get; set; }
        //装置计划元组
        public string Zz_Code { get; set; }
        //装置名称
        public string Zz_Name { get; set; }
    }

    public class Pq_A15dot1TabDong_Weight  //动
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //主键
        public int Id { get; set; }

        //[Required()]
        //故障强度扣分
        public double gzqdkf_weight { get; set; }
        //大机组故障率
        public double djzgzl_weight { get; set; }
        //故障维修率
        public double gzwxl_weight { get; set; }
        //千台离心泵密封消耗率
        public double qtlxbmfxhl_weight { get; set; }

        //紧急抢修工时率

        public double jjqxgsl_weight { get; set; }
        //工单平均完成时间
        public double gdpjwcsj_weight { get; set; }
        //机械密封检修率
        public double jxmfpjsm_weight { get; set; }
        //轴承平均寿命
        public double zcpjsm_weight { get; set; }

        //设备完好率
        public double sbwhl_weight { get; set; }
        //检修一次合格率
        public double jxychgl_weight { get; set; }
        //主要机泵平均效率
        public double zyjbpjxl_weight { get; set; }

        //机组平均效率
        public double jzpjxl_weight { get; set; }
        //往复机组故障率
        public double wfjzgzl_weight { get; set; }
        //年度百台机泵重复维修率
        public double ndbtjbcfjxtc_weight { get; set; }
        //机泵平均无故障间隔时间MTBF
        public double jbpjwgzjgsjMTBF_weight { get; set; }

        //片区名称
        public string Pq_Name { get; set; }
        //装置计划元组
        public string Zz_Code { get; set; }
        //装置名称
        public string Zz_Name { get; set; }
    }

}


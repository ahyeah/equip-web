using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipModel.Entities
{
 public   class jingmodel
    {
        public double? gzqdkf { get; set; }


        //设备腐蚀泄漏次数
        public double ?sbfsxlcs { get; set; }

        //千台冷换设备管束
        public double? qtlhsbgs { get; set; }

        //工业炉平均热效效率
        public double? gylpjrxl { get; set; }

        //换热器检修率
        public double? hrqjxl { get; set; }

        //压力容器定检率
        public double ?ylrqdjl { get; set; }

        //压力管道年度检验计划完成率
        public double? ylgdndjxjhwcl { get; set; }
        //安全阀年度校验计划完成率
        public double ?aqfndjyjhwcl { get; set; }
        //设备腐蚀检测计划完成率
        public double? sbfsjcjhwcl { get; set; }
        //静设备检维修一次合格率
        public double? jsbjwxychgl { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipModel.Entities
{
    public class dongmodel
    {
      
        public double? gzqdkf { get; set; }
        public double? djzgzl { get; set; }
        public double? gzwxl { get; set; }
        public double? qtlxbmfxhl { get; set; }
        public double? jjqxgsl { get; set; }
        public double? gdpjwcsj { get; set; }
        public double? jxmfpjsm { get; set; }
        public double? zcpjsm { get; set; }
        public double? sbwhl { get; set; }
        public double? jxychgl { get; set; }
        public double? zyjbpjxl { get; set; }
        public double? jzpjxl { get; set; }
        public double? wfjzgzl { get; set; }
        public double? ndbtjbcfjxtc { get; set; }
        public double? jbpjwgzjgsjMTBF { get; set; }

    }

    public class Dianmodel
    {
        //故障强度扣分
        public double? gzqdkf { get; set; }
        //电气误操作次数
        public double? dqwczcs { get; set; }

        //继电保护正确动作率
        public double? jdbhzqdzl { get; set; }
        //设备故障率 
        public double? sbgzl { get; set; }
        // 电机MTBF

        public double? djMTBF { get; set; }

    }
}
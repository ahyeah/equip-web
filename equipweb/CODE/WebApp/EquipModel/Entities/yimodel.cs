using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipModel.Entities
{
    public class yimodel  //仪
    {

        //故障强度扣分
        public double ?gzqdkf { get; set; }
        //联锁系统正确动作率
        public double ?lsxtzqdzl { get; set; }
        // 控制系统故障次数
        public double ?kzxtgzcs { get; set; }
        //仪表控制率
        public double ?ybkzl { get; set; }
        //仪表实际控制率
        public double ?ybsjkzl { get; set; }
        //联锁系统投用率
        public double ?lsxttyl { get; set; }
        //关键控制阀门故障次数
        public double ?gjkzfmgzcs { get; set; }
        //控制系统故障报警次数
        public double ?kzxtgzbjcs { get; set; }
        // 常规仪表故障率
        public double ?cgybgzl { get; set; }
        //调节阀门MTBF
        public double ?tjfMDBF { get; set; }

    }
}

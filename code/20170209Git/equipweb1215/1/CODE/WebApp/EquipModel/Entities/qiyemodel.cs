using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipModel.Entities
{
    public class qiyemodel
    {
        public double ?zzkkxzs { get; set; }
        //维修费用指数
        public double ?wxfyzs { get; set; }
        //千台离心泵（机泵）密封消耗率
        public double? qtlxbmfxhl { get; set; }
        //千台冷换设备管束（含整台）更换率
        public double? qtlhsbgsghl { get; set; }
        //仪表实际控制率
        public double? ybsjkzl { get; set; }
        //事件数
        public double? sjs { get; set; }
        //故障强度扣分
        public double? gzqdkf { get; set; }
        //项目逾期率
        public double? xmyql { get; set; }
        //培训及能力
        public double? pxjnl { get; set; }
    }
}

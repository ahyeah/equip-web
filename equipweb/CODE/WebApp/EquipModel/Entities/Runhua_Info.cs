using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipModel.Entities
{
    public class Runhua_Info
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Runhua_Id { get; set; }

        /// <summary>
        /// 生产车间名称
        /// </summary>
        public string EA_Name { get; set; }
        /// <summary>
        /// 润滑油间名称
        /// </summary>
        public string RH_Name { get; set; }
        /// <summary>
        /// 润滑油间负责的生产装置
        /// </summary>
        public string RH_ZZ_Name { get; set; }
        /// <summary>
        /// 润滑油间情况
        /// </summary>
        public string RH_Detail { get; set; }
        /// <summary>
        /// 预留字段1
        /// </summary>
        public string res1 { get; set; }
        /// <summary>
        /// 预留字段2
        /// </summary>
        public string res2 { get; set; }
        /// <summary>
        /// 预留字段3
        /// </summary>
        public string res3 { get; set; }
        public string Equip_GyCode { get; set; }
    }
}

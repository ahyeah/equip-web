using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipModel.Entities
{
    public class Equip_Info
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Equip_Id { get; set; }

        /// <summary>
        /// 工艺编号（代表名称）
        /// </summary>
        public string Equip_GyCode { get; set; }

        /// <summary>
        /// 设备编号
        /// </summary>
        public string Equip_Code { get; set; }

        /// <summary>
        /// 设备专业分类(动M 静电仪）
        /// </summary>
        public string Equip_Specialty { get; set; }

        /// <summary>
        /// 设备ABC标志
        /// </summary>
        public string Equip_ABCmark { get; set; }

        /// <summary>
        /// 设备型号
        /// </summary>
        public string Equip_Type { get; set; }

        /// <summary>
        /// 设备制造商
        /// </summary>
        public string Equip_Manufacturer { get; set; }

        /// <summary>
        /// 设备生产日期
        /// </summary>
        public string Equip_ManufactureDate { get; set; }

        /// <summary>
        /// 设备B相分类：动设备分为 - 特护机组，非特护机组，机泵，特种设备，专用设备
        /// </summary>
        public string Equip_PhaseB { get; set; }

        /// <summary>
        /// 设备主要性能参数（Json ??)
        /// </summary>
        public string Equip_PerformanceParasJson { get; set; }


        public string thRecordTable { get; set; }

        /// <summary>
        /// 所属装置
        /// </summary>
        public virtual Equip_Archi Equip_Location { get; set; }

        /// <summary>
        /// 设备管理人员
        /// </summary>
        public virtual ICollection<Person_Info> Equip_Manager { get; set; }

        public virtual ICollection<File_Info> Equip_Files { get; set; }
    }
}

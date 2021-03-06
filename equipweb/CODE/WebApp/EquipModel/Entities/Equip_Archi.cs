﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipModel.Entities
{
    public class Equip_Archi
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EA_Id { get; set; }

        /// <summary>
        /// 结构名称
        /// </summary>
        public string EA_Name { get; set; }
        public string EA_Title { get; set; }

        /// <summary>
        /// 结构代码， 可为空
        /// </summary>
        public string EA_Code { get; set; }

        /// <summary>
        /// 父节点
        /// </summary>
        public virtual Equip_Archi EA_Parent { get; set; }

        /// <summary>
        /// 子节点
        /// </summary>
        public virtual ICollection<Equip_Archi> EA_Childs { get; set; }

        /// <summary>
        /// 该节点包含的设备
        /// </summary>
        public virtual ICollection<Equip_Info> Equips_Belong { get; set; }
        public virtual ICollection<Person_Info> EA_Persons { get; set; }
    }
}

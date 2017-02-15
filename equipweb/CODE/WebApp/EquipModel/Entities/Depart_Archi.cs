using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipModel.Entities
{
    /// <summary>
    /// 组织结构
    /// </summary>
    public class Depart_Archi
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Depart_Id { get; set; }

        public string Depart_Name { get; set; }

        /// <summary>
        /// 父节点
        /// </summary>
        public virtual Depart_Archi Depart_Parent { get; set; }

        /// <summary>
        /// 子节点
        /// </summary>
        public virtual ICollection<Depart_Archi> Depart_child { get; set; }

        /// <summary>
        /// 部门职员
        /// </summary>
        public virtual ICollection<Person_Info> Depart_Persons { get; set; }
    }
}

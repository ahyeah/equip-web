using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipModel.Entities
{
    public class Speciaty_Info
    {  /// <summary>
        /// 主键
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Specialty_Id { get; set; }

        
            /// <summary>
        /// 专业名称
        /// </summary>
        public  string  Specialty_Name { get; set; }
        
       
        /// <summary>
        /// 父节点
        /// </summary>
        public virtual Speciaty_Info Speciaty_Parent { get; set; }

        /// <summary>
        /// 子节点
        /// </summary>
        public virtual ICollection<Speciaty_Info> Speciaty_Childs { get; set; }

        /// <summary>
        /// 和Person_Info的关系
        /// </summary>
        public virtual ICollection<Person_Info> Speciaty_Persons { get; set; }


    }
}

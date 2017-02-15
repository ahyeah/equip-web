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
    /// 文件分类的树形结构
    /// </summary>
    public class WorkSumCatalog
    {
        /// <summary>
        /// 文件分类ID
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Catalog_Id { get; set; }

        /// <summary>
        /// 文件分类的名称
        /// </summary>
        [Required()]
        public string Catalog_Name { get; set; }

        /// <summary>
        /// 该文件类包含的子类
        /// </summary>
        virtual public ICollection<WorkSumCatalog> Child_Catalogs { get; set; }

        /// <summary>
        /// 该文件类的父类
        /// </summary>
        virtual public WorkSumCatalog parent_Catalog { get; set; }

        /// <summary>
        /// 属于该类文件
        /// </summary>
        virtual public ICollection<WorkSummary> Files_Included { get; set; }
    }
}

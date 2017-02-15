using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipModel.Entities
{
    public class guidelines_info
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int File_Id { get; set; }

        /// <summary>
        /// 文件名（保存）
        /// </summary>
        public string File_NewName { get; set; }
        /// <summary>
        /// 文件名（用户上传时的文件名）
        /// </summary>
        public string File_OldName { get; set; }

        /// <summary>
        /// 保存路径
        /// </summary>
        public string File_SavePath { get; set; }

        /// <summary>
        /// 文件类型
        /// </summary>
        public string File_ExtType { get; set; }

        /// <summary>
        /// 上传日期
        /// </summary>
        public DateTime File_UploadTime { get; set; }

        /// <summary>
        /// 上传人
        /// </summary>
        public virtual Person_Info File_Submiter { get; set; }

        /// <summary>
        /// 工作流实体ID
        /// </summary>
        public int WfEntity_Id { get; set; }

        /// <summary>
        /// Mission ID
        /// </summary>
        public int Mission_Id { get; set; }

        /// <summary>
        /// 文件的类别
        /// </summary>
        virtual public guidelines_catalog Self_Catalog { get; set; }    
    }
}

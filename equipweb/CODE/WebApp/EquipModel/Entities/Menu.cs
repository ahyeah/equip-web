using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipModel.Entities
{
    public class Menu
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //主键
        public int Menu_Id { get; set; }

        [Required()]
        //菜单名称
        public string Menu_Name { get; set; }

        //菜单连接的URL
        public string Link_Url { get; set; }

        //菜单对应ICON
        public string Menu_Icon { get; set; }

        //该菜单项的子菜单
        public virtual ICollection<Menu> child_menus { get; set; }

        /// <summary>
        /// 具有改该菜单项操作功能的用户
        /// </summary>
       public virtual ICollection<Person_Info> Menu_Persons { get; set; }

       /// <summary>
       /// 具有某系统功能菜单的角色
       /// </summary>
       public virtual ICollection<Role_Info> Menu_Roles { get; set; }
       public virtual ICollection<ZhiduFile> ZhiduFiles { get; set; }

       public virtual ICollection<Person_Info> WorkFlow_Persons { get; set; }
    }
}

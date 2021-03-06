﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipModel.Entities
{
    public class Role_Info
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Role_Id { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        public string Role_Name { get; set; }

        /// <summary>
        /// 角色描述
        /// </summary>
        public string Role_Desc { get; set; }

        /// <summary>
        /// 角色对应的用户
        /// </summary>
        public virtual ICollection<Person_Info> Role_Persons { get; set; }

        /// <summary>
        /// 角色对应的系统功能菜单
        /// </summary>
        public virtual ICollection<Menu> Role_Menus { get; set; }
    }
}

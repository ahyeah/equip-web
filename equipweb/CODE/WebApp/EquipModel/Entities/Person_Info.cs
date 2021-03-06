﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipModel.Entities
{
    /// <summary>
    /// 职工信息
    /// </summary>
    public class Person_Info
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Person_Id { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        [Required()]
        public string Person_Name { get; set; }

        /// <summary>
        /// 用户密码 MD5
        /// </summary>
        public Byte[] Person_Pwd { get; set; }

        

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Person_mail { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public string Person_tel { get; set; }

        /// <summary>
        /// 所属部门
        /// </summary>
        public virtual Depart_Archi Dept_Belong { get; set; }

        /// <summary>
        ///所属专业种类
        /// </summary>
        public virtual ICollection<Speciaty_Info> Person_specialties { get; set; }

        /// <summary>
        /// 用户对应的角色
        /// </summary>
        public virtual ICollection<Role_Info> Person_roles { get; set; }

        /// <summary>
        /// 该人员管理的设备列表
        /// </summary>
        public virtual ICollection<Equip_Info> Person_Equips { get; set; }
        public virtual ICollection<Equip_Archi> Person_EquipEAs { get; set; }

        /// <summary>
        /// 该人员所能操作的功能菜单
        /// </summary>
        public virtual ICollection<Menu> Person_Menus { get; set; }

        /// <summary>
        /// 
        /// 用户与工作流对应关系
        /// </summary>
        public virtual ICollection<Menu> Person_WorkFlows { get; set; }
    }
}

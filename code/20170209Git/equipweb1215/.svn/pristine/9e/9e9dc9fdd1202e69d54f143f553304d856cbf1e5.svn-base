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
    /// 片区信息
    /// </summary>
    public class PqInfo
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //主键
        public int PqId { get; set; }

        [Required()]
        //片区名称
        public string PqName { get; set; }

        //片区代码
        public string PqCode { get; set; }

       // public virtual ICollection<CjInfo> Cjs;
 
    }

    //车间信息
    public class CjInfo
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //主键
        public int CjId { get; set; }

        [Required()]
        //车间名称
        public string CjName { get; set; }

        //车间代码
        public string CjCode { get; set; }

        //===所属片区
        public virtual PqInfo Pg_belong { get; set; }
    }

    //装置信息
    public class ZzInfo
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //主键
        public int ZzId { get; set; }

        [Required()]
        //装置名称
        public string ZzName { get; set; }

        //装置代码
        public string ZzCode { get; set; }

        //所属片区
        public int PqId { get; set; }

        //===所属车间
        public int CjId { get; set; }
    }

    //设备信息
    public class SbInfo
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //主键
        public int SbId { get; set; }

        //装置编号（数字字符串，唯一标识）
        public string SbCode { get; set; }

        //设备工艺编号
        public string SbGybh { get; set; }

        //设备位号
        public string SbWh { get; set; }

        //计划人员组 =ZzCode
        public string PlannerGroup { get; set; }

        //设备ABC分类
        public string SbABC { get; set; }

        //设备B相分类
        public string SbPhaseB { get; set; }

        //设备类型
        public string SbType { get; set; }

        //制造日期
        public string ManufactureDate { get; set; }

        //制造厂家
        public string Manufacturer { get; set; }

        //===所属装置
        public int ZzId { get; set; }
    }

    //角色信息
    public class RoleInfo
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //主键
        public int RoleId { get; set; }

        [Required()]
        //角色名称：装置设备员，检维修单位，机动处，XX处室，系统管理员
        public string RoleName { get; set; }

        //角色代码：对应上面给一个代码？
        public string RoleCode { get; set; }

        //角色职位：经理，主任
        public string RolePost { get; set; }

    }

    //用户信息
    public class UserInfo
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //主键
        public int UserId { get; set; }

        [Required()]
        //用户名
        public string UserName { get; set; }

        //用户密码
        public string Password { get; set; }

        //部门
        public string Department { get; set; }

        //===所拥有的权限 
        public int RoleId { get; set; }

    }

    //用户设备对应
    public class User2Sb
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //主键
        public int Id { get; set; }
        //
        public int  UserId { get; set; }        
        public int SbId { get; set; }
    }

    //用户装置对应
    public class User2Zz
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //主键
        public int Id { get; set; }
        //
        public int UserId { get; set; }
        public int ZzId { get; set; }
    }

    //角色权限对应
    public class Role2Authority
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //主键
        public int Id { get; set; }
        //
        public int RoleId { get; set; }
        public int AuthorityId { get; set; }
    }

    //用户权限对应
    public class User2Authority
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //主键
        public int Id { get; set; }
        //
        public int UserId { get; set; }
        public int AuthorityId { get; set; }
    }
}

using EquipModel.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipModel.Context
{
    public class EquipWebContext : DbContext
    {
        public EquipWebContext() : base("EquipWebContext") { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Depart_Archi>().HasMany(s => s.Depart_child).WithOptional(s => s.Depart_Parent);
            modelBuilder.Entity<Person_Info>().HasMany(s => s.Person_roles).WithMany(s => s.Role_Persons).Map( s =>
            {
                s.ToTable("Person_to_Role");
                s.MapLeftKey("Person_Id");
                s.MapRightKey("Role_Id");
            }
            );

            modelBuilder.Entity<Person_Info>().HasMany(s => s.Person_EquipEAs).WithMany(s =>s.EA_Persons).Map(s =>
            {
                s.ToTable("Person_to_EquipArchi");
                s.MapLeftKey("Person_Id");
                s.MapRightKey("EA_Id");
            }
           );
            modelBuilder.Entity<Person_Info>().HasMany(s => s.Person_Equips).WithMany(s => s.Equip_Manager).Map(s =>
            {
                s.ToTable("Person_to_Equip");
                s.MapLeftKey("Person_Id");
                s.MapRightKey("Equip_Id");
            }
            );
            modelBuilder.Entity<Equip_Archi>().HasMany(s => s.EA_Childs).WithOptional(s => s.EA_Parent);

            modelBuilder.Entity<Speciaty_Info>().HasMany(s => s.Speciaty_Childs).WithOptional(s => s.Speciaty_Parent);

            modelBuilder.Entity<Person_Info>().HasMany(s => s.Person_specialties).WithMany(s => s.Speciaty_Persons).Map(s =>
            {
                s.ToTable("Person_to_Speciaty");
                s.MapLeftKey("Person_Id");
                s.MapRightKey("Speciaty_Id");
            }
            );

            modelBuilder.Entity<Person_Info>().HasMany(s => s.Person_Menus).WithMany(s => s.Menu_Persons).Map(s =>
            {
                s.ToTable("Person_to_Menu");
                s.MapLeftKey("Person_Id");
                s.MapRightKey("Menu_Id");
            }
           );

            modelBuilder.Entity<Role_Info>().HasMany(s => s.Role_Menus).WithMany(s => s.Menu_Roles).Map(s =>
            {
                s.ToTable("Role_to_Menu");
                s.MapLeftKey("Role_Id");
                s.MapRightKey("Menu_Id");
            }


         );

            modelBuilder.Entity<File_Info>().HasMany(s => s.File_Equips).WithMany(s => s.Equip_Files).Map(s =>
            {
                s.ToTable("File_to_Equip");
                s.MapLeftKey("File_Id");
                s.MapRightKey("Equip_Id");
            }


         );
            modelBuilder.Entity<Person_Info>().HasMany(s => s.Person_WorkFlows).WithMany(s => s.WorkFlow_Persons).Map(s =>
            {
                s.ToTable("Person_to_WorkFlow");
                s.MapLeftKey("Person_Id");
                s.MapRightKey("Menu_Id");
            }


         );

            modelBuilder.Entity<guidelines_catalog>().HasMany(s => s.Child_Catalogs).WithOptional(s => s.parent_Catalog);

            modelBuilder.Entity<guidelines_info>().HasRequired(s => s.Self_Catalog).WithMany(s => s.Files_Included).WillCascadeOnDelete(true);

            modelBuilder.Entity<File_Catalog>().HasMany(s => s.Child_Catalogs).WithOptional(s => s.parent_Catalog);

            modelBuilder.Entity<File_Info>().HasRequired(s => s.Self_Catalog).WithMany(s => s.Files_Included).WillCascadeOnDelete(true);

            base.OnModelCreating(modelBuilder);
        }
        //系统菜单表
        public DbSet<Menu> Sys_Menus { get; set; }
        public DbSet<NWorkFlowSer> NWorkFlowSer { get; set; }
        //组织结构
        public DbSet<Depart_Archi> Department { get; set; }

        //职员信息
        public DbSet<Person_Info> Persons { get; set; }

        //角色信息
        public DbSet<Role_Info> Roles { get; set; }

        //设备组织结构
        public DbSet<Equip_Archi> EArchis { get; set; } 

        //设备信息
        public DbSet<Equip_Info> Equips { get; set; }
        //专业表
        public DbSet<Speciaty_Info> Specialties { get; set; }

        //文件类别表
        public DbSet<File_Catalog> FCatalogs { get; set; }
        public DbSet<File_Info> Files { get; set; }
        //方针政策表
        public DbSet<guidelines_catalog> gcatalogs { get; set; }
        public DbSet<guidelines_info> guidelines { get; set; }
        
        public DbSet<Quick_Entrance> Quick_Entrance { get; set; }
        public DbSet<A15dot1Tab> A15dot1Tab { get; set; }
        public DbSet<A5dot1Tab1> A5dot1Tab1 { get; set; }
        public DbSet<A5dot1Tab2> A5dot1Tab2 { get; set; }
        public DbSet<A5dot2Tab1> A5dot2Tab1 { get; set; }
        public DbSet<A5dot2Tab2> A5dot2Tab2 { get; set; }
        public DbSet<ZhiduFile> ZhiduFile { get; set; }

        public DbSet<A6dot2Tab1> A6dot2Tab1 { get; set; }
        public DbSet<A6dot2Tab2> A6dot2Tab2 { get; set; }

        public DbSet<A6dot2LsTaskTab> A6dot2LsTaskTab { get; set; }

        public DbSet<Runhua_Info> Runhua { get; set; }

        public DbSet<DSEventDetail> DSEventDetail { get; set; }

        public DbSet<DsTimeOfWork> DsTimeOfWork { get; set; }
        public DbSet<WorkSumCatalog> WCatalogs { get; set; }

        public DbSet<WorkSummary> WorkSumFiles { get; set; }

        public DbSet<Pq_Zz_map> Pq_Zz_maps { get; set; }
        public DbSet<Pq_EC_map> Pq_EC_maps { get; set; }
        public DbSet<A15dot1TabDian> A15dot1TabDian { get; set; }
        public DbSet<A15dot1TabJing> A15dot1TabJing { get; set; }
        public DbSet<A15dot1TabYi> A15dot1TabYi { get; set; }
        public DbSet<A15dot1TabQiYe> A15dot1TabQiYe { get; set; }
        public DbSet<A15dot1TabDong> A15dot1TabDong { get; set; }
        public DbSet<A15dot1TabDian_Weight> A15dot1TabDian_Weight { get; set; }
        public DbSet<A15dot1TabJing_Weight> A15dot1TabJing_Weight { get; set; }
        public DbSet<A15dot1TabYi_Weight> A15dot1TabYi_Weight { get; set; }
        public DbSet<A15dot1TabQiYe_Weight> A15dot1TabQiYe_Weight { get; set; }
        public DbSet<A15dot1TabDong_Weight> A15dot1TabDong_Weight { get; set; }
        public DbSet<Pq_A15dot1TabDian_Weight> Pq_A15dot1TabDian_Weight { get; set; }
        public DbSet<Pq_A15dot1TabJing_Weight> Pq_A15dot1TabJing_Weight { get; set; }
        public DbSet<Pq_A15dot1TabYi_Weight> Pq_A15dot1TabYi_Weight { get; set; }
        public DbSet<Pq_A15dot1TabQiYe_Weight> Pq_A15dot1TabQiYe_Weight { get; set; }
        public DbSet<Pq_A15dot1TabDong_Weight> Pq_A15dot1TabDong_Weight { get; set; }

        public DbSet<Notice_Info> Notice_Infos { get; set; }

        ////基础数据表
        //public DbSet<PqInfo> Sys_PqInfos { get; set; }
        //public DbSet<CjInfo> Sys_CjInfos { get; set; }
        //public DbSet<ZzInfo> Sys_ZzInfos { get; set; }
        //public DbSet<SbInfo> Sys_SbInfos { get; set; }
        //public DbSet<RoleInfo> Sys_RoleInfos { get; set; }
        //public DbSet<UserInfo> Sys_UserInfos { get; set; }

        //public DbSet<User2Sb> Sys_User2Sbs { get; set; }
        //public DbSet<User2Zz> Sys_User2Zzs { get; set; }
        //public DbSet<Role2Authority> Sys_Role2Authoritys { get; set; }
        //public DbSet<User2Authority> Sys_User2Authoritys { get; set; }
    }
}

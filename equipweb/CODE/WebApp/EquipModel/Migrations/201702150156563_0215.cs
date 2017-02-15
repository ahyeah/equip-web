namespace EquipModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _0215 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.guidelines_catalog",
                c => new
                    {
                        Catalog_Id = c.Int(nullable: false, identity: true),
                        Catalog_Name = c.String(nullable: false),
                        parent_Catalog_Catalog_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Catalog_Id)
                .ForeignKey("dbo.guidelines_catalog", t => t.parent_Catalog_Catalog_Id)
                .Index(t => t.parent_Catalog_Catalog_Id);
            
            CreateTable(
                "dbo.guidelines_info",
                c => new
                    {
                        File_Id = c.Int(nullable: false, identity: true),
                        File_NewName = c.String(),
                        File_OldName = c.String(),
                        File_SavePath = c.String(),
                        File_ExtType = c.String(),
                        File_UploadTime = c.DateTime(nullable: false),
                        WfEntity_Id = c.Int(nullable: false),
                        Mission_Id = c.Int(nullable: false),
                        File_Submiter_Person_Id = c.Int(),
                        Self_Catalog_Catalog_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.File_Id)
                .ForeignKey("dbo.Person_Info", t => t.File_Submiter_Person_Id)
                .ForeignKey("dbo.guidelines_catalog", t => t.Self_Catalog_Catalog_Id, cascadeDelete: true)
                .Index(t => t.File_Submiter_Person_Id)
                .Index(t => t.Self_Catalog_Catalog_Id);
            
            CreateTable(
                "dbo.Notice_Info",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Notice_ID = c.String(),
                        Notice_Type = c.String(),
                        Notice_Desc = c.String(),
                        Notice_Yx = c.String(),
                        Notice_EquipCode = c.String(),
                        Notice_Equip_ABCmark = c.String(),
                        Notice_OverCondition = c.String(),
                        Notice_State = c.String(),
                        Notice_Order_ID = c.String(),
                        Notice_FaultStart = c.String(),
                        Notice_FaultEnd = c.String(),
                        Notice_FaultStartTime = c.String(),
                        Notice_FaultEndTime = c.String(),
                        Notice_PlanGroup = c.String(),
                        Notice_CR_Person = c.String(),
                        Notice_CR_Date = c.String(),
                        Notice_Stop = c.String(),
                        Notice_Commit_Time = c.String(),
                        Notice_FunLoc = c.String(),
                        Notice_Catalog = c.String(),
                        Notice_Priority = c.String(),
                        Notice_Commit_Date = c.String(),
                        Notice_QMcode = c.String(),
                        Notice_FaultYx = c.String(),
                        Notice_LongTxt = c.String(),
                        Notice_EquipSpeciality = c.String(),
                        Notice_A13dot1State = c.Int(nullable: false),
                        Notice_A13dot1_DoDateTime = c.String(),
                        Notice_A13dot1_DoUserName = c.String(),
                        Notice_LoadinDateTime = c.String(),
                        Notice_Speciality = c.String(),
                        Notice_UpdateDate = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.guidelines_info", "Self_Catalog_Catalog_Id", "dbo.guidelines_catalog");
            DropForeignKey("dbo.guidelines_info", "File_Submiter_Person_Id", "dbo.Person_Info");
            DropForeignKey("dbo.guidelines_catalog", "parent_Catalog_Catalog_Id", "dbo.guidelines_catalog");
            DropIndex("dbo.guidelines_info", new[] { "Self_Catalog_Catalog_Id" });
            DropIndex("dbo.guidelines_info", new[] { "File_Submiter_Person_Id" });
            DropIndex("dbo.guidelines_catalog", new[] { "parent_Catalog_Catalog_Id" });
            DropTable("dbo.Notice_Info");
            DropTable("dbo.guidelines_info");
            DropTable("dbo.guidelines_catalog");
        }
    }
}

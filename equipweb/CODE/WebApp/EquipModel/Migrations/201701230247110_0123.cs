namespace EquipModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _0123 : DbMigration
    {
        public override void Up()
        {
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
            DropTable("dbo.Notice_Info");
        }
    }
}

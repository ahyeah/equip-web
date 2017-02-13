namespace EquipModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _0210 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NWorkFlowSers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        WF_Name = c.String(),
                        WE_Ser = c.String(),
                        time = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.NWorkFlowSers");
        }
    }
}

namespace FlowEngine.Modals.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modify1207 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WorkFlow_Entity", "Last_Trans_Time", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.WorkFlow_Entity", "Last_Trans_Time");
        }
    }
}

namespace FlowEngine.Modals.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dd : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CURR_Mission",
                c => new
                    {
                        Miss_Id = c.Int(nullable: false, identity: true),
                        Miss_Name = c.String(),
                        Miss_Desc = c.String(),
                        Before_Action = c.String(),
                        Current_Action = c.String(),
                        After_Action = c.String(),
                        Str_Authority = c.String(),
                        WFE_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Miss_Id)
                .ForeignKey("dbo.WorkFlow_Entity", t => t.WFE_ID, cascadeDelete: true)
                .Index(t => t.WFE_ID);
            
            CreateTable(
                "dbo.WorkFlow_Entity",
                c => new
                    {
                        WE_Id = c.Int(nullable: false, identity: true),
                        WE_Ser = c.String(),
                        WE_Status = c.Int(nullable: false),
                        WE_Binary = c.Binary(),
                        Last_Trans_Time = c.DateTime(),
                        WE_Wref_W_ID = c.Int(),
                    })
                .PrimaryKey(t => t.WE_Id)
                .ForeignKey("dbo.WorkFlow_Define", t => t.WE_Wref_W_ID)
                .Index(t => t.WE_Wref_W_ID);
            
            CreateTable(
                "dbo.Missions",
                c => new
                    {
                        Miss_Id = c.Int(nullable: false, identity: true),
                        Event_Name = c.String(),
                        Miss_Name = c.String(),
                        Miss_Desc = c.String(),
                        Miss_WFentity_WE_Id = c.Int(),
                        pre_Mission_Miss_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Miss_Id)
                .ForeignKey("dbo.WorkFlow_Entity", t => t.Miss_WFentity_WE_Id)
                .ForeignKey("dbo.Missions", t => t.pre_Mission_Miss_Id)
                .Index(t => t.Miss_WFentity_WE_Id)
                .Index(t => t.pre_Mission_Miss_Id);
            
            CreateTable(
                "dbo.Mission_Param",
                c => new
                    {
                        Param_Id = c.Int(nullable: false, identity: true),
                        Param_Name = c.String(),
                        Param_Value = c.String(),
                        Param_Type = c.String(),
                        Param_Desc = c.String(),
                        Miss_Belong_Miss_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Param_Id)
                .ForeignKey("dbo.Missions", t => t.Miss_Belong_Miss_Id)
                .Index(t => t.Miss_Belong_Miss_Id);
            
            CreateTable(
                "dbo.Process_Record",
                c => new
                    {
                        Re_Id = c.Int(nullable: false, identity: true),
                        Re_Name = c.String(),
                        Re_Value = c.String(),
                        Re_Mission_Miss_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Re_Id)
                .ForeignKey("dbo.Missions", t => t.Re_Mission_Miss_Id)
                .Index(t => t.Re_Mission_Miss_Id);
            
            CreateTable(
                "dbo.WorkFlow_Define",
                c => new
                    {
                        W_ID = c.Int(nullable: false, identity: true),
                        W_Attribution = c.String(),
                        W_Name = c.String(),
                        W_Xml = c.Binary(),
                    })
                .PrimaryKey(t => t.W_ID);
            
            CreateTable(
                "dbo.Timer_Jobs",
                c => new
                    {
                        JOB_ID = c.Int(nullable: false, identity: true),
                        job_name = c.String(),
                        Job_Type = c.Int(nullable: false),
                        PreTime = c.DateTime(),
                        status = c.Int(nullable: false),
                        t_using = c.Int(nullable: false),
                        workflow_ID = c.Int(nullable: false),
                        run_params = c.String(),
                        run_result = c.String(),
                        corn_express = c.String(),
                        create_time = c.DateTime(nullable: false),
                        custom_action = c.String(),
                        custom_flag = c.Int(nullable: false),
                        INT_RES_1 = c.Int(nullable: false),
                        INT_RES_2 = c.Int(nullable: false),
                        INT_RES_3 = c.Int(nullable: false),
                        STR_RES_1 = c.String(),
                        STR_RES_2 = c.String(),
                        STR_RES_3 = c.String(),
                    })
                .PrimaryKey(t => t.JOB_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CURR_Mission", "WFE_ID", "dbo.WorkFlow_Entity");
            DropForeignKey("dbo.WorkFlow_Entity", "WE_Wref_W_ID", "dbo.WorkFlow_Define");
            DropForeignKey("dbo.Process_Record", "Re_Mission_Miss_Id", "dbo.Missions");
            DropForeignKey("dbo.Mission_Param", "Miss_Belong_Miss_Id", "dbo.Missions");
            DropForeignKey("dbo.Missions", "pre_Mission_Miss_Id", "dbo.Missions");
            DropForeignKey("dbo.Missions", "Miss_WFentity_WE_Id", "dbo.WorkFlow_Entity");
            DropIndex("dbo.Process_Record", new[] { "Re_Mission_Miss_Id" });
            DropIndex("dbo.Mission_Param", new[] { "Miss_Belong_Miss_Id" });
            DropIndex("dbo.Missions", new[] { "pre_Mission_Miss_Id" });
            DropIndex("dbo.Missions", new[] { "Miss_WFentity_WE_Id" });
            DropIndex("dbo.WorkFlow_Entity", new[] { "WE_Wref_W_ID" });
            DropIndex("dbo.CURR_Mission", new[] { "WFE_ID" });
            DropTable("dbo.Timer_Jobs");
            DropTable("dbo.WorkFlow_Define");
            DropTable("dbo.Process_Record");
            DropTable("dbo.Mission_Param");
            DropTable("dbo.Missions");
            DropTable("dbo.WorkFlow_Entity");
            DropTable("dbo.CURR_Mission");
        }
    }
}

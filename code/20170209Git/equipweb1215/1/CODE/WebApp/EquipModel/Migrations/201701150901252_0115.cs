namespace EquipModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _0115 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.A15dot1TabDian_Weight",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        gzqdkf_weight = c.Double(nullable: false),
                        dqwczcs_weight = c.Double(nullable: false),
                        jdbhzqdzl_weight = c.Double(nullable: false),
                        sbgzl_weight = c.Double(nullable: false),
                        djMTBF_weight = c.Double(nullable: false),
                        dzdlsbMTBF_weight = c.Double(nullable: false),
                        zbglys_weight = c.Double(nullable: false),
                        dnjlphl_weight = c.Double(nullable: false),
                        Pq_Name = c.String(),
                        Zz_Code = c.String(),
                        Zz_Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.A15dot1TabDong",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        gzqdkf = c.Int(nullable: false),
                        djzgzl = c.Double(nullable: false),
                        gzwxl = c.Double(nullable: false),
                        qtlxbmfxhl = c.Double(nullable: false),
                        jjqxgsl = c.Double(nullable: false),
                        gdpjwcsj = c.Double(nullable: false),
                        jxmfpjsm = c.Double(nullable: false),
                        zcpjsm = c.Double(nullable: false),
                        sbwhl = c.Double(nullable: false),
                        jxychgl = c.Double(nullable: false),
                        zyjbpjxl = c.Double(nullable: false),
                        jzpjxl = c.Double(nullable: false),
                        wfjzgzl = c.Double(nullable: false),
                        ndbtjbcfjxtc = c.Double(nullable: false),
                        jbpjwgzjgsjMTBF = c.Double(nullable: false),
                        hiddenDangerInvestigation = c.String(),
                        rateLoad = c.String(),
                        gyChange = c.String(),
                        equipChange = c.String(),
                        otherDescription = c.String(),
                        evaluateEquipRunStaeDesc = c.String(),
                        evaluateEquipRunStaeImgPath = c.String(),
                        reliabilityConclusion = c.String(),
                        jdcAdviseImproveMeasures = c.String(),
                        submitDepartment = c.String(),
                        submitUser = c.String(),
                        submitTime = c.DateTime(),
                        jdcOperator = c.String(),
                        jdcOperateTime = c.DateTime(),
                        state = c.Int(nullable: false),
                        reportType = c.String(),
                        temp1 = c.String(),
                        temp2 = c.String(),
                        temp3 = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.A15dot1TabDong_Weight",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        gzqdkf_weight = c.Double(nullable: false),
                        djzgzl_weight = c.Double(nullable: false),
                        gzwxl_weight = c.Double(nullable: false),
                        qtlxbmfxhl_weight = c.Double(nullable: false),
                        jjqxgsl_weight = c.Double(nullable: false),
                        gdpjwcsj_weight = c.Double(nullable: false),
                        jxmfpjsm_weight = c.Double(nullable: false),
                        zcpjsm_weight = c.Double(nullable: false),
                        sbwhl_weight = c.Double(nullable: false),
                        jxychgl_weight = c.Double(nullable: false),
                        zyjbpjxl_weight = c.Double(nullable: false),
                        jzpjxl_weight = c.Double(nullable: false),
                        wfjzgzl_weight = c.Double(nullable: false),
                        ndbtjbcfjxtc_weight = c.Double(nullable: false),
                        jbpjwgzjgsjMTBF_weight = c.Double(nullable: false),
                        Pq_Name = c.String(),
                        Zz_Code = c.String(),
                        Zz_Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.A15dot1TabJing_Weight",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        gzqdkf_weight = c.Double(nullable: false),
                        sbfsxlcs_weight = c.Double(nullable: false),
                        qtlhsbgs_weight = c.Double(nullable: false),
                        gylpjrxl_weight = c.Double(nullable: false),
                        hrqjxl_weight = c.Double(nullable: false),
                        ylrqdjl_weight = c.Double(nullable: false),
                        ylgdndjxjhwcl_weight = c.Double(nullable: false),
                        aqfndjyjhwcl_weight = c.Double(nullable: false),
                        sbfsjcjhwcl_weight = c.Double(nullable: false),
                        jsbjwxychgl_weight = c.Double(nullable: false),
                        Pq_Name = c.String(),
                        Zz_Code = c.String(),
                        Zz_Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.A15dot1TabQiYe_Weight",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        zzkkxzs_weight = c.Double(nullable: false),
                        wxfyzs_weight = c.Double(nullable: false),
                        qtlxbmfxhl_weight = c.Double(nullable: false),
                        qtlhsbgsghl_weight = c.Double(nullable: false),
                        ybsjkzl_weight = c.Double(nullable: false),
                        sjs_weight = c.Double(nullable: false),
                        gzqdkf_weight = c.Double(nullable: false),
                        xmyql_weight = c.Double(nullable: false),
                        pxjnl_weight = c.Double(nullable: false),
                        Pq_Name = c.String(),
                        Zz_Code = c.String(),
                        Zz_Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.A15dot1TabYi_Weight",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        gzqdkf_weight = c.Double(nullable: false),
                        lsxtzqdzl_weight = c.Double(nullable: false),
                        kzxtgzcs_weight = c.Double(nullable: false),
                        ybkzl_weight = c.Double(nullable: false),
                        ybsjkzl_weight = c.Double(nullable: false),
                        lsxttyl_weight = c.Double(nullable: false),
                        gjkzfmgzcs_weight = c.Double(nullable: false),
                        kzxtgzbjcs_weight = c.Double(nullable: false),
                        cgybgzl_weight = c.Double(nullable: false),
                        tjfMDBF_weight = c.Double(nullable: false),
                        Pq_Name = c.String(),
                        Zz_Code = c.String(),
                        Zz_Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Pq_A15dot1TabDian_Weight",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        gzqdkf_weight = c.Double(nullable: false),
                        dqwczcs_weight = c.Double(nullable: false),
                        jdbhzqdzl_weight = c.Double(nullable: false),
                        sbgzl_weight = c.Double(nullable: false),
                        djMTBF_weight = c.Double(nullable: false),
                        dzdlsbMTBF_weight = c.Double(nullable: false),
                        zbglys_weight = c.Double(nullable: false),
                        dnjlphl_weight = c.Double(nullable: false),
                        Pq_Name = c.String(),
                        Zz_Code = c.String(),
                        Zz_Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Pq_A15dot1TabDong_Weight",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        gzqdkf_weight = c.Double(nullable: false),
                        djzgzl_weight = c.Double(nullable: false),
                        gzwxl_weight = c.Double(nullable: false),
                        qtlxbmfxhl_weight = c.Double(nullable: false),
                        jjqxgsl_weight = c.Double(nullable: false),
                        gdpjwcsj_weight = c.Double(nullable: false),
                        jxmfpjsm_weight = c.Double(nullable: false),
                        zcpjsm_weight = c.Double(nullable: false),
                        sbwhl_weight = c.Double(nullable: false),
                        jxychgl_weight = c.Double(nullable: false),
                        zyjbpjxl_weight = c.Double(nullable: false),
                        jzpjxl_weight = c.Double(nullable: false),
                        wfjzgzl_weight = c.Double(nullable: false),
                        ndbtjbcfjxtc_weight = c.Double(nullable: false),
                        jbpjwgzjgsjMTBF_weight = c.Double(nullable: false),
                        Pq_Name = c.String(),
                        Zz_Code = c.String(),
                        Zz_Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Pq_A15dot1TabJing_Weight",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        gzqdkf_weight = c.Double(nullable: false),
                        sbfsxlcs_weight = c.Double(nullable: false),
                        qtlhsbgs_weight = c.Double(nullable: false),
                        gylpjrxl_weight = c.Double(nullable: false),
                        hrqjxl_weight = c.Double(nullable: false),
                        ylrqdjl_weight = c.Double(nullable: false),
                        ylgdndjxjhwcl_weight = c.Double(nullable: false),
                        aqfndjyjhwcl_weight = c.Double(nullable: false),
                        sbfsjcjhwcl_weight = c.Double(nullable: false),
                        jsbjwxychgl_weight = c.Double(nullable: false),
                        Pq_Name = c.String(),
                        Zz_Code = c.String(),
                        Zz_Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Pq_A15dot1TabQiYe_Weight",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        zzkkxzs_weight = c.Double(nullable: false),
                        wxfyzs_weight = c.Double(nullable: false),
                        qtlxbmfxhl_weight = c.Double(nullable: false),
                        qtlhsbgsghl_weight = c.Double(nullable: false),
                        ybsjkzl_weight = c.Double(nullable: false),
                        sjs_weight = c.Double(nullable: false),
                        gzqdkf_weight = c.Double(nullable: false),
                        xmyql_weight = c.Double(nullable: false),
                        pxjnl_weight = c.Double(nullable: false),
                        Pq_Name = c.String(),
                        Zz_Code = c.String(),
                        Zz_Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Pq_A15dot1TabYi_Weight",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        gzqdkf_weight = c.Double(nullable: false),
                        lsxtzqdzl_weight = c.Double(nullable: false),
                        kzxtgzcs_weight = c.Double(nullable: false),
                        ybkzl_weight = c.Double(nullable: false),
                        ybsjkzl_weight = c.Double(nullable: false),
                        lsxttyl_weight = c.Double(nullable: false),
                        gjkzfmgzcs_weight = c.Double(nullable: false),
                        kzxtgzbjcs_weight = c.Double(nullable: false),
                        cgybgzl_weight = c.Double(nullable: false),
                        tjfMDBF_weight = c.Double(nullable: false),
                        Pq_Name = c.String(),
                        Zz_Code = c.String(),
                        Zz_Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            DropColumn("dbo.A15dot1Tab", "qtlxbmfxhl");
            DropColumn("dbo.A15dot1Tab", "wfjzgzl");
            DropColumn("dbo.A15dot1Tab", "ndbtjbcfjxtc");
        }
        
        public override void Down()
        {
            AddColumn("dbo.A15dot1Tab", "ndbtjbcfjxtc", c => c.Int(nullable: false));
            AddColumn("dbo.A15dot1Tab", "wfjzgzl", c => c.Double(nullable: false));
            AddColumn("dbo.A15dot1Tab", "qtlxbmfxhl", c => c.Double(nullable: false));
            DropTable("dbo.Pq_A15dot1TabYi_Weight");
            DropTable("dbo.Pq_A15dot1TabQiYe_Weight");
            DropTable("dbo.Pq_A15dot1TabJing_Weight");
            DropTable("dbo.Pq_A15dot1TabDong_Weight");
            DropTable("dbo.Pq_A15dot1TabDian_Weight");
            DropTable("dbo.A15dot1TabYi_Weight");
            DropTable("dbo.A15dot1TabQiYe_Weight");
            DropTable("dbo.A15dot1TabJing_Weight");
            DropTable("dbo.A15dot1TabDong_Weight");
            DropTable("dbo.A15dot1TabDong");
            DropTable("dbo.A15dot1TabDian_Weight");
        }
    }
}

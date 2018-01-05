namespace TLO_KQGL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatePrivilege : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Sys_Application",
                c => new
                    {
                        ApplicationId = c.Int(nullable: false, identity: true),
                        ApplicationCode = c.String(maxLength: 20),
                        ApplicationName = c.String(maxLength: 20),
                        ApplicationDesc = c.String(maxLength: 50),
                        ShowInMenu = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ApplicationId);
            
            CreateTable(
                "dbo.Sys_Button",
                c => new
                    {
                        BtnId = c.Int(nullable: false, identity: true),
                        BtnNo = c.String(maxLength: 5),
                        BtnClass = c.String(maxLength: 10),
                        BtnScript = c.String(maxLength: 20),
                        BtnName = c.String(maxLength: 20),
                        MenuNo = c.String(maxLength: 5),
                        BtnIcon = c.String(maxLength: 10),
                        InitStatus = c.String(maxLength: 5),
                        seqNo = c.String(maxLength: 5),
                    })
                .PrimaryKey(t => t.BtnId);
            
            CreateTable(
                "dbo.Sys_Menu",
                c => new
                    {
                        MenuId = c.Int(nullable: false, identity: true),
                        MenuNo = c.String(maxLength: 5),
                        ApplicationCode = c.String(maxLength: 20),
                        MenuParentNo = c.String(maxLength: 5),
                        MenuOrder = c.String(maxLength: 5),
                        MenuName = c.String(maxLength: 20),
                        MenuUrl = c.String(maxLength: 50),
                        MenuIcon = c.String(maxLength: 10),
                        IsVisible = c.Boolean(nullable: false),
                        IsLeaf = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.MenuId);
            
            CreateTable(
                "dbo.Privileges",
                c => new
                    {
                        PrivilegeId = c.Int(nullable: false, identity: true),
                        PrivilegeMaster = c.String(maxLength: 10),
                        PrivilegeMasterValue = c.String(maxLength: 10),
                        PrivilegeMasterAccess = c.String(maxLength: 10),
                        PrivilegeMasterAccessValue = c.String(maxLength: 10),
                        PrivilegeOperation = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.PrivilegeId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Privileges");
            DropTable("dbo.Sys_Menu");
            DropTable("dbo.Sys_Button");
            DropTable("dbo.Sys_Application");
        }
    }
}

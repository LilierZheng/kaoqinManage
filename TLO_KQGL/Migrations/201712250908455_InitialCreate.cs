namespace TLO_KQGL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Employees",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        Emp_No = c.String(),
                        DeptId = c.Guid(nullable: false),
                        Emp_Name = c.String(),
                        Emp_Sex = c.Boolean(nullable: false),
                        Address = c.String(),
                        NativePlace = c.String(),
                        Age = c.Int(nullable: false),
                        BirthDay = c.String(),
                        Job = c.String(),
                        CreateUser = c.String(),
                        CreateDate = c.DateTime(nullable: false),
                        LastUpdateUser = c.String(),
                        LastUpdateDate = c.DateTime(nullable: false),
                        Dep_ID = c.Guid(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Departments", t => t.Dep_ID)
                .Index(t => t.Dep_ID);
            
            CreateTable(
                "dbo.Departments",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        DeptNo = c.String(),
                        DeptName = c.String(),
                        CreateUser = c.String(),
                        CreateDate = c.String(),
                        LastUpdateUser = c.String(),
                        LasrUpdateDate = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Dept_Class",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Dept_Id = c.Guid(nullable: false),
                        Class_Id = c.Int(nullable: false),
                        classType_ID = c.Guid(),
                        dep_ID = c.Guid(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ClassTypes", t => t.classType_ID)
                .ForeignKey("dbo.Departments", t => t.dep_ID)
                .Index(t => t.classType_ID)
                .Index(t => t.dep_ID);
            
            CreateTable(
                "dbo.ClassTypes",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        DeptID = c.Guid(nullable: false),
                        OnWorkTime = c.DateTime(nullable: false),
                        OffWorkTime = c.DateTime(nullable: false),
                        CreateUser = c.String(),
                        CreateDate = c.String(),
                        LastUpdateUser = c.String(),
                        LasrUpdateDate = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Dept_Class", new[] { "dep_ID" });
            DropIndex("dbo.Dept_Class", new[] { "classType_ID" });
            DropIndex("dbo.Employees", new[] { "Dep_ID" });
            DropForeignKey("dbo.Dept_Class", "dep_ID", "dbo.Departments");
            DropForeignKey("dbo.Dept_Class", "classType_ID", "dbo.ClassTypes");
            DropForeignKey("dbo.Employees", "Dep_ID", "dbo.Departments");
            DropTable("dbo.ClassTypes");
            DropTable("dbo.Dept_Class");
            DropTable("dbo.Departments");
            DropTable("dbo.Employees");
        }
    }
}

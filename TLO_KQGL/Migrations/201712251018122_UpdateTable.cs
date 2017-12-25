namespace TLO_KQGL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Attendances",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        SignOn = c.DateTime(nullable: false),
                        SignOff = c.DateTime(nullable: false),
                        Late = c.Boolean(nullable: false),
                        LeaveEary = c.Boolean(nullable: false),
                        Emp_ID = c.Guid(),
                        claType_ID = c.Guid(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Employees", t => t.Emp_ID)
                .ForeignKey("dbo.ClassTypes", t => t.claType_ID)
                .Index(t => t.Emp_ID)
                .Index(t => t.claType_ID);
            
            AddColumn("dbo.Departments", "LastUpdateDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.ClassTypes", "LastUpdateDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Departments", "DeptNo", c => c.String(nullable: false, maxLength: 5));
            AlterColumn("dbo.Departments", "DeptName", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.Departments", "CreateUser", c => c.String(maxLength: 10));
            AlterColumn("dbo.Departments", "CreateDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Departments", "LastUpdateUser", c => c.String(maxLength: 10));
            AlterColumn("dbo.ClassTypes", "CreateUser", c => c.String(maxLength: 10));
            AlterColumn("dbo.ClassTypes", "CreateDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.ClassTypes", "LastUpdateUser", c => c.String(maxLength: 10));
            DropColumn("dbo.Employees", "DeptId");
            DropColumn("dbo.Employees", "LastUpdateUser");
            DropColumn("dbo.Departments", "LasrUpdateDate");
            DropColumn("dbo.Dept_Class", "Dept_Id");
            DropColumn("dbo.Dept_Class", "Class_Id");
            DropColumn("dbo.ClassTypes", "LasrUpdateDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ClassTypes", "LasrUpdateDate", c => c.String());
            AddColumn("dbo.Dept_Class", "Class_Id", c => c.Int(nullable: false));
            AddColumn("dbo.Dept_Class", "Dept_Id", c => c.Guid(nullable: false));
            AddColumn("dbo.Departments", "LasrUpdateDate", c => c.String());
            AddColumn("dbo.Employees", "LastUpdateUser", c => c.String());
            AddColumn("dbo.Employees", "DeptId", c => c.Guid(nullable: false));
            DropIndex("dbo.Attendances", new[] { "claType_ID" });
            DropIndex("dbo.Attendances", new[] { "Emp_ID" });
            DropForeignKey("dbo.Attendances", "claType_ID", "dbo.ClassTypes");
            DropForeignKey("dbo.Attendances", "Emp_ID", "dbo.Employees");
            AlterColumn("dbo.ClassTypes", "LastUpdateUser", c => c.String());
            AlterColumn("dbo.ClassTypes", "CreateDate", c => c.String());
            AlterColumn("dbo.ClassTypes", "CreateUser", c => c.String());
            AlterColumn("dbo.Departments", "LastUpdateUser", c => c.String());
            AlterColumn("dbo.Departments", "CreateDate", c => c.String());
            AlterColumn("dbo.Departments", "CreateUser", c => c.String());
            AlterColumn("dbo.Departments", "DeptName", c => c.String());
            AlterColumn("dbo.Departments", "DeptNo", c => c.String());
            DropColumn("dbo.ClassTypes", "LastUpdateDate");
            DropColumn("dbo.Departments", "LastUpdateDate");
            DropTable("dbo.Attendances");
        }
    }
}

namespace TLO_KQGL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTable1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Employees", "Emp_No", c => c.String(nullable: false, maxLength: 5));
            AlterColumn("dbo.Employees", "Emp_Name", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.Employees", "Address", c => c.String(maxLength: 50));
            AlterColumn("dbo.Employees", "NativePlace", c => c.String(maxLength: 20));
            AlterColumn("dbo.Employees", "BirthDay", c => c.String(maxLength: 20));
            AlterColumn("dbo.Employees", "Job", c => c.String(maxLength: 20));
            AlterColumn("dbo.Employees", "CreateUser", c => c.String(maxLength: 10));
            AlterColumn("dbo.Employees", "CreateDate", c => c.DateTime());
            AlterColumn("dbo.Employees", "LastUpdateDate", c => c.DateTime());
            AlterColumn("dbo.Departments", "CreateDate", c => c.DateTime());
            AlterColumn("dbo.Departments", "LastUpdateDate", c => c.DateTime());
            AlterColumn("dbo.ClassTypes", "CreateDate", c => c.DateTime());
            AlterColumn("dbo.ClassTypes", "LastUpdateDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ClassTypes", "LastUpdateDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.ClassTypes", "CreateDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Departments", "LastUpdateDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Departments", "CreateDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Employees", "LastUpdateDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Employees", "CreateDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Employees", "CreateUser", c => c.String());
            AlterColumn("dbo.Employees", "Job", c => c.String());
            AlterColumn("dbo.Employees", "BirthDay", c => c.String());
            AlterColumn("dbo.Employees", "NativePlace", c => c.String());
            AlterColumn("dbo.Employees", "Address", c => c.String());
            AlterColumn("dbo.Employees", "Emp_Name", c => c.String());
            AlterColumn("dbo.Employees", "Emp_No", c => c.String());
        }
    }
}

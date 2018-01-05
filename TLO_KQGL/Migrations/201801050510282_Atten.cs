namespace TLO_KQGL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class Atten : DbMigration
    {
        public override void Up()
        {
            CreateTable( "dbo.Attendances",
       c => new
       {
           ID = c.Guid(nullable: false),
           SignOn = c.DateTime(nullable: false),
           SignOff = c.DateTime(nullable: false),
           Late = c.Boolean(nullable: false),
           LeaveEary = c.Boolean(nullable: false),
           IsCheck=c.Boolean(nullable:false),
           Emp_ID = c.Guid(),
           claType_ID = c.Guid(),
       })
       .PrimaryKey(t => t.ID)
       .ForeignKey("dbo.Employees", t => t.Emp_ID)
       .ForeignKey("dbo.ClassTypes", t => t.claType_ID)
       .Index(t => t.Emp_ID)
       .Index(t => t.claType_ID);
        }

        public override void Down()
        {
            DropIndex("dbo.Attendances", new[] { "claType_ID" });
            DropIndex("dbo.Attendances", new[] { "Emp_ID" });
            DropForeignKey("dbo.Attendances", "claType_ID", "dbo.ClassTypes");
            DropForeignKey("dbo.Attendances", "Emp_ID", "dbo.Employees");
            DropTable("dbo.Attendances");
        }
    }
}

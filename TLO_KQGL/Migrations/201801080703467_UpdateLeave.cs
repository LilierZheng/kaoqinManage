namespace TLO_KQGL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateLeave : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Leaves",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        Title = c.String(maxLength: 20),
                        Content = c.String(maxLength: 300),
                        LeaveBeginDate = c.DateTime(),
                        LeaveEndDate = c.DateTime(),
                        IsCheck = c.Boolean(nullable: false),
                        CreateUser = c.String(maxLength: 10),
                        CreateDate = c.DateTime(),
                        LastUpdateUser = c.String(maxLength: 10),
                        LastUpdateDate = c.DateTime(),
                        emp_ID = c.Guid(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Employees", t => t.emp_ID)
                .Index(t => t.emp_ID);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Leaves", new[] { "emp_ID" });
            DropForeignKey("dbo.Leaves", "emp_ID", "dbo.Employees");
            DropTable("dbo.Leaves");
        }
    }
}

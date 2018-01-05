namespace TLO_KQGL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update11 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Employees", "Tel", c => c.String(maxLength: 11));
            AddColumn("dbo.Departments", "LastUpdateDate", c => c.DateTime());
            AddColumn("dbo.Departments","deptDesc",c=>c.String(maxLength:50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Employees", "Tel");
            DropColumn("dbo.Departments", "LastUpdateDate");
            DropColumn("dbo.Departments", "deptDesc");
        }
    }
}

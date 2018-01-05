namespace TLO_KQGL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class alterEmp : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Employees", "DeptId");
        }
        
        public override void Down()
        {
            DropColumn("dbo.Employees", "DeptId");
        }
    }
}

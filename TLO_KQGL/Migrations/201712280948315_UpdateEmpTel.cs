namespace TLO_KQGL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateEmpTel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Employees", "Tel", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Employees", "Tel");
        }
    }
}

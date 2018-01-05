namespace TLO_KQGL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class lonti : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Departments", "lontitude", c => c.Decimal(nullable: false, precision: 10, scale: 7));
            AddColumn("dbo.Departments", "latitude", c => c.Decimal(nullable: false, precision: 10, scale: 7));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Departments", "latitude");
            DropColumn("dbo.Departments", "lontitude");
        }
    }
}

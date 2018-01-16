namespace TLO_KQGL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColRestforAtten : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Attendances", "isRest", c => c.Boolean(nullable: false));
            AddColumn("dbo.Attendances", "WorkOverTime", c => c.Decimal(nullable: false, precision: 5, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Attendances", "WorkOverTime");
            DropColumn("dbo.Attendances", "isRest");
        }
    }
}

namespace TLO_KQGL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateAtten : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Attendances", "IsLeave", c => c.Boolean(nullable: false));
            AddColumn("dbo.Attendances", "LeaveDay", c => c.Decimal(nullable: false, precision: 4, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Attendances", "LeaveDay");
            DropColumn("dbo.Attendances", "IsLeave");
        }
    }
}

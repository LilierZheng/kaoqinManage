namespace TLO_KQGL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateAttenLeaveHours : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Attendances", "LeaveHours", c => c.Decimal(nullable: false, precision: 4, scale: 2));
            DropColumn("dbo.Attendances", "LeaveDay");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Attendances", "LeaveDay", c => c.Decimal(nullable: false, precision: 4, scale: 2));
            DropColumn("dbo.Attendances", "LeaveHours");
        }
    }
}

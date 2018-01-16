namespace TLO_KQGL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateLeaveHours : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ClassTypes", "BeginSleepTime", c => c.String(nullable: false, maxLength: 4));
            AddColumn("dbo.ClassTypes", "EndSleepTime", c => c.String(nullable: false, maxLength: 4));
            AddColumn("dbo.ClassTypes", "WorkEtraTime", c => c.String(nullable: false, maxLength: 4));
            AddColumn("dbo.Leaves", "Hours", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.Leaves", "Days");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Leaves", "Days", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.Leaves", "Hours");
            DropColumn("dbo.ClassTypes", "WorkEtraTime");
            DropColumn("dbo.ClassTypes", "EndSleepTime");
            DropColumn("dbo.ClassTypes", "BeginSleepTime");
        }
    }
}

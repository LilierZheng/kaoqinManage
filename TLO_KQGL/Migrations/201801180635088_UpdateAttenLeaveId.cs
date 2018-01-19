namespace TLO_KQGL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateAttenLeaveId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Attendances", "LeaveId", c => c.String(maxLength: 36));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Attendances", "LeaveId");
        }
    }
}

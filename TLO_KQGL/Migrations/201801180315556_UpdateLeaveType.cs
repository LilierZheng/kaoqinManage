namespace TLO_KQGL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateLeaveType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Leaves", "LeaveType", c => c.String(maxLength: 6));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Leaves", "LeaveType");
        }
    }
}

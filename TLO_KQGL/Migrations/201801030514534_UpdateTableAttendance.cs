namespace TLO_KQGL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTableAttendance : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Attendances", "IsCheck", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Attendances", "IsCheck");
        }
    }
}

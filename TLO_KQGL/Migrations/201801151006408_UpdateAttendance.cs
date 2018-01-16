namespace TLO_KQGL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateAttendance : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Attendances", "SignOn", c => c.DateTime());
            AlterColumn("dbo.Attendances", "SignOff", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Attendances", "SignOff", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Attendances", "SignOn", c => c.DateTime(nullable: false));
        }
    }
}

namespace TLO_KQGL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateAttenNo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Attendances", "AttenNo", c => c.String(maxLength: 8));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Attendances", "AttenNo");
        }
    }
}

namespace TLO_KQGL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateAttenNoLen : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Attendances", "AttenNo", c => c.String(maxLength: 8));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Attendances", "AttenNo", c => c.String(maxLength: 6));
        }
    }
}

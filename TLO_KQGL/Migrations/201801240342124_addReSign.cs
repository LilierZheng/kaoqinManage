namespace TLO_KQGL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addReSign : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Attendances", "ReSign", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Attendances", "ReSign");
        }
    }
}

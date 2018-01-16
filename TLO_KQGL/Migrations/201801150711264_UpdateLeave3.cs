namespace TLO_KQGL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateLeave3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Leaves", "IsPass", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Leaves", "IsPass");
        }
    }
}

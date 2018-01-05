namespace TLO_KQGL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ALTER : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ClassTypes", "OnWorkTime", c => c.String(nullable: false, maxLength: 4));
            AlterColumn("dbo.ClassTypes", "OffWorkTime", c => c.String(nullable: false, maxLength: 4));

        }
        
        public override void Down()
        {
            DropColumn("dbo.ClassTypes", "OnWorkTime");
            DropColumn("dbo.ClassTypes", "OffWorkTime");

        }
    }
}

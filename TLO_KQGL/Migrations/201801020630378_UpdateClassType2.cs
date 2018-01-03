namespace TLO_KQGL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateClassType2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ClassTypes", "onWorkTime", c => c.String(nullable: false, maxLength: 4));
            AlterColumn("dbo.ClassTypes", "offWorkTime", c => c.String(nullable: false, maxLength: 4));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ClassTypes", "OffWorkTime", c => c.String());
            AlterColumn("dbo.ClassTypes", "OnWorkTime", c => c.String());
        }
    }
}

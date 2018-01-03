namespace TLO_KQGL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateClassType : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ClassTypes", "OnWorkTime", c => c.String());
            AlterColumn("dbo.ClassTypes", "OffWorkTime", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ClassTypes", "OffWorkTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.ClassTypes", "OnWorkTime", c => c.DateTime(nullable: false));
        }
    }
}

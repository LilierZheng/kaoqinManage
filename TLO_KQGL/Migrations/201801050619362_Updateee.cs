namespace TLO_KQGL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Updateee : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.ClassTypes", "LasrUpdateDate");

            AddColumn("dbo.ClassTypes", "LasrUpdateDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ClassTypes", "LasrUpdateDate");
        }
    }
}

namespace TLO_KQGL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Updatedep : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Departments", "LasrUpdateDate");
        }
        
        public override void Down()
        {
            DropColumn("dbo.Departments", "LasrUpdateDate");
        }
    }
}

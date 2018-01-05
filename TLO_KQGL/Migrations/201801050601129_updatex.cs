namespace TLO_KQGL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatex : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ClassTypes", "DeptID", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ClassTypes", "DeptID", c => c.Guid(nullable: false));
        }
    }
}

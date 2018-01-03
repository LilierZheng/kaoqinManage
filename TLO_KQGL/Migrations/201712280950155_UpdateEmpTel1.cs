namespace TLO_KQGL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateEmpTel1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Employees", "Tel", c => c.String(nullable: false, maxLength: 11));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Employees", "Tel", c => c.String(nullable: false));
        }
    }
}

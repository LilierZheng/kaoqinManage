namespace TLO_KQGL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateEmplyee : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Employees", "LastUpdateUser", c => c.String(maxLength: 10));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Employees", "LastUpdateUser");
        }
    }
}

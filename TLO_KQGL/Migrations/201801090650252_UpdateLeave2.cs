namespace TLO_KQGL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateLeave2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Dictionaries",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        code = c.String(nullable: false, maxLength: 6),
                        TypeCode = c.String(nullable: false, maxLength: 2),
                        value = c.String(nullable: false, maxLength: 20),
                        status = c.Boolean(nullable: false),
                        desc = c.String(maxLength: 50),
                        IsDel = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Dictionaries");
        }
    }
}

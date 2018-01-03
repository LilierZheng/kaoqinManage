namespace TLO_KQGL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTableAttendance1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SignChecks",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CheckID = c.Guid(nullable: false),
                        CheckedIDs = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.SignChecks");
        }
    }
}

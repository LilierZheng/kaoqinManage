namespace TLO_KQGL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateCalendar : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.calendars",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Yeat = c.String(maxLength: 4),
                        Month = c.String(maxLength: 2),
                        Date = c.String(maxLength: 10),
                        IsWork = c.Boolean(nullable: false),
                        IsVacation = c.Boolean(nullable: false),
                        CreateUser = c.String(maxLength: 10),
                        CreateDate = c.DateTime(),
                        LastUpdateUser = c.String(maxLength: 10),
                        LastUpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.calendars");
        }
    }
}

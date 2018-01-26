namespace TLO_KQGL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SuggestAndNews : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.News",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        Title = c.String(nullable: false, maxLength: 100),
                        Content = c.String(nullable: false, maxLength: 1000),
                        CreateUser = c.String(maxLength: 10),
                        CreateDate = c.DateTime(),
                        LastUpdateUser = c.String(maxLength: 10),
                        LastUpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Suggests",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        Content = c.String(nullable: false, maxLength: 1000),
                        CreateUser = c.String(maxLength: 10),
                        CreateDate = c.DateTime(),
                        LastUpdateUser = c.String(maxLength: 10),
                        LastUpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Suggests");
            DropTable("dbo.News");
        }
    }
}

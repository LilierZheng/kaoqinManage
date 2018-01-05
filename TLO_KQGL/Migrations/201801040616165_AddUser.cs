namespace TLO_KQGL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUser : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        userNo = c.String(maxLength: 5),
                        userName = c.String(maxLength: 5),
                        password = c.String(maxLength: 20),
                        CreateUser = c.String(),
                        CreateDate = c.DateTime(),
                        LastUpdateUser = c.String(maxLength: 10),
                        LastUpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Users");
        }
    }
}

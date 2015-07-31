namespace SharedPlaylistApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addedusernamefield : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Comments", "USername", c => c.String(nullable: false, maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Comments", "USername");
        }
    }
}

namespace SharedPlaylistApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedCommentField : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PlaylistId = c.String(nullable: false, maxLength: 50),
                        TrackId = c.String(nullable: false, maxLength: 50),
                        Order = c.Int(nullable: false),
                        Comment = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Comments");
        }
    }
}

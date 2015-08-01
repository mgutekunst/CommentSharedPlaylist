namespace SharedPlaylistApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class refactoredmodelintoportableclasslibrary : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Comments", "PlaylistId", c => c.String());
            AlterColumn("dbo.Comments", "TrackId", c => c.String());
            AlterColumn("dbo.Comments", "Comment", c => c.String());
            AlterColumn("dbo.Comments", "Username", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Comments", "Username", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Comments", "Comment", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.Comments", "TrackId", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Comments", "PlaylistId", c => c.String(nullable: false, maxLength: 50));
        }
    }
}

using SharedPlaylist.Models;

namespace SharedPlaylistApi.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class CommentsModel : DbContext
    {
        public CommentsModel()
            : base("name=CommentModel")
        {
        }

        public virtual DbSet<Comments> Comments { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}

namespace SharedPlaylist.Models
{
    public partial class Comments
    {
        public int Id { get; set; }

        //[Required]
        //[StringLength(50)]
        public string PlaylistId { get; set; }

        //[Required]
        //[StringLength(50)]
        public string TrackId { get; set; }

        public int Order { get; set; }

        //[Required]
        //[StringLength(255)]
        public string Comment { get; set; }


        //[Required]
        //[StringLength(50)]
        public string Username { get; set; }
    }
}

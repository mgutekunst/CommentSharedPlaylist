using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using SharedPlaylist.Models.Annotations;

namespace SharedPlaylist.Models
{
    public partial class Comments : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }


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
        private string _comments;
        public string Comment
        {
            get { return _comments; }
            set { _comments = value; RaisePropertyChanged("Comment"); }
        }


        //[Required]
        //[StringLength(50)]
        public string Username { get; set; }
    }
}

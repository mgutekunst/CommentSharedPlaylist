using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using SpotifyAPI.Web.Models;

namespace CommentSharedPlaylist.Utils
{
    public class FullTrackToArtistListConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo language)
        {
            if (value is FullTrack)
            {
                var inputValue = (FullTrack)value;

                if (inputValue.Artists != null && inputValue.Artists.Any())
                {
                    return string.Join(", ", inputValue.Artists.Select(a => a.Name));
                }

                return "Unknown";
            }

            return "Unknown";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo language)
        {
            return value;
        }
    }
}

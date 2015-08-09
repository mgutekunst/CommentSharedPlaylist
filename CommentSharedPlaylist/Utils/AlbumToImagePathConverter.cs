using System;
using System.Globalization;
using System.Windows.Data;
using SpotifyAPI.Web.Models;


namespace CommentSharedPlaylist.Utils
{
    public class AlbumToImagePathConverter : IValueConverter
    {


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is SimpleAlbum)
            {
                var inputValue = (SimpleAlbum)value;


                Image img = null;

                foreach (var image in inputValue.Images)
                {

                    if (img != null)
                    {
                        if (image.Height < img.Height)
                        {
                            img = image;
                        }
                    }
                    else
                    {
                        img = image;
                    }
                }

                return img.Url;
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}

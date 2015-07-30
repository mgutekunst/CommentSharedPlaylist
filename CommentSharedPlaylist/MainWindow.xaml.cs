using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using SharedPlaylist.Core.ViewModels;
using SpotifyAPI.Local;
using SpotifyAPI.Local.Models;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Auth;
using SpotifyAPI.Web.Enums;
using SpotifyAPI.Web.Models;

namespace CommentSharedPlaylist
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private MainViewModel _vm;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }


        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            _vm = (MainViewModel)DataContext;
            _vm.PropertyChanged += OnPropertyChanged;
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ConnectedWeb" && _vm.ConnectedWeb)
            {
                _vm.GetCollaborativePlaylistsCommand.Execute(null);
            }
        }

        private void addCommentForTrack(object sender, RoutedEventArgs e)
        {
            var track = ((MenuItem)sender).Tag as FullTrack;

            MessageBox.Show(string.Format("[Add Comment for {0}]", track.FullTrackName), "[Comment]", MessageBoxButton.YesNo);

        }
    }

}

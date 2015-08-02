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
using Microsoft.Ajax.Utilities;
using SharedPlaylist.Core.ViewModels;
using SharedPlaylist.Models;
using SharedPlaylistApi.Models;
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

            var dialog = new CommentDialog();
            if (dialog.ShowDialog() != true)
                return;


            _vm.NewComment.TrackId = track.Id;
            _vm.NewComment.PlaylistId = track.PlaylistId;
            _vm.NewComment.Comment = dialog.ResponseText;

            _vm.SendCommentCommand.Execute(null);

            if (track.Comments == null)
            {
                track.Comments = new List<Comments>() { _vm.NewComment };
            }
            else
            {
                track.Comments.Add(_vm.NewComment);
            }

        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            if (!tbComment.Text.IsNullOrWhiteSpace())
            {
                _vm.NewComment.Comment = tbComment.Text;
                _vm.SendCommentCommand.Execute(null);
            }
            else
            {
                MessageBox.Show("[Enter valid comment]", "Validation Error]", MessageBoxButton.OK);
            }
        }
    }

}

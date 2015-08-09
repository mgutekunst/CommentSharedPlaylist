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

        private bool _isTitleMouseDown = false;

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
            if (e.PropertyName == "SelectedTrack" && _vm.SelectedTrack != null)
            {
                commentsScrollViewer.ScrollToEnd();
            }
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            if (!tbComment.Text.IsNullOrWhiteSpace())
            {
                _vm.NewComment.Comment = tbComment.Text;
                _vm.SendCommentCommand.Execute(null);
                tbComment.Text = string.Empty;
                tbComment.Focus();
            }
            else
            {
                MessageBox.Show("[Enter valid comment]", "Validation Error]", MessageBoxButton.OK);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void titleBar_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            //_isTitleMouseDown = true;
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void titleBar_OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            _isTitleMouseDown = false;
        }

        private void titleBar_OnMouseMove(object sender, MouseEventArgs e)
        {
            //if(_isTitleMouseDown)
            //{
            //    this.
            //}	

            //this.dra
        }

        private void tbComment_OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ButtonBase_OnClick(null, null);
            }

        }
    }

}

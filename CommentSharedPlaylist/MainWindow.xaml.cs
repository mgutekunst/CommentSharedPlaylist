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
        //private SpotifyLocalAPI _spotifyLocal;
        //private SpotifyWebAPI _spotifyWeb;
        //private ImplicitGrantAuth _auth;
        //PrivateProfile profile;


        private MainViewModel _vm;

        public MainWindow()
        {
            InitializeComponent();

            //_spotifyLocal = new SpotifyLocalAPI();

            Loaded += OnLoaded;
        }


        private void OnLoaded(object sender, RoutedEventArgs e)
        {


            _vm = (MainViewModel) DataContext;

            _vm.PropertyChanged += OnPropertyChanged;

            //connectLocal();
            //connectWeb();
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {

            if (e.PropertyName == "ConnectedWeb" && _vm.ConnectedWeb)
            {
                _vm.GetCollaborativePlaylistsCommand.Execute(null);
            }

        }

        //private void connectWeb()
        //{
        //    _auth = new ImplicitGrantAuth
        //    {
        //        RedirectUri = "http://localhost:88/",
        //        ClientId = "7e169a49e74049aeb4980008368dd51d",
        //        Scope = Scope.UserReadPrivate | Scope.UserReadEmail | Scope.PlaylistReadPrivate | Scope.UserLibrarayRead | Scope.UserReadPrivate | Scope.UserFollowRead | Scope.UserReadBirthdate | Scope.PlaylistReadCollaborative,
        //        State = "XSS"
        //    };
        //    _auth.OnResponseReceivedEvent += _auth_OnResponseReceivedEvent;


        //    _auth.StartHttpServer();
        //    _auth.DoAuth();


        //}


        //void _auth_OnResponseReceivedEvent(Token token, string state)
        //{
        //    _auth.StopHttpServer();

        //    if (state != "XSS")
        //    {
        //        //MessageBox.Show("Wrong state received.", "SpotifyWeb API Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        return;
        //    }
        //    if (token.Error != null)
        //    {
        //        //MessageBox.Show("Error: " + token.Error, "SpotifyWeb API Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        return;
        //    }

        //    _spotifyWeb = new SpotifyWebAPI
        //    {
        //        UseAuth = true,
        //        AccessToken = token.AccessToken,
        //        TokenType = token.TokenType
        //    };
        //    InitialSetup();
        //}












        //private void InitialSetup()
        //{
        //    profile = _spotifyWeb.GetPrivateProfile();

        //    var playlists = _spotifyWeb.GetUserPlaylists(profile.Id);

        //    var collaborative = playlists.Items.Where(e => e.Collaborative == true).ToList();

        //    handlePlaylists(collaborative);
    }

    //private void handlePlaylists(List<SimplePlaylist> collaborative)
    //{
    //if (App.Current.Dispatcher.CheckAccess())
    //{
    //    foreach (var simplePlaylist in collaborative)
    //    {
    //        AddHeader(simplePlaylist.Name);

    //        var tracks = _spotifyWeb.GetPlaylistTracks(profile.Id, simplePlaylist.Id);

    //        if (tracks.Items == null) continue;

    //        foreach (var track in tracks.Items)
    //        {
    //            addTrack(track);
    //        }
    //    }
    //}
    //else
    //{
    //    App.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
    //    {
    //        handlePlaylists(collaborative);
    //    }));
    //}
    //}

    //private void addTrack(PlaylistTrack track)
    //{
    //    var tvItem = (TreeViewItem)treeView.Items[treeView.Items.Count - 1];
    //    tvItem.Items.Add(new TreeViewItem()
    //    {
    //        Header = string.Format("{0} - {1}", track.Track.Artists.First().Name, track.Track.Name)
    //    });

    //}

    //private void AddHeader(string name)
    //{
    //    treeView.Items.Add(new TreeViewItem()
    //    {
    //        Header = name
    //    });
    //}


    //private void connectLocal()
    //{
    //    if (!SpotifyLocalAPI.IsSpotifyRunning())
    //    {
    //        MessageBox.Show("Spotify isn't running!");
    //        return;
    //    }
    //    if (!SpotifyLocalAPI.IsSpotifyWebHelperRunning())
    //    {
    //        MessageBox.Show("SpotifyWebHelper isn't running!");
    //        return;
    //    }

    //    bool successful = _spotifyLocal.Connect();
    //    if (successful)
    //    {
    //        _spotifyLocal.ListenForEvents = true;
    //    }
    //    else
    //    {
    //    }
    //}
}

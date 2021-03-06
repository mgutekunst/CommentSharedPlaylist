﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.Practices.ServiceLocation;
using SharedPlaylist.Core.Service;
using SharedPlaylist.Core.Utils;
using SharedPlaylist.Models;
using SharedPlaylistApi.Models;
using SpotifyAPI.Local;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Auth;
using SpotifyAPI.Web.Enums;
using SpotifyAPI.Web.Models;

namespace SharedPlaylist.Core.ViewModels
{
    public class MainViewModel : ViewModelBase
    {

        #region Fields

        private SpotifyLocalAPI _spotifyLocal;
        private SpotifyWebAPI _spotifyWeb;
        private ImplicitGrantAuth _auth;


        #endregion

        

        #region Properties

        private PrivateProfile _privateProfile;
        public PrivateProfile PrivateProfile
        {
            get { return _privateProfile; }
            set
            {
                _privateProfile = value;
                RaisePropertyChanged();
            }
        }


        List<SimplePlaylist> _collaborativePlaylists;
        public List<SimplePlaylist> CollaborativePlaylists
        {
            get { return _collaborativePlaylists; }
            set
            {
                _collaborativePlaylists = value;
                RaisePropertyChanged();
            }
        }


        private SimplePlaylist _selectedPlaylist;
        public SimplePlaylist SelectedPlaylist
        {
            get { return _selectedPlaylist; }
            set
            {
                _selectedPlaylist = value;
                RaisePropertyChanged("SelectedPlaylist");
                updateNewComment();
            }
        }


        private PlaylistTrack _selectedTrack;
        public PlaylistTrack SelectedTrack
        {
            get { return _selectedTrack; }
            set
            {
                _selectedTrack = value; RaisePropertyChanged("SelectedTrack");

                updateNewComment();

            }
        }

        private void updateNewComment()
        {
            if (SelectedTrack != null)
            {
                NewComment.TrackId = SelectedTrack.Track.Id;
            }

            NewComment.PlaylistId = SelectedPlaylist.Id;
        }


        Paging<SimplePlaylist> _playlists;
        public Paging<SimplePlaylist> Playlists
        {
            get { return _playlists; }
            set
            {
                _playlists = value;
                RaisePropertyChanged();
            }
        }

        private List<Comments> _comments;
        public List<Comments> Comments
        {
            get { return _comments; }
            set { _comments = value; RaisePropertyChanged("Comments"); }
        }

        private bool _connectedWeb;
        public bool ConnectedWeb
        {
            get { return _connectedWeb; }
            set { _connectedWeb = value; RaisePropertyChanged("ConnectedWeb"); }
        }


        private Comments _newComments;

        public Comments NewComment
        {
            get { return _newComments; }
            set { _newComments = value; }
        }


        #endregion


        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            set { _isLoading = value; RaisePropertyChanged(); }
        }

        public string Title
        {
            get { return "Comment Shared Playlist"; }
        }


        public RelayCommand InitCommand { get; private set; }

        public RelayCommand GetCollaborativePlaylistsCommand { get; private set; }

        public RelayCommand<SimplePlaylist> GetTracksForPlaylistCommand { get; private set; }

        public RelayCommand SendCommentCommand { get; private set; }

        public MainViewModel()
        {
            InitCommand = new RelayCommand(init);
            GetCollaborativePlaylistsCommand = new RelayCommand(getPlaylists);
            GetTracksForPlaylistCommand = new RelayCommand<SimplePlaylist>(getTracksForPlaylist);
            SendCommentCommand = new RelayCommand(sendComment);

            
            init();
        }

        private async void sendComment()
        {

            var dbComment = await ServiceLocator.Current.GetInstance<DataService>().PostCommentAsync(NewComment);
            if (dbComment.Id != 0)
            {
                SelectedTrack.Track.Comments = await ServiceLocator.Current.GetInstance<DataService>().GetCommentsForTrackById(dbComment.TrackId);

                NewComment.Comment = string.Empty;
            }

        }

        public async Task<List<Comments>> GetAllComments()
        {
            return await ServiceLocator.Current.GetInstance<DataService>().GetCommentsAsync();
        }

        private void getTracksForPlaylist(SimplePlaylist playlist)
        {
            IsLoading = true;
            var tracks = _spotifyWeb.GetPlaylistTracks(playlist.Owner.Id, playlist.Id, string.Empty, Statics.PLAYLIST_TRACK_LIMIT);

            playlist.Tracks.PlaylistTracks = tracks.Items;

            getCommentsForPlaylistTracks(playlist);

            IsLoading = false;
        }

        private void getCommentsForPlaylistTracks(SimplePlaylist playlist)
        {
            foreach (var track in playlist.Tracks.PlaylistTracks)
            {

                // needed for comments in database
                track.Track.PlaylistId = playlist.Id;

                var comments = Comments.Where(e => e.TrackId == track.Track.Id && e.PlaylistId == playlist.Id);
                if (!comments.Any())
                    continue;

                comments.OrderBy(e => e.Order);
                track.Track.Comments = new List<Comments>(comments);
            }
        }

        private async void init()
        {
            _spotifyLocal = new SpotifyLocalAPI();


            connectLocal();
            connectWeb();

        }




        #region Connection Handling


        private bool connectLocal()
        {
            if (!SpotifyLocalAPI.IsSpotifyRunning())
            {
                //MessageBox.Show("Spotify isn't running!");
                return false;
            }
            if (!SpotifyLocalAPI.IsSpotifyWebHelperRunning())
            {
                //MessageBox.Show("SpotifyWebHelper isn't running!");
                return false;
            }

            bool successful = _spotifyLocal.Connect();
            if (successful)
            {
                _spotifyLocal.ListenForEvents = true;
            }
            else
            {
            }



            return successful;
        }



        private void connectWeb()
        {
            try
            {
                _auth = new ImplicitGrantAuth
                {
                    RedirectUri = "http://localhost:88/",
                    ClientId = "7e169a49e74049aeb4980008368dd51d",
                    Scope = Scope.UserReadPrivate | Scope.UserReadEmail | Scope.PlaylistReadPrivate | Scope.UserLibrarayRead | Scope.UserReadPrivate | Scope.UserFollowRead | Scope.UserReadBirthdate | Scope.PlaylistReadCollaborative,
                    State = "XSS"
                };
                _auth.OnResponseReceivedEvent += _auth_OnResponseReceivedEvent;


                _auth.StartHttpServer();
                _auth.DoAuth();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }


        void _auth_OnResponseReceivedEvent(Token token, string state)
        {
            _auth.StopHttpServer();

            if (state != "XSS")
            {
                //MessageBox.Show("Wrong state received.", "SpotifyWeb API Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (token.Error != null)
            {
                //MessageBox.Show("Error: " + token.Error, "SpotifyWeb API Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _spotifyWeb = new SpotifyWebAPI
            {
                UseAuth = true,
                AccessToken = token.AccessToken,
                TokenType = token.TokenType
            };

            ConnectedWeb = true;

            //InitialSetup();
        }


        #endregion



        private async void getPlaylists()
        {
            IsLoading = true;
            PrivateProfile = _spotifyWeb.GetPrivateProfile();

            Playlists = _spotifyWeb.GetUserPlaylists(_privateProfile.Id);

            NewComment = new Comments()
            {
                Username = PrivateProfile.Id
            };

          
                
                //GetAllComments();


            var collaborativePlaylists = _playlists.Items.Where(e => e.Collaborative).ToList();


            if (collaborativePlaylists.Any())
            {

                string[] ids = collaborativePlaylists.Select(e => e.Id).ToArray();

                Comments = await ServiceLocator.Current.GetInstance<DataService>().GetCommentsForPlaylistIdsAsync(ids);



                foreach (var playlist in collaborativePlaylists)
                {
                    getTracksForPlaylist(playlist);
                }
            }






            CollaborativePlaylists = collaborativePlaylists;


            //var y = await ServiceLocator.Current.GetInstance<DataService>().PostCommentAsync(new Comments()
            //{
            //    Comment = "Test Dataservice",
            //    PlaylistId = "1", 
            //    TrackId = "1", 
            //    Order = 1
            //});

            IsLoading = false;
        }
    }
}

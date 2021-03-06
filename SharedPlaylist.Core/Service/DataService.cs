﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SharedPlaylist.Core.Models;
using SharedPlaylist.Models;
using SharedPlaylistApi.Models;

namespace SharedPlaylist.Core.Service
{
    public class DataService : DataServiceBase
    {
        //public const string BASE_URL = "http://localhost:1234/api/{0}";
        public const string BASE_URL = "http://commentsharedplaylist.azurewebsites.net/api/{0}";
        //public const string TOKEN_URL = "http://localhost:1234/token";


        private static string TOKEN_BODY = "grant_type=password&username={0}&password={1}";
        private static string COMMENT_URL = "Comments";
        private static string COMMENTS_FOR_TRACK_BY_ID_URL = "Comments/GetCommentsForTrackById?Id={0}";
        private static string COMMENTS_FOR_PLAYLIST_IDS_URL = "Comments/GetCommentsForPlaylistIds?{0}";

        public DataService()
        {

        }


        public async Task<List<Comments>> GetCommentsAsync()
        {
            try
            {
                var requestUrl = string.Format(BASE_URL, COMMENT_URL);
                var response = await RequestAsync<string>(requestUrl, HttpMethod.Get);

                if (response.IsSuccessStatusCode)
                {
                    var comments = JsonConvert.DeserializeObject<List<Comments>>(response.Content);
                    return comments;
                }
            }
            catch (Exception ex)
            {

                Debug.WriteLine(ex.ToString());
            }

            return new List<Comments>();
        }

        public async Task<List<Comments>> GetCommentsForPlaylistIdsAsync(string[] playlistIds)
        {
            try
            {

                //var ids = string.Join("playlistIds=,&", playlistIds);
                var ids = new StringBuilder();
                foreach (var playlistId in playlistIds)
                {
                    ids.AppendFormat("playlistIds={0}&", playlistId);
                }

                var playlistParameter = string.Format(COMMENTS_FOR_PLAYLIST_IDS_URL, ids);
                var requestUrl = string.Format(BASE_URL, playlistParameter);

                var response = await RequestAsync<string>(requestUrl, HttpMethod.Get);

                if (response.IsSuccessStatusCode)
                {
                    var comments = JsonConvert.DeserializeObject<List<Comments>>(response.Content);
                    return comments;
                }
            }
            catch (Exception ex)
            {

                Debug.WriteLine(ex.ToString());
            }

            return new List<Comments>();
        }


        public async Task<List<Comments>> GetCommentsForTrackById(string trackId)
        {
            try
            {
                var commentUrl = string.Format(COMMENTS_FOR_TRACK_BY_ID_URL, trackId);

                var requestUrl = string.Format(BASE_URL, commentUrl);
                var response = await RequestAsync<string>(requestUrl, HttpMethod.Get);

                if (response.IsSuccessStatusCode)
                {
                    var comments = JsonConvert.DeserializeObject<List<Comments>>(response.Content);
                    return comments;
                }
            }
            catch (Exception ex)
            {

                Debug.WriteLine(ex.ToString());
            }

            return new List<Comments>();
        }


        public async Task<Comments> PostCommentAsync(Comments comment)
        {
            try
            {
                var requestUrl = string.Format(BASE_URL, COMMENT_URL);
                var content = JsonConvert.SerializeObject(comment);
                var response = await RequestAsync<string>(requestUrl, HttpMethod.Post, content, CONTENT_TYPE_APPLICATION_JSON);

                if (response.IsSuccessStatusCode)
                {
                    var comments = JsonConvert.DeserializeObject<Comments>(response.Content);
                    return comments;

                }

            }
            catch (Exception ex)
            {

                Debug.WriteLine(ex.ToString());
            }

            return new Comments();
        }
    }
}

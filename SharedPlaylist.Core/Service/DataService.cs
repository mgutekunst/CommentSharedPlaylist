using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
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
        public const string BASE_URL = "https://commentsharedplaylist.azurewebsites.net/api/{0}";
        //public const string TOKEN_URL = "http://localhost:1234/token";


        private static string TOKEN_BODY = "grant_type=password&username={0}&password={1}";
        private static string COMMENT_URL = "Comments";


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

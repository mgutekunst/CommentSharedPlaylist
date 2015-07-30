using Newtonsoft.Json;

namespace SharedPlaylist.Core.Models
{
    public class HttpError : HttpErrorBase
    {
        [JsonProperty("error")]
        public string ErrorType { get; set; }

        [JsonProperty("error_description")]
        public override string ErrorMessage { get; set; }
    }
}

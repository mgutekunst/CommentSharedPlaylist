using System;
using System.Net;

namespace SharedPlaylist.Core.Models
{
    public class HttpResponse
    {
        public HttpStatusCode HttpStatusCode { get; set; }
        public string Message { get; set; }
        public Uri Location { get; set; }
        public HttpErrorBase Error { get; set; }

        public bool IsSuccessStatusCode
        {
            get
            {
                if (HttpStatusCode >= HttpStatusCode.OK)
                    return HttpStatusCode <= (HttpStatusCode)299;
                else
                    return false;
            }
        }

        public override string ToString()
        {
            return string.Format("HttpStatusCode: {0}, Message: {1}", HttpStatusCode, Message);
        }
    }

    public class HttpResponse<T> : HttpResponse
    {
        public HttpResponse()
        {

        }

        public HttpResponse(HttpResponse response)
        {
            this.HttpStatusCode = response.HttpStatusCode;
            this.Location = response.Location;
            this.Message = response.Message;
            this.Error = response.Error;
        }

        public T Content { get; set; }
    }
}

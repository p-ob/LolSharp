namespace LolSharp
{
    using System;
    using System.Net;
    using RestCore;

    public class RiotApiException : Exception
    {
        public HttpStatusCode StatusCode { get; set; }

        public RestResponse Response { get; set; }

        public RiotApiException(HttpStatusCode statusCode, string message, Exception innerException, RestResponse response)
            : base(message, innerException)
        {
            StatusCode = statusCode;
            Response = response;
        }
    }
}

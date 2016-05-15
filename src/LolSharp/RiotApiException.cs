namespace LolSharp
{
    using System;
    using System.Net;
    using RestSharp;

    [Serializable]
    public class RiotApiException : Exception
    {
        public HttpStatusCode StatusCode { get; set; }

        public IRestResponse Response { get; set; }

        public RiotApiException(HttpStatusCode statusCode, string message, Exception innerException, IRestResponse response)
            : base(message, innerException)
        {
            StatusCode = statusCode;
            Response = response;
        }
    }
}

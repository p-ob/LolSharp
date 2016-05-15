namespace LolSharp
{
    using System;
    using System.Net;

    [Serializable]
    public class RiotApiException : Exception
    {
        public HttpStatusCode StatusCode { get; set; }

        public RiotApiException(HttpStatusCode statusCode) : this(statusCode, string.Empty, null)
        {
        }

        public RiotApiException(HttpStatusCode statusCode, string message) : this(statusCode, message, null)
        {
        }

        public RiotApiException(HttpStatusCode statusCode, string message, Exception innerException)
            : base(message, innerException)
        {
            StatusCode = statusCode;
        }
    }
}

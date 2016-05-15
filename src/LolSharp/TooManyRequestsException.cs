namespace LolSharp
{
    using System;

    using System.Net;

    [Serializable]
    public class TooManyRequestsException : RiotApiException
    {
        public int RetryAfter { get; set; }

        public TooManyRequestsException() : this(-1, "No retry after value given.")
        {
        }

        public TooManyRequestsException(int retryAfter) : base((HttpStatusCode)429, string.Empty)
        {
            RetryAfter = retryAfter;
        }

        public TooManyRequestsException(int retryAfter, string message) : base((HttpStatusCode)429, message)
        {
            RetryAfter = retryAfter;
        }

        public TooManyRequestsException(int retryAfter, string message, Exception innerException)
            : base((HttpStatusCode)429, message, innerException)
        {
            RetryAfter = retryAfter;
        }
    }
}

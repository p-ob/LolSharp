namespace LolSharp
{
    using System;

    using System.Net;

    [Serializable]
    public class TooManyRequestsException : RiotApiException
    {
        public int RetryAfter { get; set; }

        public TooManyRequestsException() : this(-1)
        {
        }

        public TooManyRequestsException(int retryAfter) : this(retryAfter, string.Empty)
        {
        }

        public TooManyRequestsException(int retryAfter, string message) : this(retryAfter, message, null)
        {
        }

        public TooManyRequestsException(int retryAfter, string message, Exception innerException)
            : base((HttpStatusCode)429, message, innerException)
        {
            RetryAfter = retryAfter;
        }
    }
}

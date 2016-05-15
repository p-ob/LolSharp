namespace LolSharp
{
    using System;

    using System.Net;
    using RestSharp;

    [Serializable]
    public class TooManyRequestsException : RiotApiException
    {
        public int RetryAfter { get; set; }

        public TooManyRequestsException(Exception innerException, IRestResponse response) : base((HttpStatusCode)429, "No RetryAfter value available.", innerException, response)
        {
        }

        public TooManyRequestsException(int retryAfter, Exception innerException, IRestResponse response)
            : base((HttpStatusCode)429, "Riot responded with an HTTP status code of 429. Check RetryAfter to see minimum wait time.", innerException, response)
        {
            RetryAfter = retryAfter;
        }
    }
}

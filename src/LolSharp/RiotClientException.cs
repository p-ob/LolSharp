namespace LolSharp
{
    using System;

    [Serializable]
    public class RiotClientException : Exception
    {
        public RiotClientException(string message) : this(message, null)
        {

        }

        public RiotClientException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}

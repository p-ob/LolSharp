namespace LolSharp.RestCore
{
    using System;

    public class RestCoreException : Exception
    {
        public RestCoreException(string message) : this(message, null)
        {

        }

        public RestCoreException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}

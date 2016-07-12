namespace LolSharp.RestCore
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Net;

    public class RestResponse
    {
        public IEnumerable<Parameter> Headers { get; set; }

        public Exception ErrorException { get; set; }

        public HttpStatusCode StatusCode { get; set; }

        public RestResponse()
        {
            
        }

        public RestResponse(RestResponse other)
        {
            Headers = other.Headers;
            ErrorException = other.ErrorException;
            StatusCode = other.StatusCode;
        }
    }

    public class RestResponse<T> : RestResponse
    {
        public T Data { get; set; }

        public RestResponse(RestResponse other) : base(other)
        {
            
        }
    }
}

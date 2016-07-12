namespace LolSharp.RestCore
{
    using System.Collections.Generic;
    using System.Net.Http;
    using Newtonsoft.Json.Schema;

    public class RestRequest
    {
        public string RequestUri { get; set; }
        public HttpMethod MethodType { get; set; }
        public object Data { get; set; }
        public List<Parameter> Parameters { get; set; }
        public List<Parameter> Headers { get; set; }

        public RestRequest() : this(null)
        {

        }

        public RestRequest(string requestUri, HttpMethod method = null, object data = null)
        {
            RequestUri = requestUri;
            MethodType = method ?? HttpMethod.Get;
            Data = data;
        }

        public void AddParameter(string name, object value, ParameterType type)
        {
            Parameters.Add(new Parameter
            {
                Name = name,
                Value = value,
                Type = type
            });
        }
    }
}

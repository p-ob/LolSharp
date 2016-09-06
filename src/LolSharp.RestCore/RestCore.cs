namespace LolSharp.RestCore
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;
    using Newtonsoft.Json;

    public class RestCore : IRestCore, IDisposable
    {
        private HttpClient _client;
        private readonly Parameter _authorizationHeader;

        private Uri _baseUrl;

        public Uri BaseUrl
        {
            get { return _baseUrl; }
            set
            {
                if (BaseUrl == value) return;
                Dispose();
                _baseUrl = value;
                _client = new HttpClient { BaseAddress = value };
                _client.DefaultRequestHeaders.Accept.Clear();
                _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            }
        }

        public RestCore(Parameter authHeader = null) : this((Uri) null, authHeader)
        {
        }

        public RestCore(string baseUrl, Parameter authHeader = null) : this(new Uri(baseUrl), authHeader)
        {
        }

        public RestCore(Uri baseUri, Parameter authHeader = null)
        {
            _authorizationHeader = authHeader;
            _client = new HttpClient();
            _baseUrl = baseUri;

            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<RestResponse> Execute(RestRequest request)
        {
            var response = await MakeRequest(request);

            return MapHttpResponseToRestResponse(response);
        }

        public async Task<RestResponse<T>> Execute<T>(RestRequest request) where T : new()
        {
            var response = await MakeRequest(request);
            var data = await response.Content.ReadAsStringAsync();

            var deserializedData = JsonConvert.DeserializeObject<T>(data);
            return new RestResponse<T>(MapHttpResponseToRestResponse(response))
            {
                Data = deserializedData
            };
        }

        private async Task<HttpResponseMessage> MakeRequest(RestRequest request)
        {
            var httpRequest = FormatRequest(request);
            var response = await _client.SendAsync(httpRequest);

            if (!response.IsSuccessStatusCode)
            {
                throw new RestCoreException(response.ReasonPhrase);
            }

            return response;
        }

        private HttpRequestMessage FormatRequest(RestRequest request)
        {
            request.Headers = request.Headers ?? new List<Parameter>();
            request.Parameters = request.Parameters ?? new List<Parameter>();

            var requestUri = request.RequestUri;
            foreach (var param in request.Parameters)
            {
                switch (param.Type)
                {
                    case ParameterType.UrlParam:
                        requestUri = requestUri.Replace("{" + param.Name + "}", Uri.EscapeDataString(param.Value.ToString()));
                        break;
                    case ParameterType.QueryParam:
                        if (!requestUri.Contains("?"))
                        {
                            requestUri += "?";
                        }
                        requestUri += param.Name + "=" + param.Value;
                        break;
                    case ParameterType.HeaderParam:
                        request.Headers.Add(param);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            var httpRequest = new HttpRequestMessage(request.MethodType, requestUri);
            foreach (var header in request.Headers)
            {
                if (header.Value as IEnumerable<object> != null)
                {
                    var headerValue = header.Value as IEnumerable<object>;
                    httpRequest.Headers.Add(header.Name, headerValue.OfType<string>());
                }
                else
                {
                    httpRequest.Headers.Add(header.Name, header.Value.ToString());
                }
            }

            if (_authorizationHeader != null)
            {
                httpRequest.Headers.Authorization = new AuthenticationHeaderValue(_authorizationHeader.Name, _authorizationHeader.Value.ToString());
            }

            if (request.Data != null)
            {
                httpRequest.Content = new StringContent(JsonConvert.SerializeObject(request.Data), Encoding.UTF8, "application/json");
            }

            return httpRequest;
        }

        private static RestResponse MapHttpResponseToRestResponse(HttpResponseMessage httpResponse)
        {
            var restResponse = new RestResponse
            {
                Headers = httpResponse.Headers.Select(h => new Parameter
                {
                    Name = h.Key,
                    Type = ParameterType.HeaderParam,
                    Value = h.Value
                }),
                StatusCode = httpResponse.StatusCode
            };

            try
            {
                httpResponse.EnsureSuccessStatusCode();
            }
            catch (Exception e)
            {
                restResponse.ErrorException = e;
            }

            return restResponse;
        }

        public void Dispose()
        {
            _client.Dispose();
        }
    }
}
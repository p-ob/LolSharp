namespace LolSharp.RestCore
{
    using System;
    using System.Threading.Tasks;

    public interface IRestCore
    {
        Uri BaseUrl { get; set; }

        Task<RestResponse>  Execute(RestRequest request);

        Task<RestResponse<T>> Execute<T>(RestRequest request) where T : new();
    }
}

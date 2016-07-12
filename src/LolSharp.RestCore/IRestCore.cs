namespace LolSharp.RestCore
{
    using System.Threading.Tasks;

    public interface IRestCore
    {
        string BaseUrl { get; set; }

        Task<RestResponse>  Execute(RestRequest request);

        Task<RestResponse<T>> Execute<T>(RestRequest request) where T : new();
    }
}

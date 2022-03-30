using System.Net.Http;

namespace priceapp.WebServices
{
    public interface IPriceAppWebAccess
    {
        HttpClientHandler GetHttpClientHandler();
    }
}
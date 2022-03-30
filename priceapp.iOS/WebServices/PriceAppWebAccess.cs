using System.Net.Http;
using priceapp.iOS.WebServices;
using priceapp.WebServices;
using Xamarin.Forms;

[assembly: Dependency(typeof(PriceAppWebAccess))]
namespace priceapp.iOS.WebServices
{
    public class PriceAppWebAccess : IPriceAppWebAccess
    {
        public HttpClientHandler GetHttpClientHandler() {
            return new HttpClientHandler();
        }
    }
}
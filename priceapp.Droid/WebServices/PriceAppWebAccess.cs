using priceapp.WebServices;
using Xamarin.Forms;
using System.Net.Http;
using priceapp.Droid.WebServices;

[assembly: Dependency(typeof(PriceAppWebAccess))]
namespace priceapp.Droid.WebServices
{
    public class PriceAppWebAccess : IPriceAppWebAccess
    {
        public HttpClientHandler GetHttpClientHandler()
        {
            return new Xamarin.Android.Net.AndroidClientHandler();
        }
    }
}
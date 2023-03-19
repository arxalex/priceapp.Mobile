using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using priceapp.Events.Delegates;
using priceapp.Events.Models;
using priceapp.Repositories.Implementation;
using priceapp.Repositories.Interfaces;
using priceapp.Repositories.Models;
using priceapp.WebServices;
using RestSharp;
using Xamarin.Forms;

[assembly: Dependency(typeof(UserRepository))]

namespace priceapp.Repositories.Implementation;

public class UserRepository : IUserRepository
{
    private readonly RestClient _client;

    public UserRepository()
    {
        var priceAppWebAccess = DependencyService.Get<IPriceAppWebAccess>();
        var httpClient = new HttpClient(priceAppWebAccess.GetHttpClientHandler())
        {
            BaseAddress = new Uri(Constants.ApiUrl)
        };
        _client = new RestClient(httpClient);

        _client.AddDefaultHeader("Cookie", $"Bearer {Xamarin.Essentials.SecureStorage.GetAsync("token").Result}");
    }

    public event ConnectionErrorHandler BadConnectEvent;

    public async Task<UserRepositoryModel> GetUser()
    {
        var request = new RestRequest("User/info");

        var response = await _client.ExecuteAsync(request);
        if (response.StatusCode != HttpStatusCode.OK || response.Content == null)
        {
            BadConnectEvent?.Invoke(this,
                new ConnectionErrorArgs() {Success = false, StatusCode = (int) response.StatusCode});
            return null;
        }

        return JsonSerializer.Deserialize<UserRepositoryModel>(response.Content);
    }

    public async Task<LoginResultModel> Login(string username, string password)
    {
        var request = new RestRequest("User/login", Method.Post);

        var body = new LoginRequestModel()
        {
            Username = username,
            Password = password
        };
        
        request.AddHeader("Content-Type", "application/json");
        request.AddBody(body, "application/json");
        
        var response = await _client.ExecuteAsync(request);

        if (response.Content == null)
        {
            return new LoginResultModel(){Succsess = false, Message = ExceptionMessagesTranslated.SomethingWentWrong};
        }
        
        if (response.StatusCode == HttpStatusCode.BadRequest)
        {
            var result = JsonSerializer.Deserialize<ErrorResponseModel>(response.Content);

            var resultModel = new LoginResultModel(){Succsess = false};
            switch (result.Message)
            {
                case ExceptionMessages.UsernameIncorrect:
                    resultModel.Message = ExceptionMessagesTranslated.UsernameIncorrect;
                    break;
                case ExceptionMessages.EmailIncorrect:
                    resultModel.Message = ExceptionMessagesTranslated.EmailIncorrect;
                    break;
                case ExceptionMessages.PasswordIncorrect:
                    resultModel.Message = ExceptionMessagesTranslated.PasswordIncorrect;
                    break;
                default:
                    resultModel.Message = ExceptionMessagesTranslated.SomethingWentWrong;
                    break;
            }

            return resultModel;
        }
        
        if (response.StatusCode == HttpStatusCode.OK)
        {
            var result = JsonSerializer.Deserialize<UserLoginRepositoryModel>(response.Content);
            await Xamarin.Essentials.SecureStorage.SetAsync("token", result.Token);

            return new LoginResultModel() { Succsess = true };
        }

        return new LoginResultModel() { Succsess = false, Message = ExceptionMessagesTranslated.SomethingWentWrong };
    }

    public async Task<bool> IsUserLoggedIn()
    {
        var request = new RestRequest("Info/authorize-check");
        var response = await _client.ExecuteAsync(request);

        return response.StatusCode == HttpStatusCode.OK;
    }
}
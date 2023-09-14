using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using priceapp.Events.Delegates;
using priceapp.Events.Models;
using priceapp.Repositories.Implementation;
using priceapp.Repositories.Interfaces;
using priceapp.Repositories.Models;
using priceapp.Utils;
using RestSharp;
using Xamarin.Forms;

[assembly: Dependency(typeof(UserRepository))]

namespace priceapp.Repositories.Implementation;

public class UserRepository : IUserRepository
{
    private readonly RestClient _client = ConnectionUtil.GetRestClient();

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
            username = username,
            password = password
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
            resultModel.Message = TranslateUtill.TranslateException(result.message, username, username);

            return resultModel;
        }
        
        if (response.StatusCode == HttpStatusCode.OK)
        {
            var result = JsonSerializer.Deserialize<UserLoginRepositoryModel>(response.Content);

            ConnectionUtil.RemoveUserInfo();
            await ConnectionUtil.UpdateToken(result.token);

            return new LoginResultModel() { Succsess = true };
        }

        return new LoginResultModel() { Succsess = false, Message = ExceptionMessagesTranslated.SomethingWentWrong };
    }

    public async Task<RegisterResultModel> Register(string username, string email, string password)
    {
        var request = new RestRequest("User/register", Method.Post);

        var body = JsonSerializer.Serialize(new
        {
            username, email, password
        });
        
        request.AddHeader("Content-Type", "application/json");
        request.AddBody(body, "application/json");

        var response = await _client.ExecuteAsync(request);
        
        if (response.Content == null)
        {
            return new RegisterResultModel(){Succsess = false, Message = ExceptionMessagesTranslated.SomethingWentWrong};
        }
        
        if (response.StatusCode == HttpStatusCode.BadRequest)
        {
            var result = JsonSerializer.Deserialize<ErrorResponseModel>(response.Content);

            var resultModel = new RegisterResultModel(){Succsess = false};
            resultModel.Message = TranslateUtill.TranslateException(result.message, username, email);

            return resultModel;
        }
        
        if (response.StatusCode == HttpStatusCode.OK)
        {
            var result = JsonSerializer.Deserialize<SuccessRepositoryModel>(response.Content);

            return new RegisterResultModel() { Succsess = result.status };
        }

        return new RegisterResultModel() { Succsess = false, Message = ExceptionMessagesTranslated.SomethingWentWrong };
    }
    
    public async Task<DeleteResultModel> Delete(string password)
    {
        var request = new RestRequest("User/delete", Method.Post);

        var body = new DeleteRequestModel()
        {
            password = password
        };
        
        request.AddHeader("Content-Type", "application/json");
        request.AddBody(body, "application/json");
        
        var response = await _client.ExecuteAsync(request);

        if (response.Content == null)
        {
            return new DeleteResultModel(){Succsess = false, Message = ExceptionMessagesTranslated.SomethingWentWrong};
        }
        
        if (response.StatusCode == HttpStatusCode.BadRequest)
        {
            var result = JsonSerializer.Deserialize<ErrorResponseModel>(response.Content);

            var resultModel = new DeleteResultModel(){Succsess = false};
            resultModel.Message = TranslateUtill.TranslateException(result.message);

            return resultModel;
        }
        
        if (response.StatusCode == HttpStatusCode.OK)
        {
            var result = JsonSerializer.Deserialize<DeleteRepositoryModel>(response.Content);

            return new DeleteResultModel() { Succsess = result.status };
        }

        return new DeleteResultModel() { Succsess = false, Message = ExceptionMessagesTranslated.SomethingWentWrong };
    }

    public async Task<bool> IsUserLoggedIn()
    {
        var request = new RestRequest("Info/authorize-check");
        var response = await _client.ExecuteAsync(request);

        return response.StatusCode == HttpStatusCode.OK;
    }
}
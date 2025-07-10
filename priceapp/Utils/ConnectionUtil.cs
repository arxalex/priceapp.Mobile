using RestSharp;

namespace priceapp.Utils;

public static class ConnectionUtil
{
    private static void SetToken(this RestClient client)
    {
        var token = SecureStorage.GetAsync("token").Result;
        if (!string.IsNullOrEmpty(token))
        {
            client.AddDefaultHeader("Authorization", $"Bearer {token}");
        }
    }

    public static RestClient GetRestClient(HttpClient httpClient)
    {
        RestClient client = new RestClient(httpClient);

        client.SetToken();
        return client;
    }

    public static async Task UpdateToken(string token)
    {
        await SecureStorage.SetAsync("token", token);
    }

    public static void RemoveToken()
    {
        SecureStorage.Remove("token");
    }

    public static async Task<bool> IsTokenExists()
    {
        return !string.IsNullOrEmpty(await SecureStorage.GetAsync("token"));
    }
    
    public static void RemoveUserInfo()
    {
        SecureStorage.Remove("username");
        SecureStorage.Remove("email");
    }
    
    public static async Task<bool> IsUserInfoExists()
    {
        return !string.IsNullOrEmpty(await SecureStorage.GetAsync("username"));
    }
    
    public static async Task UpdateUserInfo(string? username, string? email)
    {
        await SecureStorage.SetAsync("username", username);
        await SecureStorage.SetAsync("email", email);
    }

    public static async Task<(string username, string? email)> GetUserInfo()
    {
        return (await SecureStorage.GetAsync("username"),
            await SecureStorage.GetAsync("email"));
    }
}
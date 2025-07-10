namespace priceapp.Utils;

public static class CookieUtil
{
    public static (string key, string value) Parse(string cookie)
    {
        var split = cookie.Split(';')[0].Split('=');
        var key = split[0];
        var value = split[1];
        return (key, value);
    }

    public static string DictionaryToCookieString(Dictionary<string, string> cookieContainer)
    {
        var cookie = "";
        cookieContainer.ForEach(x => cookie += x.Key + "=" + x.Value + "; ");
        return cookie;
    }
}
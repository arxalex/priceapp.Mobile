namespace priceapp.Utils
{
    public static class CookieUtil
    {
        public static (string key, string value) Parse(string cookie)
        {
            var split = cookie.Split(';')[0].Split('=');
            var key = split[0];
            var value = split[1];
            return (key, value);
        }
    }
}
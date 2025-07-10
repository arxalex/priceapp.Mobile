namespace priceapp.Events.Models;

public class ConnectionErrorArgs : EventArgs
{
    public bool Success { get; set; }
    public int StatusCode { get; set; }
    public string Message { get; set; }
}
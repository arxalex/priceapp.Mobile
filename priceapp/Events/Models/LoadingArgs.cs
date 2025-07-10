namespace priceapp.Events.Models;

public class LoadingArgs : EventArgs
{
    public bool Success { get; set; }
    public int Total { get; set; }
    public int LoadedCount { get; set; }
    public string Message { get; set; }
}
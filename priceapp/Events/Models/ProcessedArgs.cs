namespace priceapp.Events.Models;

public class ProcessedArgs : EventArgs
{
    public bool Success { get; set; }
    public string Message { get; set; }
}
namespace priceapp.Repositories.Models;

public class ErrorResponseModel
{
    public bool status { get; set; }
    public string message { get; set; }
    public string code { get; set; }
}
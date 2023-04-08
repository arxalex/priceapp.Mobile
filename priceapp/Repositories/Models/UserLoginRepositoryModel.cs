namespace priceapp.Repositories.Models;

public class UserLoginRepositoryModel
{
    public bool status { get; set; }
    public int role { get; set; }
    public string? token { get; set; }
}
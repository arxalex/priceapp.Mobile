namespace priceapp.Repositories.Models;

public class UserLoginRepositoryModel
{
    public bool Status { get; set; }
    public int Role { get; set; }
    public string? Token { get; set; }
}
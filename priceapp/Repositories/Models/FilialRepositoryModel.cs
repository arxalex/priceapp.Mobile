// ReSharper disable InconsistentNaming
// ReSharper disable IdentifierTypo
namespace priceapp.Repositories.Models;

public class FilialRepositoryModel
{
    public int id { get; set; }
    public int shopId { get; set; }
    public string city { get; set; }
    public string region { get; set; }
    public string street { get; set; }
    public string house { get; set; }
    public double xCord { get; set; }
    public double yCord { get; set; }
    public string label { get; set; }
}
namespace priceapp.Models;

public class Filial
{
    public int Id { get; set; }
    public Shop Shop { get; set; }
    public string City { get; set; }
    public string Street { get; set; }
    public string Region { get; set; }
    public string House { get; set; }
    public double XCord { get; set; }
    public double YCord { get; set; }
    public string Label { get; set; }
    public string Address => City + ", " + Street + ", " + House;
}
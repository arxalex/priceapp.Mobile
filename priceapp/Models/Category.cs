namespace priceapp.Models
{
    public class Category
    {
        public string Label { get; set; }
        public string Image { get; set; }
        public int Id { get; set; }
        public int? Parent { get; set; }
        public bool IsFilter { get; set; }
    }
}
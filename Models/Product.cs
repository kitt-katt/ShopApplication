namespace ShopApplication.Models
{
    public class Product
    {
        public string Name { get; set; } // Уникальное имя
        public ICollection<Stock> Stocks { get; set; } = new List<Stock>();
    }
}

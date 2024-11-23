namespace ShopApplication.Models
{
    public class Shop
    {
        public string Code { get; set; } // Уникальный код
        public string Name { get; set; }
        public string Address { get; set; }
        public ICollection<Stock> Stocks { get; set; } = new List<Stock>();
    }
}

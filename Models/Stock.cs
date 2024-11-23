namespace ShopApplication.Models
{
    public class Stock
    {
        public int Id { get; set; }
        public string ShopCode { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        public Shop Shop { get; set; }
        public Product Product { get; set; }
    }
}

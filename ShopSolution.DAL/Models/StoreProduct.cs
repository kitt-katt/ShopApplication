namespace ShopSolution.DAL.Models
{
    public class StoreProduct
    {
        public int StoreId { get; set; }
        public Store Store { get; set; } = null!;
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}

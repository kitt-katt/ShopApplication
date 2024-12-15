namespace ShopSolution.DAL.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public List<StoreProduct> StoreProducts { get; set; } = new();
    }
}

namespace ShopSolution.DAL.Models
{
    public class Store
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;

        public List<StoreProduct> StoreProducts { get; set; } = new();
    }
}

namespace ShopSolution.DAL.Repositories
{
    public class StoreProductInfo
    {
        public int StoreId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }

    public class StoreProductPriceInfo
    {
        public int StoreId { get; set; }
        public decimal Price { get; set; }
    }

    public interface IStoreProductRepository
    {
        Task UpsertAsync(int storeId, int productId, decimal price, int quantity);
        Task UpdateQuantityAsync(int storeId, int productId, int newQuantity);
        Task<List<StoreProductPriceInfo>> GetStoresWithProductAsync(int productId);
        Task<List<StoreProductInfo>> GetProductsInStoreAsync(int storeId);
        Task<StoreProductInfo?> GetAsync(int storeId, int productId);
    }
}

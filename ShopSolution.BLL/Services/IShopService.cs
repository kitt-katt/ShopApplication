using ShopSolution.BLL.DTO;

namespace ShopSolution.BLL.Services
{
    public interface IShopService
    {
        Task CreateStoreAsync(string code, string name, string address);
        Task CreateProductAsync(string productName);
        Task AddProductsToStoreAsync(string storeCode, List<PurchaseItemDTO> items);
        Task<StoreDTO?> FindCheapestStoreForProductAsync(string productName);
        Task<List<PurchaseItemDTO>> GetProductsAffordableAsync(string storeCode, decimal budget);
        Task<decimal?> BuyProductsAsync(string storeCode, List<PurchaseItemDTO> items);
        Task<StoreDTO?> FindStoreForBulkPurchaseAsync(List<PurchaseItemDTO> items);
    }
}

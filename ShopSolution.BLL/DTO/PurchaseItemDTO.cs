namespace ShopSolution.BLL.DTO
{
    public class PurchaseItemDTO
    {
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; } // Price используется при завозе партии
    }
}

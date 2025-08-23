namespace LegacyOrderService.Services.Contracts
{
    public interface IOrderService
    {
        Task<int> CreateOrderAsync(string customerName, string productName, int quantity, double price, CancellationToken cancellationToken = default);
    }
}

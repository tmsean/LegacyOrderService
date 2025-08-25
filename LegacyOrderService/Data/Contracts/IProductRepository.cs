namespace LegacyOrderService.Data.Contracts
{
    public interface IProductRepository
    {
        Task<double> GetPriceAsync(string productName, CancellationToken cancellationToken = default);
        Task<List<string>> GetAllProductKeysAsync(CancellationToken cancellationToken = default);
    }
}

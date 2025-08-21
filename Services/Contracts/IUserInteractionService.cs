namespace LegacyOrderService.Services.Contracts
{
    public interface IUserInteractionService
    {
        Task<string> GetCustomerNameAsync();
        Task<string> SelectProductAsync(IReadOnlyList<string> products);
        Task<int> GetQuantityAsync();
        void ShowMessage(string message);
    }
}

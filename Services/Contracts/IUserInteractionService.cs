using LegacyOrderService.Models;

namespace LegacyOrderService.Services.Contracts
{
    public interface IUserInteractionService
    {
        Task<string> ReadCustomerNameAsync();
        Task<string> ReadProductChoiceAsync();
        Task<string> ReadQuantityAsync();
        void ShowMessage(string message);
        void ShowOrder(Order order);
        void ShowProducts(IReadOnlyList<string> products);
    }
}

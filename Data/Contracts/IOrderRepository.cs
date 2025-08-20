using LegacyOrderService.Models;

namespace LegacyOrderService.Data.Contracts
{
    public interface IOrderRepository
    {
        Task<int> CreateAsync(Order order, CancellationToken cancellationToken = default);
    }
}

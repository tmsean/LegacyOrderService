using LegacyOrderService.Models;

namespace LegacyOrderService.Data.Contracts
{
    public interface IOrderRepository
    {
        Task<int> CreateAsync(Order order, CancellationToken cancellationToken = default);
        Task<Order?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    }
}

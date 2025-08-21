using LegacyOrderService.Data.Contracts;
using LegacyOrderService.Models;
using LegacyOrderService.Services.Contracts;

namespace LegacyOrderService.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orders) => _orderRepository = orders;


        public Task<int> CreateOrderAsync(string customerName, string productName, int quantity, double price, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(customerName))
                throw new ArgumentException("customerName is required", nameof(customerName));

            if (string.IsNullOrWhiteSpace(productName))
                throw new ArgumentException("productName is required", nameof(productName));

            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);

            var order = new Order
            {
                CustomerName = customerName,
                ProductName = productName,
                Quantity = quantity,
                Price = price
            };

            return _orderRepository.CreateAsync(order, cancellationToken);
        }
    }
}

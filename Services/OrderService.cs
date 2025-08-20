using LegacyOrderService.Data;
using LegacyOrderService.Models;

namespace LegacyOrderService.Services
{
    public class OrderService
    {
        private readonly OrderRepository _orderRepository = new();

        
        public void CreateOrder(string customerName, string productName, int quantity, double price)
        {
            if (string.IsNullOrWhiteSpace(customerName)) throw new System.ArgumentException("customerName is required");
            if (string.IsNullOrWhiteSpace(productName)) throw new System.ArgumentException("productName is required");
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);

            var order = new Order
            {
                CustomerName = customerName,
                ProductName = productName,
                Quantity = quantity,
                Price = price
            };

            _orderRepository.Save(order);
        }
    }
}

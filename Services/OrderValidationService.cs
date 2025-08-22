using LegacyOrderService.Services.Contracts;

namespace LegacyOrderService.Services
{
    public class OrderValidationService : IOrderValidationService
    {
        public void ValidateCustomerName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Customer name cannot be empty.");
        }

        public void ValidateQuantity(int qty)
        {
            if (qty <= 0)
                throw new ArgumentException("Quantity must be a positive integer.");
        }

        public void ValidateProduct(string product, IReadOnlyList<string> products)
        {
            if (!products.Contains(product))
                throw new ArgumentException($"Invalid product: {product}");
        }
    }
}

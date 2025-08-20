// Data/ProductRepository.cs
using LegacyOrderService.Data.Contracts;

namespace LegacyOrderService.Data
{
    public class ProductRepository : IProductRepository
    {
        private readonly Dictionary<string, double> _productPrices = new()
        {
            ["Widget"] = 12.99,
            ["Gadget"] = 15.49,
            ["Doohickey"] = 8.75
        };

        public async Task<double> GetPriceAsync(string productName, CancellationToken cancellationToken = default)
        {
            // Simulate an expensive lookup
            await Task.Delay(500, cancellationToken);

            if (_productPrices.TryGetValue(productName, out var price))
                return price;

            throw new Exception("Product not found");
        }

        public async Task<List<string>> GetAllProductsAsync()
        {
            var result = _productPrices.Select(p => p.Key).ToList();
            return await Task.FromResult(result);
        }
    }
}

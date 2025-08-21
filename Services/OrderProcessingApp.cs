using LegacyOrderService.Data.Contracts;
using LegacyOrderService.Services.Contracts;
using LegacyOrderService.Utilities;

namespace LegacyOrderService.Services
{
    public class OrderProcessingApp
    {
        private readonly IProductRepository _productRepository;
        private readonly IOrderService _orderService;

        public OrderProcessingApp(IProductRepository productRepository, IOrderService orderService)
        {
            _productRepository = productRepository;
            _orderService = orderService;
        }

        public async Task RunAsync()
        {
            Console.WriteLine("Welcome to Order Processor!");

            var customerName = ConsolePrompts.ReadNonEmpty("Enter customer name (cannot be empty): ");

            var products = await _productRepository.GetAllProductsAsync();
            var productName = ConsolePrompts.SelectFromList("Available products:", products);
            var price = await _productRepository.GetPriceAsync(productName);

            var qty = ConsolePrompts.ReadPositiveInt("Enter quantity (must be a positive integer): ");

            Console.WriteLine("Processing order...");

            var total = qty * price;

            Console.WriteLine("Order complete!");
            Console.WriteLine("Customer: " + customerName);
            Console.WriteLine("Product: " + productName);
            Console.WriteLine("Quantity: " + qty);
            Console.WriteLine("Total: $" + total);

            Console.WriteLine("Saving order to database...");
            await _orderService.CreateOrderAsync(customerName, productName, qty, price);
            Console.WriteLine("Done.");
        }
    }
}

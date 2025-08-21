using LegacyOrderService.Data;
using LegacyOrderService.Data.Contracts;
using LegacyOrderService.Services;
using LegacyOrderService.Services.Contracts;
using LegacyOrderService.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LegacyOrderService
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using var host = Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    services.AddSingleton<IOrderRepository, OrderRepository>();
                    services.AddSingleton<IOrderService, OrderService>();
                    services.AddSingleton<IProductRepository, ProductRepository>();
                })
                .Build();

            var productRepository = host.Services.GetRequiredService<IProductRepository>();
            var orderService = host.Services.GetRequiredService<IOrderService>();

            Console.WriteLine("Welcome to Order Processor!");

            string customerName = ConsolePrompts.ReadNonEmpty("Enter customer name (cannot be empty): ");

            var products = await productRepository.GetAllProductsAsync();
            string productName = ConsolePrompts.SelectFromList("Available products:", products);
            var price = await productRepository.GetPriceAsync(productName);

            var qty = ConsolePrompts.ReadPositiveInt("Enter quantity (must be a positive integer): ");

            Console.WriteLine("Processing order...");

            double total = qty * price;

            Console.WriteLine("Order complete!");
            Console.WriteLine("Customer: " + customerName);
            Console.WriteLine("Product: " + productName);
            Console.WriteLine("Quantity: " + qty);
            Console.WriteLine("Total: $" + total);

            Console.WriteLine("Saving order to database...");
            await orderService.CreateOrderAsync(customerName, productName, qty, price);
            Console.WriteLine("Done.");
        }
    }
}

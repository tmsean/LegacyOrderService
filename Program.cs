using LegacyOrderService.Data;
using LegacyOrderService.Data.Contracts;
using LegacyOrderService.Services;
using LegacyOrderService.Services.Contracts;
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

            string customerName;
            do
            {
                Console.Write("Enter customer name (cannot be empty): ");
                customerName = Console.ReadLine()?.Trim() ?? "";
            } while (string.IsNullOrWhiteSpace(customerName));

            var products = await productRepository.GetAllProductsAsync();

            string productName;
            double price;
            while (true)
            {
                Console.WriteLine("Available products:");
                for (int i = 0; i < products.Count; i++)
                    Console.WriteLine($"{i + 1}. {products[i]}");

                Console.Write("Select a product by number: ");
                var input = Console.ReadLine();
                if (int.TryParse(input, out int choice) && choice >= 1 && choice <= products.Count)
                {
                    productName = products[choice - 1];
                    price = await productRepository.GetPriceAsync(productName);
                    break;
                }
                Console.WriteLine("Invalid selection, please try again.");
            }

            int qty;
            while (true)
            {
                Console.Write("Enter quantity (must be a positive integer): ");
                var input = Console.ReadLine();
                if (int.TryParse(input, out qty) && qty > 0) break;
                Console.WriteLine("Invalid quantity, please try again.");
            }

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

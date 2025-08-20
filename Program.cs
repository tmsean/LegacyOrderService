using LegacyOrderService.Data;
using LegacyOrderService.Services;

namespace LegacyOrderService
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Welcome to Order Processor!");

            // Customer name
            string customerName;
            do
            {
                Console.Write("Enter customer name (cannot be empty): ");
                customerName = Console.ReadLine()?.Trim() ?? "";
            } while (string.IsNullOrWhiteSpace(customerName));

            // Product name
            string productName;
            var productRepo = new ProductRepository();
            var products = productRepo.GetAllProducts();
            double price;
            while (true)
            {
                Console.WriteLine("Available products:");
                for (int i = 0; i < products.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {products[i]}");
                }

                Console.Write("Select a product by number: ");
                var input = Console.ReadLine();
                if (int.TryParse(input, out int choice) && choice >= 1 && choice <= products.Count)
                {
                    productName = products[choice - 1];
                    price = productRepo.GetPrice(productName);
                    break;
                }

                Console.WriteLine("Invalid selection, please try again.");
            }

            // Quantity
            int qty;
            while (true)
            {
                Console.Write("Enter quantity (must be a positive integer): ");
                var input = Console.ReadLine();
                if (int.TryParse(input, out qty) && qty > 0)
                    break;
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
            var orderRepo = new OrderRepository();
            var service = new OrderService(orderRepo);
            await service.CreateOrderAsync(customerName, productName, qty, price);
            Console.WriteLine("Done.");
        }
    }
}

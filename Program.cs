using LegacyOrderService.Data;
using LegacyOrderService.Services;

namespace LegacyOrderService
{
    class Program
    {
        static void Main(string[] args)
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
            do
            {
                Console.Write("Enter product name (cannot be empty): ");
                productName = Console.ReadLine()?.Trim() ?? "";
            } while (string.IsNullOrWhiteSpace(productName));

            var productRepo = new ProductRepository();
            double price = productRepo.GetPrice(productName);

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

            double total = qty * 10.0;

            Console.WriteLine("Order complete!");
            Console.WriteLine("Customer: " + customerName);
            Console.WriteLine("Product: " + productName);
            Console.WriteLine("Quantity: " + qty);
            Console.WriteLine("Total: $" + total);

            Console.WriteLine("Saving order to database...");
            var service = new OrderService();
            service.CreateOrder(customerName, productName, qty, price);
            Console.WriteLine("Done.");
        }
    }
}

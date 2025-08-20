using System;
using LegacyOrderService.Models;
using LegacyOrderService.Data;

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

            Order order = new Order();
            order.CustomerName = name;
            order.ProductName = product;
            order.Quantity = qty;
            order.Price = 10.0;

            double total = order.Quantity * order.Price;

            Console.WriteLine("Order complete!");
            Console.WriteLine("Customer: " + order.CustomerName);
            Console.WriteLine("Product: " + order.ProductName);
            Console.WriteLine("Quantity: " + order.Quantity);
            Console.WriteLine("Total: $" + price);

            Console.WriteLine("Saving order to database...");
            var repo = new OrderRepository();
            repo.Save(order);
            Console.WriteLine("Done.");
        }
    }
}

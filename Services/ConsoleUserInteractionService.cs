using LegacyOrderService.Models;
using LegacyOrderService.Services.Contracts;

namespace LegacyOrderService.Services
{
    public class ConsoleUserInteractionService : IUserInteractionService
    {
        public Task<string> ReadCustomerNameAsync()
        {
            Console.Write("Enter customer name (cannot be empty): ");
            return Task.FromResult(Console.ReadLine()?.Trim() ?? "");
        }

        public Task<string> ReadProductChoiceAsync()
        {
            Console.Write("Select a product by number (must be the one from the list): ");
            return Task.FromResult(Console.ReadLine()?.Trim() ?? "");
        }
        public Task<string> ReadQuantityAsync()
        {
            Console.Write("Enter quantity (must be a positive integer): ");
            return Task.FromResult(Console.ReadLine()?.Trim() ?? "");
        }

        public void ShowMessage(string message) => Console.WriteLine(message);

        public void ShowOrder(Order order)
        {
            var total = order.Quantity * order.Price;
            ShowMessage($"Id: {order.Id}");
            ShowMessage($"Customer: {order.CustomerName}");
            ShowMessage($"Product: {order.ProductName}");
            ShowMessage($"Quantity: {order.Quantity}");
            ShowMessage($"Total: ${total}");
        }

        public void ShowProducts(IReadOnlyList<string> products)
        {
            Console.WriteLine("Available products:");
            for (int i = 0; i < products.Count; i++)
                Console.WriteLine($"{i + 1}. {products[i]}");
        }
    }
}

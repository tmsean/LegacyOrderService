using LegacyOrderService.Services.Contracts;

namespace LegacyOrderService.Services
{
    public class ConsoleUserInteractionService : IUserInteractionService
    {
        public Task<string> GetCustomerNameAsync()
        {
            string customerName;
            do
            {
                Console.Write("Enter customer name (cannot be empty): ");
                customerName = Console.ReadLine()?.Trim() ?? "";
            } while (string.IsNullOrWhiteSpace(customerName));

            return Task.FromResult(customerName);
        }

        public Task<string> SelectProductAsync(IReadOnlyList<string> products)
        {
            while (true)
            {
                Console.WriteLine("Available products:");
                for (int i = 0; i < products.Count; i++)
                    Console.WriteLine($"{i + 1}. {products[i]}");

                Console.Write("Select a product by number: ");
                var input = Console.ReadLine();
                if (int.TryParse(input, out int choice) && choice >= 1 && choice <= products.Count)
                    return Task.FromResult(products[choice - 1]);

                Console.WriteLine("Invalid selection, please try again.");
            }
        }

        public Task<int> GetQuantityAsync()
        {
            while (true)
            {
                Console.Write("Enter quantity (must be a positive integer): ");
                var input = Console.ReadLine();
                if (int.TryParse(input, out int qty) && qty > 0)
                    return Task.FromResult(qty);

                Console.WriteLine("Invalid quantity, please try again.");
            }
        }

        public void ShowMessage(string message) => Console.WriteLine(message);
    }
}

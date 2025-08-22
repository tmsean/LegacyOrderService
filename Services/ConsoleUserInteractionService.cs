using LegacyOrderService.Services.Contracts;

namespace LegacyOrderService.Services
{
    public class ConsoleUserInteractionService : IUserInteractionService
    {
        private readonly IOrderValidationService _validator;

        public ConsoleUserInteractionService(IOrderValidationService validator)
        {
            _validator = validator;
        }

        public Task<string> GetCustomerNameAsync()
        {
            while (true)
            {
                Console.Write("Enter customer name (cannot be empty): ");
                var customerName = Console.ReadLine()?.Trim() ?? "";

                try
                {
                    _validator.ValidateCustomerName(customerName);
                    return Task.FromResult(customerName);
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
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
                {
                    var selected = products[choice - 1];
                    try
                    {
                        _validator.ValidateProduct(selected, products);
                        return Task.FromResult(selected);
                    }
                    catch (ArgumentException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                else
                {
                    Console.WriteLine("Invalid selection, please try again.");
                }
            }
        }

        public Task<int> GetQuantityAsync()
        {
            while (true)
            {
                Console.Write("Enter quantity (must be a positive integer): ");
                var input = Console.ReadLine();

                if (int.TryParse(input, out int qty))
                {
                    try
                    {
                        _validator.ValidateQuantity(qty);
                        return Task.FromResult(qty);
                    }
                    catch (ArgumentException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                else
                {
                    Console.WriteLine("Invalid number, please try again.");
                }
            }
        }

        public void ShowMessage(string message) => Console.WriteLine(message);
    }
}

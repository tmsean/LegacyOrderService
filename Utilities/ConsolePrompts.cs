namespace LegacyOrderService.Utilities
{
    public static class ConsolePrompts
    {
        public static string ReadNonEmpty(string prompt)
        {
            string? value;
            do
            {
                Console.Write(prompt);
                value = Console.ReadLine()?.Trim();
            } while (string.IsNullOrWhiteSpace(value));
            return value!;
        }

        public static int ReadPositiveInt(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                var input = Console.ReadLine();
                if (int.TryParse(input, out var val) && val > 0) return val;
                Console.WriteLine("Invalid quantity, please try again.");
            }
        }

        public static string SelectFromList(string header, IReadOnlyList<string> items)
        {
            while (true)
            {
                Console.WriteLine(header);
                for (int i = 0; i < items.Count; i++)
                    Console.WriteLine($"{i + 1}. {items[i]}");

                Console.Write("Select a product by number: ");
                var input = Console.ReadLine();
                if (int.TryParse(input, out int choice) && choice >= 1 && choice <= items.Count)
                    return items[choice - 1];

                Console.WriteLine("Invalid selection, please try again.");
            }
        }
    }
}

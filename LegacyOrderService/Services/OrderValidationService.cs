public class OrderValidationService : IOrderValidationService
{
    public void ValidateCustomerName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Customer name cannot be empty.");
    }

    public int ParseAndValidateQuantity(string qtyInput)
    {
        if (!int.TryParse(qtyInput, out var qty) || qty <= 0)
            throw new ArgumentException("Quantity must be a positive integer.");
        return qty;
    }

    public int ParseAndValidateProductIndex(string choiceInput, int optionsCount)
    {
        if (!int.TryParse(choiceInput, out var idx) || idx < 1 || idx > optionsCount)
            throw new ArgumentException("Invalid selection.");
        return idx;
    }
}

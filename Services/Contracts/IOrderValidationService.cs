namespace LegacyOrderService.Services.Contracts
{
    public interface IOrderValidationService
    {
        void ValidateCustomerName(string name);
        int ParseAndValidateQuantity(string qtyInput);
        string ParseAndValidateProductChoice(string choiceInput, IReadOnlyList<string> products);
    }
}

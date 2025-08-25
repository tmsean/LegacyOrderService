public interface IOrderValidationService
{
    void ValidateCustomerName(string name);
    int ParseAndValidateQuantity(string qtyInput);
    int ParseAndValidateProductIndex(string choiceInput, int optionsCount);
}
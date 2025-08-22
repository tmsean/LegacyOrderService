namespace LegacyOrderService.Services.Contracts
{
    public interface IOrderValidationService
    {
        void ValidateCustomerName(string name);
        void ValidateQuantity(int qty);
        void ValidateProduct(string product, IReadOnlyList<string> products);
    }
}

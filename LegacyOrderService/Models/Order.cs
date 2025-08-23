namespace LegacyOrderService.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string CustomerName { get; set; } = String.Empty;
        public string ProductName { get; set; } = String.Empty;
        public int Quantity { get; set; }
        public double Price { get; set; }
        public double Total => Price * Quantity;
    }
}

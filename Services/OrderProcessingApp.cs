using LegacyOrderService.Data.Contracts;
using LegacyOrderService.Services.Contracts;
using LegacyOrderService.Utilities;

namespace LegacyOrderService.Services
{
    public class OrderProcessingApp
    {
        private readonly IOrderService _orderService;
        private readonly IProductRepository _productRepository;
        private readonly IUserInteractionService _ui;

        public OrderProcessingApp(
            IOrderService orderService,
            IProductRepository productRepository,
            IUserInteractionService ui)
        {
            _orderService = orderService;
            _productRepository = productRepository;
            _ui = ui;
        }

        public async Task RunAsync()
        {
            _ui.ShowMessage("Welcome to Order Processor!");

            var customerName = await _ui.GetCustomerNameAsync();

            var products = await _productRepository.GetAllProductsAsync();
            var productName = await _ui.SelectProductAsync(products);

            var price = await _productRepository.GetPriceAsync(productName);
            var qty = await _ui.GetQuantityAsync();

            _ui.ShowMessage("Processing order...");

            double total = qty * price;

            _ui.ShowMessage("Order complete!");
            _ui.ShowMessage($"Customer: {customerName}");
            _ui.ShowMessage($"Product: {productName}");
            _ui.ShowMessage($"Quantity: {qty}");
            _ui.ShowMessage($"Total: ${total}");

            _ui.ShowMessage("Saving order to database...");
            await _orderService.CreateOrderAsync(customerName, productName, qty, price);
            _ui.ShowMessage("Done.");
        }
    }
}

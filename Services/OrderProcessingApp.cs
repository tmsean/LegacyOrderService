using LegacyOrderService.Data;
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
        private readonly IOrderRepository _orderRepository;

        public OrderProcessingApp(
            IOrderService orderService,
            IProductRepository productRepository,
            IUserInteractionService ui,
            IOrderRepository orderRepository)
        {
            _orderService = orderService;
            _productRepository = productRepository;
            _ui = ui;
            _orderRepository = orderRepository;
        }

        public async Task RunAsync()
        {
            _ui.ShowMessage("Welcome to Order Processor!");

            var customerName = await _ui.GetCustomerNameAsync();
            var products = await _productRepository.GetAllProductsAsync();
            var productName = await _ui.SelectProductAsync(products);
            var price = await _productRepository.GetPriceAsync(productName);
            var qty = await _ui.GetQuantityAsync();

            _ui.ShowMessage("Saving order to database...");
            var newId = await _orderService.CreateOrderAsync(customerName, productName, qty, price);

            var created = await _orderRepository.GetByIdAsync(newId);
            if (created is null)
            {
                _ui.ShowMessage("Error: created order not found.");
                return;
            }

            _ui.ShowMessage("Order complete!");
            _ui.ShowOrder(created);
            _ui.ShowMessage("Done.");
        }
    }
}

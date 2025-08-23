using LegacyOrderService.Data.Contracts;
using LegacyOrderService.Services.Contracts;

namespace LegacyOrderService.Services
{
    public class OrderProcessingApp
    {
        private readonly IOrderService _orderService;
        private readonly IProductRepository _productRepository;
        private readonly IUserInteractionService _ui;
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderValidationService _validator;

        public OrderProcessingApp(
            IOrderService orderService,
            IProductRepository productRepository,
            IUserInteractionService ui,
            IOrderRepository orderRepository,
            IOrderValidationService validator)
        {
            _orderService = orderService;
            _productRepository = productRepository;
            _ui = ui;
            _orderRepository = orderRepository;
            _validator = validator;
        }

        public async Task RunAsync()
        {
            try
            {
                _ui.ShowMessage("Welcome to Order Processor!");

                string customerName;
                while (true)
                {
                    var input = await _ui.ReadCustomerNameAsync();
                    try { _validator.ValidateCustomerName(input); customerName = input; break; }
                    catch (ArgumentException ex) { _ui.ShowMessage(ex.Message); }
                }

                var products = await _productRepository.GetAllProductsAsync();
                _ui.ShowProducts(products);

                string productName;
                while (true)
                {
                    var choice = await _ui.ReadProductChoiceAsync();
                    try { productName = _validator.ParseAndValidateProductChoice(choice, products); break; }
                    catch (ArgumentException ex) { _ui.ShowMessage(ex.Message); }
                }

                var price = await _productRepository.GetPriceAsync(productName);

                int qty;
                while (true)
                {
                    var input = await _ui.ReadQuantityAsync();
                    try { qty = _validator.ParseAndValidateQuantity(input); break; }
                    catch (ArgumentException ex) { _ui.ShowMessage(ex.Message); }
                }

                _ui.ShowMessage("Saving order to database...");
                var newOrderId = await _orderService.CreateOrderAsync(customerName, productName, qty, price);
                var created = await _orderRepository.GetByIdAsync(newOrderId);
                if (created is null)
                {
                    _ui.ShowMessage("Error: created order not found.");
                    return;
                }

                _ui.ShowMessage("Order complete!");
                _ui.ShowOrder(created);
                _ui.ShowMessage("Done.");
            }
            catch (Exception ex)
            {
                _ui.ShowMessage(ex.Message);
            }
        }
    }
}

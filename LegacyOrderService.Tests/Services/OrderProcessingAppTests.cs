using LegacyOrderService.Data.Contracts;
using LegacyOrderService.Models;
using LegacyOrderService.Services;
using LegacyOrderService.Services.Contracts;
using Moq;

namespace LegacyOrderService.Tests.Services
{
    public class OrderProcessingAppTests
    {
        private readonly Mock<IOrderService> _mockOrderService;
        private readonly Mock<IProductRepository> _mockProductRepository;
        private readonly Mock<IUserInteractionService> _mockUserInteractionService;
        private readonly Mock<IOrderRepository> _mockOrderRepository;
        private readonly Mock<IOrderValidationService> _mockOrderValidationService;
        private readonly OrderProcessingApp _sut;

        public OrderProcessingAppTests()
        {
            _mockOrderService = new Mock<IOrderService>(MockBehavior.Strict);
            _mockProductRepository = new Mock<IProductRepository>(MockBehavior.Strict);
            _mockUserInteractionService = new Mock<IUserInteractionService>(MockBehavior.Strict);
            _mockOrderRepository = new Mock<IOrderRepository>(MockBehavior.Strict);
            _mockOrderValidationService = new Mock<IOrderValidationService>(MockBehavior.Strict);

            _sut = new OrderProcessingApp(
                _mockOrderService.Object,
                _mockProductRepository.Object,
                _mockUserInteractionService.Object,
                _mockOrderRepository.Object,
                _mockOrderValidationService.Object
            );
        }

        [Fact]
        public async Task RunAsync_HappyPath_WithRetries_ShouldCreateFetchAndDisplayOrder()
        {
            // arrange
            var products = new List<string> { "Widget", "Gadget" };
            var customerInputs = new Queue<string>(new[] { "  ", "Alice" });
            var productChoiceInputs = new Queue<string>(new[] { "99", "1" });
            var quantityInputs = new Queue<string>(new[] { "0", "3" });

            _mockUserInteractionService.Setup(u => u.ShowMessage(It.IsAny<string>()));
            _mockUserInteractionService
                .Setup(u => u.ReadCustomerNameAsync())
                .ReturnsAsync(() => customerInputs.Dequeue());
            _mockOrderValidationService
                .Setup(v => v.ValidateCustomerName(It.Is<string>(s => string.IsNullOrWhiteSpace(s))))
                .Throws<ArgumentException>();
            _mockOrderValidationService
                .Setup(v => v.ValidateCustomerName("Alice"));

            _mockProductRepository
                .Setup(p => p.GetAllProductKeysAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(products);
            _mockUserInteractionService
                .Setup(u => u.ShowProducts(products));

            _mockUserInteractionService
                .Setup(u => u.ReadProductChoiceAsync())
                .ReturnsAsync(() => productChoiceInputs.Dequeue());
            _mockOrderValidationService
                .Setup(v => v.ParseAndValidateProductChoice("99", products))
                .Throws<ArgumentException>();
            _mockOrderValidationService
                .Setup(v => v.ParseAndValidateProductChoice("1", products))
                .Returns("Widget");

            _mockProductRepository
                .Setup(p => p.GetPriceAsync("Widget", It.IsAny<CancellationToken>()))
                .ReturnsAsync(12.34);

            _mockUserInteractionService
                .Setup(u => u.ReadQuantityAsync())
                .ReturnsAsync(() => quantityInputs.Dequeue());
            _mockOrderValidationService
                .Setup(v => v.ParseAndValidateQuantity("0"))
                .Throws<ArgumentException>();
            _mockOrderValidationService
                .Setup(v => v.ParseAndValidateQuantity("3"))
                .Returns(3);

            _mockOrderService
                .Setup(s => s.CreateOrderAsync("Alice", "Widget", 3, 12.34, default))
                .ReturnsAsync(42);

            var created = new Order { Id = 42, CustomerName = "Alice", ProductName = "Widget", Quantity = 3, Price = 12.34 };
            _mockOrderRepository
                .Setup(r => r.GetByIdAsync(42, default))
                .ReturnsAsync(created);

            _mockUserInteractionService
                .Setup(u => u.ShowOrder(It.Is<Order>(o =>
                    o.Id == 42 &&
                    o.CustomerName == "Alice" &&
                    o.ProductName == "Widget" &&
                    o.Quantity == 3 &&
                    Math.Abs(o.Price - 12.34) < 0.0001)));

            // act
            await _sut.RunAsync();

            // assert
            _mockOrderService.Verify(s => s.CreateOrderAsync("Alice", "Widget", 3, 12.34, default), Times.Once);
            _mockOrderRepository.Verify(r => r.GetByIdAsync(42, default), Times.Once);
            _mockUserInteractionService.Verify(u => u.ShowOrder(It.Is<Order>(o => o.Id == 42)), Times.Once);
        }

        [Fact]
        public async Task RunAsync_WhenCreatedOrderNotFound_ShouldShowErrorAndNotDisplayOrder()
        {
            // arrange
            var products = new List<string> { "Widget" };

            _mockUserInteractionService.Setup(u => u.ShowMessage(It.IsAny<string>()));

            _mockUserInteractionService
                .Setup(u => u.ReadCustomerNameAsync())
                .ReturnsAsync("Bob");
            _mockOrderValidationService
                .Setup(v => v.ValidateCustomerName("Bob"));

            _mockProductRepository
                .Setup(p => p.GetAllProductKeysAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(products);
            _mockUserInteractionService
                .Setup(u => u.ShowProducts(products));

            _mockUserInteractionService
                .Setup(u => u.ReadProductChoiceAsync())
                .ReturnsAsync("1");
            _mockOrderValidationService
                .Setup(v => v.ParseAndValidateProductChoice("1", products))
                .Returns("Widget");

            _mockProductRepository
                .Setup(p => p.GetPriceAsync("Widget", It.IsAny<CancellationToken>()))
                .ReturnsAsync(10.0);

            _mockUserInteractionService
                .Setup(u => u.ReadQuantityAsync())
                .ReturnsAsync("2");
            _mockOrderValidationService
                .Setup(v => v.ParseAndValidateQuantity("2"))
                .Returns(2);

            _mockOrderService
                .Setup(s => s.CreateOrderAsync("Bob", "Widget", 2, 10.0, default))
                .ReturnsAsync(100);
            _mockOrderRepository
                .Setup(r => r.GetByIdAsync(100, default))
                .ReturnsAsync((Order?)null);

            // act
            await _sut.RunAsync();

            // assert
            _mockUserInteractionService.Verify(u => u.ShowMessage(It.Is<string>(m => m.Contains("created order not found", StringComparison.OrdinalIgnoreCase))), Times.Once);
            _mockUserInteractionService.Verify(u => u.ShowOrder(It.IsAny<Order>()), Times.Never);
        }
    }
}

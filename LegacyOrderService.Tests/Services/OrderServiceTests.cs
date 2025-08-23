using LegacyOrderService.Data.Contracts;
using LegacyOrderService.Models;
using LegacyOrderService.Services;
using Moq;

namespace LegacyOrderService.Tests.Models.Services
{
    public class OrderServiceTests
    {
        [Fact]
        public async Task CreateOrderAsync_ReturnsId()
        {
            // arrange
            var repo = new Mock<IOrderRepository>();
            repo.Setup(r => r.CreateAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(123);

            var service = new OrderService(repo.Object);

            // act
            var id = await service.CreateOrderAsync("Alice", "Widget", 2, 9.99);

            // assert
            Assert.Equal(123, id);
            repo.Verify(r => r.CreateAsync(It.Is<Order>(o =>
                o.CustomerName == "Alice" &&
                o.ProductName == "Widget" &&
                o.Quantity == 2 &&
                Math.Abs(o.Price - 9.99) < 0.0001
            ), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
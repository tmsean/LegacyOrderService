using LegacyOrderService.Data;
using LegacyOrderService.Models;
using LegacyOrderService.Tests.Helpers;

namespace LegacyOrderService.Tests.Data
{
    public class OrderRepositoryTests
    {

        [Fact]
        public async Task CreateAsync_ReturnsId()
        {
            // arrange
            using var db = new SqliteTestDatabase();
            var repo = new OrderRepository(db.ConnectionString);

            // act
            var id1 = await repo.CreateAsync(new Order { CustomerName = "A", ProductName = "X", Quantity = 1, Price = 1.0 });

            // assert
            Assert.True(id1 > 0);
        }

        [Fact]
        public async Task GetByIdAsync_Returns()
        {
            // arrange
            using var db = new SqliteTestDatabase();
            var repo = new OrderRepository(db.ConnectionString);
            var order = new Order { CustomerName = "Alice", ProductName = "Widget", Quantity = 2, Price = 9.99 };
            var newId = await repo.CreateAsync(order);
            Assert.True(newId > 0);

            // act
            var fetched = await repo.GetByIdAsync(newId);

            // assert
            Assert.NotNull(fetched);
            Assert.Equal("Alice", fetched!.CustomerName);
            Assert.Equal("Widget", fetched.ProductName);
            Assert.Equal(2, fetched.Quantity);
            Assert.Equal(9.99, fetched.Price, 3);
        }

        [Fact]
        public async Task GetByIdAsync_NotFound_ReturnsNull()
        {
            // arrange
            using var db = new SqliteTestDatabase();
            var repo = new OrderRepository(db.ConnectionString);

            // act
            var result = await repo.GetByIdAsync(9999);

            // assert
            Assert.Null(result);
        }
    }
}

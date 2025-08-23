using LegacyOrderService.Models;

namespace LegacyOrderService.Tests.Models
{
    public class OrderModelTests
    {
        [Fact]
        public void Properties_CanBeSetAndRead()
        {
            // arrange
            var o = new Order();

            // act
            o.Id = 1;
            o.CustomerName = "Alice";
            o.ProductName = "Widget";
            o.Quantity = 3;
            o.Price = 12.34;

            // assert
            Assert.Equal(1, o.Id);
            Assert.Equal("Alice", o.CustomerName);
            Assert.Equal("Widget", o.ProductName);
            Assert.Equal(3, o.Quantity);
            Assert.Equal(12.34, o.Price, 3);
        }
    }
}
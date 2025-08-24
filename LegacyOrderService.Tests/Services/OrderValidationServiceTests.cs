using LegacyOrderService.Services;

namespace LegacyOrderService.Tests.Services
{
    public class OrderValidationServiceTests
    {
        [Theory]
        [InlineData("Alice")]
        [InlineData("  John  ")]
        public void ValidateCustomerName_Valid_DoesNotThrow(string name)
        {
            // arrange
            var sut = new OrderValidationService();

            // act
            var ex = Record.Exception(() => sut.ValidateCustomerName(name));

            // assert
            Assert.Null(ex);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void ValidateCustomerName_Invalid_Throws(string name)
        {
            // arrange
            var sut = new OrderValidationService();

            // act
            var ex = Assert.Throws<ArgumentException>(() => sut.ValidateCustomerName(name));

            // assert
            Assert.Equal("Customer name cannot be empty.", ex.Message);
        }

        [Theory]
        [InlineData("1", 1)]
        [InlineData("  5  ", 5)]
        public void ParseAndValidateQuantity_Valid_ReturnsInt(string input, int expected)
        {
            // arrange
            var sut = new OrderValidationService();

            // act
            var result = sut.ParseAndValidateQuantity(input);

            // assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("0")]
        [InlineData("-3")]
        [InlineData("abc")]
        [InlineData("1.5")]
        public void ParseAndValidateQuantity_Invalid_Throws(string input)
        {
            // arrange
            var sut = new OrderValidationService();

            // act
            var ex = Assert.Throws<ArgumentException>(() => sut.ParseAndValidateQuantity(input));

            // assert
            Assert.Equal("Quantity must be a positive integer.", ex.Message);
        }

        [Theory]
        [InlineData("1", "Widget")]
        [InlineData("2", "Gadget")]
        [InlineData(" 3 ", "Doohickey")]
        public void ParseAndValidateProductChoice_Valid_ReturnsProduct(string input, string expected)
        {
            // arrange
            var sut = new OrderValidationService();
            IReadOnlyList<string> products = new List<string> { "Widget", "Gadget", "Doohickey" };

            // act
            var productIndex = sut.ParseAndValidateProductIndex(input, products.Count);

            // assert
            Assert.Equal(expected, products[productIndex]);
        }

        [Theory]
        [InlineData("0")]
        [InlineData("4")]
        [InlineData("-1")]
        [InlineData("abc")]
        [InlineData("1.2")]
        public void ParseAndValidateProductChoice_Invalid_Throws(string input)
        {
            // arrange
            var sut = new OrderValidationService();
            IReadOnlyList<string> products = new List<string> { "Widget", "Gadget", "Doohickey" };

            // act
            var ex = Assert.Throws<ArgumentException>(() => sut.ParseAndValidateProductIndex(input, products.Count));

            // assert
            Assert.Equal("Invalid selection.", ex.Message);
        }
    }
}

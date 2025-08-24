using LegacyOrderService.Data;

namespace LegacyOrderService.Tests.Models.Data
{
    public class ProductRepositoryTests
    {
        [Fact]
        public async Task GetAllProductsAsync_ReturnsNonEmptyList()
        {
            // arrange
            var repo = new ProductRepository();

            // act
            var list = await repo.GetAllProductKeysAsync();

            // assert
            Assert.NotNull(list);
            Assert.NotEmpty(list);
        }

        [Theory]
        [InlineData("Widget")]
        [InlineData("Gadget")]
        [InlineData("Doohickey")]
        public async Task GetPriceAsync_KnownProducts_ReturnsPrice(string name)
        {
            // arrange
            var repo = new ProductRepository();

            // act
            var price = await repo.GetPriceAsync(name);

            // assert
            Assert.True(price > 0);
        }

        [Fact]
        public async Task GetPriceAsync_UnknownProduct_Throws_WithMessage()
        {
            // arrange
            var repo = new ProductRepository();

            // act
            var ex = await Assert.ThrowsAsync<Exception>(async () => await repo.GetPriceAsync("UNKNOWN"));

            // assert
            Assert.Equal("Product not found", ex.Message);
        }
    }
}
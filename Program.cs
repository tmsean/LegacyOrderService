using LegacyOrderService.Data;
using LegacyOrderService.Data.Contracts;
using LegacyOrderService.Services;
using LegacyOrderService.Services.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LegacyOrderService
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using var host = Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    services.AddSingleton<IOrderRepository, OrderRepository>();
                    services.AddSingleton<IOrderService, OrderService>();
                    services.AddSingleton<IProductRepository, ProductRepository>();
                    services.AddSingleton<IUserInteractionService, ConsoleUserInteractionService>();
                    services.AddSingleton<OrderProcessingApp>();
                })
                .Build();

            var app = host.Services.GetRequiredService<OrderProcessingApp>();
            await app.RunAsync();
        }
    }
}

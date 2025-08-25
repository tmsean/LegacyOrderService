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
                    services.AddScoped<IOrderRepository, OrderRepository>();
                    services.AddScoped<IOrderService, OrderService>();
                    services.AddScoped<IProductRepository, ProductRepository>();

                    services.AddSingleton<IOrderValidationService, OrderValidationService>();
                    services.AddSingleton<IUserInteractionService, ConsoleUserInteractionService>();

                    services.AddScoped<OrderProcessingApp>();
                })
                .Build();

            var app = host.Services.GetRequiredService<OrderProcessingApp>();
            await app.RunAsync();
        }
    }
}

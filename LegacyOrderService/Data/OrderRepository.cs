using LegacyOrderService.Data.Contracts;
using LegacyOrderService.Models;
using Microsoft.Data.Sqlite;

namespace LegacyOrderService.Data
{

    public class OrderRepository : IOrderRepository
    {
        private readonly string _connectionString;

        public OrderRepository(string? connectionString = null)
        {
            _connectionString = connectionString
                ?? $"Data Source={Path.Combine(AppContext.BaseDirectory, "orders.db")}";
        }

        public async Task<int> CreateAsync(Order order, CancellationToken cancellationToken = default)
        {
            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            using var command = connection.CreateCommand();
            command.CommandText = @"
                INSERT INTO Orders (CustomerName, ProductName, Quantity, Price)
                VALUES ($name, $product, $qty, $price);
                SELECT last_insert_rowid();";
            command.Parameters.Add("$name", SqliteType.Text).Value = order.CustomerName;
            command.Parameters.Add("$product", SqliteType.Text).Value = order.ProductName;
            command.Parameters.Add("$qty", SqliteType.Integer).Value = order.Quantity;
            command.Parameters.Add("$price", SqliteType.Real).Value = order.Price;

            var scalar = await command.ExecuteScalarAsync(cancellationToken);
            var id = (int)(long)(scalar ?? 0L);
            return id;
        }

        public async Task<Order?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            await using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            await using var cmd = connection.CreateCommand();
            cmd.CommandText = @"
                SELECT Id, CustomerName, ProductName, Quantity, Price
                FROM Orders
                WHERE Id = $id
                LIMIT 1;";
            cmd.Parameters.Add("$id", SqliteType.Integer).Value = id;

            await using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
            if (!await reader.ReadAsync(cancellationToken)) return null;

            return new Order
            {
                Id = reader.GetInt32(0),
                CustomerName = reader.GetString(1),
                ProductName = reader.GetString(2),
                Quantity = reader.GetInt32(3),
                Price = reader.GetDouble(4)
            };
        }


        //Not sure what is this method for. But I've decided to keep it
        public void SeedBadData()
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            using var cmd = connection.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO Orders (CustomerName, ProductName, Quantity, Price)
                VALUES ($name, $product, $qty, $price);";
            cmd.Parameters.Add("$name", SqliteType.Text).Value = "John";
            cmd.Parameters.Add("$product", SqliteType.Text).Value = "Widget";
            cmd.Parameters.Add("$qty", SqliteType.Integer).Value = 9999;
            cmd.Parameters.Add("$price", SqliteType.Real).Value = 9.99;
            cmd.ExecuteNonQuery();
        }
    }
}

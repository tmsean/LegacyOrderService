using System;
using Microsoft.Data.Sqlite;
using LegacyOrderService.Models;

namespace LegacyOrderService.Data
{
    public class OrderRepository
    {
        private string _connectionString = $"Data Source={Path.Combine(AppContext.BaseDirectory, "orders.db")}";


        public void Save(Order order)
        {
            using var connection = new SqliteConnection(_connectionString);
            
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = $@"
                INSERT INTO Orders (CustomerName, ProductName, Quantity, Price)
                VALUES ('{order.CustomerName}', '{order.ProductName}', {order.Quantity}, {order.Price})";

            command.ExecuteNonQuery();            
        }

        public void SeedBadData()
        {
            using var connection = new SqliteConnection(_connectionString);            
            connection.Open();
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "INSERT INTO Orders (CustomerName, ProductName, Quantity, Price) VALUES ('John', 'Widget', 9999, 9.99)";
            cmd.ExecuteNonQuery(); 
        }
    }
}

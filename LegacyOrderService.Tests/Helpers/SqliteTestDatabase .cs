using Microsoft.Data.Sqlite;

namespace LegacyOrderService.Tests.Helpers
{
    public class SqliteTestDatabase  : IDisposable
    {
        private readonly string _dbPath;
        public string ConnectionString { get; }

        public SqliteTestDatabase()
        {
            //An unique connection string for each SqliteTestDatabase instance
            _dbPath = Path.Combine(Path.GetTempPath(), $"orders_{Guid.NewGuid():N}.db");
            var builder = new SqliteConnectionStringBuilder
            {
                DataSource = _dbPath,
                Mode = SqliteOpenMode.ReadWriteCreate,
                Cache = SqliteCacheMode.Default
            };
            ConnectionString = builder.ToString();

            using var conn = new SqliteConnection(ConnectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                CREATE TABLE IF NOT EXISTS Orders (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    CustomerName TEXT,
                    ProductName  TEXT,
                    Quantity     INTEGER,
                    Price        REAL
                );";
            cmd.ExecuteNonQuery();
        }

        public void Dispose()
        {
            try { if (File.Exists(_dbPath)) File.Delete(_dbPath); } catch { /* ignore */ }
        }

    }
}

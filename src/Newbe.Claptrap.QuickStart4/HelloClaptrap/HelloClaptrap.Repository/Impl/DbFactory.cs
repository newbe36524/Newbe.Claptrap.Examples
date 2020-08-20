using System;
using System.Data;
using System.Data.SQLite;
using DbUp;
using DbUp.Engine;
using DbUp.SQLite.Helpers;
using Microsoft.Extensions.Options;

namespace HelloClaptrap.Repository.Impl
{
    public class DbFactory : IDbFactory
    {
        private readonly IOptions<DbOptions> _options;
        private readonly Lazy<bool> _initDb;

        public DbFactory(
            IOptions<DbOptions> options)
        {
            _options = options;
            _initDb = new Lazy<bool>(ValueFactory);
        }

        private bool ValueFactory()
        {
            using var sqliteConnection = new SQLiteConnection(_options.Value.OrderDbConnectionString);
            var db =
                DeployChanges.To
                    .SQLiteDatabase(new SharedConnection(sqliteConnection))
                    .WithScript(new SqlScript("createOrderTable", $@"
CREATE TABLE {OrderEntity.TableName} (
OrderId TEXT PRIMARY KEY,
UserId TEXT NOT NULL,
OrderJson TEXT NOT NULL
)"))
                    .LogToConsole()
                    .Build();

            db.PerformUpgrade();
            return true;
        }

        public IDbConnection GetOrderDb()
        {
            _ = _initDb.Value;
            return new SQLiteConnection(_options.Value.OrderDbConnectionString);
        }
    }
}
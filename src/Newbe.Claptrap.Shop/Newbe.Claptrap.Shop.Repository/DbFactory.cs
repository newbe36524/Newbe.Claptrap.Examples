using System.Data;
using Npgsql;

namespace Newbe.Claptrap.Shop.Repository
{
    public class DbFactory : IDbFactory
    {
        private readonly IDbConnectionStringFactory _dbConnectionStringFactory;

        public DbFactory(
            IDbConnectionStringFactory dbConnectionStringFactory)
        {
            _dbConnectionStringFactory = dbConnectionStringFactory;
        }

        public IDbConnection GetMainDb()
        {
            var connectionString = _dbConnectionStringFactory.GetMainDb();
            var re = new NpgsqlConnection(connectionString);
            return re;
        }
    }
}
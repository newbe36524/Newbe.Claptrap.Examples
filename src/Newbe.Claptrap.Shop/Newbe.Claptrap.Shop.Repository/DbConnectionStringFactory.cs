using Microsoft.Extensions.Options;

namespace Newbe.Claptrap.Shop.Repository
{
    public class DbConnectionStringFactory : IDbConnectionStringFactory
    {
        private readonly IOptions<DbOptions> _options;

        public DbConnectionStringFactory(
            IOptions<DbOptions> options)
        {
            _options = options;
        }

        public string GetMainDb()
        {
            var re = _options.Value.MainDb;
            return re;
        }
    }
}
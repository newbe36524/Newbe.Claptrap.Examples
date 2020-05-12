using System.Data;

namespace Newbe.Claptrap.Shop.Repository
{
    public interface IDbFactory
    {
        IDbConnection GetMainDb();
    }
}
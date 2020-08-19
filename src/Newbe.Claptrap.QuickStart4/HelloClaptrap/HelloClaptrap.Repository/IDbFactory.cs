using System.Data;

namespace HelloClaptrap.Repository
{
    public interface IDbFactory
    {
        IDbConnection GetOrderDb();
    }
}
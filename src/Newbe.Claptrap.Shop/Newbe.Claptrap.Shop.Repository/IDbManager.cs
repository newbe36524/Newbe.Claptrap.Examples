using System.Threading.Tasks;

namespace Newbe.Claptrap.Shop.Repository
{
    public interface IDbManager
    {
        Task InitAsync();
        Task RemoveAsync();
    }
}
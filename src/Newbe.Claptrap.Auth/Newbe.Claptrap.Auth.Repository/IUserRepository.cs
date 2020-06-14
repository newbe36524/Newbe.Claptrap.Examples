using System.Threading.Tasks;
using Newbe.Claptrap.Auth.Models;

namespace Newbe.Claptrap.Auth.Repository
{
    public interface IUserRepository
    {
        Task<UserInfoEntity> GetUserInfoAsync(string userId);

        Task<UserInfoEntity[]> GetUserInfosAsync(int pageIndex, int pageSize);
    }
}
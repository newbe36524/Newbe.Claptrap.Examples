using System.Threading.Tasks;
using Newbe.Claptrap.Auth.Models;
using Orleans;

namespace Newbe.Claptrap.Auth.IGrains
{
    [ClaptrapState(typeof(UserInfo), ClaptrapCodes.User)]
    [ClaptrapEvent(typeof(UserLoginEvent), ClaptrapCodes.UserLoginEvent)]
    public interface IUserGrain : IGrainWithStringKey
    {
        /// <summary>
        /// Login. return token if success or null.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<string> LoginAsync(string username, string password);

        /// <summary>
        /// validate token. return true if valid or false.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<bool> ValidateAsync(string token);
    }
}
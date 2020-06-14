using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Newbe.Claptrap.Auth.Models;

namespace Newbe.Claptrap.Auth.Repository
{
    public class UserRepository : IUserRepository
    {
        public Task<UserInfoEntity> GetUserInfoAsync(string userId)
        {
            return Task.FromResult(GetUserInfo(userId));
        }

        public Task<UserInfoEntity[]> GetUserInfosAsync(int pageIndex, int pageSize)
        {
            var re = Enumerable.Range(pageIndex * pageSize, pageSize)
                .Select(userId => GetUserInfo(userId.ToString()))
                .ToArray();
            return Task.FromResult(re);
        }

        private UserInfoEntity GetUserInfo(string userId)
        {
            var secret = GetSecret(userId);
            var re = new UserInfoEntity
            {
                UserId = userId,
                Secret = secret,
                Password = GetPassword(secret, userId + "pwd"),
                Username = $"User{userId}"
            };
            return re;
        }

        private string GetSecret(string userId)
            => GetMd5(userId);

        private string GetMd5(string source)
        {
            using var md5 = MD5.Create();
            var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(source));
            var re = string.Join("", hash.Select(x => x.ToString("x2")));
            return re;
        }

        private string GetPassword(string secret, string password)
        {
            using (var hmac = HMAC.Create())
            {
                hmac.Key = Encoding.UTF8.GetBytes(secret);
                hmac.Initialize();
                var pwdHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                var re = Convert.ToBase64String(pwdHash);
                return re;
            }
        }
    }
}
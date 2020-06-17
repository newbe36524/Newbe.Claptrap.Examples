using System;
using System.Security.Cryptography;
using System.Text;

namespace Newbe.Claptrap.Auth.Repository
{
    public static class PasswordHelper
    {
        public static string HashPassword(string secret, string password)
        {
            using var hmac = new HMACSHA512
            {
                Key = Encoding.UTF8.GetBytes(secret)
            };
            hmac.Initialize();
            var pwdHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            var re = Convert.ToBase64String(pwdHash);
            return re;
        }
    }
}
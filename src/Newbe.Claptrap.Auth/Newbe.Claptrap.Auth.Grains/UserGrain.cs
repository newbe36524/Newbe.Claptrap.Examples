using System;
using System.Threading.Tasks;
using JWT.Algorithms;
using JWT.Builder;
using Newbe.Claptrap.Auth.IGrains;
using Newbe.Claptrap.Auth.Models;
using Newbe.Claptrap.Auth.Repository;
using Newbe.Claptrap.Orleans;

namespace Newbe.Claptrap.Auth.Grains
{
    [ClaptrapStateInitialFactoryHandler(typeof(UserInfoInitFactory))]
    [ClaptrapEventHandler(typeof(UserLoginEventHandler), ClaptrapCodes.UserLoginEvent)]
    public class UserGrain : ClaptrapBoxGrain<UserInfo>, IUserGrain
    {
        public UserGrain(IClaptrapGrainCommonService claptrapGrainCommonService)
            : base(claptrapGrainCommonService)
        {
        }

        private string UserId => Claptrap.State.Identity.Id;

        public async Task<string> LoginAsync(string username, string password)
        {
            var userInfo = StateData;
            var hashPassword = PasswordHelper.HashPassword(userInfo.Secret,password);
            if (username != userInfo.Username || hashPassword != userInfo.Password)
            {
                return null;
            }

            var eventData = new UserLoginEvent();
            var evt = this.CreateEvent(eventData);
            await Claptrap.HandleEventAsync(evt);
            var jwt = CreateJwt(UserId, userInfo.Secret);
            return jwt;
        }

        public Task<bool> ValidateAsync(string token)
        {
            var userInfo = StateData;
            var re = ValidJwt(token, userInfo.Secret);
            return Task.FromResult(re);
        }

        private static string CreateJwt(string userId, string secret)
        {
            var builder = new JwtBuilder();
            var jwt = builder
                .WithAlgorithm(new HMACSHA512Algorithm())
                .WithSecret(secret)
                .WithVerifySignature(true)
                .AddClaim(ClaimName.Subject, userId)
                .ExpirationTime(DateTime.UtcNow + TimeSpan.FromMinutes(30))
                .Encode();
            return jwt;
        }

        private static bool ValidJwt(string jwt, string secret)
        {
            var builder = new JwtBuilder();
            try
            {
                builder
                    .WithAlgorithm(new HMACSHA512Algorithm())
                    .WithSecret(secret)
                    .WithVerifySignature(true)
                    .ExpirationTime(DateTime.UtcNow + TimeSpan.FromMinutes(30))
                    .Decode(jwt);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
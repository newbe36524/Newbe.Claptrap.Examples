using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newbe.Claptrap.Auth.Models;
using Newbe.Claptrap.Auth.Repository;

namespace Newbe.Claptrap.Auth.Grains
{
    public class UserInfoInitFactory : IInitialStateDataFactory
    {
        private readonly IUserRepository _userRepository;

        public UserInfoInitFactory(
            IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IStateData> Create(IClaptrapIdentity identity)
        {
            var userId = identity.Id;
            var userInfoEntity = await _userRepository.GetUserInfoAsync(userId);
            var userInfo = new UserInfo
            {
                Password = userInfoEntity.Password,
                Secret = userInfoEntity.Secret,
                Username = userInfoEntity.Username,
                LastLoginTimes = new Queue<DateTime>()
            };
            return userInfo;
        }
    }
}
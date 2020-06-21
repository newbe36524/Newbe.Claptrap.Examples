using System;
using System.Threading.Tasks;
using Newbe.Claptrap.Auth.Models;

namespace Newbe.Claptrap.Auth.Grains
{
    public class UserLoginEventHandler
        : NormalEventHandler<UserInfo, UserLoginEvent>
    {
        public override ValueTask HandleEvent(UserInfo stateData, UserLoginEvent eventData, IEventContext eventContext)
        {
            if (stateData.LastLoginTimes.Count == 10)
            {
                stateData.LastLoginTimes.Dequeue();
            }

            stateData.LastLoginTimes.Enqueue(DateTime.UtcNow);
            return new ValueTask();
        }
    }
}
using System;
using System.Collections.Generic;

namespace Newbe.Claptrap.Auth.Models
{
    public class UserInfo : IStateData
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Secret { get; set; }

        /// <summary>
        /// last login time. max 10 items
        /// </summary>
        public Queue<DateTime> LastLoginTimes { get; set; }
    }
}
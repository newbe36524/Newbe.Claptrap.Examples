namespace Newbe.Claptrap.Auth.Models
{
    public static class ClaptrapCodes
    {
        private const string Suffix = ".auth.claptrap.newbe";
        public const string User = "user" + Suffix;
        public const string UserLoginEvent = "user_login.events.user" + Suffix;
    }
}
using System.Threading.Tasks;
using Newbe.Claptrap.Box;

namespace Newbe.Claptrap.Standalone
{
    [ClaptrapEventHandler(typeof(ChangeAccountBalanceEventHandler), ClaptrapCodes.BalanceChangeEvent)]
    public class AccountClaptrap : NormalClaptrapBox, IAccountClaptrap
    {
        public new delegate AccountClaptrap Factory(IClaptrapIdentity identity);

        public AccountClaptrap(
            IClaptrapIdentity identity,
            IClaptrapFactory claptrapFactory) : base(identity,
            claptrapFactory)
        {
        }

        public Task ActivateAsync()
        {
            return Claptrap.ActivateAsync();
        }

        public Task ChangeBalanceAsync(decimal diff)
        {
            var accountStateData = (AccountStateData) Claptrap.State.Data;
            if (accountStateData.Balance + diff < 0)
            {
                return Task.CompletedTask;
            }

            var dataEvent = new DataEvent(
                Claptrap.State.Identity,
                ClaptrapCodes.BalanceChangeEvent,
                new ChangeAccountBalanceEventData
                {
                    Diff = diff,
                    NewBalance = accountStateData.Balance + diff,
                    OldBalance = accountStateData.Balance,
                });
            return Claptrap.HandleEventAsync(dataEvent);
        }

        public Task<decimal> GetBalanceAsync()
        {
            var accountStateData = (AccountStateData) Claptrap.State.Data;
            return Task.FromResult(accountStateData.Balance);
        }
    }
}
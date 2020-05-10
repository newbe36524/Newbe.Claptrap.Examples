using System;
using System.Threading.Tasks;

namespace Newbe.Claptrap.Standalone
{
    [ClaptrapStateInitialFactoryHandler]
    [ClaptrapEventHandler(typeof(ChangeAccountBalanceEventHandler), ClaptrapCodes.BalanceChangeEvent)]
    public class AccountClaptrap : IAccountClaptrap
    {
        public delegate AccountClaptrap Factory(IClaptrapIdentity identity);

        private readonly IClaptrapIdentity _identity;
        private readonly IClaptrap _claptrap;

        public AccountClaptrap(
            IClaptrapIdentity identity,
            IClaptrapFactory claptrapFactory)
        {
            _identity = identity;
            _claptrap = claptrapFactory.Create(identity);
        }

        public Task ActivateAsync()
        {
            return _claptrap.ActivateAsync();
        }

        public Task ChangeBalanceAsync(decimal diff)
        {
            var accountStateData = (AccountStateData) _claptrap.State.Data;
            if (accountStateData.Balance + diff < 0)
            {
                return Task.CompletedTask;
            }

            var dataEvent = new DataEvent(
                _identity,
                ClaptrapCodes.BalanceChangeEvent,
                new ChangeAccountBalanceEventData
                {
                    Diff = diff,
                    NewBalance = accountStateData.Balance + diff,
                    OldBalance = accountStateData.Balance,
                }, Guid.NewGuid().ToString());
            return _claptrap.HandleEventAsync(dataEvent);
        }

        public Task<decimal> GetBalanceAsync()
        {
            var accountStateData = (AccountStateData) _claptrap.State.Data;
            return Task.FromResult(accountStateData.Balance);
        }
    }
}
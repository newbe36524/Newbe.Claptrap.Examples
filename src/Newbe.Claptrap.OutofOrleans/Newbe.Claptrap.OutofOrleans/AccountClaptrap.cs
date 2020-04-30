using System;
using System.Threading.Tasks;
using Newbe.Claptrap.Preview.Abstractions.Core;
using Newbe.Claptrap.Preview.Attributes;
using Newbe.Claptrap.Preview.Impl;
using Newbe.Claptrap.Preview.StorageProvider.SQLite;

namespace Newbe.Claptrap.OutofOrleans
{
    [ClaptrapStateInitialFactoryHandler]
    [EventStore(typeof(SQLiteEventStoreFactory), typeof(SQLiteEventStoreFactory))]
    [StateStore(typeof(SQLiteStateStoreFactory), typeof(SQLiteStateStoreFactory))]
    [ClaptrapEventHandler(typeof(ChangeAccountBalanceEventHandler), typeof(ChangeAccountBalanceEventData))]
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
                typeof(ChangeAccountBalanceEventData).FullName!,
                new ChangeAccountBalanceEventData
                {
                    Diff = diff
                },
                Guid.NewGuid().ToString());
            return _claptrap.HandleEvent(dataEvent);
        }

        public Task<decimal> GetBalanceAsync()
        {
            var accountStateData = (AccountStateData) _claptrap.State.Data;
            return Task.FromResult(accountStateData.Balance);
        }
    }
}
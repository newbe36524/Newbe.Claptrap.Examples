using System.Threading.Tasks;
using Newbe.Claptrap.Preview.Abstractions.Components;
using Newbe.Claptrap.Preview.Abstractions.Core;

namespace Newbe.Claptrap.OutofOrleans
{
    public class ChangeAccountBalanceEventHandler : IEventHandler
    {
        public ValueTask DisposeAsync()
        {
            return new ValueTask();
        }

        public Task<IState> HandleEvent(IEventContext eventContext)
        {
            var accountStateData = (AccountStateData) eventContext.State.Data;
            var changeAccountBalanceEventData = (ChangeAccountBalanceEventData) eventContext.Event.Data;
            accountStateData.Balance += changeAccountBalanceEventData.Diff;
            return Task.FromResult(eventContext.State);
        }
    }
}
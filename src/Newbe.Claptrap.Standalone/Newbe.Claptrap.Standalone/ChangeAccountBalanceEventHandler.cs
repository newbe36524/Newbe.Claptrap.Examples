using System.Threading.Tasks;

namespace Newbe.Claptrap.Standalone
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
            accountStateData.Balance = changeAccountBalanceEventData.NewBalance;
            return Task.FromResult(eventContext.State);
        }
    }
}
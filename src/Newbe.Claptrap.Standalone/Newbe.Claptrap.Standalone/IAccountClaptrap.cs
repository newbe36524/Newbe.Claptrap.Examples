using System.Threading.Tasks;

namespace Newbe.Claptrap.Standalone
{
    [ClaptrapState(typeof(AccountStateData), ClaptrapCodes.Account)]
    [ClaptrapEvent(typeof(ChangeAccountBalanceEventData), ClaptrapCodes.BalanceChangeEvent)]
    public interface IAccountClaptrap
    {
        Task ActivateAsync();
        Task ChangeBalanceAsync(decimal diff);

        Task<decimal> GetBalanceAsync();
    }

    public class AccountStateData : IStateData
    {
        public decimal Balance { get; set; }
    }

    public class ChangeAccountBalanceEventData : IEventData
    {
        public decimal Diff { get; set; }
        public decimal NewBalance { get; set; }
        public decimal OldBalance { get; set; }
    }
}
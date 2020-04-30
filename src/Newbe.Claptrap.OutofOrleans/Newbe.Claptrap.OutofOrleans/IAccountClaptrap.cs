using System.Threading.Tasks;
using Newbe.Claptrap.Preview.Attributes;

namespace Newbe.Claptrap.OutofOrleans
{
    [ClaptrapState(typeof(AccountStateData))]
    [ClaptrapEvent(typeof(ChangeAccountBalanceEventData))]
    public interface IAccountClaptrap
    {
        Task ActivateAsync();
        Task ChangeBalanceAsync(decimal diff);

        Task<decimal> GetBalanceAsync();
    }
}
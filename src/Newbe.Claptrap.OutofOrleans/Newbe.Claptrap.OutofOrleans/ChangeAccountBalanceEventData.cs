using Newbe.Claptrap.Preview.Abstractions.Core;

namespace Newbe.Claptrap.OutofOrleans
{
    public class ChangeAccountBalanceEventData : IEventData
    {
        public decimal Diff { get; set; }
    }
}
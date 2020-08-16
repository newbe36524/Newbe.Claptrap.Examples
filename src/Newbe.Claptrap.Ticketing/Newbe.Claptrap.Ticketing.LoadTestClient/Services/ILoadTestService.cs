using System.Threading.Tasks;

namespace Newbe.Claptrap.Ticketing.LoadTestClient.Services
{
    public interface ILoadTestService
    {
        Task RunAsync();
    }
}
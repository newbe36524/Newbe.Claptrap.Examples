using System.Threading.Tasks;

namespace HelloClaptrap.Repository
{
    public interface IOrderRepository
    {
        Task SaveAsync(string userId, string orderId, string orderJson);
        Task<OrderEntity[]> GetAllAsync();
    }
}
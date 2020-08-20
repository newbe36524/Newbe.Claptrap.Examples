using System.Linq;
using System.Threading.Tasks;
using Dapper.Contrib.Extensions;

namespace HelloClaptrap.Repository.Impl
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IDbFactory _dbFactory;

        public OrderRepository(
            IDbFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public async Task SaveAsync(string userId, string orderId, string orderJson)
        {
            using var dbConnection = _dbFactory.GetOrderDb();
            await dbConnection.InsertAsync(new OrderEntity
            {
                OrderId = orderId,
                OrderJson = orderJson,
                UserId = userId
            });
        }

        public async Task<OrderEntity[]> GetAllAsync()
        {
            using var db = _dbFactory.GetOrderDb();
            var items = await db.GetAllAsync<OrderEntity>();
            var re = items.ToArray();
            return re;
        }
    }
}
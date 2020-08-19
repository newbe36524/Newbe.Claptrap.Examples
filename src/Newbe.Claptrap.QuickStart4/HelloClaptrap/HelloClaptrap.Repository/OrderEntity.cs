using Dapper.Contrib.Extensions;

namespace HelloClaptrap.Repository
{
    [Table(TableName)]
    public class OrderEntity
    {
        public const string TableName = "Orders";
        public string UserId { get; set; }
        [ExplicitKey] public string OrderId { get; set; }
        public string OrderJson { get; set; }
    }
}
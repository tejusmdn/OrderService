using CafeCommon;
using CafeCommon.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Serialization.HybridRow.Schemas;
using System.ComponentModel;
using Container = Microsoft.Azure.Cosmos.Container;
using PartitionKey = Microsoft.Azure.Cosmos.PartitionKey;

namespace OrderService.DataAccess
{
    public class OrderRepository : IOrderRepository
    {
        private readonly string? _endPoint;
        private readonly string? _primaryKey;
        private readonly string? _databaseName;
        private readonly CosmosClient _dbClient;
        private readonly Container orderContainer;

        public OrderRepository(IConfiguration configuration)
        {
            this._endPoint = configuration.GetConnectionString("EndPoint");
            this._primaryKey = configuration.GetConnectionString("PrimaryKey");
            this._databaseName = configuration.GetValue<string>("Database");
            
            this._dbClient = new CosmosClient(_endPoint, _primaryKey);
            this.orderContainer = this._dbClient.GetContainer(_databaseName, "orders");
            if (this.orderContainer == null)
            {
                var database = this._dbClient.GetDatabase(_databaseName);
                database.CreateContainerIfNotExistsAsync("orders", "partitionKey");
                this.orderContainer = this._dbClient.GetContainer(_databaseName, "orders");
            }
        }

        public Order? Get(string id)
        {
            var query = orderContainer.GetItemLinqQueryable<Order>(true);
            return query.Where(x => x.Id == id).ToList().FirstOrDefault();
        }

        public Order Add(Order order)
        {
            var itemResponse = orderContainer.CreateItemAsync(order, new PartitionKey(order.PartitionKey)).GetAwaiter()
                .GetResult();

            return itemResponse.Resource;
        }

        public Order Update(Order order)
        {
            var itemResponse = orderContainer.ReplaceItemAsync(order, order.Id, new PartitionKey(order.CreatedDate.ToShortDateString())).GetAwaiter()
                .GetResult();

            return itemResponse.Resource;
        }


        public IEnumerable<Order> GetAll()
        {
            var query = orderContainer.GetItemLinqQueryable<Order>(true);
            return query.Where(x => true).ToList();
        }
        
    }
}

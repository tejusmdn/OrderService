using CafeCommon;
using CafeCommon.Models;

namespace OrderService.DataAccess;

public interface IOrderRepository
{
    Order? Get(string id);
    Order Add(Order order);
    Order Update(Order order);
    IEnumerable<Order> GetAll();
}
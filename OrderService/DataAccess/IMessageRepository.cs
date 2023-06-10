using CafeCommon.Models;

namespace OrderService.DataAccess;

public interface IMessageRepository
{
    Task<bool> SendMessage(Order order);
}
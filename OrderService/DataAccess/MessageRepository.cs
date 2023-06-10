using System.Text.Json;
using Azure.Messaging.ServiceBus;
using CafeCommon.Models;

namespace OrderService.DataAccess
{
    public class MessageRepository : IMessageRepository
    {
        private readonly string? messageConnectionString;
        private readonly string? messageQueue;

        public MessageRepository(IConfiguration configuration)
        {
            this.messageConnectionString = configuration.GetConnectionString("MessageConnectionString");
            this.messageQueue = configuration.GetValue<string>("MessageQueue");
        }

        public async Task<bool> SendMessage(Order order)
        {
            try
            {
                await using var client = new ServiceBusClient(messageConnectionString);
                var sender = client.CreateSender(this.messageQueue);
                var textMsg = JsonSerializer.Serialize(order);
                var message = new ServiceBusMessage(textMsg);
                await sender.SendMessageAsync(message);
            }
            catch (Exception e)
            {
                return false;
            }
            
            return true;
        }
    }
}

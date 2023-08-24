using CafeCommon.Models;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace OrderService.DataAccess
{
    public class OrderHubClient  
    {
        private readonly IConfiguration _configuration;
        private readonly HubConnection hubConnection;

        public OrderHubClient(IConfiguration configuration)
        {
            var hubUrl = configuration.GetValue<string>("CafeHub");
            this.hubConnection = new HubConnectionBuilder().WithUrl(hubUrl).Build();
        }

        public async Task Publish(Order order)
        {
            if (hubConnection.State != HubConnectionState.Connected)
            {
                await hubConnection.StartAsync();
            }

            await hubConnection.InvokeAsync("BroadcastMessage", JsonSerializer.Serialize(order).ToString());
        }

    }
}

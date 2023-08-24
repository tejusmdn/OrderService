using CafeCommon.Models;
using OrderService.DataAccess;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();
builder.Services.AddScoped<OrderHubClient>();

// Configure SignalR
var signalRServerBuilder = builder.Services.AddSignalR();
var azureSignalRConnectionString = builder.Configuration.GetValue<string>("AzureSignalR");
if (!string.IsNullOrWhiteSpace(azureSignalRConnectionString))
{
    signalRServerBuilder.AddAzureSignalR(azureSignalRConnectionString);
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

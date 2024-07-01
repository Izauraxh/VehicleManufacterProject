using Common;
using MassTransit;
using MassTransit.RabbitMqTransport;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using OrderService;
using OrderService.Interface;
using OrderService.Repository;
using Serilog;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<OrderDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// Add services to the container.
builder.Services.AddHttpClient("InventoryAPI", client =>
{
    client.BaseAddress = new Uri(builder.Configuration.GetValue<string>("InventoryAPI"));
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});
var sp = builder.Services.BuildServiceProvider();

builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddSingleton<LoggerFactoryWrapper>(new LoggerFactoryWrapper(sp.GetService<ILoggerFactory>()));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
builder.Services.AddMassTransit(x =>
{
    x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
    {
        cfg.Host("rabbitmq://localhost");
    }));
});
builder.Services.AddMassTransitHostedService();
builder.Services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog());
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Order API",
        Version = "v1",
        Description = "Description",
        Contact = new OpenApiContact
        {
            Name = "Izaura ",
            Email = "Izaura.Xhumari@gmail.com"

        }
    });
});
builder.Services.AddControllers();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});


app.Run();


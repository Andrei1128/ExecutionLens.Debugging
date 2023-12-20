using Logging.APPLICATION.Helpers;
using PostMortemTests.Helpers;
using PostMortemTests.Repositories;
using PostMortemTests.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddLogger();

builder.Services.AddLoggedScoped<IOrderService, OrderService>();
builder.Services.AddLoggedScoped<IOrderMapper, OrderMapper>();
builder.Services.AddLoggedScoped<IClockService, ClockService>();
builder.Services.AddLoggedScoped<IOrderRepository, OrderRepository>();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

using Data.DataAccess;
using Services.Core;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<AppDbContext>();
builder.Services.AddScoped<IOrderService, OrderService>();

builder.Services.Configure<AppDatabaseSetting>(
    builder.Configuration.GetSection("ApplicationDatabase"));

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

try
{
    var dbContext = app.Services.GetRequiredService<AppDbContext>();
    dbContext.CreateCollectionsIfNotExisted();
}
catch (Exception ex)
{
    Console.WriteLine(ex.ToString());
    throw;
}

app.Run();

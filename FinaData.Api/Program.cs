using FinaData.Api.Data;
using FinaData.Api.Endpoints;
using FinaData.Api.Handlers;
using FinaData.Core.Handlers;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? string.Empty;
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(connectionString);
});

builder.Services.AddTransient<ICategoryHandler, CategoryHandler>();

builder.Services.AddTransient<ITransactionHandler, TransactionHandler>();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(x => { x.CustomSchemaIds(n => n.FullName); });

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/", () => new { message = "OK" });
app.MapEndpoints();

app.Run();

using gRPC_Server.Model;
using gRPC_Server.Repositories;
using gRPC_Server.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddScoped<IRateRepository, RateRepository>();

Npgsql.NpgsqlConnection.GlobalTypeMapper.MapEnum<OrdersCurrency>("OrdersCurrency");
Npgsql.NpgsqlConnection.GlobalTypeMapper.MapEnum<ActionType>("ActionType");


builder.Services.AddDbContext<CurrencyContext>(options =>
{
    var conn = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseNpgsql(conn);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<CryptoService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();

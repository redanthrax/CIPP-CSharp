using CIPP.Api.Data;
using CIPP.Api.Modules.Swagger;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.EntityFrameworkCore;
using Prometheus;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(connectionString);
});

var redisConnection = builder.Configuration.GetConnectionString("Redis") ?? "localhost:6379";
var redisAvailable = false;
try
{
    Log.Information("Attempting to connect to Redis at {RedisConnection}...", redisConnection);
    var redisOptions = StackExchange.Redis.ConfigurationOptions.Parse(redisConnection);
    redisOptions.ConnectTimeout = 5000;
    redisOptions.AbortOnConnectFail = false;
    var redis = StackExchange.Redis.ConnectionMultiplexer.Connect(redisOptions);
    if (redis.IsConnected)
    {
        Log.Information("Redis connection successful.");
        builder.Services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = redisConnection;
            options.InstanceName = "CIPP_";
        });
        redisAvailable = true;
    }
}
catch (Exception ex)
{
    Log.Warning(ex, "Redis connection failed. Falling back to in-memory cache.");
}

if (!redisAvailable)
{
    builder.Services.AddDistributedMemoryCache();
    Log.Information("Using in-memory distributed cache.");
}

builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UsePostgreSqlStorage(c => c.UseNpgsqlConnection(connectionString)));

var serverName = Environment.MachineName + "-" + Environment.ProcessId;
builder.Services.AddHangfireServer(options =>
{
    options.ServerName = serverName;
    options.WorkerCount = Math.Max(1, Environment.ProcessorCount / 2);
    options.Queues = new[] { "default", "high-priority" };
    options.ServerTimeout = TimeSpan.FromMinutes(4);
    options.HeartbeatInterval = TimeSpan.FromSeconds(30);
    options.ServerCheckInterval = TimeSpan.FromMinutes(1);
    options.SchedulePollingInterval = TimeSpan.FromSeconds(15);
});
Log.Information("Hangfire configured with PostgreSQL storage.");

var corsOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? Array.Empty<string>();
var allowCredentials = builder.Configuration.GetValue<bool>("Cors:AllowCredentials", false);

builder.Services.AddCors(options => {
    options.AddPolicy("AllowWasmFrontend", policy => {
        if (corsOrigins.Length > 0) {
            policy.WithOrigins(corsOrigins);
        }
        
        policy.AllowAnyHeader()
              .AllowAnyMethod();
              
        if (allowCredentials) {
            policy.AllowCredentials();
        }
    });
});

builder.Services.AddSignalR();
builder.Services.AddModules(builder.Configuration);
builder.Services.AddSingleton<IMetricServer>(provider =>
{
    var server = new MetricServer(9090);
    return server;
});

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    try
    {
        Log.Information("Attempting to connect to database...");
        var canConnect = await context.Database.CanConnectAsync();
        if (!canConnect)
        {
            throw new InvalidOperationException("Unable to establish connection to database.");
        }
        Log.Information("Database connection successful. Running migrations...");
        await context.Database.MigrateAsync();
        Log.Information("Database migrations completed successfully.");
    }
    catch (Exception ex)
    {
        Log.Fatal(ex, "Failed to connect to database. Application will not start.");
        Log.CloseAndFlush();
        Environment.Exit(1);
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerModule();
    app.UseHangfireDashboard("/hangfire");
}

app.UseSerilogRequestLogging();
app.UseHttpMetrics();
app.UseCors("AllowWasmFrontend");
app.UseAuthentication();
app.UseAuthorization();

app.MapModuleEndpoints();

await app.InitializeModulesAsync();
app.MapMetrics();
try
{
    Log.Information("Starting CIPP API");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}

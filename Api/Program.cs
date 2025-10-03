using CIPP.Api.Data;
using CIPP.Api.Modules.Swagger;
using Hangfire;
using Hangfire.Redis.StackExchange;
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
    if (builder.Environment.IsDevelopment())
    {
        options.EnableSensitiveDataLogging();
        options.EnableDetailedErrors();
    }
});

var redisConnection = builder.Configuration.GetConnectionString("Redis") ?? "localhost:6379";
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = redisConnection;
    options.InstanceName = "CIPP_";
});

builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseRedisStorage(redisConnection));
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

// Add CORS for WASM frontend
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
    await context.Database.MigrateAsync();
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

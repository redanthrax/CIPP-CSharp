using System.Reflection;
using Asp.Versioning;
using CIPP.Api.Extensions;
public static class RegisterModules {
    public static void AddModules(this IServiceCollection services, IConfiguration configuration)
    {
        var moduleTypes = DiscoverModules();
        foreach (var moduleType in moduleTypes)
        {
            RegisterModule(services, configuration, moduleType);
        }
    }
    public static void MapModuleEndpoints(this WebApplication app)
    {
        var moduleTypes = DiscoverModules();
        foreach (var moduleType in moduleTypes)
        {
            ConfigureModuleEndpoints(app, moduleType);
        }
    }
    
    public static async Task InitializeModulesAsync(this WebApplication app)
    {
        var loggerFactory = app.Services.GetRequiredService<ILoggerFactory>();
        var logger = loggerFactory.CreateLogger("RegisterModules");
        var moduleTypes = DiscoverModules();
        
        logger.LogInformation("Initializing {Count} modules...", moduleTypes.Count());
        
        foreach (var moduleType in moduleTypes)
        {
            await InitializeModuleAsync(app, moduleType, logger);
        }
        
        logger.LogInformation("Module initialization complete");
    }
    private static IEnumerable<Type> DiscoverModules()
    {
        var currentAssembly = Assembly.GetExecutingAssembly();
        return currentAssembly.GetTypes()
            .Where(type => type.Name.EndsWith("Module") && 
                          type.IsClass && 
                          !type.IsAbstract &&
                          type.Namespace != null &&
                          type.Namespace.StartsWith("CIPP.Api.Modules.") &&
                          type != typeof(RegisterModules) &&
                          HasRequiredMethods(type));
    }
    private static bool HasRequiredMethods(Type moduleType)
    {
        var registerServicesMethod = moduleType.GetMethod("RegisterServices", 
            BindingFlags.Public | BindingFlags.Instance,
            null,
            new[] { typeof(IServiceCollection), typeof(IConfiguration) },
            null);
        var configureEndpointsMethod = moduleType.GetMethod("ConfigureEndpoints",
            BindingFlags.Public | BindingFlags.Instance,
            null,
            new[] { typeof(RouteGroupBuilder) },
            null);
        return registerServicesMethod != null && configureEndpointsMethod != null;
    }
    
    private static bool ShouldExcludeFromVersioning(Type moduleType)
    {
        if (typeof(IInternalModule).IsAssignableFrom(moduleType))
            return true;
            
        var excludedNamespaces = new[] 
        {
            "Frontend",
            "Swagger",
            "Versioning"
        };
        
        return excludedNamespaces.Any(excluded => 
            moduleType.Namespace?.Contains(excluded, StringComparison.OrdinalIgnoreCase) == true);
    }
    private static void RegisterModule(IServiceCollection services, IConfiguration configuration, Type moduleType)
    {
        try
        {
            var moduleInstance = Activator.CreateInstance(moduleType);
            if (moduleInstance == null)
            {
                throw new InvalidOperationException($"Failed to create instance of module {moduleType.Name}");
            }
            var registerServicesMethod = moduleType.GetMethod("RegisterServices");
            registerServicesMethod?.Invoke(moduleInstance, new object[] { services, configuration });
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to register services for module {moduleType.Name}", ex);
        }
    }
    private static void ConfigureModuleEndpoints(WebApplication app, Type moduleType)
    {
        try
        {
            var moduleInstance = Activator.CreateInstance(moduleType);
            if (moduleInstance == null)
            {
                throw new InvalidOperationException($"Failed to create instance of module {moduleType.Name}");
            }
            
            var moduleName = moduleType.Name.EndsWith("Module") 
                ? moduleType.Name.Substring(0, moduleType.Name.Length - 6).ToLowerInvariant()
                : moduleType.Name.ToLowerInvariant();
                
            var shouldExcludeFromVersioning = ShouldExcludeFromVersioning(moduleType);
            var isInternalModule = typeof(IInternalModule).IsAssignableFrom(moduleType);
            
            RouteGroupBuilder moduleGroup;
            
            if (shouldExcludeFromVersioning)
            {
                moduleGroup = app.MapGroup($"/api/{moduleName}")
                    .WithTags(moduleType.Name.Replace("Module", ""));
                    
                if (isInternalModule)
                {
                    moduleGroup = moduleGroup.Internal();
                }
            }
            else
            {
                var versionSet = app.NewApiVersionSet()
                    .HasApiVersion(new ApiVersion(1, 0))
                    .ReportApiVersions()
                    .Build();
                    
                moduleGroup = app.MapGroup($"/api/v{{version:apiVersion}}/{moduleName}")
                    .WithApiVersionSet(versionSet)
                    .WithTags(moduleType.Name.Replace("Module", ""));
                    
                if (isInternalModule)
                {
                    moduleGroup = moduleGroup.Internal();
                }
            }
            
            var configureEndpointsMethod = moduleType.GetMethod("ConfigureEndpoints");
            configureEndpointsMethod?.Invoke(moduleInstance, new object[] { moduleGroup });
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to configure endpoints for module {moduleType.Name}", ex);
        }
    }
    
    private static async Task InitializeModuleAsync(WebApplication app, Type moduleType, ILogger logger)
    {
        try
        {
            var staticConfigureMethod = moduleType.GetMethod("ConfigureAsync", 
                BindingFlags.Public | BindingFlags.Static,
                null,
                new[] { typeof(IApplicationBuilder) },
                null);
            
            if (staticConfigureMethod != null)
            {
                logger.LogInformation("Initializing module: {ModuleName}", moduleType.Name);
                var result = staticConfigureMethod.Invoke(null, new object[] { app });
                if (result is Task task)
                {
                    await task;
                }
                logger.LogInformation("Successfully initialized module: {ModuleName}", moduleType.Name);
            }
            
            var instanceInitializeMethod = moduleType.GetMethod("InitializeAsync",
                BindingFlags.Public | BindingFlags.Instance,
                null,
                new[] { typeof(IApplicationBuilder) },
                null);
            
            if (instanceInitializeMethod != null)
            {
                var moduleInstance = Activator.CreateInstance(moduleType);
                if (moduleInstance != null)
                {
                    logger.LogInformation("Initializing module instance: {ModuleName}", moduleType.Name);
                    var result = instanceInitializeMethod.Invoke(moduleInstance, new object[] { app });
                    if (result is Task task)
                    {
                        await task;
                    }
                    logger.LogInformation("Successfully initialized module instance: {ModuleName}", moduleType.Name);
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to initialize module {ModuleName}", moduleType.Name);
        }
    }
}

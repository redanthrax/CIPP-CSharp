using CIPP.Api.Modules.Standards.Interfaces;

namespace CIPP.Api.Modules.Standards.Services;

public class StandardExecutorFactory {
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<StandardExecutorFactory> _logger;
    private readonly Dictionary<string, Type> _executors = new();

    public StandardExecutorFactory(IServiceProvider serviceProvider, ILogger<StandardExecutorFactory> logger) {
        _serviceProvider = serviceProvider;
        _logger = logger;
        DiscoverExecutors();
    }

    private void DiscoverExecutors() {
        var executorTypes = typeof(StandardExecutorFactory).Assembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && typeof(IStandardExecutor).IsAssignableFrom(t))
            .ToList();

        foreach (var executorType in executorTypes) {
            var instance = (IStandardExecutor?)Activator.CreateInstance(
                executorType,
                _serviceProvider.GetService(executorType.GetConstructors().First().GetParameters().First().ParameterType),
                _serviceProvider.GetService<ILoggerFactory>()?.CreateLogger(executorType)
            );

            if (instance != null) {
                _executors[instance.StandardType] = executorType;
                _logger.LogInformation("Registered standard executor for type: {StandardType}", instance.StandardType);
            }
        }
    }

    public IStandardExecutor? GetExecutor(string standardType) {
        if (!_executors.TryGetValue(standardType, out var executorType)) {
            _logger.LogWarning("No executor found for standard type: {StandardType}", standardType);
            return null;
        }

        try {
            return (IStandardExecutor?)_serviceProvider.GetService(executorType);
        } catch (Exception ex) {
            _logger.LogError(ex, "Failed to create executor for type {StandardType}", standardType);
            return null;
        }
    }

    public IEnumerable<string> GetSupportedTypes() {
        return _executors.Keys;
    }
}

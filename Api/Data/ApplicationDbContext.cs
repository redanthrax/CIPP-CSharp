using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;
using System.Reflection;

namespace CIPP.Api.Data;

public class ApplicationDbContext : DbContext
{
    private static readonly ConcurrentDictionary<Type, string> _entityMappings = new();
    private readonly Dictionary<string, object> _dbSets = new();

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
    {
        InitializeEntities();
    }

    private void InitializeEntities()
    {
        if (_entityMappings.IsEmpty)
        {
            DiscoverEntities();
        }
    }

    private static void DiscoverEntities()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var entityTypes = assembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && 
                       t.GetInterfaces().Any(i => i.IsGenericType && 
                                                  i.GetGenericTypeDefinition() == typeof(IEntityConfiguration<>)))
            .ToList();

        foreach (var entityType in entityTypes)
        {
            var entityNameProperty = entityType.GetProperty("EntityName", BindingFlags.Public | BindingFlags.Static);
            if (entityNameProperty != null)
            {
                var entityName = (string?)entityNameProperty.GetValue(null);
                if (!string.IsNullOrEmpty(entityName))
                {
                    _entityMappings.TryAdd(entityType, entityName);
                }
            }
        }
    }

    public DbSet<T> GetEntitySet<T>() where T : class
    {
        var entityType = typeof(T);
        if (_entityMappings.TryGetValue(entityType, out var entityName))
        {
            if (!_dbSets.ContainsKey(entityName))
            {
                _dbSets[entityName] = Set<T>();
            }
            return (DbSet<T>)_dbSets[entityName];
        }
        return Set<T>();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        var assembly = Assembly.GetExecutingAssembly();
        var entityTypes = assembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && 
                       t.GetInterfaces().Any(i => i.IsGenericType && 
                                                  i.GetGenericTypeDefinition() == typeof(IEntityConfiguration<>)))
            .ToList();

        foreach (var entityType in entityTypes)
        {
            var configureMethod = entityType.GetMethod("Configure", BindingFlags.Public | BindingFlags.Static);
            configureMethod?.Invoke(null, new object[] { modelBuilder });
        }
    }
}

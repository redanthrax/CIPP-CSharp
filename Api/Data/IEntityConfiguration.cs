using Microsoft.EntityFrameworkCore;

namespace CIPP.Api.Data;

public interface IEntityConfiguration<T> where T : class
{
    static abstract string EntityName { get; }
    static abstract void Configure(ModelBuilder modelBuilder);
}

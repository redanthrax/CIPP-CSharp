using CIPP.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace CIPP.Api.Modules.Standards.Models;

public class StandardExecutionConfiguration : IEntityConfiguration<StandardExecution> {
    public static string EntityName => "StandardExecutions";

    public static void Configure(ModelBuilder modelBuilder) {
        modelBuilder.Entity<StandardExecution>(entity => {
            entity.ToTable("StandardExecutions");
            
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.TemplateId)
                .IsRequired();
            
            entity.Property(e => e.TenantId)
                .IsRequired();
            
            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(50);
            
            entity.Property(e => e.Result)
                .HasColumnType("jsonb");
            
            entity.Property(e => e.ErrorMessage)
                .HasMaxLength(2000);
            
            entity.Property(e => e.ExecutedDate)
                .IsRequired();
            
            entity.Property(e => e.ExecutedBy)
                .HasMaxLength(100);
            
            entity.HasOne(e => e.Template)
                .WithMany(t => t.Executions)
                .HasForeignKey(e => e.TemplateId)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.HasIndex(e => e.TemplateId);
            entity.HasIndex(e => e.TenantId);
            entity.HasIndex(e => e.Status);
            entity.HasIndex(e => e.ExecutedDate);
        });
    }
}

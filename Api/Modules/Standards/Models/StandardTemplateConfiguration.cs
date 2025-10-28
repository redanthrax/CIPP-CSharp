using CIPP.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace CIPP.Api.Modules.Standards.Models;

public class StandardTemplateConfiguration : IEntityConfiguration<StandardTemplate> {
    public static string EntityName => "StandardTemplates";

    public static void Configure(ModelBuilder modelBuilder) {
        modelBuilder.Entity<StandardTemplate>(entity => {
            entity.ToTable("StandardTemplates");
            
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(200);
            
            entity.Property(e => e.Description)
                .HasMaxLength(1000);
            
            entity.Property(e => e.Type)
                .IsRequired()
                .HasMaxLength(100);
            
            entity.Property(e => e.Category)
                .IsRequired()
                .HasMaxLength(100);
            
            entity.Property(e => e.Configuration)
                .IsRequired()
                .HasColumnType("jsonb");
            
            entity.Property(e => e.IsEnabled)
                .IsRequired()
                .HasDefaultValue(true);
            
            entity.Property(e => e.IsGlobal)
                .IsRequired()
                .HasDefaultValue(false);
            
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(100);
            
            entity.Property(e => e.CreatedDate)
                .IsRequired();
            
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(100);
            
            entity.HasMany(e => e.Executions)
                .WithOne(e => e.Template)
                .HasForeignKey(e => e.TemplateId)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.HasIndex(e => e.Type);
            entity.HasIndex(e => e.Category);
            entity.HasIndex(e => e.IsEnabled);
        });
    }
}

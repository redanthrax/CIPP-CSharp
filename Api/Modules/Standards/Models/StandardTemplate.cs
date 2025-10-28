using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CIPP.Api.Modules.Standards.Models;

public class StandardTemplate {
    [Key]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string Description { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Type { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Category { get; set; } = string.Empty;

    [Required]
    [Column(TypeName = "jsonb")]
    public string Configuration { get; set; } = "{}";

    public bool IsEnabled { get; set; } = true;

    public bool IsGlobal { get; set; }

    [MaxLength(100)]
    public string? CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    [MaxLength(100)]
    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public ICollection<StandardExecution> Executions { get; set; } = new List<StandardExecution>();
}

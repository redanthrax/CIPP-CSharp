using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CIPP.Api.Modules.Standards.Models;

public class StandardExecution {
    [Key]
    public Guid Id { get; set; }

    [Required]
    public Guid TemplateId { get; set; }

    [ForeignKey(nameof(TemplateId))]
    public StandardTemplate Template { get; set; } = null!;

    [Required]
    public Guid TenantId { get; set; }

    [Required]
    [MaxLength(50)]
    public string Status { get; set; } = string.Empty;

    [Column(TypeName = "jsonb")]
    public string? Result { get; set; }

    [MaxLength(2000)]
    public string? ErrorMessage { get; set; }

    public DateTime ExecutedDate { get; set; } = DateTime.UtcNow;

    [MaxLength(100)]
    public string? ExecutedBy { get; set; }

    public int? DurationMs { get; set; }
}

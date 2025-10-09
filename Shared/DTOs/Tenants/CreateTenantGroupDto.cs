namespace CIPP.Shared.DTOs.Tenants;

public class CreateTenantGroupDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public List<Guid>? MemberTenantIds { get; set; }

    public CreateTenantGroupDto() { }

    public CreateTenantGroupDto(string name, string? description, List<Guid>? memberTenantIds = null)
    {
        Name = name;
        Description = description;
        MemberTenantIds = memberTenantIds;
    }
}

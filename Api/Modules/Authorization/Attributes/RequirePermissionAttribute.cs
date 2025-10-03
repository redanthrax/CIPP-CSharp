namespace CIPP.Api.Modules.Authorization.Attributes;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
public class RequirePermissionAttribute : Attribute {
    public string Permission { get; }

    public string Description { get; }

    public string? Category { get; set; }

    public RequirePermissionAttribute(string permission, string description) {
        Permission = permission ?? throw new ArgumentNullException(nameof(permission));
        Description = description ?? throw new ArgumentNullException(nameof(description));
        
        if (string.IsNullOrEmpty(Category) && permission.Contains('.')) {
            Category = permission.Split('.')[0];
        }
    }
}

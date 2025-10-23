using CIPP.Api.Modules.Identity.Interfaces;
using CIPP.Api.Modules.MsGraph.Interfaces;
using CIPP.Api.Modules.MsGraph.Services;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Graph.Beta.Models;
using Microsoft.Graph.Beta.Users.Item.Authentication.Methods;
using BetaGraphServiceClient = Microsoft.Graph.Beta.GraphServiceClient;
using StableGraphServiceClient = Microsoft.Graph.GraphServiceClient;
using StableUnifiedRoleAssignment = Microsoft.Graph.Models.UnifiedRoleAssignment;

namespace CIPP.Api.Modules.Identity.Services;

public class UserService : IUserService {
    private readonly GraphUserService _graphUserService;
    private readonly IMicrosoftGraphService _graphService;
    private readonly ILicenseService _licenseService;
    private readonly ILogger<UserService> _logger;

    public UserService(
        GraphUserService graphUserService,
        IMicrosoftGraphService graphService,
        ILicenseService licenseService,
        ILogger<UserService> logger) {
        _graphUserService = graphUserService;
        _graphService = graphService;
        _licenseService = licenseService;
        _logger = logger;
    }

    public async Task<PagedResponse<UserDto>> GetUsersAsync(string tenantId, PagingParameters? paging = null, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Getting users for tenant {TenantId}", tenantId);
        
        paging ??= new PagingParameters();
        var response = await _graphUserService.ListUsersAsync(tenantId, paging: paging);
        
        if (response?.Value == null) {
            return new PagedResponse<UserDto> {
                Items = new List<UserDto>(),
                TotalCount = 0,
                PageNumber = paging.PageNumber,
                PageSize = paging.PageSize
            };
        }

        var users = new List<UserDto>();
        foreach (var user in response.Value) {
            var userDto = await MapToUserDtoAsync(user, tenantId);
            users.Add(userDto);
        }

        return new PagedResponse<UserDto> {
            Items = users,
            TotalCount = (int)(response.OdataCount ?? users.Count),
            PageNumber = paging.PageNumber,
            PageSize = paging.PageSize,
            SkipToken = response.OdataNextLink != null ? ExtractSkipToken(response.OdataNextLink) : null
        };
    }
    
    private static string? ExtractSkipToken(string? nextLink) {
        if (string.IsNullOrEmpty(nextLink)) return null;
        var uri = new Uri(nextLink);
        var query = QueryHelpers.ParseQuery(uri.Query);
        return query.TryGetValue("$skiptoken", out var token) ? token.ToString() : null;
    }

    public async Task<UserDto?> GetUserAsync(string tenantId, string userId, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Getting user {UserId} for tenant {TenantId}", userId, tenantId);
        
        var user = await _graphUserService.GetUserAsync(userId, tenantId);
        if (user == null) {
            return null;
        }

        return await MapToUserDtoAsync(user, tenantId);
    }

    public async Task<UserDto> CreateUserAsync(CreateUserDto createUserDto, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Creating user {UserPrincipalName} for tenant {TenantId}", 
            createUserDto.UserPrincipalName, createUserDto.TenantId);

        var graphClient = await _graphService.GetGraphServiceClientAsync(createUserDto.TenantId);
        
        var user = new User {
            UserPrincipalName = createUserDto.UserPrincipalName,
            DisplayName = createUserDto.DisplayName,
            GivenName = createUserDto.GivenName,
            Surname = createUserDto.Surname,
            Mail = createUserDto.Mail,
            JobTitle = createUserDto.JobTitle,
            Department = createUserDto.Department,
            MobilePhone = createUserDto.MobilePhone,
            BusinessPhones = !string.IsNullOrEmpty(createUserDto.BusinessPhone) 
                ? new List<string> { createUserDto.BusinessPhone } : null,
            OfficeLocation = createUserDto.OfficeLocation,
            Country = createUserDto.Country,
            City = createUserDto.City,
            State = createUserDto.State,
            PostalCode = createUserDto.PostalCode,
            StreetAddress = createUserDto.StreetAddress,
            AccountEnabled = createUserDto.AccountEnabled,
            PasswordProfile = new PasswordProfile {
                Password = !string.IsNullOrEmpty(createUserDto.Password) 
                    ? createUserDto.Password 
                    : GenerateRandomPassword(),
                ForceChangePasswordNextSignIn = createUserDto.ForceChangePasswordNextSignIn
            }
        };

        var createdUser = await _graphUserService.CreateUserAsync(user, createUserDto.TenantId);
        
        if (createdUser == null) {
            throw new InvalidOperationException("Failed to create user");
        }

        if (createUserDto.AssignedLicenses != null && createUserDto.AssignedLicenses.Count > 0) {
            await _licenseService.AssignLicensesAsync(createUserDto.TenantId, createdUser.Id!, createUserDto.AssignedLicenses, cancellationToken);
        }
        if (createUserDto.AssignedRoles != null && createUserDto.AssignedRoles.Count > 0) {
            await AssignRolesAsync(graphClient, createdUser.Id!, createUserDto.AssignedRoles, cancellationToken);
        }

        return await MapToUserDtoAsync(createdUser, createUserDto.TenantId);
    }

    public async Task<UserDto> UpdateUserAsync(string tenantId, string userId, UpdateUserDto updateUserDto, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Updating user {UserId} for tenant {TenantId}", userId, tenantId);

        var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
        
        var user = new User();
        
        if (updateUserDto.DisplayName != null) user.DisplayName = updateUserDto.DisplayName;
        if (updateUserDto.GivenName != null) user.GivenName = updateUserDto.GivenName;
        if (updateUserDto.Surname != null) user.Surname = updateUserDto.Surname;
        if (updateUserDto.Mail != null) user.Mail = updateUserDto.Mail;
        if (updateUserDto.JobTitle != null) user.JobTitle = updateUserDto.JobTitle;
        if (updateUserDto.Department != null) user.Department = updateUserDto.Department;
        if (updateUserDto.MobilePhone != null) user.MobilePhone = updateUserDto.MobilePhone;
        if (updateUserDto.BusinessPhone != null) user.BusinessPhones = new List<string> { updateUserDto.BusinessPhone };
        if (updateUserDto.OfficeLocation != null) user.OfficeLocation = updateUserDto.OfficeLocation;
        if (updateUserDto.Country != null) user.Country = updateUserDto.Country;
        if (updateUserDto.City != null) user.City = updateUserDto.City;
        if (updateUserDto.State != null) user.State = updateUserDto.State;
        if (updateUserDto.PostalCode != null) user.PostalCode = updateUserDto.PostalCode;
        if (updateUserDto.StreetAddress != null) user.StreetAddress = updateUserDto.StreetAddress;
        if (updateUserDto.AccountEnabled.HasValue) user.AccountEnabled = updateUserDto.AccountEnabled.Value;

        var updatedUser = await _graphUserService.UpdateUserAsync(userId, user, tenantId);
        
        if (updatedUser == null) {
            throw new InvalidOperationException("Failed to update user");
        }

        if (updateUserDto.AssignedLicenses != null) {
            await _licenseService.AssignLicensesAsync(tenantId, userId, updateUserDto.AssignedLicenses, cancellationToken);
        }

        if (updateUserDto.AssignedRoles != null) {
            await AssignRolesAsync(graphClient, userId, updateUserDto.AssignedRoles, cancellationToken);
        }

        return await MapToUserDtoAsync(updatedUser, tenantId);
    }

    public async Task DeleteUserAsync(string tenantId, string userId, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Deleting user {UserId} for tenant {TenantId}", userId, tenantId);

        await _graphUserService.DeleteUserAsync(userId, tenantId);
    }

    public async Task<string> ResetUserPasswordAsync(string tenantId, string userId, ResetUserPasswordDto resetPasswordDto, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Resetting password for user {UserId} in tenant {TenantId}", userId, tenantId);

        var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
        
        var password = resetPasswordDto.AutoGeneratePassword || string.IsNullOrEmpty(resetPasswordDto.Password) 
            ? GenerateRandomPassword() 
            : resetPasswordDto.Password;

        var user = new User {
            PasswordProfile = new PasswordProfile {
                Password = password,
                ForceChangePasswordNextSignIn = resetPasswordDto.ForceChangePasswordNextSignIn
            }
        };

        await _graphUserService.UpdateUserAsync(userId, user, tenantId);
        
        return password;
    }

    public async Task EnableUserMfaAsync(string tenantId, string userId, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Enabling MFA for user {UserId} in tenant {TenantId}", userId, tenantId);
        
        var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
        
        var user = new User {
            AdditionalData = new Dictionary<string, object> {
                ["strongAuthenticationRequirements"] = new[] {
                    new Dictionary<string, object> {
                        ["state"] = "Enabled"
                    }
                }
            }
        };

        await _graphUserService.UpdateUserAsync(userId, user, tenantId);
    }

    public async Task DisableUserMfaAsync(string tenantId, string userId, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Disabling MFA for user {UserId} in tenant {TenantId}", userId, tenantId);
        
        var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
        
        var user = new User {
            AdditionalData = new Dictionary<string, object> {
                ["strongAuthenticationRequirements"] = Array.Empty<object>()
            }
        };

        await _graphUserService.UpdateUserAsync(userId, user, tenantId);
    }

    public async Task<UserMfaStatusDto> GetUserMfaStatusAsync(string tenantId, string userId, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Getting MFA status for user {UserId} in tenant {TenantId}", userId, tenantId);
        
        var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
        
        try {
            var authMethods = await graphClient.Users[userId].Authentication.Methods.GetAsync(null, cancellationToken);
            
            var methods = new List<string>();
            var hasStrongAuth = false;
            
            if (authMethods?.Value != null) {
                foreach (var method in authMethods.Value) {
                    switch (method.OdataType) {
                        case "#microsoft.graph.microsoftAuthenticatorAuthenticationMethod":
                            methods.Add("Microsoft Authenticator");
                            hasStrongAuth = true;
                            break;
                        case "#microsoft.graph.phoneAuthenticationMethod":
                            methods.Add("Phone");
                            hasStrongAuth = true;
                            break;
                        case "#microsoft.graph.emailAuthenticationMethod":
                            methods.Add("Email");
                            break;
                        case "#microsoft.graph.passwordAuthenticationMethod":
                            methods.Add("Password");
                            break;
                        case "#microsoft.graph.fido2AuthenticationMethod":
                            methods.Add("FIDO2");
                            hasStrongAuth = true;
                            break;
                    }
                }
            }

            var strongAuthRequirements = await GetStrongAuthRequirementsAsync(graphClient, userId, cancellationToken);

            return new UserMfaStatusDto {
                Enabled = hasStrongAuth,
                Enforced = strongAuthRequirements.Contains("Enforced"),
                Methods = methods,
                DefaultMethod = methods.FirstOrDefault() ?? ""
            };
        }
        catch (Exception ex) {
            _logger.LogWarning(ex, "Could not retrieve MFA status for user {UserId}", userId);
            return new UserMfaStatusDto {
                Enabled = false,
                Enforced = false,
                Methods = new List<string>(),
                DefaultMethod = ""
            };
        }
    }

    private async Task<UserDto> MapToUserDtoAsync(User user, string tenantId) {
        var betaGraphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
        
        var assignedRoles = await GetUserRolesAsync(betaGraphClient, user.Id!);
        var mfaStatus = await GetUserMfaStatusAsync(tenantId, user.Id!);

        return new UserDto {
            Id = user.Id ?? "",
            UserPrincipalName = user.UserPrincipalName ?? "",
            DisplayName = user.DisplayName ?? "",
            GivenName = user.GivenName ?? "",
            Surname = user.Surname ?? "",
            Mail = user.Mail ?? "",
            JobTitle = user.JobTitle ?? "",
            Department = user.Department ?? "",
            MobilePhone = user.MobilePhone ?? "",
            BusinessPhone = user.BusinessPhones?.FirstOrDefault() ?? "",
            OfficeLocation = user.OfficeLocation ?? "",
            Country = user.Country ?? "",
            City = user.City ?? "",
            State = user.State ?? "",
            PostalCode = user.PostalCode ?? "",
            StreetAddress = user.StreetAddress ?? "",
            AccountEnabled = user.AccountEnabled ?? false,
            CreatedDateTime = user.CreatedDateTime?.DateTime,
            LastSignInDateTime = user.SignInActivity?.LastSignInDateTime?.DateTime,
            Manager = await GetManagerDisplayNameAsync(betaGraphClient, user.Id),
            AssignedLicenses = user.AssignedLicenses?.Select(l => l.SkuId?.ToString() ?? "").ToList() ?? new List<string>(),
            AssignedRoles = assignedRoles,
            MfaStatus = mfaStatus,
            TenantId = tenantId
        };
    }

    private static async Task<List<string>> GetUserRolesAsync(BetaGraphServiceClient graphClient, string userId) {
        try {
            var memberOf = await graphClient.Users[userId].MemberOf.GetAsync();
            var roles = new List<string>();
            
            if (memberOf?.Value != null) {
                foreach (var item in memberOf.Value) {
                    if (item is DirectoryRole role) {
                        roles.Add(role.DisplayName ?? "");
                    }
                }
            }
            
            return roles;
        }
        catch {
            return new List<string>();
        }
    }

    private static async Task<string?> GetManagerDisplayNameAsync(BetaGraphServiceClient graphClient, string? userId) {
        try {
            if (string.IsNullOrEmpty(userId)) return null;
            var manager = await graphClient.Users[userId].Manager.GetAsync();
            return manager is User managerUser ? managerUser.DisplayName : null;
        }
        catch {
            return null;
        }
    }

    private static async Task<List<string>> GetStrongAuthRequirementsAsync(BetaGraphServiceClient graphClient, string userId, CancellationToken cancellationToken) {
        try {
            var user = await graphClient.Users[userId].GetAsync(null, cancellationToken);
            if (user?.AdditionalData?.ContainsKey("strongAuthenticationRequirements") == true) {
                var requirements = user.AdditionalData["strongAuthenticationRequirements"];
                if (requirements is IEnumerable<object> reqList) {
                    return reqList.OfType<Dictionary<string, object>>()
                        .Select(r => r.ContainsKey("state") ? r["state"].ToString() : "")
                        .Where(s => !string.IsNullOrEmpty(s))
                        .ToList()!;
                }
            }
            return new List<string>();
        }
        catch {
            return new List<string>();
        }
    }


    private static async Task AssignRolesAsync(BetaGraphServiceClient betaGraphClient, string? userId, List<string> roleIds, CancellationToken cancellationToken) {
        if (string.IsNullOrEmpty(userId) || !roleIds.Any()) return;

        try {
            var stableClient = new StableGraphServiceClient(betaGraphClient.RequestAdapter);
            
            foreach (var roleId in roleIds) {
                var roleAssignment = new StableUnifiedRoleAssignment {
                    PrincipalId = userId,
                    RoleDefinitionId = roleId,
                    DirectoryScopeId = "/"
                };

                await stableClient.RoleManagement.Directory.RoleAssignments.PostAsync(roleAssignment, requestConfiguration => { }, cancellationToken);
            }
        }
        catch (Exception ex) {
            throw new InvalidOperationException($"Failed to assign roles: {ex.Message}", ex);
        }
    }

    private static string GenerateRandomPassword() {
        const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnpqrstuvwxyz23456789!@#$%&*";
        var random = new Random();
        return new string(Enumerable.Repeat(chars, 12)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}
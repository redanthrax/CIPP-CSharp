using CIPP.Api.Modules.Identity.Endpoints;
using CIPP.Api.Modules.Identity.Interfaces;
using CIPP.Api.Modules.Identity.Services;
using DispatchR.Extensions;
using System.Reflection;

namespace CIPP.Api.Modules.Identity;

public class IdentityModule {
    public void RegisterServices(IServiceCollection services, IConfiguration configuration) {
        services.AddDispatchR(Assembly.GetExecutingAssembly(), withPipelines: true, withNotifications: true);
        
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IGroupService, GroupService>();
        services.AddScoped<IDeviceService, DeviceService>();
        services.AddScoped<IRoleService, RoleService>();
    }

    public void ConfigureEndpoints(RouteGroupBuilder group) {
        var userGroup = group.MapGroup("/users").WithTags("Users");
        userGroup.MapGetUsers();
        userGroup.MapGetUser();
        userGroup.MapCreateUser();
        userGroup.MapUpdateUser();
        userGroup.MapDeleteUser();
        userGroup.MapResetUserPassword();
        userGroup.MapEnableUserMfa();
        userGroup.MapDisableUserMfa();
        userGroup.MapGetUserMfaStatus();
        userGroup.MapCreateBulkUsers();
        userGroup.MapSetUserSignInState();
        
        var groupGroup = group.MapGroup("/groups").WithTags("Groups");
        groupGroup.MapGetGroups();
        groupGroup.MapGetGroup();
        groupGroup.MapCreateGroup();
        groupGroup.MapUpdateGroup();
        groupGroup.MapDeleteGroup();
        groupGroup.MapAddGroupMember();
        groupGroup.MapRemoveGroupMember();
        
        var deviceGroup = group.MapGroup("/devices").WithTags("Devices");
        deviceGroup.MapGetDevices();
        deviceGroup.MapGetDevice();
        deviceGroup.MapDeleteDevice();
        deviceGroup.MapDisableDevice();
        deviceGroup.MapEnableDevice();
        
        var roleGroup = group.MapGroup("/roles").WithTags("Roles");
        roleGroup.MapGetRoles();
        roleGroup.MapGetRole();
        roleGroup.MapAssignRole();
        roleGroup.MapRemoveRole();
    }
}
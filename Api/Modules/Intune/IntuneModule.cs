using CIPP.Api.Modules.Intune.Endpoints;
using CIPP.Api.Modules.Intune.Interfaces;
using CIPP.Api.Modules.Intune.Services;
using DispatchR.Extensions;
using System.Reflection;

namespace CIPP.Api.Modules.Intune;

public class IntuneModule {
    public void RegisterServices(IServiceCollection services, IConfiguration configuration) {
        services.AddDispatchR(Assembly.GetExecutingAssembly(), withPipelines: true, withNotifications: true);
        
        services.AddScoped<IIntunePolicyService, IntunePolicyService>();
        services.AddScoped<IAutopilotService, AutopilotService>();
        services.AddScoped<IManagedDeviceService, ManagedDeviceService>();
        services.AddScoped<IIntuneAppService, IntuneAppService>();
    }

    public void ConfigureEndpoints(RouteGroupBuilder group) {
        var policiesGroup = group.MapGroup("/policies").WithTags("Intune Policies");
        policiesGroup.MapGetIntunePolicies();
        policiesGroup.MapCreateIntunePolicy();
        policiesGroup.MapUpdateIntunePolicy();
        policiesGroup.MapDeleteIntunePolicy();
        
        var autopilotGroup = group.MapGroup("/autopilot").WithTags("Autopilot");
        autopilotGroup.MapGetAutopilotDevices();
        autopilotGroup.MapDeleteAutopilotDevice();
        autopilotGroup.MapSyncAutopilotDevices();
        
        var devicesGroup = group.MapGroup("/devices").WithTags("Device Management");
        devicesGroup.MapWipeDevice();
        devicesGroup.MapRetireDevice();
        devicesGroup.MapSyncDevice();
        devicesGroup.MapRebootDevice();
        devicesGroup.MapLocateDevice();
        devicesGroup.MapDefenderScan();
        devicesGroup.MapCleanWindowsDevice();
        devicesGroup.MapResetPasscode();
        devicesGroup.MapRemoteLockDevice();
        devicesGroup.MapSetDeviceName();
        devicesGroup.MapRotateLocalAdminPassword();
        devicesGroup.MapDefenderUpdateSignatures();
        devicesGroup.MapCreateDeviceLogCollection();
        
        var appsGroup = group.MapGroup("/applications").WithTags("Intune Applications");
        appsGroup.MapGetIntuneApps();
        appsGroup.MapAssignIntuneApp();
        appsGroup.MapDeleteIntuneApp();
    }
}

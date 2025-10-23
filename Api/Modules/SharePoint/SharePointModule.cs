using CIPP.Api.Modules.SharePoint.Endpoints.Sites;
using CIPP.Api.Modules.SharePoint.Endpoints.Teams;
using CIPP.Api.Modules.SharePoint.Interfaces;
using CIPP.Api.Modules.SharePoint.Services;
using DispatchR.Extensions;
using System.Reflection;

namespace CIPP.Api.Modules.SharePoint;

public class SharePointModule {
    public void RegisterServices(IServiceCollection services, IConfiguration configuration) {
        services.AddDispatchR(Assembly.GetExecutingAssembly(), withPipelines: true, withNotifications: true);
        services.AddScoped<ISharePointSiteService, SharePointSiteService>();
        services.AddScoped<ITeamsService, TeamsService>();
    }

    public void ConfigureEndpoints(RouteGroupBuilder moduleGroup) {
        var sitesGroup = moduleGroup.MapGroup("/sites").WithTags("SharePoint Sites");
        sitesGroup.MapListSharePointSites();
        sitesGroup.MapGetSharePointSite();
        sitesGroup.MapCreateSharePointSite();
        sitesGroup.MapDeleteSharePointSite();
        sitesGroup.MapGetSharePointSettings();
        sitesGroup.MapGetSharePointQuota();
        sitesGroup.MapSetSharePointPermissions();

        var teamsGroup = moduleGroup.MapGroup("/teams").WithTags("Teams");
        teamsGroup.MapListTeams();
        teamsGroup.MapGetTeam();
        teamsGroup.MapCreateTeam();
        teamsGroup.MapListTeamsActivity();
        teamsGroup.MapListTeamsVoice();
        teamsGroup.MapAssignTeamsPhoneNumber();
        teamsGroup.MapRemoveTeamsPhoneNumber();
    }
}

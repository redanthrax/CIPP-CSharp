namespace CIPP.Api.Modules.Standards.Models;

public class MfaSettingsConfig {
    public bool? EnableFido2 { get; set; }
    public bool? EnableAuthenticator { get; set; }
    public bool RequireMfaForAdmins { get; set; }
}

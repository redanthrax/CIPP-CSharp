namespace CIPP.Shared.DTOs.Tenants;

public record OrganizationDataDto(
    string DisplayName,
    string City,
    string Country,
    string CountryLetterCode,
    string Street,
    string State,
    string PostalCode,
    List<string> BusinessPhones,
    List<string> TechnicalNotificationMails,
    string TenantType,
    string CreatedDateTime,
    string OnPremisesLastPasswordSyncDateTime,
    string OnPremisesLastSyncDateTime,
    bool OnPremisesSyncEnabled,
    List<AssignedPlanDto> AssignedPlans
);
using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CIPP.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AlertConfigurations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    AlertType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    LogType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    TenantFilter = table.Column<string>(type: "text", nullable: false),
                    ExcludedTenants = table.Column<string>(type: "text", nullable: false),
                    Conditions = table.Column<string>(type: "text", nullable: false),
                    Actions = table.Column<string>(type: "text", nullable: false),
                    ScheduleCron = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    HangfireJobId = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastExecuted = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastExecutionResult = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    WebhookSubscriptionId = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    RawConfiguration = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlertConfigurations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApiKey",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    KeyHash = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    LastUsedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UsageCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiKey", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationUser",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Email = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    DisplayName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    AzureAdObjectId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    IsFirstUser = table.Column<bool>(type: "boolean", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastLoginAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Metadata = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUser", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppPermissionSet",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TemplateName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    PermissionsJson = table.Column<string>(type: "text", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppPermissionSet", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppTemplate",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TemplateName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    TemplateType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    TemplateJson = table.Column<string>(type: "text", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppTemplate", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AuditLog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LogId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Tenant = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Title = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    IP = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: true),
                    ActionUrl = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    ActionText = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CIPPUserKey = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    UserId = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    UserKey = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    ClientIP = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: true),
                    CIPPAction = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CIPPClause = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Operation = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Workload = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ResultStatus = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ObjectId = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    RecordType = table.Column<int>(type: "integer", nullable: true),
                    OrganizationId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UserType = table.Column<int>(type: "integer", nullable: true),
                    AdditionalPropertiesJson = table.Column<string>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AuditLogSearch",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SearchId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Tenant = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    DisplayName = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    StartTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ProcessedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    TotalLogs = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    MatchedLogs = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    ErrorMessage = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    QueryJson = table.Column<string>(type: "jsonb", nullable: true),
                    MatchedRulesJson = table.Column<string>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLogSearch", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ConditionalAccessTemplate",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TemplateName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    TemplateJson = table.Column<string>(type: "text", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConditionalAccessTemplate", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Permission",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Category = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permission", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    IsBuiltIn = table.Column<bool>(type: "boolean", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tenant",
                columns: table => new
                {
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    DisplayName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    DefaultDomainName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    Metadata = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    InitialDomainName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    DomainList = table.Column<string>(type: "text", nullable: false),
                    GraphErrorCount = table.Column<int>(type: "integer", nullable: false),
                    LastSyncAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    TenantAlias = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Excluded = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    ExcludeUser = table.Column<Guid>(type: "uuid", nullable: true),
                    ExcludeDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DelegatedPrivilegeStatus = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    RequiresRefresh = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    LastGraphError = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    LastRefresh = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    OriginalDisplayName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Capabilities_HasExchange = table.Column<bool>(type: "boolean", nullable: true),
                    Capabilities_HasSharePoint = table.Column<bool>(type: "boolean", nullable: true),
                    Capabilities_HasTeams = table.Column<bool>(type: "boolean", nullable: true),
                    Capabilities_HasIntune = table.Column<bool>(type: "boolean", nullable: true),
                    Capabilities_HasDefender = table.Column<bool>(type: "boolean", nullable: true),
                    Capabilities_Licenses = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tenant", x => x.TenantId);
                });

            migrationBuilder.CreateTable(
                name: "TenantGroup",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TenantGroup", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApiKeyRole",
                columns: table => new
                {
                    ApiKeyId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiKeyRole", x => new { x.ApiKeyId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_ApiKeyRole_ApiKey_ApiKeyId",
                        column: x => x.ApiKeyId,
                        principalTable: "ApiKey",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApiKeyRole_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RolePermission",
                columns: table => new
                {
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false),
                    PermissionId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermission", x => new { x.RoleId, x.PermissionId });
                    table.ForeignKey(
                        name: "FK_RolePermission_Permission_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permission",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolePermission_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRole",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRole_ApplicationUser_UserId",
                        column: x => x.UserId,
                        principalTable: "ApplicationUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRole_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TenantDomain",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    DomainName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    IsInitial = table.Column<bool>(type: "boolean", nullable: false),
                    IsDefault = table.Column<bool>(type: "boolean", nullable: false),
                    IsVerified = table.Column<bool>(type: "boolean", nullable: false),
                    AuthenticationType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    AvailabilityStatus = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TenantDomain", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TenantDomain_Tenant_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenant",
                        principalColumn: "TenantId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TenantProperty",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    Key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Value = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TenantProperty", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TenantProperty_Tenant_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenant",
                        principalColumn: "TenantId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TenantGroupMembership",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantGroupId = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TenantGroupMembership", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TenantGroupMembership_TenantGroup_TenantGroupId",
                        column: x => x.TenantGroupId,
                        principalTable: "TenantGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TenantGroupMembership_Tenant_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenant",
                        principalColumn: "TenantId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AlertConfigurations_AlertType",
                table: "AlertConfigurations",
                column: "AlertType");

            migrationBuilder.CreateIndex(
                name: "IX_AlertConfigurations_HangfireJobId",
                table: "AlertConfigurations",
                column: "HangfireJobId",
                unique: true,
                filter: "\"HangfireJobId\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AlertConfigurations_IsActive",
                table: "AlertConfigurations",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_ApiKey_IsActive",
                table: "ApiKey",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_ApiKey_KeyHash",
                table: "ApiKey",
                column: "KeyHash");

            migrationBuilder.CreateIndex(
                name: "IX_ApiKey_Name",
                table: "ApiKey",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_ApiKeyRole_RoleId",
                table: "ApiKeyRole",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUser_AzureAdObjectId",
                table: "ApplicationUser",
                column: "AzureAdObjectId",
                unique: true,
                filter: "\"AzureAdObjectId\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUser_Email",
                table: "ApplicationUser",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppPermissionSet_CreatedOn",
                table: "AppPermissionSet",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_AppPermissionSet_TemplateName",
                table: "AppPermissionSet",
                column: "TemplateName");

            migrationBuilder.CreateIndex(
                name: "IX_AppTemplate_CreatedOn",
                table: "AppTemplate",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_AppTemplate_TemplateName",
                table: "AppTemplate",
                column: "TemplateName");

            migrationBuilder.CreateIndex(
                name: "IX_AppTemplate_TemplateType",
                table: "AppTemplate",
                column: "TemplateType");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLog_CreationTime",
                table: "AuditLog",
                column: "CreationTime");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLog_LogId",
                table: "AuditLog",
                column: "LogId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLog_Operation",
                table: "AuditLog",
                column: "Operation");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLog_Tenant",
                table: "AuditLog",
                column: "Tenant");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLog_Timestamp",
                table: "AuditLog",
                column: "Timestamp");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLog_UserId",
                table: "AuditLog",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLog_Workload",
                table: "AuditLog",
                column: "Workload");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogSearch_CreatedAt",
                table: "AuditLogSearch",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogSearch_SearchId",
                table: "AuditLogSearch",
                column: "SearchId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogSearch_StartTime",
                table: "AuditLogSearch",
                column: "StartTime");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogSearch_Status",
                table: "AuditLogSearch",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogSearch_Tenant",
                table: "AuditLogSearch",
                column: "Tenant");

            migrationBuilder.CreateIndex(
                name: "IX_ConditionalAccessTemplate_CreatedOn",
                table: "ConditionalAccessTemplate",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_ConditionalAccessTemplate_TemplateName",
                table: "ConditionalAccessTemplate",
                column: "TemplateName");

            migrationBuilder.CreateIndex(
                name: "IX_Permission_Name",
                table: "Permission",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Role_Name",
                table: "Role",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RolePermission_PermissionId",
                table: "RolePermission",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_Tenant_DelegatedPrivilegeStatus",
                table: "Tenant",
                column: "DelegatedPrivilegeStatus");

            migrationBuilder.CreateIndex(
                name: "IX_Tenant_Excluded",
                table: "Tenant",
                column: "Excluded");

            migrationBuilder.CreateIndex(
                name: "IX_Tenant_LastSyncAt",
                table: "Tenant",
                column: "LastSyncAt");

            migrationBuilder.CreateIndex(
                name: "IX_Tenant_RequiresRefresh",
                table: "Tenant",
                column: "RequiresRefresh");

            migrationBuilder.CreateIndex(
                name: "IX_Tenant_Status",
                table: "Tenant",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_TenantDomain_DomainName",
                table: "TenantDomain",
                column: "DomainName");

            migrationBuilder.CreateIndex(
                name: "IX_TenantDomain_TenantId_IsInitial",
                table: "TenantDomain",
                columns: new[] { "TenantId", "IsInitial" },
                unique: true,
                filter: "\"IsInitial\" = true");

            migrationBuilder.CreateIndex(
                name: "IX_TenantGroup_CreatedAt",
                table: "TenantGroup",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_TenantGroup_Name",
                table: "TenantGroup",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TenantGroupMembership_CreatedAt",
                table: "TenantGroupMembership",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_TenantGroupMembership_TenantGroupId_TenantId",
                table: "TenantGroupMembership",
                columns: new[] { "TenantGroupId", "TenantId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TenantGroupMembership_TenantId",
                table: "TenantGroupMembership",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_TenantProperty_TenantId_Key",
                table: "TenantProperty",
                columns: new[] { "TenantId", "Key" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_RoleId",
                table: "UserRole",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AlertConfigurations");

            migrationBuilder.DropTable(
                name: "ApiKeyRole");

            migrationBuilder.DropTable(
                name: "AppPermissionSet");

            migrationBuilder.DropTable(
                name: "AppTemplate");

            migrationBuilder.DropTable(
                name: "AuditLog");

            migrationBuilder.DropTable(
                name: "AuditLogSearch");

            migrationBuilder.DropTable(
                name: "ConditionalAccessTemplate");

            migrationBuilder.DropTable(
                name: "RolePermission");

            migrationBuilder.DropTable(
                name: "TenantDomain");

            migrationBuilder.DropTable(
                name: "TenantGroupMembership");

            migrationBuilder.DropTable(
                name: "TenantProperty");

            migrationBuilder.DropTable(
                name: "UserRole");

            migrationBuilder.DropTable(
                name: "ApiKey");

            migrationBuilder.DropTable(
                name: "Permission");

            migrationBuilder.DropTable(
                name: "TenantGroup");

            migrationBuilder.DropTable(
                name: "Tenant");

            migrationBuilder.DropTable(
                name: "ApplicationUser");

            migrationBuilder.DropTable(
                name: "Role");
        }
    }
}

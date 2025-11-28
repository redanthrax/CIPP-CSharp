using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CIPP.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddWebhookInfrastructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CachedWebhookEvents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantFilter = table.Column<string>(type: "text", nullable: false),
                    NotificationData = table.Column<string>(type: "text", nullable: false),
                    ResourceData = table.Column<string>(type: "text", nullable: false),
                    ChangeType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Resource = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    SubscriptionId = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    ReceivedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsProcessed = table.Column<bool>(type: "boolean", nullable: false),
                    ProcessedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    RetryCount = table.Column<int>(type: "integer", nullable: false),
                    ErrorMessage = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CachedWebhookEvents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WebhookRules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantFilter = table.Column<string>(type: "text", nullable: false),
                    ExcludedTenants = table.Column<string>(type: "text", nullable: false),
                    Conditions = table.Column<string>(type: "text", nullable: false),
                    Actions = table.Column<string>(type: "text", nullable: false),
                    LogType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    AlertComment = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastTriggered = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    TriggerCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebhookRules", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AuditLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantFilter = table.Column<string>(type: "text", nullable: false),
                    Title = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    ActionUrl = table.Column<string>(type: "text", nullable: true),
                    ActionText = table.Column<string>(type: "text", nullable: true),
                    RawData = table.Column<string>(type: "text", nullable: false),
                    IpAddress = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    LocationInfo = table.Column<string>(type: "text", nullable: true),
                    ActionsTaken = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    WebhookRuleId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuditLogs_WebhookRules_WebhookRuleId",
                        column: x => x.WebhookRuleId,
                        principalTable: "WebhookRules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "GraphWebhookSubscriptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<string>(type: "text", nullable: false),
                    SubscriptionId = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Resource = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    ChangeType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    NotificationUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    ClientState = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    ExpirationDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastRenewed = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    WebhookRuleId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GraphWebhookSubscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GraphWebhookSubscriptions_WebhookRules_WebhookRuleId",
                        column: x => x.WebhookRuleId,
                        principalTable: "WebhookRules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_CreatedAt",
                table: "AuditLogs",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_TenantFilter",
                table: "AuditLogs",
                column: "TenantFilter");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_WebhookRuleId",
                table: "AuditLogs",
                column: "WebhookRuleId");

            migrationBuilder.CreateIndex(
                name: "IX_CachedWebhookEvents_ReceivedAt",
                table: "CachedWebhookEvents",
                column: "ReceivedAt");

            migrationBuilder.CreateIndex(
                name: "IX_CachedWebhookEvents_SubscriptionId",
                table: "CachedWebhookEvents",
                column: "SubscriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_CachedWebhookEvents_TenantFilter_IsProcessed",
                table: "CachedWebhookEvents",
                columns: new[] { "TenantFilter", "IsProcessed" });

            migrationBuilder.CreateIndex(
                name: "IX_GraphWebhookSubscriptions_ExpirationDateTime",
                table: "GraphWebhookSubscriptions",
                column: "ExpirationDateTime");

            migrationBuilder.CreateIndex(
                name: "IX_GraphWebhookSubscriptions_IsActive",
                table: "GraphWebhookSubscriptions",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_GraphWebhookSubscriptions_SubscriptionId",
                table: "GraphWebhookSubscriptions",
                column: "SubscriptionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GraphWebhookSubscriptions_TenantId_Resource",
                table: "GraphWebhookSubscriptions",
                columns: new[] { "TenantId", "Resource" });

            migrationBuilder.CreateIndex(
                name: "IX_GraphWebhookSubscriptions_WebhookRuleId",
                table: "GraphWebhookSubscriptions",
                column: "WebhookRuleId");

            migrationBuilder.CreateIndex(
                name: "IX_WebhookRules_IsActive",
                table: "WebhookRules",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_WebhookRules_LogType",
                table: "WebhookRules",
                column: "LogType");

            migrationBuilder.CreateIndex(
                name: "IX_WebhookRules_TenantFilter",
                table: "WebhookRules",
                column: "TenantFilter");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditLogs");

            migrationBuilder.DropTable(
                name: "CachedWebhookEvents");

            migrationBuilder.DropTable(
                name: "GraphWebhookSubscriptions");

            migrationBuilder.DropTable(
                name: "WebhookRules");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CIPP.Api.Migrations
{
    /// <inheritdoc />
    public partial class EnhancedTenantFeatures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "DisplayName",
                table: "Tenant",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "DefaultDomainName",
                table: "Tenant",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<bool>(
                name: "Capabilities_HasDefender",
                table: "Tenant",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Capabilities_HasExchange",
                table: "Tenant",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Capabilities_HasIntune",
                table: "Tenant",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Capabilities_HasSharePoint",
                table: "Tenant",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Capabilities_HasTeams",
                table: "Tenant",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Capabilities_Licenses",
                table: "Tenant",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DomainList",
                table: "Tenant",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "GraphErrorCount",
                table: "Tenant",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "InitialDomainName",
                table: "Tenant",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastSyncAt",
                table: "Tenant",
                type: "timestamp with time zone",
                nullable: true);

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
                        principalColumn: "Id",
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
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tenant_LastSyncAt",
                table: "Tenant",
                column: "LastSyncAt");

            migrationBuilder.CreateIndex(
                name: "IX_Tenant_Status",
                table: "Tenant",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Tenant_TenantId",
                table: "Tenant",
                column: "TenantId",
                unique: true);

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
                name: "IX_TenantProperty_TenantId_Key",
                table: "TenantProperty",
                columns: new[] { "TenantId", "Key" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TenantDomain");

            migrationBuilder.DropTable(
                name: "TenantProperty");

            migrationBuilder.DropIndex(
                name: "IX_Tenant_LastSyncAt",
                table: "Tenant");

            migrationBuilder.DropIndex(
                name: "IX_Tenant_Status",
                table: "Tenant");

            migrationBuilder.DropIndex(
                name: "IX_Tenant_TenantId",
                table: "Tenant");

            migrationBuilder.DropColumn(
                name: "Capabilities_HasDefender",
                table: "Tenant");

            migrationBuilder.DropColumn(
                name: "Capabilities_HasExchange",
                table: "Tenant");

            migrationBuilder.DropColumn(
                name: "Capabilities_HasIntune",
                table: "Tenant");

            migrationBuilder.DropColumn(
                name: "Capabilities_HasSharePoint",
                table: "Tenant");

            migrationBuilder.DropColumn(
                name: "Capabilities_HasTeams",
                table: "Tenant");

            migrationBuilder.DropColumn(
                name: "Capabilities_Licenses",
                table: "Tenant");

            migrationBuilder.DropColumn(
                name: "DomainList",
                table: "Tenant");

            migrationBuilder.DropColumn(
                name: "GraphErrorCount",
                table: "Tenant");

            migrationBuilder.DropColumn(
                name: "InitialDomainName",
                table: "Tenant");

            migrationBuilder.DropColumn(
                name: "LastSyncAt",
                table: "Tenant");

            migrationBuilder.AlterColumn<string>(
                name: "DisplayName",
                table: "Tenant",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "DefaultDomainName",
                table: "Tenant",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);
        }
    }
}

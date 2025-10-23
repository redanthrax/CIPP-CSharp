using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CIPP.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddAuditLogSearchTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditLogSearch");
        }
    }
}

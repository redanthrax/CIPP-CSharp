using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CIPP.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddAuditLogTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditLog");
        }
    }
}

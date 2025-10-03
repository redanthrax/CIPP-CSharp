using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CIPP.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddApiKeyRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateIndex(
                name: "IX_ApiKeyRole_RoleId",
                table: "ApiKeyRole",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApiKeyRole");
        }
    }
}

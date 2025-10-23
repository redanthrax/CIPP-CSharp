using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CIPP.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddTransportRules : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "IX_ConditionalAccessTemplate_CreatedOn",
                table: "ConditionalAccessTemplate",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_ConditionalAccessTemplate_TemplateName",
                table: "ConditionalAccessTemplate",
                column: "TemplateName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppPermissionSet");

            migrationBuilder.DropTable(
                name: "AppTemplate");

            migrationBuilder.DropTable(
                name: "ConditionalAccessTemplate");
        }
    }
}

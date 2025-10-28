using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CIPP.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddStandardsTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StandardTemplates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    Type = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Category = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Configuration = table.Column<string>(type: "jsonb", nullable: false),
                    IsEnabled = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    IsGlobal = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StandardTemplates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StandardExecutions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TemplateId = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Result = table.Column<string>(type: "jsonb", nullable: true),
                    ErrorMessage = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    ExecutedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ExecutedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    DurationMs = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StandardExecutions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StandardExecutions_StandardTemplates_TemplateId",
                        column: x => x.TemplateId,
                        principalTable: "StandardTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StandardExecutions_ExecutedDate",
                table: "StandardExecutions",
                column: "ExecutedDate");

            migrationBuilder.CreateIndex(
                name: "IX_StandardExecutions_Status",
                table: "StandardExecutions",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_StandardExecutions_TemplateId",
                table: "StandardExecutions",
                column: "TemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_StandardExecutions_TenantId",
                table: "StandardExecutions",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_StandardTemplates_Category",
                table: "StandardTemplates",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_StandardTemplates_IsEnabled",
                table: "StandardTemplates",
                column: "IsEnabled");

            migrationBuilder.CreateIndex(
                name: "IX_StandardTemplates_Type",
                table: "StandardTemplates",
                column: "Type");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StandardExecutions");

            migrationBuilder.DropTable(
                name: "StandardTemplates");
        }
    }
}

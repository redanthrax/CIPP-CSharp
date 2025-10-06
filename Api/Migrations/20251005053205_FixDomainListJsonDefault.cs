using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CIPP.Api.Migrations
{
    /// <inheritdoc />
    public partial class FixDomainListJsonDefault : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Update existing empty string values to valid JSON array
            migrationBuilder.Sql("UPDATE \"Tenant\" SET \"DomainList\" = '[]' WHERE \"DomainList\" = '';");
            
            // Also update any null/empty Capabilities_Licenses values
            migrationBuilder.Sql("UPDATE \"Tenant\" SET \"Capabilities_Licenses\" = '[]' WHERE \"Capabilities_Licenses\" IS NULL OR \"Capabilities_Licenses\" = '';");
            
            // Update the default value for future records
            migrationBuilder.AlterColumn<string>(
                name: "DomainList",
                table: "Tenant",
                type: "text",
                nullable: false,
                defaultValue: "[]",
                oldClrType: typeof(string),
                oldType: "text",
                oldDefaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Van_Authentication.Migrations
{
    /// <inheritdoc />
    public partial class PurgeSheet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "78475ec2-8fbe-4101-b6f9-0c48d6690c3b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bc22fa67-63cb-4766-9e6f-29b6869c76d5");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d6451243-bb09-4643-b23e-906d870fdc7c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "dfe45d0a-e64b-4f0b-a4a4-96219e16aa46");

            migrationBuilder.RenameColumn(
                name: "line",
                table: "Certs",
                newName: "Line");

            migrationBuilder.AddColumn<string>(
                name: "PurgeSheet",
                table: "Tcps",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "057a5e79-152c-43b9-bdbe-9015a9b5f481", null, "coordinator", "coordinator" },
                    { "27d509d5-b6f6-4a03-b6fa-d53a00bf95bd", null, "supervisor", "supervisor" },
                    { "a5ad4384-dbc6-42a5-9b20-3f8e7f3aab0f", null, "manager", "manager" },
                    { "a5ad952f-8141-4565-b883-49e7eadc3592", null, "auditor", "auditor" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "057a5e79-152c-43b9-bdbe-9015a9b5f481");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "27d509d5-b6f6-4a03-b6fa-d53a00bf95bd");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a5ad4384-dbc6-42a5-9b20-3f8e7f3aab0f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a5ad952f-8141-4565-b883-49e7eadc3592");

            migrationBuilder.DropColumn(
                name: "PurgeSheet",
                table: "Tcps");

            migrationBuilder.RenameColumn(
                name: "Line",
                table: "Certs",
                newName: "line");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "78475ec2-8fbe-4101-b6f9-0c48d6690c3b", null, "supervisor", "supervisor" },
                    { "bc22fa67-63cb-4766-9e6f-29b6869c76d5", null, "coordinator", "coordinator" },
                    { "d6451243-bb09-4643-b23e-906d870fdc7c", null, "auditor", "auditor" },
                    { "dfe45d0a-e64b-4f0b-a4a4-96219e16aa46", null, "manager", "manager" }
                });
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Van_Authentication.Migrations
{
    /// <inheritdoc />
    public partial class TCP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "51ea7952-69ff-4442-9859-ca71c5353d7c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "534125a6-516d-46e3-9ef8-90d0a1c4fc56");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9992ccde-44f0-4f29-af48-f1d8498cf943");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e5adedd6-c039-48d5-a18f-edca68c3c007");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Tcps",
                type: "nvarchar(6)",
                maxLength: 6,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(3)",
                oldMaxLength: 3);

            migrationBuilder.AlterColumn<string>(
                name: "LastRepaired",
                table: "Tcps",
                type: "nvarchar(12)",
                maxLength: 12,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EngineeringNotes",
                table: "Tcps",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MaintenanceNotes",
                table: "Tcps",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductionNotes",
                table: "Tcps",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RepairProcedure",
                table: "Tcps",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "102174d8-23d7-4efe-a67e-6c9f10fcc95a", null, "auditor", "auditor" },
                    { "41bdb961-0392-4553-9af3-496fce0b83c5", null, "manager", "manager" },
                    { "8e5fadd9-2af7-4c85-992d-0557d5c97794", null, "supervisor", "supervisor" },
                    { "d803cb2e-8463-469c-b3f5-f8c804dcf808", null, "coordinator", "coordinator" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "102174d8-23d7-4efe-a67e-6c9f10fcc95a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "41bdb961-0392-4553-9af3-496fce0b83c5");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8e5fadd9-2af7-4c85-992d-0557d5c97794");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d803cb2e-8463-469c-b3f5-f8c804dcf808");

            migrationBuilder.DropColumn(
                name: "EngineeringNotes",
                table: "Tcps");

            migrationBuilder.DropColumn(
                name: "MaintenanceNotes",
                table: "Tcps");

            migrationBuilder.DropColumn(
                name: "ProductionNotes",
                table: "Tcps");

            migrationBuilder.DropColumn(
                name: "RepairProcedure",
                table: "Tcps");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Tcps",
                type: "nvarchar(3)",
                maxLength: 3,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(6)",
                oldMaxLength: 6);

            migrationBuilder.AlterColumn<string>(
                name: "LastRepaired",
                table: "Tcps",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(12)",
                oldMaxLength: 12,
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "51ea7952-69ff-4442-9859-ca71c5353d7c", null, "manager", "manager" },
                    { "534125a6-516d-46e3-9ef8-90d0a1c4fc56", null, "auditor", "auditor" },
                    { "9992ccde-44f0-4f29-af48-f1d8498cf943", null, "supervisor", "supervisor" },
                    { "e5adedd6-c039-48d5-a18f-edca68c3c007", null, "coordinator", "coordinator" }
                });
        }
    }
}

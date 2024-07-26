using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Van_Authentication.Migrations
{
    /// <inheritdoc />
    public partial class Certs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "04f7534a-2796-4c59-b328-f4dec022ce3d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1379225d-ca7e-421c-b193-6cd85228fa8c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "eab0a1d6-8df0-4584-bcbd-2bfbe0ad3743");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f2983592-21d2-4194-89a9-b6a79de4ec8a");

            migrationBuilder.AlterColumn<string>(
                name: "Model",
                table: "Audits",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Certs",
                columns: table => new
                {
                    CertId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Requestor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    line = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Station = table.Column<int>(type: "int", nullable: false),
                    RobotNumber = table.Column<int>(type: "int", nullable: false),
                    Style = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Failure = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FailureDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RepairDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Trade = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cleanpoint = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Auditor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InspectedUnit = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Result = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Certs", x => x.CertId);
                });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Certs");

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

            migrationBuilder.AlterColumn<string>(
                name: "Model",
                table: "Audits",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "04f7534a-2796-4c59-b328-f4dec022ce3d", null, "coordinator", "coordinator" },
                    { "1379225d-ca7e-421c-b193-6cd85228fa8c", null, "manager", "manager" },
                    { "eab0a1d6-8df0-4584-bcbd-2bfbe0ad3743", null, "supervisor", "supervisor" },
                    { "f2983592-21d2-4194-89a9-b6a79de4ec8a", null, "auditor", "auditor" }
                });
        }
    }
}

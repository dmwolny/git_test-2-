using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Van_Authentication.Migrations
{
    /// <inheritdoc />
    public partial class MaintShiftNotes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            /*            migrationBuilder.DeleteData(
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
                            keyValue: "a5ad952f-8141-4565-b883-49e7eadc3592");*/

            migrationBuilder.AddColumn<string>(
               name: "Graphic",
               table: "maintenanceNotes",
               type: "nvarchar(max)",
               nullable: true);

/*            migrationBuilder.CreateTable(
                name: "maintenanceNotes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Shift = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    Safety = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quality = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Delivery = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cost = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Morale = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Graphic = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_maintenanceNotes", x => x.Id);
                });*/
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "maintenanceNotes");

/*            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "057a5e79-152c-43b9-bdbe-9015a9b5f481", null, "coordinator", "coordinator" },
                    { "27d509d5-b6f6-4a03-b6fa-d53a00bf95bd", null, "supervisor", "supervisor" },
                    { "a5ad4384-dbc6-42a5-9b20-3f8e7f3aab0f", null, "manager", "manager" },
                    { "a5ad952f-8141-4565-b883-49e7eadc3592", null, "auditor", "auditor" }
                });*/
        }
    }
}

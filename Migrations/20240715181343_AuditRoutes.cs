using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Van_Authentication.Migrations
{
    /// <inheritdoc />
    public partial class AuditRoutes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<string>(
                name: "Line",
                table: "WeldAudit",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Tcps",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Model",
                table: "Audits",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RepairProcedure",
                columns: table => new
                {
                    RepairProcedureId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RepairProcedureName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RepairProcedure", x => x.RepairProcedureId);
                });

            migrationBuilder.CreateTable(
                name: "WorkStations",
                columns: table => new
                {
                    WorkStationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkStationName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkStations", x => x.WorkStationId);
                });

            migrationBuilder.CreateTable(
                name: "PartModels",
                columns: table => new
                {
                    PartModelId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PartModelName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PartModelType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LotControl = table.Column<int>(type: "int", nullable: false),
                    WorkStationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartModels", x => x.PartModelId);
                    table.ForeignKey(
                        name: "FK_PartModels_WorkStations_WorkStationId",
                        column: x => x.WorkStationId,
                        principalTable: "WorkStations",
                        principalColumn: "WorkStationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AuditRoutes",
                columns: table => new
                {
                    AuditRouteId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuditRouteName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PartModelId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditRoutes", x => x.AuditRouteId);
                    table.ForeignKey(
                        name: "FK_AuditRoutes_PartModels_PartModelId",
                        column: x => x.PartModelId,
                        principalTable: "PartModels",
                        principalColumn: "PartModelId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AuditRouteWelds",
                columns: table => new
                {
                    AuditRouteId = table.Column<int>(type: "int", nullable: false),
                    WeldId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditRouteWelds", x => new { x.AuditRouteId, x.WeldId });
                    table.ForeignKey(
                        name: "FK_AuditRouteWelds_AuditRoutes_AuditRouteId",
                        column: x => x.AuditRouteId,
                        principalTable: "AuditRoutes",
                        principalColumn: "AuditRouteId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuditRouteWelds_Weld_WeldId",
                        column: x => x.WeldId,
                        principalTable: "Weld",
                        principalColumn: "WeldID",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_AuditRoutes_PartModelId",
                table: "AuditRoutes",
                column: "PartModelId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditRouteWelds_WeldId",
                table: "AuditRouteWelds",
                column: "WeldId");

            migrationBuilder.CreateIndex(
                name: "IX_PartModels_WorkStationId",
                table: "PartModels",
                column: "WorkStationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditRouteWelds");

            migrationBuilder.DropTable(
                name: "RepairProcedure");

            migrationBuilder.DropTable(
                name: "AuditRoutes");

            migrationBuilder.DropTable(
                name: "PartModels");

            migrationBuilder.DropTable(
                name: "WorkStations");

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

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Tcps");

            migrationBuilder.DropColumn(
                name: "Model",
                table: "Audits");

            migrationBuilder.AlterColumn<string>(
                name: "Line",
                table: "WeldAudit",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

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
    }
}

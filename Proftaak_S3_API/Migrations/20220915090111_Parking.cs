using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Proftaak_S3_API.Migrations
{
    public partial class Parking : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Gerage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    OpeningTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ClosingTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gerage", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Parking",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GerageId = table.Column<int>(type: "int", nullable: false),
                    CarId = table.Column<int>(type: "int", nullable: false),
                    ArrivalTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DepartureTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Price = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parking", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Parking_Auto_CarId",
                        column: x => x.CarId,
                        principalTable: "Auto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Parking_Gerage_GerageId",
                        column: x => x.GerageId,
                        principalTable: "Gerage",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Parking_CarId",
                table: "Parking",
                column: "CarId");

            migrationBuilder.CreateIndex(
                name: "IX_Parking_GerageId",
                table: "Parking",
                column: "GerageId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Parking");

            migrationBuilder.DropTable(
                name: "Gerage");
        }
    }
}

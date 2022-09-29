using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Proftaak_S3_API.Migrations
{
    public partial class nameFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Parking_Gerage_GerageId",
                table: "Parking");

            migrationBuilder.DropTable(
                name: "Gerage");

            migrationBuilder.RenameColumn(
                name: "GerageId",
                table: "Parking",
                newName: "GarageId");

            migrationBuilder.RenameIndex(
                name: "IX_Parking_GerageId",
                table: "Parking",
                newName: "IX_Parking_GarageId");

            migrationBuilder.CreateTable(
                name: "Garage",
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
                    table.PrimaryKey("PK_Garage", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Parking_Garage_GarageId",
                table: "Parking",
                column: "GarageId",
                principalTable: "Garage",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Parking_Garage_GarageId",
                table: "Parking");

            migrationBuilder.DropTable(
                name: "Garage");

            migrationBuilder.RenameColumn(
                name: "GarageId",
                table: "Parking",
                newName: "GerageId");

            migrationBuilder.RenameIndex(
                name: "IX_Parking_GarageId",
                table: "Parking",
                newName: "IX_Parking_GerageId");

            migrationBuilder.CreateTable(
                name: "Gerage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClosingTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OpeningTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Price = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gerage", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Parking_Gerage_GerageId",
                table: "Parking",
                column: "GerageId",
                principalTable: "Gerage",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

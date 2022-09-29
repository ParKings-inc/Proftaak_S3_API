using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Proftaak_S3_API.Migrations
{
    public partial class Parking_Space : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FreeSpace",
                table: "Garage",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MaxSpace",
                table: "Garage",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FreeSpace",
                table: "Garage");

            migrationBuilder.DropColumn(
                name: "MaxSpace",
                table: "Garage");
        }
    }
}

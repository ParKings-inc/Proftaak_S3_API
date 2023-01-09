using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Proftaak_S3_API.Migrations
{
    public partial class spaceStatus_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Space");

            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "Space",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "Space");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Space",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}

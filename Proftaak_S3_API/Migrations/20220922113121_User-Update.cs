using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Proftaak_S3_API.Migrations
{
    public partial class UserUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "User");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "User",
                newName: "SubId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SubId",
                table: "User",
                newName: "Name");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "User",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}

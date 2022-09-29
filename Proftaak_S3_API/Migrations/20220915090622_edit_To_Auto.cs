using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Proftaak_S3_API.Migrations
{
    public partial class edit_To_Auto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Auto");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Auto",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Auto_UserId",
                table: "Auto",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Auto_User_UserId",
                table: "Auto",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Auto_User_UserId",
                table: "Auto");

            migrationBuilder.DropIndex(
                name: "IX_Auto_UserId",
                table: "Auto");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Auto");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Auto",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}

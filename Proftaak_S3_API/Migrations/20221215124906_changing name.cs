using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Proftaak_S3_API.Migrations
{
    public partial class changingname : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SpeceStatus",
                table: "SpeceStatus");

            migrationBuilder.RenameTable(
                name: "SpeceStatus",
                newName: "SpaceStatus");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SpaceStatus",
                table: "SpaceStatus",
                column: "Id");

            migrationBuilder.InsertData(
                table: "SpaceStatus",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "Available" });

            migrationBuilder.InsertData(
                table: "SpaceStatus",
                columns: new[] { "Id", "Name" },
                values: new object[] { 2, "Occupied" });

            migrationBuilder.InsertData(
                table: "SpaceStatus",
                columns: new[] { "Id", "Name" },
                values: new object[] { 3, "Unavailable" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SpaceStatus",
                table: "SpaceStatus");

            migrationBuilder.DeleteData(
                table: "SpaceStatus",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "SpaceStatus",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "SpaceStatus",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.RenameTable(
                name: "SpaceStatus",
                newName: "SpeceStatus");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SpeceStatus",
                table: "SpeceStatus",
                column: "Id");
        }
    }
}

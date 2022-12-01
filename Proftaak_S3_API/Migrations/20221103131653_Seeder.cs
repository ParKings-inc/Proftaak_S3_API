using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Proftaak_S3_API.Migrations
{
    public partial class Seeder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "name",
                table: "SpaceType",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "SpaceType",
                newName: "Id");

            migrationBuilder.InsertData(
                table: "Garage",
                columns: new[] { "Id", "ClosingTime", "MaxPrice", "MaxSpace", "Name", "NormalPrice", "OpeningTime" },
                values: new object[] { 1, null, 5m, 5, "Test", 0m, null });

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "User" },
                    { 2, "Admin" }
                });

            migrationBuilder.InsertData(
                table: "Space",
                columns: new[] { "ID", "Floor", "GarageID", "Row", "Spot", "TypeId" },
                values: new object[,]
                {
                    { 1, 1, 1, "a", 1, 1 },
                    { 2, 1, 1, "a", 2, 1 },
                    { 3, 1, 1, "a", 3, 1 },
                    { 4, 1, 1, "b", 1, 1 },
                    { 5, 2, 1, "a", 1, 1 }
                });

            migrationBuilder.InsertData(
                table: "SpaceType",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Normal" },
                    { 2, "Electric" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Garage",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Space",
                keyColumn: "ID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Space",
                keyColumn: "ID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Space",
                keyColumn: "ID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Space",
                keyColumn: "ID",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Space",
                keyColumn: "ID",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "SpaceType",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "SpaceType",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "SpaceType",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "SpaceType",
                newName: "id");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace Sgart.Net5.ConsoleApp.Data.Migrations
{
    public partial class RenameField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Modified",
                table: "Todos",
                newName: "ModifiedUTC");

            migrationBuilder.RenameColumn(
                name: "Created",
                table: "Todos",
                newName: "CreatedUTC");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ModifiedUTC",
                table: "Todos",
                newName: "Modified");

            migrationBuilder.RenameColumn(
                name: "CreatedUTC",
                table: "Todos",
                newName: "Created");
        }
    }
}

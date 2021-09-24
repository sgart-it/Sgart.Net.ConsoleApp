using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sgart.Net5.ConsoleApp.Data.Migrations
{
    public partial class TodoModified : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Modified",
                table: "Todos",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2021, 09, 24, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Modified",
                table: "Todos");
        }
    }
}

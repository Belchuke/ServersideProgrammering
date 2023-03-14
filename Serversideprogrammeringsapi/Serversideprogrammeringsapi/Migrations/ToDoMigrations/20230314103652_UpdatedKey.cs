using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Serversideprogrammeringsapi.Migrations.ToDoMigrations
{
    /// <inheritdoc />
    public partial class UpdatedKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "KeyName",
                table: "ToDoLists");

            migrationBuilder.DropColumn(
                name: "KeyDescription",
                table: "ToDoListIteams");

            migrationBuilder.DropColumn(
                name: "KeyName",
                table: "ToDoListIteams");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "KeyName",
                table: "ToDoLists",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "KeyDescription",
                table: "ToDoListIteams",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "KeyName",
                table: "ToDoListIteams",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}

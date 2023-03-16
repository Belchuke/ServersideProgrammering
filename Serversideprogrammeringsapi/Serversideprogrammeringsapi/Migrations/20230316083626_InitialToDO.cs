using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Serversideprogrammeringsapi.Migrations
{
    /// <inheritdoc />
    public partial class InitialToDO : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ToDoLists",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataName = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    IVName = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    DataDescription = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    IVDescription = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Updated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Disabled = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ToDoLists", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ToDoListIteams",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataName = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    IVName = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    DataDescription = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    IVDescription = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    ToDoListId = table.Column<long>(type: "bigint", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Updated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Disabled = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ToDoListIteams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ToDoListIteams_ToDoLists_ToDoListId",
                        column: x => x.ToDoListId,
                        principalTable: "ToDoLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ToDoListIteams_ToDoListId",
                table: "ToDoListIteams",
                column: "ToDoListId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ToDoListIteams");

            migrationBuilder.DropTable(
                name: "ToDoLists");
        }
    }
}

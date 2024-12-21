using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
{
    migrationBuilder.CreateTable(
        name: "base_manipulators",
        columns: table => new
        {
            id = table.Column<Guid>(type: "uuid", nullable: false),
            name = table.Column<string>(type: "varchar(255)", nullable: false),
            type = table.Column<short>(type: "smallint", nullable: false),
            position = table.Column<string>(type: "varchar(255)", nullable: false)
        },
        constraints: table =>
        {
            table.PrimaryKey("pk_base_manipulators", x => x.id);
        });

    migrationBuilder.CreateTable(
        name: "industrial_manipulators",
        columns: table => new
        {
            id = table.Column<Guid>(type: "uuid", nullable: false),
            welds_amount = table.Column<int>(type: "integer", nullable: false, defaultValue: 0)
        },
        constraints: table =>
        {
            table.PrimaryKey("pk_industrial_manipulators", x => x.id); // Ensure unique primary key name
            table.ForeignKey(
                name: "fk_industrial_manipulators_base_manipulators_id",
                column: x => x.id,
                principalTable: "base_manipulators",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        });

    migrationBuilder.CreateTable(
        name: "service_manipulators",
        columns: table => new
        {
            id = table.Column<Guid>(type: "uuid", nullable: false),
            serves_amount = table.Column<int>(type: "integer", nullable: false, defaultValue: 0)
        },
        constraints: table =>
        {
            table.PrimaryKey("pk_service_manipulators", x => x.id);
            table.ForeignKey(
                name: "fk_service_manipulators_base_manipulators_id",
                column: x => x.id,
                principalTable: "base_manipulators",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        });
}


        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "industrial_manipulators");

            migrationBuilder.DropTable(
                name: "service_manipulators");

            migrationBuilder.DropTable(
                name: "base_manipulators");
        }
    }
}

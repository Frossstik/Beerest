using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Beerest.Migrations
{
    /// <inheritdoc />
    public partial class migrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "beers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Volume = table.Column<int>(type: "int", nullable: false),
                    VolumeAlc = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_beers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "bars",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BeerId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bars", x => x.Id);
                    table.ForeignKey(
                        name: "FK_bars_beers_BeerId",
                        column: x => x.BeerId,
                        principalTable: "beers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "persons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Age = table.Column<int>(type: "int", nullable: false),
                    BeerId = table.Column<int>(type: "int", nullable: true),
                    BarId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_persons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_persons_bars_BarId",
                        column: x => x.BarId,
                        principalTable: "bars",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_persons_beers_BeerId",
                        column: x => x.BeerId,
                        principalTable: "beers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_bars_BeerId",
                table: "bars",
                column: "BeerId");

            migrationBuilder.CreateIndex(
                name: "IX_persons_BarId",
                table: "persons",
                column: "BarId");

            migrationBuilder.CreateIndex(
                name: "IX_persons_BeerId",
                table: "persons",
                column: "BeerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "persons");

            migrationBuilder.DropTable(
                name: "bars");

            migrationBuilder.DropTable(
                name: "beers");
        }
    }
}

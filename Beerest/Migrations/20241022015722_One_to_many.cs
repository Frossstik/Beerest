using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Beerest.Migrations
{
    /// <inheritdoc />
    public partial class One_to_many : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_bars_beers_BeerId",
                table: "bars");

            migrationBuilder.DropIndex(
                name: "IX_bars_BeerId",
                table: "bars");

            migrationBuilder.DropColumn(
                name: "BeerId",
                table: "bars");

            migrationBuilder.AddColumn<int>(
                name: "BeerIds",
                table: "beers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_beers_BeerIds",
                table: "beers",
                column: "BeerIds");

            migrationBuilder.AddForeignKey(
                name: "FK_beers_bars_BeerIds",
                table: "beers",
                column: "BeerIds",
                principalTable: "bars",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_beers_bars_BeerIds",
                table: "beers");

            migrationBuilder.DropIndex(
                name: "IX_beers_BeerIds",
                table: "beers");

            migrationBuilder.DropColumn(
                name: "BeerIds",
                table: "beers");

            migrationBuilder.AddColumn<int>(
                name: "BeerId",
                table: "bars",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_bars_BeerId",
                table: "bars",
                column: "BeerId");

            migrationBuilder.AddForeignKey(
                name: "FK_bars_beers_BeerId",
                table: "bars",
                column: "BeerId",
                principalTable: "beers",
                principalColumn: "Id");
        }
    }
}

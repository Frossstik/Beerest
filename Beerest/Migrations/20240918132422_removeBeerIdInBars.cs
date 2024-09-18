using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Beerest.Migrations
{
    /// <inheritdoc />
    public partial class removeBeerIdInBars : Migration
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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

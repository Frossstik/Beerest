using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Beerest.Migrations
{
    /// <inheritdoc />
    public partial class somethingChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_persons_beers_BeerId",
                table: "persons");

            migrationBuilder.DropIndex(
                name: "IX_persons_BeerId",
                table: "persons");

            migrationBuilder.DropColumn(
                name: "BeerId",
                table: "persons");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                name: "BeerId",
                table: "persons",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_persons_BeerId",
                table: "persons",
                column: "BeerId");

            migrationBuilder.AddForeignKey(
                name: "FK_persons_beers_BeerId",
                table: "persons",
                column: "BeerId",
                principalTable: "beers",
                principalColumn: "Id");
        }
    }
}

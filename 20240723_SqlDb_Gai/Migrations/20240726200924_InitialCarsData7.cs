using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _20240723_SqlDb_Gai.Migrations
{
    /// <inheritdoc />
    public partial class InitialCarsData7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_cars_colors_ColorId",
                table: "cars");

            migrationBuilder.RenameColumn(
                name: "ColorId",
                table: "cars",
                newName: "FK_ColorId");

            migrationBuilder.RenameIndex(
                name: "IX_cars_ColorId",
                table: "cars",
                newName: "IX_cars_FK_ColorId");

            migrationBuilder.AddForeignKey(
                name: "FK_cars_colors_FK_ColorId",
                table: "cars",
                column: "FK_ColorId",
                principalTable: "colors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_cars_colors_FK_ColorId",
                table: "cars");

            migrationBuilder.RenameColumn(
                name: "FK_ColorId",
                table: "cars",
                newName: "ColorId");

            migrationBuilder.RenameIndex(
                name: "IX_cars_FK_ColorId",
                table: "cars",
                newName: "IX_cars_ColorId");

            migrationBuilder.AddForeignKey(
                name: "FK_cars_colors_ColorId",
                table: "cars",
                column: "ColorId",
                principalTable: "colors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

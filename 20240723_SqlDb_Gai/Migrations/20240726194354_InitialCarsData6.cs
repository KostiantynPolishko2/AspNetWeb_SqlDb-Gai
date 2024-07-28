using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _20240723_SqlDb_Gai.Migrations
{
    /// <inheritdoc />
    public partial class InitialCarsData6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_cars_marks_MarkId",
                table: "cars");

            migrationBuilder.RenameColumn(
                name: "MarkId",
                table: "cars",
                newName: "FK_MarkId");

            migrationBuilder.RenameIndex(
                name: "IX_cars_MarkId",
                table: "cars",
                newName: "IX_cars_FK_MarkId");

            migrationBuilder.AddForeignKey(
                name: "FK_cars_marks_FK_MarkId",
                table: "cars",
                column: "FK_MarkId",
                principalTable: "marks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_cars_marks_FK_MarkId",
                table: "cars");

            migrationBuilder.RenameColumn(
                name: "FK_MarkId",
                table: "cars",
                newName: "MarkId");

            migrationBuilder.RenameIndex(
                name: "IX_cars_FK_MarkId",
                table: "cars",
                newName: "IX_cars_MarkId");

            migrationBuilder.AddForeignKey(
                name: "FK_cars_marks_MarkId",
                table: "cars",
                column: "MarkId",
                principalTable: "marks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

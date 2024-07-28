using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _20240723_SqlDb_Gai.Migrations
{
    /// <inheritdoc />
    public partial class InitialCarsData3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_carsdata",
                table: "carsdata");

            migrationBuilder.RenameTable(
                name: "carsdata",
                newName: "cars");

            migrationBuilder.AddColumn<int>(
                name: "MarkId",
                table: "cars",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_cars",
                table: "cars",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "marks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaintThkMin = table.Column<int>(type: "int", nullable: false),
                    PaintThkMax = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_marks", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_cars_MarkId",
                table: "cars",
                column: "MarkId");

            migrationBuilder.AddForeignKey(
                name: "FK_cars_marks_MarkId",
                table: "cars",
                column: "MarkId",
                principalTable: "marks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_cars_marks_MarkId",
                table: "cars");

            migrationBuilder.DropTable(
                name: "marks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_cars",
                table: "cars");

            migrationBuilder.DropIndex(
                name: "IX_cars_MarkId",
                table: "cars");

            migrationBuilder.DropColumn(
                name: "MarkId",
                table: "cars");

            migrationBuilder.RenameTable(
                name: "cars",
                newName: "carsdata");

            migrationBuilder.AddPrimaryKey(
                name: "PK_carsdata",
                table: "carsdata",
                column: "Id");
        }
    }
}

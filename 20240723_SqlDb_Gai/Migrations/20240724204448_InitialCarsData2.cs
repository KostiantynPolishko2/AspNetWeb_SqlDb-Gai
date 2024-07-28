using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _20240723_SqlDb_Gai.Migrations
{
    /// <inheritdoc />
    public partial class InitialCarsData2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Cars",
                table: "Cars");

            migrationBuilder.RenameTable(
                name: "Cars",
                newName: "carsdata");

            migrationBuilder.AddPrimaryKey(
                name: "PK_carsdata",
                table: "carsdata",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_carsdata",
                table: "carsdata");

            migrationBuilder.RenameTable(
                name: "carsdata",
                newName: "Cars");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Cars",
                table: "Cars",
                column: "Id");
        }
    }
}

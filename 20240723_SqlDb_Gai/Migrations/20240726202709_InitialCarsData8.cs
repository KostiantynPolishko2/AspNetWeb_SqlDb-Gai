using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _20240723_SqlDb_Gai.Migrations
{
    /// <inheritdoc />
    public partial class InitialCarsData8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddCheckConstraint(
                name: "ValidVolume",
                table: "cars",
                sql: "Volume > 0 AND Volume <= 6");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "ValidVolume",
                table: "cars");
        }
    }
}

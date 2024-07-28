using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _20240723_SqlDb_Gai.Migrations
{
    /// <inheritdoc />
    public partial class InitialCarsData5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "Volume",
                table: "cars",
                type: "real",
                nullable: false,
                defaultValue: 0f,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AlterColumn<string>(
                name: "VinCode",
                table: "cars",
                type: "nvarchar(17)",
                maxLength: 17,
                nullable: false,
                defaultValue: "none",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Number",
                table: "cars",
                type: "nvarchar(8)",
                maxLength: 8,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Model",
                table: "cars",
                type: "nvarchar(8)",
                maxLength: 8,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IndexNumber",
                table: "cars",
                column: "Number",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IndexNumber",
                table: "cars");

            migrationBuilder.AlterColumn<float>(
                name: "Volume",
                table: "cars",
                type: "real",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real",
                oldDefaultValue: 0f);

            migrationBuilder.AlterColumn<string>(
                name: "VinCode",
                table: "cars",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(17)",
                oldMaxLength: 17,
                oldDefaultValue: "none");

            migrationBuilder.AlterColumn<string>(
                name: "Number",
                table: "cars",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(8)",
                oldMaxLength: 8);

            migrationBuilder.AlterColumn<string>(
                name: "Model",
                table: "cars",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(8)",
                oldMaxLength: 8);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SirketFiloTakip.Migrations
{
    /// <inheritdoc />
    public partial class AddGerekliEhliyetSinifiToArac : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GerekliEhliyetSinifi",
                table: "Araclar",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GerekliEhliyetSinifi",
                table: "Araclar");
        }
    }
}

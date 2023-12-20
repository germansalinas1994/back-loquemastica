using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class actualizoprecio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "precio",
                table: "publicacion");

            migrationBuilder.AddColumn<float>(
                name: "precio",
                table: "producto",
                type: "float",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "precio",
                table: "producto");

            migrationBuilder.AddColumn<float>(
                name: "precio",
                table: "publicacion",
                type: "float",
                nullable: true);
        }
    }
}

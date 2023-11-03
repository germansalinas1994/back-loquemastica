using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class enviopedido6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {


            migrationBuilder.CreateIndex(
                name: "id_envio_UNIQUE",
                table: "pedido",
                column: "id_envio",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "id_envio_UNIQUE",
                table: "pedido");

            migrationBuilder.CreateIndex(
                name: "id_envio_UNIQUE",
                table: "pedido",
                column: "id_envio");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class enviopedido3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "id_envio_FKPE",
                table: "pedido");

            migrationBuilder.AddForeignKey(
                name: "id_envio_UNIQUE",
                table: "pedido",
                column: "id_envio",
                principalTable: "envio",
                principalColumn: "idEnvio");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "id_envio_UNIQUE",
                table: "pedido");

            migrationBuilder.AddForeignKey(
                name: "id_envio_FKPE",
                table: "pedido",
                column: "id_envio",
                principalTable: "envio",
                principalColumn: "idEnvio");
        }
    }
}

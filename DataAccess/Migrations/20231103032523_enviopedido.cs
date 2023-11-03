using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class enviopedido : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "id_pedido_FKE",
                table: "envio");

            migrationBuilder.DropColumn(
                name: "Pedidocol",
                table: "pedido");

            migrationBuilder.AddColumn<int>(
                name: "id_envio",
                table: "pedido",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "id_envio_FKPE_idx",
                table: "pedido",
                column: "id_envio");

            migrationBuilder.AddForeignKey(
                name: "id_envio_FKPE",
                table: "pedido",
                column: "id_envio",
                principalTable: "envio",
                principalColumn: "idEnvio");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "id_envio_FKPE",
                table: "pedido");

            migrationBuilder.DropIndex(
                name: "id_envio_FKPE_idx",
                table: "pedido");

            migrationBuilder.DropColumn(
                name: "id_envio",
                table: "pedido");

            migrationBuilder.AddColumn<string>(
                name: "Pedidocol",
                table: "pedido",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "id_pedido_FKE",
                table: "envio",
                column: "id_pedido",
                principalTable: "pedido",
                principalColumn: "idPedido");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class actualizacionBase5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PedidoPublicacion_pedido_IdPedidoNavigationIdPedido",
                table: "PedidoPublicacion");

            migrationBuilder.DropForeignKey(
                name: "FK_PedidoPublicacion_publicacion_IdPublicacionNavigationIdPubli~",
                table: "PedidoPublicacion");

            migrationBuilder.DropIndex(
                name: "IX_PedidoPublicacion_IdPedidoNavigationIdPedido",
                table: "PedidoPublicacion");

            migrationBuilder.DropIndex(
                name: "IX_PedidoPublicacion_IdPublicacionNavigationIdPublicacion",
                table: "PedidoPublicacion");

            migrationBuilder.DropColumn(
                name: "IdPedidoNavigationIdPedido",
                table: "PedidoPublicacion");

            migrationBuilder.DropColumn(
                name: "IdPublicacionNavigationIdPublicacion",
                table: "PedidoPublicacion");

            migrationBuilder.CreateIndex(
                name: "IX_PedidoPublicacion_IdPedido",
                table: "PedidoPublicacion",
                column: "IdPedido");

            migrationBuilder.CreateIndex(
                name: "IX_PedidoPublicacion_IdPublicacion",
                table: "PedidoPublicacion",
                column: "IdPublicacion");

            migrationBuilder.AddForeignKey(
                name: "FK_PedidoPublicacion_pedido_IdPedido",
                table: "PedidoPublicacion",
                column: "IdPedido",
                principalTable: "pedido",
                principalColumn: "idPedido",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PedidoPublicacion_publicacion_IdPublicacion",
                table: "PedidoPublicacion",
                column: "IdPublicacion",
                principalTable: "publicacion",
                principalColumn: "idPublicacion",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PedidoPublicacion_pedido_IdPedido",
                table: "PedidoPublicacion");

            migrationBuilder.DropForeignKey(
                name: "FK_PedidoPublicacion_publicacion_IdPublicacion",
                table: "PedidoPublicacion");

            migrationBuilder.DropIndex(
                name: "IX_PedidoPublicacion_IdPedido",
                table: "PedidoPublicacion");

            migrationBuilder.DropIndex(
                name: "IX_PedidoPublicacion_IdPublicacion",
                table: "PedidoPublicacion");

            migrationBuilder.AddColumn<int>(
                name: "IdPedidoNavigationIdPedido",
                table: "PedidoPublicacion",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdPublicacionNavigationIdPublicacion",
                table: "PedidoPublicacion",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PedidoPublicacion_IdPedidoNavigationIdPedido",
                table: "PedidoPublicacion",
                column: "IdPedidoNavigationIdPedido");

            migrationBuilder.CreateIndex(
                name: "IX_PedidoPublicacion_IdPublicacionNavigationIdPublicacion",
                table: "PedidoPublicacion",
                column: "IdPublicacionNavigationIdPublicacion");

            migrationBuilder.AddForeignKey(
                name: "FK_PedidoPublicacion_pedido_IdPedidoNavigationIdPedido",
                table: "PedidoPublicacion",
                column: "IdPedidoNavigationIdPedido",
                principalTable: "pedido",
                principalColumn: "idPedido",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PedidoPublicacion_publicacion_IdPublicacionNavigationIdPubli~",
                table: "PedidoPublicacion",
                column: "IdPublicacionNavigationIdPublicacion",
                principalTable: "publicacion",
                principalColumn: "idPublicacion",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

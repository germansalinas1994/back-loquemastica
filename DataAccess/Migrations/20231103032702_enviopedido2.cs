using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class enviopedido2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "id_pedido_FKE_idx",
                table: "envio");

            migrationBuilder.DropColumn(
                name: "id_pedido",
                table: "envio");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "id_pedido",
                table: "envio",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "id_pedido_FKE_idx",
                table: "envio",
                column: "id_pedido");
        }
    }
}

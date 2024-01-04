using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class agregamosSucursalPedido : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "id_sucursal_pedido",
                table: "pedido",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "id_sucursal_pedido_Fk",
                table: "pedido",
                column: "id_sucursal_pedido");

            migrationBuilder.AddForeignKey(
                name: "id_sucursal_pedido_Fk",
                table: "pedido",
                column: "id_sucursal_pedido",
                principalTable: "sucursal",
                principalColumn: "idSucursal");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "id_sucursal_pedido_Fk",
                table: "pedido");

            migrationBuilder.DropIndex(
                name: "id_sucursal_pedido_Fk",
                table: "pedido");

            migrationBuilder.DropColumn(
                name: "id_sucursal_pedido",
                table: "pedido");
        }
    }
}

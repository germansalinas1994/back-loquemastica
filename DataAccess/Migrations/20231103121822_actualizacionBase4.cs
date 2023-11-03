using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class actualizacionBase4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PedidoPublicacion",
                columns: table => new
                {
                    IdPedidoPublicacion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    IdPedido = table.Column<int>(type: "int", nullable: false),
                    IdPublicacion = table.Column<int>(type: "int", nullable: false),
                    IdPedidoNavigationIdPedido = table.Column<int>(type: "int", nullable: false),
                    IdPublicacionNavigationIdPublicacion = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PedidoPublicacion", x => x.IdPedidoPublicacion);
                    table.ForeignKey(
                        name: "FK_PedidoPublicacion_pedido_IdPedidoNavigationIdPedido",
                        column: x => x.IdPedidoNavigationIdPedido,
                        principalTable: "pedido",
                        principalColumn: "idPedido",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PedidoPublicacion_publicacion_IdPublicacionNavigationIdPubli~",
                        column: x => x.IdPublicacionNavigationIdPublicacion,
                        principalTable: "publicacion",
                        principalColumn: "idPublicacion",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_PedidoPublicacion_IdPedidoNavigationIdPedido",
                table: "PedidoPublicacion",
                column: "IdPedidoNavigationIdPedido");

            migrationBuilder.CreateIndex(
                name: "IX_PedidoPublicacion_IdPublicacionNavigationIdPublicacion",
                table: "PedidoPublicacion",
                column: "IdPublicacionNavigationIdPublicacion");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PedidoPublicacion");
        }
    }
}

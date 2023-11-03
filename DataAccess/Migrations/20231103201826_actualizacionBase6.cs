using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class actualizacionBase6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropTable(
                name: "pago");

            migrationBuilder.DropTable(
                name: "PedidoPublicacion");

            migrationBuilder.DropTable(
                name: "pedido");
            migrationBuilder.DropTable(
 name: "envio");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "pago",
                columns: table => new
                {
                    idPago = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    fechaAlta = table.Column<DateTime>(type: "date", nullable: false),
                    nroTransaccion = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.idPago);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "pedido",
                columns: table => new
                {
                    idPedido = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    envio = table.Column<sbyte>(type: "tinyint", nullable: true),
                    fechaAlta = table.Column<DateTime>(type: "date", nullable: true),
                    fechaBaja = table.Column<DateTime>(type: "date", nullable: true),
                    fechaModificacion = table.Column<DateTime>(type: "date", nullable: true),
                    id_envio = table.Column<int>(type: "int", nullable: true),
                    id_estadoPedido = table.Column<int>(type: "int", nullable: true),
                    id_pago = table.Column<int>(type: "int", nullable: true),
                    id_tipoPedido = table.Column<int>(type: "int", nullable: true),
                    id_usuario = table.Column<int>(type: "int", nullable: true),
                    nroPedido = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.idPedido);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "envio",
                columns: table => new
                {
                    idEnvio = table.Column<int>(type: "int", nullable: false),
                    id_estadoEnvio = table.Column<int>(type: "int", nullable: true),
                    fechaEnvio = table.Column<DateTime>(type: "date", nullable: true),
                    precio = table.Column<float>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.idEnvio);
                    table.ForeignKey(
                        name: "id_envio_UNIQUE",
                        column: x => x.idEnvio,
                        principalTable: "pedido",
                        principalColumn: "idPedido",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "id_estadoEnvio_FKE",
                        column: x => x.id_estadoEnvio,
                        principalTable: "estadoenvio",
                        principalColumn: "idEstadoEnvio");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PedidoPublicacion",
                columns: table => new
                {
                    IdPedidoPublicacion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    IdPedido = table.Column<int>(type: "int", nullable: false),
                    IdPublicacion = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PedidoPublicacion", x => x.IdPedidoPublicacion);
                    table.ForeignKey(
                        name: "FK_PedidoPublicacion_pedido_IdPedido",
                        column: x => x.IdPedido,
                        principalTable: "pedido",
                        principalColumn: "idPedido",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PedidoPublicacion_publicacion_IdPublicacion",
                        column: x => x.IdPublicacion,
                        principalTable: "publicacion",
                        principalColumn: "idPublicacion",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "id_estadoEnvio_FKE_idx",
                table: "envio",
                column: "id_estadoEnvio");

            migrationBuilder.CreateIndex(
                name: "id_envio_UNIQUE",
                table: "pedido",
                column: "id_envio",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "id_estadoPedido_idx",
                table: "pedido",
                column: "id_estadoPedido");

            migrationBuilder.CreateIndex(
                name: "id_pago_FKP_idx",
                table: "pedido",
                column: "id_pago");

            migrationBuilder.CreateIndex(
                name: "id_tipoPedido_FKP_idx",
                table: "pedido",
                column: "id_tipoPedido");

            migrationBuilder.CreateIndex(
                name: "id_usuario_FKP_idx",
                table: "pedido",
                column: "id_usuario");

            migrationBuilder.CreateIndex(
                name: "IX_PedidoPublicacion_IdPedido",
                table: "PedidoPublicacion",
                column: "IdPedido");

            migrationBuilder.CreateIndex(
                name: "IX_PedidoPublicacion_IdPublicacion",
                table: "PedidoPublicacion",
                column: "IdPublicacion");
        }
    }
}

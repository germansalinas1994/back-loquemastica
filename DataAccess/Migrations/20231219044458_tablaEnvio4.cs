using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class tablaEnvio4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Envio",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    id_pedido = table.Column<int>(type: "int", nullable: false),
                    fechaAlta = table.Column<DateTime>(type: "datetime", nullable: false),
                    fechaBaja = table.Column<DateTime>(type: "datetime", nullable: true),
                    fechaModificacion = table.Column<DateTime>(type: "datetime", nullable: false),
                    IdEstadoEnvio = table.Column<int>(type: "int", nullable: false),
                    IdDomicilio = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "FK_Envio_domicilio_IdDomicilio",
                        column: x => x.IdDomicilio,
                        principalTable: "domicilio",
                        principalColumn: "idDomicilio",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Envio_estadoenvio_IdEstadoEnvio",
                        column: x => x.IdEstadoEnvio,
                        principalTable: "estadoenvio",
                        principalColumn: "idEstadoEnvio",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "id_Pedido_FKPedidoEnvio",
                        column: x => x.id_pedido,
                        principalTable: "pedido",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "id_Domicilio_FKDomicilio",
                table: "Envio",
                column: "id_pedido");

            migrationBuilder.CreateIndex(
                name: "id_Pedido_FKPedidoEnvio",
                table: "Envio",
                column: "id_pedido",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Envio_IdDomicilio",
                table: "Envio",
                column: "IdDomicilio");

            migrationBuilder.CreateIndex(
                name: "IX_Envio_IdEstadoEnvio",
                table: "Envio",
                column: "IdEstadoEnvio");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Envio");
        }
    }
}

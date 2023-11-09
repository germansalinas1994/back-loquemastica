using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class agregoTabladepedido : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "pedido",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    id_usuario = table.Column<int>(type: "int", nullable: true),
                    fechaAlta = table.Column<DateTime>(type: "datetime", nullable: false),
                    fechaBaja = table.Column<DateTime>(type: "datetime", nullable: true),
                    fechaModificacion = table.Column<DateTime>(type: "datetime", nullable: false),
                    orden_MercadoPago = table.Column<int>(type: "int", nullable: true),
                    total = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "id_Usuario_FKPedido",
                        column: x => x.id_usuario,
                        principalTable: "usuario",
                        principalColumn: "idUsuario");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "id_Usuario_FKPedido",
                table: "pedido",
                column: "id_usuario");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "pedido");
        }
    }
}

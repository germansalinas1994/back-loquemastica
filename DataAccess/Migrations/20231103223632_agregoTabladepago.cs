using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class agregoTabladepago : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "pago",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    id_pedido = table.Column<int>(type: "int", nullable: false),
                    fechaAlta = table.Column<DateTime>(type: "datetime", nullable: false),
                    fechaBaja = table.Column<DateTime>(type: "datetime", nullable: true),
                    fechaModificacion = table.Column<DateTime>(type: "datetime", nullable: false),
                    estadoPago = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    detallePago = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    total = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "id_Pedido_FKPedido",
                        column: x => x.id_pedido,
                        principalTable: "pedido",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "id_Pedido_FKPedido",
                table: "pago",
                column: "id_pedido",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "pago");
        }
    }
}

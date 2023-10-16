using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class migracioninicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "categoria",
                columns: table => new
                {
                    idCategoria = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    descripcion = table.Column<string>(type: "longtext", nullable: true),
                    fechaDesde = table.Column<DateTime>(type: "date", nullable: true),
                    fechaHasta = table.Column<DateTime>(type: "date", nullable: true),
                    fechaModificacion = table.Column<DateTime>(type: "date", nullable: true),
                    nombre = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.idCategoria);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "estadoenvio",
                columns: table => new
                {
                    idEstadoEnvio = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    descripcion = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.idEstadoEnvio);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "estadopedido",
                columns: table => new
                {
                    idEstadoPedido = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    descipcion = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.idEstadoPedido);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

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
                    id_usuario = table.Column<int>(type: "int", nullable: true),
                    id_pago = table.Column<int>(type: "int", nullable: true),
                    id_tipoPedido = table.Column<int>(type: "int", nullable: true),
                    id_publicacion = table.Column<int>(type: "int", nullable: true),
                    nroPedido = table.Column<int>(type: "int", nullable: true),
                    fechaAlta = table.Column<DateTime>(type: "date", nullable: true),
                    fechaModificacion = table.Column<DateTime>(type: "date", nullable: true),
                    fechaBaja = table.Column<DateTime>(type: "date", nullable: true),
                    id_estadoPedido = table.Column<int>(type: "int", nullable: true),
                    envio = table.Column<sbyte>(type: "tinyint", nullable: true),
                    Pedidocol = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.idPedido);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "sucursal",
                columns: table => new
                {
                    idSucursal = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    direccion = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    nombre = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.idSucursal);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tipopedido",
                columns: table => new
                {
                    idTipoPedido = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    descipcion = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.idTipoPedido);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "usuario",
                columns: table => new
                {
                    idUsuario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    nombre = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: true),
                    apellido = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: true),
                    email = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    dni = table.Column<int>(type: "int", nullable: true),
                    telefono = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.idUsuario);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "producto",
                columns: table => new
                {
                    idProducto = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    descripcion = table.Column<string>(type: "longtext", nullable: true),
                    id_categoria = table.Column<int>(type: "int", nullable: true),
                    fechaAlta = table.Column<DateTime>(type: "date", nullable: true),
                    fechaBaja = table.Column<DateTime>(type: "date", nullable: true),
                    fechaModificacion = table.Column<DateTime>(type: "date", nullable: true),
                    urlImagen = table.Column<string>(type: "longtext", nullable: true),
                    nombre = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.idProducto);
                    table.ForeignKey(
                        name: "id_Categoria_FKP",
                        column: x => x.id_categoria,
                        principalTable: "categoria",
                        principalColumn: "idCategoria");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "envio",
                columns: table => new
                {
                    idEnvio = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    id_estadoEnvio = table.Column<int>(type: "int", nullable: true),
                    precio = table.Column<float>(type: "float", nullable: true),
                    fechaEnvio = table.Column<DateTime>(type: "date", nullable: true),
                    id_pedido = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.idEnvio);
                    table.ForeignKey(
                        name: "id_estadoEnvio_FKE",
                        column: x => x.id_estadoEnvio,
                        principalTable: "estadoenvio",
                        principalColumn: "idEstadoEnvio");
                    table.ForeignKey(
                        name: "id_pedido_FKE",
                        column: x => x.id_pedido,
                        principalTable: "pedido",
                        principalColumn: "idPedido");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "domicilio",
                columns: table => new
                {
                    idDomicilio = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    altura = table.Column<int>(type: "int", nullable: true),
                    calle = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: true),
                    aclaracion = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: true),
                    departamento = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: true),
                    id_usuario = table.Column<int>(type: "int", nullable: false),
                    Domiciliocol = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.idDomicilio);
                    table.ForeignKey(
                        name: "id_Usuario_FKD",
                        column: x => x.id_usuario,
                        principalTable: "usuario",
                        principalColumn: "idUsuario");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "publicacion",
                columns: table => new
                {
                    idPublicacion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    fechaDesde = table.Column<DateTime>(type: "date", nullable: true),
                    fechaHasta = table.Column<DateTime>(type: "date", nullable: true),
                    precio = table.Column<float>(type: "float", nullable: true),
                    id_producto = table.Column<int>(type: "int", nullable: true),
                    stock = table.Column<int>(type: "int", nullable: true),
                    id_sucursal = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.idPublicacion);
                    table.ForeignKey(
                        name: "id_producto_FKP",
                        column: x => x.id_producto,
                        principalTable: "producto",
                        principalColumn: "idProducto");
                    table.ForeignKey(
                        name: "id_sucursal_FKP",
                        column: x => x.id_sucursal,
                        principalTable: "sucursal",
                        principalColumn: "idSucursal");
                },
                comment: "						")
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "id_Usuario_FKD_idx",
                table: "domicilio",
                column: "id_usuario");

            migrationBuilder.CreateIndex(
                name: "id_estadoEnvio_FKE_idx",
                table: "envio",
                column: "id_estadoEnvio");

            migrationBuilder.CreateIndex(
                name: "id_pedido_FKE_idx",
                table: "envio",
                column: "id_pedido");

            migrationBuilder.CreateIndex(
                name: "id_estadoPedido_idx",
                table: "pedido",
                column: "id_estadoPedido");

            migrationBuilder.CreateIndex(
                name: "id_pago_FKP_idx",
                table: "pedido",
                column: "id_pago");

            migrationBuilder.CreateIndex(
                name: "id_publicacion_FKP_idx",
                table: "pedido",
                column: "id_publicacion");

            migrationBuilder.CreateIndex(
                name: "id_tipoPedido_FKP_idx",
                table: "pedido",
                column: "id_tipoPedido");

            migrationBuilder.CreateIndex(
                name: "id_usuario_FKP_idx",
                table: "pedido",
                column: "id_usuario");

            migrationBuilder.CreateIndex(
                name: "id_Categoria_FKP_idx",
                table: "producto",
                column: "id_categoria");

            migrationBuilder.CreateIndex(
                name: "id_producto_FKP_idx",
                table: "publicacion",
                column: "id_producto");

            migrationBuilder.CreateIndex(
                name: "id_sucursal_FKP_idx",
                table: "publicacion",
                column: "id_sucursal");

            migrationBuilder.CreateIndex(
                name: "email_UNIQUE",
                table: "usuario",
                column: "email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "domicilio");

            migrationBuilder.DropTable(
                name: "envio");

            migrationBuilder.DropTable(
                name: "estadopedido");

            migrationBuilder.DropTable(
                name: "pago");

            migrationBuilder.DropTable(
                name: "publicacion");

            migrationBuilder.DropTable(
                name: "tipopedido");

            migrationBuilder.DropTable(
                name: "usuario");

            migrationBuilder.DropTable(
                name: "estadoenvio");

            migrationBuilder.DropTable(
                name: "pedido");

            migrationBuilder.DropTable(
                name: "producto");

            migrationBuilder.DropTable(
                name: "sucursal");

            migrationBuilder.DropTable(
                name: "categoria");
        }
    }
}

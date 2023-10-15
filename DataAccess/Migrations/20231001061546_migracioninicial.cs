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
            migrationBuilder.DropTable(
                name: "__EFMigrationsHistory");

            migrationBuilder.DropTable(
                name: "envio");

            migrationBuilder.DropTable(
                name: "domicilio");

            migrationBuilder.DropTable(
                name: "pedido");

            migrationBuilder.DropTable(
                name: "estadopedido");

            migrationBuilder.DropTable(
                name: "publicacion");

            migrationBuilder.DropTable(
                name: "sucursal");

            migrationBuilder.DropTable(
                name: "usuario");

            migrationBuilder.DropTable(
                name: "estadopublicacion");

            migrationBuilder.DropTable(
                name: "producto");

            migrationBuilder.DropTable(
                name: "rol");

            migrationBuilder.DropTable(
                name: "categoria");


            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "__EFMigrationsHistory",
                columns: table => new
                {
                    MigrationId = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    ProductVersion = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.MigrationId);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "categoria",
                columns: table => new
                {
                    idCategoria = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    descripcion = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.idCategoria);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "estadopedido",
                columns: table => new
                {
                    idEstadoPedido = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    descripcion = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.idEstadoPedido);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "estadopublicacion",
                columns: table => new
                {
                    idEstadoPublicacion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    descripcion = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.idEstadoPublicacion);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "rol",
                columns: table => new
                {
                    idRol = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    descripcion = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.idRol);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "sucursal",
                columns: table => new
                {
                    idSucursal = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.idSucursal);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "producto",
                columns: table => new
                {
                    idProducto = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    descripcion = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: true),
                    id_categoria = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.idProducto);
                    table.ForeignKey(
                        name: "id_categoria_FK",
                        column: x => x.id_categoria,
                        principalTable: "categoria",
                        principalColumn: "idCategoria");
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
                    email = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false),
                    password = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false),
                    telefono = table.Column<int>(type: "int", nullable: true),
                    dni = table.Column<int>(type: "int", nullable: true),
                    id_rol = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.idUsuario);
                    table.ForeignKey(
                        name: "id_rol_FK",
                        column: x => x.id_rol,
                        principalTable: "rol",
                        principalColumn: "idRol");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "publicacion",
                columns: table => new
                {
                    idPublicacion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    fecha_desde = table.Column<DateTime>(type: "datetime", nullable: false),
                    fecha_hasta = table.Column<DateTime>(type: "datetime", nullable: true),
                    precio = table.Column<float>(type: "float", nullable: true),
                    id_producto = table.Column<int>(type: "int", nullable: false),
                    id_estado = table.Column<int>(type: "int", nullable: true),
                    stock = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.idPublicacion);
                    table.ForeignKey(
                        name: "id_estadopublicacion_fk1",
                        column: x => x.id_estado,
                        principalTable: "estadopublicacion",
                        principalColumn: "idEstadoPublicacion");
                    table.ForeignKey(
                        name: "id_producto_fkp",
                        column: x => x.id_producto,
                        principalTable: "producto",
                        principalColumn: "idProducto");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "domicilio",
                columns: table => new
                {
                    idDomicilio = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    altura = table.Column<int>(type: "int", nullable: false),
                    calle = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    id_usuario = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.idDomicilio);
                    table.ForeignKey(
                        name: "id_usuario_FK2",
                        column: x => x.id_usuario,
                        principalTable: "usuario",
                        principalColumn: "idUsuario");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "pedido",
                columns: table => new
                {
                    idPedido = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    token = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: true),
                    id_publicacion = table.Column<int>(type: "int", nullable: false),
                    cantidad = table.Column<int>(type: "int", nullable: false),
                    id_usuario = table.Column<int>(type: "int", nullable: false),
                    id_tipo_entrega = table.Column<int>(type: "int", nullable: false),
                    id_estado_pedido = table.Column<int>(type: "int", nullable: false),
                    id_sucursal = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.idPedido);
                    table.ForeignKey(
                        name: "id_estado_pedido_FK",
                        column: x => x.id_estado_pedido,
                        principalTable: "estadopedido",
                        principalColumn: "idEstadoPedido");
                    table.ForeignKey(
                        name: "id_publicacion_FK",
                        column: x => x.id_publicacion,
                        principalTable: "publicacion",
                        principalColumn: "idPublicacion");
                    table.ForeignKey(
                        name: "id_sucursal_FK",
                        column: x => x.id_sucursal,
                        principalTable: "sucursal",
                        principalColumn: "idSucursal");
                    table.ForeignKey(
                        name: "id_usuario_FK",
                        column: x => x.id_usuario,
                        principalTable: "usuario",
                        principalColumn: "idUsuario");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "envio",
                columns: table => new
                {
                    idEnvio = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    id_pedido = table.Column<int>(type: "int", nullable: false),
                    codigo_seguimiento = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false),
                    id_domicilio = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.idEnvio);
                    table.ForeignKey(
                        name: "id_domicilio",
                        column: x => x.id_domicilio,
                        principalTable: "domicilio",
                        principalColumn: "idDomicilio");
                    table.ForeignKey(
                        name: "id_pedido_FK",
                        column: x => x.id_pedido,
                        principalTable: "pedido",
                        principalColumn: "idPedido");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "id_usuario_FK_idx",
                table: "domicilio",
                column: "id_usuario");

            migrationBuilder.CreateIndex(
                name: "id_domicilio_idx",
                table: "envio",
                column: "id_domicilio");

            migrationBuilder.CreateIndex(
                name: "id_pedido_FK_idx",
                table: "envio",
                column: "id_pedido");

            migrationBuilder.CreateIndex(
                name: "id_pedido_UNIQUE",
                table: "envio",
                column: "id_pedido",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "id_estado_pedido_FK_idx",
                table: "pedido",
                column: "id_estado_pedido");

            migrationBuilder.CreateIndex(
                name: "id_publicacion_FK_idx",
                table: "pedido",
                column: "id_publicacion");

            migrationBuilder.CreateIndex(
                name: "id_sucursal_FK_idx",
                table: "pedido",
                column: "id_sucursal");

            migrationBuilder.CreateIndex(
                name: "id_tipo_entrega_FK_idx",
                table: "pedido",
                column: "id_tipo_entrega");

            migrationBuilder.CreateIndex(
                name: "id_usuario_FK_idx1",
                table: "pedido",
                column: "id_usuario");

            migrationBuilder.CreateIndex(
                name: "id_categoria_FK_idx",
                table: "producto",
                column: "id_categoria");

            migrationBuilder.CreateIndex(
                name: "id_estadopublicacion_fk1_idx",
                table: "publicacion",
                column: "id_estado");

            migrationBuilder.CreateIndex(
                name: "id_producto_fkp_idx",
                table: "publicacion",
                column: "id_producto");

            migrationBuilder.CreateIndex(
                name: "dni_UNIQUE",
                table: "usuario",
                column: "dni",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "email_UNIQUE",
                table: "usuario",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "id_rol_FK_idx",
                table: "usuario",
                column: "id_rol");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "__EFMigrationsHistory");

            migrationBuilder.DropTable(
                name: "envio");

            migrationBuilder.DropTable(
                name: "domicilio");

            migrationBuilder.DropTable(
                name: "pedido");

            migrationBuilder.DropTable(
                name: "estadopedido");

            migrationBuilder.DropTable(
                name: "publicacion");

            migrationBuilder.DropTable(
                name: "sucursal");

            migrationBuilder.DropTable(
                name: "usuario");

            migrationBuilder.DropTable(
                name: "estadopublicacion");

            migrationBuilder.DropTable(
                name: "producto");

            migrationBuilder.DropTable(
                name: "rol");

            migrationBuilder.DropTable(
                name: "categoria");
        }
    }
}

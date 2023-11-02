﻿// <auto-generated />
using System;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DataAccess.Migrations
{
    [DbContext(typeof(DbveterinariaContext))]
    partial class DbveterinariaContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("DataAccess.Entities.Categoria", b =>
                {
                    b.Property<int>("IdCategoria")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("idCategoria");

                    b.Property<string>("Descripcion")
                        .HasColumnType("longtext")
                        .HasColumnName("descripcion");

                    b.Property<DateTime>("FechaDesde")
                        .HasColumnType("date")
                        .HasColumnName("fechaDesde");

                    b.Property<DateTime?>("FechaHasta")
                        .HasColumnType("date")
                        .HasColumnName("fechaHasta");

                    b.Property<DateTime>("FechaModificacion")
                        .HasColumnType("date")
                        .HasColumnName("fechaModificacion");

                    b.Property<string>("Nombre")
                        .HasMaxLength(45)
                        .HasColumnType("varchar(45)")
                        .HasColumnName("nombre");

                    b.HasKey("IdCategoria")
                        .HasName("PRIMARY");

                    b.ToTable("categoria", (string)null);
                });

            modelBuilder.Entity("DataAccess.Entities.Domicilio", b =>
                {
                    b.Property<int>("IdDomicilio")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("idDomicilio");

                    b.Property<string>("Aclaracion")
                        .HasMaxLength(45)
                        .HasColumnType("varchar(45)")
                        .HasColumnName("aclaracion");

                    b.Property<int?>("Altura")
                        .HasColumnType("int")
                        .HasColumnName("altura");

                    b.Property<string>("Calle")
                        .HasMaxLength(45)
                        .HasColumnType("varchar(45)")
                        .HasColumnName("calle");

                    b.Property<string>("CodigoPostal")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Departamento")
                        .HasMaxLength(45)
                        .HasColumnType("varchar(45)")
                        .HasColumnName("departamento");

                    b.Property<DateTime>("FechaActualizacion")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("FechaDesde")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("FechaHasta")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("IdUsuario")
                        .HasColumnType("int")
                        .HasColumnName("id_usuario");

                    b.HasKey("IdDomicilio")
                        .HasName("PRIMARY");

                    b.HasIndex(new[] { "IdUsuario" }, "id_Usuario_FKD_idx");

                    b.ToTable("domicilio", (string)null);
                });

            modelBuilder.Entity("DataAccess.Entities.Envio", b =>
                {
                    b.Property<int>("IdEnvio")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("idEnvio");

                    b.Property<DateTime?>("FechaEnvio")
                        .HasColumnType("date")
                        .HasColumnName("fechaEnvio");

                    b.Property<int?>("IdEstadoEnvio")
                        .HasColumnType("int")
                        .HasColumnName("id_estadoEnvio");

                    b.Property<int?>("IdPedido")
                        .HasColumnType("int")
                        .HasColumnName("id_pedido");

                    b.Property<float?>("Precio")
                        .HasColumnType("float")
                        .HasColumnName("precio");

                    b.HasKey("IdEnvio")
                        .HasName("PRIMARY");

                    b.HasIndex(new[] { "IdEstadoEnvio" }, "id_estadoEnvio_FKE_idx");

                    b.HasIndex(new[] { "IdPedido" }, "id_pedido_FKE_idx");

                    b.ToTable("envio", (string)null);
                });

            modelBuilder.Entity("DataAccess.Entities.Estadoenvio", b =>
                {
                    b.Property<int>("IdEstadoEnvio")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("idEstadoEnvio");

                    b.Property<string>("Descripcion")
                        .HasMaxLength(45)
                        .HasColumnType("varchar(45)")
                        .HasColumnName("descripcion");

                    b.HasKey("IdEstadoEnvio")
                        .HasName("PRIMARY");

                    b.ToTable("estadoenvio", (string)null);
                });

            modelBuilder.Entity("DataAccess.Entities.Estadopedido", b =>
                {
                    b.Property<int>("IdEstadoPedido")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("idEstadoPedido");

                    b.Property<string>("Descipcion")
                        .HasMaxLength(45)
                        .HasColumnType("varchar(45)")
                        .HasColumnName("descipcion");

                    b.HasKey("IdEstadoPedido")
                        .HasName("PRIMARY");

                    b.ToTable("estadopedido", (string)null);
                });

            modelBuilder.Entity("DataAccess.Entities.Pago", b =>
                {
                    b.Property<int>("IdPago")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("idPago");

                    b.Property<DateTime>("FechaAlta")
                        .HasColumnType("date")
                        .HasColumnName("fechaAlta");

                    b.Property<int>("NroTransaccion")
                        .HasColumnType("int")
                        .HasColumnName("nroTransaccion");

                    b.HasKey("IdPago")
                        .HasName("PRIMARY");

                    b.ToTable("pago", (string)null);
                });

            modelBuilder.Entity("DataAccess.Entities.Pedido", b =>
                {
                    b.Property<int>("IdPedido")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("idPedido");

                    b.Property<sbyte?>("Envio")
                        .HasColumnType("tinyint")
                        .HasColumnName("envio");

                    b.Property<DateTime?>("FechaAlta")
                        .HasColumnType("date")
                        .HasColumnName("fechaAlta");

                    b.Property<DateTime?>("FechaBaja")
                        .HasColumnType("date")
                        .HasColumnName("fechaBaja");

                    b.Property<DateTime?>("FechaModificacion")
                        .HasColumnType("date")
                        .HasColumnName("fechaModificacion");

                    b.Property<int?>("IdEstadoPedido")
                        .HasColumnType("int")
                        .HasColumnName("id_estadoPedido");

                    b.Property<int?>("IdPago")
                        .HasColumnType("int")
                        .HasColumnName("id_pago");

                    b.Property<int?>("IdPublicacion")
                        .HasColumnType("int")
                        .HasColumnName("id_publicacion");

                    b.Property<int?>("IdTipoPedido")
                        .HasColumnType("int")
                        .HasColumnName("id_tipoPedido");

                    b.Property<int?>("IdUsuario")
                        .HasColumnType("int")
                        .HasColumnName("id_usuario");

                    b.Property<int?>("NroPedido")
                        .HasColumnType("int")
                        .HasColumnName("nroPedido");

                    b.Property<string>("Pedidocol")
                        .HasMaxLength(45)
                        .HasColumnType("varchar(45)");

                    b.HasKey("IdPedido")
                        .HasName("PRIMARY");

                    b.HasIndex(new[] { "IdEstadoPedido" }, "id_estadoPedido_idx");

                    b.HasIndex(new[] { "IdPago" }, "id_pago_FKP_idx");

                    b.HasIndex(new[] { "IdPublicacion" }, "id_publicacion_FKP_idx");

                    b.HasIndex(new[] { "IdTipoPedido" }, "id_tipoPedido_FKP_idx");

                    b.HasIndex(new[] { "IdUsuario" }, "id_usuario_FKP_idx");

                    b.ToTable("pedido", (string)null);
                });

            modelBuilder.Entity("DataAccess.Entities.Producto", b =>
                {
                    b.Property<int>("IdProducto")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("idProducto");

                    b.Property<string>("Descripcion")
                        .HasColumnType("longtext")
                        .HasColumnName("descripcion");

                    b.Property<DateTime?>("FechaAlta")
                        .HasColumnType("date")
                        .HasColumnName("fechaAlta");

                    b.Property<DateTime?>("FechaBaja")
                        .HasColumnType("date")
                        .HasColumnName("fechaBaja");

                    b.Property<DateTime?>("FechaModificacion")
                        .HasColumnType("date")
                        .HasColumnName("fechaModificacion");

                    b.Property<int?>("IdCategoria")
                        .HasColumnType("int")
                        .HasColumnName("id_categoria");

                    b.Property<string>("Nombre")
                        .HasColumnType("longtext")
                        .HasColumnName("nombre");

                    b.Property<string>("UrlImagen")
                        .HasColumnType("longtext")
                        .HasColumnName("urlImagen");

                    b.HasKey("IdProducto")
                        .HasName("PRIMARY");

                    b.HasIndex(new[] { "IdCategoria" }, "id_Categoria_FKP_idx");

                    b.ToTable("producto", (string)null);
                });

            modelBuilder.Entity("DataAccess.Entities.Publicacion", b =>
                {
                    b.Property<int>("IdPublicacion")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("idPublicacion");

                    b.Property<DateTime?>("FechaDesde")
                        .HasColumnType("date")
                        .HasColumnName("fechaDesde");

                    b.Property<DateTime?>("FechaHasta")
                        .HasColumnType("date")
                        .HasColumnName("fechaHasta");

                    b.Property<int?>("IdProducto")
                        .HasColumnType("int")
                        .HasColumnName("id_producto");

                    b.Property<int?>("IdSucursal")
                        .HasColumnType("int")
                        .HasColumnName("id_sucursal");

                    b.Property<float?>("Precio")
                        .HasColumnType("float")
                        .HasColumnName("precio");

                    b.Property<int?>("Stock")
                        .HasColumnType("int")
                        .HasColumnName("stock");

                    b.HasKey("IdPublicacion")
                        .HasName("PRIMARY");

                    b.HasIndex(new[] { "IdProducto" }, "id_producto_FKP_idx");

                    b.HasIndex(new[] { "IdSucursal" }, "id_sucursal_FKP_idx");

                    b.ToTable("publicacion", null, t =>
                        {
                            t.HasComment("						");
                        });
                });

            modelBuilder.Entity("DataAccess.Entities.Sucursal", b =>
                {
                    b.Property<int>("IdSucursal")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("idSucursal");

                    b.Property<string>("Direccion")
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("direccion");

                    b.Property<string>("Nombre")
                        .HasMaxLength(45)
                        .HasColumnType("varchar(45)")
                        .HasColumnName("nombre");

                    b.HasKey("IdSucursal")
                        .HasName("PRIMARY");

                    b.ToTable("sucursal", (string)null);
                });

            modelBuilder.Entity("DataAccess.Entities.Tipopedido", b =>
                {
                    b.Property<int>("IdTipoPedido")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("idTipoPedido");

                    b.Property<string>("Descipcion")
                        .HasMaxLength(45)
                        .HasColumnType("varchar(45)")
                        .HasColumnName("descipcion");

                    b.HasKey("IdTipoPedido")
                        .HasName("PRIMARY");

                    b.ToTable("tipopedido", (string)null);
                });

            modelBuilder.Entity("DataAccess.Entities.Usuario", b =>
                {
                    b.Property<int>("IdUsuario")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("idUsuario");

                    b.Property<string>("Apellido")
                        .HasMaxLength(45)
                        .HasColumnType("varchar(45)")
                        .HasColumnName("apellido");

                    b.Property<int?>("Dni")
                        .HasColumnType("int")
                        .HasColumnName("dni");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("email");

                    b.Property<DateTime>("FechaAlta")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("FechaBaja")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("FechaModificacion")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Nombre")
                        .HasMaxLength(45)
                        .HasColumnType("varchar(45)")
                        .HasColumnName("nombre");

                    b.Property<string>("Telefono")
                        .HasMaxLength(45)
                        .HasColumnType("varchar(45)")
                        .HasColumnName("telefono");

                    b.HasKey("IdUsuario")
                        .HasName("PRIMARY");

                    b.HasIndex(new[] { "Email" }, "email_UNIQUE")
                        .IsUnique();

                    b.ToTable("usuario", (string)null);
                });

            modelBuilder.Entity("DataAccess.Entities.Domicilio", b =>
                {
                    b.HasOne("DataAccess.Entities.Usuario", "IdUsuarioNavigation")
                        .WithMany("Domicilio")
                        .HasForeignKey("IdUsuario")
                        .IsRequired()
                        .HasConstraintName("id_Usuario_FKD");

                    b.Navigation("IdUsuarioNavigation");
                });

            modelBuilder.Entity("DataAccess.Entities.Envio", b =>
                {
                    b.HasOne("DataAccess.Entities.Estadoenvio", "IdEstadoEnvioNavigation")
                        .WithMany("Envio")
                        .HasForeignKey("IdEstadoEnvio")
                        .HasConstraintName("id_estadoEnvio_FKE");

                    b.HasOne("DataAccess.Entities.Pedido", "IdPedidoNavigation")
                        .WithMany("EnvioNavigation")
                        .HasForeignKey("IdPedido")
                        .HasConstraintName("id_pedido_FKE");

                    b.Navigation("IdEstadoEnvioNavigation");

                    b.Navigation("IdPedidoNavigation");
                });

            modelBuilder.Entity("DataAccess.Entities.Producto", b =>
                {
                    b.HasOne("DataAccess.Entities.Categoria", "IdCategoriaNavigation")
                        .WithMany("Producto")
                        .HasForeignKey("IdCategoria")
                        .HasConstraintName("id_Categoria_FKP");

                    b.Navigation("IdCategoriaNavigation");
                });

            modelBuilder.Entity("DataAccess.Entities.Publicacion", b =>
                {
                    b.HasOne("DataAccess.Entities.Producto", "IdProductoNavigation")
                        .WithMany("Publicacion")
                        .HasForeignKey("IdProducto")
                        .HasConstraintName("id_producto_FKP");

                    b.HasOne("DataAccess.Entities.Sucursal", "IdSucursalNavigation")
                        .WithMany("Publicacion")
                        .HasForeignKey("IdSucursal")
                        .HasConstraintName("id_sucursal_FKP");

                    b.Navigation("IdProductoNavigation");

                    b.Navigation("IdSucursalNavigation");
                });

            modelBuilder.Entity("DataAccess.Entities.Categoria", b =>
                {
                    b.Navigation("Producto");
                });

            modelBuilder.Entity("DataAccess.Entities.Estadoenvio", b =>
                {
                    b.Navigation("Envio");
                });

            modelBuilder.Entity("DataAccess.Entities.Pedido", b =>
                {
                    b.Navigation("EnvioNavigation");
                });

            modelBuilder.Entity("DataAccess.Entities.Producto", b =>
                {
                    b.Navigation("Publicacion");
                });

            modelBuilder.Entity("DataAccess.Entities.Sucursal", b =>
                {
                    b.Navigation("Publicacion");
                });

            modelBuilder.Entity("DataAccess.Entities.Usuario", b =>
                {
                    b.Navigation("Domicilio");
                });
#pragma warning restore 612, 618
        }
    }
}

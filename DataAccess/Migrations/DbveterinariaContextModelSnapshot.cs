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

                    b.Property<bool?>("Agrupador")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Descripcion")
                        .HasColumnType("longtext")
                        .HasColumnName("descripcion");

                    b.Property<DateTime>("FechaDesde")
                        .HasColumnType("datetime")
                        .HasColumnName("fechaDesde");

                    b.Property<DateTime?>("FechaHasta")
                        .HasColumnType("datetime")
                        .HasColumnName("fechaHasta");

                    b.Property<DateTime>("FechaModificacion")
                        .HasColumnType("datetime")
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
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<DateTime>("FechaAlta")
                        .HasColumnType("datetime")
                        .HasColumnName("fechaAlta");

                    b.Property<DateTime?>("FechaBaja")
                        .HasColumnType("datetime")
                        .HasColumnName("fechaBaja");

                    b.Property<DateTime>("FechaModificacion")
                        .HasColumnType("datetime")
                        .HasColumnName("fechaModificacion");

                    b.Property<int>("IdDomicilio")
                        .HasColumnType("int");

                    b.Property<int>("IdEstadoEnvio")
                        .HasColumnType("int");

                    b.Property<int>("IdPedido")
                        .HasColumnType("int")
                        .HasColumnName("id_pedido");

                    b.HasKey("Id")
                        .HasName("PRIMARY");

                    b.HasIndex("IdDomicilio");

                    b.HasIndex("IdEstadoEnvio");

                    b.HasIndex(new[] { "IdPedido" }, "id_Domicilio_FKDomicilio");

                    b.HasIndex(new[] { "IdPedido" }, "id_Pedido_FKPedidoEnvio")
                        .IsUnique();

                    b.ToTable("Envio", (string)null);
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
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<string>("EstadoPago")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("estadoPago");

                    b.Property<DateTime?>("FechaAlta")
                        .IsRequired()
                        .HasColumnType("datetime")
                        .HasColumnName("fechaAlta");

                    b.Property<DateTime?>("FechaBaja")
                        .HasColumnType("datetime")
                        .HasColumnName("fechaBaja");

                    b.Property<DateTime?>("FechaModificacion")
                        .IsRequired()
                        .HasColumnType("datetime")
                        .HasColumnName("fechaModificacion");

                    b.Property<long>("IdPagoMercadoPago")
                        .HasColumnType("bigint")
                        .HasColumnName("idPagoMercadoPago");

                    b.Property<int>("IdPedido")
                        .HasColumnType("int")
                        .HasColumnName("id_pedido");

                    b.Property<decimal?>("Total")
                        .IsRequired()
                        .HasColumnType("decimal")
                        .HasColumnName("total");

                    b.HasKey("Id")
                        .HasName("PRIMARY");

                    b.HasIndex(new[] { "IdPedido" }, "id_Pedido_FKPedido")
                        .IsUnique();

                    b.ToTable("pago", (string)null);
                });

            modelBuilder.Entity("DataAccess.Entities.Pedido", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<DateTime?>("FechaAlta")
                        .IsRequired()
                        .HasColumnType("datetime")
                        .HasColumnName("fechaAlta");

                    b.Property<DateTime?>("FechaBaja")
                        .HasColumnType("datetime")
                        .HasColumnName("fechaBaja");

                    b.Property<DateTime?>("FechaModificacion")
                        .IsRequired()
                        .HasColumnType("datetime")
                        .HasColumnName("fechaModificacion");

                    b.Property<int?>("IdSucursalPedido")
                        .HasColumnType("int")
                        .HasColumnName("id_sucursal_pedido");

                    b.Property<int?>("IdUsuario")
                        .HasColumnType("int")
                        .HasColumnName("id_usuario");

                    b.Property<long>("Orden_MercadoPago")
                        .HasColumnType("bigint");

                    b.Property<decimal?>("Total")
                        .IsRequired()
                        .HasColumnType("decimal")
                        .HasColumnName("total");

                    b.HasKey("Id")
                        .HasName("PRIMARY");

                    b.HasIndex(new[] { "IdUsuario" }, "id_Usuario_FKPedido");

                    b.HasIndex(new[] { "IdSucursalPedido" }, "id_sucursal_pedido_Fk");

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

                    b.Property<float?>("Precio")
                        .HasColumnType("float")
                        .HasColumnName("precio");

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

                    b.Property<DateTime>("FechaActualizacion")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("FechaDesde")
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

            modelBuilder.Entity("DataAccess.Entities.PublicacionPedido", b =>
                {
                    b.Property<int>("IdPublicacionPedido")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("Cantidad")
                        .HasColumnType("int");

                    b.Property<DateTime>("FechaAlta")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("FechaBaja")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("FechaModificacion")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("IdPedido")
                        .HasColumnType("int");

                    b.Property<int>("IdPublicacion")
                        .HasColumnType("int");

                    b.Property<decimal>("Precio")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("IdPublicacionPedido");

                    b.HasIndex("IdPedido");

                    b.HasIndex("IdPublicacion");

                    b.ToTable("PublicacionPedido");
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

                    b.Property<string>("EmailSucursal")
                        .IsRequired()
                        .HasColumnType("longtext");

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
                    b.HasOne("DataAccess.Entities.Domicilio", "Domicilio")
                        .WithMany("Envios")
                        .HasForeignKey("IdDomicilio")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DataAccess.Entities.Estadoenvio", "EstadoEnvio")
                        .WithMany("Envios")
                        .HasForeignKey("IdEstadoEnvio")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DataAccess.Entities.Pedido", "Pedido")
                        .WithOne("Envio")
                        .HasForeignKey("DataAccess.Entities.Envio", "IdPedido")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("id_Pedido_FKPedidoEnvio");

                    b.Navigation("Domicilio");

                    b.Navigation("EstadoEnvio");

                    b.Navigation("Pedido");
                });

            modelBuilder.Entity("DataAccess.Entities.Pago", b =>
                {
                    b.HasOne("DataAccess.Entities.Pedido", "Pedido")
                        .WithOne("Pago")
                        .HasForeignKey("DataAccess.Entities.Pago", "IdPedido")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("id_Pedido_FKPedido");

                    b.Navigation("Pedido");
                });

            modelBuilder.Entity("DataAccess.Entities.Pedido", b =>
                {
                    b.HasOne("DataAccess.Entities.Sucursal", "Sucursal")
                        .WithMany("Pedidos")
                        .HasForeignKey("IdSucursalPedido")
                        .HasConstraintName("id_sucursal_pedido_Fk");

                    b.HasOne("DataAccess.Entities.Usuario", "Usuario")
                        .WithMany("Pedido")
                        .HasForeignKey("IdUsuario")
                        .HasConstraintName("id_Usuario_FKPedido");

                    b.Navigation("Sucursal");

                    b.Navigation("Usuario");
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

            modelBuilder.Entity("DataAccess.Entities.PublicacionPedido", b =>
                {
                    b.HasOne("DataAccess.Entities.Pedido", "Pedido")
                        .WithMany("PublicacionPedido")
                        .HasForeignKey("IdPedido")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DataAccess.Entities.Publicacion", "Publicacion")
                        .WithMany()
                        .HasForeignKey("IdPublicacion")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Pedido");

                    b.Navigation("Publicacion");
                });

            modelBuilder.Entity("DataAccess.Entities.Categoria", b =>
                {
                    b.Navigation("Producto");
                });

            modelBuilder.Entity("DataAccess.Entities.Domicilio", b =>
                {
                    b.Navigation("Envios");
                });

            modelBuilder.Entity("DataAccess.Entities.Estadoenvio", b =>
                {
                    b.Navigation("Envios");
                });

            modelBuilder.Entity("DataAccess.Entities.Pedido", b =>
                {
                    b.Navigation("Envio");

                    b.Navigation("Pago")
                        .IsRequired();

                    b.Navigation("PublicacionPedido");
                });

            modelBuilder.Entity("DataAccess.Entities.Producto", b =>
                {
                    b.Navigation("Publicacion");
                });

            modelBuilder.Entity("DataAccess.Entities.Sucursal", b =>
                {
                    b.Navigation("Pedidos");

                    b.Navigation("Publicacion");
                });

            modelBuilder.Entity("DataAccess.Entities.Usuario", b =>
                {
                    b.Navigation("Domicilio");

                    b.Navigation("Pedido");
                });
#pragma warning restore 612, 618
        }
    }
}

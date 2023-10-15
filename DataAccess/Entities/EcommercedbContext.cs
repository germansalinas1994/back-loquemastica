using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Entities;

public partial class EcommercedbContext : DbContext
{
    public EcommercedbContext()
    {
    }

    public EcommercedbContext(DbContextOptions<EcommercedbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Categoria> Categoria { get; set; }

    public virtual DbSet<Domicilio> Domicilio { get; set; }

    public virtual DbSet<EfmigrationsHistory> EfmigrationsHistory { get; set; }

    public virtual DbSet<Envio> Envio { get; set; }

    public virtual DbSet<Estadopedido> Estadopedido { get; set; }

    public virtual DbSet<Estadopublicacion> Estadopublicacion { get; set; }

    public virtual DbSet<Pedido> Pedido { get; set; }

    public virtual DbSet<Producto> Producto { get; set; }

    public virtual DbSet<Publicacion> Publicacion { get; set; }

    public virtual DbSet<Rol> Rol { get; set; }

    public virtual DbSet<Sucursal> Sucursal { get; set; }

    public virtual DbSet<Usuario> Usuario { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
// #warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySQL("server=localhost;port=3306;database=ecommercedb;uid=root;password=12345678;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasKey(e => e.IdCategoria).HasName("PRIMARY");

            entity.ToTable("categoria");

            entity.Property(e => e.IdCategoria).HasColumnName("idCategoria");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(45)
                .HasColumnName("descripcion");
        });

        modelBuilder.Entity<Domicilio>(entity =>
        {
            
            entity.HasKey(e => e.IdDomicilio).HasName("PRIMARY");

            entity.ToTable("domicilio");

            entity.HasIndex(e => e.IdUsuario, "id_usuario_FK_idx");

            entity.Property(e => e.IdDomicilio).HasColumnName("idDomicilio");
            entity.Property(e => e.Altura).HasColumnName("altura");
            entity.Property(e => e.Calle)
                .HasMaxLength(200)
                .HasColumnName("calle");
            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Domicilio)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("id_usuario_FK2");
        });

        modelBuilder.Entity<EfmigrationsHistory>(entity =>
        {
            entity.HasKey(e => e.MigrationId).HasName("PRIMARY");

            entity.ToTable("__EFMigrationsHistory");

            entity.Property(e => e.MigrationId).HasMaxLength(150);
            entity.Property(e => e.ProductVersion).HasMaxLength(32);
        });

        modelBuilder.Entity<Envio>(entity =>
        {
            entity.HasKey(e => e.IdEnvio).HasName("PRIMARY");

            entity.ToTable("envio");

            entity.HasIndex(e => e.IdDomicilio, "id_domicilio_idx");

            entity.HasIndex(e => e.IdPedido, "id_pedido_FK_idx");

            entity.HasIndex(e => e.IdPedido, "id_pedido_UNIQUE").IsUnique();

            entity.Property(e => e.IdEnvio).HasColumnName("idEnvio");
            entity.Property(e => e.CodigoSeguimiento)
                .HasMaxLength(45)
                .HasColumnName("codigo_seguimiento");
            entity.Property(e => e.IdDomicilio).HasColumnName("id_domicilio");
            entity.Property(e => e.IdPedido).HasColumnName("id_pedido");

            entity.HasOne(d => d.IdDomicilioNavigation).WithMany(p => p.Envio)
                .HasForeignKey(d => d.IdDomicilio)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("id_domicilio");

            entity.HasOne(d => d.IdPedidoNavigation).WithOne(p => p.Envio)
                .HasForeignKey<Envio>(d => d.IdPedido)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("id_pedido_FK");
        });

        modelBuilder.Entity<Estadopedido>(entity =>
        {
            entity.HasKey(e => e.IdEstadoPedido).HasName("PRIMARY");

            entity.ToTable("estadopedido");

            entity.Property(e => e.IdEstadoPedido).HasColumnName("idEstadoPedido");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(45)
                .HasColumnName("descripcion");
        });

        modelBuilder.Entity<Estadopublicacion>(entity =>
        {
            entity.HasKey(e => e.IdEstadoPublicacion).HasName("PRIMARY");

            entity.ToTable("estadopublicacion");

            entity.Property(e => e.IdEstadoPublicacion).HasColumnName("idEstadoPublicacion");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(45)
                .HasColumnName("descripcion");
        });

        modelBuilder.Entity<Pedido>(entity =>
        {
            entity.HasKey(e => e.IdPedido).HasName("PRIMARY");

            entity.ToTable("pedido");

            entity.HasIndex(e => e.IdEstadoPedido, "id_estado_pedido_FK_idx");

            entity.HasIndex(e => e.IdPublicacion, "id_publicacion_FK_idx");

            entity.HasIndex(e => e.IdSucursal, "id_sucursal_FK_idx");

            entity.HasIndex(e => e.IdTipoEntrega, "id_tipo_entrega_FK_idx");

            entity.HasIndex(e => e.IdUsuario, "id_usuario_FK_idx1");

            entity.Property(e => e.IdPedido).HasColumnName("idPedido");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.IdEstadoPedido).HasColumnName("id_estado_pedido");
            entity.Property(e => e.IdPublicacion).HasColumnName("id_publicacion");
            entity.Property(e => e.IdSucursal).HasColumnName("id_sucursal");
            entity.Property(e => e.IdTipoEntrega).HasColumnName("id_tipo_entrega");
            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.Token)
                .HasMaxLength(45)
                .HasColumnName("token");

            entity.HasOne(d => d.IdEstadoPedidoNavigation).WithMany(p => p.Pedido)
                .HasForeignKey(d => d.IdEstadoPedido)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("id_estado_pedido_FK");

            entity.HasOne(d => d.IdPublicacionNavigation).WithMany(p => p.Pedido)
                .HasForeignKey(d => d.IdPublicacion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("id_publicacion_FK");

            entity.HasOne(d => d.IdSucursalNavigation).WithMany(p => p.Pedido)
                .HasForeignKey(d => d.IdSucursal)
                .HasConstraintName("id_sucursal_FK");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Pedido)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("id_usuario_FK");
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.IdProducto).HasName("PRIMARY");

            entity.ToTable("producto");

            entity.HasIndex(e => e.IdCategoria, "id_categoria_FK_idx");

            entity.Property(e => e.IdProducto).HasColumnName("idProducto");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(45)
                .HasColumnName("descripcion");
            entity.Property(e => e.IdCategoria).HasColumnName("id_categoria");

            entity.HasOne(d => d.IdCategoriaNavigation).WithMany(p => p.Producto)
                .HasForeignKey(d => d.IdCategoria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("id_categoria_FK");
        });

        modelBuilder.Entity<Publicacion>(entity =>
        {
            entity.HasKey(e => e.IdPublicacion).HasName("PRIMARY");

            entity.ToTable("publicacion");

            entity.HasIndex(e => e.IdEstado, "id_estadopublicacion_fk1_idx");

            entity.HasIndex(e => e.IdProducto, "id_producto_fkp_idx");

            entity.Property(e => e.IdPublicacion).HasColumnName("idPublicacion");
            entity.Property(e => e.FechaDesde)
                .HasColumnType("datetime")
                .HasColumnName("fecha_desde");
            entity.Property(e => e.FechaHasta)
                .HasColumnType("datetime")
                .HasColumnName("fecha_hasta");
            entity.Property(e => e.IdEstado).HasColumnName("id_estado");
            entity.Property(e => e.IdProducto).HasColumnName("id_producto");
            entity.Property(e => e.Precio).HasColumnName("precio");
            entity.Property(e => e.Stock).HasColumnName("stock");

            entity.HasOne(d => d.IdEstadoNavigation).WithMany(p => p.Publicacion)
                .HasForeignKey(d => d.IdEstado)
                .HasConstraintName("id_estadopublicacion_fk1");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.Publicacion)
                .HasForeignKey(d => d.IdProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("id_producto_fkp");
        });

        modelBuilder.Entity<Rol>(entity =>
        {
            entity.HasKey(e => e.IdRol).HasName("PRIMARY");

            entity.ToTable("rol");

            entity.Property(e => e.IdRol).HasColumnName("idRol");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(45)
                .HasColumnName("descripcion");
        });

        modelBuilder.Entity<Sucursal>(entity =>
        {
            entity.HasKey(e => e.IdSucursal).HasName("PRIMARY");

            entity.ToTable("sucursal");

            entity.Property(e => e.IdSucursal).HasColumnName("idSucursal");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PRIMARY");

            entity.ToTable("usuario");

            entity.HasIndex(e => e.Dni, "dni_UNIQUE").IsUnique();

            entity.HasIndex(e => e.Email, "email_UNIQUE").IsUnique();

            entity.HasIndex(e => e.IdRol, "id_rol_FK_idx");

            entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");
            entity.Property(e => e.Apellido)
                .HasMaxLength(45)
                .HasColumnName("apellido");
            entity.Property(e => e.Dni).HasColumnName("dni");
            entity.Property(e => e.Email)
                .HasMaxLength(45)
                .HasColumnName("email");
            entity.Property(e => e.IdRol).HasColumnName("id_rol");
            entity.Property(e => e.Nombre)
                .HasMaxLength(45)
                .HasColumnName("nombre");
            entity.Property(e => e.Password)
                .HasMaxLength(45)
                .HasColumnName("password");
            entity.Property(e => e.Telefono).HasColumnName("telefono");

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.Usuario)
                .HasForeignKey(d => d.IdRol)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("id_rol_FK");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

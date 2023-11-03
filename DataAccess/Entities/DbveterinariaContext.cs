using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Entities;

public partial class DbveterinariaContext : DbContext
{
    public DbveterinariaContext()
    {
    }

    public DbveterinariaContext(DbContextOptions<DbveterinariaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Categoria> Categoria { get; set; }

    public virtual DbSet<Domicilio> Domicilio { get; set; }

    public virtual DbSet<Envio> Envio { get; set; }

    public virtual DbSet<Estadoenvio> Estadoenvio { get; set; }

    public virtual DbSet<Estadopedido> Estadopedido { get; set; }

    public virtual DbSet<Pago> Pago { get; set; }

    public virtual DbSet<Pedido> Pedido { get; set; }

    public virtual DbSet<Producto> Producto { get; set; }

    public virtual DbSet<Publicacion> Publicacion { get; set; }

    public virtual DbSet<Sucursal> Sucursal { get; set; }

    public virtual DbSet<Tipopedido> Tipopedido { get; set; }

    public virtual DbSet<Usuario> Usuario { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySQL("server=localhost;port=3306;database=dbveterinaria;uid=root;password=12345678;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasKey(e => e.IdCategoria).HasName("PRIMARY");

            entity.ToTable("categoria");

            entity.Property(e => e.IdCategoria).HasColumnName("idCategoria");
            entity.Property(e => e.Descripcion).HasColumnName("descripcion");
            entity.Property(e => e.FechaDesde)
                .HasColumnType("datetime")
                .HasColumnName("fechaDesde");
            entity.Property(e => e.FechaHasta)
                .HasColumnType("datetime")
                .HasColumnName("fechaHasta");
            entity.Property(e => e.FechaModificacion)
                .HasColumnType("datetime")
                .HasColumnName("fechaModificacion");
            entity.Property(e => e.Nombre)
                .HasMaxLength(45)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Domicilio>(entity =>
        {
            entity.HasKey(e => e.IdDomicilio).HasName("PRIMARY");

            entity.ToTable("domicilio");

            entity.HasIndex(e => e.IdUsuario, "id_Usuario_FKD_idx");

            entity.Property(e => e.IdDomicilio).HasColumnName("idDomicilio");
            entity.Property(e => e.Aclaracion)
                .HasMaxLength(45)
                .HasColumnName("aclaracion");
            entity.Property(e => e.Altura).HasColumnName("altura");
            entity.Property(e => e.Calle)
                .HasMaxLength(45)
                .HasColumnName("calle");
            entity.Property(e => e.Departamento)
                .HasMaxLength(45)
                .HasColumnName("departamento");
            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Domicilio)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("id_Usuario_FKD");
        });

        modelBuilder.Entity<Envio>(entity =>
        {
            entity.HasKey(e => e.IdEnvio).HasName("PRIMARY");

            entity.ToTable("envio");

            entity.HasIndex(e => e.IdEstadoEnvio, "id_estadoEnvio_FKE_idx");

            entity.Property(e => e.IdEnvio).HasColumnName("idEnvio");
            entity.Property(e => e.FechaEnvio)
                .HasColumnType("date")
                .HasColumnName("fechaEnvio");
            entity.Property(e => e.IdEstadoEnvio).HasColumnName("id_estadoEnvio");
            entity.Property(e => e.Precio).HasColumnName("precio");

            entity.HasOne(d => d.IdEstadoEnvioNavigation).WithMany(p => p.Envio)
                .HasForeignKey(d => d.IdEstadoEnvio)
                .HasConstraintName("id_estadoEnvio_FKE");

 
        });

        modelBuilder.Entity<Estadoenvio>(entity =>
        {
            entity.HasKey(e => e.IdEstadoEnvio).HasName("PRIMARY");

            entity.ToTable("estadoenvio");

            entity.Property(e => e.IdEstadoEnvio).HasColumnName("idEstadoEnvio");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(45)
                .HasColumnName("descripcion");
        });

        modelBuilder.Entity<Estadopedido>(entity =>
        {
            entity.HasKey(e => e.IdEstadoPedido).HasName("PRIMARY");

            entity.ToTable("estadopedido");

            entity.Property(e => e.IdEstadoPedido).HasColumnName("idEstadoPedido");
            entity.Property(e => e.Descipcion)
                .HasMaxLength(45)
                .HasColumnName("descipcion");
        });

        modelBuilder.Entity<Pago>(entity =>
        {
            entity.HasKey(e => e.IdPago).HasName("PRIMARY");

            entity.ToTable("pago");

            entity.Property(e => e.IdPago).HasColumnName("idPago");
            entity.Property(e => e.FechaAlta)
                .HasColumnType("date")
                .HasColumnName("fechaAlta");
            entity.Property(e => e.NroTransaccion).HasColumnName("nroTransaccion");
        });

        modelBuilder.Entity<Pedido>(entity =>
        {
            entity.HasKey(e => e.IdPedido).HasName("PRIMARY");

            entity.ToTable("pedido");

            entity.HasIndex(e => e.IdEstadoPedido, "id_estadoPedido_idx");

            entity.HasIndex(e => e.IdPago, "id_pago_FKP_idx");

            entity.HasIndex(e => e.IdPublicacion, "id_publicacion_FKP_idx");

            entity.HasIndex(e => e.IdEnvio, "id_envio_UNIQUE").IsUnique();


            entity.HasIndex(e => e.IdTipoPedido, "id_tipoPedido_FKP_idx");

            entity.HasIndex(e => e.IdUsuario, "id_usuario_FKP_idx");

            entity.Property(e => e.IdPedido).HasColumnName("idPedido");
            entity.Property(e => e.Envio).HasColumnName("envio");
            entity.Property(e => e.IdEnvio).HasColumnName("id_envio");

            entity.Property(e => e.FechaAlta)
                .HasColumnType("date")
                .HasColumnName("fechaAlta");
            entity.Property(e => e.FechaBaja)
                .HasColumnType("date")
                .HasColumnName("fechaBaja");
            entity.Property(e => e.FechaModificacion)
                .HasColumnType("date")
                .HasColumnName("fechaModificacion");
            entity.Property(e => e.IdEstadoPedido).HasColumnName("id_estadoPedido");
            entity.Property(e => e.IdPago).HasColumnName("id_pago");
            entity.Property(e => e.IdPublicacion).HasColumnName("id_publicacion");
            entity.Property(e => e.IdTipoPedido).HasColumnName("id_tipoPedido");
            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.NroPedido).HasColumnName("nroPedido");
            entity.HasOne(d => d.IdEnvioNavigation).WithOne(p => p.Pedido)
            .HasForeignKey<Envio>(d => d.IdEnvio)
            .HasConstraintName("id_envio_UNIQUE");

        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.IdProducto).HasName("PRIMARY");

            entity.ToTable("producto");

            entity.HasIndex(e => e.IdCategoria, "id_Categoria_FKP_idx");

            entity.Property(e => e.IdProducto).HasColumnName("idProducto");
            entity.Property(e => e.Descripcion).HasColumnName("descripcion");
            entity.Property(e => e.FechaAlta)
                .HasColumnType("date")
                .HasColumnName("fechaAlta");
            entity.Property(e => e.FechaBaja)
                .HasColumnType("date")
                .HasColumnName("fechaBaja");
            entity.Property(e => e.FechaModificacion)
                .HasColumnType("date")
                .HasColumnName("fechaModificacion");
            entity.Property(e => e.IdCategoria).HasColumnName("id_categoria");
            entity.Property(e => e.Nombre).HasColumnName("nombre");
            entity.Property(e => e.UrlImagen).HasColumnName("urlImagen");

            entity.HasOne(d => d.IdCategoriaNavigation).WithMany(p => p.Producto)
                .HasForeignKey(d => d.IdCategoria)
                .HasConstraintName("id_Categoria_FKP");
        });

        modelBuilder.Entity<Publicacion>(entity =>
        {
            entity.HasKey(e => e.IdPublicacion).HasName("PRIMARY");

            entity.ToTable("publicacion", tb => tb.HasComment("						"));

            entity.HasIndex(e => e.IdProducto, "id_producto_FKP_idx");

            entity.HasIndex(e => e.IdSucursal, "id_sucursal_FKP_idx");

            entity.Property(e => e.IdPublicacion).HasColumnName("idPublicacion");
            entity.Property(e => e.FechaDesde)
                .HasColumnType("date")
                .HasColumnName("fechaDesde");
            entity.Property(e => e.FechaHasta)
                .HasColumnType("date")
                .HasColumnName("fechaHasta");
            entity.Property(e => e.IdProducto).HasColumnName("id_producto");
            entity.Property(e => e.IdSucursal).HasColumnName("id_sucursal");
            entity.Property(e => e.Precio).HasColumnName("precio");
            entity.Property(e => e.Stock).HasColumnName("stock");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.Publicacion)
                .HasForeignKey(d => d.IdProducto)
                .HasConstraintName("id_producto_FKP");

            entity.HasOne(d => d.IdSucursalNavigation).WithMany(p => p.Publicacion)
                .HasForeignKey(d => d.IdSucursal)
                .HasConstraintName("id_sucursal_FKP");
        });

        modelBuilder.Entity<Sucursal>(entity =>
        {
            entity.HasKey(e => e.IdSucursal).HasName("PRIMARY");

            entity.ToTable("sucursal");

            entity.Property(e => e.IdSucursal).HasColumnName("idSucursal");
            entity.Property(e => e.Direccion)
                .HasMaxLength(100)
                .HasColumnName("direccion");
            entity.Property(e => e.Nombre)
                .HasMaxLength(45)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Tipopedido>(entity =>
        {
            entity.HasKey(e => e.IdTipoPedido).HasName("PRIMARY");

            entity.ToTable("tipopedido");

            entity.Property(e => e.IdTipoPedido).HasColumnName("idTipoPedido");
            entity.Property(e => e.Descipcion)
                .HasMaxLength(45)
                .HasColumnName("descipcion");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PRIMARY");

            entity.ToTable("usuario");

            entity.HasIndex(e => e.Email, "email_UNIQUE").IsUnique();

            entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");
            entity.Property(e => e.Apellido)
                .HasMaxLength(45)
                .HasColumnName("apellido");
            entity.Property(e => e.Dni).HasColumnName("dni");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.Nombre)
                .HasMaxLength(45)
                .HasColumnName("nombre");
            entity.Property(e => e.Telefono)
                .HasMaxLength(45)
                .HasColumnName("telefono");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

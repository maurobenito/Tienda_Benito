using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace Tienda_Benito.Models;

public partial class ProyectoTiendaContext : DbContext
{
    public ProyectoTiendaContext()
    {
    }

    public ProyectoTiendaContext(DbContextOptions<ProyectoTiendaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cliente> Cliente { get; set; }

    public virtual DbSet<Producto> Producto { get; set; }

    public virtual DbSet<Proveedor> Proveedor { get; set; }

    public virtual DbSet<Rubro> Rubro { get; set; }

    public virtual DbSet<Usuario> Usuario { get; set; }

    public virtual DbSet<Ventadetalle> Ventadetalle { get; set; }

    public virtual DbSet<Ventum> Venta { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;user=root;database=proyecto_tienda2", Microsoft.EntityFrameworkCore.ServerVersion.Parse("10.4.32-mariadb"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_general_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.ClienteId).HasName("PRIMARY");
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.ProductoId).HasName("PRIMARY");
        });
modelBuilder.Entity<Producto>(entity =>
{
    entity.HasKey(e => e.ProductoId).HasName("PRIMARY");

    entity.HasOne(p => p.Proveedor)
        .WithMany()
        .HasForeignKey(p => p.ProveedorId)
        .OnDelete(DeleteBehavior.SetNull)
        .HasConstraintName("producto_ibfk_proveedor");

    entity.HasOne(p => p.Rubro)
        .WithMany()
        .HasForeignKey(p => p.RubroId)
        .OnDelete(DeleteBehavior.SetNull)
        .HasConstraintName("producto_ibfk_rubro");
});

        modelBuilder.Entity<Proveedor>(entity =>
        {
            entity.HasKey(e => e.ProveedorId).HasName("PRIMARY");
        });

        modelBuilder.Entity<Rubro>(entity =>
        {
            entity.HasKey(e => e.RubroId).HasName("PRIMARY");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.UsuarioId).HasName("PRIMARY");
        });

        modelBuilder.Entity<Ventadetalle>(entity =>
        {
            entity.HasKey(e => e.VentaDetalleId).HasName("PRIMARY");

            entity.HasOne(d => d.Producto).WithMany(p => p.Ventadetalles)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ventadetalle_ibfk_2");

            entity.HasOne(d => d.Venta).WithMany(p => p.Ventadetalles)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ventadetalle_ibfk_1");
        });

        modelBuilder.Entity<Ventum>(entity =>
        {
            entity.HasKey(e => e.VentaId).HasName("PRIMARY");

            entity.HasOne(d => d.Cliente).WithMany(p => p.Venta).HasConstraintName("venta_ibfk_2");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Venta).HasConstraintName("venta_ibfk_1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

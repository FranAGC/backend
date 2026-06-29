// AppDbContext — maps entities to existing SQL Server tables defined in database/ddl.sql
using api.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Cliente> Clientes => Set<Cliente>();
    public DbSet<Servicio> Servicios => Set<Servicio>();
    public DbSet<ServicioCable> ServiciosCable => Set<ServicioCable>();
    public DbSet<ServicioInternet> ServiciosInternet => Set<ServicioInternet>();
    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Rol> Roles => Set<Rol>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ── roles ────────────────────────────────────────────────────
        modelBuilder.Entity<Rol>(e =>
        {
            e.ToTable("roles");
            e.HasKey(r => r.IdRol);
            e.Property(r => r.IdRol).HasColumnName("id_rol");
            e.Property(r => r.NombreRol).HasColumnName("nombre_rol").HasMaxLength(50).IsRequired();
            e.Property(r => r.Descripcion).HasColumnName("descripcion");
            e.HasIndex(r => r.NombreRol).IsUnique();
        });

        // ── usuarios ─────────────────────────────────────────────────
        modelBuilder.Entity<Usuario>(e =>
        {
            e.ToTable("usuarios");
            e.HasKey(u => u.IdUsuario);
            e.Property(u => u.IdUsuario).HasColumnName("id_usuario");
            e.Property(u => u.IdEmpleado).HasColumnName("id_empleado");
            e.Property(u => u.IdRol).HasColumnName("id_rol");
            e.Property(u => u.Username).HasColumnName("username").HasMaxLength(50).IsRequired();
            e.Property(u => u.PasswordHash).HasColumnName("password_hash").HasMaxLength(255).IsRequired();
            e.Property(u => u.Estado).HasColumnName("estado").HasMaxLength(20).IsRequired();
            e.Property(u => u.CreadoEn).HasColumnName("creado_en");
            e.HasIndex(u => u.Username).IsUnique();

            e.HasOne(u => u.Rol)
             .WithMany(r => r.Usuarios)
             .HasForeignKey(u => u.IdRol)
             .OnDelete(DeleteBehavior.NoAction);
        });

        // ── clientes ─────────────────────────────────────────────────
        modelBuilder.Entity<Cliente>(e =>
        {
            e.ToTable("clientes");
            e.HasKey(c => c.IdCliente);
            e.Property(c => c.IdCliente).HasColumnName("id_cliente");
            e.Property(c => c.Codigo).HasColumnName("codigo").HasMaxLength(20).IsRequired();
            e.Property(c => c.Nombre).HasColumnName("nombre").HasMaxLength(150).IsRequired();
            e.Property(c => c.FechaAlta).HasColumnName("fecha_alta");
            e.Property(c => c.Direccion).HasColumnName("direccion").IsRequired();
            e.Property(c => c.Correo).HasColumnName("correo").HasMaxLength(255).IsRequired();
            e.Property(c => c.Telefono).HasColumnName("telefono").HasMaxLength(20).IsRequired();
            e.Property(c => c.Estado).HasColumnName("estado").HasMaxLength(20).IsRequired();
            e.Property(c => c.CreadoEn).HasColumnName("creado_en");
            e.HasIndex(c => c.Codigo).IsUnique();
            e.HasIndex(c => c.Correo).IsUnique();
        });

        // ── servicios ────────────────────────────────────────────────
        modelBuilder.Entity<Servicio>(e =>
        {
            e.ToTable("servicios");
            e.HasKey(s => s.IdServicio);
            e.Property(s => s.IdServicio).HasColumnName("id_servicio");
            e.Property(s => s.CodigoServicio).HasColumnName("codigo_servicio").HasMaxLength(20).IsRequired();
            e.Property(s => s.IdCliente).HasColumnName("id_cliente");
            e.Property(s => s.TipoServicio).HasColumnName("tipo_servicio").HasMaxLength(20).IsRequired();
            e.Property(s => s.CostoMensualBase).HasColumnName("costo_mensual_base").HasPrecision(10, 2);
            e.Property(s => s.LugarInstalacion).HasColumnName("lugar_instalacion").IsRequired();
            e.Property(s => s.FechaContratacion).HasColumnName("fecha_contratacion");
            e.Property(s => s.Estado).HasColumnName("estado").HasMaxLength(20).IsRequired();
            e.HasIndex(s => s.CodigoServicio).IsUnique();

            e.HasOne(s => s.Cliente)
             .WithMany(c => c.Servicios)
             .HasForeignKey(s => s.IdCliente)
             .OnDelete(DeleteBehavior.NoAction);
        });

        // ── servicios_cable ──────────────────────────────────────────
        modelBuilder.Entity<ServicioCable>(e =>
        {
            e.ToTable("servicios_cable");
            e.HasKey(sc => sc.IdServicio);
            e.Property(sc => sc.IdServicio).HasColumnName("id_servicio");
            e.Property(sc => sc.DireccionInstalacion).HasColumnName("direccion_instalacion").IsRequired();
            e.Property(sc => sc.PlanCanales).HasColumnName("plan_canales").HasMaxLength(50).IsRequired();
            e.Property(sc => sc.Estado).HasColumnName("estado").HasMaxLength(20).IsRequired();

            e.HasOne(sc => sc.Servicio)
             .WithOne(s => s.ServicioCable)
             .HasForeignKey<ServicioCable>(sc => sc.IdServicio)
             .OnDelete(DeleteBehavior.Cascade);
        });

        // ── servicios_internet ───────────────────────────────────────
        modelBuilder.Entity<ServicioInternet>(e =>
        {
            e.ToTable("servicios_internet");
            e.HasKey(si => si.IdServicio);
            e.Property(si => si.IdServicio).HasColumnName("id_servicio");
            e.Property(si => si.VelocidadMbps).HasColumnName("velocidad_mbps");
            e.Property(si => si.MesesConsecutivosPagos).HasColumnName("meses_consecutivos_pagos");
            e.Property(si => si.Estado).HasColumnName("estado").HasMaxLength(20).IsRequired();

            e.HasOne(si => si.Servicio)
             .WithOne(s => s.ServicioInternet)
             .HasForeignKey<ServicioInternet>(si => si.IdServicio)
             .OnDelete(DeleteBehavior.Cascade);
        });
    }
}

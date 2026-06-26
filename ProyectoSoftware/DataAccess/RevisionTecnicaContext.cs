using Microsoft.EntityFrameworkCore;
using ProyectoSoftware.Models;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;

namespace ProyectoSoftware.DataAccess
{
    public class RevisionTecnicaContext : DbContext
    {
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Proyecto> Proyectos { get; set; }
        public DbSet<Actividad> Actividades { get; set; }
        public DbSet<Revision> Revisiones { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Aquí configuras tu cadena de conexión a SQL Server
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=TU_SERVIDOR;Database=DB_RevisionTecnica;Trusted_Connection=True;TrustServerCertificate=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ==========================================
            // CONFIGURACIÓN TABLA: Usuarios
            // ==========================================
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.IdUsuario);
                entity.Property(e => e.Nombre).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.HasIndex(e => e.Email).IsUnique(); // El correo no se puede repetir
                entity.Property(e => e.Password).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Rol).IsRequired().HasMaxLength(20);
            });

            // ==========================================
            // CONFIGURACIÓN TABLA: Proyectos
            // ==========================================
            modelBuilder.Entity<Proyecto>(entity =>
            {
                entity.HasKey(e => e.IdProyecto);
                entity.Property(e => e.IdProyecto).HasMaxLength(10);
                entity.Property(e => e.Descripcion).IsRequired();
                entity.Property(e => e.Estado).IsRequired().HasMaxLength(20);
            });

            // ==========================================
            // CONFIGURACIÓN TABLA: Actividades
            // ==========================================
            modelBuilder.Entity<Actividad>(entity =>
            {
                entity.HasKey(e => e.IdActividad);
                entity.Property(e => e.NombreActividad).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Estado).IsRequired().HasMaxLength(20);
                entity.Property(e => e.FechaRecepcion).IsRequired();

                // Relación: Actividad -> Proyecto
                entity.HasOne(a => a.Proyecto)
                      .WithMany(p => p.Actividades)
                      .HasForeignKey(a => a.IdProyecto)
                      .OnDelete(DeleteBehavior.Restrict);

                // Relación: Actividad -> Usuario (Desarrollador)
                entity.HasOne(a => a.IngenieroAsignado)
                      .WithMany(u => u.ActividadesAsignadas)
                      .HasForeignKey(a => a.IdIngenieroAsignado)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // ==========================================
            // CONFIGURACIÓN TABLA: Revisiones
            // ==========================================
            modelBuilder.Entity<Revision>(entity =>
            {
                entity.HasKey(e => e.IdRevision);
                entity.Property(e => e.Veredicto).HasMaxLength(20);

                // Relación: Revision -> Actividad
                entity.HasOne(r => r.Actividad)
                      .WithMany(a => a.Revisiones)
                      .HasForeignKey(r => r.IdActividad)
                      .OnDelete(DeleteBehavior.Cascade); // Si se borra la actividad, se borran sus revisiones

                // Relación: Revision -> Usuario (Revisor)
                entity.HasOne(r => r.IngenieroRevisor)
                      .WithMany(u => u.RevisionesAsignadas)
                      .HasForeignKey(r => r.IdIngenieroRevisor)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}

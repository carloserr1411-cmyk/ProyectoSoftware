using Microsoft.EntityFrameworkCore;
using ProyectoSoftware.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ProyectoSoftware.DataAccess
{
    // Aplicamos la herencia de DbContext e implementamos explícitamente IDisposable como en tu proyecto anterior
    public class RevisionTecnicaContext : DbContext, IDisposable
    {
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Proyecto> Proyectos { get; set; }
        public DbSet<Actividad> Actividades { get; set; }
        public DbSet<Revision> Revisiones { get; set; }

        public RevisionTecnicaContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // METODOLOGÍA PORTABLE: Localiza la carpeta AppData local del usuario en Windows
                string rutaAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                string carpetaSistema = Path.Combine(rutaAppData, "ProyectoRevisionTecnica");

                // Asegura que la carpeta para la base de datos exista físicamente
                Directory.CreateDirectory(carpetaSistema);

                // Define la ruta del archivo .db de SQLite
                string rutaBD = Path.Combine(carpetaSistema, "revision_tecnica.db");

                // Cambiamos UseSqlServer por UseSqlite
                optionsBuilder.UseSqlite($"Data Source={rutaBD}");
            }
        }

        /// <summary>
        /// Intercepta los guardados síncronos tradicionales del sistema.
        /// </summary>
        public override int SaveChanges()
        {
            ProcesarAuditoria();
            return base.SaveChanges();
        }

        /// <summary>
        /// Intercepta de forma automática todos los guardados asíncronos (Async) del sistema.
        /// </summary>
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ProcesarAuditoria();
            return base.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Centraliza la lógica de automatización de fechas o auditorías.
        /// Si en el futuro tus modelos (ej. Actividad o Proyecto) tienen campos de control, aquí se actualizarán solos.
        /// </summary>
        private void ProcesarAuditoria()
        {
            var entradas = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var entrada in entradas)
            {
                // Ejemplo analógico a "Beneficiario": Si agregas una propiedad FechaModificacion en tus modelos
                // puedes descomentar y usar este bloque para que se estampe automáticamente.
                /*
                if (entrada.Entity is Actividad actividad)
                {
                    actividad.FechaUltimaModificacion = DateTime.Now;
                }
                */
            }
        }

        // Implementación explícita de IDisposable para garantizar la liberación de conexiones usando 'using'
        public new void Dispose()
        {
            try
            {
                base.Dispose();
            }
            catch
            {
                throw;
            }
            GC.SuppressFinalize(this);
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
                entity.HasIndex(e => e.Email).IsUnique(); // El correo sigue siendo único
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
                      .OnDelete(DeleteBehavior.Cascade);

                // Relación: Revision -> Usuario (Revisor)
                entity.HasOne(r => r.IngenieroRevisor)
                      .WithMany(u => u.RevisionesAsignadas)
                      .HasForeignKey(r => r.IdIngenieroRevisor)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Inyección de Datos Semilla (Data Seeding) para el Login Inicial
            modelBuilder.Entity<Usuario>().HasData(
                new Usuario
                {
                    IdUsuario = 1,
                    Nombre = "Gerente Principal",
                    Email = "gerente@sistema.com",
                    Password = "123",
                    Rol = "Gerencia"
                }
            );
        }
    }
}
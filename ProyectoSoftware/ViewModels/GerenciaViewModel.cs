using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProyectoSoftware.DataAccess;
using ProyectoSoftware.Models;
using System.Collections.ObjectModel;
using System.Linq;

namespace ProyectoSoftware.ViewModels
{
    public partial class GerenciaViewModel : ObservableObject
    {
        // ==========================================
        // 1. GESTIÓN DE VISIBILIDAD DE LOS DIÁLOGOS
        // ==========================================
        [ObservableProperty]
        private bool _isRegistroIngenieroOpen;

        [ObservableProperty]
        private bool _isRegistroProyectoOpen;

        [ObservableProperty]
        private bool _isAsignacionTareaOpen;

        // ==========================================
        // 2. PROPIEDADES DEL FORMULARIO DE INGENIERO
        // ==========================================
        [ObservableProperty]
        private string _nombreNuevoIngeniero = string.Empty;

        [ObservableProperty]
        private string _emailNuevoIngeniero = string.Empty;

        [ObservableProperty]
        private string _passwordNuevoIngeniero = string.Empty;

        // ==========================================
        // 3. PROPIEDADES DEL FORMULARIO DE PROYECTO
        // ==========================================
        [ObservableProperty]
        private string _idNuevoProyecto = string.Empty;

        [ObservableProperty]
        private string _descripcionNuevoProyecto = string.Empty;

        // Propiedad para la tabla del Dashboard (Monitor global)
        [ObservableProperty]
        private ObservableCollection<Actividad> _actividadesGlobales = new();

        public GerenciaViewModel()
        {
            CargarDashboard();
        }

        private void CargarDashboard()
        {
            // Aquí puedes cargar la tabla general leyendo el RevisionTecnicaContext
            using (var context = new RevisionTecnicaContext())
            {
                ActividadesGlobales = new ObservableCollection<Actividad>(context.Actividades.ToList());
            }
        }

        // ==========================================
        // COMANDOS DE APERTURA Y CIERRE
        // ==========================================
        [RelayCommand]
        private void AbrirRegistroIngeniero() => IsRegistroIngenieroOpen = true;

        [RelayCommand]
        private void CerrarRegistroIngeniero()
        {
            IsRegistroIngenieroOpen = false;
            NombreNuevoIngeniero = string.Empty;
            EmailNuevoIngeniero = string.Empty;
            PasswordNuevoIngeniero = string.Empty;
        }

        [RelayCommand]
        private void AbrirRegistroProyecto() => IsRegistroProyectoOpen = true;

        [RelayCommand]
        private void CerrarRegistroProyecto()
        {
            IsRegistroProyectoOpen = false;
            IdNuevoProyecto = string.Empty;
            DescripcionNuevoProyecto = string.Empty;
        }

        [RelayCommand]
        private void AbrirAsignacionTarea() => IsAsignacionTareaOpen = true;

        [RelayCommand]
        private void CerrarAsignacionTarea() => IsAsignacionTareaOpen = false;

        // ==========================================
        // COMANDOS DE BASE DE DATOS (EF CORE)
        // ==========================================
        [RelayCommand]
        private void GuardarIngeniero()
        {
            // Cláusula de guarda (Validación básica)
            if (string.IsNullOrWhiteSpace(NombreNuevoIngeniero) || string.IsNullOrWhiteSpace(EmailNuevoIngeniero))
                return;

            using (var context = new RevisionTecnicaContext())
            {
                var nuevoUsuario = new Usuario
                {
                    Nombre = NombreNuevoIngeniero,
                    Email = EmailNuevoIngeniero,
                    Password = PasswordNuevoIngeniero,
                    Rol = "Ingeniero" // Definimos el rol de forma fija [cite: 500]
                };

                context.Usuarios.Add(nuevoUsuario);
                context.SaveChanges();
            }

            CerrarRegistroIngeniero();
        }

        [RelayCommand]
        private void GuardarProyecto()
        {
            // Cláusula de guarda
            if (string.IsNullOrWhiteSpace(IdNuevoProyecto) || string.IsNullOrWhiteSpace(DescripcionNuevoProyecto))
                return;

            using (var context = new RevisionTecnicaContext())
            {
                var nuevoProyecto = new Proyecto
                {
                    IdProyecto = IdNuevoProyecto, // Ej: P01 [cite: 497]
                    Descripcion = DescripcionNuevoProyecto,
                    Estado = "Activo"
                };

                context.Proyectos.Add(nuevoProyecto);
                context.SaveChanges();
            }

            CerrarRegistroProyecto();
            CargarDashboard(); // Refrescar la tabla en caso de que sea necesario
        }
    }
}
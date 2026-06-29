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
            // 1. Cláusula de guarda estricta (Incluimos la validación del Password)
            if (string.IsNullOrWhiteSpace(NombreNuevoIngeniero) ||
                string.IsNullOrWhiteSpace(EmailNuevoIngeniero) ||
                string.IsNullOrWhiteSpace(PasswordNuevoIngeniero))
            {
                System.Windows.MessageBox.Show("Por favor, llena todos los campos.", "Campos incompletos", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                return;
            }

            try
            {
                using (var context = new RevisionTecnicaContext())
                {
                    var nuevoUsuario = new Usuario
                    {
                        Nombre = NombreNuevoIngeniero,
                        Email = EmailNuevoIngeniero,
                        Password = PasswordNuevoIngeniero,
                        Rol = "Ingeniero"
                    };

                    context.Usuarios.Add(nuevoUsuario);
                    context.SaveChanges();
                }

                // 2. Retroalimentación Visual de Éxito
                System.Windows.MessageBox.Show($"El ingeniero '{NombreNuevoIngeniero}' ha sido registrado exitosamente en el sistema.", "Registro Exitoso", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);

                CerrarRegistroIngeniero();
            }
            catch (System.Exception ex)
            {
                // 3. Captura de errores de Base de Datos
                System.Windows.MessageBox.Show($"Ocurrió un error al guardar en la base de datos:\n\n{ex.InnerException?.Message ?? ex.Message}", "Error Crítico", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

        [RelayCommand]
        private void GuardarProyecto()
        {
            if (string.IsNullOrWhiteSpace(IdNuevoProyecto) || string.IsNullOrWhiteSpace(DescripcionNuevoProyecto))
            {
                System.Windows.MessageBox.Show("Por favor, ingresa el ID y la descripción del proyecto.", "Campos incompletos", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                return;
            }

            try
            {
                using (var context = new RevisionTecnicaContext())
                {
                    var nuevoProyecto = new Proyecto
                    {
                        IdProyecto = IdNuevoProyecto,
                        Descripcion = DescripcionNuevoProyecto,
                        Estado = "Activo"
                    };

                    context.Proyectos.Add(nuevoProyecto);
                    context.SaveChanges();
                }

                System.Windows.MessageBox.Show($"El proyecto '{IdNuevoProyecto}' ha sido creado exitosamente.", "Proyecto Creado", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);

                CerrarRegistroProyecto();
                CargarDashboard();
            }
            catch (System.Exception ex)
            {
                System.Windows.MessageBox.Show($"Ocurrió un error al guardar el proyecto:\n\n{ex.InnerException?.Message ?? ex.Message}", "Error Crítico", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }
    }
}
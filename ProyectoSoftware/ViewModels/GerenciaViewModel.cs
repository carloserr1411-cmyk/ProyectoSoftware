using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProyectoSoftware.Models;
using System.Collections.ObjectModel;

namespace ProyectoSoftware.ViewModels
{
    public partial class GerenciaViewModel : ObservableObject
    {
        // Lista observable para la tabla principal
        [ObservableProperty]
        private ObservableCollection<Actividad>? _todasLasActividades;

        public GerenciaViewModel()
        {
            // Simulamos la carga de datos (En la realidad esto viene de _context.Actividades.ToList())
            CargarDatosGlobales();
        }

        private void CargarDatosGlobales()
        {
            /*TodasLasActividades = new ObservableCollection<Actividad>
            {
                new Actividad { IdProyecto = "P01", NombreActividad = "Modelo del Dominio", Estado = "Terminada", IngenieroAsignado = new Usuario { Nombre = "Pablo Daza" } },
                new Actividad { IdProyecto = "P01", NombreActividad = "Casos de Uso", Estado = "Pendiente", IngenieroAsignado = new Usuario { Nombre = "Ana Campos" } },
                new Actividad { IdProyecto = "P02", NombreActividad = "Diseño de BD", Estado = "En Revisión", IngenieroAsignado = new Usuario { Nombre = "Luis Torres" } }
            };*/
        }

        // --- Comandos para las acciones de Gerencia ---

        [RelayCommand]
        private void AbrirRegistroIngeniero()
        {
            // Aquí la lógica para abrir la vista/diálogo de registro de ingeniero
            System.Diagnostics.Debug.WriteLine("Abriendo formulario para Registrar Ingeniero...");
        }

        [RelayCommand]
        private void AbrirRegistroProyecto()
        {
            // Aquí la lógica para abrir la vista/diálogo de registro de proyecto
            System.Diagnostics.Debug.WriteLine("Abriendo formulario para Registrar Proyecto...");
        }

        [RelayCommand]
        private void AbrirAsignacionTarea()
        {
            // Aquí la lógica para asignar una nueva tarea inicial (Estado Pendiente)
            System.Diagnostics.Debug.WriteLine("Abriendo formulario para Asignar Tarea...");
        }
    }
}
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProyectoSoftware.Models;
using System;
using System.Collections.ObjectModel;

namespace ProyectoSoftware.ViewModels
{
    public partial class IngenieroDesarrolladorViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<Actividad>? _misActividades;

        [ObservableProperty]
        private Actividad? _actividadSeleccionada;

        public IngenieroDesarrolladorViewModel()
        {
            CargarMisActividadesMock();
        }

        private void CargarMisActividadesMock()
        {
            // Simulamos las tareas asignadas específicamente a este ingeniero
            /*MisActividades = new ObservableCollection<Actividad>
            {
                new Actividad
                {
                    IdProyecto = "P01",
                    NombreActividad = "Modelo del Dominio",
                    Estado = "Pendiente",
                    FechaRecepcion = DateTime.Now.AddDays(-2)
                },
                new Actividad
                {
                    IdProyecto = "P01",
                    NombreActividad = "Interfaz de Login",
                    Estado = "En Corrección",
                    FechaRecepcion = DateTime.Now.AddDays(-1)
                }
            };*/
        }

        [RelayCommand]
        private void CulminarActividad()
        {
            if (ActividadSeleccionada != null)
            {
                // Regla de negocio: Solo puede culminar tareas que estén Pendientes o En Corrección
                if (ActividadSeleccionada.Estado == "Pendiente" || ActividadSeleccionada.Estado == "En Corrección")
                {
                    // Actualizamos el estado y la fecha de culminación
                    ActividadSeleccionada.Estado = "Terminada";
                    ActividadSeleccionada.FechaCulminacion = DateTime.Now;

                    System.Diagnostics.Debug.WriteLine($"La actividad '{ActividadSeleccionada.NombreActividad}' ha sido marcada como Terminada.");

                    // En el futuro, aquí guardaremos los cambios con _context.SaveChanges() en EF Core

                    // Forzamos la actualización visual del DataGrid simulado
                    var temp = MisActividades;
                    MisActividades = null;
                    MisActividades = temp;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("La actividad seleccionada ya fue terminada o está en revisión.");
                }
            }
        }
    }
}